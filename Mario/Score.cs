using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mario
{
    //Класс счета очков
    internal class Score : TextureDrawing
    {
        //координаты картинки монетки в левом верхнем углу
        private float[] xCoordinates = new float[] { -0.9f, -0.85f, -0.85f, -0.9f };
        private float[] yCoordinates = new float[] { 0.85f, 0.85f, 0.97f, 0.97f };

        //координаты цифры - счета кол-ва собранных монет рядом с картинкой монетки
        private float[] CoinXCoordinates = new float[] { -0.97f, -0.9f, -0.9f, -0.97f };
        private float[] CoinYCoordinates = new float[] { 0.85f, 0.85f, 0.97f, 0.97f };

        //координаты бокса
        public float[,] texCoordinates = new float[,] { { 0f, 1f, 1f, 0f }, { 1f, 1f, 0f, 0f } };

        //переменные для спрайта цифр

        //кол-во рядов в спрайте
        private int rows = 1,
            //количество колонок в спрайте
            columns = 6,
            //кол-во анимаций в выбранном спрайте
            choosenAnimationFrameCount = 6,
            //номер текущей картинки в спрайте
            choosenAnimationFrameNumber = 0,
            //номер текущего ряда картинок в спрайте
            choosenAnimationRowNumber = 0;

        //переменные ширины и высоты одной картинки в спрайте
        private float frameWidth, frameHeight;

        //конструктор без параметров
        public Score()
        {
            //рассчитываем ширину одной картинки в спрайте
            frameWidth = 1.0f / columns;
            //рассчитываем высоту одной картинки в спрайте
            frameHeight = 1.0f / rows;
        }

        //отрисовываем счет, принимаем вторым параметром текущий счет собранных монеток
        public void DrawScore(int textureId, int score)
        {
            //биндим текстуру
            base.Bind(textureId);

            //координаты для бокса на основе текущей картинки в спрайте
            float x = choosenAnimationFrameNumber * frameWidth;
            float y = choosenAnimationRowNumber * frameHeight;

            base.Draw(
                //массив координат бокса
               new float[,] {
                    { x, x + frameWidth, x + frameWidth, x},
                    { y+ frameHeight, y + frameHeight, y , y },
               },
               //массив координат положения текстуры
               new float[,]
               {
                    {xCoordinates[0], xCoordinates[1], xCoordinates[2], xCoordinates[3] },
                    {yCoordinates[0], yCoordinates[1], yCoordinates[2], yCoordinates[3] },
               }
           );

            //устанавливаем номер картинки равный текущему счету собранных монеток
            choosenAnimationFrameNumber = score;
            //присваиваем значение остатка от деления на количество картинок в спрайте, так как
            //choosenAnimationFrameNumber может принимать значение, большее, чем количество картинок в спрайте
            //Принятый способ для вычисления номера картинки в спрайте
            choosenAnimationFrameNumber %= choosenAnimationFrameCount;
        }

        //отрисовываем монетку в левом верхнем углу
        public void DrawCoin(int textureId)
        {
            //биндим текстуру
            base.Bind(textureId);

            //отрисовываем текстуру
            base.Draw(
                texCoordinates,
                new float[,]
                {
                    {CoinXCoordinates[0], CoinXCoordinates[1], CoinXCoordinates[2], CoinXCoordinates[3] },
                    {CoinYCoordinates[0], CoinYCoordinates[1], CoinYCoordinates[2], CoinYCoordinates[3] },
                }
                );
        }
    }
}