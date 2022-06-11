using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

using TiledSharp;
namespace Platformer_02
{
    class Player
    {
        private SpriteSheet spriteSheet;

        private Object obj;

        private const int speed = 120;
        private const int jumpHeight = 260;

        private const float jumpPressGravityMultiplier = 1.5f, jumpUnPressGravityMultiplier = 5f;

        bool rightTrueleftFalse;

        ParticleImage firstFoot, secondFoot, landing;

        bool canWalk = true;

        float time1, time2;

        
        public Player(ContentManager Content, TileMapManager tileMapManager)
        {
            spriteSheet = new SpriteSheet(Content.Load<Texture2D>("Sheet_Player"), 
                32, 
                32,
                new Dictionary<string, (int, int)> { { "null", (0, 0) }, { "idleL", (2, 3) }, { "idleR", (4, 5) }, {"runR", (6, 13) }, {"runL", (14, 21) }, {"jumpRStart", (22, 23) },{"jumpRMid", (24, 26) }, {"jumpREnd", (27, 29) }, {"jumpL", (30, 37) } });


            List<Rectangle> collisionObjectsInMap = new List<Rectangle>();
            foreach(var b in tileMapManager.map.ObjectGroups["Collision"].Objects)
            {
                collisionObjectsInMap.Add(new Rectangle((int)b.X, (int)b.Y, (int)b.Width, (int)b.Height));
            }

            obj = new Object(spriteSheet, 1.2f, new Collision(new Rectangle(10, 7, 13, 26), collisionObjectsInMap, 10, 7));

            firstFoot = new ParticleImage(Content.Load<Texture2D>("ParticleImage_PlayerFoot1"), 32);
            secondFoot = new ParticleImage(Content.Load<Texture2D>("ParticleImage_PlayerFoot2"), 32);
            landing = new ParticleImage(Content.Load<Texture2D>("Particleimage_PlayerLanding"), 32);

        }

        public void Update(GameTime gameTime)
        {
            //Walking particles
            if (obj.inAir == false)
            {
                if (spriteSheet.currentSprite == 20 || spriteSheet.currentSprite == 8)
                {
                    secondFoot.Instantiate(obj.pos, 40);
                }
                else if (spriteSheet.currentSprite == 16 || spriteSheet.currentSprite == 12)
                {
                    firstFoot.Instantiate(obj.pos, 40);
                }
            }
            //Movement
            if (Keyboard.GetState().IsKeyDown(Keys.Right) && !obj.inAir && canWalk)
            {
                rightTrueleftFalse = true;
                obj.initialVelocityX = speed;
                SetCurrentAnim("runR", 50, false);
            }
            else if(!Keyboard.GetState().IsKeyDown(Keys.Left) && rightTrueleftFalse && !obj.inAir && canWalk)
            {
                obj.initialVelocityX = 0;
                SetCurrentAnim("idleR", 300, false);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Left) && !obj.inAir && canWalk)
            {
                rightTrueleftFalse = false;
                obj.initialVelocityX = -speed;
                SetCurrentAnim("runL", 50, false);
            }
            else if(!Keyboard.GetState().IsKeyDown(Keys.Right) && !rightTrueleftFalse && !obj.inAir && canWalk)
            {
                obj.initialVelocityX = 0;
                SetCurrentAnim("idleL", 300, false);
            }

            if (obj.inAir)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Right))
                {
                    rightTrueleftFalse = true;
                    obj.initialVelocityX = Vector2.Lerp(new Vector2(obj.initialVelocityX, 0f), new Vector2(speed), 0.9f).X;
                    SetCurrentAnim("runR", 50, false);
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Left))
                {
                    rightTrueleftFalse = false;
                    obj.initialVelocityX = Vector2.Lerp(new Vector2(obj.initialVelocityX, 0f), new Vector2(-speed), 0.9f).X;
                    SetCurrentAnim("runL", 50, false);
                }
                else
                {
                    obj.initialVelocityX = 0;
                    SetCurrentAnim("idleL", 300, false);
                }
            }
            //Jumping
            bool beforeInAir = obj.inAir;
            Jump(gameTime);
            //Update object
            obj.Update(gameTime);
            if(obj.inAir != beforeInAir)
            {
                landing.Instantiate(obj.pos, 50);
                spriteSheet.setSprite(rightTrueleftFalse ? 28 : 36);
               
                
            }
            if(gameTime.TotalGameTime.TotalMilliseconds - time1 > 100 && time1 !=0)
            {
                canWalk = true;
            }
        }

        private void Jump(GameTime gameTime)
        {
            keyboard.GetState();
           
            if (keyboard.HasBeenPressed(Keys.Z) && !obj.inAir)
            {
                time2 = (float)gameTime.TotalGameTime.TotalMilliseconds;
                spriteSheet.setSprite(rightTrueleftFalse ? 22 : 30);
                canWalk = false;
            }
            else if(gameTime.TotalGameTime.TotalMilliseconds - time2 > 50 && time2 != 0)
            {
                obj.initialVelocityY -= jumpHeight;
                obj.inAir = true;

                time2 = 0;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Z) && obj.initialVelocityY < 0)
            {
                obj.gravityIncrease = jumpPressGravityMultiplier * 6;
            }
            else
            {
                obj.gravityIncrease = Vector2.Lerp(new Vector2(0, obj.gravityIncrease), new Vector2(0, jumpUnPressGravityMultiplier), 1f).Y * 6;

            }

            if (obj.inAir)
            {
                spriteSheet.setSprite(FindCorrectSprite(obj.initialVelocityY, gameTime));
            }
            
            
        }
        private int FindCorrectSprite(float velocityY, GameTime gameTime)
        {
            float x = jumpHeight / 4;

            Debug.WriteLine(obj.initialVelocityY);
            if(velocityY <= -jumpHeight + x * 1 && velocityY > -jumpHeight)
            {
                return rightTrueleftFalse ? 23 : 31;
            }
            else if (velocityY <= -jumpHeight + x * 2&& velocityY > -jumpHeight + x * 1)
            {
                return rightTrueleftFalse ? 24 : 32;
            }
            else if (velocityY <= -jumpHeight + x * 5 && velocityY > -jumpHeight + x *2)
            {
                return rightTrueleftFalse ? 25 : 33;
            }
            else  if (velocityY <= -jumpHeight + x * 8 && velocityY > -jumpHeight + x * 4)
            {
                return rightTrueleftFalse ? 26 : 34;
            }
            else
            {
                time1 = (float)gameTime.TotalGameTime.TotalMilliseconds;
                return rightTrueleftFalse ? 27 : 35;
            }
           
           
          
            
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            obj.Draw(spriteBatch);
        }
        public void SetCurrentAnim(string name, int speedMS, bool waitForEnd)
        {
            spriteSheet.setAnim(name, speedMS, waitForEnd, false);
        }
    }
}
