﻿using RpgGame.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpgGame.Commands
{
    public class DebugModeCommand : Command
    {
        public override string command => "debugmode";

        public override bool CanExecute(string[] args, out string output)
        {
            output = command + " ran successfully";
            return true;
        }

        public override void Run(string[] args)
        {
            Globals.DebugMode = !Globals.DebugMode;
        }
    }
}
