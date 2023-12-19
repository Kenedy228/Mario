using System;
using System.Collections.Generic;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Mario
{
    internal class Game : GameWindow
    {
        int backgroundId, playerRunId, playerId,
            pipeId, brickId;

        Background background = new Background();

        static List<Pipe> pipes = new List<Pipe>
        {
            new Pipe(
                new float[] { 0.2f, 0.3f, 0.3f, 0.2f },
                new float[] { -0.89f, -0.89f, -0.69f, -0.69f }
            ),
        };

        static List<Brick> bricks = new List<Brick>
        {
            new Brick(
                new float[] {-0.23f, 0.13f, 0.13f, -0.23f },
                new float[] {-0.65f, -0.65f, -0.6f, -0.6f }
                ),
        };

        Player player = new Player(pipes, bricks);

        private bool gameStart = false;

        public Game(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeSettings)
            : base(gameWindowSettings, nativeSettings)
        {
            backgroundId = GameTextures.LoadTextures("Textures/background.png");
            playerId = GameTextures.LoadTextures("Textures/mario-stay.png");
            playerRunId = GameTextures.LoadTextures("Textures/mario-run.png");
            pipeId = GameTextures.LoadTextures("Textures/pipe.png");
            brickId = GameTextures.LoadTextures("Textures/brick.png");
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                Close();
            }

            if (KeyboardState.IsKeyDown(Keys.Left) && gameStart == true)
            {
                player.run = true;
                player.OnMove(0);
            }

            if (KeyboardState.IsKeyDown(Keys.Right) && gameStart == true)
            {
                player.run = true;
                player.OnMove(1);
            }
        }

        protected override void OnKeyUp(KeyboardKeyEventArgs e)
        {
            base.OnKeyUp(e);

            if (e.Key == Keys.Space && gameStart == true)
            {
                player.run = false;
                player.Jump();
            }

            if (e.Key == Keys.Enter && gameStart == true)
            {
                gameStart = player.CheckFinish();
            } else
            {
                gameStart = true;
            }
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            GL.Enable(EnableCap.Texture2D);
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, e.Width, e.Height);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            GL.Clear(ClearBufferMask.ColorBufferBit);
            background.DrawBackground(backgroundId);

            for (int i = 0; i < pipes.Count; i++)
            {
                pipes[i].DrawPipe(pipeId);
            }

            for (int i = 0; i < bricks.Count; i++)
            {
                bricks[i].DrawBrick(brickId);
            }

            if (gameStart)
            {
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

                player.CheckPlatform();
            }

            if (player.finish)
            {
                Clear();
            }

            SwapBuffers();
        }

        public void Clear()
        {
            player = new Player(pipes, bricks);
        }
    }
}