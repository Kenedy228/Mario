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
    //класс игры, наследуется от встроенного в OpenGL класса GameWindow
    internal class Game : GameWindow
    {
        //id текстур
        int backgroundId, playerRunId, playerId,
            pipeId, brickId, mysteryBlockId, coinId,
            emptyBlockId, scoreId, startButtonId,
            exitButtonId, scoreCoinId, restartButtonId,
            menuButtonId;

        //создаем фон
        Background background = new Background();
        //создаем счет
        Score score = new Score();
        //создаем меню
        Menu menu = new Menu();

        //создаем трубы
        static List<Pipe> pipes = new List<Pipe>
        {
            new Pipe(
                new float[] { 0.2f, 0.3f, 0.3f, 0.2f },
                new float[] { -0.89f, -0.89f, -0.69f, -0.69f }
            ),
        };

        //создаем кирпичи
        static List<Brick> bricks = new List<Brick>
        {
            new Brick(
                new float[] {-0.23f, 0.13f, 0.13f, -0.23f },
                new float[] {-0.65f, -0.65f, -0.6f, -0.6f }
                ),
        };

        //создаем лаки блоки
        static List<MysteryBlock> mysteryBlocks = new List<MysteryBlock>
        {
            new MysteryBlock(
                new float[] {0.5f, 0.54f, 0.54f, 0.5f },
                new float[] { -0.6f, -0.6f, -0.55f, -0.55f }
                ),
            new MysteryBlock(
                new float[] {-0.7f, -0.66f, -0.66f, -0.7f },
                new float[] { -0.6f, -0.6f, -0.55f, -0.55f }
                ),
            new MysteryBlock(
                    new float[] {-0.5f, -0.46f, -0.46f, -0.5f },
                    new float[] { -0.6f, -0.6f, -0.55f, -0.55f }
                ),
            new MysteryBlock(
                    new float[] {0f, 0.04f, 0.04f, 0f },
                    new float[] {-0.15f, -0.15f, -0.1f, -0.1f }
                ),
            new MysteryBlock(
                    new float[] {-0.18f, -0.14f, -0.14f, -0.18f },
                    new float[] {-0.15f, -0.15f, -0.1f, -0.1f }
                ),
        };

        //создаем монетки
        List<Coin> coins = new List<Coin>
        {
            new Coin(
                new float[] { 0.51f, 0.53f, 0.53f, 0.51f },
                new float[] { -0.6f, -0.6f, -0.55f, -0.55f }
                ),
            new Coin(
                new float[] {-0.69f, -0.67f, -0.67f, -0.69f },
                new float[] { -0.6f, -0.6f, -0.55f, -0.55f }
                ),
            new Coin(
                new float[] {-0.49f, -0.47f, -0.47f, -0.49f },
                new float[] { -0.6f, -0.6f, -0.55f, -0.55f }
                ),
            new Coin(
                    new float[] {-0.17f, -0.15f, -0.15f, -0.17f },
                    new float[] {-0.15f, -0.15f, -0.1f, -0.1f }
                ),
            new Coin(
                    new float[] {-0.17f, -0.15f, -0.15f, -0.17f },
                    new float[] {-0.15f, -0.15f, -0.1f, -0.1f }
                ),
        };

        //создаем марио
        Player player = new Player(pipes, bricks, mysteryBlocks);

        //делаем счетчик собранных монеток
        int counter = 0;

        //создаем вектор, который будет хранить позицию точки, когда нажали ЛКМ
        Vector2 cursorPosition = new Vector2();

        //конструктор с параметрами, инициализация игры
        public Game(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeSettings)
            : base(gameWindowSettings, nativeSettings)
        {
            //задаем id текстурам
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

        //метод обновления кадра
        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            //если нажимаем на Escape, то закрываем игру
            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                Close();
            }

            //если нажали влево и игра началась, то двигаем марио влево
            if (KeyboardState.IsKeyDown(Keys.Left) && menu.gameStatus == 1)
            {
                player.run = true;
                player.OnMove(0);
            }

            //если нажали вправо и игра началась, то двигаем марио вправо
            if (KeyboardState.IsKeyDown(Keys.Right) && menu.gameStatus == 1)
            {
                player.run = true;
                player.OnMove(1);
            }

        }

        //метод для отжатия клавиши (располагаем не в методе OnUpdateFrame,
        //так как реагируем на отжатие клавиши)
        protected override void OnKeyUp(KeyboardKeyEventArgs e)
        {
            base.OnKeyUp(e);

            //если отжали пробел и игра началась, ты марио прыгает
            if (e.Key == Keys.Space && menu.gameStatus == 1)
            {
                player.run = false;
                player.Jump();
            }

            //если отжали Enter и игра началась, то проверяем, можно ли закончить игру
            if (e.Key == Keys.Enter && menu.gameStatus == 1)
            {
                menu.gameStatus = player.CheckFinish();
            }
        }

        //мето для обработки нажатия кнопок на мышке
        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);

            //если кликаем ЛКМ, то вызываем обработчик нажатия на ЛКМ
            if (e.Button == MouseButton.Button1)
            {
                menu.MouseClickHandler(cursorPosition);
            }
        }

        //метод для считывания координат мышки при движении мышкой
        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            base.OnMouseMove(e);

            //меняем x координату у курсора (необходимо перевести в значение в float)
            cursorPosition.X = 2 * e.Position.X / ClientSize.X - 1.0f;
            //меняем y координату у курсора (необходимо перевести в значение в float)
            cursorPosition.Y = -(2 * e.Position.Y / ClientSize.Y - 1.0f);
        }

        //метод при загрузке игры
        protected override void OnLoad()
        {
            base.OnLoad();

            GL.ClearColor(0, 0, 0, 0);

            GL.Enable(EnableCap.Texture2D);

        }

        //метод для изменения размеров окна
        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, e.Width, e.Height);
        }

        //метод, вызываем перед закрытием окна
        protected override void OnUnload()
        {
            base.OnUnload();
        }

        //метод для рендера кадра
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            //очищаем буфер
            GL.Clear(ClearBufferMask.ColorBufferBit);
            //отрисовываем задний фон
            background.DrawBackground(backgroundId);

            //обрабатываем статус игры
            switch (menu.gameStatus)
            {
                //если мы в меню, то отрисовываем кнопки
                case 0:
                    menu.DrawMenu(
                        new int[] { startButtonId, exitButtonId },
                        new string[] { "start", "exit" }
                        );
                    break;
                //если игра началась, отрисовываем все текстуры и выполняем логику игры
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

                    for (int i = 0; i < pipes.Count; i++)
                    {
                        pipes[i].DrawPipe(pipeId);
                    }

                    for (int i = 0; i < bricks.Count; i++)
                    {
                        bricks[i].DrawBrick(brickId);
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

                    player.CheckPlatform();

                    for (int i = 0; i < coins.Count; i++)
                    {
                        if (coins[i].startCoinAnimation)
                        {
                            coins[i].DrawCoin(coinId);
                        }
                    }

                    score.DrawScore(scoreId, counter);
                    score.DrawCoin(scoreCoinId);

                    break;
                //если игра закончена, то убираем марио, отрисовываем все текстуры 
                //и кнопки для перезапуска игры
                case 2:

                    for (int i = 0; i < pipes.Count; i++)
                    {
                        pipes[i].DrawPipe(pipeId);
                    }

                    for (int i = 0; i < bricks.Count; i++)
                    {
                        bricks[i].DrawBrick(brickId);
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
                //если нажали на выход, то закрываем игру
                case -1:
                    Close();
                    break;
            }

            //переключаем буфер
            SwapBuffers();
        }

        //метод для очищения (используется при перезапуске игры)
        public void Clear()
        {
            score = new Score();

            counter = 0;

            mysteryBlocks = new List<MysteryBlock>
            {
                new MysteryBlock(
                    new float[] {0.5f, 0.54f, 0.54f, 0.5f },
                    new float[] {-0.6f, -0.6f, -0.55f, -0.55f }
                    ),
                new MysteryBlock(
                    new float[] {-0.7f, -0.66f, -0.66f, -0.7f },
                    new float[] {-0.6f, -0.6f, -0.55f, -0.55f }
                ),
                new MysteryBlock(
                    new float[] {-0.5f, -0.46f, -0.46f, -0.5f },
                    new float[] {-0.6f, -0.6f, -0.55f, -0.55f }
                ),
                new MysteryBlock(
                    new float[] {0f, 0.04f, 0.04f, 0f },
                    new float[] {-0.15f, -0.15f, -0.1f, -0.1f }
                ),
                new MysteryBlock(
                    new float[] {-0.18f, -0.14f, -0.14f, -0.18f },
                    new float[] {-0.15f, -0.15f, -0.1f, -0.1f }
                ),

            };

            coins = new List<Coin>
            {
                new Coin(
                    new float[] { 0.51f, 0.53f, 0.53f, 0.51f },
                    new float[] { -0.6f, -0.6f, -0.55f, -0.55f }
                    ),
                new Coin(
                    new float[] {-0.69f, -0.67f, -0.67f, -0.69f },
                    new float[] { -0.6f, -0.6f, -0.55f, -0.55f }
                    ),
                new Coin(
                    new float[] {-0.49f, -0.47f, -0.47f, -0.49f },
                    new float[] { -0.6f, -0.6f, -0.55f, -0.55f }
                ),
                new Coin(
                    new float[] {0.01f, 0.03f, 0.03f, 0.01f },
                    new float[] {-0.15f, -0.15f, -0.1f, -0.1f }
                ),
                new Coin(
                    new float[] {-0.17f, -0.15f, -0.15f, -0.17f },
                    new float[] {-0.15f, -0.15f, -0.1f, -0.1f }
                ),
            };

            player = new Player(pipes, bricks, mysteryBlocks);
        }
    }
}