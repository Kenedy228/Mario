using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;

namespace Mario
{
    internal class Game : GameWindow
    {
        int backgroundId, playerRunId, playerId,
            pipeId, brickId, mysteryBlockId, coinId,
            emptyBlockId, scoreId, startButtonId,
            exitButtonId, scoreCoinId, restartButtonId,
            menuButtonId;

        Background background = new Background();
        Player player = new Player();
        Score score = new Score();
        Menu menu = new Menu();

        Pipe pipe = new Pipe(
            new float[] { 0.2f, 0.3f, 0.3f, 0.2f },
            new float[] { -0.89f, -0.89f, -0.69f, -0.69f }
        );

        List<Brick> bricks = new List<Brick>
        {
            new Brick(
                new float[] {-0.2f, -0.16f, -0.16f, -0.2f },
                new float[] {-0.5f, -0.5f, -0.45f, -0.45f }
                ),
             new Brick(
                new float[] {0.5f, 0.54f, 0.54f, 0.5f },
                new float[] {-0.5f, -0.5f, -0.45f, -0.45f }
                ),
            new Brick(
                new float[] {-0.4f, -0.36f, -0.36f, -0.4f },
                new float[] {-0.5f, -0.5f, -0.45f, -0.45f }
                ),
        };

        List<MysteryBlock> mysteryBlocks = new List<MysteryBlock>
        {
            new MysteryBlock(
                new float[] {0.5f, 0.54f, 0.54f, 0.5f },
                new float[] {-0.5f, -0.5f, -0.45f, -0.45f }
                ),
            new MysteryBlock(
                new float[] {-0.4f, -0.36f, -0.36f, -0.4f },
                new float[] {-0.5f, -0.5f, -0.45f, -0.45f }
                ),
        };

        List<Coin> coins = new List<Coin>
        {
            new Coin(
                new float[] { 0.51f, 0.53f, 0.53f, 0.51f },
                new float[] { -0.5f, -0.5f, -0.45f, -0.45f }
                ),
            new Coin(
                new float[] {-0.39f, -0.37f, -0.37f, -0.39f },
                new float[] { -0.5f, -0.5f, -0.45f, -0.45f }
                )
        };

        int counter = 0;

        Vector2 cursorPosition = new Vector2();

        double targetFPS = 30.0;
        Stopwatch stopwatch = new Stopwatch();
        double elapsedTime = 0.0;

        public Game(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeSettings)
            : base(gameWindowSettings, nativeSettings)
        {
            backgroundId = GameTextures.LoadTextures("Textures/background.png");
            playerId = GameTextures.LoadTextures("Textures/mario-stay.png");
            playerRunId = GameTextures.LoadTextures("Textures/mario-run.png");
            pipeId = GameTextures.LoadTextures("Textures/pipe.png");
            brickId = GameTextures.LoadTextures("Textures/brick.png");
            mysteryBlockId = GameTextures.LoadTextures("Textures/mysteryblock.png");
            coinId = GameTextures.LoadTextures("Textures/coin.png");
            emptyBlockId = GameTextures.LoadTextures("Textures/emptyblock.png");
            scoreId = GameTextures.LoadTextures("Textures/numbers.png");
            startButtonId = GameTextures.LoadTextures("Textures/startButton.png");
            exitButtonId = GameTextures.LoadTextures("Textures/exitButton.png");
            scoreCoinId = GameTextures.LoadTextures("Textures/scoreCoin.png");
            restartButtonId = GameTextures.LoadTextures("Textures/restartButton.png");
            menuButtonId = GameTextures.LoadTextures("Textures/menuButton.png");
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                Close();
            }

            if (KeyboardState.IsKeyDown(Keys.Left) && !player.jump && menu.gameStatus == 1)
            {
                player.run = true;
                player.OnMove(0);
            }

            if (KeyboardState.IsKeyDown(Keys.Left) && player.jump && menu.gameStatus == 1)
            {
                player.OnMove(0);
            }

            if (KeyboardState.IsKeyDown(Keys.Right) && !player.jump && menu.gameStatus == 1)
            {
                player.run = true;
                player.OnMove(1);
            }

            if (KeyboardState.IsKeyDown(Keys.Right) && player.jump && menu.gameStatus == 1)
            {
                player.OnMove(1);
            }

        }

        protected override void OnKeyUp(KeyboardKeyEventArgs e)
        {
            base.OnKeyUp(e);

            if (e.Key == Keys.Space && !player.jump && menu.gameStatus == 1)
            {
                player.jump = true;
                player.run = false;
                player.Jump();
            }

            if (e.Key == Keys.Enter && menu.gameStatus == 1)
            {
                menu.gameStatus = player.CheckFinish();
            }
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);

