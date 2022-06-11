using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Platformer_02
{
    class Player
    {
        private SpriteSheet spriteSheet;

        private Object obj;
        public Player(ContentManager Content)
        {
            spriteSheet = new SpriteSheet(Content.Load<Texture2D>("Sheet_Player"), 
                32, 
                32,
                new Dictionary<string, (int, int)> { { "null", (0, 0) }, { "idleL", (2, 5) }, { "idleR", (6, 9) } });

            obj = new Object(spriteSheet, 10);

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
