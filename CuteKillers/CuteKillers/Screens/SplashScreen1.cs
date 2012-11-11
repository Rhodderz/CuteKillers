using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameStateManagement;
using System.Threading;

namespace CuteKillers
{
    class SplashScreen1 : GameScreen
    {
        ContentManager content;
        SpriteFont gameFont;

        Vector2 enemyPosition = new Vector2(100, 100);
        Vector2 otherPosition = new Vector2(100, 120);

        MainMenuScreen mainMenuScreen;

        float tempTime = 0;
        int tempTimeInt = 0;
        bool timeMet = false;

        public SplashScreen1()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        public override void Activate(bool instancePreserved)
        {
            if (!instancePreserved)
            {

                if (content == null)
                    content = new ContentManager(ScreenManager.Game.Services, "Content");

                gameFont = content.Load<SpriteFont>("gamefont");

                Thread.Sleep(1000);

                ScreenManager.Game.ResetElapsedTime();
            }
        }

        public override void Deactivate()
        {
            base.Deactivate();
        }

        public override void Unload()
        {
            content.Unload();
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            tempTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (tempTime >= 3)
                timeMet = true;

               if (IsActive)
                {
                    // enemyPosition.X += (float)(random.NextDouble() - 0.5) * randomization;
                    // enemyPosition.Y += (float)(random.NextDouble() - 0.5) * randomization;
                }

               tempTimeInt = Convert.ToInt32(tempTime);

               if (timeMet == true)
               {
                   SplashScreenLoading.Load(ScreenManager, true, null,
                                  new MainMenuScreen());
               }
                
               base.Update(gameTime, otherScreenHasFocus, false);
            }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target,
                                               Color.BlueViolet, 0, 0);

            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            //string timer = tempTimeInt.ToString();
            //string timerMet = timeMet.ToString();
            string tempMessage = "A splash screen will go here";

            spriteBatch.Begin();
            spriteBatch.DrawString(gameFont, tempMessage, otherPosition, Color.Green);
            //spriteBatch.DrawString(gameFont, timer, enemyPosition, Color.Gold);
            //spriteBatch.DrawString(gameFont, timerMet, otherPosition, Color.Gold);
            spriteBatch.End();
        }
    }
}
