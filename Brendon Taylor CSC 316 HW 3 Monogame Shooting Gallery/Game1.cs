using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

// Open Content
// Right Click COntent.mgcb
// Open MGCB Editor
// Copy files from BB to editor?

namespace Brendon_Taylor_CSC_316_HW_3_Monogame_Shooting_Gallery
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Texture2D target_Sprite; 
        Texture2D crosshair_Sprite;
        Texture2D background_Sprite;
        Texture2D startScreen_Sprite;
        SpriteFont gameFont;

        int score = 0;
        int health = 5;
        bool mReleased = false;
        bool start = false;
        bool end = false;
        bool hit = false;
        Vector2 targetSpeed = new Vector2(2,0);

        float distance = 0;
        const int RADIUS = 45;

        float timer = 10; // Could always change this. 
        Vector2 targetPosition = new Vector2(100, 100);
        MouseState mState;

        // Our main method. All basic setups happen here. This includes Window Size, Mouse Visibility, etc.
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this); // This is our window.
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        // Load content to the game. Not going to use it for this projeect.
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here.

            base.Initialize();
        }

        // Load images, sounds, fonts.
        protected override void LoadContent()
        {
            // Load images into Texture2D objects
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            startScreen_Sprite = Content.Load<Texture2D>("sky");
            target_Sprite = Content.Load<Texture2D>("duck_target_white"); // File name within paranthesis. Should see it on right of screen in Solution Explorer?
            crosshair_Sprite = Content.Load<Texture2D>("crosshair_blue_small");
            background_Sprite = Content.Load<Texture2D>("forest");
            gameFont = Content.Load<SpriteFont>("galleryFont");

            // TODO: use this.Content to load your game content here
        }

        // Main game loop. 
        // 60fps.
        // Code here that needs to be updated with game tick.
        protected override void Update(GameTime gameTime)
        {
            if (!start)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    start = true;
                }
            }
            else
            {
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                    Exit();

                distance = Vector2.Distance(targetPosition, new Vector2(mState.X, mState.Y));
                mState = Mouse.GetState();

                if (timer > 0)
                {
                    if (hit)
                    {
                        targetPosition.Y += 20;
                        if (targetPosition.Y > 850)
                        {
                            hit = false;
                            Random rand = new Random();
                            targetPosition.X = rand.Next(RADIUS, _graphics.PreferredBackBufferWidth - RADIUS);
                            targetPosition.Y = rand.Next(RADIUS, _graphics.PreferredBackBufferHeight - RADIUS); // Keeping within our window.
                        }
                    } 
                    else
                    {
                        timer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                        targetPosition += targetSpeed;
                        if (targetPosition.X >= 850)
                        {
                            targetPosition.X = 0;
                        }
                    }
                }

                if (mState.LeftButton == ButtonState.Pressed && mReleased == true)
                {
                    if (distance < RADIUS && timer > 0)
                    {
                        score++;
                        hit = true;
                    }
                    else
                    {
                        health--;
                    }
                    mReleased = false;
                }
                else if (mState.LeftButton == ButtonState.Released)
                {
                    mReleased = true;
                }
                base.Update(gameTime);
            }
        }

        // Everything we want to draw on screen.
        // 60fps.
        // This is only used to draw frames.
        // No calculations here.
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();

            if (!start)
            {
                _spriteBatch.Draw(startScreen_Sprite, new Vector2(0, 0), Color.White);
                _spriteBatch.DrawString(gameFont, "Please press the SPACEBAR to Start ", new Vector2(90,100), Color.Black);
                _spriteBatch.DrawString(gameFont, "our shooting gallery game! ", new Vector2(90, 130), Color.Black);
            } else if (timer > 0)
            {
                _spriteBatch.Draw(target_Sprite, new Vector2(0, 0), Color.White);
                _spriteBatch.Draw(background_Sprite, new Vector2(0, 0), Color.White);
                _spriteBatch.Draw(target_Sprite, new Vector2(targetPosition.X - RADIUS, targetPosition.Y - RADIUS), Color.White);
                _spriteBatch.DrawString(gameFont, "Score: " + score + "  | Health: " + health, new Vector2(10, 0), Color.Black);
                _spriteBatch.DrawString(gameFont, "Time Left: " + (int)timer, new Vector2(10, 40), Color.Black); // Could always use ToString if necessary
            }

            if ((timer < 0 && !end) || (health <= 0 && !end))
            {
                _spriteBatch.Draw(startScreen_Sprite, new Vector2(0, 0), Color.White);
                _spriteBatch.DrawString(gameFont, "Time's Up! ", new Vector2(250, 100), Color.Black);
                _spriteBatch.DrawString(gameFont, "Final Score: " + score,  new Vector2(250, 150), Color.Black);
                _spriteBatch.DrawString(gameFont, "Press SPACEBAR to exit!", new Vector2(250, 200), Color.Black);

                if (Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    end = true;
                    this.Exit();
                }
            }
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}