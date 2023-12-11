using System;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Drawing.Imaging;

namespace Mario
{
    //класс для обработки текстуры для дальнейшего использования в игре
    class GameTextures
    {
        //метод загрузки текстуры, принимает путь текстуры
        static public int LoadTextures(string path)
        {
            //выдаем исключение, если по указанному пути не файла
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"Загружаемый файл не найден по пути {path}");
            }

            //создаем id текстуры, который генерирует OpenGL
            int id = GL.GenTexture();
            //биндим текстуру с целевым объектом
            GL.BindTexture(TextureTarget.Texture2D, id);

            //создаем объект текстуры
            Bitmap bmp = new Bitmap(path);
            //сохраняем данные о текстуре в системную память
            BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            //описываем 2D текстуру
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

            //удаляем данные об текстуре в системной памяти
            bmp.UnlockBits(data);

            //устанавливаем параметры текстуры
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
                (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter,
                (int)TextureMagFilter.Linear);

            //возвращаем id текстуры
            return id;
        }
    }
}