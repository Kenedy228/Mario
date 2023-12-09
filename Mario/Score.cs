using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mario
{
    internal class Score : TextureDrawing
    {
        private float[] xCoordinates = new float[] { -0.9f, -0.85f, -0.85f, -0.9f };
        private float[] yCoordinates = new float[] { 0.85f, 0.85f, 0.97f, 0.97f };

        private float[] CoinXCoordinates = new float[] { -0.97f, -0.9f, -0.9f, -0.97f };
        private float[] CoinYCoordinates = new float[] { 0.85f, 0.85f, 0.97f, 0.97f };

        public float[,] texCoordinates = new float[,] { { 0f, 1f, 1f, 0f }, { 1f, 1f, 0f, 0f } };

        private int rows = 1,
            columns = 3,
            choosenAnimationFrameCount = 3,
            choosenAnimationFrameNumber = 0,
            choosenAnimationRowNumber = 0;

        private float frameWidth, frameHeight;

        public Score()
        {
            frameWidth = 1.0f / columns;
            frameHeight = 1.0f / rows;
        }

        public void DrawScore(int textureId, int score)
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


            choosenAnimationFrameNumber = score;
            choosenAnimationFrameNumber %= choosenAnimationFrameCount;
        }

        public void DrawCoin(int textureId)
        {
            base.Bind(textureId);

            base.Draw(
                texCoordinates,
                new float[,]
                {
                    {CoinXCoordinates[0], CoinXCoordinates[1], CoinXCoordinates[2], CoinXCoordinates[3] },
                    {CoinYCoordinates[0], CoinYCoordinates[1], CoinYCoordinates[2], CoinYCoordinates[3] },
                }
                );
        }
    }
}
