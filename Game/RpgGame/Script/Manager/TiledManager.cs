using RpgGame.Structure;
using System.IO;
using System;
using TiledSharp;
using System.Linq;

namespace RpgGame.Managers
{
    public static class TiledManager
    {
        public static Map GetMap(string path)
        {
            string mapPath = GetFullMapPath(path);
            Map map = new Map("Test", mapPath);
            
            return map;
        }

        private static string GetFullMapPath(string mapPath)
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            if (Globals.initialArgs == null || Globals.initialArgs.Length <= 0)
                mapPath = Map.GetFirstMap(basePath);
            else
            {
                Debug.LogDebug("Starting up map " + Globals.initialArgs[0]);
                mapPath = Globals.initialArgs[0];
            }

            return mapPath;
        }
    }
}
