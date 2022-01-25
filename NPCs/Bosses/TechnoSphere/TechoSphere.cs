﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using System;

namespace BinaryTechnologies.NPCs.Bosses.TechnoSphere
{
    [AutoloadBossHead]
    public class TechnoSphere : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Techno Sphere");
            Main.npcFrameCount[NPC.type] = 1;

            NPCID.Sets.MPAllowedEnemies[Type] = true;
            NPCID.Sets.BossBestiaryPriority.Add(Type);

            NPCDebuffImmunityData debuffData = new NPCDebuffImmunityData
            {
                SpecificallyImmuneTo = new int[] {
                    BuffID.Poisoned,

                    BuffID.Confused
				}
            };
            NPCID.Sets.DebuffImmunitySets.Add(Type, debuffData);
        }

        public override string Texture => "BinaryTechnologies/NPCs/Bosses/TechnoSphere/SphereCore";

        public override string BossHeadTexture => "BinaryTechnologies/NPCs/Bosses/TechnoSphere/SphereCore";

        public override void SetDefaults()
        {
            NPC.width = 64;
            NPC.height = 64;
            NPC.damage = 30;
            NPC.defense = 20;
            NPC.lifeMax = 2200;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath14;
            NPC.value = Item.buyPrice(gold: 1);
            NPC.knockBackResist = 0f;
            NPC.boss = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.npcSlots = 10f;
            NPC.aiStyle = -1;
            NPC.scale = 1.5f;

            //AIType = NPCID.EyeofCthulhu;
            //AnimationType = NPCID.Zombie;
            //Banner = Item.NPCtoBanner(NPCID.Zombie);
            //BannerItem = Item.BannerToItem(Banner);
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                new FlavorTextBestiaryInfoElement(Language.GetTextValue(BinaryTechnologies.TransPath + "Bestiary.TechnoSphere"))
            });
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            for (int i = 0; i < 10; i++)
            {
                int dustType = Main.rand.Next(139, 143);
                int dustIndex = Dust.NewDust(NPC.position, NPC.width, NPC.height, dustType);
                Dust dust = Main.dust[dustIndex];
                dust.velocity.X = dust.velocity.X + Main.rand.Next(-50, 51) * 0.01f;
                dust.velocity.Y = dust.velocity.Y + Main.rand.Next(-50, 51) * 0.01f;
                dust.scale *= 1f + Main.rand.Next(-30, 31) * 0.01f;
            }
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            
        }

        public bool SecondPhase
        {
            get => NPC.ai[0] == 1f;
            set => NPC.ai[0] = value ? 1f : 0f;
        }

        public int ShieldLeft
        {
            get => (int)NPC.localAI[0];
            set => NPC.localAI[0] = value;
        }

        public float MovingPosX
        {
            get => NPC.ai[1];
            set => NPC.ai[1] = value;
        }

        public float MovingPosY
        {
            get => NPC.ai[2];
            set => NPC.ai[2] = value;
        }

        public float MovingTimer
        {
            get => NPC.ai[3];
            set => NPC.ai[3] = value;
        }

        public float LaserTimer
        {
            get => NPC.localAI[1];
            set => NPC.localAI[1] = value;
        }

        private float speed = 20f;

        private void SpawnShields()
        {
            float radius = 120f;

            if (ShieldLeft == 0 && !SecondPhase)
            {
                int index;
                TechnoSphereShield shield;
                index = NPC.NewNPC((int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<NPCs.Bosses.TechnoSphere.TechnoSphereShield>(), 0, NPC.whoAmI);
                shield = Main.npc[index].ModNPC as TechnoSphereShield;
                if (shield != null)
                {
                    shield.radius = new Vector2(radius, 0f);
                }
                index = NPC.NewNPC((int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<NPCs.Bosses.TechnoSphere.TechnoSphereShield>(), 0, NPC.whoAmI);
                shield = Main.npc[index].ModNPC as TechnoSphereShield;
                if (shield != null)
                {
                    shield.radius = new Vector2(-radius, 0f);
                }
                index = NPC.NewNPC((int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<NPCs.Bosses.TechnoSphere.TechnoSphereShield>(), 0, NPC.whoAmI);
                shield = Main.npc[index].ModNPC as TechnoSphereShield;
                if (shield != null)
                {
                    shield.radius = new Vector2(0f, radius);
                }
                index = NPC.NewNPC((int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<NPCs.Bosses.TechnoSphere.TechnoSphereShield>(), 0, NPC.whoAmI);
                shield = Main.npc[index].ModNPC as TechnoSphereShield;
                if (shield != null)
                {
                    shield.radius = new Vector2(0f, -radius);
                }
                ShieldLeft = 4;
            }
        }

        private void MovingAround()
        {
            if (!NPC.HasValidTarget) return;

            MovingTimer--;
            Vector2 targetPos;
            if (MovingTimer < 0f)
            {
                float YOffset = 200f;
                float XMaxOffset = 300f;
                MovingPosX = Main.rand.NextFloat(Main.player[NPC.target].position.X - XMaxOffset, 
                    Main.player[NPC.target].position.X + XMaxOffset);
                MovingPosY = Main.player[NPC.target].position.Y - YOffset;
                MovingTimer = 180f;
                //Main.NewText(MovingPosX + " " + MovingPosY);
            }
            targetPos = new Vector2(MovingPosX, MovingPosY);
            Vector2 destinationVector = targetPos - NPC.Center;

            float movingSpeed = Math.Min(200f, destinationVector.Length()) / speed;

            NPC.velocity = destinationVector.SafeNormalize(Vector2.Zero) * movingSpeed;
        }

        private void ShootLaser()
        {
            if (SecondPhase)
            {
                Player player = Main.player[NPC.target];

                if (LaserTimer <= 0f)
                {
                    Vector2 projDirection = Vector2.Normalize(player.position - NPC.Center) * 8f;
                    Projectile.NewProjectile(NPC.GetProjectileSpawnSource(), new Vector2(NPC.Center.X, NPC.Center.Y), projDirection, ProjectileID.DeathLaser, NPC.damage, 0f, Main.myPlayer);
                    LaserTimer = 120f;
                    NPC.netUpdate = true;
                }
                else
                {
                    LaserTimer--;
                }
            }
        }

        public override void AI()
        {
            if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
            {
                NPC.TargetClosest();
            }

            Player player = Main.player[NPC.target];

            if (player.dead)
            {
                NPC.velocity.Y -= 0.04f;
                NPC.EncourageDespawn(10);
                return;
            }

            if (ShieldLeft == 0 && MovingPosX != 0f && !SecondPhase)
            {
                SecondPhase = true;
                speed /= 2;
            }

            SpawnShields();

            NPC.dontTakeDamage = !SecondPhase;

            MovingAround();

            ShootLaser();
        }

        public void ShieldKilled()
        {
            ShieldLeft--;
            //Main.NewText(ShieldLeft);
            
        }



        public override void OnKill()
        {
            //NPC.SetEventFlagCleared(ref DownedBossSystem.downedMinionBoss, -1);
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.ElectMaterial>(), 3, 1, 1));
        }
    }
}