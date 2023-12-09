using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mario
{
    internal class Pipe : TextureDrawing
    {
        public float[] xCoordinates;
        public float[] yCoordinates;

        private float[,] texCoordinates = new float[,]
        {
            { 0f, 1f, 1f, 0f },
            { 1f, 1f, 0f, 0f }
        };

        public Pipe(float[] xCoordinates, float[] yCoordinates)
        {
            this.xCoordinates = xCoordinates;
            this.yCoordinates = yCoordinates;
        }

        public void DrawPipe(int textureId)
        {
            base.Bind(textureId);

            base.Draw(
                texCoordinates,
                new float[,]
                {
                    { xCoordinates[0], xCoordinates[1], xCoordinates[2], xCoordinates[3] },
                    { yCoordinates[0], yCoordinates[1], yCoordinates[2], yCoordinates[3] }
                }
                );
        }

    }
}
