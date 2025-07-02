using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpgGame.Structure
{
    public class Component
    {
        public Vector2 position { get { return gameObject.position; } set {  gameObject.position = value; } }
        public GameObject gameObject;
        public virtual void Awake() { }
        public virtual void Start() { }
        public virtual void Update(GameTime gametime) { }
        public virtual void Unload() { }
    }
}
