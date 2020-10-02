﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using System;
using Terraria.ID;
using System.Linq;
using System.Text;
using ModdersToolkit.UIElements;
using ModdersToolkit.Tools;
using Terraria.Graphics.Shaders;
using System.Collections.Generic;

namespace ModdersToolkit.Tools.Projectiles
{
	// TODO: DrawOffsetX and Y setting
	// Also NPC drawOffsetY

    internal class ProjectilesUI : UIState
	{
		internal UIPanel mainPanel;
		private UserInterface userInterface;
		public ProjectilesUI(UserInterface userInterface)
		{
			this.userInterface = userInterface;
		}

		internal static UIFloatRangedDataValue speedXDataProperty;
		internal static UIFloatRangedDataValue speedYDataProperty;
		internal static UIFloatRangedDataValue ai0DataProperty;
		internal static UIFloatRangedDataValue ai1DataProperty;
		internal static UIIntRangedDataValue damageDataProperty;
		internal static UIIntRangedDataValue aiStyleDataProperty;
		internal static UIIntRangedDataValue drawOffsetXDataProperty;
		internal static UIIntRangedDataValue drawOriginOffsetXDataProperty;
		internal static UIIntRangedDataValue drawOriginOffsetYDataProperty;
		//internal static UIIntRangedDataValue aiTypeDataProperty;
		internal static UIFloatRangedDataValue knockbackDataProperty;
		internal static UIBoolNDataValue hostile;
		internal static UIBoolNDataValue friendly;
		internal static UICheckbox pause;
		internal static UICheckbox freeze;
		internal static bool stepPrevious;
		internal static bool step;

        private UIGrid projectileGrid;
		internal NewUITextBox searchFilter;

