using System;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.Common;
using System.Drawing;
using OpenTK.Windowing.Common.Input;
using Mario;

namespace FlappyBird
{
    class Program
    {
        static void Main()
        {
            //создаем объект Bitmap, передавая путь нашей картинки
            Bitmap image = new Bitmap("Textures/mario-stay.png");

            //объект иконки из пространства имен OpenTK
            //принимает ширину, высоту и массив байтов из цветов каждого пикселя картинки 
            OpenTK.Windowing.Common.Input.Image icon = new OpenTK.Windowing.Common.Input.Image(image.Width, image.Height, GetPixels(image));

            //объект класса GameWindowSettings - необходимо передавать в класс Game
            GameWindowSettings gSettings = new GameWindowSettings() 
            {
                //задаем частоту обновления кадров
                UpdateFrequency = 60.0
            };
            //объект класса NativeWindowSettings - необходимо передавать в класс Game
            NativeWindowSettings nSettings = new NativeWindowSettings()
            {
                //Название окна
                Title = "Mario",
                //Размер окна
                Size = (800, 600),
                //Дословно означает установку текущего профиля графики
                //Значение Default устанавливает значение ForwardCompatible, которое позволяет запустить
                //игру на macOS
                Flags = ContextFlags.Default,
                //Дословно означает профиль API текущей графики
                Profile = ContextProfile.Compatability,
                //Иконка окна в таксбаре и слева вверху окна
                Icon = new WindowIcon(icon),
            };

            //создание объекта класса Game
            Game game = new Game(gSettings, nSettings);
            //Запуск игры
            game.Run();
        }

        //обработка пикселей, составление массива байтов из каждого пикселя иконки
        static byte[] GetPixels(Bitmap image)
        {
            //массив байтов для хранения пикселей (умножаем на 4 так как 4 составляющих пикселя)
            //R - красный цвет
            //G - зеленый цвет
            //B - синий цвет
            //A - прозрачность
            byte[] pixels = new byte[image.Width * image.Height * 4];

            int index = 0;

            for (int i = 0; i < image.Height; i++)
            {
                for (int j = 0; j < image.Width; j++)
                {
                    pixels[index] = image.GetPixel(j, i).R;
                    index++;
                    pixels[index] = image.GetPixel(j, i).G;
                    index++;
                    pixels[index] = image.GetPixel(j, i).B;
                    index++;
                    pixels[index] = image.GetPixel(j, i).A;
                    index++;
                }
            }

            return pixels;
        }

    }
}