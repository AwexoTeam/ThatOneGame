using RpgGame.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpgGame.Commands
{
    public class ToggleGodModeCommand : Command
    {
        public override string command => "togglegodmode";
        public override string[] aliases => _aliases;
        private string[] _aliases = new string[] { "tgm" };

        public override bool CanExecute(string[] args, out string output)
        {
            output = command + " ran successfully";
            return true;
        }

        public override void Run(string[] args)
        {
            Player.player.godMode = !Player.player.godMode;
        }
    }
}
