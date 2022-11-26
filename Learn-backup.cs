using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.VectorDraw;
//using SharpDX.Direct3D11;

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
        private static Random _random;

        /// <summary>
        /// Координаты игрока.
        /// </summary>
        private Vector2 _playerCoords = new Vector2(30, 220);

        /// <summary>
        /// Скорость движения игрока.
        /// </summary>
        private float _playerSpeed = 400;
        
        /// <summary>
        /// Очки игрока.
        /// </summary>
        private static int _playerScore = 0;

        /// <summary>
        /// Направление движения игрока.
        /// 0 - вверх, 1 - вправо, 2 - вниз, 3 - влево.
        /// </summary>
        private int _playerDirection = 1;

        /// <summary>
        /// Размер игрока.
        /// </summary>
        private int _playerSize = 32;
        
        /// <summary>
        /// Координаты еды.
        /// </summary>
        private Vector2 _foodCoords;

        /// <summary>
        /// Размеры еды по вертикали и горизонтали.
        /// </summary>
        private static int _foodSize = 32;

        /// <summary>
        /// Квадратик размером 128х128.
        /// </summary>
        private Texture2D _sprite128;

        private Color[] _sprite128Color = new Color[128 * 128];

        private Texture2D _backBufferData;

        /// <summary>
        /// Коэффициент масштабирования по осям.
        /// </summary>
        private Vector2 _scaleFactor = new Vector2(1, 1);

        public Learn()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 600;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.AllowUserResizing = true;
            Window.Title = "Моя игра";

            Window.ClientSizeChanged += WindowOnClientSizeChanged;
        }

        /// <summary>
        /// Обработчик события изменения размера окна игры.
        /// </summary>
        private void WindowOnClientSizeChanged(object sender, EventArgs e)
        {
            _scaleFactor.X = Window.ClientBounds.Width / 800f;
            _scaleFactor.Y = Window.ClientBounds.Height / 600f;
            _graphics.GraphicsDevice.PresentationParameters.BackBufferWidth = Window.ClientBounds.Width;
            _graphics.GraphicsDevice.PresentationParameters.BackBufferHeight = Window.ClientBounds.Height;
            _graphics.ApplyChanges();
        }

        /// <summary>
        /// Инициализация объектов игры.
        /// </summary>
        protected override void Initialize()
        {
            _random = new Random((int)(DateTime.Now.Ticks & 0xFFFFFFFF));
            _foodCoords = new Vector2(_random.Next(0, 800 - _foodSize), _random.Next(0, 600 - _foodSize));

            base.Initialize();
        }

        /// <summary>
        /// Загрузка контента игры.
        /// </summary>
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(_graphics.GraphicsDevice);

            // TODO: use this.Content to load your game content here

            _backBufferData = new Texture2D(
                _graphics.GraphicsDevice,
                800,
                600,
                false,
                SurfaceFormat.Color);

            _sprite128 = Texture2D.FromFile(_graphics.GraphicsDevice, @"Content\square128.png");
            _sprite128.GetData<Color>(_sprite128Color);
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

            MovePlayer(gameTime);

            //обнаружение столкновения с едой
            if (_playerCoords.X + _playerSize > _foodCoords.X && _foodCoords.X + _foodSize > _playerCoords.X && 
                _playerCoords.Y + _playerSize > _foodCoords.Y && _foodCoords.Y + _foodSize > _playerCoords.Y)
            {
                //разместить новую еду
                _foodCoords.X = _random.Next(0, 800 - _foodSize);
                _foodCoords.Y = _random.Next(0, 600 - _foodSize);
       
                //посчитать очки и повысить сложность игры
                _playerScore += 1;
                _playerSpeed += 10;
            }
            
            //обнаружение выхода за пределы экрана (проигрыш)
            if (_playerCoords.X < 0 || _playerCoords.X > 800 - _playerSize || _playerCoords.Y < 0 || _playerCoords.Y > 600 - _playerSize)
            {
                //TODO: код завершения игры
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

            _backBufferData.SetData(0, new Rectangle(0, 0, 128, 128), _sprite128Color, 0, _sprite128Color.Length);
            _backBufferData.SetData(0, new Rectangle(0, 600 - 128, 128, 128), _sprite128Color, 0, _sprite128Color.Length);
            _backBufferData.SetData(0, new Rectangle(800 - 128, 0, 128, 128), _sprite128Color, 0, _sprite128Color.Length);
            _backBufferData.SetData(0, new Rectangle(800 - 128, 600 - 128, 128, 128), _sprite128Color, 0, _sprite128Color.Length);

            _spriteBatch.Begin();

            _spriteBatch.Draw(_backBufferData, new Rectangle(0, 0, (int)(800 * _scaleFactor.X), (int)(600 * _scaleFactor.Y)), Color.White);

            //_spriteBatch.Draw(_sprite128, new Rectangle((int)(0 * _scaleFactor.X), (int)(0 * _scaleFactor.Y), (int)(128 * _scaleFactor.X), (int)(128 * _scaleFactor.Y)), Color.White);
            //_spriteBatch.Draw(_sprite128, new Rectangle((int)(0 * _scaleFactor.X), (int)((600 - 128) * _scaleFactor.Y), (int)(128 * _scaleFactor.X), (int)(128 * _scaleFactor.Y)), Color.White);
            //_spriteBatch.Draw(_sprite128, new Rectangle((int)((800 - 128) * _scaleFactor.X), (int)(0 * _scaleFactor.Y), (int)(128 * _scaleFactor.X), (int)(128 * _scaleFactor.Y)), Color.White);
            //_spriteBatch.Draw(_sprite128, new Rectangle((int)((800 - 128) * _scaleFactor.X), (int)((600 - 128) * _scaleFactor.Y), (int)(128 * _scaleFactor.X), (int)(128 * _scaleFactor.Y)), Color.White);

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
        /// Отрисовка игрока.
        /// </summary>
        private void DrawPlayer()
        {
        }

        private void DrawFood()
        {
        }
    }
}
