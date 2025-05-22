using RpgGame.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RpgGame.Structure
{
    public static class Debug
    {
        public static List<LogLevel> logLevels = new List<LogLevel>();


        public static void LogInfo(object info) => Log(LogLevel.Info, info, "NILL", -1);

        public static void LogVerbose(object info) => Log(LogLevel.Verbose, info, "NILL", -1);

        public static void LogDebug(object info, [CallerFilePath] string callerMember = "", [CallerLineNumber] int line = -1)
        {
            Log(LogLevel.Debug, info, callerMember, line);
        }

        public static void LogWarning(object info, [CallerFilePath] string callerMember = "", [CallerLineNumber] int line = -1)
        {
            Log(LogLevel.Warning, info, callerMember, line);
        }

        public static void LogError(object info, [CallerFilePath] string callerMember = "", [CallerLineNumber] int line = -1)
        {
            Log(LogLevel.Error, info, callerMember, line);
        }

        private static void Log(LogLevel level, object info, string callerName, int callerLine)
        {
            ConsoleColor consoleColor = GetColor(level);
            string infoBracketText = GetInfoBracket(level, callerName, callerLine);

            Console.ForegroundColor = consoleColor;
            Console.WriteLine(infoBracketText + " " + info.ToString());
            Console.ForegroundColor = ConsoleColor.White;
        }

        private static string GetInfoBracket(LogLevel level, string callerName, int callerLine)
        {
            if (level == LogLevel.Info || level == LogLevel.Verbose)
                return $"[{DateTime.Now.ToString("HH:mm:ss")}]";

            callerName = Path.GetFileNameWithoutExtension(callerName);
            return $"[{callerName} L:{callerLine}]";
        }

        private static ConsoleColor GetColor(LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Verbose:
                    return ConsoleColor.Gray;
                case LogLevel.Warning:
                    return ConsoleColor.Yellow;
                case LogLevel.Error:
                    return ConsoleColor.Red;
                case LogLevel.Debug:
                    return ConsoleColor.DarkGray;
                default:
                    return ConsoleColor.White;
            }
        }
    }
}