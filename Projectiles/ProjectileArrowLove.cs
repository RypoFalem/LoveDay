using LoveDay.Buffs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LoveDay.Projectiles
{
	/*
	 * An arrow that can inflict regeneration and confusion on the target
	 */
	class ProjectileArrowLove : ModProjectile
	{
		// 5 seconds
		private const int BUFF_TIME = 5 * 60;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Love Arrow");
		}

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

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(mod.BuffType<InLove>(), BUFF_TIME); // NPCs don't react to regen buff sadly
			target.AddBuff(BuffID.Confused, BUFF_TIME);
			target.AddBuff(BuffID.Lovestruck, BUFF_TIME);
		}

		public override void OnHitPvp(Player target, int damage, bool crit) => OnHitPlayer(target, damage, crit);
		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			target.AddBuff(mod.BuffType<InLove>(), BUFF_TIME);
			target.AddBuff(BuffID.Confused, BUFF_TIME);
			target.AddBuff(BuffID.Lovestruck, BUFF_TIME);
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
