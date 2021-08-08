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

        private Transform[] debugCorners;

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
                data[i] = Color.White;
            }
            testPixelTexture.SetData(data);

            testScene = new Scene();

            GameEntity testEntity1 = new GameEntity();
            testEntity1.AddComponent(new SpriteRenderer());
            testEntity1.GetComponent<SpriteRenderer>().sprite = testPixelTexture;
            testTransform = testEntity1.Transform;
            BoxCollider c1 = testEntity1.AddComponent<BoxCollider>();
            c1.Size = new Vector2(20, 20);

            CollisionTracker tracker = testEntity1.AddComponent<CollisionTracker>();
            tracker.OnCollisionEnter += ((CollisionResult result) => testEntity1.GetComponent<SpriteRenderer>().tintColor = Color.Red);
            tracker.OnCollisionExit += ((CollisionResult result) => testEntity1.GetComponent<SpriteRenderer>().tintColor = Color.White);

            GameEntity testEntity2 = new GameEntity();
            testEntity2.Transform.Position = new Vector2(0, 0);
            testEntity2.AddComponent(new SpriteRenderer()).sprite = testPixelTexture;
            testEntity2.GetComponent<SpriteRenderer>().tintColor = Color.Black;
            BoxCollider c2 = testEntity2.AddComponent<BoxCollider>();
            c2.Size = new Vector2(20, 20);

            c1.Register();
            c2.Register();

            GameEntity camEntity = new GameEntity();
            camEntity.AddComponent(new Camera());
            camEntity.GetComponent<Camera>().Bounds = GraphicsDevice.Viewport.Bounds;
            testScene.AddEntity(testEntity2);
            testScene.AddEntity(testEntity1);
            testScene.AddEntity(camEntity);
            testScene.MainCamera = camEntity.GetComponent<Camera>();

            debugCorners = new Transform[4];
            for(int i = 0; i < 4; i++)
            {
                debugCorners[i] = new GameEntity().Transform;
                debugCorners[i].AddComponent<SpriteRenderer>().sprite = testPixelTexture;
                debugCorners[i].GetComponent<SpriteRenderer>().tintColor = Color.Lime;
                debugCorners[i].Scale = new Vector2(0.2f, 0.2f);
                testScene.AddEntity(debugCorners[i].Entity);
            }

            // GameEntity test2 = new GameEntity();
            // test2.Transform.Parent = testTransform;
            // test2.AddComponent<SpriteRenderer>().sprite = testPixelTexture;
            // test2.GetComponent<SpriteRenderer>().tintColor = Color.Olive;
            // test2.Transform.LocalPosition = new Vector2(50, 0);
            // testScene.AddEntity(test2);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            testScene.Update();
            float x = MathF.Cos((float)gameTime.TotalGameTime.TotalSeconds / 4);
            float y = MathF.Sin((float)gameTime.TotalGameTime.TotalSeconds / 4);
            testTransform.Position = new Vector2(0, y) * 100;
            // testTransform.Position = new Vector2(100, 100);
            // testTransform.PointTowards(Vector2.Zero);
            // testTransform.Rotation = x;
            // testTransform.Scale = new Vector2(MathF.Abs(x)+.25f, MathF.Abs(y)+.25f);

            BoxCollider col = testTransform.GetComponent<BoxCollider>();
            for(int i = 0; i < 4; i++)
            {
                debugCorners[i].Position = col.EffectivePoints[i];
            }

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
