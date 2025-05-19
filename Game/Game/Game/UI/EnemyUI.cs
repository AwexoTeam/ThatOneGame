using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThatOneGame.GameCode;
using ThatOneGame.Script.UI;

namespace ThatOneGame.Structure.UI
{
    public class EnemyUI : UIButton
    {
        public int id;
        public Enemy enemy;
        public int barSize;

        public int tileX = 0;
        public int tileY = 0;
        public int tileSize = 16;

        public Texture2D texture;

        public EnemyUI(Enemy enemy, Point _position, Point _size, Color _color, UIAnchor _anchor = UIAnchor.TopLeft, UIStretch _stretch = UIStretch.None) :
        base(_position, _size, _color, _anchor, _stretch)
        {
            this.enemy = enemy;

            texture = Texture2D.FromFile(Engine.batch.GraphicsDevice,enemy.texturePath);
        }

        public override void Draw(SpriteBatch batch)
        {
            color = BattleManager.instance.selectedIndex == id ? Color.Lime : Color.White;
            base.Draw(batch);

            batch.Draw(texture,rect, new Rectangle(tileSize*tileX, tileSize*tileY, tileSize, tileSize), Color.White);

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
            float max = enemy.entity.GetStat(Stats.Max_Hp);
            float curr = enemy.entity.GetStat(Stats.Hp);

            return 1-(curr / max);
        }

        public override bool isClicked()
        {
            if (enemy.isNull())
                return false;

            if (enemy.entity.isDead())
                return false;

            return base.isClicked();
        }
    }
}
