using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace HentaiGame.Structure
{
    public class GameScreen
    {
        protected ContentManager content;

        public virtual void LoadContent(ContentManager content)
        {
            this.content = new ContentManager(ScreenManager.Instance.content.ServiceProvider, "Content");
        }

        public virtual void UnloadContent()
        {
            content.Unload();
        }

        public virtual void Update(GameTime gameTime)
        {

        }

        public virtual void Draw(SpriteBatch batch)
        {

        }
    }
}
