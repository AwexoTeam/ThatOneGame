using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ThatOneGame.Structure
{
    public class AnimatedObject : SpriteObject
    {
        protected int animationTick;
        protected string basePath;
        protected string baseName;

        public AnimatedObject() { }
        public AnimatedObject(Vector2 position) : base(position)
        {
        
        }

        public override void Start()
        {
            base.Start();

            Timer timer = new Timer(250f);
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            TickAnimation();
        }

        protected virtual void TickAnimation()
        {
            animationTick++;
            if (animationTick * tileSize < texture.Width)
                return;

            animationTick = 0;
        }
    }
}
