using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using ThatOneGame.Structure;

namespace ThatOneGame.GameCode
{
    public class ScreenManager : Manager
    {
        public static ScreenManager instance;
        public GameScreen screen;
        public GameScreen[] screens;

        private bool hasInitScene;

        public override void Initialize()
        {
            if (instance != null)
                return;

            instance = this;
            screens = Utils.GetAllTypes<GameScreen>();

            int index = GetScreenId<MenuScreen>();
            LoadScreen(index);
        }

        public override void Update(GameTime gameTime)
        {
            Engine.window.Title = screen.GetType().Name;
            if (!hasInitScene)
                return;

            screen.Update(gameTime);
        }

        public int GetScreenId<T>() where T : GameScreen
            => Array.FindIndex(screens, x => x.GetType() ==  typeof(T));

        public void LoadScreen(int screenId)
        {
            var screen = screens[screenId];
            LoadScreen(screen);
        }

        public override void Draw(SpriteBatch batch)
        {
            base.Draw(batch);
            screen.Draw(batch);
        }

        public override void PostDraw(SpriteBatch batch)
        {
            base.PostDraw(batch);
        }

        public override void DrawUI(SpriteBatch batch)
        {
            base.DrawUI(batch);
            screen.UIDraw(batch);
        }

        public void LoadScreen(GameScreen screen)
        {
            if(screen != null)
                screen.UnloadContent();
            
            Debug.LogDebug("Loading " + screen.GetType().Name);
            hasInitScene = false;
            this.screen = screen;
            screen.Start();

            hasInitScene = true;

        }
    }
}
