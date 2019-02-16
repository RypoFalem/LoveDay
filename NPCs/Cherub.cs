using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using LoveDay.Projectiles;

namespace LoveDay.NPCs
{
	/*
	 * A hostile NPC that attempts to float a safe distance from the player
	 * while pelting said player with heart arrows
	 */
	class Cherub : ModNPC
	{
		float xMaxSpeed = 5;
		float yMaxSpeed = 4;
		float xAcceleration = .1f;
		float yAcceleration = .1f;
		float xMaintainDistance = 150; // distance to stay away from player
		float yMaintainDistance = 100;
		const float PROJECTILE_SPEED = 10f; // speed of arrows shot
		const int AI_SHOOT_COUNTUP_INDEX = 2; // index of ai variable to track when it can shoot
		const int SHOOT_DELAY = 100; // delay between shots, roughly

		float AIShootCountup {
			get{return npc.ai[AI_SHOOT_COUNTUP_INDEX];}
			set{npc.ai[AI_SHOOT_COUNTUP_INDEX] = value;}
		}

		Player TargetPlayer {
			get { return Main.player[npc.target]; }
			set { Main.player[npc.target] = value; }
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cherub");
		}

		public override void SetDefaults()
		{
			npc.width = 34;
			npc.height = 48;
			npc.aiStyle = -1;
			npc.damage = 26;
			npc.defense = 12;
			npc.lifeMax = 96;
			npc.HitSound = SoundID.NPCHit1;
			npc.knockBackResist = 0.8f;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.value = 1000f;
			npc.noGravity = true;
			npc.scale = Main.rand.NextFloat(.5f) + .5f;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (spawnInfo.player.ZoneHoly
				&& spawnInfo.player.ZoneOverworldHeight)
				return SpawnCondition.OverworldHallow.Chance;
			return 0;
		}

		// random number between -range and +range
		private float RandomOffset(float range)
		{
			return Main.rand.NextFloat(range * 2 + 1) - range;
		}

