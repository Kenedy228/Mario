using System;

namespace Mario
{
    internal class Background : TextureDrawing
    {
        private float[,] coordinates = new float[,]
        {
            {-1, 1, 1, -1 },
            {-1, -1, 1, 1 },
        };

        private float[,] texCoordinates = new float[,]
        {
            { 0f, 1f, 1f, 0f },
            { 1f, 1f, 0f, 0f }
        };

        public void DrawBackground(int textureId)
        {
            base.Bind(textureId);

            base.Draw(
                texCoordinates,
                coordinates
                );
        }
    }
}