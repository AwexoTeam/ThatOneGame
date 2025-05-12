using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThatOneGame.Structure;

namespace ThatOneGame.GameCode
{
    public class CommandManager : Manager
    {
        public static CommandManager instance;
        public Command[] commands;

        public override void Initialize()
        {
            if (instance != null)
                return;

            instance = this;

            commands = Utils.GetAllTypes<Command>();
        }

        public bool TryCommand(string text, out string output)
        {
            output = "NILL";
            if (text == null)
                return false;

            if (text == string.Empty)
                return false;

            string[] args = text.Split(" ");
            string cmdName = args[0];
            cmdName = cmdName.ToLower();

            var cmd = Array.Find(commands, x => x.command == cmdName);
            if (cmd == null)
            {
                output = cmdName + " does not exist.";
                Debug.LogWarning(cmdName + " does not exist.");
                return false;
            }

            args = args.Skip(1).ToArray();
            if (!cmd.CanExecute(args, out output))
            {
                Debug.LogWarning(output);
                return false;
            }

            cmd.Run(args);
            return true;
        }
    }
}
