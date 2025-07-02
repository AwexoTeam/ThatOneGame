using Gum.Wireframe;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameGum.Forms.Controls;
using MonoGameGum.GueDeriving;
using RpgGame.Managers;
using RpgGame.Script.Manager;
using System;
using System.Collections.Generic;
using System.ComponentModel;

//If you haven't seen this yet, check this out: https://docs.flatredball.com/gum/gum-tool/gum-elements/container/children-layout#auto-grid-horizontal-and-auto-grid-vertical

namespace RpgGame.Structure
{
    public class EquipmentSprite
    {
        public int x;
        public int y;
        public SpriteRuntime sprite;

        public EquipmentSprite(int x, int y, SpriteRuntime sprite)
        {
            this.x = x;
            this.y = y;
            this.sprite = sprite;
        }
    }

    public class PlayerInventory : Component
    {
        private Panel window;
        private Panel equipment;

        private NineSliceRuntime backgroundWindow;
        private NineSliceRuntime backgroundEquipment;

        private Button topLayer;
        private Button middleLayer;
        private Button underLayer;

        private int buttonHeight = 20;
        private string basePath = @"..\Tiles\World of Solaria UI\Inventory.png";

        private int currentSelected = 0;
        private List<EquipmentSprite> equipmentSprites = new List<EquipmentSprite>();
        private bool needUpdate;

        public override void Start()
        {
            int offset = 5;
            int doubleOffset = offset * 2;

            window = new Panel();
            window.Width = 600;
            window.Height = 400;
            GUI.AddElement(window);
            window.IsVisible = false;

            backgroundWindow = GUI.GetWindow(0, 0, 0, (int)window.Width, (int)window.Height);
            int height = (int)(backgroundWindow.Height);
            height -= doubleOffset + offset; 
            height -= buttonHeight;

            backgroundEquipment = GUI.GetWindow(2, offset, doubleOffset + buttonHeight, 200, height);

            backgroundWindow.AddChild(backgroundEquipment);

            equipment = new Panel();
            equipment.Width = backgroundEquipment.Width;
            equipment.Height = backgroundEquipment.Height;
            equipment.X = backgroundEquipment.X;
            equipment.Y = backgroundEquipment.Y;

            window.AddChild(backgroundWindow);
            window.AddChild(equipment);
            
            UpdateButton(ref topLayer, "Top", 0, offset);
            UpdateButton(ref middleLayer, "Middle", 1, offset);
            UpdateButton(ref underLayer, "Under", 2, offset);

            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    AddEquipmentButton(x, y, 3, 4);
                }
            }
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
            btn.Click += (s, e) => { currentSelected = id; needUpdate = true; };
            window.AddChild(btn);
        }

        private void AddEquipmentButton(int x, int y, int maxX, int maxY)
        {
            if (y == 0 && x == 0)
                return;

            if (y == 0 && x == 2)
                return;

            int btnHeight = 48;

            float scale = btnHeight / 16;

            var sprite = new SpriteRuntime();
            sprite.SourceFileName = AppDomain.CurrentDomain.BaseDirectory + basePath;
            sprite.TextureAddress = Gum.Managers.TextureAddress.Custom;

            sprite.Width = scale*100;
            sprite.Height = scale * 100;

            equipmentSprites.Add(new EquipmentSprite(x,y,sprite));

            SetTexture(ref sprite, currentSelected, x, y);

            Button btn = new Button();
            btn.Width = btnHeight;
            btn.Height = btnHeight;

            int offset = 5;

            int initialOffsetX = (maxX * btnHeight) + (offset * (maxX - 1));
            
            initialOffsetX = (int)equipment.Width - initialOffsetX;
            initialOffsetX = initialOffsetX / 2;

            int initialOffsetY = (maxY * btnHeight) + (offset * (maxY - 1));
            initialOffsetY = (int)equipment.Height - initialOffsetY;
            initialOffsetY = initialOffsetY / 2;

            btn.X = initialOffsetX + (x * (btnHeight+ offset));
            btn.Y = initialOffsetY + (y * (btnHeight+ offset));

            btn.Text = "";

            btn.Visual.Children.Add(sprite);
            equipmentSprites.Add(new EquipmentSprite(x, y, sprite));
            equipment.AddChild(btn);
        }

        public override void Update(GameTime gametime)
        {
            if (needUpdate)
            {
                for (int i = 0; i < equipmentSprites.Count; i++)
                {
                    var equipment = equipmentSprites[i];
                    
                    SetTexture(ref equipment.sprite, currentSelected, equipment.x, equipment.y);
                }

                needUpdate = false;
            }

            if (!Input.IsKeyUp(Keys.Tab))
                return;

            window.IsVisible = !window.IsVisible;
            
        }

        private void SetTexture(ref SpriteRuntime sprite, int color, int x, int y)
        {
            if (y == 0 && x == 0)
                return;

            if (y == 0 && x == 2)
                return;

            int numberOfColumns = 0;
            int size = 16;

            int tx = 0;
            int ty = 0;

            sprite.TextureTop = color * numberOfColumns;
            sprite.TextureWidth = size;
            sprite.TextureHeight = size;
            sprite.TextureLeft = 0;

            bool isEven = x % 2 == 0;
            if (isEven)
            {
                tx = y == 1 ? 1 : 2;
                ty = 2;

                ty += 5 * color;

                sprite.TextureLeft = tx * size;
                sprite.TextureTop = ty * size;
                return;
            }

            tx = y == 0 ? 5 : tx;
            tx = y == 1 ? 4 : tx;
            tx = y == 2 ? 2 : tx;
            tx = y == 3 ? 1 : tx;

            ty = y == 0 ? 0 : ty;
            ty = y == 1 ? 1 : ty;
            ty = y == 2 ? 3 : ty;
            ty = y == 3 ? 4 : ty;

            ty += 5 * color;

            sprite.TextureLeft = tx * size;
            sprite.TextureTop = ty * size;
        }
    }
}
