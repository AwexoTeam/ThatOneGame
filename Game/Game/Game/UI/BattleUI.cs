using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThatOneGame.Structure;

namespace ThatOneGame.GameCode
{
    public class BattleUI : SpriteObject
    {
        private bool isOn;
        private Rectangle screenSize;

        public override void Start()
        {
            screenSize = new Rectangle(0, 0, 0, 0);

            screenSize.Width = Globals.RESOLUTION_WIDTH;
            screenSize.Height = Globals.RESOLUTION_HEIGHT;

            base.Start();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (!Input.IsKeyUp(Keys.F9))
                return;

            isOn = !isOn;
        }

        public override void UIDraw(SpriteBatch batch)
        {
            if (!isOn)
                return;

            base.UIDraw(batch);
            batch.FillRectangle(screenSize, Color.White);
        }
    }
}
