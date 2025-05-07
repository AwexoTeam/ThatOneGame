using HentaiGame.Structure;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct2D1;
using System;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;

namespace HentaiGame
{
    public class Engine : Game
    {
        private GraphicsDeviceManager graphics;
        public static SpriteBatch batch;

        public const int RESOLUTION_WIDTH = 400;
        public const int RESOLUTION_HEIGHT = 225;

        private bool isResizing;
        private RenderTarget2D renderer;
        public static Rectangle renderRect;
        public static Matrix transform;

        public static float deltaTime;

        public Engine()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = 1028;
            graphics.PreferredBackBufferHeight = 600;

            graphics.ApplyChanges();

            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += Window_ClientSizeChanged;

            base.Initialize();
        }

        private void Window_ClientSizeChanged(object sender, System.EventArgs e)
        {
            if (isResizing)
                return;

            if (Window.ClientBounds.Width <= 0)
                return;

            if (Window.ClientBounds.Height <= 0)
                return;

            isResizing = true;
            UpdateScaleMatrix();
            isResizing = false;
        }

        protected override void LoadContent()
        {
            batch = new SpriteBatch(GraphicsDevice);
            ScreenManager.Instance.LoadContent(Content);
            renderer = new RenderTarget2D(GraphicsDevice, RESOLUTION_WIDTH, RESOLUTION_HEIGHT);
            UpdateScaleMatrix();
        }

        protected override void Update(GameTime gameTime)
        {
            deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            UpdateScaleMatrix(true);

            ScreenManager.Instance.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(renderer);
            GraphicsDevice.Clear(Color.CornflowerBlue);

            batch.Begin(transformMatrix: transform, samplerState: SamplerState.PointWrap);
            ScreenManager.Instance.Draw(batch);
            batch.End();

            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.Black);
            batch.Begin(samplerState: SamplerState.PointWrap);
            batch.Draw(renderer, renderRect, Color.White);
            batch.End();

            base.Draw(gameTime);
        }

        protected void UpdateScaleMatrix(bool playerUpdate = false)
        {
            if (playerUpdate)
                return;

            Point size = GraphicsDevice.Viewport.Bounds.Size;
            
            float scaleX = (float)size.X / renderer.Width;
            float scaleY = (float)size.Y / renderer.Height;
            float scale = MathF.Min(scaleX, scaleY);

            renderRect.Width = (int)(MathF.Floor(renderer.Width * scale));
            renderRect.Height = (int)(MathF.Floor(renderer.Height * scale));

            renderRect.X = (size.X  - renderRect.Width) / 2;
            renderRect.Y = (size.Y  - renderRect.Height) / 2;
        }
    }
}
