using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mario
{
    //класс кирпичной плитки, наследуется от класса, созданного для отрисовки текстуры
    internal class Brick : TextureDrawing
    {
        //координаты кирпичной плиты
        public float[] xCoordinates = new float[] { };
        public float[] yCoordinates = new float[] { };

        //координаты для бокса, внутри которого будет помещена текстура плитки
        private float[,] texCoordinates = new float[,]
        {
            //x
            { 0f, 1f, 1f, 0f },
            //y
            { 1f, 1f, 0f, 0f }
        };

        //конструктор с параметрами, принимает массивы координат по осям x и y
        public Brick(float[] xCoordinates, float[] yCoordinates)
        {
            this.xCoordinates = xCoordinates;
            this.yCoordinates = yCoordinates;
        }

        //метод для отрисовки кирпичной плитки
        public void DrawBrick(int textureId)
        {
            //биндим текстуру
            base.Bind(textureId);

            //отрисовываем текстуру
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
