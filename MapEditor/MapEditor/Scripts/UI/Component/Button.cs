using MapEditor.Scripts.Definations.Enums;
using MapEditor.Scripts.Definations.Structure;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapEditor.Scripts.UI.Component
{
    public class Button : Panel
    {
        public string text;
        public Action OnClick;
        private bool hasClicked;

        public Button(AnchorMode mode, Point point, Point size, Color color, string text = "")
        : base(mode, point, size, color)
        {
            this.text = text;
        }

        public override void Update(GameTime gameTime)
        {
            hasClicked = true;
            base.Update(gameTime);
            var state = Mouse.GetState();

            if (!Input.isPressed(0))
                return;

            if (!EnterButton())
                return;


            OnClick();
            hasClicked = false;
        }

        public bool EnterButton()
        {
            var pos = GetActualPosition();

            if (Engine.MousePosition.X < pos.X)
                return false;

            if(Engine.MousePosition.X > pos.X + width)
                return false;

            if(Engine.MousePosition.Y < pos.Y)
                return false;

            if(Engine.MousePosition.Y > pos.Y + height)
                return false;

            return true;
        }

        public override void Draw(SpriteBatch batch)
        {
            var pos = GetActualPosition();
            var size = Globals.font.MeasureString(text);

            base.Draw(batch);

            pos.X += width / 2;
            pos.X -= (int)(size.X / 2);

            pos.Y += height / 2;
            pos.Y -= (int)(size.Y / 2);
            
            batch.DrawString(Globals.font, text, new Vector2(pos.X, pos.Y), Color.White);
        }
    }
}
