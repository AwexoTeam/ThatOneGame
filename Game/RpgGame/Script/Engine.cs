using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RpgGame.Managers;
using RpgGame.Structure;
using System;
using System.Collections.Generic;

namespace RpgGame
{
    public class Engine : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch batch;

        public static List<Action<GameTime>> updateCalls;

        public static RenderManager renderer;
        public Engine()
        {
            updateCalls = new List<Action<GameTime>>();
            
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
            EventManager.Invoke(EventManagerTypes.EngineInitizile, null);
        }

        protected override void LoadContent()
        {
            batch = new SpriteBatch(GraphicsDevice);
            renderer.InitializeGraphics(_graphics, Window);
            EventManager.Invoke(EventManagerTypes.EngineContentLoad, null);

        }

        protected override void Update(GameTime gameTime)
        {
            updateCalls.ForEach(x => x.Invoke(gameTime));
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            renderer.Draw(batch);
        }
    }
}
