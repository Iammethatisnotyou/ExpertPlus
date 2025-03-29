using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace ExpertPlus.Items.GooeyAlliance{
	public sealed class GooeyAlliance : ModItem{
		public static readonly string InlineWikiLibValue = @"
## Destroyer Collar ![ExpertPlus/Items/GooeyAlliance]t4

The Destroyer Collar Grants 15% damage reduction and 6 defense.
";

		public override void SetStaticDefaults(){
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults(){
		    Item.accessory = true;
		    Item.width = 48;
		    Item.height = 48;
		    Item.value = Item.sellPrice(0, 10);
		    Item.rare = ItemRarityID.Expert;
		    Item.stack = 1;
		}

		public override void UpdateAccessory(Player player, bool hideVisual){
			player.npcTypeNoAggro[NPCID.BlueSlime] = true;
			player.npcTypeNoAggro[NPCID.SpikedIceSlime] = true;
			player.npcTypeNoAggro[NPCID.SpikedJungleSlime] = true;
			player.npcTypeNoAggro[NPCID.SlimeSpiked] = true;
			player.npcTypeNoAggro[NPCID.ToxicSludge] = true;
			player.npcTypeNoAggro[NPCID.MotherSlime] = true;
			player.npcTypeNoAggro[NPCID.LavaSlime] = true;
			player.npcTypeNoAggro[NPCID.DungeonSlime] = true;
			player.npcTypeNoAggro[NPCID.IlluminantSlime] = true;
			player.npcTypeNoAggro[NPCID.CorruptSlime] = true;
			player.npcTypeNoAggro[NPCID.Crimslime] = true;

			player.volatileGelatin = true;
		}

		public override void AddRecipes(){
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.VolatileGelatin);
			recipe.AddIngredient(ItemID.RoyalGel);
			recipe.AddIngredient(ItemID.SoulofLight, 10);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.Register();
        	}
	}
}
