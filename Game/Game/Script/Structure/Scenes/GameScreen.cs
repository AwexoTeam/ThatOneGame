using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame;
using System;
using System.Collections.Generic;
using System.Linq;
using ThatOneGame.Structure;

namespace ThatOneGame.Structure
{
    public class GameScreen
    {
        public Map map;
        
        private List<GameObject> gameObjects = new List<GameObject>();
        private List<SpriteObject> renderables = new List<SpriteObject>();

        public virtual void UnloadContent()
        {
            gameObjects.ForEach(x => x.Unload());
        }

        public virtual void Start()
        {
            InvalidateLists();

            foreach (var gobj in gameObjects)
            {
                gobj.Start();
            }
        }

        public virtual void Update(GameTime gameTime)
        {
            if (gameObjects == null || gameObjects.Count <= 0)
                return;

            gameObjects.ForEach(x => x.Update(gameTime));
        }

        public virtual void Draw(SpriteBatch batch)
        {
            if (renderables == null || renderables.Count <= 0)
                return;

            renderables.ForEach(x => x.Draw(batch));
        }

        public virtual void UIDraw(SpriteBatch batch)
        {
            if (renderables == null || renderables.Count <= 0)
                return;

            renderables.ForEach(x => x.UIDraw(batch));
        }

        public virtual void AddGameObject(GameObject gameObject)
        {
            if (gameObject is SpriteObject)
                renderables.Add((SpriteObject)gameObject);

            gameObjects.Add(gameObject);
            InvalidateLists();
        }

        public virtual void InvalidateLists()
        {
            gameObjects = gameObjects.OrderBy(x => x.priority).ToList();
            renderables = renderables.OrderBy(x => x.priority).ToList();
        }

        public List<GameObject> GetGameObjects() => gameObjects;
    }
}
