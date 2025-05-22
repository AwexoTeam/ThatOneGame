using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpgGame.Structure
{
    public interface IHookable
    {
        public int priority { get; }
        public bool CanHook(IHookable[] hooks, out string error);
        public void Run();

    }
}
