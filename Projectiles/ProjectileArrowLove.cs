using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;

namespace LoveDay.Projectiles
{
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
}
