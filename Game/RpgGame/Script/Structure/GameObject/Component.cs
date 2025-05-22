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
        public virtual void Start() { }
        public virtual void Update(GameTime gametime) { }
        public virtual void Unload() { }
    }
}
