using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace Mario
{
    //класс заднего фона
    internal class Background : TextureDrawing
    {
        //массив координат по x и y
        private float[,] coordinates = new float[,]
        {
            {-1, 1, 1, -1 },
            {-1, -1, 1, 1 },
        };

        //массив координат бокса
        private float[,] texCoordinates = new float[,]
        {
            { 0f, 1f, 1f, 0f },
            { 1f, 1f, 0f, 0f }
        };

        //рисуем задний фон
        public void DrawBackground(int textureId)
        {
            //биндим текстуру
            base.Bind(textureId);

            //отрисовываем текстуру
            base.Draw(
                texCoordinates,
                coordinates
                );
        }
    }
}