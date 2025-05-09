
using ThatOneGame.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame;
using System;
using ThatOneGame.Structure.JsonObjects;
using System.Timers;
using System.Collections.Generic;
using System.Linq;

namespace ThatOneGame.Structure
{
    public enum Direction
    {
        North,
        South,
        West,
        East,

    }

    public enum PlayerState
    {
        Idle,
        Walk,
        Axe,
        Death,
    }

    public class Player
    {
        public Vector2 position;
        
        public int animationTick;

        public int tileX;
        public int tileY;

        private float speed = 50;
        private Vector2 minPos, maxPos;
        private int tileSize = 80;
        private Vector2 direction;
        private Direction dir;
        private Direction lastDir;

        private Rectangle hitbox;
        private Rectangle collisionBox;
        private Rectangle tileTarget;

        private bool hasInit = false;
        public static bool debugMode = false;

        private KeyboardState lastState;
        private string baseName = "Base";
        private PlayerState currState;

        private Dictionary<PlayerState, Texture2D> stateTextures;

        public Player(Vector2 _position)
        {
            position = _position;

            hitbox = new Rectangle(0, 0, 16, 16);
            collisionBox = new Rectangle(0, 0, 16, 8);
        }

        public void Init()
        {
            stateTextures = new Dictionary<PlayerState, Texture2D>();

            var graphics = Engine.batch.GraphicsDevice;
            string basePath = System.AppDomain.CurrentDomain.BaseDirectory + "//";
            basePath += "Tiles\\Sprite Pack - New\\16x16\\Base\\Base Character PNG\\";
            foreach (var state in Enum.GetValues(typeof(PlayerState)).Cast<PlayerState>())
            {
                Texture2D texture = Texture2D.FromFile(graphics, $"{basePath}//{baseName} {state}.png");
                stateTextures.Add(state,texture);
            }

            Timer timer = new Timer(150);
            timer.Elapsed += AnimationTicked;
            timer.Start();
        }

        private void AnimationTicked(object sender, ElapsedEventArgs e)
        {
            animationTick++;
            Texture2D texture = stateTextures[currState];
            if (animationTick * tileSize < texture.Width)
                return;

            animationTick = 0;
        }

        public void Update()
        {
            var state = Keyboard.GetState();
            
            if (state.IsKeyDown(Keys.F10) && !lastState.IsKeyDown(Keys.F10))
                debugMode = !debugMode;

            direction = Vector2.Zero;
            if (state.IsKeyDown(Keys.W)) direction.Y--;
            if (state.IsKeyDown(Keys.A)) direction.X--;
            if (state.IsKeyDown(Keys.S)) direction.Y++;
            if (state.IsKeyDown(Keys.D)) direction.X++;

            currState = direction == Vector2.Zero ? PlayerState.Idle : PlayerState.Walk;

            SetDirection();
            tileX = animationTick;
            tileY = (int)dir;

            Vector2 directionNormalized = direction;
            if (direction != Vector2.Zero)
                directionNormalized.Normalize();

            hitbox.X = (int)position.X;
            hitbox.Y = (int)position.Y;

            collisionBox.X = hitbox.X;
            collisionBox.Y = hitbox.Y;

            collisionBox.X += (int)direction.X;
            collisionBox.Y += (int)direction.Y + 8;

            var tiles = SplashScreen.map.tiles.FindAll(x => x.destination.Intersects(collisionBox));

            if (tiles.Count <= 0)
            {
                lastState = state;
                return;
            }

            bool isColling = false;
            foreach (var tile in tiles)
            {
                if (!isCollising(tile))
                    continue;

                isColling = true;
                break;
            }

            if (isColling && hasInit)
            {
                lastState = state;
                return;
            }

            position += directionNormalized * speed * Engine.deltaTime;
            position = Vector2.Round(position);

            var dx = (Engine.RESOLUTION_WIDTH / 2) - position.X;
            var dy = (Engine.RESOLUTION_HEIGHT / 2) - position.Y;

            Engine.transform = Matrix.CreateTranslation(dx, dy, 1);

            if (!hasInit)
                hasInit = true;

            lastState = state;
        }

        private void SetDirection()
        {
            if (direction == Vector2.Zero)
                return;

            if (direction.Y < 0)
                dir = Direction.North;

            if (direction.Y > 0)
                dir = Direction.South;

            if (direction.X < 0)
                dir = Direction.West;

            if(direction.X > 0)
                dir = Direction.East;

            if (lastDir != dir)
            {
                lastDir = dir;
                animationTick = 0;
            }
        }

        public bool isCollising(Tile tile)
        {
            var tileCollisions = Tile.GetCollisionRects(tile);
            if (tileCollisions == null)
                return false;

            if (tileCollisions.Count <= 0)
                return false;

            foreach(var collision in tileCollisions)
            {
                if (collision.Intersects(collisionBox))
                    return true;
            }

            return false;
        }


        public void Draw(SpriteBatch batch)
        {
            Rectangle destination = new Rectangle((int)position.X, (int)position.Y, tileSize, tileSize);
            destination.X -= tileSize / 2 - 8;
            destination.Y -= tileSize / 2 - 8;

            int x = tileX * tileSize;
            int y = tileY * tileSize;

            int width = tileSize;
            int height = tileSize;

            Rectangle sourceRect = new Rectangle(x, y, width, height);

            Texture2D texture = stateTextures[currState];
            batch.Draw(texture, destination, sourceRect, Color.White);

            if (!debugMode)
                return;

            batch.DrawRectangle(hitbox, Color.Pink);
            batch.DrawRectangle(collisionBox, Color.Blue);
            batch.DrawRectangle(tileTarget, Color.Red);
        }
    }
}
