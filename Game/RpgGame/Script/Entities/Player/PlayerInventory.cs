using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGameGum.Forms.Controls;
using MonoGameGum.GueDeriving;
using RpgGame.Script.Manager;

namespace RpgGame.Structure
{
    public class PlayerInventory : Component
    {
        private Panel window;

        private NineSliceRuntime backgroundWindow;
        private NineSliceRuntime backgroundEquipment;

        private Button topLayer;
        private Button middleLayer;
        private Button underLayer;

        private int buttonHeight = 20;

        public override void Start()
        {
            int offset = 5;
            int doubleOffset = offset * 2;

            window = new Panel();
            window.Width = 600;
            window.Height = 400;
            GUI.AddElement(window);
            window.IsVisible = false;

            backgroundWindow = GUI.GetWindow(0, 0, 0, 600, 400);
            backgroundEquipment = GUI.GetWindow(2, offset, doubleOffset + buttonHeight, 200, 400-doubleOffset - buttonHeight - offset);

            backgroundWindow.AddChild(backgroundEquipment);
            window.AddChild(backgroundWindow);
            
            UpdateButton(ref topLayer, "Top", 0, offset);
            UpdateButton(ref middleLayer, "Middle", 1, offset);
            UpdateButton(ref underLayer, "Under", 2, offset);

        }

        private void UpdateButton(ref Button btn, string name, int id, int offset)
        {
            btn = new Button();
            btn.Height = buttonHeight;

            int width = (int)((backgroundEquipment.Width-offset) / 3);

            btn.Width = width - offset;
            
            btn.X = offset + (width + offset)*id;
            btn.Y = offset * 2;

            btn.Text = name;

            window.AddChild(btn);
        }

        public override void Update(GameTime gametime)
        {
            if (!Input.IsKeyUp(Keys.Tab))
                return;

            window.IsVisible = !window.IsVisible;
            
        }
    }
}
