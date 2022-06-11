using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace Platformer_02
{
    class Object
    {
        public float velocityX, velocityY;

        private SpriteSheet spriteSheet;

        private float weight;

        private Vector2 pos;

        

        public Object(SpriteSheet pSpriteSheet, float pWeight)
        {
            spriteSheet = pSpriteSheet;

            weight = pWeight;
        }
        public void Update(GameTime gameTime)
        {
            ApplyGravity();



            pos += new Vector2(velocityX * (float)(gameTime.ElapsedGameTime.TotalMilliseconds / 1000), velocityY * (float)(gameTime.ElapsedGameTime.TotalMilliseconds / 1000));
        }

        public void ApplyGravity()
        {
            velocityY += weight;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteSheet.Draw(pos, spriteBatch, 0f);
        }


        public void SetCurrentAnim(string name, int speedMS, bool waitForEnd)
        {
            spriteSheet.setAnim(name, speedMS, waitForEnd);
        }
    }
}
