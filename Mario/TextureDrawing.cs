using System;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace Mario
{
    //родительский класс для текстур, необходим для того, чтобы избежать дубликаты кода, так как отрисовка
    //везде одиннаковая, отличается лишь id текстуры и координатами бокса и положения текстуры
    internal class TextureDrawing
    {
        //метод Bind позволяет связать текстуру с целевым объектом
        public void Bind(int textureId)
        {
            GL.Enable(EnableCap.Texture2D);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.Enable(EnableCap.Blend);
            GL.BindTexture(TextureTarget.Texture2D, textureId);
        }

        //метод отрисовки, идея в том, чтобы координатам бокса сопоставить координаты текстуры
        public void Draw(float[,] texCoordinates, float[,] vertexCoordinate)
        {
            GL.Begin(PrimitiveType.Quads);

            //TexCoord2 - координата точки бокса
            //Vertex2 - координата точки текстуры

            GL.TexCoord2(texCoordinates[0, 0], texCoordinates[1, 0]);
            GL.Vertex2(vertexCoordinate[0, 0], vertexCoordinate[1, 0]);

            GL.TexCoord2(texCoordinates[0, 1], texCoordinates[1, 1]);
            GL.Vertex2(vertexCoordinate[0, 1], vertexCoordinate[1, 1]);

            GL.TexCoord2(texCoordinates[0, 2], texCoordinates[1, 2]);
            GL.Vertex2(vertexCoordinate[0, 2], vertexCoordinate[1, 2]);

            GL.TexCoord2(texCoordinates[0, 3], texCoordinates[1, 3]);
            GL.Vertex2(vertexCoordinate[0, 3], vertexCoordinate[1, 3]);

            GL.End();
        }
    }
}