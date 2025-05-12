using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame;
using System.Linq;
using System;
using ThatOneGame.GameCode;
using static System.Net.Mime.MediaTypeNames;

namespace ThatOneGame.Structure.UI
{
    public class Inputfield
    {
        public string currentText = "";
        public Vector2 currentTextPosition { get; set; }
        public Vector2 cursorPosition { get; set; }
        public int animationTime { get; set; }
        public bool visible { get; set; }
        public Vector2 position { get; set; }
        public bool selected { get; set; }
        
        public int textBoxWidth;
        public int textBoxHeight = 20;

        public int _length;
        private bool _numericOnly;
        public SpriteFont _font;

        public void Update()
        {
            if (Input.IsKeyUp(Keys.Back))
            {
                RemoveText();
                return;
            }

            if (Input.IsMouseUp(0) && IsClickedOn())
            {
                Player.player.blockInput = true;
                selected = true;
            }

            else if (Input.IsMouseUp(0) && !IsClickedOn())
            {
                Player.player.blockInput = false;
                selected = false;
            }

            if (!selected)
                return;

            animationTime++;

            var keysPressed = Input.GetPressedKeys();
            if (keysPressed.Length <= 0)
                return;

            foreach ( var pressedKey in keysPressed)
            {
                if (!Input.IsKeyUp(pressedKey))
                    return;

                char pressedKeyChar = 'a';
                if (!TryGetChar(pressedKey, out pressedKeyChar))
                    return;

                Debug.LogDebug(pressedKeyChar);
                AddMoreText(pressedKeyChar);
            }
        }

        public bool TryGetChar(Keys key, out char c)
        {
            c = key.ToString()[0];
            if (key.ToString().Length <= 1)
                return true;

            int id = (int)key;
            if(id >= (int)Keys.NumPad0 && id < (int)Keys.NumPad9)
            {
                c = (id-(int)Keys.NumPad0).ToString()[0];
                return true;
            }

            if (id >= (int)Keys.D0 && id < (int)Keys.D9)
            {
                c = (id - (int)Keys.D0).ToString()[0];
                return true;
            }

            if(key == Keys.D9 || key == Keys.NumPad9)
            {
                c = '9';
                return true;
            }

            if (id >= (int)Keys.NumPad0 && id < (int)Keys.NumPad9)
            {
                c = '1';
                return true;
            }

            if (key == Keys.OemComma)
            {
                c = ',';
                return true;
            }

            if(key == Keys.OemPeriod)
            {
                c = '.';
                return true;
            }

            if(key == Keys.Space)
            {
                c = ' ';
                return true;
            }

            return false;
        }

        private bool IsClickedOn()
        {
            var cursor = Input.MousePositionRaw;

            if (cursor.X < position.X)
                return false;

            if (cursor.Y < position.Y)
                return false;

            if (cursor.X > position.X + textBoxWidth)
            {
                return false;
            }

            if (cursor.Y > position.Y + textBoxHeight)
                return false;

            Debug.LogDebug("I was clicked");
            return true;
        }

        public bool isFlashingCursorVisible()
        {
            int time = animationTime % 60;
            return time >= 0 && time < 31;
        }

        public void AddMoreText(char text)
        {
            Vector2 spacing = new Vector2();
            KeyboardState keyboardState = Keyboard.GetState();
            bool lowerThisChar = true;

            if (keyboardState.CapsLock || keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift))
                lowerThisChar = false;

            if(_numericOnly && (int)char.GetNumericValue(text) < 0 || (int)char.GetNumericValue(text) > 9)
            {
                if (text != '\b')
                    return;
            }

            if(text != '\b')
            {
                if(currentText.Length < _length)
                {
                    if (lowerThisChar)
                        text = char.ToLower(text);

                    currentText += text;
                    spacing = _font.MeasureString(text.ToString());
                    cursorPosition = new Vector2(cursorPosition.X + spacing.X, cursorPosition.Y);
                }
            }
            else
            {
                if(currentText.Length > 0)
                {
                    spacing = _font.MeasureString(currentText.Substring(currentText.Length - 1));

                    currentText = currentText.Remove(currentText.Length - 1, 1);
                    cursorPosition = new Vector2(cursorPosition.X - spacing.X, cursorPosition.Y);
                }
            }
        }

        private void RemoveText()
        {
            if (currentText == "" || currentText == string.Empty)
                return;

            Vector2 spacing = new Vector2();
            spacing = _font.MeasureString(currentText.Last().ToString());
            currentText = currentText.Remove(currentText.Length - 1);
            cursorPosition = new Vector2(cursorPosition.X - spacing.X, cursorPosition.Y);
        }

        public void Draw(SpriteBatch batch)
        {
            if (!visible)
                return;

            batch.FillRectangle(position, new Vector2(textBoxWidth, textBoxHeight), Color.White);
            batch.DrawString(_font, currentText, new Vector2(currentTextPosition.X + position.X, position.Y), Color.Black);

            if (isFlashingCursorVisible())
            {
                int size = currentText == "" || currentText == string.Empty ? 10 : 2;
                batch.FillRectangle(new Vector2(cursorPosition.X+10, position.Y), new Vector2(size, textBoxHeight-1), Color.Black);
            }

        }

        public void Reset()
        {
            currentText = "";
            cursorPosition = new Vector2();
        }
    }
}
