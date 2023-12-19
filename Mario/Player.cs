using System;

namespace Mario
{
    internal class Player : TextureDrawing
    {
        public float[] xCoordinates = new float[] { -0.8f, -0.75f, -0.75f, -0.8f };
        public float[] yCoordinates = new float[] { -0.89f, -0.89f, -0.8f, -0.8f };

        public float[,] texCoordinates = new float[,] { { 0f, 1f, 1f, 0f }, { 1f, 1f, 0f, 0f } };
        public float initialY = -0.89f;

        public bool finish = false;

        List<Pipe> pipes;
        List<Brick> bricks;

        private int rows = 1,
            columns = 3,
            choosenAnimationFrameCount = 3,
            choosenAnimationFrameNumber = 0,
            choosenAnimationRowNumber = 0,
            frameDelay = 0;

        private float frameWidth, frameHeight;

        public bool turnedLeft = false;
        public bool run = false;

        public bool isGrounded = true;

        public float gravity = 0.02f;

        public float xDistance = 0.05f;

        public float yDistance = 0.09f;

        public Player(List<Pipe> pipes, List<Brick> bricks)
        {
            frameWidth = 1.0f / columns;
            frameHeight = 1.0f / rows;

            this.pipes = pipes;
            this.bricks = bricks;
        }

        public void DrawPlayer(int textureId)
        {
            base.Bind(textureId);

            base.Draw(
                texCoordinates,

                new float[,]
                {
                    {xCoordinates[0], xCoordinates[1], xCoordinates[2], xCoordinates[3] },
                    {yCoordinates[0], yCoordinates[1], yCoordinates[2], yCoordinates[3] },
                }
            );

            Fall();
        }

        public void RunPlayer(int textureId)
        {
            base.Bind(textureId);

            float x = choosenAnimationFrameNumber * frameWidth;
            float y = choosenAnimationRowNumber * frameHeight;

            base.Draw(
                new float[,] {
                    { x, x + frameWidth, x + frameWidth, x},
                    { y+ frameHeight, y + frameHeight, y , y },
                },

                new float[,]
                {
                    {xCoordinates[0], xCoordinates[1], xCoordinates[2], xCoordinates[3] },
                    {yCoordinates[0], yCoordinates[1], yCoordinates[2], yCoordinates[3] },
                }
            );

            frameDelay++;

            if (frameDelay > 10)
            {
                choosenAnimationFrameNumber++;
                choosenAnimationFrameNumber %= choosenAnimationFrameCount;
                frameDelay = 0;
            }
            
            run = false;

            Fall();
        }

        public void OnMove(int align)
        {
            switch (align)
            {
                case 0:
                    if (!turnedLeft)
                    {
                        TurnPlayer();
                        turnedLeft = true;
                    }

                    for (int i = 0; i < xCoordinates.Length; i++)
                    {
                        xCoordinates[i] -= 0.004f;
                    }
                    break;
                case 1:
                    if (turnedLeft)
                    {
                        TurnPlayer();
                        turnedLeft = false;
                    }

                    for (int i = 0; i < xCoordinates.Length; i++)
                    {
                        xCoordinates[i] += 0.004f;
                    }
                    break;
            }

            if (!turnedLeft && xCoordinates[1] > 1f)
            {
                xCoordinates[0] = 0.95f;
                xCoordinates[1] = 1f;
                xCoordinates[2] = 1f;
                xCoordinates[3] = 0.95f;
            }
            else if (turnedLeft && xCoordinates[1] < -1f)
            {
                xCoordinates[0] = -0.95f;
                xCoordinates[1] = -1f;
                xCoordinates[2] = -1f;
                xCoordinates[3] = -0.95f;
            }

        }

        public void TurnPlayer()
        {
            float[] tempCoordsX = new float[] { xCoordinates[0], xCoordinates[1], xCoordinates[2], xCoordinates[3] };

            xCoordinates[0] = tempCoordsX[1];
            xCoordinates[1] = tempCoordsX[0];
            xCoordinates[2] = tempCoordsX[3];
            xCoordinates[3] = tempCoordsX[2];
        }

        public void Jump()
        {
            if (isGrounded)
            {
                float jumpForce = FindJumpForce();

                for (int i = 0; i < yCoordinates.Length; i++)
                {
                    yCoordinates[i] += jumpForce;
                }

                isGrounded = false;
            }
        }

        public void Fall()
        {
            if (!isGrounded || !CheckPlatform())
            {
                for (int i = 0; i < yCoordinates.Length; i++)
                {
                    yCoordinates[i] -= gravity;
                }

                if (CheckPlatform())
                {
                    isGrounded = true;
                }
            } 
        }

