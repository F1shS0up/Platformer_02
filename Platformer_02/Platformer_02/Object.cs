using System.Diagnostics;
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
        public float initialVelocityX, initialVelocityY;

        private float velocityX, velocityY;

        private SpriteSheet spriteSheet;

        private float weight;

        public float gravityIncrease = 9.8f;

        public Vector2 pos;

        public Collision col;

        public bool inAir = true;

        private Rectangle currentStandingOn;

        public Object(SpriteSheet pSpriteSheet, float pWeight, Collision pCol)
        {
            spriteSheet = pSpriteSheet;

            weight = pWeight;

            col = pCol;
        }
        public void Update(GameTime gameTime)
        {
            ApplyMovement();

            velocityX = initialVelocityX * (float)(gameTime.ElapsedGameTime.TotalMilliseconds / 1000);
            velocityY = initialVelocityY * (float)(gameTime.ElapsedGameTime.TotalMilliseconds / 1000);

            CollisionUpdate();
            
            if(pos.X + velocityX + col.originX > currentStandingOn.Right || pos.X + velocityX + (spriteSheet.width - col.originX - col.width) < currentStandingOn.Left)
            {
                inAir = true;
            }
            pos += new Vector2(velocityX, velocityY);
            col.UpdateRectangleA(new Rectangle((int)pos.X + col.originX, (int)pos.Y + col.originY, col.width, col.height));
        }
        void ApplyMovement()
        {
            if (inAir)
            {
                ApplyGravity();
            }
        }
        private void CollisionUpdate()
        {
            CheckCollisionY();

            CheckCollisionX();
        }
        void CheckCollisionY()
        {
            
            (bool colBottom, int posBottom, Rectangle CurrentStandingOn) = col.IsTouchingBottom((int)velocityY);
            
            (bool colTop, int posTop) = col.IsTouchingTop((int)velocityY);
            if (colBottom && inAir)
            {
                initialVelocityY = 0;
                velocityY = 0;
                pos.Y = posBottom - (col.originY + col.height - 1);

                inAir = false;
                currentStandingOn = CurrentStandingOn;
                
            }

            if (colTop)
            {
                initialVelocityY = 0;
                velocityY = 0;
                pos.Y = posTop - col.originY;
                
            }
        }
        void CheckCollisionX()
        {
            (bool colRight, int posRight) = col.IsTouchingRight((int)velocityX);
            (bool colLeft, int posLeft) = col.IsTouchingLeft((int)velocityX);
            if (colRight)
            {
                initialVelocityX = 0;
                velocityX = 0;
                pos.X = posRight - (col.originX + col.width - 1);
            }
            else if (colLeft)
            {
                initialVelocityX = 0;
                velocityX = 0;
                pos.X = posLeft - col.originX;
            }
        }

        public void ApplyGravity()
        {
            initialVelocityY += weight * gravityIncrease;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteSheet.Draw(pos, spriteBatch, 0f);
        }


        public void SetCurrentAnim(string name, int speedMS, bool waitForEnd)
        {
            spriteSheet.setAnim(name, speedMS, waitForEnd, false);
        }
    }
}
