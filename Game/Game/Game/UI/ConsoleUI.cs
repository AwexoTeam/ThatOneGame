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

        private Inputfield inputField;
        private string currResponse = "";
        private SpriteFont font;

        public override void Start()
        {
            string fontData;
            using (var stream = TitleContainer.OpenStream("Fonts/test.fnt"))
            {
                using (var reader = new StreamReader(stream))
                {
                    fontData = reader.ReadToEnd();
                }
            }

            inputField = new Inputfield();
            font = BMFontLoader.Load(fontData, name => TitleContainer.OpenStream("Fonts/" + name), Engine.batch.GraphicsDevice);
            inputField.textBoxWidth = (int)Globals.screenSize.X - 20;
            inputField.position = new Vector2(10, (int)Globals.screenSize.Y - inputField.textBoxHeight-5);
            inputField.visible = true;
            inputField._font = font;

            inputField._length = 999999;
            base.Start();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
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
            base.UIDraw(batch);
            inputField.Draw(batch);

            Vector2 rectPos = new Vector2(inputField.position.X, inputField.position.Y - inputField.textBoxHeight-5);
            Rectangle rect = new Rectangle((int)rectPos.X, (int)rectPos.Y, inputField.textBoxWidth, inputField.textBoxHeight);
            batch.FillRectangle(rect, Color.White);
            batch.DrawString(font, currResponse, rectPos, Color.Black);
        }
    }
}
