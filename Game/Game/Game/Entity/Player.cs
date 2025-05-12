using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame;
using System;
using System.Timers;
using System.Collections.Generic;
using System.Linq;
using ThatOneGame.Structure;
using ThatOneGame;

namespace ThatOneGame.GameCode
{
    public class Player : EntityBase
    {
        public static Player player;

        public bool godMode;
        public bool blockInput;
        private float speed = 50;
        
        private bool hasInit = false;
        private Texture2D swordTexture;

        public Player() : base ()
        {
            player = this;
            tileSize = 80;

            basePath = AppDomain.CurrentDomain.BaseDirectory;
            basePath += "..\\Tiles\\Sprite Pack - New\\16x16\\Base\\Base Character PNG\\";

            baseName = "Base";
            var swordPath = basePath + @"..\Base Tools PNG\Base Attack (One Hand Weapons)\Base Sword\Base Sword 01.png";
            swordTexture = Texture2D.FromFile(Engine.batch.GraphicsDevice, swordPath);
        }
        
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (player.blockInput)
                return;

            if (Input.IsMouseUp(0))
            {
                animationTick = 0;
                state = EntityState.Attack;
                return;
            }

            if (state == EntityState.Attack)
            {
                tileX = animationTick;

                if (animationTick != 5)
                {
                    float div = animationTick;
                    float t = div / 5f;
                    return;
                }

                animationTick = 0;
                state = EntityState.Idle;
            }

            direction = Vector2.Zero;
            if (Input.IsKeyDown(Keys.W)) direction.Y--;
            if (Input.IsKeyDown(Keys.A)) direction.X--;
            if (Input.IsKeyDown(Keys.S)) direction.Y++;
            if (Input.IsKeyDown(Keys.D)) direction.X++;

            state = direction == Vector2.Zero ? EntityState.Idle : EntityState.Walk;

            SetDirection();
            tileX = animationTick;
            tileY = (int)dir;

            var tiles = ScreenManager.instance.screen.map.tiles.FindAll(x => x.destination.Intersects(collisionBox));

            if (tiles.Count <= 0)
                return;

            Vector2 collisionDir = Vector2.Zero;
            bool isColling = false;
            foreach (var tile in tiles)
            {
                if (!IsCollidingWith(tile, out collisionDir))
                    continue;

                isColling = true;
                break;
            }

            if (isColling && hasInit)
            {
                direction = new Vector2((int)direction.X, (int)direction.Y);
                return;
            }

            UpdateCamera();
            Move(direction, speed);

            if (!hasInit)
                hasInit = true;
        }

        private void UpdateCamera()
        {
            var dx = Globals.RESOLUTION_WIDTH / 2 - position.X;
            var dy = Globals.RESOLUTION_HEIGHT / 2 - position.Y;

            Engine.transform = Matrix.CreateTranslation(dx, dy, 1);

        }

        protected override void PreDraw(SpriteBatch batch)
        {
            texture = stateTextures[state];
            base.PreDraw(batch);
        }

        public override void Draw(SpriteBatch batch)
        {
            base.Draw(batch);
            if (state == EntityState.Attack)
            {
                batch.Draw(swordTexture, destination, sourceRect, Color.White);
            }

            if (!Globals.DebugMode)
                return;

            batch.DrawRectangle(hitbox, Color.Pink);
            batch.DrawRectangle(collisionBox, Color.Blue);
            batch.DrawRectangle(tileTarget, Color.Red);
        }
    }
}