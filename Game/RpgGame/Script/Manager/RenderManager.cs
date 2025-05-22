using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RpgGame.Managers;
using RpgGame.Structure;
using System;
using System.Collections.Generic;

namespace RpgGame.Managers
{
    public class RenderManager : IHookable
    {
        public int priority => 9000;

        public static List<Action<SpriteBatch>> mainDrawCalls;
        public static List<Action<SpriteBatch>> postDrawCalls;
        public static List<Action<SpriteBatch>> uiDrawCalls;

        private GraphicsDeviceManager graphicsDeviceManager;
        private GraphicsDevice graphics;
        private RenderTarget2D renderer;
        public static Rectangle renderRect;
        public static Matrix transform;

        public readonly SamplerState samplerState = SamplerState.PointClamp;

        public bool CanHook(IHookable[] hooks, out string error)
        {
            error = "none";
            return true;
        }
        public void Run()
        {
            mainDrawCalls = new List<Action<SpriteBatch>>();
            postDrawCalls = new List<Action<SpriteBatch>>();
            uiDrawCalls = new List<Action<SpriteBatch>>();

            EventManager.OnEngineInitizile += OnEngineStart;
            EventManager.OnEngineContentLoad += ContentLoad;
        }

        private void ContentLoad()
        {
            int preferedWidth = 1028;
            int preferedHeight = 100;

            graphicsDeviceManager.PreferredBackBufferWidth = preferedWidth;
            graphicsDeviceManager.PreferredBackBufferHeight = preferedHeight;
            Globals.screenSize = new Vector2(preferedWidth, preferedHeight);

            renderer = new RenderTarget2D(graphics, (int)Globals.preferedResolution.X, (int)Globals.preferedResolution.Y);
            UpdateScaleMatrix();
        }

        private void OnEngineStart()
        {
            Engine.updateCalls.Add(Update);
        }

        internal void InitializeGraphics(GraphicsDeviceManager g, GameWindow w)
        {
            graphicsDeviceManager = g;
            graphics = g.GraphicsDevice;

            w.AllowUserResizing = true;
            w.ClientSizeChanged += OnWindowChanged;
        }

        private void OnWindowChanged(object sender, EventArgs e)
        {
            UpdateScaleMatrix();
            EventManager.Invoke(EventManagerTypes.WindowSizeChanged, e);
        }

        private void UpdateScaleMatrix()
        {
            Point size = graphics.Viewport.Bounds.Size;

            float _scaleX = (float)size.X / renderer.Width;
            float _scaleY = (float)size.Y / renderer.Height;
            Globals.scale = MathF.Min(_scaleX, _scaleY);

            renderRect.Width = (int)(MathF.Floor(renderer.Width * Globals.scale));
            renderRect.Height = (int)(MathF.Floor(renderer.Height * Globals.scale));

            renderRect.X = (size.X - renderRect.Width) / 2;
            renderRect.Y = (size.Y - renderRect.Height) / 2;
        }

        internal void Update(GameTime gameTime)
        {
            UpdateScaleMatrix();
        }

        internal void Draw(SpriteBatch batch)
        {
            graphics.SetRenderTarget(renderer);
            graphics.Clear(Color.CornflowerBlue);

            batch.Begin(transformMatrix: transform, samplerState: samplerState);
            mainDrawCalls.ForEach(x => x.Invoke(batch));
            batch.End();

            batch.Begin(samplerState: samplerState);
            mainDrawCalls.ForEach(x => x.Invoke(batch));
            batch.End();

            graphics.SetRenderTarget(null);
            graphics.Clear(Color.Black);

            batch.Begin(samplerState: samplerState);
            batch.Draw(renderer, renderRect, Color.White);
            uiDrawCalls.ForEach(x => x.Invoke(batch));
            batch.End();

        }
    }
}
