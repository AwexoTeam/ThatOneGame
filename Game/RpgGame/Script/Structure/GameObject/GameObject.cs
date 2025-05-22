using Microsoft.Xna.Framework;
using MonoGame.Extended.Timers;
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
        public List<Component> components;

        public int priority;
        public Vector2 position;

        public GameObject() { }
        public GameObject(Vector2 _position)
        {
            position = _position;
        }

        public virtual void Load() { }

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

        public static T FindObjectOfType<T>() where T : GameObject
            => FindObjectsOfType<T>().First();

        public static T[] FindObjectsOfType<T>() where T : GameObject
        {
            var gobjs = SceneManager.instance.scene.GetGameObjects();
            var validTypes = gobjs.Where(x => x.GetType().IsAssignableFrom(typeof(T))).Cast<T>();

            return validTypes.ToArray();
        }
    }
}