		public override void AI()
		{
			npc.noGravity = true;
			if (npc.ai[0] == 0f)
			{
				npc.TargetClosest(true);
				if (Main.netMode != 1)
				{
					if (npc.velocity.X != 0f || npc.velocity.Y < 0f || (double)npc.velocity.Y > 0.3)
					{
						npc.ai[0] = 1f;
						npc.netUpdate = true;
					}
					else
					{
						Rectangle rectangle = new Rectangle((int)TargetPlayer.position.X, (int)TargetPlayer.position.Y, TargetPlayer.width, TargetPlayer.height);
						Rectangle rectangle2 = new Rectangle((int)npc.position.X - 100, (int)npc.position.Y - 100, npc.width + 200, npc.height + 200);
						if (rectangle2.Intersects(rectangle) || npc.life < npc.lifeMax)
						{
							npc.ai[0] = 1f;
							npc.velocity.Y = npc.velocity.Y - 6f;
							npc.netUpdate = true;
						}
					}
				}
			}
			// Move around player
			else if (!TargetPlayer.dead)
			{
				if (npc.collideX)
				{
					npc.velocity.X = npc.oldVelocity.X * -0.5f;
					if (npc.direction == -1 && npc.velocity.X > 0f && npc.velocity.X < 2f)
					{
						npc.velocity.X = 2f;
					}
					if (npc.direction == 1 && npc.velocity.X < 0f && npc.velocity.X > -2f)
					{
						npc.velocity.X = -2f;
					}
				}
				if (npc.collideY)
				{
					npc.velocity.Y = npc.oldVelocity.Y * -0.5f;
					if (npc.velocity.Y > 0f && npc.velocity.Y < 1f)
					{
						npc.velocity.Y = 1f;
					}
					if (npc.velocity.Y < 0f && npc.velocity.Y > -1f)
					{
						npc.velocity.Y = -1f;
					}
				}

				// Set movement goal above and away from player
				npc.TargetClosest(true);
				float xGoal = npc.direction == 1
					? TargetPlayer.Center.X - xMaintainDistance
					: TargetPlayer.Center.X + xMaintainDistance;
				xGoal += RandomOffset(10);
				float yGoal = TargetPlayer.position.Y - (float)( npc.height / 2 ) - yMaintainDistance + RandomOffset(10);
				// Adjust movement goal away from other cherubs
				foreach (NPC otherNPC in Main.npc)
				{
					if (!otherNPC.active
						|| otherNPC.type != npc.type
						|| ( otherNPC.position - npc.position ).LengthSquared() > npc.width * npc.height * 5)
						continue;
					xGoal += npc.position.X - otherNPC.position.X > 0
						? npc.width : -npc.width;
					yGoal += npc.position.Y - otherNPC.position.Y > 0
						? npc.height : -npc.height;
				}

				// Move towards Movement goal
				if (npc.Center.X > xGoal && npc.velocity.X > -xMaxSpeed)
				{
					npc.velocity.X = npc.velocity.X - xAcceleration;
					if (npc.velocity.X > xMaxSpeed)
					{
						npc.velocity.X = npc.velocity.X - xAcceleration;
					}
					else if (npc.velocity.X > 0f)
					{
						npc.velocity.X = npc.velocity.X - xAcceleration / 2;
					}
					if (npc.velocity.X < -xMaxSpeed)
					{
						npc.velocity.X = -xMaxSpeed;
					}
				}
				else if (npc.Center.X < xGoal && npc.velocity.X < xMaxSpeed)
				{
					npc.velocity.X = npc.velocity.X + xAcceleration;
					if (npc.velocity.X < -3f)
					{
						npc.velocity.X = npc.velocity.X + xAcceleration;
					}
					else if (npc.velocity.X < 0f)
					{
						npc.velocity.X = npc.velocity.X + xAcceleration / 2;
					}
					if (npc.velocity.X > xMaxSpeed)
					{
						npc.velocity.X = xMaxSpeed;
					}
				}
				if (npc.position.Y < yGoal)
				{
					npc.velocity.Y = npc.velocity.Y + yAcceleration;
					if (npc.velocity.Y < 0f)
					{
						npc.velocity.Y = npc.velocity.Y + yAcceleration / 5;
					}
				}
				else
				{
					npc.velocity.Y = npc.velocity.Y - yAcceleration;
					if (npc.velocity.Y > 0f)
					{
						npc.velocity.Y = npc.velocity.Y - yAcceleration / 5;
					}
				}
				if (npc.velocity.Y < -yMaxSpeed)
				{
					npc.velocity.Y = -yMaxSpeed;
				}
				if (npc.velocity.Y > yMaxSpeed)
				{
					npc.velocity.Y = yMaxSpeed;
				}
			}

			//slow down if wet
			if (npc.wet)
			{
				if (npc.velocity.Y > 0f)
				{
					npc.velocity.Y = npc.velocity.Y * 0.95f;
				}
				npc.velocity.Y = npc.velocity.Y - 0.5f;
				if (npc.velocity.Y < -4f)
				{
					npc.velocity.Y = -4f;
				}
				npc.TargetClosest(true);
				return;
			}

			// Face the sprite towards target player or in the direction it moves
			if (!TargetPlayer.dead)
			{
				float xDeltaDistance = npc.position.X + (float)( npc.width / 2 ) - ( TargetPlayer.position.X + (float)( TargetPlayer.width / 2 ) );
				npc.spriteDirection = xDeltaDistance > 0 ? -1 : 1;
			}
			else
			{
				if (npc.velocity.X > 0f)
				{
					npc.spriteDirection = 1;
				}
				if (npc.velocity.X < 0f)
				{
					npc.spriteDirection = -1;
				}
			}

			// Arrow Shoot and cooldown logic
			if (Main.netMode != 1)
			{
				AIShootCountup += Main.rand.Next(5, 20) * 0.1f * npc.scale;
				if (TargetPlayer.stealth == 0f && TargetPlayer.itemAnimation == 0)
				{
					AIShootCountup = 0f;
				}

				// Shoot a projectile at the player
				if (!TargetPlayer.dead && AIShootCountup > SHOOT_DELAY)
				{
					if (Collision.CanHit(npc.position, npc.width, npc.height, TargetPlayer.position, TargetPlayer.width, TargetPlayer.height))
					{
						//public static int NewProjectile(Vector2 position, Vector2 velocity, int Type, int Damage, float KnockBack, int Owner = 255, float ai0 = 0, float ai1 = 0);
						Vector2 direction = TargetPlayer.Center - npc.Center;
						direction.Normalize();
						int proj = Projectile.NewProjectile(
							npc.Center, direction * PROJECTILE_SPEED, mod.ProjectileType<ProjectileArrowLoveHostile>(), npc.damage / 2, 0f, Main.myPlayer, 0, 0);
						AIShootCountup = 0f;
					}
				}
			}

			// rotate the sprite towards it's movement direction
			npc.rotation = npc.velocity.X * 0.05f;
		}
	}
}
