using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace LoveDay.Buffs
{
	/*
	 * A buff that heals both players and enemies
	 */
	class InLove : ModBuff
	{
		public const int HEAL_PER_SECOND = 4;

		public override void SetDefaults()
		{
			DisplayName.SetDefault("Love Regeneration");
			Description.SetDefault("The power of love regenerates some health!");
			Main.debuff[Type] = false;
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
			canBeCleared = false;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			if (npc.buffTime[buffIndex] % 60 == 0)
			{
				int heal = Math.Min(HEAL_PER_SECOND, npc.lifeMax - npc.life);
				if (heal < 1) return;
				npc.HealEffect(heal);
				npc.life += heal;
			}
		}

		public override void Update(Player player, ref int buffIndex)
		{
			if (player.buffTime[buffIndex] % 60 == 0)
			{
				int heal = Math.Min(HEAL_PER_SECOND, player.statLifeMax - player.statLife);
				if(heal < 1) return;
				player.HealEffect(heal);
				player.statLife += heal;
			}
		}
	}
}
