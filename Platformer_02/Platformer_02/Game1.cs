using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

using TiledSharp;
namespace Platformer_02
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        RenderTarget2D changing, nonChanging;

        Vector2 sizeWindow;

        Player player;

        TileMapManager tileMapManager;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            graphics.PreferredBackBufferWidth = GraphicsDevice.Adapter.CurrentDisplayMode.Width;
            graphics.PreferredBackBufferHeight = GraphicsDevice.Adapter.CurrentDisplayMode.Height;
            graphics.ApplyChanges();

            sizeWindow = new Vector2(graphics.PreferredBackBufferWidth / 384, graphics.PreferredBackBufferHeight / 216);

            changing = new RenderTarget2D(GraphicsDevice, 384, 216);
            nonChanging = new RenderTarget2D(GraphicsDevice, 384, 216);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);


            TmxMap map = new TmxMap("Content/TestMap.tmx");
            tileMapManager = new TileMapManager(map, Content.Load<Texture2D>("tileSetImage"), 48 / map.TileWidth, map.TileWidth, map.TileHeight);
            player = new Player(Content, tileMapManager);
            player.SetCurrentAnim("idleL", 300, false);
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            player.Update(gameTime);
            // TODO: Add your update logic here
            SpriteSheetManager.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            DrawChanging();
            DrawNonChanging();
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
            spriteBatch.Draw(nonChanging, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, sizeWindow, SpriteEffects.None, 0f);
            spriteBatch.Draw(changing, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, sizeWindow, SpriteEffects.None, 0f);
            spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
        void DrawChanging()
        {
            GraphicsDevice.SetRenderTarget(changing);
            GraphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
            player.Draw(spriteBatch);
            ParticleImageManager.Draw(spriteBatch);
            spriteBatch.End();
            GraphicsDevice.SetRenderTarget(null);
        }
        void DrawNonChanging()
        {
            GraphicsDevice.SetRenderTarget(nonChanging);
            GraphicsDevice.Clear(Color.LightBlue);
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
            tileMapManager.Draw(spriteBatch);
            spriteBatch.End();
            GraphicsDevice.SetRenderTarget(null);
        }
    }
}
