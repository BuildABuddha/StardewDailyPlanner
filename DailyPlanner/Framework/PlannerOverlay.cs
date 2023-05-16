using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

using StardewValley;
using StardewValley.Locations;
using StardewModdingAPI;
using StardewValley.Menus;
using StardewValley.Objects;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DailyPlanner.Framework
{
    internal class PlannerOverlay
    {
        public int offsetX = 0;
        public int offsetY = 0;
        private const int marginTop = 5;
        private const int marginLeft = 5;
        private const int marginRight = 25;
        private const int marginBottom = 5;

        private readonly ModEntry ModEntry;

        private readonly ModConfig Config;

        public PlannerOverlay(ModEntry modEntry, ModConfig config)
        {
            this.ModEntry = modEntry;
            this.Config = config;
        }

        private static string ConcatLine(string text, int ListMaxHeaderLength)
        {
            int length = text.Length;
            while (Game1.smallFont.MeasureString($"{text[..length]}...").X > ListMaxHeaderLength)
            {
                length--;
            }
            return $"{text[..length]}...";
        }

        private string GetFormattedList(List<string> todaysPlanList)
        {
            StringBuilder stringBuilder = new();
            int numLines = 0;
            int ListMaxHeaderLength = (int)Game1.smallFont.MeasureString(new string('_', this.Config.OverlayMaxLength)).X;
            foreach (string str in todaysPlanList)
            {
                if (Game1.smallFont.MeasureString($"• {str}").X < ListMaxHeaderLength) stringBuilder.AppendLine($"• {str}");
                else stringBuilder.AppendLine(ConcatLine($"• {str}", ListMaxHeaderLength));
                numLines++;
                if (numLines == this.Config.OverlayMaxLines) break;
            }
            return  stringBuilder.ToString();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            List<string> todaysPlanList = this.ModEntry.Planner.GetDailyPlan();

            if (todaysPlanList.Count > 0)
            {
                string todaysPlan = GetFormattedList(todaysPlanList);
                Vector2 ListHeaderSize = Game1.smallFont.MeasureString(todaysPlan);
                offsetX = Game1.graphics.GraphicsDevice.Viewport.TitleSafeArea.Left;
                offsetY = Game1.graphics.GraphicsDevice.Viewport.TitleSafeArea.Top;
                offsetX = (int) Math.Floor((double)Game1.graphics.GraphicsDevice.Viewport.TitleSafeArea.Right * this.Config.OverlayXBufferPercent / 100);
                offsetY = (int) Math.Floor((double)Game1.graphics.GraphicsDevice.Viewport.TitleSafeArea.Bottom * this.Config.OverlayYBufferPercent / 100);

                Rectangle effectiveBounds = new(offsetX, offsetY, (int)(ListHeaderSize.X + marginLeft + marginRight), (int)(marginTop + (ListHeaderSize.Y)) + marginBottom);

                if (Game1.CurrentMineLevel > 0
               || Game1.currentLocation is VolcanoDungeon vd && vd.level.Value > 0
               || Game1.currentLocation is Club)
                {
                    int yAdjust = Game1.uiMode ? (int)MathF.Ceiling(80f * Game1.options.zoomLevel / Game1.options.uiScale) : 80;
                    effectiveBounds.Y = Math.Max(effectiveBounds.Y, yAdjust);
                }

                float topPx = effectiveBounds.Y + marginTop;
                float leftPx = effectiveBounds.X + marginLeft;
                if(this.Config.OverlayBackgroundOpacity > 0) IClickableMenu.drawTextureBox(spriteBatch, Game1.menuTexture, new Rectangle(0, 256, 60, 60), effectiveBounds.X, effectiveBounds.Y, effectiveBounds.Width, effectiveBounds.Height, Color.White * this.Config.OverlayBackgroundOpacity);

                spriteBatch.DrawString(Game1.smallFont, $"{todaysPlan}", new Vector2(leftPx+8, topPx+8), Color.Black * this.Config.OverlayTextOpacity);
            }
        }
    }
}
