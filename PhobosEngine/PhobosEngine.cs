using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using PhobosEngine.Graphics;

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
            // TODO: Add your initialization logic here


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
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            testScene.Update();
            int dir = gameTime.TotalGameTime.Seconds % 2 == 0 ? 1 : -1;
            testTransform.position += Vector2.UnitX * ((float)gameTime.ElapsedGameTime.TotalSeconds) * 40 * dir;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            testScene.Draw(_spriteBatch);

            base.Draw(gameTime);
        }
    }
}
