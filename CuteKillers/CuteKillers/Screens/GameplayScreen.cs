#region File Description
//-----------------------------------------------------------------------------
// GameplayScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Griddy2D;
using System.IO;
using GameStateManagement;
using System.Threading;
using EasyConfig;
#endregion

namespace CuteKillers
{
    /// <summary>
    /// This screen implements the actual game logic. It is just a
    /// placeholder to get the idea across: you'll probably want to
    /// put some more interesting gameplay in here!
    /// </summary>
    class GameplayScreen : GameScreen
    {
        #region Fields

        ContentManager content;
        SpriteFont gameFont;

        public Game game;
        SpriteBatch spriteBatch;

        Vector2 playerPosition = new Vector2(100, 100);
        Vector2 enemyPosition = new Vector2(100, 100);

        Random random = new Random();

        Grid level;

        float pauseAlpha;

        InputAction pauseAction;


        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public GameplayScreen(Game game)
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            pauseAction = new InputAction(
                new Buttons[] { Buttons.Start, Buttons.Back },
                new Keys[] { Keys.Escape },
                true);

            this.game = game;
        }


        /// <summary>
        /// Load graphics content for the game.
        /// </summary>
        public override void Activate(bool instancePreserved)
        {
            if (!instancePreserved)
            {
                //graphics = new GraphicsDeviceManager(ScreenManager.Game);

                spriteBatch = ScreenManager.SpriteBatch;

                //graphics = new 

                //game.GraphicsDevice.Viewport.
                LoadGriddy();

                if (content == null)
                    content = new ContentManager(ScreenManager.Game.Services, "Content");

                gameFont = content.Load<SpriteFont>("gamefont");

                // A real game would probably have more content than this sample, so
                // it would take longer to load. We simulate that by delaying for a
                // while, giving you a chance to admire the beautiful loading screen.
                Thread.Sleep(1000);

                // once the load has finished, we use ResetElapsedTime to tell the game's
                // timing mechanism that we have just finished a very long frame, and that
                // it should not try to catch up.
                ScreenManager.Game.ResetElapsedTime();
            }

        }

        private void LoadGriddy()
        {
            Stream gridDataStream = new FileStream("Content/Maps/demoLevel.tmx", FileMode.Open, FileAccess.Read);
            Stream tileBankStream = new FileStream("Content/Maps/tileBank.xml", FileMode.Open, FileAccess.Read);

            GridData gridData = GridData.NewFromStreamAndWorldPosition(gridDataStream, new Vector2(1, 0));
            TileBank tileBank = TileBank.CreateFromSerializedData(tileBankStream, ScreenManager.Game.Content);

            gridDataStream.Position = 0;
            SerializedGridFactory gridFactory = SerializedGridFactory.NewFromData(gridDataStream, gridData, tileBank);

            level = Grid.NewGrid(gridData, gridFactory, DefaultGridDrawer.NewFromGridData(gridData, ScreenManager.Game.Content, Color.Black));

            /*
            GraphicsDeviceManager.PreferredBackBufferWidth = gridData.BoundingBox.Width;
            graphics.PreferredBackBufferHeight = gridData.BoundingBox.Height;
            graphics.ApplyChanges();
            */
        }


        public override void Deactivate()
        {
            base.Deactivate();
        }


        /// <summary>
        /// Unload graphics content used by the game.
        /// </summary>
        public override void Unload()
        {
            content.Unload();
        }


        #endregion

        #region Update and Draw


        /// <summary>
        /// Updates the state of the game. This method checks the GameScreen.IsActive
        /// property, so the game will stop updating when the pause menu is active,
        /// or if you tab away to a different application.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            // Gradually fade in or out depending on whether we are covered by the pause screen.
            if (coveredByOtherScreen)
                pauseAlpha = Math.Min(pauseAlpha + 1f / 32, 1);
            else
                pauseAlpha = Math.Max(pauseAlpha - 1f / 32, 0);

