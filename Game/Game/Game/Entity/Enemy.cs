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
        public string defeatScene;
        public string texturePath;
        public Entity entity;
        public Dictionary<string, int> skills;

        public static string GetDefaultJson()
        {
            Enemy enemy = new Enemy();
            enemy.entity = new Entity();
            enemy.entity.Initialize("Default");
            enemy.defeatScene = "Some Path";
            enemy.skills = new Dictionary<string, int>();

            enemy.skills.Add("Normal Attack", 100);
            enemy.skills.Add("Defence", 5);

            return JsonConvert.SerializeObject(enemy, Formatting.Indented);
        }
        public bool isNull() => this == null;
        
        public void TickAI()
        {
            Player.player.entity.DoDamage(this.entity);
            entity.RegenTick();
            return;
        }

        public void DamageByPlayer()
        {
            entity.DoDamage(Player.player.entity);
        }
    }
}
