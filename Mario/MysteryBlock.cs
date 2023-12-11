using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mario
{
    //класс лаки блока
    internal class MysteryBlock : TextureDrawing
    {
        //кординаты по x и y
        public float[] xCoordinates = new float[] { };
        public float[] yCoordinates = new float[] { };

        //флаг, содержит ли блок монетку (изначально истина)
        public bool coin = true;
        //флаг, был ли удар марио о блок (изначально ложь)
        public bool hit = false;

        //кординаты для бокса
        private float[,] texCoordinates = new float[,]
        {
            { 0f, 1f, 1f, 0f },
            { 1f, 1f, 0f, 0f }
        };

        //конструктор с параметрами, принимает координаты по x и y
        public MysteryBlock(float[] xCoordinates, float[] yCoordinates)
        {
            this.xCoordinates = xCoordinates;
            this.yCoordinates = yCoordinates;
        }

        //метод отрисовки лаки блока
        public void DrawMysteryBlock(int textureId)
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
