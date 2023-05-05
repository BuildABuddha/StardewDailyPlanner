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

        public SliderComponent(int minValue, int maxValue, string labelPredicate, PlannerMenu menu, int x = -1, int y = -1) 
            : base("", 0, x, y)
        {
            this.MinValue = minValue;
            this.MaxValue = maxValue;
            this.LabelPredicate = labelPredicate;
            this.Menu = menu;
        }
        public SliderComponent(List<string> optionsList, string labelPredicate, PlannerMenu menu, int x = -1, int y = -1)
            : base("", 0, x, y)
        {
            this.OptionsList = optionsList;
            this.MinValue = 0;
            this.MaxValue = this.OptionsList.Count - 1;
            this.LabelPredicate = labelPredicate;
            this.Menu = menu;
        }

        public override void leftClickReleased(int x, int y)
        {
            this.Menu.RefreshRemoveTaskTab();
            base.leftClickReleased(x, y);
        }

        public int GetOutputInt()
        {
            int range = this.MaxValue - this.MinValue;
            double result = (double)this.value * range / 100;
            result += this.MinValue;
            return (int)Math.Round(result);
        }

        public string GetOutputString()
        {
            int index = GetOutputInt() - this.MinValue;
            if (!(this.OptionsList is null) && index < this.OptionsList.Count) return this.OptionsList[index];
            return GetOutputInt().ToString();
        }

        public override void draw(SpriteBatch b, int slotX, int slotY, IClickableMenu context = null)
        {
            //if (this.OptionsList.Count > 0 && this.value < this.OptionsList.Count) this.label = this.OptionsList[this.value];
            this.label = $"{this.LabelPredicate}: {GetOutputString()}";
            base.draw(b, slotX, slotY, context);
        }
    }
}
