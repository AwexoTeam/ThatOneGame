using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThatOneGame.Script.UI;
using ThatOneGame.Structure;
using ThatOneGame.Structure.UI;

namespace ThatOneGame.GameCode
{
    public class BattleManager : Manager
    {
        public static BattleManager instance;
        
        private Enemy[] enemies;
        private List<EnemyUI> enemiesUIs;
        private bool isPlayerTurn;
        private bool isActive;

        private HorizontalLayout enemyContainer;
        private UIButton attackButton;

        public int selectedIndex;

        public override void Initialize()
        {
            if (instance != null)
                return;

            instance = this;

            string path = "Templates/{0}.json";
            
            File.WriteAllText(string.Format(path, "EnemyTemplate"), Enemy.GetDefaultJson());
            File.WriteAllText(string.Format(path, "EntityTemplate"), Entity.GetDefaultJson());

            
        }

        public bool IsValidBattle(string battleFileName, out string error)
        {
            var files = Directory.GetFiles("Data/Enemies/");
            var file = Array.Find(files, x => Path.GetFileNameWithoutExtension(x) == battleFileName);

            if (file == null)
            {
                error = "There is no such file such as " + battleFileName + ".json";
                Debug.LogWarning(error);
                return false;
            }

            error = "";
            return true;
        }
        public void StartBattle(string battleFileName, int amount, out string error)
        {
            var files = Directory.GetFiles("Data/Enemies/");
            if (files.Length <= 0)
            {
                error = "There is no enemy files";
                Debug.LogWarning(error);
                return;
            }

            if (!IsValidBattle(battleFileName, out error))
                return;

            error = "";
            var file = Array.Find(files, x => Path.GetFileNameWithoutExtension(x) == battleFileName);
            InitiateBattle(file, amount);
        }
        public void StartBattle(int id, int amount, out string error)
        {
            var files = Directory.GetFiles("Data/Enemies/");
            if (files.Length <= 0)
            {
                error = "There is no enemy files";
                Debug.LogWarning(error);
                return;
            }

            error = "";
            InitiateBattle(files[id], amount);
        }
        private void InitiateBattle(string file, int amount)
        {
            enemiesUIs = new List<EnemyUI>();
            var jsonStr = File.ReadAllText(file);
            
            Player.player.blockInput = true;
            isActive = true;
            isPlayerTurn = true;

            enemyContainer = new HorizontalLayout(Point.Zero, new Point((int)Globals.screenSize.X, 300), Color.White, UIAnchor.Middle, UIStretch.Width);
            enemyContainer.padding = 5;
            

            enemies = new Enemy[amount];

            List<UIElement> el = new List<UIElement>();
            for (int i = 0; i < amount; i++)
            {
                var enemy = JsonConvert.DeserializeObject<Enemy>(jsonStr);
                enemy.entity.AdjustStats();
                enemies[i] = enemy;

                var enemyUI = new EnemyUI(enemy, Point.Zero, new Point(100, 100), Color.Cyan);
                enemyUI.barSize = 10;
                el.Add(enemyUI);
                
                enemyUI.id = i;
                enemyUI.onClick = () => { BattleManager.instance.selectedIndex = enemyUI.id; };
                enemyUI.Adjust();
                enemiesUIs.Add(enemyUI);
            }

            enemyContainer.AddRange(el.ToArray());
            attackButton = new UIButton(new Point(0, 0), new Point(300, 80), Color.White, UIAnchor.BottomMiddle);
            attackButton.onClick = OnAttackClick;
            attackButton.Adjust();
            enemyContainer.Adjust();
            
            //TODO: THIS SHOULDNT BE NESSACRY
            EventManager.Invoke(EventManagerTypes.WindowSizeChanged, null);
        }
   
        public override void Update(GameTime gameTime)
        {
            if (!isActive)
                return;

            if (isActive && !Array.Exists(enemies, x => !x.entity.isDead()))
            {
                isActive = false;
                Player.player.blockInput = false;
                isPlayerTurn = false;
                return;
            }

            if (enemies[selectedIndex] == null || enemies[selectedIndex].entity.isDead())
                selectedIndex = Array.FindIndex(enemies, e => e != null && e.entity.isDead());

            attackButton.color = isPlayerTurn ? Color.White : Color.Gray;

            if (!isPlayerTurn)
            {
                foreach (var enemy in enemies)
                {
                    enemy.TickAI();
                    isPlayerTurn = true;
                    if (Player.player.IsDead())
                    {
                        isActive = false;
                        Player.player.blockInput = false;
                        isPlayerTurn = false;

                        //TODO: REMOVE HEALING???
                        Player.player.Heal(-1);

                        Debug.LogVerbose("Defeated by " + enemy.entity.name + " Should play " + enemy.defeatScene + " scene.");
                        break;
                    }
                }
                
                return;
            }

            attackButton.CheckAndExecute();
            enemiesUIs.ForEach(x => x.CheckAndExecute());
        }

        public override void DrawUI(SpriteBatch batch)
        {
            if (!isActive)
                return;

            base.DrawUI(batch);
            batch.FillRectangle(Vector2.Zero, Globals.screenSize, Color.Gray);
            enemyContainer.Draw(batch);

            attackButton.Draw(batch);
            batch.DrawString(Globals.font, "ATTACK", new Vector2(attackButton.rect.X, attackButton.rect.Y), Color.Black);

            Vector2 playerHpLocation = new Vector2(attackButton.rect.X, attackButton.rect.Y);
            Vector2 playerHpSize = new Vector2(300, 40);

            playerHpLocation.Y -= playerHpSize.Y;
            playerHpLocation.Y -= 5;

            float max = Player.player.entity.GetStat(Stats.Max_Hp);
            float curr = Player.player.entity.GetStat(Stats.Hp);
            
            float percent = 1-(curr / max);
            float num = playerHpSize.X * (1 - percent);

            Vector2 playerHpSizeFront = new Vector2(num, playerHpSize.Y);

            batch.FillRectangle(playerHpLocation, playerHpSize, Color.Red);
            batch.FillRectangle(playerHpLocation, playerHpSizeFront, Color.Green);
        }

        private void OnAttackClick()
        {
            if (!isPlayerTurn)
                return;

            enemies[selectedIndex].DamageByPlayer();
            Player.player.entity.RegenTick();
            isPlayerTurn = false;
        }
    }
}
