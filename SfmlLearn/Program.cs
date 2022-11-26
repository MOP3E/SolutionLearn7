using System;
using System.Runtime.CompilerServices;
using SFML.Graphics;
using SFML.Learning;
using SFML.Window;


class Program : Game
{
    /// <summary>
    /// Игровой бог.
    /// </summary>
    private static Random _random;

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
    /// 0 - вверх, 1 - вправо, 2 - вниз, 3 - влево.
    /// </summary>
    private static int _playerDirection = 1;

    /// <summary>
    /// Текстура игрока.
    /// </summary>
    private static string _playerTexture;

    /// <summary>
    /// Положение спрайтов игрока на текстуре игрока по оси X.
    /// </summary>
    private static int[] _playerSpriresX = {64, 0, 0, 64};

    /// <summary>
    /// Положение спрайтов игрока на текстуре игрока по оси Y.
    /// </summary>
    private static int[] _playerSpriresY = {64, 0, 64, 0};

    /// <summary>
    /// Размер спрайтов игрока по оси X.
    /// </summary>
    private static int[] _playerSizeX = {27, 56, 27, 56};

    /// <summary>
    /// Размер спрайтов игрока по оси Y.
    /// </summary>
    private static int[] _playerSizeY = {56, 48, 56, 48};

    /// <summary>
    /// Очки игрока.
    /// </summary>
    private static int _playerScore = 0;

    /// <summary>
    /// Рекорд игрока.
    /// </summary>
    private static int _playerRecord = 0;

    /// <summary>
    /// Звук съедания еды игроком.
    /// </summary>
    private static string _eatSound;

    /// <summary>
    /// Звук столкновения игрока с границами экрана.
    /// </summary>
    private static string _crashSound;

    /// <summary>
    /// Координата X еды.
    /// </summary>
    private static int _foodX;

    /// <summary>
    /// Координата Y еды.
    /// </summary>
    private static int _foodY;

    /// <summary>
    /// Текстура еды.
    /// </summary>
    private static string _foodTexture;

    /// <summary>
    /// Размеры еды по оси X.
    /// </summary>
    private static int _foodSizeX = 32;

    /// <summary>
    /// Размеры еды по оси Y.
    /// </summary>
    private static int _foodSizeY = 28;

    /// <summary>
    /// Игрок проиграл.
    /// </summary>
    private static bool _lose = false;

    /// <summary>
    /// Фоновая текстура.
    /// </summary>
    private static string _backgroundTexture;

    /// <summary>
    /// Фоновая музыка.
    /// </summary>
    private static string _backgroundMusic;

    static void Main(string[] args)
    {
        InitWindow(800, 600, "Кошачье безумие");

        _playerTexture = LoadTexture("player.png");
        _foodTexture = LoadTexture("food.png");
        _eatSound = LoadSound("meow_sound.wav");
        _crashSound = LoadSound("cat_crash_sound.wav");

        _backgroundTexture = LoadTexture("background.png");
        _backgroundMusic = LoadMusic("bg_music.wav");
        SetFont("comic.ttf");
        SetFillColor(Color.Black);

        //проинициализировать игрового бога
        _random = new Random((int)(DateTime.Now.Ticks & 0xFFFFFFFF));
        //разместить еду в первый раз
        PlaceFood();
        //включить музыку
        PlayMusic(_backgroundMusic);

        while (true)
        {
            //1. Расчёт.
            DispatchEvents();

            //выход из игры
            if (GetKey(Keyboard.Key.Escape)) break;

            //проверка, не проиграна ли игра
            if(!_lose)
            {
                //игровая логика
                MovePlayer();
                //обнаружение столкновения с едой
                if (_playerX + _playerSizeX[_playerDirection] > _foodX && _foodX + _foodSizeX > _playerX && _playerY + _playerSizeY[_playerDirection] > _foodY &&
                    _foodY + _foodSizeY > _playerY)
                {
                    //мяукнуть
                    PlaySound(_eatSound);
                    //разместить новую еду
                    PlaceFood();

                    //посчитать очки и повысить сложность игры
                    _playerScore += 1;
                    _playerSpeed += 10;
                }

                //обнаружение выхода за пределы экрана (проигрыш)
                if (_playerX < 0 || _playerX > 800 - _playerSizeX[_playerDirection] || _playerY < 168 || _playerY > 600 - _playerSizeY[_playerDirection])
                {
                    //стукнуться об стену
                    PlaySound(_crashSound);
                    //сохранить рекорд
                    if (_playerRecord < _playerScore) _playerRecord = _playerScore;
                    //завершить игру
                    _lose = true;
                }
            }
            else
            {
                //перезапуск по нажатию Enter
                if (GetKey(Keyboard.Key.Return))
                {
                    //перезапуск игры - сбросить проигрыш, очки, заново разместить игрока и еду
                    _lose = false;
                    _playerX = 30;
                    _playerY = 220;
                    _playerSpeed = 400;
                    _playerDirection = 1;
                    _playerScore = 0;
                    PlaceFood();
                }
            }

            //2. Очистка буфера и окна.
            ClearWindow(100, 149, 237);

            //3. Отрисовка буфера в окне.

            //Вызов методов отрисовки объектов.

            Draw();

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
        //проверка нажатия кнопок
        if (GetKey(Keyboard.Key.Up)) _playerDirection = 0;
        if (GetKey(Keyboard.Key.Right)) _playerDirection = 1;
        if (GetKey(Keyboard.Key.Down)) _playerDirection = 2;
        if (GetKey(Keyboard.Key.Left)) _playerDirection = 3;

        //перемещение игрока
        switch (_playerDirection)
        {
            case 0:
                _playerY -= _playerSpeed * DeltaTime;
                break;
            case 1:
                _playerX += _playerSpeed * DeltaTime;
                break;
            case 2:
                _playerY += _playerSpeed * DeltaTime;
                break;
            case 3:
                _playerX -= _playerSpeed * DeltaTime;
                break;
        }
    }

    /// <summary>
    /// Размещение еды.
    /// </summary>
    private static void PlaceFood()
    {
        _foodX = _random.Next(0, 800 - _foodSizeX);
        _foodY = _random.Next(168, 600 - _foodSizeY);
    }

    /// <summary>
    /// Отрисовка текущего состояния экрана.
    /// </summary>
    private static void Draw()
    {
        //нарисовать задний фон
        DrawSprite(_backgroundTexture, 0, 0);
        //нарисовать игрока
        DrawPlayer();
        //нарисовать еду
        if(!_lose) DrawFood();
        //вывести на экран очки игрока
        DrawText(5, 0, $"Съедено {(_playerScore == 0 ? "-" : _playerScore.ToString())}", 24);
        //вывести на экран рекорд игрока
        DrawText(665, 0, $"Рекорд {(_playerRecord == 0 ? "-" : _playerRecord.ToString())}", 24);
        //нарисовать сообщение об окончании игры
        if(_lose)
        {
            DrawText(202, 300, "Ну и чего ты носишься по кухне?!", 24);
            DrawText(147, 350, "Нажми [ENTER] чтобы перезапустить игру!", 24);
        }
    }

    /// <summary>
    /// Отрисовка игрока.
    /// </summary>
    private static void DrawPlayer()
    {
        DrawSprite(_playerTexture,
            _playerX,
            _playerY,
            _playerSpriresX[_playerDirection],
            _playerSpriresY[_playerDirection],
            _playerSizeX[_playerDirection],
            _playerSizeY[_playerDirection]);
    }

    private static void DrawFood()
    {
        DrawSprite(_foodTexture, _foodX, _foodY, 0, 2, _foodSizeX, _foodSizeY);
    }
}

