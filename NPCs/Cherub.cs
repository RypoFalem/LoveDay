using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using System;
using System.Linq;

namespace LoveDay.NPCs
{
	class Cherub : ModNPC
	{
		float xMaxSpeed = 5;
		float yMaxSpeed = 4;
		float xAcceleration = .1f;
		float yAcceleration = .1f;
		float xMaintainDistance = 150;
		float yMaintainDistance = 100;
		const float PROJECTILE_SPEED = 10f;
		const int AI_SHOOT_COUNTUP = 2;

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

		float RandomOffset(float range)
		{
			return Main.rand.NextFloat(range * 2 + 1) - range;
		}

		public override void AI()
		{
			
			npc.noGravity = true;
			if (npc.ai[0] == 0f) {
				npc.TargetClosest(true);
				if (Main.netMode != 1) {
					if (npc.velocity.X != 0f || npc.velocity.Y < 0f || (double)npc.velocity.Y > 0.3) {
						npc.ai[0] = 1f;
						npc.netUpdate = true;
					} else {
						Rectangle rectangle = new Rectangle((int)Main.player[npc.target].position.X, (int)Main.player[npc.target].position.Y, Main.player[npc.target].width, Main.player[npc.target].height);
						Rectangle rectangle2 = new Rectangle((int)npc.position.X - 100, (int)npc.position.Y - 100, npc.width + 200, npc.height + 200);
						if (rectangle2.Intersects(rectangle) || npc.life < npc.lifeMax) {
							npc.ai[0] = 1f;
							npc.velocity.Y = npc.velocity.Y - 6f;
							npc.netUpdate = true;
						}
					}
				}
			}
			// Move around player
			else if (!Main.player[npc.target].dead) {
				if (npc.collideX) {
					npc.velocity.X = npc.oldVelocity.X * -0.5f;
					if (npc.direction == -1 && npc.velocity.X > 0f && npc.velocity.X < 2f) {
						npc.velocity.X = 2f;
					}
					if (npc.direction == 1 && npc.velocity.X < 0f && npc.velocity.X > -2f) {
						npc.velocity.X = -2f;
					}
				}
				if (npc.collideY) {
					npc.velocity.Y = npc.oldVelocity.Y * -0.5f;
					if (npc.velocity.Y > 0f && npc.velocity.Y < 1f) {
						npc.velocity.Y = 1f;
					}
					if (npc.velocity.Y < 0f && npc.velocity.Y > -1f) {
						npc.velocity.Y = -1f;
					}
				}

				// Set movement goal above and away from player
				npc.TargetClosest(true);
				float xGoal = npc.direction == 1
					? Main.player[npc.target].Center.X - xMaintainDistance
					: Main.player[npc.target].Center.X + xMaintainDistance;
				xGoal += RandomOffset(10);
				float yGoal = Main.player[npc.target].position.Y - (float)( npc.height / 2 ) - yMaintainDistance + RandomOffset(10);
				// Adjust movement goal away from other cherubs
				foreach (NPC otherNPC in Main.npc) {
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
				if (npc.Center.X > xGoal && npc.velocity.X > -xMaxSpeed) {
					npc.velocity.X = npc.velocity.X - xAcceleration;
					if (npc.velocity.X > xMaxSpeed) {
						npc.velocity.X = npc.velocity.X - xAcceleration;
					} else if (npc.velocity.X > 0f) {
						npc.velocity.X = npc.velocity.X - xAcceleration / 2;
					}
					if (npc.velocity.X < -xMaxSpeed) {
						npc.velocity.X = -xMaxSpeed;
					}
				} else if (npc.Center.X < xGoal && npc.velocity.X < xMaxSpeed) {
					npc.velocity.X = npc.velocity.X + xAcceleration;
					if (npc.velocity.X < -3f) {
						npc.velocity.X = npc.velocity.X + xAcceleration;
					} else if (npc.velocity.X < 0f) {
						npc.velocity.X = npc.velocity.X + xAcceleration / 2;
					}
					if (npc.velocity.X > xMaxSpeed) {
						npc.velocity.X = xMaxSpeed;
					}
				}
				if (npc.position.Y < yGoal) {
					npc.velocity.Y = npc.velocity.Y + yAcceleration;
					if (npc.velocity.Y < 0f) {
						npc.velocity.Y = npc.velocity.Y + yAcceleration / 5;
					}
				} else {
					npc.velocity.Y = npc.velocity.Y - yAcceleration;
					if (npc.velocity.Y > 0f) {
						npc.velocity.Y = npc.velocity.Y - yAcceleration / 5;
					}
				}
				if (npc.velocity.Y < -yMaxSpeed) {
					npc.velocity.Y = -yMaxSpeed;
				}
				if (npc.velocity.Y > yMaxSpeed) {
					npc.velocity.Y = yMaxSpeed;
				}
			}

			//slow down if wet
			if (npc.wet) {
				if (npc.velocity.Y > 0f) {
					npc.velocity.Y = npc.velocity.Y * 0.95f;
				}
				npc.velocity.Y = npc.velocity.Y - 0.5f;
				if (npc.velocity.Y < -4f) {
					npc.velocity.Y = -4f;
				}
				npc.TargetClosest(true);
				return;
			}

			// Face the sprite towards target player or in the direction it moves
			if (!Main.player[npc.target].dead) {
				float xDeltaDistance = npc.position.X + (float)( npc.width / 2 ) - ( Main.player[npc.target].position.X + (float)( Main.player[npc.target].width / 2 ) );
				npc.spriteDirection = xDeltaDistance > 0 ? -1 : 1;
			} else {
				if (npc.velocity.X > 0f) {
					npc.spriteDirection = 1;
				}
				if (npc.velocity.X < 0f) {
					npc.spriteDirection = -1;
				}
			}

			if (Main.netMode != 1) {
				npc.ai[AI_SHOOT_COUNTUP] += Main.rand.Next(5, 20) * 0.1f * npc.scale;
				if (Main.player[npc.target].stealth == 0f && Main.player[npc.target].itemAnimation == 0) {
					npc.ai[AI_SHOOT_COUNTUP] = 0f;
				}

				// Shoot a projectile at the player
				if (!Main.player[npc.target].dead && npc.ai[AI_SHOOT_COUNTUP] > 45f) {
					if (Collision.CanHit(npc.position, npc.width, npc.height, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height)) {
						//public static int NewProjectile(Vector2 position, Vector2 velocity, int Type, int Damage, float KnockBack, int Owner = 255, float ai0 = 0, float ai1 = 0);
						Vector2 direction = Main.player[npc.target].Center - npc.Center;
						direction.Normalize();
						int proj = Projectile.NewProjectile(
							npc.Center, direction * PROJECTILE_SPEED , ProjectileID.WoodenArrowHostile, npc.damage / 2, 0f, Main.myPlayer, 0, 0);
						npc.ai[AI_SHOOT_COUNTUP] = 0f;
					}
				}
			}



			npc.rotation = npc.velocity.X * 0.05f;
		}
	}
}
