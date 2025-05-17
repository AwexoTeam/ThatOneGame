using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame;
using SpriteFontPlus;
using System;
using System.IO;
using System.Linq;
using ThatOneGame.Structure;
using ThatOneGame.Structure.UI;


namespace ThatOneGame.GameCode
{
    public class ConsoleUI : SpriteObject
    {

        public bool isVisible;

        public Inputfield inputField;
        private string currResponse = "";
        
        public override void Start()
        {
            
            inputField = new Inputfield();
            inputField.textBoxWidth = (int)Globals.screenSize.X - 20;
            inputField.position = new Vector2(10, (int)Globals.screenSize.Y - inputField.textBoxHeight-5);
            inputField.visible = true;
            inputField._font = Globals.font;

            inputField._length = 999999;
            base.Start();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            inputField.visible = isVisible;
            inputField.position = new Vector2(10, (int)Globals.screenSize.Y - inputField.textBoxHeight - 5);
            inputField.textBoxWidth = (int)Globals.screenSize.X - 20;
            inputField.Update();

            if (Input.IsKeyUp(Keys.Enter))
            {
                CommandManager.instance.TryCommand(inputField.currentText, out currResponse);
                inputField.Reset();
            }
        }

        public override void UIDraw(SpriteBatch batch)
        {
            if (!isVisible)
                return;

            base.UIDraw(batch);
            inputField.Draw(batch);

            Vector2 rectPos = new Vector2(inputField.position.X, inputField.position.Y - inputField.textBoxHeight-5);
            Rectangle rect = new Rectangle((int)rectPos.X, (int)rectPos.Y, inputField.textBoxWidth, inputField.textBoxHeight);
            batch.FillRectangle(rect, Color.White);
            batch.DrawString(Globals.font, currResponse, rectPos, Color.Black);
        }
    }
}
