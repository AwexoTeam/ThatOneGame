using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RpgGame.Scenes;
using RpgGame.Structure;
using System;

namespace RpgGame.Managers
{
    public partial class SceneManager : IHookable
    {
        public static SceneManager instance;
        public Scene scene;
        public Scene[] scenes;

        private bool hasInitScene;

        public int priority => 10000;

        public bool CanHook(IHookable[] hooks, out string error)
        {
            error = "none";
            return true;
        }

        public void Run()
        {
            if (instance != null)
                return;

            instance = this;
            scenes = Utils.GetAllTypes<Scene>();
            EventManager.OnEngineInitizile += OnInitialize;
        }

        private void OnInitialize()
        {
            RenderManager.mainDrawCalls.Add(Draw);
            RenderManager.postDrawCalls.Add(PostDraw);
            RenderManager.uiDrawCalls.Add(DrawUI);
            LoadMainMenu();
        }

        internal void LoadMainMenu()
        {
            int index = GetScreenId<MenuScene>();
            LoadScreen(index);
        }
        
        public void Update(GameTime gameTime)
        {

            Globals.window.Title = scene.GetType().Name;
            if (!hasInitScene)
                return;

            scene.Update(gameTime);
        }

        public int GetScreenId<T>() where T : Scene
            => Array.FindIndex(scenes, x => x.GetType() == typeof(T));

        public void LoadScreen(int screenId)
        {
            var screen = scenes[screenId];
            LoadScreen(screen);
        }

        public void Draw(SpriteBatch batch)
        {
            scene.Draw(batch);
        }

        public void PostDraw(SpriteBatch batch)
        {
        
        }

        public void DrawUI(SpriteBatch batch)
        {
            scene.UIDraw(batch);
        }

        public void LoadScreen(Scene _scene)
        {
            int id = Array.FindIndex(scenes, x => x.GetType() == _scene.GetType());
            EventManager.Invoke(EventManagerTypes.Scenequeued, id);

            int oldId = -1;
            if (_scene != null)
            {
                _scene.UnloadContent();
                oldId = Array.FindIndex(scenes, x => x.GetType() == _scene.GetType());
                EventManager.Invoke(EventManagerTypes.SceneUnloaded, oldId);
            }

            Debug.LogDebug("Loading " + _scene.GetType().Name);
            hasInitScene = false;
            scene = _scene;

            EventManager.Invoke(EventManagerTypes.SceneLoaded, id);
            _scene.Start();

            EventManager.Invoke(EventManagerTypes.SceneChanged, id, oldId);
            hasInitScene = true;

        }

    }
}
