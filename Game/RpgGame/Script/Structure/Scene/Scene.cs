using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpgGame.Structure
{
    public class Scene
    {
        public Map map;

        private List<GameObject> gameObjects = new List<GameObject>();
        
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

        }

        public virtual void UIDraw(SpriteBatch batch)
        {

        }

        public virtual void AddGameObject(GameObject gameObject)
        {
            gameObjects.Add(gameObject);
            InvalidateLists();
        }

        public virtual void InvalidateLists()
        {
            gameObjects = gameObjects.OrderBy(x => x.priority).ToList();
        }

        public List<GameObject> GetGameObjects() => gameObjects;
    }
}
