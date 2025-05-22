using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using RpgGame.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpgGame.Scenes
{
    public class MenuScene : Scene
    {
        public override void Start()
        {
            
        }

        public override void UIDraw(SpriteBatch batch)
        {
            batch.FillRectangle(new RectangleF(0, 0, 100, 100), Color.White);
        }
    }
}
