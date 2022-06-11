using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Platformer_02
{
    class SpriteSheet
    {
        private Texture2D texture;

        public int width, height;

        private Texture2D[] splitedTexture;

        private Dictionary<string, (int, int)> animKeys;

        private int currentSprite;
        private string currentAnim;

        private int currentAnimSpeed;
        private bool waitForEnd;

        private float time = 0;
        public SpriteSheet(Texture2D pTexture, int pWidth, int pHeight, Dictionary<string, (int, int)> pAnimKeys)
        {
            width = pWidth;
            height = pHeight;

            texture = pTexture;

            animKeys = pAnimKeys;

            splitedTexture = Split(texture, pWidth, pHeight);

            time = 0;

            SpriteSheetManager.spriteSheets.Add(this);
        }
        public void setAnim(string name, int speedMS, bool pWaitForEnd)
        {
            if(currentAnim == name) { return; }
            currentAnim = name;
            currentAnimSpeed = speedMS;
            waitForEnd = pWaitForEnd;

            time = 0;
            currentSprite = animKeys[currentAnim].Item1;
        }
        public void Update(GameTime gameTime)
        {
            if(currentAnim != "null")
            {
                if(time > currentAnimSpeed)
                {
                    currentSprite++;
                    if(currentSprite > animKeys[currentAnim].Item2)
                    {
                        currentSprite = animKeys[currentAnim].Item1;
                    }
                    time = 0;
                }
                else
                {
                    time += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                }
            }
        }
        public void Draw(Vector2 position, SpriteBatch spriteBatch, float rotation)
        {
            if(currentAnim != "null")
                spriteBatch.Draw(splitedTexture[currentSprite - 1], position, null, Color.White, rotation, Vector2.Zero, 1f,SpriteEffects.None, 0f);
        }
        public static Texture2D[] Split(Texture2D original, int partWidth, int partHeight)
        {
            int xCount, yCount;
            yCount = original.Height / partHeight;//The number of textures in each horizontal row
            xCount = original.Width / partWidth;//The number of textures in each vertical column
            Texture2D[] r = new Texture2D[xCount * yCount];//Number of parts = (area of original) / (area of each part).
            int dataPerPart = partWidth * partHeight;//Number of pixels in each of the split parts

            //Get the pixel data from the original texture:
            Color[] originalData = new Color[original.Width * original.Height];
            original.GetData<Color>(originalData);

            int index = 0;
            for (int y = 0; y < yCount * partHeight; y += partHeight)
                for (int x = 0; x < xCount * partWidth; x += partWidth)
                {
                    //The texture at coordinate {x, y} from the top-left of the original texture
                    Texture2D part = new Texture2D(original.GraphicsDevice, partWidth, partHeight);
                    //The data for part
                    Color[] partData = new Color[dataPerPart];

                    //Fill the part data with colors from the original texture
                    for (int py = 0; py < partHeight; py++)
                        for (int px = 0; px < partWidth; px++)
                        {
                            int partIndex = px + py * partWidth;
                            //If a part goes outside of the source texture, then fill the overlapping part with Color.Transparent
                            if (y + py >= original.Height || x + px >= original.Width)
                                partData[partIndex] = Color.Transparent;
                            else
                                partData[partIndex] = originalData[(x + px) + (y + py) * original.Width];
                        }

                    //Fill the part with the extracted data
                    part.SetData<Color>(partData);
                    //Stick the part in the return array:                    
                    r[index++] = part;
                }
            //Return the array of parts.
            return r;
        }
    }
}
