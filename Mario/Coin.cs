using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mario
{
    internal class Coin : TextureDrawing
    {
        public float[] xCoordinates = new float[] { };
        public float[] yCoordinates = new float[] { };

        public int counter = 0;
        public bool up = true;
        public bool startCoinAnimation = false;

        private float[,] texCoordinates = new float[,]
        {
            { 0f, 1f, 1f, 0f },
            { 1f, 1f, 0f, 0f }
        };

        public Coin(float[] xCoordinates, float[] yCoordinates)
        {
            this.xCoordinates = xCoordinates;
            this.yCoordinates = yCoordinates;
        }

        public void DrawCoin(int textureId)
        {
            base.Bind(textureId);

            base.Draw(
                texCoordinates,
                new float[,]
                {
                    {xCoordinates[0], xCoordinates[1], xCoordinates[2], xCoordinates[3] },
                    {yCoordinates[0], yCoordinates[1], yCoordinates[2], yCoordinates[3] }
                }
                );
            AnimationHandler();
        }

        public void AnimationHandler()
        {
            if (up)
            {
                for (int i = 0; i < yCoordinates.Length; i++)
                {
                    yCoordinates[i] += 0.001f;
                }
                counter++;
            }
            else
            {
                for (int i = 0; i < yCoordinates.Length; i++)
                {
                    yCoordinates[i] -= 0.001f;
                }
                counter--;
            }

            if (counter > 100)
            {
                up = false;
            }

            if (!up && counter == 0)
            {
                startCoinAnimation = false;
            }
        }
    }
}
