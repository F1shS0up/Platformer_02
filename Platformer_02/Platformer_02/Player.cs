using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
namespace Platformer_02
{
    class Player
    {
        private SpriteSheet spriteSheet;

        private Object obj;

        private const int speed = 100;
        bool rightTrueleftFalse;
        public Player(ContentManager Content)
        {
            spriteSheet = new SpriteSheet(Content.Load<Texture2D>("Sheet_Player"), 
                32, 
                32,
                new Dictionary<string, (int, int)> { { "null", (0, 0) }, { "idleL", (2, 5) }, { "idleR", (6, 9) }, {"runR", (10, 17) }, {"runL", (18, 25) } });

            obj = new Object(spriteSheet, 10);

        }

        public void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                rightTrueleftFalse = true;
                obj.velocityX = speed;
                SetCurrentAnim("runR", 80, false);
            }
            else if(!Keyboard.GetState().IsKeyDown(Keys.Left) && rightTrueleftFalse)
            {
                obj.velocityX = 0;
                SetCurrentAnim("idleR", 80, false);
            }


            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                rightTrueleftFalse = false;
                obj.velocityX = -speed;
                SetCurrentAnim("runL", 80, false);
            }
            else if(!Keyboard.GetState().IsKeyDown(Keys.Right) && !rightTrueleftFalse)
            {
                obj.velocityX = 0;
                SetCurrentAnim("idleL", 80, false);
            }
            obj.Update(gameTime);
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            obj.Draw(spriteBatch);
        }
        public void SetCurrentAnim(string name, int speedMS, bool waitForEnd)
        {
            spriteSheet.setAnim(name, speedMS, waitForEnd);
        }
    }
}
