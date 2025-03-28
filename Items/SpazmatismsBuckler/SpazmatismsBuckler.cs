using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace ExpertPlus.Items.SpazmatismsBuckler{
	[AutoloadEquip(EquipType.Shield)]
	public sealed class SpazmatismsBuckler : ModItem{
		public static readonly string InlineWikiLibValue = @"
	## Spazmatisms Buckler ![ExpertPlus/Items/SpazmatismsBuckler]t4

	The Spazmatisms Buckler Grants Allows the player to dash into the enemy";
		public override void SetStaticDefaults(){
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults(){
			Item.accessory = true;
			Item.width = 48;
			Item.height = 32;
			Item.value = Item.sellPrice(0, 5);
			Item.rare = ItemRarityID.Expert;
			Item.stack = 1;
			Item.defense = 4;
			Item.damage = 60;
			Item.knockBack = 9f;
			Item.noMelee = false;
			Item.DamageType = DamageClass.Melee;
			//Item.GetCritChance(DamageClass.Melee) += 20;
		}
		public override void UpdateAccessory(Player player, bool hideVisual){
			//public static readonly int MeleeCritBonus = 10;
			player.GetDamage(DamageClass.Generic) += 1f; // Increase ALL player damage by 100%
			player.endurance = 1f - (0.1f * (1f - player.endurance));  // The percentage of damage reduction
			player.GetModPlayer<ExampleDashPlayer>().DashAccessoryEquipped = true;
		}
		public override void AddRecipes(){
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.EoCShield);
			recipe.AddIngredient(ItemID.SoulofSight, 20);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.Register();
		}
	}
	public class ExampleDashPlayer : ModPlayer{
		// These indicate what direction is what in the timer arrays used
		public const int DashDown = 0;
		public const int DashUp = 1;
		public const int DashRight = 2;
		public const int DashLeft = 3;

		public const int DashCooldown = 50;
		public const int DashDuration = 35; // dash afterimage effect

		// The initial velocity.  10 velocity is about 37.5 tiles/second or 50 mph
		public const float DashVelocity = 10f;

		// The direction the player has double tapped.  Defaults to -1 for no dash double tap
		public int DashDir = -1;

		public bool DashAccessoryEquipped;
		public int DashDelay = 0; // frames remaining till we can dash again
		public int DashTimer = 0; // frames remaining in the dash

		public override void ResetEffects() {
			DashAccessoryEquipped = false;

			// ResetEffects is called not long after player.doubleTapCardinalTimer's values have been set
			// When a directional key is pressed and released, vanilla starts a 15 tick (1/4 second) timer during which a second press activates a dash
			// If the timers are set to 15, then this is the first press just processed by the vanilla logic.  Otherwise, it's a double-tap
			if (Player.controlDown && Player.releaseDown && Player.doubleTapCardinalTimer[DashDown] < 15) {
				DashDir = DashDown;
			}
			else if (Player.controlUp && Player.releaseUp && Player.doubleTapCardinalTimer[DashUp] < 15) {
				DashDir = DashUp;
			}
			else if (Player.controlRight && Player.releaseRight && Player.doubleTapCardinalTimer[DashRight] < 15 && Player.doubleTapCardinalTimer[DashLeft] == 0) {
				DashDir = DashRight;
			}
			else if (Player.controlLeft && Player.releaseLeft && Player.doubleTapCardinalTimer[DashLeft] < 15 && Player.doubleTapCardinalTimer[DashRight] == 0) {
				DashDir = DashLeft;
			}
			else {
				DashDir = -1;
			}
		}

		// This is the perfect place to apply dash movement, it's after the vanilla movement code, and before the player's position is modified based on velocity.
		// If they double tapped this frame, they'll move fast this frame
		public override void PreUpdateMovement() {
			// if the player can use our dash, has double tapped in a direction, and our dash isn't currently on cooldown
			if (CanUseDash() && DashDir != -1 && DashDelay == 0) {
				Vector2 newVelocity = Player.velocity;

				switch (DashDir) {
					// Only apply the dash velocity if our current speed in the wanted direction is less than DashVelocity
					case DashUp when Player.velocity.Y > -DashVelocity:
					case DashDown when Player.velocity.Y < DashVelocity: {
							// Y-velocity is set here
							// If the direction requested was DashUp, then we adjust the velocity to make the dash appear "faster" due to gravity being immediately in effect
							// This adjustment is roughly 1.3x the intended dash velocity
							float dashDirection = DashDir == DashDown ? 1 : -1.3f;
							newVelocity.Y = dashDirection * DashVelocity;
							break;
						}
					case DashLeft when Player.velocity.X > -DashVelocity:
					case DashRight when Player.velocity.X < DashVelocity: {
							// X-velocity is set here
							float dashDirection = DashDir == DashRight ? 1 : -1;
							newVelocity.X = dashDirection * DashVelocity;
							break;
						}
					default:
						return; // not moving fast enough, so don't start our dash
				}

				// start our dash
				DashDelay = DashCooldown;
				DashTimer = DashDuration;
				Player.velocity = newVelocity;
				// Here you'd be able to set an effect that happens when the dash first activates
			}

			if (DashDelay > 0)
				DashDelay--;

			if (DashTimer > 0) { // dash is active
				// This is where we set the afterimage effect.  You can replace these two lines with whatever you want to happen during the dash
				// Some examples include:  spawning dust where the player is, adding buffs, making the player immune, etc.
				// Here we take advantage of "player.eocDash" and "player.armorEffectDrawShadowEOCShield" to get the Shield of Cthulhu's afterimage effect
				Player.eocDash = DashTimer;
				Player.armorEffectDrawShadowEOCShield = true;
				foreach (NPC npc in Main.npc) {
					if (Player.Hitbox.Intersects(npc.Hitbox)) {
						// 1️⃣ Damage Calculation
						bool crit = Main.rand.NextFloat() < Player.GetCritChance(DamageClass.Melee) / 100f;
						npc.SimpleStrikeNPC(60, 1, false, 1, DamageClass.Melee); // Apply damage, knockback, and direction

						npc.AddBuff(BuffID.CursedInferno, 180); // 3 sec of debuff
						npc.AddBuff(BuffID.BrokenArmor, 180);

						Player.immune = true;
						Player.immuneTime = 10;
					}
				}
				// count down frames remaining
				DashTimer--;
			}
		}

		private bool CanUseDash() {
			return DashAccessoryEquipped
				&& Player.dashType == DashID.None // player doesn't have Tabi or EoCShield equipped (give priority to those dashes)
				&& !Player.setSolar // player isn't wearing solar armor
				&& !Player.mount.Active; // player isn't mounted, since dashes on a mount look weird
		}
	}
}
