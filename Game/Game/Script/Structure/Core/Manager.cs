using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThatOneGame.Structure
{
    public abstract class Manager
    {
        public abstract void Initialize();
        public virtual void Destroy() { }
        public virtual void Update(GameTime gameTime) { }

        public virtual void Draw(SpriteBatch spriteBatch) { }
        public virtual void PostDraw(SpriteBatch spriteBatch) { }
        public virtual void DrawUI(SpriteBatch spriteBatch) { }
    }
}
