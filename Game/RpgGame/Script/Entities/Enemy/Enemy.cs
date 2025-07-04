using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using RpgGame.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpgGame.Entities
{
    public class Enemy : Entity
    {
        public string name;
        public string defeatScene;
        public string texturePath;
        
        public Dictionary<string, int> skills;

        public static string GetDefaultJson()
        {
            Enemy enemy = new Enemy();
            enemy.Initialize("Default");
            enemy.defeatScene = "Some Path";
            enemy.skills = new Dictionary<string, int>();

            enemy.skills.Add("Normal Attack", 100);
            enemy.skills.Add("Defence", 5);

            return JsonConvert.SerializeObject(enemy, Formatting.Indented);
        }
        public bool isNull() => this == null;

        public void TickAI()
        {
            Player.player.entity.DoDamage(this);
            RegenTick();
            return;
        }

        public void DamageByPlayer()
        {
            DoDamage(Player.player.entity);
        }
    }
}
