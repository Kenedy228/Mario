using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;

namespace Mario
{
    internal class Menu : TextureDrawing
    {
        public float[,] playCoordinates = {
            { -0.12f, 0.12f, 0.12f, -0.12f },
            { 0.05f, 0.05f, -0.05f, -0.05f }
        };

        public float[,] exitCoordinates = {
            { -0.12f, 0.12f, 0.12f, -0.12f},
            {-0.07f, -0.07f, -0.17f, -0.17f }
        };

        public float[,] restartCoordinates = {
            { -0.12f, 0.12f, 0.12f, -0.12f },
            { 0.05f, 0.05f, -0.05f, -0.05f }
        };

        public float[,] menuCoordinates = {
            { -0.12f, 0.12f, 0.12f, -0.12f},
            {-0.07f, -0.07f, -0.17f, -0.17f }
        };

        public float[,] texCoordinates = new float[,] { { 0f, 1f, 1f, 0f }, { 0f, 0f, 1f, 1f } };

        public bool restart = false;

        public int gameStatus = 0;

        public void DrawMenu(int[] textureIds, string[] buttonNames)
        {
            for (int i = 0; i < textureIds.Length; i++)
            {
                base.Bind(textureIds[i]);

                float[,] coordinates = new float[,] { };

                switch (buttonNames[i])
                {
                    case "start":
                        coordinates = playCoordinates;
                        break;
                    case "exit":
                        coordinates = exitCoordinates;
                        break;
                    case "restart":
                        coordinates = restartCoordinates;
                        break;
                    case "menu":
                        coordinates = menuCoordinates;
                        break;
                }

                base.Draw(
                    texCoordinates,
                    coordinates
                );
            }
        }

        public void MouseClickHandler(Vector2 cursorPosition)
        {
            if (gameStatus == 0)
            {
                if (IsButton(cursorPosition, playCoordinates))
                {
                    gameStatus = 1;
                    restart = true;
                }
                else if (IsButton(cursorPosition, exitCoordinates))
                {
                    gameStatus = -1;
                }
            }
            else if (gameStatus == 2)
            {
                if (IsButton(cursorPosition, restartCoordinates))
                {
                    gameStatus = 1;
                    restart = true;
                }
                else if (IsButton(cursorPosition, menuCoordinates))
                {
                    gameStatus = 0;
                }
            }
        }

        public bool IsButton(Vector2 cursorPosition, float[,] coordinates)
        {
            if
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
