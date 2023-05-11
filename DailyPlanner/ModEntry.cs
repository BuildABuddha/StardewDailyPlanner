using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using DailyPlanner.Framework;
using StardewModdingAPI.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using GenericModConfigMenu;

namespace DailyPlanner
{
    /// <summary>The mod entry point.</summary>
    public class ModEntry : Mod
    {
        /*********
        ** Fields
        *********/

        /// <summary>The mod settings.</summary>
        private ModConfig Config;

        public Planner Planner;

        private PlannerOverlay Overlay;

        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            this.Config = helper.ReadConfig<ModConfig>();

            helper.Events.Input.ButtonPressed += this.OnButtonPressed;
            helper.Events.GameLoop.DayStarted += this.OnDayStarted;
            helper.Events.GameLoop.SaveLoaded += this.OnSaveLoaded;
            helper.Events.Display.RenderingHud += this.OnRenderingHud;
            helper.Events.GameLoop.GameLaunched += this.OnGameLaunched;
        }


        /*********
        ** Private methods
        *********/
        /// <summary>Raised after the player presses a button on the keyboard, controller, or mouse.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        private void OnButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            // ignore if player hasn't loaded a save yet
            if (!Context.IsPlayerFree)
                return;

            // Open window if button is tab button
            if ((e.Button == this.Config.OpenMenuKey) & (Context.IsWorldReady))
            {
                Game1.activeClickableMenu = new PlannerMenu(this.Config.DefaultTab, this.Config, this.Planner, this.Helper.Translation, this.Monitor);
                Game1.soundBank.PlayCue("bigSelect");
            }
                
        }

        private void OnSaveLoaded(object sender, SaveLoadedEventArgs e)
        {
            this.Overlay = new PlannerOverlay(this, this.Config);
        }

            private void OnDayStarted(object sender, DayStartedEventArgs e)
        {
            this.Planner = new Planner(Game1.year, this.Helper.DirectoryPath, this.Helper.Translation, this.Monitor);
            this.Planner.CreateDailyPlan();
        }

        private void OnRenderingHud(object sender, RenderingHudEventArgs e)
        {
            if (this.Config.ShowOverlay) this.Overlay?.Draw(e.SpriteBatch);
        }

        // TODO: Replace options menu settings with translations 
        private void OnGameLaunched(object sender, GameLaunchedEventArgs e)
        {
            // get Generic Mod Config Menu's API (if it's installed)
            var configMenu = this.Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
            if (configMenu is null) return;

            // register mod
            configMenu.Register(
                mod: this.ModManifest,
                reset: () => this.Config = new ModConfig(),
                save: () => this.Helper.WriteConfig(this.Config));

            // add keybind
            configMenu.AddKeybind(
                mod: ModManifest,
                name: () => "Open menu key",
                tooltip: () => "",
                getValue: () => this.Config.OpenMenuKey,
                setValue: (SButton val) => this.Config.OpenMenuKey = val);

            configMenu.AddBoolOption(
                mod: ModManifest,
                name: () => "Show Overlay",
                tooltip: () => "",
                getValue: () => this.Config.ShowOverlay,
                setValue: (bool val) => this.Config.ShowOverlay = val);

            configMenu.AddNumberOption(
                mod: ModManifest,
                name: () => "Overlay background opacity",
                tooltip: () => "",
                getValue: () => this.Config.OverlayBackgroundOpacity,
                setValue: (float val) => this.Config.OverlayBackgroundOpacity = val,
                min: 0.0F,
                max: 1.0F,
                interval: 0.1F);

            configMenu.AddNumberOption(
                mod: ModManifest,
                name: () => "Overlay text opacity",
                tooltip: () => "",
                getValue: () => this.Config.OverlayTextOpacity,
                setValue: (float val) => this.Config.OverlayTextOpacity = val,
                min: 0.0F,
                max: 1.0F,
                interval: 0.1F);

            configMenu.AddNumberOption(
                mod: ModManifest,
                name: () => "Overlay max lines",
                tooltip: () => "",
                getValue: () => this.Config.OverlayMaxLines,
                setValue: (int val) => this.Config.OverlayMaxLines = val,
                min: 1,
                max: 25,
                interval: 1);

            configMenu.AddNumberOption(
                mod: ModManifest,
                name: () => "Overlay max line length",
                tooltip: () => "",
                getValue: () => this.Config.OverlayMaxLength,
                setValue: (int val) => this.Config.OverlayMaxLength = val,
                min: 10,
                max: 40,
                interval: 1);
        }
    }
}
