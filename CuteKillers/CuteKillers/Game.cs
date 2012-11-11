using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using GameStateManagement;

namespace CuteKillers
{
    public class Game : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        ScreenManager screenManager;
        ScreenFactory screenFactory;

        public int backBufferHeight;
        public int backBufferWidth;

        public Game()
        {
            Content.RootDirectory = "Content";
            graphics = new GraphicsDeviceManager(this);

            TargetElapsedTime = TimeSpan.FromTicks(333333);

            screenFactory = new ScreenFactory();
            Services.AddService(typeof(IScreenFactory), screenFactory);

            screenManager = new ScreenManager(this);
            Components.Add(screenManager);

            AddInitialScreens();
        }

        private void AddInitialScreens()
        {
            screenManager.AddScreen(new BackgroundScreen(), null);

            //screenManager.AddScreen(new MainMenuScreen(), null);
            screenManager.AddScreen(new SplashScreen1(), null);
            //screenManager.AddScreen(new GameplayScreen(), PlayerIndex.One);
        }

        void SplashScreen1Startup(object sender, PlayerIndexEventArgs e)
        {
            
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            //graphics.PreferredBackBufferHeight = backBufferHeight;
            //graphics.PreferredBackBufferWidth = backBufferWidth;
        }

        public static Dictionary<String, T> LoadContent<T>(ContentManager contentmanager, string contentFolder)
        {
            DirectoryInfo dir = new DirectoryInfo(contentmanager.RootDirectory + "\\" + contentFolder);
            if (!dir.Exists)
                throw new DirectoryNotFoundException();

            Dictionary<String, T> result = new Dictionary<String, T>();

            FileInfo[] files = dir.GetFiles("*.tmx");
            foreach (FileInfo file in files)
            {
                string key = Path.GetFileNameWithoutExtension(file.Name);

                result[key] = contentmanager.Load<T>(contentmanager.RootDirectory + "/" + contentFolder + "/" + key);
            }

            return result;
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.Black);
            base.Draw(gameTime);
        }


        #region GetSets
        public GraphicsDeviceManager Graphics
        {
            get { return graphics; }
            set { graphics = value; }
        }

        public int BackBufferHeight
        {
            get { return backBufferHeight; }
            set { backBufferHeight = value; }
        }

        public int BackBufferWidth
        {
            get { return backBufferWidth; }
            set { backBufferWidth = value; }
        }
        #endregion
    }
}
