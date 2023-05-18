using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Menus;

namespace DailyPlanner.Framework
{

    class SliderComponent : OptionsSlider
    {
        private readonly int MinValue;
        private readonly int MaxValue;
        private readonly List<string> OptionsList;
        private readonly string LabelPredicate;
        private readonly PlannerMenu Menu;

        /// <summary>
        /// An options slider that returns a number from a min value to a max value.
        /// </summary>
        /// <param name="minValue">Minimum value for the slider</param>
        /// <param name="maxValue">Maximum value for the slider</param>
        /// <param name="labelPredicate">Label to apply to the slider</param>
        /// <param name="menu">The menu that this slider is applied to</param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public SliderComponent(int minValue, int maxValue, string labelPredicate, PlannerMenu menu, int x = -1, int y = -1) 
            : base("", 0, x, y)
        {
            this.MinValue = minValue;
            this.MaxValue = maxValue;
            this.LabelPredicate = labelPredicate;
            this.Menu = menu;
        }

        /// <summary>
        /// An options slider that returns a string from a list of strings.
        /// </summary>
        /// <param name="optionsList">List of options the slider may return.</param>
        /// <param name="labelPredicate"></param>
        /// <param name="menu">Label to apply to the slider</param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public SliderComponent(List<string> optionsList, string labelPredicate, PlannerMenu menu, int x = -1, int y = -1)
            : base("", 0, x, y)
        {
            this.OptionsList = optionsList;
            this.MinValue = 0;
            this.MaxValue = this.OptionsList.Count - 1;
            this.LabelPredicate = labelPredicate;
            this.Menu = menu;
        }

        /// <summary>
        /// Called when the player is done adjusting the slider.
        /// </summary>
        /// <param name="x">X pos of click</param>
        /// <param name="y">Y pos of click</param>
        public override void leftClickReleased(int x, int y)
        {
            this.Menu.RefreshRemoveTaskTab();
            base.leftClickReleased(x, y);
        }

        /// <summary>
        /// Returns the output of the slider in int form. If constructed with a string list, returns the index of the output.
        /// </summary>
        /// <returns></returns>
        public int GetOutputInt()
        {
            int range = this.MaxValue - this.MinValue;
            double result = (double)this.value * range / 100;
            result += this.MinValue;
            return (int)Math.Round(result);
        }

        /// <summary>
        /// Returns the output of the slider in string form. If constructed with a min and max int, converts the output int to a string.
        /// </summary>
        /// <returns></returns>
        public string GetOutputString()
        {
            int index = GetOutputInt() - this.MinValue;
            if (this.OptionsList is not null && index < this.OptionsList.Count) return this.OptionsList[index];
            return GetOutputInt().ToString();
        }

        /// <summary>
        /// Draw the slider.
        /// </summary>
        /// <param name="b">Spritebatch used to draw the slider</param>
        /// <param name="slotX">X position of the slot</param>
        /// <param name="slotY">Y position of the slot</param>
        /// <param name="context"></param>
        public override void draw(SpriteBatch b, int slotX, int slotY, IClickableMenu context = null)
        {
            //if (this.OptionsList.Count > 0 && this.value < this.OptionsList.Count) this.label = this.OptionsList[this.value];
            this.label = $"{this.LabelPredicate}: {GetOutputString()}";
            base.draw(b, slotX, slotY, context);
        }
    }
}
