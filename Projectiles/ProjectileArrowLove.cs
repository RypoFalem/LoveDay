﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;

namespace LoveDay.Projectiles
{
	/*
	 * An arrow that can inflict regeneration and confusion on the target
	 */
	class ProjectileArrowLove : ModProjectile
	{

		public override void SetDefaults()
		{
			projectile.arrow = true;
			projectile.aiStyle = 1;
			projectile.width = 14;
			projectile.height = 30;
			projectile.friendly = true;
			projectile.ranged = true;
			aiType = ProjectileID.WoodenArrowFriendly;
		}
	}

	class ProjectileArrowLoveHostile : ProjectileArrowLove
	{
		public override string Texture => "LoveDay/Projectiles/ProjectileArrowLove";

		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.friendly = false;
			projectile.hostile = true;
			aiType = ProjectileID.WoodenArrowHostile;
		}
	}
}
