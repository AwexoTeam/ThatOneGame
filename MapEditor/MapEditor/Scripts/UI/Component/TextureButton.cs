using MapEditor.Scripts.Definations.Enums;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapEditor.Scripts.UI.Component
{
    public class TextureButton : Button
    {
        public int index;

        public TextureButton(AnchorMode mode, Point point, Point size, string text = "") : base(mode, point, size, Color.White, text)
        {
        }
    }
}
