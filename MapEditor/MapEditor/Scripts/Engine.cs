using MapEditor.Scripts.Definations.Enums;
using MapEditor.Scripts.Definations.Structure;
using MapEditor.Scripts.Scenes;
using MapEditor.Scripts.UI.Component;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame;
using SpriteFontPlus;
using System;
using System.IO;
using System.Windows.Forms;

using Panel = MapEditor.Scripts.UI.Component.Panel;

namespace MapEditor
{
    public class Engine : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        public static Point ScreenSize;
        public static Point MousePosition;

        private Scene currentScene;
        public delegate void _OnWindowChanged();
        public static event _OnWindowChanged OnWindowChanged;

        public Engine()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            Window.AllowUserResizing = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
            Window.ClientSizeChanged += Window_ClientSizeChanged;
            Mouse.SetPosition(0, 0);
        }

        private void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            ScreenSize = new Point(Window.ClientBounds.Width, Window.ClientBounds.Height);
            OnWindowChanged?.Invoke();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            ScreenSize = new Point(Window.ClientBounds.Width, Window.ClientBounds.Height);

            var fontBakeResult = TtfFontBaker.Bake(File.ReadAllBytes(@"C:\\Windows\\Fonts\arial.ttf"),
            25,
            1024,
            1024,
            new[]
            {
                CharacterRange.BasicLatin,
                CharacterRange.Latin1Supplement,
                CharacterRange.LatinExtendedA,
                CharacterRange.Cyrillic
            });
            SpriteFont font = fontBakeResult.CreateSpriteFont(GraphicsDevice);
            Globals.font = font;
            
            LoadScene(new TestScene());
        }

        public void LoadScene(Scene scene)
        {
            currentScene = scene;
            scene.Initialize();

            foreach (var gameObject in scene.gameObjects)
                gameObject.Start(this);

        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (currentScene == null)
                return;

            var mouseState = Mouse.GetState();
            MousePosition = new Point(mouseState.X, mouseState.Y);
            
            foreach (var gameObject in currentScene.gameObjects)
                gameObject.Update(gameTime);

        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            GraphicsDevice.Clear(Color.CornflowerBlue);
            if (currentScene == null)
                return;
            
            spriteBatch.Begin();
            foreach (var gameObject in currentScene.gameObjects)
                gameObject.Draw(spriteBatch);

            spriteBatch.End();
        }
    }
}
