using MapEditor.Scripts.Definations.Enums;
using MapEditor.Scripts.Definations.Structure;
using MapEditor.Scripts.UI.Component;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapEditor.Scripts.Scenes.TestSceneObj
{
    public class MenuPanel : GameObject
    {
        private Panel textPanel;
        private Panel idPanel;
        private Button btn;
        private string currentMode = "Drawing";

        public override void Start(Game game)
        {
            base.Start(game);
            SetupUI();
            Engine.OnWindowChanged += Engine_OnWindowChanged;
        }

        private void Engine_OnWindowChanged() => SetupUI();

        private void SetupUI()
        {
            int offset = 5;
            int doubleOffset = offset * 2;

            int textpanelWidth = 400;
            int textpanelHeight = Engine.ScreenSize.Y - doubleOffset;

            Point buttonLocation = new Point(doubleOffset, doubleOffset);
            Point buttonSize = new Point(textpanelWidth - doubleOffset, 50);

            textPanel = new Panel(AnchorMode.TopRight, offset, offset, textpanelWidth, textpanelHeight, Color.White);
            btn = new Button(AnchorMode.TopRight, buttonLocation, buttonSize, Color.Black);
            btn.OnClick = new Action(() => Console.WriteLine("I was clicked!"));

            int tilePanelY = doubleOffset + btn.height + offset;

            int tilePanelWidth = textpanelWidth - doubleOffset;
            int tilePanelHeight = Engine.ScreenSize.Y - tilePanelY - doubleOffset;

            idPanel = new Panel(AnchorMode.TopRight, doubleOffset, tilePanelY, tilePanelWidth, tilePanelHeight, Color.Blue);
        }

        public override void Update(GameTime gameTime)
        {
            btn.Update(gameTime);
            btn.text = currentMode;
            
        }

        public override void Draw(SpriteBatch batch)
        {
            textPanel.Draw(batch);
            idPanel.Draw(batch);
            btn.Draw(batch);

        }
    }
}
