using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Mario
{
    //класс игрока
    internal class Player : TextureDrawing
    {
        //устанавливаем координаты текстуры по x и y
        public float[] xCoordinates = new float[] { -0.8f, -0.75f, -0.75f, -0.8f };
        public float[] yCoordinates = new float[] { -0.89f, -0.89f, -0.8f, -0.8f };

        //устанаваливаем координаты бокса
        public float[,] texCoordinates = new float[,] { { 0f, 1f, 1f, 0f }, { 1f, 1f, 0f, 0f } };

        //переменная для хранения y координаты земли
        public float initialY = -0.89f;

        //флаг для окончания игры
        public bool finish = false;

        //Списки труб, кирпичей и лаки блоков
        List<Pipe> pipes;
        List<Brick> bricks;
        List<MysteryBlock> mysteryBlocks;

        //переменные для спрайта бега марио

        //кол-во рядов в спрайте
        private int rows = 1,
            //количество колонок в спрайте
            columns = 3,
            //кол-во анимаций в выбранном спрайте
            choosenAnimationFrameCount = 3,
            //номер текущей картинки в спрайте
            choosenAnimationFrameNumber = 0,
            //номер текущего ряда картинок в спрайте
            choosenAnimationRowNumber = 0,
            //переменная для искусственной задержки между сменой картинок во время бега
            frameDelay = 0;

        //переменные ширины и высоты одной картинки в спрайте
        private float frameWidth, frameHeight;

        //флаг для проверки разворота марио
        public bool turnedLeft = false;
        //флаг для проверки бега марио
        public bool run = false;

        //флаг для проверки нахождения марио на земле
        public bool isGrounded = true;

        //переменная гравитации (скорости падения марио на землю
        public float gravity = 0.02f;

        //переменная - дистанция между координатами по оси x
        public float xDistance = 0.05f;
        //переменная - дистанция между координатами по оси y
        public float yDistance = 0.09f;

        //конструктор с параметрами, принимаем списки труб, кирпиче и лаки блоков
        public Player(List<Pipe> pipes, List<Brick> bricks, List<MysteryBlock> mysteryBlocks)
        {
            //рассчитываем ширину одной картинки в спрайте бега марио
            frameWidth = 1.0f / columns;
            //рассчитываем высоту одной картинки в спрайте бега марио
            frameHeight = 1.0f / rows;

            this.pipes = pipes;
            this.bricks = bricks;
            this.mysteryBlocks = mysteryBlocks;

        }

        //отрисовываем марио, когда он стоит
        public void DrawPlayer(int textureId)
        {
            //биндим текстуру
            base.Bind(textureId);

            //отрисовываем марио
            base.Draw(
                texCoordinates,

                new float[,]
                {
                    {xCoordinates[0], xCoordinates[1], xCoordinates[2], xCoordinates[3] },
                    {yCoordinates[0], yCoordinates[1], yCoordinates[2], yCoordinates[3] },
                }
            );

            //вызываем падение марио
            Fall();
        }

        //отрисовываем марио, когда он бежит
        public void RunPlayer(int textureId)
        {
            //биндим текстуру
            base.Bind(textureId);

            //координаты для бокса на основе текущей картинки в спрайте
            float x = choosenAnimationFrameNumber * frameWidth;
            float y = choosenAnimationRowNumber * frameHeight;

            //отрисовываем текстуру из спрайта бега марио
            base.Draw(
                new float[,] {
                    { x, x + frameWidth, x + frameWidth, x},
                    { y+ frameHeight, y + frameHeight, y , y },
                },

                new float[,]
                {
                    {xCoordinates[0], xCoordinates[1], xCoordinates[2], xCoordinates[3] },
                    {yCoordinates[0], yCoordinates[1], yCoordinates[2], yCoordinates[3] },
                }
            );

            //увеличиваем задержку между сменой картинки в спрайте
            frameDelay++;

            //если задержка больше 10, меняем картинку
            if (frameDelay > 10)
            {
                //увеличиваем номер картинки в спрайте бега
                choosenAnimationFrameNumber++;
                //присваиваем значение остатка от деления на количество картинок в спрайте, так как
                //choosenAnimationFrameNumber может принимать значение, большее, чем количество картинок в спрайте
                //Принятый способ для вычисления номера картинки в спрайте
                choosenAnimationFrameNumber %= choosenAnimationFrameCount;
                //обнуляем задержку
                frameDelay = 0;
            }
            
            //устанавливаем флаг бега на ложь
            run = false;

            //вызываем метод падения
            Fall();
        }

        //метод для изменения положения марио относительно оси OX
        public void OnMove(int align)
        {
            //конструкция switch-case на основе значения направления движения (вправо или влево)
            switch (align)
            {
                //0 - движемся влево
                case 0:
                    //если мы повернуты вправо, то поворачиваем влево
                    if (!turnedLeft)
                    {
                        TurnPlayer();
                        turnedLeft = true;
                    }

                    //отнимаем значение каждой x координаты (так как движемся влево)
                    for (int i = 0; i < xCoordinates.Length; i++)
                    {
                        xCoordinates[i] -= 0.004f;
                    }
                    break;
                //1 - движение вправо
                case 1:
                    //если мы повернуты влево, то поворачиваем вправо
                    if (turnedLeft)
                    {
                        TurnPlayer();
                        turnedLeft = false;
                    }

                    //прибавляем значение каждой x координаты (так как движемся вправо)
                    for (int i = 0; i < xCoordinates.Length; i++)
                    {
                        xCoordinates[i] += 0.004f;
                    }
                    break;
            }

            //ограничиваем выход за карту при движении вправо
            if (!turnedLeft && xCoordinates[1] > 1f)
            {
                xCoordinates[0] = 0.95f;
                xCoordinates[1] = 1f;
                xCoordinates[2] = 1f;
                xCoordinates[3] = 0.95f;
            }
            //ограничиваем выход за карту при движении влево
            else if (turnedLeft && xCoordinates[1] < -1f)
            {
                xCoordinates[0] = -0.95f;
                xCoordinates[1] = -1f;
                xCoordinates[2] = -1f;
                xCoordinates[3] = -0.95f;
            }

        }

        //метод для поворота марио в другую сторону (вправо, влево)
        public void TurnPlayer()
        {
            //создаем массив из текущих координат по x
            float[] tempCoordsX = new float[] { xCoordinates[0], xCoordinates[1], xCoordinates[2], xCoordinates[3] };

            //меняем местами соседние координаты
            xCoordinates[0] = tempCoordsX[1];
            xCoordinates[1] = tempCoordsX[0];
            xCoordinates[2] = tempCoordsX[3];
            xCoordinates[3] = tempCoordsX[2];
        }

        //метод для прыжка
        public void Jump()
        {
            //можем прыгать, только если мы на земле или платформе
            if (isGrounded)
            {
                //находим высоту прыжка (максимальная 0.5f)
                float jumpForce = FindJumpForce();

                //увеличиваем каждую координату по y
                for (int i = 0; i < yCoordinates.Length; i++)
                {
                    yCoordinates[i] += jumpForce;
                }

                //во время прыжка мы не на земле
                isGrounded = false;
            }
        }

        //метод падения марио
        public void Fall()
        {
            //если мы не на земле или не на платформе
            if (!isGrounded || !CheckPlatform())
            {
                //уемньшаем координаты по y
                for (int i = 0; i < yCoordinates.Length; i++)
                {
                    yCoordinates[i] -= gravity;
                }

                //проверяем после уменьшения координат стоим ли на платформе
                if (CheckPlatform())
                {
                    isGrounded = true;
                }
            } 
        }

        //метод для проверки стоим ли на платформе
        public bool CheckPlatform()
        {

            //если нижняя координата по y меньше координаты земли, то мы на земле
            if (yCoordinates[0] < initialY)
            {
                //вызываем метод для проверки стоит ли перед нами труба
                CheckPipe();
                return true;
            }

            
            //если наши координаты по y на трубе и по x мы в пределах трубы, то мы на трубе
            for (int i = 0; i < pipes.Count; i++)
            {
                if (yCoordinates[0] < pipes[i].yCoordinates[2] && yCoordinates[0] > pipes[i].yCoordinates[0])
                {
                    if ((!turnedLeft
                        &&((xCoordinates[1] > pipes[i].xCoordinates[0] && xCoordinates[0] < pipes[i].xCoordinates[0])
                        || (xCoordinates[0] < pipes[i].xCoordinates[1] && xCoordinates[1] > pipes[i].xCoordinates[1])
                        || (xCoordinates[0] > pipes[i].xCoordinates[0] && xCoordinates[1] < pipes[i].xCoordinates[1])))
                        || 
                        (turnedLeft 
                        && ((xCoordinates[1] < pipes[i].xCoordinates[1] && xCoordinates[0] > pipes[i].xCoordinates[1])
                        || (xCoordinates[1] < pipes[i].xCoordinates[0] && xCoordinates[0] > pipes[i].xCoordinates[0])
                        || (xCoordinates[1] > pipes[i].xCoordinates[0] && xCoordinates[0] < pipes[i].xCoordinates[1]))))
                    {
                        yCoordinates[0] = pipes[i].yCoordinates[2];
                        yCoordinates[1] = pipes[i].yCoordinates[2];
                        yCoordinates[2] = pipes[i].yCoordinates[2] + yDistance;
                        yCoordinates[3] = pipes[i].yCoordinates[2] + yDistance;
                        return true;
                    }
                }
            }

            //если наши координаты по y на кирпиче и по x мы в пределах кирпича, то мы на кирпиче
            for (int i = 0; i < bricks.Count; i++)
            {
                if (yCoordinates[0] <= bricks[i].yCoordinates[2] && yCoordinates[0] > bricks[i].yCoordinates[0])
                {
                    if ((!turnedLeft 
                        && ((xCoordinates[1] > bricks[i].xCoordinates[0] && xCoordinates[0] < bricks[i].xCoordinates[0])
                        || (xCoordinates[0] < bricks[i].xCoordinates[1] && xCoordinates[1] > bricks[i].xCoordinates[1])
                        || (xCoordinates[0] > bricks[i].xCoordinates[0] && xCoordinates[1] < bricks[i].xCoordinates[1]))
                        || 
                        (turnedLeft 
                        && ((xCoordinates[1] < bricks[i].xCoordinates[1] && xCoordinates[0] > bricks[i].xCoordinates[1])
                        || (xCoordinates[1] < bricks[i].xCoordinates[0] && xCoordinates[0] > bricks[i].xCoordinates[0])
                        || (xCoordinates[1] > bricks[i].xCoordinates[0] && xCoordinates[0] < bricks[i].xCoordinates[1])))
                        ))
                    {
                        yCoordinates[0] = bricks[i].yCoordinates[2];
                        yCoordinates[1] = bricks[i].yCoordinates[2];
                        yCoordinates[2] = bricks[i].yCoordinates[2] + yDistance;
                        yCoordinates[3] = bricks[i].yCoordinates[2] + yDistance;
                        return true;
                    }
                }
            }

            //если наши координаты по y на лаки блоке и по x мы в пределах лаки блоке, то мы на лаки блоке
            for (int i = 0; i < mysteryBlocks.Count; i++)
            {
                if (yCoordinates[0] <= mysteryBlocks[i].yCoordinates[2] && yCoordinates[0] > mysteryBlocks[i].yCoordinates[0])
                {
                    if ((!turnedLeft 
                        && ((xCoordinates[1] > mysteryBlocks[i].xCoordinates[0] && xCoordinates[0] < mysteryBlocks[i].xCoordinates[0])
                        || (xCoordinates[0] < mysteryBlocks[i].xCoordinates[1] && xCoordinates[1] > mysteryBlocks[i].xCoordinates[1])
                        || (xCoordinates[0] > mysteryBlocks[i].xCoordinates[0] && xCoordinates[1] < mysteryBlocks[i].xCoordinates[1]))
                        || 
                        (turnedLeft 
                        && ((xCoordinates[1] < mysteryBlocks[i].xCoordinates[1] && xCoordinates[0] > mysteryBlocks[i].xCoordinates[1])
                        || (xCoordinates[1] < mysteryBlocks[i].xCoordinates[0] && xCoordinates[0] > mysteryBlocks[i].xCoordinates[0])
                        || (xCoordinates[1] > mysteryBlocks[i].xCoordinates[0] && xCoordinates[0] < mysteryBlocks[i].xCoordinates[1])
                        ))
                    ))
                    {
                        yCoordinates[0] = mysteryBlocks[i].yCoordinates[2];
                        yCoordinates[1] = mysteryBlocks[i].yCoordinates[2];
                        yCoordinates[2] = mysteryBlocks[i].yCoordinates[2] + yDistance;
                        yCoordinates[3] = mysteryBlocks[i].yCoordinates[2] + yDistance;
                        return true;
                    }
                }
            }

            //мы не на платформе
            return false;
        }

        //метод для нахождения высоты прыжка
        public float FindJumpForce()
        {
            //максимальная высота прыжка
            float jumpForce = 0.5f;

            //если мы под кирпичем и разница между нижней координатой кирпича и максимальной высотой прыжка отрицательная
            //то находим расстояние между верхней точкой марио и нижней точкой кирпича
            for (int i = 0; i < bricks.Count; i++)
            {
                if (
                    (!turnedLeft 
                    && ((xCoordinates[1] > bricks[i].xCoordinates[0] && xCoordinates[0] < bricks[i].xCoordinates[0])
                    || (xCoordinates[0] < bricks[i].xCoordinates[1] && xCoordinates[1] > bricks[i].xCoordinates[1])
                    || (xCoordinates[0] > bricks[i].xCoordinates[0] && xCoordinates[1] < bricks[i].xCoordinates[1]))
                    || 
                    (turnedLeft 
                    && ((xCoordinates[1] < bricks[i].xCoordinates[1] && xCoordinates[0] > bricks[i].xCoordinates[1])
                    || (xCoordinates[1] < bricks[i].xCoordinates[0] && xCoordinates[0] > bricks[i].xCoordinates[0])
                    || (xCoordinates[1] > bricks[i].xCoordinates[0] && xCoordinates[0] < bricks[i].xCoordinates[1])))
                    ))
                {
                    if (
                        yCoordinates[0] < bricks[i].yCoordinates[0] && bricks[i].yCoordinates[0] - jumpForce < 0
                        )
                    {
                        Console.WriteLine(xCoordinates[0] + " : " + xCoordinates[1]);
                        Console.WriteLine(bricks[i].xCoordinates[0] + " : " + bricks[i].xCoordinates[1]);
                        return bricks[i].yCoordinates[0] - yCoordinates[2];
                    }
                }
            }

            //если мы под лаки блоком и разница между нижней координатой лаки блока и максимальной высотой прыжка отрицательная
            //то находим расстояние между верхней точкой марио и нижней точкой лаки блока

            for (int i = 0; i < mysteryBlocks.Count; i++)
            {
                if (
                    (!turnedLeft &&
                    ((xCoordinates[1] > mysteryBlocks[i].xCoordinates[0] && xCoordinates[0] < mysteryBlocks[i].xCoordinates[0])
                    || (xCoordinates[0] < mysteryBlocks[i].xCoordinates[1] && xCoordinates[1] > mysteryBlocks[i].xCoordinates[1])
                    || (xCoordinates[0] > mysteryBlocks[i].xCoordinates[0] && xCoordinates[1] < mysteryBlocks[i].xCoordinates[1]))
                    )
                    || (turnedLeft 
                    && ((xCoordinates[1] < mysteryBlocks[i].xCoordinates[1] && xCoordinates[0] > mysteryBlocks[i].xCoordinates[1])
                    || (xCoordinates[1] < mysteryBlocks[i].xCoordinates[0] && xCoordinates[0] > mysteryBlocks[i].xCoordinates[0])
                    || (xCoordinates[1] > mysteryBlocks[i].xCoordinates[0] && xCoordinates[0] < mysteryBlocks[i].xCoordinates[1]))
                    ))
                {
                    if (
                        yCoordinates[0] < mysteryBlocks[i].yCoordinates[2] && mysteryBlocks[i].yCoordinates[0] - jumpForce < 0
                        )
                    {
                        mysteryBlocks[i].hit = true;
                        return mysteryBlocks[i].yCoordinates[0] - yCoordinates[2];
                    }
                }
            }

            //иначе возвращаем максимальную высоту прыжка
            return jumpForce;
        }

        //проверка на трубу перед марио
        public void CheckPipe()
        {
            //если движемся вправо и правая граница марио больше левой границы трубы и левая граница
            //марио меньше левой границы трубы, то марио наткнулся на трубу
            
            //если движемся влево и левая граница марио меньше правой границы трубы и првая граница марио
            //больше правой границы трубы, то марио наткнулся на трубу
            for (int i = 0; i < pipes.Count; i++)
            {
                if (yCoordinates[0] < pipes[i].yCoordinates[2])
                {
                    if ((!turnedLeft && xCoordinates[1] > pipes[i].xCoordinates[0]
                        && xCoordinates[0] < pipes[i].xCoordinates[0]))
                    {
                        xCoordinates[0] = pipes[i].xCoordinates[0] - xDistance;
                        xCoordinates[1] = pipes[i].xCoordinates[0];
                        xCoordinates[2] = pipes[i].xCoordinates[0];
                        xCoordinates[3] = pipes[i].xCoordinates[0] - xDistance;
                    }

                    if (turnedLeft && xCoordinates[1] < pipes[i].xCoordinates[1] 
                        && xCoordinates[0] > pipes[i].xCoordinates[1])
                    {
                        xCoordinates[0] = pipes[i].xCoordinates[1] + xDistance;
                        xCoordinates[1] = pipes[i].xCoordinates[1];
                        xCoordinates[2] = pipes[i].xCoordinates[1];
                        xCoordinates[3] = pipes[i].xCoordinates[1] + xDistance;
                    }
                }
            }
        }

        //проверка на окончание игры
        public int CheckFinish()
        {
            //если координата по x больше 0.65f, то есть марио около замка, то игра окончена
            if (xCoordinates[0] > 0.65f)
            {
                return 2;
            }
            else
            {
                return 1;
            }
        }
    }
}