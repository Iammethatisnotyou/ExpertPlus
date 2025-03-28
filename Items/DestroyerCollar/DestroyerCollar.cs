using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace ExpertPlus.Items.DestroyerCollar{
	[AutoloadEquip(EquipType.Neck)]
	public sealed class DestroyerCollar : ModItem{
		public static readonly string InlineWikiLibValue = @"
## Destroyer Collar ![AdvancedAccessoryCombinations/Items/DestroyerCollar]t4

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
		    Item.defense = 6;
		}

		public override void UpdateAccessory(Player player, bool hideVisual){
			player.endurance += 0.15f;
		}

		public override void AddRecipes(){
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.WormScarf);
			recipe.AddIngredient(ItemID.SoulofMight, 20);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.Register();
        	}
	}
}
