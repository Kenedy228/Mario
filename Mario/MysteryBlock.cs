using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mario
{
    internal class MysteryBlock : TextureDrawing
    {
        public float[] xCoordinates = new float[] { };
        public float[] yCoordinates = new float[] { };

        public bool coin = true;
        public bool hit = false;

        private float[,] texCoordinates = new float[,]
        {
            { 0f, 1f, 1f, 0f },
            { 1f, 1f, 0f, 0f }
        };

        public MysteryBlock(float[] xCoordinates, float[] yCoordinates)
        {
            this.xCoordinates = xCoordinates;
            this.yCoordinates = yCoordinates;
        }

        public void DrawMysteryBlock(int textureId)
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
        }
    }
}
