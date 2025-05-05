using HentaiGame.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HentaiGame.Structure
{
    public class ScreenManager
    {
        private static ScreenManager _instance;
        
        private GameScreen currentScreen;

        public ContentManager content { private set; get; }

        public ScreenManager()
        {
            currentScreen = new SplashScreen();
        }

        public static ScreenManager Instance
        {
            get
            {
                if(_instance == null)
                    _instance = new ScreenManager();

                return _instance;
            }
        }

        public void LoadContent(ContentManager content)
        {
            this.content = new ContentManager(content.ServiceProvider, "Content");
            currentScreen.LoadContent(content);
        }

        public void UnloadContent()
        {
            currentScreen.UnloadContent();
        }

        public void Update(GameTime gameTime)
        {
            currentScreen.Update(gameTime);
        }

        public void Draw(SpriteBatch batch)
        {
            currentScreen.Draw(batch);
        }
    }
}
