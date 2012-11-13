using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using GameStateManagement;
using System.IO;
using EasyConfig;

namespace CuteKillers
{
    class SplashScreenLoading : MainMenuScreen
    {
        bool loadingIsSlow;
        bool otherScreensAreGone;

        Vector2 contentTextPos;

        ContentManager content;
        SpriteFont gameFont;

        MainMenuScreen[] screensToLoad;

        private SplashScreenLoading(ScreenManager screenManager, bool loadingIsSlow,
                                MainMenuScreen[] screensToLoad)
        {
            this.loadingIsSlow = loadingIsSlow;
            this.screensToLoad = screensToLoad;

            TransitionOnTime = TimeSpan.FromSeconds(0.5);
        }

        public static void Load(ScreenManager screenManager, bool loadingIsSlow,
                                PlayerIndex? controllingPlayer,
                                params MainMenuScreen[] screensToLoad)
        {
            foreach (GameScreen screen in screenManager.GetScreens())
                screen.ExitScreen();

            SplashScreenLoading splashScreenLoading = new SplashScreenLoading(screenManager, loadingIsSlow, screensToLoad);

            screenManager.AddScreen(splashScreenLoading, controllingPlayer);
        }

        public override void Activate(bool instancePreserved)
        {
            if (!instancePreserved)
            {
                if (content == null)
                    content = new ContentManager(ScreenManager.Game.Services, "Content");

                contentTextPos.X = 10;
                contentTextPos.Y = 20;

                gameFont = content.Load<SpriteFont>("loadingFont");
            }
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            //DirectoryInfo dir = new DirectoryInfo(content.RootDirectory + "\\Characters");
            //FileInfo[] filePaths = dir.GetFiles("*.ini");

            //foreach (FileInfo file in filePaths)
            //{
                //loadedContent(System.IO.Path.GetFileName(file.Name));
            //}

            if (otherScreensAreGone)
            {
                ScreenManager.RemoveScreen(this);

                foreach (GameScreen screen in screensToLoad)
                {
                    if (screen != null)
                    {
                        ScreenManager.AddScreen(new BackgroundScreen(), null); 
                        ScreenManager.AddScreen(screen, ControllingPlayer);
                    }
                }

                ScreenManager.Game.ResetElapsedTime();


            }
        }

        public override void Draw(GameTime gameTime)
        {
            if ((ScreenState == ScreenState.Active) && (ScreenManager.GetScreens().Length == 1))
            {
                otherScreensAreGone = true;
            }

            if (loadingIsSlow)
            {
                SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
                SpriteFont font = ScreenManager.Font;

                const string message = "Splash Screen 1, Loading...";

                Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
                Vector2 viewportSize = new Vector2(viewport.Width, viewport.Height);
                Vector2 textSize = font.MeasureString(message);
                Vector2 textPosition = (viewportSize - textSize) / 2;

                Color color = Color.White * TransitionAlpha;

                // Draw the text.
                spriteBatch.Begin();
                spriteBatch.DrawString(font, message, textPosition, color);
                spriteBatch.End();
            }
        }

        public void loadedContent(string cname)
        {
            string contentName = cname;

            SpriteBatch spriteBatchContent = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.Font;

            spriteBatchContent.Begin();
            spriteBatchContent.DrawString(gameFont, contentName, contentTextPos, Color.White);
            spriteBatchContent.End();

            contentTextPos.Y ++;
        }
    }
}
