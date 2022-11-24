using System;
using System.Runtime.CompilerServices;
using SFML.Graphics;
using SFML.Learning;
using SFML.Window;


class Program : Game
{
    /// <summary>
    /// Координата X игрока.
    /// </summary>
    static float _playerX = 30;

    /// <summary>
    /// Координата Y игрока.
    /// </summary>
    static float _playerY = 220;
    
    /// <summary>
    /// Скорость движения игрока.
    /// </summary>
    private static float _playerSpeed = 400;

    /// <summary>
    /// Направление движения игрока.
    /// </summary>
    private static int _playerDirection = 1;

    /// <summary>
    /// Размер игрока.
    /// </summary>
    private static int _playerSize = 32;

    /// <summary>
    /// Очки игрока.
    /// </summary>
    private static int _playerScore = 0;

    /// <summary>
    /// Игровой бог.
    /// </summary>
    private static Random _random;

    /// <summary>
    /// Координата X еды.
    /// </summary>
    private static int _foodX;

    /// <summary>
    /// Координата Y еды.
    /// </summary>
    private static int _foodY;

    /// <summary>
    /// Размеры еды по вертикали и горизонтали.
    /// </summary>
    private static int _foodSize = 32;

    static void Main(string[] args)
    {
        InitWindow(800, 600, "Моя игра");

        _random = new Random((int)(DateTime.Now.Ticks & 0xFFFFFFFF));
        _foodX = _random.Next(0, 800 - _foodSize);
        _foodY = _random.Next(0, 600 - _foodSize);

        while (true)
        {
            //1. Расчёт.
            DispatchEvents();

            if (GetKey(Keyboard.Key.Escape)) break;
            
            MovePlayer();

            //обнаружение столкновения с едой
            if (_playerX + _playerSize > _foodX && _foodX + _foodSize > _playerX && _playerY + _playerSize > _foodY && _foodY + _foodSize > _playerY)
            {
                //разместить новую еду
                _foodX = _random.Next(0, 800 - _foodSize);
                _foodY = _random.Next(0, 600 - _foodSize);

                //посчитать очки и повысить сложность игры
                _playerScore += 1;
                _playerSpeed += 10;
            }

            //обнаружение выхода за пределы экрана (проигрыш)
            if (_playerX < 0 || _playerX > 800 - _playerSize || _playerY < 0 || _playerY > 600 - _playerSize)
            {

            }

            //Игровая логика.

            //2. Очистка буфера и окна.
            ClearWindow(100, 149, 237);

            //3. Отрисовка буфера в окне.

            //Вызов методов отрисовки объектов.

            DrawPlayer();
            DrawFood();

            DisplayWindow();

            //5. Ожидание
            Delay(1);
        }
    }

    /// <summary>
    /// Перемещение игрока.
    /// </summary>
    private static void MovePlayer()
    {
        if (GetKey(Keyboard.Key.Up)) _playerDirection = 0;
        if (GetKey(Keyboard.Key.Right)) _playerDirection = 1;
        if (GetKey(Keyboard.Key.Down)) _playerDirection = 2;
        if (GetKey(Keyboard.Key.Left)) _playerDirection = 3;

        if (_playerDirection == 0) _playerY -= _playerSpeed * DeltaTime;
        if (_playerDirection == 1) _playerX += _playerSpeed * DeltaTime;
        if (_playerDirection == 2) _playerY += _playerSpeed * DeltaTime;
        if (_playerDirection == 3) _playerX -= _playerSpeed * DeltaTime;
    }

    /// <summary>
    /// Отрисовка игрока.
    /// </summary>
    private static void DrawPlayer()
    {
        SetFillColor(Color.White);
        FillRectangle(_playerX, _playerY, _playerSize, _playerSize);
        //FillCircle(_playerX, _playerY, _playerSize/2);
    }

    private static void DrawFood()
    {
        SetFillColor(Color.Magenta);
        FillCircle(_foodX, _foodY, _foodSize/2);
        SetFillColor(Color.Red);
        FillRectangle(_foodX, _foodY, _foodSize, _foodSize);
    }
}

