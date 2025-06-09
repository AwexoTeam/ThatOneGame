using RpgGame.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpgGame.Commands
{
    public class HealCommand : Command
    {
        public override string command => "heal";
        public override string[] aliases => new string[] { "h" };

        public override bool CanExecute(string[] args, out string output)
        {
            output = "Successfully healed player fully!";
            return true;
        }

        public override void Run(string[] args)
        {
            Player.player.Heal(-1);
        }
    }
}
