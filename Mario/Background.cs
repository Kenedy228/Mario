using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

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

        private Color4 maskColor = Color4.White;

        public void DrawBackground(int textureId)
        {
            base.Bind(textureId);

            GL.Color4(maskColor);

            base.Draw(
                texCoordinates,
                coordinates
                );
        }
    }
}