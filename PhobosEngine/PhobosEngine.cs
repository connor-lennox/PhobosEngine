using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PhobosEngine
{
    public class PhobosEngine : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Scene testScene;
        private Transform testTransform;

        public PhobosEngine()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            Texture2D testPixelTexture = new Texture2D(GraphicsDevice, 20, 20);
            Color[] data = new Color[400];
            for(int i = 0; i < 400; i++) {
                data[i] = Color.Black;
            }
            testPixelTexture.SetData(data);

            testScene = new Scene();
            GameEntity testEntity = new GameEntity();
            testEntity.AddComponent(new SpriteRenderer());
            testEntity.GetComponent<SpriteRenderer>().sprite = testPixelTexture;
            testTransform = testEntity.Transform;
            GameEntity camEntity = new GameEntity();
            camEntity.AddComponent(new Camera());
            camEntity.GetComponent<Camera>().Bounds = GraphicsDevice.Viewport.Bounds;
            testScene.AddEntity(testEntity);
            testScene.AddEntity(camEntity);
            testScene.MainCamera = camEntity.GetComponent<Camera>();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            testScene.Update();
            float x = MathF.Cos((float)gameTime.TotalGameTime.TotalSeconds);
            float y = MathF.Sin((float)gameTime.TotalGameTime.TotalSeconds);
            testTransform.Position = new Vector2(x, y) * 100;
            testTransform.PointTowards(Vector2.Zero);
            testTransform.Scale = new Vector2(MathF.Abs(x)+.25f, MathF.Abs(y)+.25f);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            testScene.Draw(_spriteBatch);

            base.Draw(gameTime);
        }
    }
}
