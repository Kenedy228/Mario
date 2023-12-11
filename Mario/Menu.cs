using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;

namespace Mario
{
    //класс меню
    internal class Menu : TextureDrawing
    {
        //координаты для кнопки Играть
        public float[,] playCoordinates = {
            { -0.12f, 0.12f, 0.12f, -0.12f },
            { 0.05f, 0.05f, -0.05f, -0.05f }
        };

        //координаты для кнопки выход
        public float[,] exitCoordinates = {
            { -0.12f, 0.12f, 0.12f, -0.12f},
            {-0.07f, -0.07f, -0.17f, -0.17f }
        };

        //координаты для кнопки Заново
        public float[,] restartCoordinates = {
            { -0.12f, 0.12f, 0.12f, -0.12f },
            { 0.05f, 0.05f, -0.05f, -0.05f }
        };

        //координаты для кнопки Меню
        public float[,] menuCoordinates = {
            { -0.12f, 0.12f, 0.12f, -0.12f},
            {-0.07f, -0.07f, -0.17f, -0.17f }
        };

        //координаты для бокса, внутри которого будет распологаться текстура кнопки
        public float[,] texCoordinates = new float[,] { { 0f, 1f, 1f, 0f }, { 0f, 0f, 1f, 1f } };

        //флаг для перезапуска игры
        public bool restart = false;
        
        //флаг для отображения статуса игры
        public int gameStatus = 0;

        //статусы игры
        //0: начальное меню (кнопки начать и выход, некоторые текстуры)
        //1: непосредственно игровой процесс
        //2: игра пройдена (кнопки заново и меню)
        //-1: выход из игры

        //метод для отрисовки меню, принимает массив id текстур кнопок и названия кнопок (задаем сами)
        public void DrawMenu(int[] textureIds, string[] buttonNames)
        {
            //цикл для прохождения по массиву id текстур кнопок
            for (int i = 0; i < textureIds.Length; i++)
            {
                //биндим текстуру кнопки
                base.Bind(textureIds[i]);

                //создаем массив координат, которые потом передаем в метод Draw
                float[,] coordinates = new float[,] { };

                //конструкция switch-case для обработки названий кнопок
                //и задания массиву координат coordinates соответствующие координаты
                switch (buttonNames[i])
                {
                    //кнопки Начать
                    case "start":
                        coordinates = playCoordinates;
                        break;
                    //кнопка Выход
                    case "exit":
                        coordinates = exitCoordinates;
                        break;
                    //кнопка Заново
                    case "restart":
                        coordinates = restartCoordinates;
                        break;
                    //кнопка Меню
                    case "menu":
                        coordinates = menuCoordinates;
                        break;
                }

                //отрисовываем кнопку
                base.Draw(
                    texCoordinates,
                    coordinates
                );
            }
        }

        //обрабатываем нажание ЛКМ на место на экране, принимает текущие координаты точки (места),
        //где было нажатие ЛКМ
        public void MouseClickHandler(Vector2 cursorPosition)
        {
            //если статус игры 0
            if (gameStatus == 0)
            {
                //если точка внутри кнопки Начать
                if (IsButton(cursorPosition, playCoordinates))
                {
                    //меняем статус игры
                    gameStatus = 1;
                    //меняем значение флага для перезапуска игры (сделано для перезапуска игры, если после окончания игры
                    //вышли в главное меню)
                    restart = true;
                }
                //если точка внути кнопки Выход
                else if (IsButton(cursorPosition, exitCoordinates))
                {
                    //меняем статус игры
                    gameStatus = -1;
                }
            }
            //если статус игры 2
            else if (gameStatus == 2)
            {
                //если точка внутри кнопки Заново
                if (IsButton(cursorPosition, restartCoordinates))
                {
                    //меняем статус
                    gameStatus = 1;
                    //меняем значение флага для перезапуска игры
                    restart = true;
                }
                //если точка внутри кнопки Меню
                else if (IsButton(cursorPosition, menuCoordinates))
                {
                    //меняем статус игры
                    gameStatus = 0;
                }
            }
        }

        //метод для проверки положения кнопки относительно точки при нажатии ЛКМ (первый параметр)
        //и переданных координат кнопки (второй параметр)
        public bool IsButton(Vector2 cursorPosition, float[,] coordinates)
        {
            if
                //условие, если точка находится внутри кнопки
                (
                cursorPosition.X > coordinates[0, 0] && cursorPosition.X < coordinates[0, 1]
                && cursorPosition.Y > coordinates[1, 2] && cursorPosition.Y < coordinates[1, 0]
                )
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
