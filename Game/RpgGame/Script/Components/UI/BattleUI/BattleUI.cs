using Gum.Converters;
using Gum.DataTypes;
using Gum.Managers;
using Gum.Wireframe;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameGum.Forms.Controls;
using MonoGameGum.GueDeriving;
using RpgGame.Components;
using RpgGame.Entities;
using RpgGame.Managers;
using RpgGame.Script.Manager;
using RpgGame.Structure;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace RpgGame.Script.Components.UI.BattleUI
{
    public class EnemyUI
    {
        public Enemy enemy;
        public int id;

        public Button container;
        public ColoredRectangleRuntime selectionDisplay;
        public SpriteRuntime spriteContainer;
        public ColoredRectangleRuntime backHealthBar;
        public ColoredRectangleRuntime frontHealthBar;

        public EnemyUI(Enemy enemy, GraphicalUiElement parent, int i)
        {
            this.id = i;
            this.enemy = enemy;
            container = new Button();
            parent.AddChild(container.Visual);

            container.Visual.WidthUnits = DimensionUnitType.PercentageOfParent;
            container.Visual.HeightUnits = DimensionUnitType.PercentageOfParent;

            container.Width = 100;
            container.Height = 100;

            selectionDisplay = new ColoredRectangleRuntime();
            container.AddChild(selectionDisplay);

            selectionDisplay.WidthUnits = DimensionUnitType.PercentageOfOtherDimension;
            selectionDisplay.HeightUnits = DimensionUnitType.PercentageOfParent;

            selectionDisplay.Width = 100;
            selectionDisplay.Height = 75;
            selectionDisplay.Color = Color.Green;
            selectionDisplay.Alpha = 0;

            selectionDisplay.Anchor(Anchor.Top);
            selectionDisplay.YUnits = GeneralUnitType.Percentage;
            selectionDisplay.Y = 5;


            spriteContainer = new SpriteRuntime();
            selectionDisplay.AddChild(spriteContainer);

            spriteContainer.HeightUnits = DimensionUnitType.PercentageOfParent;
            spriteContainer.WidthUnits = DimensionUnitType.PercentageOfParent;
            spriteContainer.Width = 100;
            spriteContainer.Height = 100;

            spriteContainer.Texture = Texture2D.FromFile(Engine.graphics.GraphicsDevice,enemy.texturePath);
            spriteContainer.TextureAddress = TextureAddress.Custom;
            spriteContainer.TextureLeft = 0;
            spriteContainer.TextureTop = 0;
            spriteContainer.TextureHeight = 16;
            spriteContainer.TextureWidth = 16;


            float height = (100 - selectionDisplay.Height) / 2;
            backHealthBar = new ColoredRectangleRuntime();
            frontHealthBar = new ColoredRectangleRuntime();

            container.AddChild(backHealthBar);
            container.AddChild(frontHealthBar);

            backHealthBar.HeightUnits = DimensionUnitType.PercentageOfParent;
            frontHealthBar.HeightUnits = DimensionUnitType.PercentageOfParent;
            backHealthBar.WidthUnits = DimensionUnitType.PercentageOfParent;
            frontHealthBar.WidthUnits = DimensionUnitType.PercentageOfParent;

            frontHealthBar.XUnits = GeneralUnitType.Percentage;
            backHealthBar.XUnits = GeneralUnitType.Percentage;

            frontHealthBar.X = 5;
            backHealthBar.X = 5;

            backHealthBar.Width = 90;
            frontHealthBar.Width = 45;
            backHealthBar.Height = height;
            frontHealthBar.Height = height;

            backHealthBar.Color = Color.DarkRed;
            frontHealthBar.Color = Color.Green;

            backHealthBar.Anchor(Anchor.BottomLeft);
            frontHealthBar.Anchor(Anchor.BottomLeft);

            container.Text = "";
            container.Click += OnEnemyClick;
        }

        private void OnEnemyClick(object sender, EventArgs e)
        {
            BattleManager.instance.ChangeSelected(id);
        }

        public void UpdateUI()
        {
            if (enemy.isDead())
            {
                container.IsEnabled = false;
                frontHealthBar.Width = 0;
                selectionDisplay.Alpha = 0;
                return;
            }

            selectionDisplay.Alpha = BattleManager.instance.selectedEnemy == id ? 255 : 0;
            Debug.LogDebug("Alpha on #" + id + " is now " + selectionDisplay.Alpha);

            float curr = enemy.GetStat(Stats.Hp);
            float max = enemy.GetStat(Stats.Max_Hp);

            float percentage = curr / max;

            frontHealthBar.Width = 90 * percentage;
        }
    }

    public static class BattleUI
    {
        public static ColoredRectangleRuntime enemyPanel;
        public static NineSliceRuntime containerWindow;
        public static NineSliceRuntime actionPanel;

        public static int buttonRows = 3;
        public static int buttonColumns = 3;

        public static List<EnemyUI> enemyUIs = new List<EnemyUI>();

        public static void SetUIToBattle()
        {
            SetContainer();
            SetActionPanel();
            SetEnemyPanel();
            SetActionButtons();
            DrawEnemies();

            MainPlayerUI.instance.ChangeMiddlePanel(containerWindow);
        }

        private static void SetEnemyPanel()
        {
            enemyPanel = new ColoredRectangleRuntime();
            containerWindow.AddChild(enemyPanel);

            enemyPanel.HeightUnits = DimensionUnitType.PercentageOfParent;
            enemyPanel.WidthUnits = DimensionUnitType.PercentageOfParent;

            enemyPanel.XUnits = GeneralUnitType.Percentage;
            enemyPanel.YUnits = GeneralUnitType.Percentage;
            enemyPanel.X = 1;
            enemyPanel.Y = 2;

            enemyPanel.Height = 65;
            enemyPanel.Width = 98;
            enemyPanel.Color = Color.DarkGray;

            enemyPanel.ChildrenLayout = ChildrenLayout.AutoGridHorizontal;
            enemyPanel.AutoGridHorizontalCells = 3;
            enemyPanel.AutoGridVerticalCells = 3;
        }

        private static void SetActionPanel()
        {
            actionPanel = GUI.GetWindow(10, 0, 0, 0, 0);
            containerWindow.AddChild(actionPanel);

            actionPanel.HeightUnits = DimensionUnitType.PercentageOfParent;
            actionPanel.WidthUnits = DimensionUnitType.PercentageOfParent;
            actionPanel.Height = 30;
            actionPanel.Width = 98;

            actionPanel.Anchor(Anchor.BottomLeft);

            actionPanel.XUnits = GeneralUnitType.Percentage;
            actionPanel.YUnits = GeneralUnitType.Percentage;
            actionPanel.X = 1;
            actionPanel.Y = 99;
        }

        private static void SetActionButtons()
        {
            for (int y = 0; y < buttonRows; y++)
            {
                for (int x = 0; x < buttonColumns; x++)
                {
                    CreateActionButton(x, y);
                }
            }
        }

        private static void CreateActionButton(int x, int y)
        {
            float width = 100 / buttonColumns;
            float height = 100 / buttonRows;

            Button btn = new Button();
            btn.Visual.XUnits = GeneralUnitType.Percentage;
            btn.Visual.YUnits = GeneralUnitType.Percentage;

            btn.Visual.WidthUnits = DimensionUnitType.PercentageOfParent;
            btn.Visual.HeightUnits = DimensionUnitType.PercentageOfParent;

            actionPanel.AddChild(btn.Visual);

            btn.X = x*width+1;
            btn.Y = y*height+2;

            btn.Width = width-1;
            btn.Height = height-2;

            btn.Text = "N/A";
            
            if (x != 0 || y != 0)
                return;

            btn.Text = "Attack";
            btn.Click += OnSkillClick;
        }

        private static void OnSkillClick(object sender, EventArgs e)
        {
            if (!BattleManager.instance.isPlayerTurn)
                return;

            Button btn = (Button)sender;
            BattleManager.instance.ExecuteAttack(0);
        }

        private static void SetContainer()
        {
            containerWindow = GUI.GetWindow(0, 0, 0, 0, 0);
            MainPlayerUI.instance.ChangeMiddlePanel(containerWindow);

            containerWindow.HeightUnits = DimensionUnitType.PercentageOfParent;
            containerWindow.WidthUnits = DimensionUnitType.PercentageOfParent;

            containerWindow.Height = 100;
            containerWindow.Width = 100;
        }

        public static void DrawEnemies()
        {
            for (int i = 0; i < BattleManager.instance.enemies.Count; i++)
            {
                EnemyUI eui = new EnemyUI(BattleManager.instance.enemies[i], enemyPanel,i);
                enemyUIs.Add(eui);
            }
        }

        public static void UpdateUI()
        {
            foreach (var eui in enemyUIs)
            {
                eui.UpdateUI();
            }
        }
    }
}
