using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpgGame.Commands
{
    public abstract class Command
    {
        public abstract string command { get; }
        public virtual string[] aliases { get; }
        public abstract bool CanExecute(string[] args, out string output);
        public abstract void Run(string[] args);
    }
}
