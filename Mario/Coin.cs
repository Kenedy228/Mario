using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mario
{
    //класс для монетки, которая появляется при столкновении марио и мистери блока
    //анимация монетки состоит из 2-ух частей (подъем монетки и опускание)
    internal class Coin : TextureDrawing
    {
        //массивы для координат
        public float[] xCoordinates = new float[] { };
        public float[] yCoordinates = new float[] { };

        //переменная счетчик - искуственно задаем длительность анимации монетки
        public int counter = 0;
        //булевая переменная, которая будет указывать, как должна вести себя монетка в момент анимации
        //подниматься или опускаться (изначально подниматься)
        public bool up = true;
        //флаг для начала анимации (нужен для вызова анимации из класса Game)
        public bool startCoinAnimation = false;

        //координаты для бокса, внутри которого помещена текстура монетки
        private float[,] texCoordinates = new float[,]
        {
            { 0f, 1f, 1f, 0f },
            { 1f, 1f, 0f, 0f }
        };

        //конструктор с параметрами, принимает массивы координат по x и y
        public Coin(float[] xCoordinates, float[] yCoordinates)
        {
            this.xCoordinates = xCoordinates;
            this.yCoordinates = yCoordinates;
        }

        //метод для отрисовки монетки
        public void DrawCoin(int textureId)
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

            //вызов метода для изменения положения монетки
            AnimationHandler();
        }

        //метод для изменения положения монетки
        public void AnimationHandler()
        {
            //если монетка поднимается, увеличиваем ее высоту
            if (up)
            {
                for (int i = 0; i < yCoordinates.Length; i++)
                {
                    yCoordinates[i] += 0.005f;
                }
                //увеличиваем счетчик
                counter++;
            }
            //если монетка опускается, уменьшаем ее высоту
            else
            {
                for (int i = 0; i < yCoordinates.Length; i++)
                {
                    yCoordinates[i] -= 0.005f;
                }
                //уменьшаем счетчик
                counter--;
            }

            //если счетчик больше 20, тогда опускаем монетку
            if (counter > 20)
            {
                up = false;
            }

            //счетчик равен 0 и монетка опускалась, то анимацию прекращаем
            if (!up && counter == 0)
            {
                startCoinAnimation = false;
            }
        }
    }
}