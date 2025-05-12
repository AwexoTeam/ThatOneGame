using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame;
using System;
using System.Timers;
using System.Collections.Generic;
using System.Linq;
using ThatOneGame;
using ThatOneGame.GameCode;

namespace ThatOneGame.Structure
{
    public class GameObject
    {
        public int priority;
        public Vector2 position;

        public GameObject() { }
        public GameObject(Vector2 _position)
        {
            position = _position;
        }

        public virtual void Load() { }

        public virtual void Start() { }

        public virtual void Update(GameTime gameTime) { }

        public virtual void Unload() { }

        public virtual void Move(Vector2 dir, float speed)
        {
            Vector2 directionNormalized = dir;
            if (dir != Vector2.Zero)
                directionNormalized.Normalize();

            position += directionNormalized * speed * Engine.deltaTime;
            position = Vector2.Round(position);

        }

        public static GameObject Instantiate(GameObject gameObject)
        {
            ScreenManager.instance.screen.AddGameObject(gameObject);
            return gameObject;
        }

        public static T FindObjectOfType<T>() where T : GameObject
            => FindObjectsOfType<T>().First();

        public static T[] FindObjectsOfType<T>() where T : GameObject
        {
            var gobjs = ScreenManager.instance.screen.GetGameObjects();
            var validTypes = gobjs.Where(x => x.GetType().IsAssignableFrom(typeof(T))).Cast<T>();

            return validTypes.ToArray();
        }
    }
}
