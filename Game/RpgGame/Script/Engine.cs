using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameGum;
using RpgGame.Managers;
using RpgGame.Script.Manager;
using RpgGame.Structure;
using System;
using System.Collections.Generic;

namespace RpgGame
{
    public class Engine : Game
    {
        public static Engine engine;

        public static GraphicsDeviceManager graphics;
        private SpriteBatch batch;

        public static List<Action<GameTime>> updateCalls;

        public static RenderManager renderer;
        public Engine()
        {
            updateCalls = new List<Action<GameTime>>();
            
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            Window.ClientSizeChanged += Window_ClientSizeChanged;
        }

        private void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            EventManager.Invoke(EventManagerTypes.WindowSizeChanged, e);
        }

        protected override void Initialize()
        {
            base.Initialize();
            GUI.Init(this);
            EventManager.Invoke(EventManagerTypes.EngineInitizile, null);
        }

        protected override void LoadContent()
        {
            batch = new SpriteBatch(GraphicsDevice);
            renderer.InitializeGraphics(graphics, Window);
            EventManager.Invoke(EventManagerTypes.EngineContentLoad, null);
        }

        protected override void Update(GameTime gameTime)
        {
            Input.StartUpdate(gameTime);
            updateCalls.ForEach(x => x.Invoke(gameTime));
            base.Update(gameTime);
            Input.EndUpdate(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            renderer.Draw(batch);
        }
    }
}
