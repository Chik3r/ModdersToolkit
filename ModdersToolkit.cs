using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ModdersToolkit.Tools;
using ReLogic.OS;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

//todo, tool to make townnpc spritesheet out of current player 
//todo, Main.ignoreErrors = true; -- set to false and report errors to console
// TODO: Trigger spawn TownNPC

// TODO: Clear Projectiles
// TODO: Reveal Map (Also, highlight changes?)
// TODO: Tooltips/Stats: Player position, block names, block strength (minpick).
// TODO: Moveable Windows
// TODO:

namespace ModdersToolkit
{
	public class ModdersToolkit : Mod
	{
		public override void Load() => Instance = this;

		public override void Unload() => Instance = null;

		public static ModdersToolkit Instance { get; set; }
	}
}
