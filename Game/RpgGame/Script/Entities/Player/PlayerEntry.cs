using Gum.Converters;
using Gum.DataTypes;
using Gum.Wireframe;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame;
using MonoGameGum;
using MonoGameGum.Forms.Controls;
using Newtonsoft.Json;
using RpgGame.Components;
using RpgGame.Managers;
using RpgGame.Script.Components;
using RpgGame.Script.Manager;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Timers;

namespace RpgGame.Structure
{
    public partial class Player : Component, IRenderable
    {
        protected int animationTick;
        protected string basePath;
        protected string baseName;

        //Collision Specific
        
        protected Rectangle tileTarget;

        //Character Sprite Specific
        protected Dictionary<EntityState, Texture2D> stateTextures;

        protected Vector2 direction;
        protected Direction dir;
        protected Direction lastDir;

        protected EntityState state;

        public static Player instance;

        public static Player player;

        public bool godMode;
        public bool blockInput;
        private float speed = 50;

        private bool hasInit = false;
        private Texture2D swordTexture;

        private Map map;
        private TextBox console;
        public Entity entity;
        
        public int priority;

        private Renderer renderer;
        public BoxCollider collisionBox;
        public BoxCollider hitBox;

        public Player() : base()
        {
            instance = this;

            player = this;
            
            basePath = AppDomain.CurrentDomain.BaseDirectory;
            basePath += "..\\Tiles\\Sprite Pack - New\\16x16\\Base\\Base Character PNG\\";

            baseName = "Base";
            var swordPath = basePath + @"..\Base Tools PNG\Base Attack (One Hand Weapons)\Base Sword\Base Sword 01.png";
            swordTexture = Texture2D.FromFile(RenderManager.graphics, swordPath);

            PlayerEntityInit();
        }

        public void PlayerEntityInit()
        {
            entity = new Entity();
            entity.Initialize("Player");

            string json = File.ReadAllText("Data/Player.json");
            entity = JsonConvert.DeserializeObject<Entity>(json);
            entity.AdjustStats();
        }

        public override void Start()
        {
            Timer timer = new Timer(250f);
            timer.Elapsed += Timer_Elapsed;
            timer.Start();

            renderer = gameObject.GetComponent<Renderer>();

            stateTextures = new Dictionary<EntityState, Texture2D>();

            foreach (var state in Enum.GetValues(typeof(EntityState)).Cast<EntityState>())
            {
                Texture2D _texture = null;
                if (baseName == "NILL")
                {

                    _texture = Texture2D.FromFile(RenderManager.graphics, $"{basePath}");
                    stateTextures.Add(state, _texture);
                    continue;
                }

                _texture = Texture2D.FromFile(RenderManager.graphics, $"{basePath}//{baseName} {state}.png");
                stateTextures.Add(state, _texture);
            }

            console = new TextBox();
            console.Visual.WidthUnits = DimensionUnitType.PercentageOfParent;
            console.Visual.XUnits = GeneralUnitType.Percentage;
            console.X = 20;
            console.Width = 50;
            console.Height = 40;
            
            console.Anchor(Anchor.Bottom);
        }


        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            TickAnimation();
        }

        public override void Update(GameTime gameTime)
        {
            UpdateCamera();

            if (DoErrorChecking())
                return;

            renderer.texture = stateTextures[state];

            if (DoConsoleUpdate())
                return;

            direction = Vector2.Zero;
            if (Input.IsKeyDown(Keys.W)) direction.Y--;
            if (Input.IsKeyDown(Keys.A)) direction.X--;
            if (Input.IsKeyDown(Keys.S)) direction.Y++;
            if (Input.IsKeyDown(Keys.D)) direction.X++;

            state = direction == Vector2.Zero ? EntityState.Idle : EntityState.Walk;
            
            SetDirection();
            renderer.tileX = animationTick;
            renderer.tileY = (int)dir;

            if (Collision() && hasInit)
                return;

            Move(direction, speed);

            if (!hasInit)
                hasInit = true;
        }

