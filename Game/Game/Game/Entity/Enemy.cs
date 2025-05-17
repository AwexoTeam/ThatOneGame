using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using ThatOneGame.Structure;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ThatOneGame.GameCode
{
    public class Enemy
    {
        public string name;
        public string defeatScene;
        public Dictionary<Stats, float> stats;
        public Dictionary<string, int> skills;

        public Enemy()
        {

        }

        public void AdjustStats()
        {
            var values = Enum.GetValues(typeof(Stats)).Cast<Stats>();
            foreach (var stat in values)
            {
                if (stats.ContainsKey(stat))
                    continue;

                stats.Add(stat, 1);
            }

            stats[Stats.Hp] = stats[Stats.Max_Hp];
            stats[Stats.Mp] = stats[Stats.Max_Mp];
            stats[Stats.Sp] = stats[Stats.Max_Sp];

        }

        public static string GetDefaultJson()
        {
            Enemy enemy = new Enemy();
            enemy.stats = new Dictionary<Stats, float>();
            enemy.skills = new Dictionary<string, int>();

            enemy.name = "Default";
            enemy.defeatScene = "None";

            var values = Enum.GetValues(typeof(Stats)).Cast<Stats>();
            foreach (var stat in values)
                enemy.stats.Add(stat, 1);

            enemy.skills.Add("Default1", 1);
            enemy.skills.Add("Default2", 10);

            return JsonConvert.SerializeObject(enemy, Formatting.Indented);
        }
        public bool isNull() => this == null;
        public bool isDeadh() => stats[Stats.Hp] <= 0;

        public void TickAI()
        {
            Player.player.DoDamage(this);
            return;
        }

        public void DamageByPlayer()
        {
            stats[Stats.Hp] -= 10;
        }
    }
}
