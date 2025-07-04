using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame;
using Newtonsoft.Json;
using RpgGame.Components;
using RpgGame.Entities;
using RpgGame.Script.Components.UI.BattleUI;
using RpgGame.Script.Structure;
using RpgGame.Structure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RpgGame.Managers
{
    

    public class BattleManager : IHookable
    {
        public static BattleManager instance { get; set; }

        public int priority => throw new NotImplementedException();

        public List<Enemy> enemies = new List<Enemy>();
        public List<Enemy> enemyDataBase = new List<Enemy>();
        public int selectedEnemy;
        public bool isInBattle = false;

        public bool isPlayerTurn;

        public bool CanHook(IHookable[] hooks, out string error) { error = "Success"; return true; }

        public void Run()
        {
            if (instance != null)
                return;

            string path = "Data/Enemies/";
            string defaultTxt = Enemy.GetDefaultJson();
            File.WriteAllText(path + "default.json", defaultTxt);

            instance = this;
            var enemyFiles = Directory.GetFiles(path);
            foreach (var file in enemyFiles)
            {
                if (file.Contains("default.json"))
                    continue;

                var text = File.ReadAllText(file);
                var enemy = JsonConvert.DeserializeObject<Enemy>(text);

                enemyDataBase.Add(enemy);
            }

            EventManager.OnEngineInitizile += PostInit;
        }

        private void PostInit()
        {
            Engine.updateCalls.Add(Update);
        }

        private void Update(GameTime gameTime)
        {
            if (enemies.Count == 0)
                return;

            if (Player.instance.IsDead())
            {
                enemies.Clear();
                enemies = new List<Enemy>();
                Debug.LogVerbose("Player DIED");
                return;
            }

            bool allDead = enemies.TrueForAll(x => x.isDead());
            
            if (enemies.Count <= 0 && isInBattle || allDead)
            {
                MainPlayerUI.instance.ClearMiddleWindow();
                isInBattle = false;
                Debug.LogVerbose("Player WON");

                enemies.Clear();
                return;
            }

            if (isPlayerTurn)
                return;

            foreach (var enemy in enemies)
                enemy.TickAI();

            Debug.LogDebug("Player has " + Player.instance.entity.GetStat(Stats.Hp));
            isPlayerTurn = true;
        }

        public void StartBattle(string qualifier, int amount)
        {
            int id = 0;
            Enemy enemy = enemyDataBase.Find(x => x.name.ToLower() == qualifier.ToLower());
            if (int.TryParse(qualifier, out id))
                enemy = enemyDataBase[id];

            string path = "Data/Enemies/";
            Debug.LogDebug("Starting battle x" + amount + " " + enemy.name);
            for (int i = 0; i < amount; i++)
            {
                var text = File.ReadAllText(path + enemy.name + ".json");
                var instancedEnemy = JsonConvert.DeserializeObject<Enemy>(text);

                enemies.Add(instancedEnemy);
            }

            BattleUI.SetUIToBattle();
            BattleUI.UpdateUI();

            isInBattle = true;
        }

        public void ChangeSelected(int id)
        {
            if (enemies[id].isDead())
                return;

            Debug.LogDebug("Setting selection to " + id);
            selectedEnemy = id;
            BattleUI.UpdateUI();
        }

        public void ExecuteAttack(int id)
        {
            if (!isInBattle)
                return;

            Enemy target = enemies[selectedEnemy];
            target.DoDamage(Player.instance.entity);

            Debug.LogDebug("Enemy has " + target.GetStat(Stats.Hp));
            BattleUI.UpdateUI();

            isPlayerTurn = false;

            if (!target.isDead())
                return;

            selectedEnemy = 0;
            BattleUI.UpdateUI();
        }
    }
}
