﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics.CodeAnalysis;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Menus;

namespace DailyPlanner.Framework
{
    internal class TextBoxComponent : OptionsElement
    {
        /*********
        ** Fields
        *********/

        /// <summary>The original menu, so it can be refreshed.</summary>
        private readonly PlannerMenu PlannerMenu;

        private readonly TextBox InputBox;
        private Rectangle InputBoxBounds;

        /// <summary>Whether the user explicitly selected the textbox by clicking on it, so the selection should be maintained.</summary>
        private bool IsInputBoxSelectedExplicitly;

        /// <summary>The source rectangle for the 'set' button sprite.</summary>
        private readonly Rectangle SetButtonSprite = new(294, 428, 21, 11);

        /// <summary>Area on the screen that the button occupies. Used to check if we click on it.</summary>
        private Rectangle SetButtonBounds;

        private readonly string ButtonType;

        /*********
        ** Public methods
        *********/
        /// <summary>Construct a button with Planner helper.</summary>
        /// <param name="buttonType">The type of button.</param>
        /// <param name="slotWidth">The field width.</param>
        /// <param name="plannermenu">The PlannerMenu creating this button.</param>
        public TextBoxComponent(string buttonType, int slotWidth, PlannerMenu plannermenu)
          : base("", -1, -1, slotWidth + 1, 11 * Game1.pixelZoom)
        {
            this.PlannerMenu = plannermenu;
            this.InputBox = new(Game1.content.Load<Texture2D>("LooseSprites\\textBox"), null, Game1.smallFont, Color.Black);
            this.InputBox.TitleText = label;
            this.InputBox.Text = label;
            this.SetButtonBounds = new Rectangle(slotWidth - 28 * Game1.pixelZoom, -1 + Game1.pixelZoom * 3, 21 * Game1.pixelZoom, 11 * Game1.pixelZoom);
            this.ButtonType = buttonType;
        }

        /// <summary>Set the search textbox selected.</summary>
        /// <param name="explicitly">Whether the textbox was selected explicitly by the user (rather than automatically by hovering), so the selection should be maintained.</param>
        private void SelectInputBox(bool explicitly)
        {
            this.InputBox.Selected = true;
            this.IsInputBoxSelectedExplicitly = explicitly;
            this.InputBox.Width = this.InputBoxBounds.Width;
            this.PlannerMenu.HasSelectedTextbox = true;
        }

        /// <summary>Set the search textbox non-selected.</summary>
        private void DeselectInputBox()
        {
            Game1.closeTextEntry();

            this.InputBox.Selected = false;
            this.IsInputBoxSelectedExplicitly = false;
            this.PlannerMenu.HasSelectedTextbox = false;
        }

        /// <summary>Called when player left clicks on the menu.</summary>
        /// <param name="x">X coordinate of the click.</param>
        /// <param name="y">Y coordinate of the click.</param>
        public override void receiveLeftClick(int x, int y)
        {
            this.InputBoxBounds = new(0, 4, this.InputBox.Width, 24 * Game1.pixelZoom);

            // Click on text box
            if (this.InputBoxBounds.Contains(x, y))
            {
                if (!this.InputBox.Selected || !this.IsInputBoxSelectedExplicitly)
                    this.SelectInputBox(explicitly: true);
            }
            // Click on button
            else if (this.SetButtonBounds.Contains(x, y))
            {
                Game1.soundBank.PlayCue("achievement");
                this.DeselectInputBox();
                this.PlannerMenu.OnAddTaskButtonPressed(this.ButtonType, this.InputBox.Text);
                this.InputBox.Text = "";
            }
            else
            {
                if (this.InputBox.Selected)
                    this.DeselectInputBox();
            }
            return;
        }

        public override void receiveKeyPress(Keys key)
        {
            if (key == Keys.Escape || key == Keys.Enter) this.DeselectInputBox();
        }

        public override void draw(SpriteBatch spriteBatch, int slotX, int slotY, IClickableMenu context = null)
        {
            this.InputBox.X = this.bounds.X + slotX;
            this.InputBox.Y = this.bounds.Y + slotY;
            this.InputBox.Width = 700;
            this.InputBox.Draw(spriteBatch);
            Utility.drawWithShadow(
                spriteBatch,
                Game1.mouseCursors,
                new Vector2(this.SetButtonBounds.X + slotX, this.SetButtonBounds.Y + slotY),
                this.SetButtonSprite,
                Color.White,
                0.0f,
                Vector2.Zero,
                Game1.pixelZoom,
                false,
                0.15f
            );
            return;
        }
    }
}