        private bool Collision()
        {
            
            var tilesCollision = map.tiles.Where(x => Tile.GetCollisionRects(x) != null);
            var collisions = tilesCollision.SelectMany(x => Tile.GetCollisionRects(x));

            Point location = new Point((int)position.X, (int)position.Y);
            Point size = new Point(16, 16);

            int num = 1;

            location.X += (int)(direction.X * num);
            location.Y += (int)(direction.Y * num);
            tileTarget = new Rectangle(location, size);

            if(direction.X == 0 || direction.Y == 0)
                return collisions.Any(x => x.Intersects(tileTarget));

            Rectangle xCollision = new Rectangle(tileTarget.X, (int)position.Y, 16, 16);
            Rectangle yCollision = new Rectangle((int)position.X, tileTarget.Y, 16, 16);

            bool xColliding = collisions.Any(x => x.Intersects(xCollision));
            bool yColliding = collisions.Any(x => x.Intersects(yCollision));

            if (xColliding && yColliding)
                return true;

            int x = xColliding ? 0 : (int)direction.X;
            int y = yColliding ? 0 : (int)direction.Y;

            direction = new Vector2(x, y);
            return false;
        }

        private bool DoErrorChecking()
        {
            if (map == null)
            {
                map = GameObject.FindObjectOfType<Map>();
                UpdateCamera();
                return true;
            }

            return false;
        }

        private bool isConsoleUp = false;
        public bool DoConsoleUpdate()
        {

            if (Input.IsKeyUp(Keys.Q))
            {
                if (isConsoleUp)
                {
                    console.RemoveFromRoot();
                    isConsoleUp = false;
                }
                else
                {
                    console.AddToRoot();
                    isConsoleUp = true;
                }


            }

            if (console.IsVisible)
            {
                if (Input.IsKeyUp(Keys.Enter))
                {
                    string err = "Successful";
                    CommandManager.instance.TryCommand(console.Text, out err);
                    console.Placeholder = err;

                    console.Text = string.Empty;
                }

                return blockInput;
            }

            return blockInput;
        }

        private void UpdateCamera()
        {
            var dx = Globals.RESOLUTION_WIDTH / 2 - gameObject.position.X;
            var dy = Globals.RESOLUTION_HEIGHT / 2 - gameObject.position.Y;

            RenderManager.transform = Matrix.CreateTranslation(dx, dy, 1);

        }

        public void Draw(SpriteBatch batch)
        {
            if (!Globals.DebugMode)
                return;

            batch.DrawRectangle(tileTarget, Color.Red);
        }

        public virtual void Move(Vector2 dir, float speed)
        {
            Vector2 directionNormalized = dir;
            if (dir != Vector2.Zero)
                directionNormalized.Normalize();

            gameObject.position += directionNormalized * speed * Input.DeltaTime;
            gameObject.position = Vector2.Round(gameObject.position);

        }

        public bool IsDead()
        {
            if (godMode)
                return false;

            return entity.isDead();
        }

        public void Heal(float val)
        {
            if (val < 0)
                val = entity.GetStat(Stats.Max_Hp);

            entity.ModifyStat(Stats.Hp, val);
        }

        protected void TickAnimation()
        {
            if (renderer == null)
                return;

            if (renderer.texture == null)
                return;

            animationTick++;
            if (animationTick * renderer.tileSize < renderer.texture.Width)
                return;

            animationTick = 0;
        }

        protected void SetDirection()
        {
            if (direction == Vector2.Zero)
                return;

            if (direction.Y < 0)
                dir = Direction.North;

            if (direction.Y > 0)
                dir = Direction.South;

            if (direction.X < 0)
                dir = Direction.West;

            if (direction.X > 0)
                dir = Direction.East;

            if (lastDir != dir)
            {
                lastDir = dir;
                animationTick = 0;
            }
        }

        public void PostDraw(SpriteBatch batch)
        {
            
        }
    }
}
