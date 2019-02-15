using LoveDay.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LoveDay.Items.Weapons
{
	class BowOfHearts : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bow of Hearts");
			Tooltip.SetDefault("Turns Wooden Arrows into Love Arrows");
		}

		public override void SetDefaults()
		{
			// basic stats
			item.damage = 40;
			item.knockBack = 4.5f;
			item.useTime = 25;
			item.useAnimation = 25;
			item.value = 35000;
			item.autoReuse = true;

			// It's a bow
			item.useAmmo = AmmoID.Arrow;
			item.UseSound = SoundID.Item5; // bow shooty sound
			item.noMelee = true;
			item.useStyle = 5; // ranged style
			item.ranged = true;
			item.shootSpeed = 6.6f; // arrow velocity?
			item.shoot = mod.ProjectileType<ProjectileArrowLove>();

			// It's thiiiiiiis big!
			item.width = 12;
			item.height = 28;

		}

		public override void PickAmmo(Player player, ref int type, ref float speed, ref int damage, ref float knockback)
		{
			Main.NewText("type: " + type);
		}
	}
}
