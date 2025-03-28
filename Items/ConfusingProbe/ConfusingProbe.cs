using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace ExpertPlus.Items.ConfusingProbe{

	/*public class ConfusingProbeDodge : ModPlayer{
		public void DodgeEffect(in Player.Hurtinfo info){
			ModPlayer.FreeDodge(Player, in Player.HurtInfo info);
			Player.SetImmuneTimeForAllTypes(60);
		}

	}*/
	public sealed class ConfusingProbe : ModItem{

		public static readonly string InlineWikiLibValue = @"
## Destroyer Collar ![AdvancedAccessoryCombinations/Items/ConfusingProbe]t4

The Destroyer Collar Grants 15% damage reduction and 6 defense.
";

		public override void SetStaticDefaults(){
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults(){
		    Item.accessory = true;
		    Item.width = 32;
		    Item.height = 32;
		    Item.value = Item.sellPrice(0, 18);
		    Item.rare = ItemRarityID.Expert;
		    Item.stack = 1;
		}

		public override void UpdateAccessory(Player player, bool hideVisual){
			//public float DodgeChance = 0.20f;
			/*if(Main.rand.Next(5) == 2){
				player.GetModPlayer<ConfusingProbeDodge>().DodgeEffect();
				//player.GetModPlayer<ConfusingProbeDodge>().DodgeEffect();
			}*/
			player.blackBelt = true;
		}

		public override void AddRecipes(){
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.BrainOfConfusion);
			recipe.AddIngredient(ItemID.SoulofMight, 20);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.Register();
        	}
	}
}
