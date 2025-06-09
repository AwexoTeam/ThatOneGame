using Gum.Wireframe;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameGum;
using MonoGameGum.Forms.Controls;
using MonoGameGum.GueDeriving;
using RenderingLibrary.Graphics;
using RpgGame.Managers;
using System;
using System.ComponentModel;
using System.Drawing;

namespace RpgGame.Script.Manager
{
    public static class GUI
    {
        private static GumService service => GumService.Default;
        private static StackPanel mainPanel;
        private static GraphicsDevice graphics;
        private static string basePath = @"..\Tiles\World of Solaria UI\Windows.png";

        internal static void Init(Game window)
        {
            graphics = window.GraphicsDevice;
            EventManager.OnSceneUnloaded += OnSceneUnloaded;
            EventManager.OnWindowSizeChanged += OnWindowchanged;
            GumService.Default.Initialize(window);
            Engine.updateCalls.Add(Update);
        }

        private static void OnWindowchanged(System.EventArgs e)
        {
            GraphicalUiElement.CanvasWidth = graphics.Viewport.Width;
            GraphicalUiElement.CanvasHeight = graphics.Viewport.Height;

            // Grab your rootmost object and tell it to resize:
            GumService.Default.Root.UpdateLayout();
            
        }

        private static void Update(GameTime gameTime)
        {
            GumService.Default.Update(gameTime);
        }

        private static void OnSceneUnloaded(int id)
        {
            GumService.Default.Root.Children.Clear();

            mainPanel = new StackPanel();
            mainPanel.AddToRoot();
            mainPanel.Spacing = 3;
            mainPanel.Dock(Dock.Fill);
        }

        public static void AddElement(FrameworkElement element)
        {
            mainPanel.AddChild(element);
        }

        public static NineSliceRuntime GetWindow(int windowType, int x, int y, int width, int height)
        {
            var nineSlice = new NineSliceRuntime();
            nineSlice.SourceFileName = AppDomain.CurrentDomain.BaseDirectory+basePath;
            nineSlice.Height = height;
            nineSlice.Width = width;
            
            nineSlice.TextureTop = 0;
            nineSlice.TextureWidth = 48;
            nineSlice.TextureHeight = 48;
            nineSlice.TextureAddress = Gum.Managers.TextureAddress.Custom;
            nineSlice.TextureLeft = windowType * nineSlice.TextureLeft;

            nineSlice.X = x;
            nineSlice.Y = y;

            nineSlice.Visible = true;

            return nineSlice;
        }
    }
}
