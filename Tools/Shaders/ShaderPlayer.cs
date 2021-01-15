using ReLogic.OS;
using Terraria.ModLoader;

namespace ModdersToolkit.Tools.Shaders
{
	internal class ShaderPlayer : ModPlayer
	{
		public override void UpdateBiomeVisuals() {
			if (Platform.IsWindows) {
				bool forcedShaderActive = ShaderTool.shaderUI.forceShaderCheckbox.Selected && ShaderTool.shaderUI.lastShaderIsScreenShader;
				Player.ManageSpecialBiomeVisuals("ModdersToolkit:TestScreenShader", forcedShaderActive);
			}
		}
	}
}
