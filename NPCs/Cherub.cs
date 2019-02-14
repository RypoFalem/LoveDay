using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using System;

namespace LoveDay.NPCs
{
    class Cherub : ModNPC
    {
		float xMaxSpeed = 5;
		float yMaxSpeed = 4;
		float xAcceleration = .1f;
		float yAcceleration = .1f;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cherub");
        }

        public override void SetDefaults()
        {
            npc.CloneDefaults(NPCID.Hornet);
			npc.aiStyle = -1;
            npc.value = 1000;
		}

        public override void AI()
        {
            npc.noGravity = true;
            if (npc.ai[0] == 0f) {
                npc.noGravity = false;
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
            } else if (!Main.player[npc.target].dead) {
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
                npc.TargetClosest(true);
                if (npc.direction == -1 && npc.velocity.X > -xMaxSpeed) { 
                    npc.velocity.X = npc.velocity.X - xAcceleration;
                    if (npc.velocity.X > xMaxSpeed) {
                        npc.velocity.X = npc.velocity.X - xAcceleration;
                    } else if (npc.velocity.X > 0f) {
                        npc.velocity.X = npc.velocity.X - xAcceleration/2; 
                    }
                    if (npc.velocity.X < -xMaxSpeed) {
                        npc.velocity.X = -xMaxSpeed;
                    }
                } else if (npc.direction == 1 && npc.velocity.X < xMaxSpeed) { 
                    npc.velocity.X = npc.velocity.X + xAcceleration;
                    if (npc.velocity.X < -3f) {
                        npc.velocity.X = npc.velocity.X + xAcceleration;
                    } else if (npc.velocity.X < 0f) {
                        npc.velocity.X = npc.velocity.X + xAcceleration/2; 
					}
                    if (npc.velocity.X > xMaxSpeed) {
                        npc.velocity.X = xMaxSpeed;
                    }
                }
                float xDistanceFromPlayer = Math.Abs(npc.position.X + (float)( npc.width / 2 ) - ( Main.player[npc.target].position.X + (float)( Main.player[npc.target].width / 2 ) ));
                float yGoal = Main.player[npc.target].position.Y - (float)( npc.height / 2 );
                if (xDistanceFromPlayer > 50f) {
                    yGoal -= 100f;
                }
                if (npc.position.Y < yGoal) {
                    npc.velocity.Y = npc.velocity.Y + yAcceleration;
                    if (npc.velocity.Y < 0f) {
                        npc.velocity.Y = npc.velocity.Y + yAcceleration/5;
                    }
                } else {
                    npc.velocity.Y = npc.velocity.Y - yAcceleration;
                    if (npc.velocity.Y > 0f) {
                        npc.velocity.Y = npc.velocity.Y - yAcceleration/5;
                    }
                }
                if (npc.velocity.Y < -yMaxSpeed) {
                    npc.velocity.Y = -yMaxSpeed;
                }
                if (npc.velocity.Y > yMaxSpeed) {
                    npc.velocity.Y = yMaxSpeed;
                }
            }
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
        }
    }
}
