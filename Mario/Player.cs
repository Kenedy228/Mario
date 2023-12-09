using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Mario
{
    internal class Player : TextureDrawing
    {
        public float[] xCoordinates = new float[] { -0.8f, -0.72f, -0.72f, -0.8f };
        public float[] yCoordinates = new float[] { -0.89f, -0.89f, -0.7f, -0.7f };
        public float[] yJump = { -0.89f, -0.89f };

        public float[,] texCoordinates = new float[,] { { 0f, 1f, 1f, 0f }, { 1f, 1f, 0f, 0f } };
        public float initialY = -0.89f;

        public bool finish = false;

        private int rows = 1,
            columns = 3,
            choosenAnimationFrameCount = 3,
            choosenAnimationFrameNumber = 0,
            choosenAnimationRowNumber = 0,
            frameDelay = 0;

        private float frameWidth, frameHeight;
        public bool turnedLeft = false;
        public bool run = false;
        public bool jump = false;

        public Player()
        {
            frameWidth = 1.0f / columns;
            frameHeight = 1.0f / rows;
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

            if (jump == true)
            {
                JumpHandler();
            }
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

            if (frameDelay > 20)
            {
                choosenAnimationFrameNumber++;
                choosenAnimationFrameNumber %= choosenAnimationFrameCount;
                frameDelay = 0;
            }

            run = false;
        }

        public void OnMove(int align)
        {
            //0 - left
            //1 - right

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
                        xCoordinates[i] -= 0.001f;
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
                        xCoordinates[i] += 0.001f;
                    }
                    break;
            }

            if (!turnedLeft && xCoordinates[1] > 1f)
            {
                xCoordinates[0] = 0.92f;
                xCoordinates[1] = 1f;
                xCoordinates[2] = 1f;
                xCoordinates[3] = 0.92f;
            }
            else if (turnedLeft && xCoordinates[1] < -1f)
            {
                xCoordinates[0] = -0.92f;
                xCoordinates[1] = -1f;
                xCoordinates[2] = -1f;
                xCoordinates[3] = -0.92f;
            }

        }

        public void TurnPlayer()
        {
            float[] tempCoordsX = new float[] { xCoordinates[0], xCoordinates[1] };

            xCoordinates[0] = tempCoordsX[1];
            xCoordinates[1] = tempCoordsX[0];
            xCoordinates[2] = tempCoordsX[0];
            xCoordinates[3] = tempCoordsX[1];
        }

        public void Jump()
        {
            yJump[0] += 0.4f;
        }

        public void JumpHandler()
        {
            if (yCoordinates[0] < yJump[0])
            {
                for (int i = 0; i < yCoordinates.Length; i++)
                {
                    yCoordinates[i] += 0.012f;
                }
            }
            else if (yCoordinates[0] >= yJump[0])
            {
                for (int i = 0; i < yCoordinates.Length; i++)
                {
                    yCoordinates[i] -= 0.012f;
                }
                yJump[0] -= 0.012f;
            }

            if (yJump[0] < yJump[1])
            {
                yJump[0] = yJump[1];

                yCoordinates[0] = yJump[0];
                yCoordinates[1] = yJump[0];
                yCoordinates[2] = yJump[0] + 0.19f;
                yCoordinates[3] = yJump[0] + 0.19f;

                jump = false;
            }
            else if (yJump[0] > yJump[1])
            {
                jump = true;
            }

        }

        public void CheckPipe(Pipe pipe)
        {
            float distance = 0.02f;

            if (!turnedLeft && xCoordinates[1] > pipe.xCoordinates[0] && xCoordinates[0] < pipe.xCoordinates[0]
                && yCoordinates[0] > pipe.yCoordinates[2])
            {
                yJump[1] = pipe.yCoordinates[2];
            }
            else if (!turnedLeft && xCoordinates[0] + distance > pipe.xCoordinates[1] && yCoordinates[0] >= pipe.yCoordinates[2])
            {
                jump = true;
                yJump[1] = initialY;
            }
            else if (!turnedLeft && xCoordinates[1] > pipe.xCoordinates[0] && xCoordinates[0] < pipe.xCoordinates[0]
                && yCoordinates[0] < pipe.yCoordinates[2])
            {
                xCoordinates[0] = pipe.xCoordinates[0] - 0.08f;
                xCoordinates[1] = pipe.xCoordinates[0];
                xCoordinates[2] = pipe.xCoordinates[0];
                xCoordinates[3] = pipe.xCoordinates[0] - 0.08f;
            }

            if (turnedLeft && xCoordinates[1] < pipe.xCoordinates[1] && xCoordinates[0] > pipe.xCoordinates[1]
                && yCoordinates[0] > pipe.yCoordinates[2])
            {
                yJump[1] = pipe.yCoordinates[2];
            }
            else if (turnedLeft && xCoordinates[0] - distance < pipe.xCoordinates[0] && yCoordinates[0] >= pipe.yCoordinates[2])
            {
                jump = true;
                yJump[1] = initialY;
            }
            else if (turnedLeft && xCoordinates[1] < pipe.xCoordinates[1] && xCoordinates[0] > pipe.xCoordinates[1]
                && yCoordinates[0] < pipe.yCoordinates[2])
            {
                xCoordinates[0] = pipe.xCoordinates[1] + 0.08f;
                xCoordinates[1] = pipe.xCoordinates[1];
                xCoordinates[2] = pipe.xCoordinates[1];
                xCoordinates[3] = pipe.xCoordinates[1] + 0.08f;

            }
        }

        //доделать кирпичи
        //попадание сбоку

        public void CheckBrick(Brick brick)
        {
            float distanceY = 0.19f;
            float distanceX = 0.02f;

            if (!turnedLeft && xCoordinates[1] - distanceX >= brick.xCoordinates[0] && xCoordinates[0] + distanceX <= brick.xCoordinates[1]
                && yCoordinates[0] + distanceY > brick.yCoordinates[0])
            {
                yJump[0] = brick.yCoordinates[0] - distanceY;
            }

            if (turnedLeft && xCoordinates[1] + distanceX <= brick.xCoordinates[1] && xCoordinates[0] - distanceX >= brick.xCoordinates[0]
                && yCoordinates[0] + distanceY > brick.yCoordinates[0])
            {
                yJump[0] = brick.yCoordinates[0] - distanceY;
            }
        }

        public void CheckMysteryBlock(MysteryBlock mysteryBlock)
        {
            float distanceY = 0.19f;
            float distanceX = 0.02f;


            if (!turnedLeft && xCoordinates[1] - distanceX >= mysteryBlock.xCoordinates[0] && xCoordinates[0] + distanceX <= mysteryBlock.xCoordinates[1]
                && yCoordinates[0] + distanceY > mysteryBlock.yCoordinates[0])
            {
                yJump[0] = mysteryBlock.yCoordinates[0] - distanceY;
                mysteryBlock.hit = true;
            }

            if (turnedLeft && xCoordinates[1] + distanceX <= mysteryBlock.xCoordinates[1] && xCoordinates[0] - distanceX >= mysteryBlock.xCoordinates[0]
                && yCoordinates[0] + distanceY > mysteryBlock.yCoordinates[0])
            {
                yJump[0] = mysteryBlock.yCoordinates[0] - distanceY;
                mysteryBlock.hit = true;
            }
        }

        public int CheckFinish()
        {
            if (xCoordinates[0] > 0.6f)
            {
                FinishAnimation();
                return 2;
            }
            else
            {
                return 1;
            }
        }

        public void FinishAnimation()
        {
            if (!turnedLeft)
            {
                if (xCoordinates[0] < 0.75f)
                {
                    OnMove(1);
                }
                else
                {
                    finish = true;
                }
            }
            else
            {
                if (xCoordinates[0] > 0.75f)
                {
                    OnMove(0);
                }
                else
                {
                    finish = true;
                }
            }
        }
    }
}