using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThatOneGame.GameCode;

namespace ThatOneGame.Structure.UI
{
    public class EnemyUI : UIElement
    {
        public Enemy enemy;
        public int barSize;

        public EnemyUI(Enemy enemy, Point _position, Point _size, Color _color, UIAnchor _anchor = UIAnchor.TopLeft, UIStretch _stretch = UIStretch.None) :
        base(_position, _size, _color, _anchor, _stretch)
        {
            this.enemy = enemy;
        }

        public override void Draw(SpriteBatch batch)
        {
            base.Draw(batch);
            
            Point barPos = rect.Location;
            barPos.Y += size.Y;
            barPos.Y -= barSize;

            Point barBackSize = size;
            barBackSize.Y = barSize;

            Point barFrontSize = this.size;
            barFrontSize.Y = barSize;

            float percentage = GetPercentage();
            float width = barBackSize.X;

            float num = width * (1 - percentage);
            barFrontSize.X = (int)num;

            batch.FillRectangle(new Rectangle(barPos, barBackSize), Color.Red);
            batch.FillRectangle(new Rectangle(barPos, barFrontSize), Color.Green);
        }

        public float GetPercentage()
        {
            float max = enemy.stats[Stats.Max_Hp];
            float curr = enemy.stats[Stats.Hp];

            return 1-(curr / max);
        }

        
    }
}
