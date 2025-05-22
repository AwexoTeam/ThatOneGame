using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpgGame.Structure
{
    public interface IRenderable
    {
        void Draw(SpriteBatch batch);
        void PostDraw(SpriteBatch batch);
        void DrawUI(SpriteBatch batch);
    }
}
