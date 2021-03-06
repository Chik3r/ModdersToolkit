﻿using Terraria.UI;


namespace ModdersToolkit.UIElements
{
	internal class UIRadioButtonGroup : UIElement
	{
		private int idCount = 0;
		public UIRadioButtonGroup() {
			this.Height.Set(20f, 0f);
			this.Width.Set(0f, 1f);
		}

		public virtual void Add(UIRadioButton radioButton) {
			radioButton.partOfGroup = true;
			radioButton.groupID = idCount;
			radioButton.Top.Set(20f * idCount, 0f);
			idCount++;
			Append(radioButton);
			Height.Set(20f * idCount, 0f);
			Recalculate();

		}

		internal void ButtonClicked(int id) {
			for (int i = 0; i < idCount; i++) {
				(Elements[i] as UIRadioButton).Selected = false;
			}
			(Elements[id] as UIRadioButton).Selected = true;
		}
	}
}