            if (IsActive)
            {
                //dirt = level.GetLayer("terrain").GetAllMatchingTiles(tile => tile.Name == "dirt")
                if (Keyboard.GetState().IsKeyDown(Keys.A) == true)
                {
                    level.GetLayer("terrain").GetAllMatchingTiles(tile => tile.Name == "dirt");
                }

                //Gameplay goes here.
            }
        }


        /// <summary>
        /// Lets the game respond to player input. Unlike the Update method,
        /// this will only be called when the gameplay screen is active.
        /// </summary>
        public override void HandleInput(GameTime gameTime, InputState input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            // Look up inputs for the active player profile.
            int playerIndex = (int)ControllingPlayer.Value;

            KeyboardState keyboardState = input.CurrentKeyboardStates[playerIndex];
            GamePadState gamePadState = input.CurrentGamePadStates[playerIndex];

            // The game pauses either if the user presses the pause button, or if
            // they unplug the active gamepad. This requires us to keep track of
            // whether a gamepad was ever plugged in, because we don't want to pause
            // on PC if they are playing with a keyboard and have no gamepad at all!
            bool gamePadDisconnected = !gamePadState.IsConnected &&
                                       input.GamePadWasConnected[playerIndex];

            PlayerIndex player;
            if (pauseAction.Evaluate(input, ControllingPlayer, out player) || gamePadDisconnected)
            {
                ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
            }
            else
            {
                // Otherwise move the player position.
                Vector2 movement = Vector2.Zero;

                if (keyboardState.IsKeyDown(Keys.Left))
                    movement.X--;

                if (keyboardState.IsKeyDown(Keys.Right))
                    movement.X++;

                if (keyboardState.IsKeyDown(Keys.Up))
                    movement.Y--;

                if (keyboardState.IsKeyDown(Keys.Down))
                    movement.Y++;

                Vector2 thumbstick = gamePadState.ThumbSticks.Left;

                movement.X += thumbstick.X;
                movement.Y -= thumbstick.Y;

                if (movement.Length() > 1)
                    movement.Normalize();

                playerPosition += movement * 8f;
            }
        }


        /// <summary>
        /// Draws the gameplay screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            // This game has a blue background. Why? Because!
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target,
                                               Color.CornflowerBlue, 0, 0);

            Vector2 tempPlacement;
            tempPlacement.X = 10;
            tempPlacement.Y = 10;

            Vector2 tempPlacement2;
            tempPlacement2.X = 100;
            tempPlacement2.Y = 40;

            DirectoryInfo dir = new DirectoryInfo(content.RootDirectory + "\\Characters");
            FileInfo[] filePaths = dir.GetFiles("*.ini");
            spriteBatch.Begin();

            foreach (FileInfo file in filePaths)
            {
                string key = System.IO.Path.GetFileNameWithoutExtension(file.Name) + ".ini";
                string key2 = System.IO.Path.GetFileName(file.Name);

                ConfigFile configFile = new ConfigFile("Content\\Characters\\" + System.IO.Path.GetFileName(file.Name));

                foreach (KeyValuePair<string, SettingsGroup> group in configFile.SettingGroups)
                {
                    spriteBatch.DrawString(gameFont, group.Key + ":", tempPlacement, Color.Black);

                    foreach (KeyValuePair<string, Setting> value in group.Value.Settings)
                    {
                        spriteBatch.DrawString(gameFont, value.Key + " = " + value.Value.RawValue, tempPlacement2, Color.Black);
                        tempPlacement2.Y = tempPlacement2.Y + 40;
                    }

                    tempPlacement.Y = tempPlacement.Y + 40;
                }

                tempPlacement.Y = tempPlacement.Y + 60;
                tempPlacement2.Y = tempPlacement2.Y + 60;
                //spriteBatch.DrawString(gameFont, key, tempPlacement, Color.Black);
            }

            //DrawGameObjects(spriteBatch);

            spriteBatch.End();

            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0 || pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, pauseAlpha / 2);

                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }

        private void DrawGameObjects(SpriteBatch spriteBatch)
        {
            level.Draw(spriteBatch);
        }

        #endregion
    }
}
