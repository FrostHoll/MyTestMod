﻿using Terraria;
using Terraria.ModLoader;

namespace MyTestMod.Buffs
{
	// Ethereal Flames is an example of a buff that causes constant loss of life.
	// See ExamplePlayer.UpdateBadLifeRegen and ExampleGlobalNPC.UpdateLifeRegen for more information.
	public class ElectShock : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Electrical Shock");
			Description.SetDefault("Greatly losing life");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.GetModPlayer<MyTestModPlayer>().electShock = true;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.GetGlobalNPC<MyGlobalNPC>().electShock = true;
		}
	}
}