		public override void OnInitialize()
		{
			mainPanel = new UIPanel();
			int width = 310;
			int height = 610;
			mainPanel.Left.Set(-40f - width, 1f);
			mainPanel.Top.Set(-100f - height, 1f);
			mainPanel.Width.Set(width, 0f);
			mainPanel.Height.Set(height, 0f);
			mainPanel.SetPadding(12);
			mainPanel.BackgroundColor = Color.Orange * 0.7f;

			int top = 0;
			UIText text = new UIText("Projectiles:", 0.85f);
			text.Top.Set(top, 0f);
			mainPanel.Append(text);

			UITextPanel<string> clearProjectilesButton = new UITextPanel<string>("Clear Projectiles");
			clearProjectilesButton.OnClick += (a, b) =>
			{
				for (int i = 0; i < Main.maxProjectiles; i++)
				{
					if (Main.projectile[i].active)
					{
						Main.projectile[i].Kill();
						if (Main.netMode == 1)
						{
							NetMessage.SendData(27, -1, -1, null, i, 0f, 0f, 0f, 0);
						}
					}
				}
			};
			clearProjectilesButton.Top.Set(top, 0f);
			clearProjectilesButton.Width.Set(-10, 0.5f);
			clearProjectilesButton.HAlign = 1;
			clearProjectilesButton.SetPadding(4);
			mainPanel.Append(clearProjectilesButton);

			top += 20;

			UIText text2 = new UIText("Filter:", 0.85f);
			text2.Top.Set(top, 0f);
			mainPanel.Append(text2);

			searchFilter = new NewUITextBox("Search", 0.85f);
			searchFilter.SetPadding(0);
			searchFilter.OnTextChanged += () => { ValidateInput(); updateneeded = true; };
			searchFilter.Top.Set(top, 0f);
			searchFilter.Left.Set(text2.GetInnerDimensions().Width, 0f);
			searchFilter.Width.Set(-text2.GetInnerDimensions().Width, 1f);
			searchFilter.Height.Set(20, 0f);
			//searchFilter.VAlign = 0.5f;
			mainPanel.Append(searchFilter);
			top += 20;

			speedXDataProperty = new UIFloatRangedDataValue("SpeedX:", 0, -10, 10);
			speedYDataProperty = new UIFloatRangedDataValue("SpeedY:", 0, -10, 10);
			var ui2DRange = new UI2DRange<float>(speedXDataProperty, speedYDataProperty);
			ui2DRange.Top.Set(top, 0f);
			mainPanel.Append(ui2DRange);
			top += 30;

			ai0DataProperty = new UIFloatRangedDataValue("ai0:", 0, -10, 10);
			var uiRange = new UIRange<float>(ai0DataProperty);
			uiRange.Top.Set(top, 0f);
			uiRange.Width.Set(0, 1f);
			mainPanel.Append(uiRange);
			top += 30;

			ai1DataProperty = new UIFloatRangedDataValue("ai1:", 0, -10, 10);
			uiRange = new UIRange<float>(ai1DataProperty);
			uiRange.Top.Set(top, 0f);
			uiRange.Width.Set(0, 1f);
			mainPanel.Append(uiRange);
			top += 30;

			knockbackDataProperty = new UIFloatRangedDataValue("Knockback:", 0, 0, 20);
			uiRange = new UIRange<float>(knockbackDataProperty);
			uiRange.Top.Set(top, 0f);
			uiRange.Width.Set(0, 1f);
			mainPanel.Append(uiRange);
			top += 30;

			damageDataProperty = new UIIntRangedDataValue("Damage:", 20, 0, 200);
			var uiRangeI = new UIRange<int>(damageDataProperty);
			uiRangeI.Top.Set(top, 0f);
			uiRangeI.Width.Set(0, 1f);
			mainPanel.Append(uiRangeI);
			top += 30;

			aiStyleDataProperty = new UIIntRangedDataValue("AIStyle:", -1, -1, 145);
			uiRangeI = new UIRange<int>(aiStyleDataProperty);
			uiRangeI.Top.Set(top, 0f);
			uiRangeI.Width.Set(0, 1f);
			mainPanel.Append(uiRangeI);
			top += 30;

			drawOffsetXDataProperty = new UIIntRangedDataValue("drawOffsetX:", 0, -30, 30);
			uiRangeI = new UIRange<int>(drawOffsetXDataProperty);
			uiRangeI.Top.Set(top, 0f);
			uiRangeI.Width.Set(0, 1f);
			mainPanel.Append(uiRangeI);
			top += 30;

			drawOriginOffsetXDataProperty = new UIIntRangedDataValue("drawOriginOffsetX:", 0, -30, 30);
			uiRangeI = new UIRange<int>(drawOriginOffsetXDataProperty);
			uiRangeI.Top.Set(top, 0f);
			uiRangeI.Width.Set(0, 1f);
			mainPanel.Append(uiRangeI);
			top += 30;

			drawOriginOffsetYDataProperty = new UIIntRangedDataValue("drawOriginOffsetY:", 0, -30, 30);
			uiRangeI = new UIRange<int>(drawOriginOffsetYDataProperty);
			uiRangeI.Top.Set(top, 0f);
			uiRangeI.Width.Set(0, 1f);
			mainPanel.Append(uiRangeI);
			top += 30;

			hostile = new UIBoolNDataValue("Hostile");
			var hostileCheckbox = new UITriStateCheckbox(hostile);
			hostileCheckbox.Top.Set(top, 0f);
			mainPanel.Append(hostileCheckbox);
			top += 30;

			friendly = new UIBoolNDataValue("Friendly");
			var friendlyCheckbox = new UITriStateCheckbox(friendly);
			friendlyCheckbox.Top.Set(top, 0f);
			mainPanel.Append(friendlyCheckbox);
			top += 25;

			pause = new UICheckbox("Pause", "Pauses All Projectiles (Prevent AI from running)");
			pause.Top.Set(top, 0f);
			mainPanel.Append(pause);

			UIHoverImageButton stepButton = new UIHoverImageButton(ModdersToolkit.Instance.GetTexture("UIElements/next"), "Step");
			stepButton.OnClick += (s, e) => step = true;
			stepButton.Top.Set(top - 6, 0f);
			stepButton.Left.Set(80, 0f);
			mainPanel.Append(stepButton);

			top += 25;

			freeze = new UICheckbox("Freeze", "Zero out velocity during PreAI for All Projectiles");
			freeze.Top.Set(top, 0f);
			mainPanel.Append(freeze);
			top += 25;

			projectileGrid = new UIGrid(7);
			projectileGrid.Top.Pixels = top;
			//autoTrashGrid.Left.Pixels = spacing;
			projectileGrid.Width.Set(-25f, 1f); // leave space for scroll?
			projectileGrid.Height.Set(-top, 1f);
			projectileGrid.ListPadding = 6f;
			mainPanel.Append(projectileGrid);

			// this will initialize grid
			updateneeded = true;

			var projectileGridScrollbar = new UIElements.FixedUIScrollbar(userInterface);
			projectileGridScrollbar.SetView(100f, 1000f);
			projectileGridScrollbar.Top.Pixels = top;// + spacing;
			projectileGridScrollbar.Height.Set(-top /*- spacing*/, 1f);
			projectileGridScrollbar.HAlign = 1f;
			mainPanel.Append(projectileGridScrollbar);
			projectileGrid.SetScrollbar(projectileGridScrollbar);

			Append(mainPanel);
		}

		private void ValidateInput()
		{
			if (searchFilter.Text.Length > 0)
			{
				bool found = false;
				for (int i = 1; i < Main.projectileTexture.Length; i++)
				{
					if (Lang.GetProjectileName(i).Value.ToLower().IndexOf(searchFilter.Text, StringComparison.OrdinalIgnoreCase) != -1)
					{
						found = true;
						break;
					}
				}
				if (!found)
				{
					searchFilter.SetText(searchFilter.Text.Substring(0, searchFilter.Text.Length - 1));
				}
			}
		}

