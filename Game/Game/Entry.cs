using System;
using ThatOneGame.Structure;

public static class Entry
{
    public static void Main(string[] args)
    {
        Globals.initialArgs = args;
        using var game = new ThatOneGame.Engine();
        game.Run();
    }
}