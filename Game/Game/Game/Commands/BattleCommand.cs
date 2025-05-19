using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThatOneGame.Structure;

namespace ThatOneGame.GameCode
{
    public class BattleCommand : Command
    {
        public override string command => "battle";
        public override string[] aliases => new string[] { "b" };

        public override bool CanExecute(string[] args, out string output)
        {
            output = "Initiated battle with {0}x {1} successfully!";
            if(args.Length <= 1)
            {
                output = "Not enough arguments. Need id or name then amount.";
                return false;
            }

            string identifier = args[0];
            string amountStr = args[1];

            int id;
            int amount;

            if(!int.TryParse(amountStr, out amount))
            {
                output = amountStr + " is not a valid number.";
                return false;
            }

            output = string.Format(output, amount, identifier);
            bool isById = int.TryParse(identifier, out id);
            if (isById)
                return true;

            bool isValid = BattleManager.instance.IsValidBattle(identifier, out output);
            return isValid;
        }

        public override void Run(string[] args)
        {
            string identifier = args[0];
            string amountStr = args[1];

            int amount = int.Parse(amountStr);
            int id = 0;

            string error;

            if (int.TryParse(identifier, out id))
                BattleManager.instance.StartBattle(id, amount, out error);
            else
                BattleManager.instance.StartBattle(identifier, amount, out error);
        }
    }
}
