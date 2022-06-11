using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer_02
{
    static class ParticleImageManager
    {
        public static List<ParticleImage> particleImages = new List<ParticleImage>();
        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach(ParticleImage image in particleImages)
            {
                image.Draw(spriteBatch);
            }
        }
    }
    class ParticleImage
    {
        SpriteSheet spriteSheet;

        private Vector2 pos;

      
        public ParticleImage(Texture2D texture, int partWidth, Dictionary<string, (int, int)> keys = null)
        {
            int xCount = texture.Width / partWidth;
            if (keys != null)
            {
                spriteSheet = new SpriteSheet(texture, partWidth, partWidth, keys);
            }
            else
            {
                spriteSheet = new SpriteSheet(texture, partWidth, partWidth, new Dictionary<string, (int, int)> { { "null", (0, 0) }, { "default", (1, xCount) } });
            }
            ParticleImageManager.particleImages.Add(this);

            spriteSheet.setAnim("null", 0, false, false);
        }
        public void Instantiate(Vector2 pPos, int Speed)
        {
            pos = pPos;
            spriteSheet.setAnim("default", Speed, false, true);
            
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteSheet.Draw(pos, spriteBatch, 0f);
        }
        
        
    }
}
