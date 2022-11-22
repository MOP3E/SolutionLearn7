using System;
using SFML.Learning;


class Program : Game
{
    static void Main(string[] args)
    {
        InitWindow(800, 600, "Моя игра");

        while (true)
        {
            //1. Расчёт.
            DispatchEvents();

            //Игровая логика.

            //2. Очистка буфера и окна.
            ClearWindow(100, 149, 237);

            //3. Отрисовка буфера в окне.

            //Вызов методов отрисовки объектов.

            DisplayWindow();

            //5. Ожидание
            Delay(1);
        }
    }
}

