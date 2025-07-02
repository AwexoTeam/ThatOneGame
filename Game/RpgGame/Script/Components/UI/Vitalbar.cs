using Gum.DataTypes;
using Microsoft.Xna.Framework;
using MonoGameGum.GueDeriving;
using RpgGame.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpgGame.Script.Components.UI
{
    public class Vitalbar
    {
        public Stats currStat;
        public Stats maxStat;

        public ColoredRectangleRuntime backBar;
        public ColoredRectangleRuntime frontBar;

        public Color color;

        public Vitalbar(ColoredRectangleRuntime container, Stats currStat, Stats maxStat, Color color)
        {
            this.currStat = currStat;
            this.maxStat = maxStat;
            this.color = color;

            backBar = new ColoredRectangleRuntime();
            frontBar = new ColoredRectangleRuntime();

            backBar.WidthUnits = DimensionUnitType.PercentageOfParent;
            backBar.HeightUnits = DimensionUnitType.PercentageOfParent;
            backBar.Width = 100;
            backBar.Height = 45;
            backBar.Color = Color.Gray;

            frontBar.WidthUnits = DimensionUnitType.PercentageOfParent;
            frontBar.HeightUnits = DimensionUnitType.PercentageOfParent;
            frontBar.Width = 50;
            frontBar.Height = 100;
            frontBar.Color = color;

            backBar.AddChild(frontBar);
            container.AddChild(backBar);
        }

        public void UpdateBar()
        {
            var entity = Player.player.entity;

            float curr = entity.GetStat(currStat);
            float max = entity.GetStat(maxStat);

            float percentage = curr / max;
            frontBar.Width = percentage;
        }
    }
}
