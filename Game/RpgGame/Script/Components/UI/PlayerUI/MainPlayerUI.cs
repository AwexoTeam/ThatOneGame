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
using RpgGame.UI;
using RpgGame.Script.Components.UI.BattleUI;
using RpgGame.Managers;

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

        public ContainerRuntime mainWindow;
        
        public override void Start()
        {
            if (instance != null)
                return;

            instance = this;

            SetupPanel(ref leftPanel, ref leftPanelContainer, ref leftNineSlice, Anchor.Left);
            SetupPanel(ref rightPanel, ref rightPanelContainer, ref rightNineSlice, Anchor.Right);

            leftPanel.AddToRoot();
            rightPanel.AddToRoot();

            mainWindow = new ContainerRuntime();
            mainWindow.HeightUnits = DimensionUnitType.PercentageOfParent;
            mainWindow.WidthUnits = DimensionUnitType.PercentageOfParent;
            mainWindow.Width = 100 - (sidePanelPercentage * 2);
            mainWindow.XUnits = GeneralUnitType.Percentage;
            mainWindow.X = sidePanelPercentage;
            mainWindow.Height = 100;

            mainWindow.AddToRoot();

            PlayerStatView.Initialize();
            ChangeLeftPanel(PlayerStatView.GetStatView());
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
            ClearMiddleWindow();
            mainWindow.AddChild(element);
        }

        public void ClearMiddleWindow()
        {
            mainWindow.Children.Clear();
        }
    }
}
