using LoveDay.Projectiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LoveDay.Items.Weapons
{
	/*
	 * Ammo item for Love Arrows
	 */
	class ItemArrowLove : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Love Arrow");
			Tooltip.SetDefault("Confuses and heals enemies");
		}

		public override void SetDefaults()
		{
			item.damage = 1;
			item.ranged = true;
			item.width = 14;
			item.height = 30;
			item.maxStack = 999;
			item.consumable = true;
			item.knockBack = 1f;
			item.value = Item.sellPrice(0, 0, 1, 0);
			item.rare = 8;
			item.shoot = mod.ProjectileType<ProjectileArrowLove>();
			item.ammo = AmmoID.Arrow; // The first item in an ammo class sets the AmmoID to it's type
		}
	}
}
