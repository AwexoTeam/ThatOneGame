using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThatOneGame.Structure;

namespace ThatOneGame.GameCode
{
    public class PlayerRefreshCommand : Command
    {
        public override string command => "playerrefresh";
        public override string[] aliases => new string[] { "pr" };

        public override bool CanExecute(string[] args, out string output)
        {
            output = "Player Refresh Completed";
            return true;
        }

        public override void Run(string[] args)
        {
            Player.player.PlayerEntityInit();
        }
    }
}
