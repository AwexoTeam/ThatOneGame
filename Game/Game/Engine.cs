using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using ThatOneGame.Structure;
using ThatOneGame.GameCode;
using System.Collections.Generic;
using System.Windows.Forms;
using SpriteFontPlus;
using System.IO;

namespace ThatOneGame
{
    public class Engine : Game
    {
        private GraphicsDeviceManager graphics;
        public static SpriteBatch batch;

        private bool isResizing;
        private RenderTarget2D renderer;
        public static Rectangle renderRect;
        public static Matrix transform;
        
        public static float deltaTime;

        public static Manager[] managers;
        public static GameWindow window;

        public Engine()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Engine.window = Window;
            managers = Utils.GetAllTypes<Manager>();
        }

        protected override void Initialize()
        {
            int preferedWidth = 1028;
            int preferedHeight = 600;

            graphics.PreferredBackBufferWidth = preferedWidth;
            graphics.PreferredBackBufferHeight = preferedHeight;
            Globals.screenSize = new Vector2(preferedWidth, preferedHeight);
            graphics.ApplyChanges();

            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += Window_ClientSizeChanged;

            base.Initialize();
            Array.ForEach(managers, x => x.Initialize());
            Globals.random = new Random();
        }

        private void Window_ClientSizeChanged(object sender, System.EventArgs e)
        {
            if (isResizing)
                return;

            if (Window.ClientBounds.Width <= 0)
                return;

            if (Window.ClientBounds.Height <= 0)
                return;

            Globals.screenSize = new Vector2(Window.ClientBounds.Width, Window.ClientBounds.Height);

            isResizing = true;
            UpdateScaleMatrix();
            isResizing = false;
            
            EventManager.Invoke(EventManagerTypes.WindowSizeChanged, e);
        }

        protected override void LoadContent()
        {
            batch = new SpriteBatch(GraphicsDevice);
            
            renderer = new RenderTarget2D(GraphicsDevice, (int)Globals.preferedResolution.X, (int)Globals.preferedResolution.Y);

            string fontData;

            using (var stream = TitleContainer.OpenStream("Fonts/test.fnt"))
            {
                using (var reader = new StreamReader(stream))
                {
                    fontData = reader.ReadToEnd();
                }
            }

            Globals.font = BMFontLoader.Load(fontData, name => TitleContainer.OpenStream("Fonts/" + name), Engine.batch.GraphicsDevice);


            UpdateScaleMatrix();

        }

        protected override void Update(GameTime gameTime)
        {
            Input.StartUpdate();
            deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            UpdateScaleMatrix(true);

            Array.ForEach(managers, x => x.Update(gameTime));
            
            base.Update(gameTime);
            Input.EndUpdate();
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(renderer);
            GraphicsDevice.Clear(Color.CornflowerBlue);


            batch.Begin(transformMatrix: transform, samplerState: SamplerState.PointClamp);
            Array.ForEach(managers, x => x.Draw(batch));
            batch.End();

            batch.Begin(samplerState: SamplerState.PointClamp);
            Array.ForEach(managers, x => x.PostDraw(batch));
            batch.End();

            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.Black);
            batch.Begin(samplerState: SamplerState.PointClamp);
            batch.Draw(renderer, renderRect, Color.White);
            Array.ForEach(managers, x => x.DrawUI(batch));
            batch.End();

            base.Draw(gameTime);
        }

        protected void UpdateScaleMatrix(bool playerUpdate = false)
        {
            if (playerUpdate)
                return;

            Point size = GraphicsDevice.Viewport.Bounds.Size;
            
            float _scaleX = (float)size.X / renderer.Width;
            float _scaleY = (float)size.Y / renderer.Height;
            Globals.scale = MathF.Min(_scaleX, _scaleY);

            renderRect.Width = (int)(MathF.Floor(renderer.Width * Globals.scale));
            renderRect.Height = (int)(MathF.Floor(renderer.Height * Globals.scale));

            renderRect.X = (size.X  - renderRect.Width) / 2;
            renderRect.Y = (size.Y  - renderRect.Height) / 2;
        }
    }
}
