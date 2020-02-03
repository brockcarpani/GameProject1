using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace MonoGameWindowsStarter
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D monster;
        KeyboardState keyboardState;
        int monsterSpeed = 5;
        Rectangle monsterRect;
        Texture2D fruit;
        bool fruitCollected = false;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            graphics.PreferredBackBufferWidth = 1042;
            graphics.PreferredBackBufferHeight = 768;
            graphics.ApplyChanges();

            // Monster Rectangle init
            monsterRect.X = 50;
            monsterRect.Y = 50;
            monsterRect.Width = 75;
            monsterRect.Height = 75;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            monster = Content.Load<Texture2D>("monster");
            fruit = Content.Load<Texture2D>("fruit");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            keyboardState = Keyboard.GetState();

            // Monster Movement
            if (keyboardState.IsKeyDown(Keys.Down))
            {
                monsterRect.Y += monsterSpeed;
            }
            if (keyboardState.IsKeyDown(Keys.Up))
            {
                monsterRect.Y -= monsterSpeed;
            }
            if (keyboardState.IsKeyDown(Keys.Right))
            {
                monsterRect.X += monsterSpeed;
            }
            if (keyboardState.IsKeyDown(Keys.Left))
            {
                monsterRect.X -= monsterSpeed;
            }

            // Keep Monster on Screen
            if (monsterRect.Y < 0) monsterRect.Y = 0;
            if (monsterRect.Y > GraphicsDevice.Viewport.Height - monsterRect.Height) monsterRect.Y = GraphicsDevice.Viewport.Height - monsterRect.Height;
            if (monsterRect.X < 0) monsterRect.X = 0;
            if (monsterRect.X > GraphicsDevice.Viewport.Width - monsterRect.Width) monsterRect.X = GraphicsDevice.Viewport.Width - monsterRect.Width;

            // TODO: Add your update logic here

            // Spawn fruit if it was collected
            if (fruitCollected)
            {
                spawnFruit();
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            spriteBatch.Draw(monster, monsterRect, Color.White);
            spawnFruit();
            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void spawnFruit()
        {
            Random r = new Random();
            int randomX = r.Next(0, GraphicsDevice.Viewport.Width);
            int randomY = r.Next(0, GraphicsDevice.Viewport.Height);
            spriteBatch.Draw(fruit, new Rectangle(randomX, randomY, 30, 30), Color.White);
        }
    }
}
