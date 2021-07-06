using ReLogic.OS;
using Terraria;
using Terraria.ModLoader;

namespace ModdersToolkit.Tools.Shaders
{
	internal class ShaderPlayer : ModPlayer
	{
		public override void PostUpdate() {
			if (Platform.IsWindows) {
				bool forcedShaderActive = ShaderTool.shaderUI.forceShaderCheckbox.Selected && ShaderTool.shaderUI.lastShaderIsScreenShader;
				Player.ManageSpecialBiomeVisuals("ModdersToolkit:TestScreenShader", forcedShaderActive);
			}
		}

		//	public override void UpdateBiomeVisuals() {
		//		if (Platform.IsWindows) {
		//			bool forcedShaderActive = ShaderTool.shaderUI.forceShaderCheckbox.Selected && ShaderTool.shaderUI.lastShaderIsScreenShader;
		//			Player.ManageSpecialBiomeVisuals("ModdersToolkit:TestScreenShader", forcedShaderActive);
		//		}
		//	}
	}

	//class ShaderVisuals : ModBiome
	//{
	//	public override void BiomeVisuals(Player player) {
	//		if (Platform.IsWindows) {
	//			bool forcedShaderActive = ShaderTool.shaderUI.forceShaderCheckbox.Selected && ShaderTool.shaderUI.lastShaderIsScreenShader;
	//			player.ManageSpecialBiomeVisuals("ModdersToolkit:TestScreenShader", forcedShaderActive);
	//		}
	//	}
	//}
}
