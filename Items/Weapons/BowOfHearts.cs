using LoveDay.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LoveDay.Items.Weapons
{
	/*
	 * A bow that shoots turns any fired  arrow into hear arrows
	 */
	class BowOfHearts : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bow of Hearts");
			Tooltip.SetDefault("Turns Arrows into Love Arrows");
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
			item.shoot = 1;

			// It's thiiiiiiis big!
			item.width = 12;
			item.height = 28;

		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			// Shoot custom arrow type but keep the basic properties of whatever arrow was shot
			type = mod.ProjectileType<ProjectileArrowLove>();
			return true;
		}
	}
}
