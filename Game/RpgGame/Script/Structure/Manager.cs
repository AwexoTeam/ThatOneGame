using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpgGame.Script.Structure
{
    internal abstract class Manager
    {
        public abstract void Initialize();
        public virtual void Destroy() { }
    }
}