        public bool CheckPlatform()
        {

            if (yCoordinates[0] < initialY)
            {
                CheckPipe();
                return true;
            }

            for (int i = 0; i < pipes.Count; i++)
            {
                if (yCoordinates[0] < pipes[i].yCoordinates[2] && yCoordinates[0] > pipes[i].yCoordinates[0])
                {
                    if ((!turnedLeft
                        &&((xCoordinates[1] > pipes[i].xCoordinates[0] && xCoordinates[0] < pipes[i].xCoordinates[0])
                        || (xCoordinates[0] < pipes[i].xCoordinates[1] && xCoordinates[1] > pipes[i].xCoordinates[1])
                        || (xCoordinates[0] > pipes[i].xCoordinates[0] && xCoordinates[1] < pipes[i].xCoordinates[1])))
                        || 
                        (turnedLeft 
                        && ((xCoordinates[1] < pipes[i].xCoordinates[1] && xCoordinates[0] > pipes[i].xCoordinates[1])
                        || (xCoordinates[1] < pipes[i].xCoordinates[0] && xCoordinates[0] > pipes[i].xCoordinates[0])
                        || (xCoordinates[1] > pipes[i].xCoordinates[0] && xCoordinates[0] < pipes[i].xCoordinates[1]))))
                    {
                        yCoordinates[0] = pipes[i].yCoordinates[2];
                        yCoordinates[1] = pipes[i].yCoordinates[2];
                        yCoordinates[2] = pipes[i].yCoordinates[2] + yDistance;
                        yCoordinates[3] = pipes[i].yCoordinates[2] + yDistance;
                        return true;
                    }
                }
            }

            for (int i = 0; i < bricks.Count; i++)
            {
                if (yCoordinates[0] <= bricks[i].yCoordinates[2] && yCoordinates[0] > bricks[i].yCoordinates[0])
                {
                    if ((!turnedLeft 
                        && ((xCoordinates[1] > bricks[i].xCoordinates[0] && xCoordinates[0] < bricks[i].xCoordinates[0])
                        || (xCoordinates[0] < bricks[i].xCoordinates[1] && xCoordinates[1] > bricks[i].xCoordinates[1])
                        || (xCoordinates[0] > bricks[i].xCoordinates[0] && xCoordinates[1] < bricks[i].xCoordinates[1]))
                        || 
                        (turnedLeft 
                        && ((xCoordinates[1] < bricks[i].xCoordinates[1] && xCoordinates[0] > bricks[i].xCoordinates[1])
                        || (xCoordinates[1] < bricks[i].xCoordinates[0] && xCoordinates[0] > bricks[i].xCoordinates[0])
                        || (xCoordinates[1] > bricks[i].xCoordinates[0] && xCoordinates[0] < bricks[i].xCoordinates[1])))
                        ))
                    {
                        yCoordinates[0] = bricks[i].yCoordinates[2];
                        yCoordinates[1] = bricks[i].yCoordinates[2];
                        yCoordinates[2] = bricks[i].yCoordinates[2] + yDistance;
                        yCoordinates[3] = bricks[i].yCoordinates[2] + yDistance;
                        return true;
                    }
                }
            }

            return false;
        }

        public float FindJumpForce()
        {
            float jumpForce = 0.5f;

            for (int i = 0; i < bricks.Count; i++)
            {
                if (
                    (!turnedLeft 
                    && ((xCoordinates[1] > bricks[i].xCoordinates[0] && xCoordinates[0] < bricks[i].xCoordinates[0])
                    || (xCoordinates[0] < bricks[i].xCoordinates[1] && xCoordinates[1] > bricks[i].xCoordinates[1])
                    || (xCoordinates[0] > bricks[i].xCoordinates[0] && xCoordinates[1] < bricks[i].xCoordinates[1]))
                    || 
                    (turnedLeft 
                    && ((xCoordinates[1] < bricks[i].xCoordinates[1] && xCoordinates[0] > bricks[i].xCoordinates[1])
                    || (xCoordinates[1] < bricks[i].xCoordinates[0] && xCoordinates[0] > bricks[i].xCoordinates[0])
                    || (xCoordinates[1] > bricks[i].xCoordinates[0] && xCoordinates[0] < bricks[i].xCoordinates[1])))
                    ))
                {
                    if (
                        yCoordinates[0] < bricks[i].yCoordinates[0] && bricks[i].yCoordinates[0] - jumpForce < 0
                        )
                    {
                        return bricks[i].yCoordinates[0] - yCoordinates[2];
                    }
                }
            }

            return jumpForce;
        }

        public void CheckPipe()
        {
            for (int i = 0; i < pipes.Count; i++)
            {
                if (yCoordinates[0] < pipes[i].yCoordinates[2])
                {
                    if ((!turnedLeft && xCoordinates[1] > pipes[i].xCoordinates[0]
                        && xCoordinates[0] < pipes[i].xCoordinates[0]))
                    {
                        xCoordinates[0] = pipes[i].xCoordinates[0] - xDistance;
                        xCoordinates[1] = pipes[i].xCoordinates[0];
                        xCoordinates[2] = pipes[i].xCoordinates[0];
                        xCoordinates[3] = pipes[i].xCoordinates[0] - xDistance;
                    }

                    if (turnedLeft && xCoordinates[1] < pipes[i].xCoordinates[1] 
                        && xCoordinates[0] > pipes[i].xCoordinates[1])
                    {
                        xCoordinates[0] = pipes[i].xCoordinates[1] + xDistance;
                        xCoordinates[1] = pipes[i].xCoordinates[1];
                        xCoordinates[2] = pipes[i].xCoordinates[1];
                        xCoordinates[3] = pipes[i].xCoordinates[1] + xDistance;
                    }
                }
            }
        }

        public bool CheckFinish()
        {
            if (xCoordinates[0] > 0.65f)
            {
                finish = true;
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}