using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Microsoft.Xna.Framework.Audio;

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
        Rectangle fruitRect;
        bool fruitWasCollected = true;
        int score = 0;
        SpriteFont font;
        SpriteEffects spriteEffects;
        Texture2D pixel;
        Vector2 pixelPosition = Vector2.Zero;
        Vector2 pixelVelocity;
        Random r = new Random();
        SoundEffect eat;
        SoundEffect fail;
        Rectangle monsterSourceRect;
        int frame;
        TimeSpan timer;
        int lives = 3;

        /// <summary>
        /// How quickly the animation should advance frames (1/8 second as milliseconds)
        /// </summary>
        const int ANIMATION_FRAME_RATE = 124;


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
            monsterRect.X = 500;
            monsterRect.Y = 200;
            monsterRect.Width = 75;
            monsterRect.Height = 75;

            // Monster source rect init
            monsterSourceRect.X = 0;
            monsterSourceRect.Y = 0;
            monsterSourceRect.Width = 50;
            monsterSourceRect.Height = 50;

            // Fruit Rect init
            fruitRect.X = -30;
            fruitRect.Y = -30;
            fruitRect.Width = 30;
            fruitRect.Height = 30;

            // Pixel Velocity init
            pixelVelocity = new Vector2(
            (float)r.NextDouble(),
            (float)r.NextDouble()
            );
            pixelVelocity.Normalize();

            timer = new TimeSpan(0);

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
            monster = Content.Load<Texture2D>("mon_sprite");
            fruit = Content.Load<Texture2D>("fruit");
            font = Content.Load<SpriteFont>("font");
            pixel = Content.Load<Texture2D>("pixel");
            eat = Content.Load<SoundEffect>("eat");
            fail = Content.Load<SoundEffect>("fail");
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
                spriteEffects = SpriteEffects.None;
            }
            if (keyboardState.IsKeyDown(Keys.Left))
            {
                monsterRect.X -= monsterSpeed;
                spriteEffects = SpriteEffects.FlipHorizontally;
            }

            // Keep Monster on Screen
            if (monsterRect.Y < 0) monsterRect.Y = 0;
            if (monsterRect.Y > GraphicsDevice.Viewport.Height - monsterRect.Height) monsterRect.Y = GraphicsDevice.Viewport.Height - monsterRect.Height;
            if (monsterRect.X < 0) monsterRect.X = 0;
            if (monsterRect.X > GraphicsDevice.Viewport.Width - monsterRect.Width) monsterRect.X = GraphicsDevice.Viewport.Width - monsterRect.Width;

            // Pixel Movement
            checkPixelBounds();

            // TODO: Add your update logic here
            if (pixelHitMonster())
            {
                lives -= 1;
                pixelVelocity.X *= -1;
                pixelVelocity.Y *= -1;
                pixelPosition.X += 100;
                pixelPosition.Y += 100;
                fail.Play();
            }

            if (lives <= 0) Exit();

            if (monsterCollectedFruit())
            {
                eat.Play();
                fruitWasCollected = true;
                score++;
            }

            if (fruitWasCollected)
            {
                changeFruitPosition();
            }


            timer += gameTime.ElapsedGameTime;
            // Determine the frame should increase.  Using a while 
            // loop will accomodate the possiblity the animation should 
            // advance more than one frame.
            while (timer.TotalMilliseconds > ANIMATION_FRAME_RATE)
            {
                // increase by one frame
                frame++;
                // reduce the timer by one frame duration
                timer -= new TimeSpan(0, 0, 0, 0, ANIMATION_FRAME_RATE);
            }

            // Keep the frame within bounds (there are four frames)
            frame %= 4;

            monsterSourceRect.X = frame * 50;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LawnGreen);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            spriteBatch.Draw(monster, monsterRect, monsterSourceRect, Color.White, 0.0f, new Vector2(0, 0), spriteEffects, 0.0f);
            spriteBatch.Draw(fruit, fruitRect, Color.White);
            spriteBatch.DrawString(font, "Score: " + score.ToString(), new Vector2(0, 0), Color.Black);
            spriteBatch.DrawString(font, "Lives: " + lives.ToString(), new Vector2(0, 50), Color.Black);
            spriteBatch.Draw(pixel, new Rectangle((int)pixelPosition.X, (int)pixelPosition.Y, 50, 50), Color.Red);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void changeFruitPosition()
        {
            fruitRect.X = r.Next(0, GraphicsDevice.Viewport.Width - fruitRect.Width);
            fruitRect.Y = r.Next(0, GraphicsDevice.Viewport.Height - fruitRect.Height);
            fruitWasCollected = !fruitWasCollected;
        }

        public bool monsterCollectedFruit()
        {
            return (monsterRect.X < fruitRect.X + fruitRect.Width
                && monsterRect.X + monsterRect.Width > fruitRect.X
                && monsterRect.Y < fruitRect.Y + fruitRect.Height
                && monsterRect.Y + monsterRect.Height > fruitRect.Y);
        }

        public bool pixelHitMonster()
        {
            return (pixelPosition.X < monsterRect.X + monsterRect.Width
                && pixelPosition.X + 50 > monsterRect.X
                && pixelPosition.Y < monsterRect.Y + monsterRect.Height
                && pixelPosition.Y + 50 > monsterRect.Y);
        }

        public void checkPixelBounds()
        {
            pixelPosition += 5 * pixelVelocity;

            // Check for wall collisions
            if (pixelPosition.Y < 0)
            {
                pixelVelocity.Y *= -1;
                float delta = 0 - pixelPosition.Y;
                pixelPosition.Y += 2 * delta;
            }

            if (pixelPosition.Y > graphics.PreferredBackBufferHeight - 50)
            {
                pixelVelocity.Y *= -1;
                float delta = graphics.PreferredBackBufferHeight - 50 - pixelPosition.Y;
                pixelPosition.Y += 2 * delta;
            }

            if (pixelPosition.X < 0)
            {
                pixelVelocity.X *= -1;
                float delta = 0 - pixelPosition.X;
                pixelPosition.X += 2 * delta;
            }

            if (pixelPosition.X > graphics.PreferredBackBufferWidth - 50)
            {
                pixelVelocity.X *= -1;
                float delta = graphics.PreferredBackBufferWidth - 50 - pixelPosition.X;
                pixelPosition.X += 2 * delta;
            }
        }
    }
}
