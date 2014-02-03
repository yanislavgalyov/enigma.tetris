using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Tetris.Lib;
using System.Threading;

namespace Tetris.App
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Level level;

        int LevelXOffset = 352;
        int LevelYOffset = 64;

        Texture2D block;
        Texture2D block2;

        Texture2D mainBackground;

        SpriteFont font;
        SpriteFont scoreFont;

        SoundEffect auuu;
        SoundEffectInstance auuuInstance;

        SoundEffect[] music = new SoundEffect[3];
        SoundEffectInstance musicInstance;

        string[] musicList = new string[] { "01-tyler_bates-to_victory", "02-tyler_bates-the_agoge", "03-tyler_bates-the_wolf" };
        private int musicIndex = 0;

        KeyboardState currentState = Keyboard.GetState();
        KeyboardState oldState;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 768;

            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            block = Content.Load<Texture2D>("shield2_300");
            block2 = Content.Load<Texture2D>("shield2_300");

            mainBackground = Content.Load<Texture2D>("mainbackground");

            font = Content.Load<SpriteFont>("Main");
            scoreFont = Content.Load<SpriteFont>(@"Fonts\ScoreFont");

            auuu = Content.Load<SoundEffect>(@"Audio\Waves\auuuu");
            auuuInstance = auuu.CreateInstance();
            auuuInstance.Volume = 1.0f;

            music[0] = Content.Load<SoundEffect>(@"Audio\Mp3\" + musicList[0]);

            level = new Level();
            level.GetNext();

            Thread thread = new Thread(new ThreadStart(LoadAudio));
            thread.Start();
        }

        private void LoadAudio()
        {
            music[1] = Content.Load<SoundEffect>(@"Audio\Mp3\" + musicList[1]);
            music[2] = Content.Load<SoundEffect>(@"Audio\Mp3\" + musicList[2]);
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            oldState = currentState;
            currentState = Keyboard.GetState();

            if (currentState.IsKeyUp(Keys.Escape) && oldState.IsKeyDown(Keys.Escape))
                this.Exit();

            if (level.IsActive)
            {
                if (currentState.IsKeyUp(Keys.OemPlus) && oldState.IsKeyDown(Keys.OemPlus))
                {
                    level.updateGameInterval -= level.deltaUpdateGameInterval;
                    level.updateGameInterval = MathHelper.Clamp(level.updateGameInterval, level.minUpdateGameInterval, level.maxUpdateGameInterval);
                    level.updateGameIntervalConst = level.updateGameInterval;
                }

                if (currentState.IsKeyUp(Keys.OemMinus) && oldState.IsKeyDown(Keys.OemMinus))
                {
                    level.updateGameInterval += level.deltaUpdateGameInterval;
                    level.updateGameInterval = MathHelper.Clamp(level.updateGameInterval, level.minUpdateGameInterval, level.maxUpdateGameInterval);
                    level.updateGameIntervalConst = level.updateGameInterval;
                }

                if (currentState.IsKeyDown(Keys.Down))
                {
                    level.updateGameInterval = level.minUpdateGameInterval;
                }
                else
                {
                    level.updateGameInterval = level.updateGameIntervalConst;
                }

                if (currentState.IsKeyUp(Keys.Left) && oldState.IsKeyDown(Keys.Left))
                {
                    if (!level.CheckForObstacle(Direction.Left))
                    {
                        level.Translate(Direction.Left);
                    }
                }

                if (currentState.IsKeyUp(Keys.Right) && oldState.IsKeyDown(Keys.Right))
                {
                    if (!level.CheckForObstacle(Direction.Right))
                    {
                        level.Translate(Direction.Right);
                    }
                }

                if (currentState.IsKeyUp(Keys.Space) && oldState.IsKeyDown(Keys.Space))
                {
                    level.Rotate();
                }

                level.Update(gameTime);

                if (musicInstance == null || musicInstance.State == SoundState.Stopped)
                {
                    musicInstance = GetSoundEffectInstance(musicIndex);
                    if (musicIndex == music.GetUpperBound(0))
                    {
                        musicIndex = 0;
                    }
                    else
                    {
                        musicIndex++;
                    }

                    musicInstance.Play();
                }

                if (level.PlaySound)
                {
                    if (auuuInstance.State == SoundState.Stopped || auuuInstance.State == SoundState.Paused)
                    {
                        auuuInstance.Play();
                    }
                    level.PlaySound = false;
                }
            }
        }

        private SoundEffectInstance GetSoundEffectInstance(int index)
        {
            var instance = music[musicIndex].CreateInstance();
            instance.Volume = 0.1f;

            return instance;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            spriteBatch.Draw(mainBackground, Vector2.Zero, Color.White);

            spriteBatch.DrawString(scoreFont, "Next:", new Vector2(100, 78), Color.Red);
            for (int i = 0; i <= level.NextCells.GetUpperBound(0); i++)
            {
                for (int j = 0; j <= level.NextCells.GetUpperBound(1); j++)
                {
                    switch (level.NextCells[i, j])
                    {
                        case 1:
                            spriteBatch.Draw(block, new Vector2(100 + j * block.Width, 100 + i * block.Height), Color.White);
                            break;
                        case 2:
                            spriteBatch.Draw(block2, new Vector2(100 + j * block2.Width, 100 + i * block2.Height), Color.White);
                            break;
                    }
                }
            }

            for (int i = 0; i < Level.Rows; i++)
            {
                for (int j = 0; j < Level.Cols; j++)
                {
                    switch(level.Cells[i, j])
                    {
                        case 1:
                            spriteBatch.Draw(block, new Vector2(LevelXOffset + j * block.Width, LevelYOffset + i * block.Height), Color.White);
                        break;
                        case 2:
                        spriteBatch.Draw(block2, new Vector2(LevelXOffset + j * block2.Width, LevelYOffset + i * block2.Height), Color.White);
                        break;
                    }
                }
            }

            if (!level.IsActive)
            {
                string gameOver = "Game Over";
                Vector2 fontWidth = font.MeasureString(gameOver);
                spriteBatch.DrawString(font, gameOver, new Vector2((graphics.PreferredBackBufferWidth - fontWidth.X) / 2, (graphics.PreferredBackBufferHeight - fontWidth.Y) / 2), Color.Red);
            }

            spriteBatch.DrawString(scoreFont, level.ScoreBoard(), new Vector2(880, 10), Color.Red);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