            if (e.Button == MouseButton.Button1)
            {
                menu.MouseClickHandler(cursorPosition);
            }
        }

        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            base.OnMouseMove(e);

            cursorPosition.X = 2 * e.Position.X / ClientSize.X - 1.0f;
            cursorPosition.Y = -(2 * e.Position.Y / ClientSize.Y - 1.0f);
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            GL.ClearColor(0, 0, 0, 0);

            GL.Enable(EnableCap.Texture2D);

            stopwatch.Start();

        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, e.Width, e.Height);
        }

        protected override void OnUnload()
        {
            base.OnUnload();
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            double deltaTime = stopwatch.Elapsed.TotalMilliseconds;
            elapsedTime += deltaTime;

            if (elapsedTime >= 1 / targetFPS)
            {
                RenderFrame();
                SwapBuffers();
                elapsedTime = 0;
            }

            Thread.Sleep(5);
        }

        public void RenderFrame()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);
            background.DrawBackground(backgroundId);

            switch (menu.gameStatus)
            {
                case 0:
                    menu.DrawMenu(
                        new int[] { startButtonId, exitButtonId },
                        new string[] { "start", "exit" }
                        );
                    break;
                case 1:

                    if (menu.restart)
                    {
                        Clear();
                        menu.restart = false;
                    }

                    if (!player.run)
                    {
                        player.DrawPlayer(playerId);
                    }
                    else
                    {
                        player.RunPlayer(playerRunId);
                    }

                    pipe.DrawPipe(pipeId);

                    for (int i = 0; i < bricks.Count; i++)
                    {
                        bricks[i].DrawBrick(brickId);
                        player.CheckBrick(bricks[i]);
                    }

                    for (int i = 0; i < mysteryBlocks.Count; i++)
                    {
                        if (mysteryBlocks[i].coin)
                        {
                            mysteryBlocks[i].DrawMysteryBlock(mysteryBlockId);
                        }
                        else
                        {
                            mysteryBlocks[i].DrawMysteryBlock(emptyBlockId);
                        }
                        player.CheckMysteryBlock(mysteryBlocks[i]);

                        if (mysteryBlocks[i].hit && mysteryBlocks[i].coin)
                        {
                            coins[i].startCoinAnimation = true;
                            mysteryBlocks[i].coin = false;
                            counter++;
                        }
                    }

                    for (int i = 0; i < coins.Count; i++)
                    {
                        if (coins[i].startCoinAnimation)
                        {
                            coins[i].DrawCoin(coinId);
                        }
                    }

                    score.DrawScore(scoreId, counter);
                    score.DrawCoin(scoreCoinId);

                    player.CheckPipe(pipe);
                    break;
                case 2:

                    pipe.DrawPipe(pipeId);

                    for (int i = 0; i < bricks.Count; i++)
                    {
                        bricks[i].DrawBrick(brickId);
                        player.CheckBrick(bricks[i]);
                    }

                    for (int i = 0; i < mysteryBlocks.Count; i++)
                    {
                        if (mysteryBlocks[i].coin)
                        {
                            mysteryBlocks[i].DrawMysteryBlock(mysteryBlockId);
                        }
                        else
                        {
                            mysteryBlocks[i].DrawMysteryBlock(emptyBlockId);
                        }

                        if (mysteryBlocks[i].hit && mysteryBlocks[i].coin)
                        {
                            coins[i].startCoinAnimation = true;
                            mysteryBlocks[i].coin = false;
                            counter++;
                        }
                    }

                    for (int i = 0; i < coins.Count; i++)
                    {
                        if (coins[i].startCoinAnimation)
                        {
                            coins[i].DrawCoin(coinId);
                        }
                    }

                    score.DrawScore(scoreId, counter);
                    score.DrawCoin(scoreCoinId);

                    if (!player.finish)
                    {
                        player.RunPlayer(playerRunId);

                        if (!player.turnedLeft)
                        {
                            player.OnMove(1);
                        }
                        else
                        {
                            player.OnMove(0);
                        }

                        player.FinishAnimation();
                    }
                    else
                    {
                        menu.DrawMenu(
                        new int[] { restartButtonId, menuButtonId },
                        new string[] { "restart", "menu" }
                        );
                    }

                    break;
                case -1:
                    Close();
                    break;
            }

            GL.Flush();
        }

        public void Clear()
        {
            player = new Player();
            score = new Score();

            counter = 0;

            mysteryBlocks = new List<MysteryBlock>
            {
                new MysteryBlock(
                    new float[] {0.5f, 0.54f, 0.54f, 0.5f },
                    new float[] {-0.5f, -0.5f, -0.45f, -0.45f }
                    ),
                new MysteryBlock(
                    new float[] {-0.4f, -0.36f, -0.36f, -0.4f },
                    new float[] {-0.5f, -0.5f, -0.45f, -0.45f }
                    ),
            };

            coins = new List<Coin>
            {
                new Coin(
                    new float[] { 0.51f, 0.53f, 0.53f, 0.51f },
                    new float[] { -0.5f, -0.5f, -0.45f, -0.45f }
                    ),
                new Coin(
                    new float[] {-0.39f, -0.37f, -0.37f, -0.39f },
                    new float[] { -0.5f, -0.5f, -0.45f, -0.45f }
                    )
            };
        }
    }
}