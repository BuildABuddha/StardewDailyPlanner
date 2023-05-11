using StardewModdingAPI;
using DailyPlanner.Framework.Constants;

namespace DailyPlanner.Framework
{
    class ModConfig
    {
        /*********
        ** Accessors
        *********/

        /****
        ** Keyboard buttons
        ****/
        /// <summary>The keyboard button which opens the menu.</summary>
        public SButton OpenMenuKey { get; set; } = SButton.OemOpenBrackets;

        /****
        ** Menu settings
        ****/
        /// <summary>The tab shown by default when you open the menu.</summary>
        public MenuTab DefaultTab { get; set; } = MenuTab.Daily;
        public bool ShowOverlay { get; set; } = true;
        public float OverlayBackgroundOpacity { get; set; } = 0.7F;
        public float OverlayTextOpacity { get; set; } = 1.0F;
        public int OverlayMaxLength { get; set; } = 25;
        public int OverlayMaxLines { get; set; } = 10;
    }
}
