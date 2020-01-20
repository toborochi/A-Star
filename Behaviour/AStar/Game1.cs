
using AStar.src.Core.BehaviourCore;
using AStar.src.Core.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace AStar
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D texCharacter;
        Texture2D texBackground;
        Texture2D texObstacle;
        int circles = 20,actors=5;
        int screenWidth,screenHeight;

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

            graphics.PreferredBackBufferWidth = 1408;
            graphics.PreferredBackBufferHeight = 1024;

            graphics.ApplyChanges();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            screenWidth = this.GraphicsDevice.Viewport.Width;
            screenHeight = this.GraphicsDevice.Viewport.Height;

            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load the Textures
            texCharacter = this.Content.Load<Texture2D>("spr_Character");
            texBackground = this.Content.Load<Texture2D>("spr_Background");
            texObstacle = this.Content.Load<Texture2D>("spr_Obstacle");

            // Bruteforce random spawning of  circles without overlapping
            List<Actor> obstacles = new List<Actor>();

            int counter = 0, limit = 5000;

            for (int i = 0; i < circles && counter<limit; ++i)
            {
                Vector2 position = Actor.GetRandomPosition(screenWidth, screenHeight); 
                bool overlapping = false;
                for (int j = 0; j < obstacles.Count; ++j) {
                    Actor existing = obstacles[j];
                    float d = Vector2.Distance(new Vector2(existing.Position.X, existing.Position.Y),
                                               new Vector2(position.X,position.Y));

                    if (d < 200)
                    {
                        overlapping = true;
                        break;
                    }
                }

                if (!overlapping)
                {
                    obstacles.Add(new Actor(texObstacle,Color.DodgerBlue));
                    obstacles[obstacles.Count - 1].Position = position;
                }

                counter++;
            }


            // Main Actor
            Actor Leader = new Actor(texCharacter,Color.LimeGreen);
            Leader.Speed = 8;
            Leader.Direction = Actor.GetRandomDirection();
            Leader.Position = Actor.GetRandomPosition(screenWidth,screenHeight);
            Leader.BehaviourList.Add(new BehaviourKeyboard(0.75f));
            Leader.BehaviourList.Add(new BehaviourWander(0.2f,60));

            // Enemies
            BehaviourSeek seek = new BehaviourSeek(0.05f, Leader);
            for (int i = 0; i < actors; ++i)
            {
                Actor Drone = new Actor(texCharacter, Color.Red);
                Drone.Speed = 5;
                Drone.Direction = Actor.GetRandomDirection();
                Drone.Position = Actor.GetRandomPosition(screenWidth, screenHeight);
                Drone.BehaviourList.Add(seek);
                Drone.BehaviourList.Add(new BehaviourWander(0.03f,15));
                for (int j = 0; j < obstacles.Count; ++j) {
                    Drone.BehaviourList.Add(new BehaviourAvoid(0.4f, obstacles[j],100));
                }
                Leader.BehaviourList.Add(new BehaviourAvoid(0.6f,Drone,120));
            }

            foreach (var obstacle in obstacles) {
                Leader.BehaviourList.Add(new BehaviourAvoid(0.3f,obstacle,100));
            }

            // TODO: use this.Content to load your game content here
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

            // TODO: Add your update logic here
            foreach (var actor in Actor.Actors) {
                actor.Update();
                if (actor.Position.X > screenWidth)
                {
                    actor.Position.X = 0;
                }
                else if(actor.Position.X < 0)
                {
                    actor.Position.X = screenWidth;
                }

                if (actor.Position.Y > screenHeight)
                {
                    actor.Position.Y = 0;
                }
                else if (actor.Position.Y < 0)
                {
                    actor.Position.Y = screenHeight;
                }

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

            spriteBatch.Begin();

            for (int i = 0; i < screenWidth / 64; ++i) {

                for (int j = 0; j < screenHeight / 64; ++j)
                {
                    spriteBatch.Draw(texBackground, new Vector2(i * 64, j*64), Color.White);
                }


            }


            foreach (var actor in Actor.Actors) {
                actor.Draw(spriteBatch);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
