using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.VectorDraw;

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
        /// Задание для отрисовки примитивов.
        /// </summary>
        private PrimitiveBatch _primitiveBatch;
        
        /// <summary>
        /// Рисовальщик примитивов.
        /// </summary>
        private PrimitiveDrawing _primitiveDrawing;
        
        private Matrix _localProjection;
        private Matrix _localView;


        /// <summary>
        /// Текстура квардратика 128x128 пикселей.
        /// </summary>
        private Texture2D _square;

        public Learn()
        {
            _graphics = new GraphicsDeviceManager(this);
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
            _graphics.GraphicsDevice.PresentationParameters.BackBufferWidth = Window.ClientBounds.Width;
            _graphics.GraphicsDevice.PresentationParameters.BackBufferHeight = Window.ClientBounds.Height;
            _graphics.ApplyChanges();
        }

        /// <summary>
        /// Инициализация объектов игры.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// Загрузка контента игры.
        /// </summary>
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            //инициализация для рисования примитивов
            _primitiveBatch = new PrimitiveBatch(GraphicsDevice);
            _primitiveDrawing = new PrimitiveDrawing(_primitiveBatch);
            _localProjection = Matrix.CreateOrthographicOffCenter(0f, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, 0f, 0f, 1f);
            _localView = Matrix.Identity;

            _square = Texture2D.FromFile(_graphics.GraphicsDevice, @"Content\square128.png");

            // TODO: use this.Content to load your game content here
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

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// Перерисовка экрана.
        /// </summary>
        /// <param name="gameTime">Игровое время.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _primitiveBatch.Begin(ref _localProjection, ref _localView);

            _primitiveDrawing.DrawSegment(new Vector2(10,10), new Vector2(20, 100), Color.Red);

            _primitiveBatch.End();


            //_spriteBatch.Begin();
            //for (int j = 0, y = 0; y < _graphics.GraphicsDevice.PresentationParameters.BackBufferHeight; j++, y+=128)
            //{
            //    for (int i = 0, x = 0; x < _graphics.GraphicsDevice.PresentationParameters.BackBufferWidth; i++, x+=128)
            //    {
            //        _spriteBatch.Draw(_square, new Vector2(x, y), (i+j)%2 > 0 ? Color.DarkGreen : Color.White );
            //    }
            //}

            //_spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
