using System;
using System.IO;
using System.Numerics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace MonoGameLearn
{
    public class Learn : Game
    {
        /// <summary>
        /// Графический менеджер.
        /// </summary>
        private GraphicsDeviceManager _graphics;
        
        /// <summary>
        /// Задание для отрисовки спрайтов.
        /// </summary>
        private SpriteBatch _spriteBatch;
            
        /// <summary>
        /// Игровой бог.
        /// </summary>
        private Random _random;

        /// <summary>
        /// Координаты игрока.
        /// </summary>
        private Vector2 _playerCoords = new Vector2(30, 220);

        /// <summary>
        /// Скорость движения игрока.
        /// </summary>
        private float _playerSpeed = 400;

        /// <summary>
        /// Направление движения игрока.
        /// 0 - вверх, 1 - вправо, 2 - вниз, 3 - влево.
        /// </summary>
        private int _playerDirection = 1;

        /// <summary>
        /// Текстура игрока.
        /// </summary>
        private Texture2D _playerTexture;

        /// <summary>
        /// Положение спрайтов игрока на текстуре игрока.
        /// </summary>
        private Vector2[] _playerSprires = { new Vector2(64, 64), new Vector2(0, 0), new Vector2(0, 64), new Vector2(64, 0) };

        /// <summary>
        /// Размер спрайов игрока.
        /// </summary>
        private Vector2[] _playerSize = { new Vector2(27, 56), new Vector2(56, 48), new Vector2(27, 56), new Vector2(56, 48) };

        /// <summary>
        /// Очки игрока.
        /// </summary>
        private int _playerScore = 0;

        /// <summary>
        /// Рекорд игрока.
        /// </summary>
        private int _playerRecord = 0;
        
        /// <summary>
        /// Звук съедания еды игроком.
        /// </summary>
        private SoundEffect _eatSound;

        /// <summary>
        /// Звук столкновения игрока с границами экрана.
        /// </summary>
        private SoundEffect _crashSound;

        /// <summary>
        /// Текстура еды.
        /// </summary>
        private Texture2D _foodTexture;

        /// <summary>
        /// Координаты еды.
        /// </summary>
        private Vector2 _foodCoords;

        /// <summary>
        /// Размеры еды.
        /// </summary>
        private Vector2 _foodSize = new Vector2(32, 28);
        
        /// <summary>
        /// Игрок проиграл.
        /// </summary>
        private bool _lose = false;

        /// <summary>
        /// Фоновая текстура.
        /// </summary>
        private Texture2D _backgroundTexture;

        /// <summary>
        /// Фоновая музыка.
        /// </summary>
        private SoundEffect _backgroundMusic;

        /// <summary>
        /// Проигрыватель фоновой музыки.
        /// </summary>
        private SoundEffectInstance _backgroundMusicInstance;

        /// <summary>
        /// Шрифт для вывода сообщений на экран.
        /// </summary>
        private SpriteFont _mainFont;

        public Learn()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 600;
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
            Window.AllowUserResizing = false;
            Window.Title = "Кошачье безумие";
        }

        /// <summary>
        /// Инициализация объектов игры.
        /// </summary>
        protected override void Initialize()
        {
            _random = new Random((int)(DateTime.Now.Ticks & 0xFFFFFFFF));
            //разместить еду в первый раз
            PlaceFood();

            base.Initialize();
        }

        /// <summary>
        /// Загрузка контента игры.
        /// </summary>
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(_graphics.GraphicsDevice);

            _playerTexture = Content.Load<Texture2D>(@"player");
            _foodTexture = Content.Load<Texture2D>(@"food");
            _eatSound = SoundEffect.FromStream(File.OpenRead(@"Content\meow_sound.wav"));
            _crashSound = SoundEffect.FromStream(File.OpenRead(@"Content\cat_crash_sound.wav"));

            _backgroundTexture = Content.Load<Texture2D>(@"background");
            _backgroundMusic = SoundEffect.FromStream(File.OpenRead(@"Content\bg_music.wav"));
            _backgroundMusicInstance = _backgroundMusic.CreateInstance();
            _backgroundMusicInstance.IsLooped = true;
            _backgroundMusicInstance.Play();

            _mainFont = Content.Load<SpriteFont>("ComicSans24");
        }

        /// <summary>
        /// Обработка игровой логики.
        /// </summary>
        /// <param name="gameTime">Игровое время.</param>
        protected override void Update(GameTime gameTime)
        {
            //Выход из игры по-умолчанию.
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (!_lose)
            {
                MovePlayer(gameTime);

                //обнаружение столкновения с едой
                if (_playerCoords.X + _playerSize[_playerDirection].X > _foodCoords.X && _foodCoords.X + _foodSize.X > _playerCoords.X && 
                    _playerCoords.Y + _playerSize[_playerDirection].Y > _foodCoords.Y && _foodCoords.Y + _foodSize.Y > _playerCoords.Y)
                {
                    //мяукнуть
                    _eatSound.Play();
                    //разместить новую еду
                    PlaceFood();

                    //посчитать очки и повысить сложность игры
                    _playerScore += 1;
                    _playerSpeed += 10;
                }
            
                //обнаружение выхода за пределы экрана (проигрыш)
                if (_playerCoords.X < 0 || _playerCoords.X > 800 - _playerSize[_playerDirection].X || _playerCoords.Y < 168 || _playerCoords.Y > 600 - _playerSize[_playerDirection].Y)
                {
                    //стукнуться об стену
                    _crashSound.Play();
                    //сохранить рекорд
                    if (_playerRecord < _playerScore) _playerRecord = _playerScore;
                    //завершить игру
                    _lose = true;
                }
            }
            else
            {
                //перезапуск по нажатию Enter
                if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    //перезапуск игры - сбросить проигрыш, очки, заново разместить игрока и еду
                    _lose = false;
                    _playerCoords.X = 30;
                    _playerCoords.Y = 220;
                    _playerSpeed = 400;
                    _playerDirection = 1;
                    _playerScore = 0;
                    PlaceFood();
                }
            }

            base.Update(gameTime);
        }
        
        /// <summary>
        /// Перерисовка экрана.
        /// </summary>
        /// <param name="gameTime">Игровое время.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            DrawScreen();

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Перемещение игрока.
        /// </summary>
        private void MovePlayer(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Up)) _playerDirection = 0;
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad8)) _playerDirection = 0;
            if (Keyboard.GetState().IsKeyDown(Keys.Right)) _playerDirection = 1;
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad6)) _playerDirection = 1;
            if (Keyboard.GetState().IsKeyDown(Keys.Down)) _playerDirection = 2;
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad2)) _playerDirection = 2;
            if (Keyboard.GetState().IsKeyDown(Keys.Left)) _playerDirection = 3;
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad4)) _playerDirection = 4;

            float deltaTime = (float)gameTime.ElapsedGameTime.Milliseconds / 1000.0f;
            if (_playerDirection == 0) _playerCoords.Y -= _playerSpeed * deltaTime;
            if (_playerDirection == 1) _playerCoords.X += _playerSpeed * deltaTime;
            if (_playerDirection == 2) _playerCoords.Y += _playerSpeed * deltaTime;
            if (_playerDirection == 3) _playerCoords.X -= _playerSpeed * deltaTime;
        }

        /// <summary>
        /// Размещение еды.
        /// </summary>
        private void PlaceFood()
        {
            _foodCoords.X = _random.Next(0, 800 - (int)_foodSize.X);
            _foodCoords.Y = _random.Next(168, 600 - (int)_foodSize.Y);
        }
        
        /// <summary>
        /// Отрисовка текущего состояния экрана.
        /// </summary>
        private void DrawScreen()
        {
            //нарисовать задний фон
            _spriteBatch.Draw(_backgroundTexture, Vector2.Zero, Color.White);
            //нарисовать игрока
            DrawPlayer();
            //нарисовать еду
            if(!_lose) DrawFood();
            //вывести на экран очки игрока
            _spriteBatch.DrawString(_mainFont, $"Съедено {(_playerScore == 0 ? "-" : _playerScore.ToString())}", new Vector2(5, 0), Color.Black);
            //вывести на экран рекорд игрока
            _spriteBatch.DrawString(_mainFont, $"Рекорд {(_playerRecord == 0 ? "-" : _playerRecord.ToString())}", new Vector2(665, 0), Color.Black);
            //нарисовать сообщение об окончании игры
            if(_lose)
            {
                _spriteBatch.DrawString(_mainFont, "Ну и чего ты носишься по кухне?!", new Vector2(202, 300), Color.Black);
                _spriteBatch.DrawString(_mainFont, "Нажми [ENTER] чтобы перезапустить игру!", new Vector2(147, 350), Color.Black);
            }
        }

        /// <summary>
        /// Отрисовка игрока.
        /// </summary>
        private void DrawPlayer()
        {
            _spriteBatch.Draw(_playerTexture,
                _playerCoords,
                new Rectangle((int)_playerSprires[_playerDirection].X,
                    (int)_playerSprires[_playerDirection].Y,
                    (int)_playerSize[_playerDirection].X,
                    (int)_playerSize[_playerDirection].Y),
                Color.White);
        }

        private void DrawFood()
        {
            _spriteBatch.Draw(_foodTexture, _foodCoords, new Rectangle(0, 2, (int)_foodSize.X, (int)_foodSize.Y), Color.White);
        }
    }
}
