//Code for MainUI
using GumRuntime;
using MonoGameGum;
using MonoGameGum.GueDeriving;
using Gum.Converters;
using Gum.DataTypes;
using Gum.Managers;
using Gum.Wireframe;

using RenderingLibrary.Graphics;

using System.Linq;
using RpgGame.Structure;
using RpgGame.Script.Manager;
using MonoGameGum.Forms.Controls;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Microsoft.Xna.Framework;
using System;
using RpgGame.Script.Components.UI;

namespace RpgGame.Components
{
    
    public class MainPlayerUI : Component
    {
        public static MainPlayerUI instance;
        public const int sidePanelPercentage = 20;
        public ColoredRectangleRuntime vitalsBarContainer;

        public ContainerRuntime rightPanel;
        public ContainerRuntime leftPanel;
        public ContainerRuntime leftPanelContainer;
        public ContainerRuntime rightPanelContainer;
        
        public NineSliceRuntime leftNineSlice;
        public NineSliceRuntime rightNineSlice;

        public NineSliceRuntime mainWindow;
        public NineSliceRuntime mainNineSlice;

        public ColoredRectangleRuntime playerPortrait;

        public override void Start()
        {
            if (instance != null)
                return;

            instance = this;

            SetupPanel(ref leftPanel, ref leftPanelContainer, ref leftNineSlice, Anchor.Left);
            SetupPanel(ref rightPanel, ref rightPanelContainer, ref rightNineSlice, Anchor.Right);

            leftPanel.AddToRoot();
            rightPanel.AddToRoot();

            playerPortrait = new ColoredRectangleRuntime();
            var statBar = new TextRuntime();

            playerPortrait.WidthUnits = DimensionUnitType.PercentageOfParent;
            playerPortrait.Width = 80;
            playerPortrait.XUnits = GeneralUnitType.Percentage;
            playerPortrait.X = 10;

            playerPortrait.HeightUnits = DimensionUnitType.PercentageOfParent;
            playerPortrait.Height = 40;
            playerPortrait.YUnits = GeneralUnitType.Percentage;
            playerPortrait.Y = 2;

            statBar.WidthUnits = DimensionUnitType.PercentageOfParent;
            statBar.Width = 98;
            statBar.XUnits = GeneralUnitType.Percentage;
            statBar.X = 2;

            statBar.HeightUnits = DimensionUnitType.PercentageOfParent;
            statBar.Height = 50;
            //statBar.YUnits = GeneralUnitType.Percentage;
            statBar.Y = 10;
            statBar.Anchor(Anchor.Bottom);

            leftPanelContainer.AddChild(statBar);
            leftPanelContainer.AddChild(playerPortrait);

            statBar.Color = Color.Black;
            statBar.HorizontalAlignment = HorizontalAlignment.Left;

            foreach (var kv in Player.instance.entity.stats)
            {
                statBar.Text += $"{kv.Key}: {kv.Value}\n";
            }
        }

        private void SetupPanel(ref ContainerRuntime container, ref ContainerRuntime inside, ref NineSliceRuntime nineslice, Anchor anchor)
        {
            int alpha = 250;

            container = new ContainerRuntime();
            container.HeightUnits = DimensionUnitType.PercentageOfParent;
            container.WidthUnits = DimensionUnitType.PercentageOfParent;

            container.Height = 100f;
            container.Width = sidePanelPercentage;
            container.Anchor(anchor);
            
            nineslice = GUI.GetWindow(9, 0, 0, 0, 0);
            container.AddChild(nineslice);
            nineslice.WidthUnits = DimensionUnitType.PercentageOfParent;
            nineslice.HeightUnits = DimensionUnitType.PercentageOfParent;
            nineslice.Height = 100f;
            nineslice.Width = 100f;

            inside = new ContainerRuntime();
            inside.WidthUnits = DimensionUnitType.PercentageOfParent;
            inside.HeightUnits = DimensionUnitType.PercentageOfParent;
            inside.Height = 100f;
            inside.Width = 100f;

            container.AddChild(inside);
        }

        public void ChangeLeftPanel(GraphicalUiElement element)
        {
            leftPanelContainer.Children.Clear();
            leftPanelContainer.AddChild(element);
        }

        public void ChangeRightPanel(GraphicalUiElement element)
        {
            rightPanelContainer.Children.Clear();
            rightPanelContainer.AddChild(element);
        }

        public void ChangeMiddlePanel(GraphicalUiElement element)
        {
            mainWindow.Children.Clear();
            mainWindow.AddChild(element);
        }

    }
}
