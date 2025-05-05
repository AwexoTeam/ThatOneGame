using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapEditor.Scripts.Definations.Structure
{
    public class GameObject
    {
        public int priority;
        public virtual void Start(Game game) { }
        public virtual void Update(GameTime gameTime) { }
        public virtual void Draw(SpriteBatch batch) { }
    }
}
