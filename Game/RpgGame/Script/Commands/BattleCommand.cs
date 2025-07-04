using RpgGame.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpgGame.Commands
{
    public class BattleCommand : Command
    {
        public override string command => "battle";
        public override string[] aliases => new string[] { "b" };

        public override bool CanExecute(string[] args, out string output)
        {
            output = "Initiated battle with {0}x {1} successfully!";
            if (args.Length <= 1)
            {
                output = "Not enough arguments. Need id or name then amount.";
                return false;
            }

            string identifier = args[0];
            string amountStr = args[1];

            int id;
            int amount;

            if (!int.TryParse(amountStr, out amount))
            {
                output = amountStr + " is not a valid number.";
                return false;
            }

            output = string.Format(output, amount, identifier);
            bool isById = int.TryParse(identifier, out id);
            if (isById)
                return true;

            return true;
        }

        public override void Run(string[] args)
        {
            string identifier = args[0];
            string amountStr = args[1];

            int amount = int.Parse(amountStr);
            
            BattleManager.instance.StartBattle(identifier, amount);
        }
    }
}