		private bool updateneeded;
		internal void UpdateGrid()
		{
			if (!updateneeded) { return; }
			updateneeded = false;

			projectileGrid.Clear();
			for (int i = 1; i < Main.projectileTexture.Length; i++)
			{
				if (Lang.GetProjectileName(i).Value.ToLower().IndexOf(searchFilter.Text, StringComparison.OrdinalIgnoreCase) != -1)
				{
					var box = new ProjectileSlot(i);
					projectileGrid._items.Add(box);
					projectileGrid._innerList.Append(box);
				}
			}

			projectileGrid.UpdateOrder();
			projectileGrid._innerList.Recalculate();
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			UpdateGrid();
			if (step && stepPrevious)
			{
				step = false;
			}
			stepPrevious = step;
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			if (mainPanel.ContainsPoint(Main.MouseScreen))
			{
				Main.LocalPlayer.mouseInterface = true;
			}
		}
	}

    internal class ProjectileSlot : UIElement
	{
		public static Texture2D backgroundTexture = Main.inventoryBack9Texture;
		private float scale = .6f;
		public int type;
		public ProjectileSlot(int type)
		{
			this.type = type;
			//this.Height.Set(20f, 0f);
			//this.Width.Set(20f, 0f);

			this.Width.Set(backgroundTexture.Width * scale, 0f);
			this.Height.Set(backgroundTexture.Height * scale, 0f);

			Main.instance.LoadProjectile(type);
		}

		internal int frameCounter = 0;
		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			frameCounter++;
			CalculatedStyle dimensions = base.GetInnerDimensions();
			spriteBatch.Draw(backgroundTexture, dimensions.Position(), null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
			Rectangle rectangle = dimensions.ToRectangle();

			int frames = Main.projFrames[type];
			Texture2D texture = Main.projectileTexture[type];
			int height = texture.Height / frames;
			int width = texture.Width;
			int frame = frameCounter % frames;
			int y = height * frame;

			float drawScale = 1f;
			float availableWidth = (float)backgroundTexture.Width * scale;
			if (width > availableWidth || height > availableWidth)
			{
				if (width > height)
				{
					drawScale = availableWidth / width;
				}
				else
				{
					drawScale = availableWidth / height;
				}
			}
			Vector2 drawPosition = dimensions.Position();
			drawPosition.X += backgroundTexture.Width * scale / 2f - (float)width * drawScale / 2f;
			drawPosition.Y += backgroundTexture.Height * scale / 2f - (float)height * drawScale / 2f;


			Main.spriteBatch.Draw(texture, drawPosition, new Rectangle(0, y, width, height), Color.White, 0, Vector2.Zero, drawScale, SpriteEffects.None, 0);
			if (IsMouseHovering)
			{
				Main.hoverItemName = Lang.GetProjectileName(type).Value;
			}
		}
		public override void Click(UIMouseEvent evt)
		{
			Main.NewText("Spawn projectile " + type);
			Projectile p = Main.projectile[Projectile.NewProjectile(
				Main.LocalPlayer.Center + new Vector2(0, -40),
				new Vector2(Main.LocalPlayer.direction * ProjectilesUI.speedXDataProperty.Data, ProjectilesUI.speedYDataProperty.Data),
				type,
				ProjectilesUI.damageDataProperty.Data,
				ProjectilesUI.knockbackDataProperty.Data,
				Main.myPlayer,
				ProjectilesUI.ai0DataProperty.Data,
				ProjectilesUI.ai1DataProperty.Data)];
			if (ProjectilesUI.hostile.Data.HasValue)
			{
				p.hostile = ProjectilesUI.hostile.Data.Value;
			}
			if (ProjectilesUI.friendly.Data.HasValue)
			{
				p.friendly = ProjectilesUI.friendly.Data.Value;
			}
			if (ProjectilesUI.aiStyleDataProperty.Data != -1)
			{
				p.aiStyle = ProjectilesUI.aiStyleDataProperty.Data;
			}
			if (p.modProjectile != null)
			{
				p.modProjectile.drawOffsetX = ProjectilesUI.drawOffsetXDataProperty.Data;
				p.modProjectile.drawOriginOffsetX = ProjectilesUI.drawOriginOffsetXDataProperty.Data;
				p.modProjectile.drawOriginOffsetY = ProjectilesUI.drawOriginOffsetYDataProperty.Data;
			}

			// support for aitype??
			//p.modProjectile = new ModProjectile();
			//p.hostile = ;
			//p.friendly = ;
		}

		public override int CompareTo(object obj)
		{
			return type.CompareTo((obj as ProjectileSlot).type);
		}
	}

    internal class ProjectilesUIGlobalItem : GlobalProjectile
	{
		public override bool PreAI(Projectile projectile)
		{
			if (ProjectilesUI.freeze?.Selected ?? false)
			{
				projectile.velocity = Vector2.Zero;
			}
			if (ProjectilesUI.pause?.Selected ?? false)
			{
				if (ProjectilesUI.step)
				{
					return true;
				}
				return false;
			}
			return !ProjectilesUI.pause?.Selected ?? true;
		}
	}
}
