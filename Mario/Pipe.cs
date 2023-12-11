using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mario
{
    //класс трубы
    internal class Pipe : TextureDrawing
    {
        //координаты по x и y
        public float[] xCoordinates;
        public float[] yCoordinates;

        //координаты бокса
        private float[,] texCoordinates = new float[,]
        {
            { 0f, 1f, 1f, 0f },
            { 1f, 1f, 0f, 0f }
        };

        //конструктор с параметрами, устанавливаем значения координат по x и y
        public Pipe(float[] xCoordinates, float[] yCoordinates)
        {
            this.xCoordinates = xCoordinates;
            this.yCoordinates = yCoordinates;
        }

        //отрисовываем трубу
        public void DrawPipe(int textureId)
        {
            //биндим текстуру
            base.Bind(textureId);

            //отрисовываем текстуру
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
