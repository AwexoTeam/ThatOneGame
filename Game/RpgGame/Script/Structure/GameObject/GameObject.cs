using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RpgGame.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpgGame.Structure
{
    public class GameObject
    {
        public List<Component> components = new List<Component>();
        private List<IRenderable> renderables = new List<IRenderable>();

        public int priority;
        public Vector2 position;

        public GameObject() { }
        public GameObject(Vector2 _position)
        {
            position = _position;
        }

        public virtual void Load() { }

        public virtual void Awake()
        {
            if (components != null && components.Count > 0)
                components.ForEach(x => x.Awake());
        }

        public virtual void Start()
        {
            if(components != null && components.Count > 0) 
                components.ForEach(x => x.Start());
        }

        public virtual void Update(GameTime gameTime)
        {
            if (components != null && components.Count > 0)
                components.ForEach(x => x.Update(gameTime));
        }

        public virtual void Draw(SpriteBatch batch)
        {
            if (renderables != null && renderables.Count > 0)
                renderables.ForEach(x => x.Draw(batch));
        }

        public virtual void PostDraw(SpriteBatch batch)
        {
            if (renderables != null && renderables.Count > 0)
                renderables.ForEach(x => x.PostDraw(batch));
        }

        public virtual void Unload()
        {
            if (components != null && components.Count > 0)
                components.ForEach(x => x.Unload());
        }

        public static GameObject Instantiate(GameObject gameObject)
        {
            SceneManager.instance.scene.AddGameObject(gameObject);
            return gameObject;
        }

        public virtual T GetComponent<T>() where T : Component
        {
            var c = components.Find(x => x.GetType() ==  typeof(T));
            return (T)c;
        }

        public static T FindObjectOfType<T>() where T : Component
        {
            return FindObjectsOfType<T>().FirstOrDefault();
        }

        public static T[] FindObjectsOfType<T>() where T : Component
        {
            var gobjs = SceneManager.instance.scene.GetGameObjects();

            List<T> rtns = new List<T>();
            foreach (var gobj in gobjs)
            {
                var comp = gobj.components.Find(x => x is T);
                if (comp == null)
                    continue;

                rtns.Add((T)comp);
            }

            return rtns.ToArray();
        }

        public void AddComponent<T>() where T : Component, new()
        {
            var comp = new T();
            components.Add(comp);

            comp.gameObject = this;
            if(typeof(T).IsAssignableFrom(typeof(IRenderable)))
            {
                renderables = components.FindAll(x => x is IRenderable).Cast<IRenderable>().ToList();
            }
        }

        public void AddComponent(Component compononet)
        {
            components.Add(compononet);
            compononet.gameObject = this;
            if (compononet is IRenderable)
            {
                renderables = components.FindAll(x => x is IRenderable).Cast<IRenderable>().ToList();
            }
        }
    }
}
