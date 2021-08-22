using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using FontStashSharp;

using PhobosEngine;
using PhobosEngine.Input;

namespace PhobosExample
{
    public class PhobosExample : PhobosGame
    {
        private Scene testScene;
        private Transform testTransform;

        private Camera testCamera;

        private Transform[] debugCorners;
        private BoxCollider boxToDebug;

        private FontSystem testFont;

        public PhobosExample() : base(320, 240)
        {
        }

        protected override void Initialize()
        {
            base.Initialize();
            IsMouseVisible = true;
            Window.AllowUserResizing = true;
        }

        protected override void LoadContent()
        {
            Texture2D testPixelTexture = new Texture2D(GraphicsDevice, 20, 20);
            Texture2D smallPixelTexture = new Texture2D(GraphicsDevice, 4, 4);
            Color[] data = new Color[400];
            for(int i = 0; i < 400; i++) {
                data[i] = Color.White;
            }

            Texture2D alienTexture = ResourceDatabase.LoadTexture("alien1.png");
            Texture2D alien2Texture = ResourceDatabase.LoadTexture("alien2.png");

            testPixelTexture.SetData(data);

            Color[] data2 = new Color[16];
            for(int i = 0; i < 16; i++)
            {
                data2[i] = Color.White;
            }
            smallPixelTexture.SetData(data2);

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
            testEntity2.Transform.Position = new Vector2(-40, 0);
            testEntity2.AddComponent(new SpriteRenderer()).sprite = alienTexture;
            testEntity2.GetComponent<SpriteRenderer>().tintColor = Color.White;
            BoxCollider c2 = testEntity2.AddComponent<BoxCollider>();
            c2.Size = new Vector2(20, 20);

            GameEntity testEntity3 = new GameEntity();
            testEntity3.Transform.Position = new Vector2(30, -10);
            testEntity3.AddComponent(new SpriteRenderer()).sprite = alien2Texture;
            testEntity3.GetComponent<SpriteRenderer>().tintColor = Color.White;
            BoxCollider c3 = testEntity3.AddComponent<BoxCollider>();
            c3.Size = new Vector2(20, 20);

            c1.Register();
            c2.Register();
            c3.Register();

            GameEntity camEntity = new GameEntity();
            testCamera = camEntity.AddComponent(new Camera());
            testScene.AddEntity(testEntity3);
            testScene.AddEntity(testEntity2);
            testScene.AddEntity(testEntity1);
            testScene.AddEntity(camEntity);
            testScene.MainCamera = camEntity.GetComponent<Camera>();

            GameEntity test2 = new GameEntity();
            test2.Transform.Parent = testTransform;
            test2.AddComponent<SpriteRenderer>().sprite = testPixelTexture;
            test2.GetComponent<SpriteRenderer>().tintColor = Color.Olive;
            test2.Transform.LocalPosition = new Vector2(50, 0);
            testScene.AddEntity(test2);
            boxToDebug = test2.AddComponent<BoxCollider>();
            boxToDebug.Size = new Vector2(20, 20);
            CollisionTracker tracker2 = test2.AddComponent<CollisionTracker>();
            tracker2.OnCollisionEnter += ((CollisionResult res) => test2.GetComponent<SpriteRenderer>().tintColor = Color.DarkViolet);
            tracker2.OnCollisionExit += ((CollisionResult res) => test2.GetComponent<SpriteRenderer>().tintColor = Color.Olive);
            boxToDebug.Register();

            debugCorners = new Transform[4];
            for(int i = 0; i < 4; i++)
            {
                debugCorners[i] = new GameEntity().Transform;
                debugCorners[i].AddComponent<SpriteRenderer>().sprite = smallPixelTexture;
                debugCorners[i].GetComponent<SpriteRenderer>().tintColor = Color.Lime;
                testScene.AddEntity(debugCorners[i].Entity);
            }


            testFont = ResourceDatabase.LoadFontSystem("pixellocale-v-1-4.ttf");
            GameEntity textTestEntity = new GameEntity();
            TextRenderer tr = textTestEntity.AddComponent<TextRenderer>();
            tr.FontSystem = testFont;
            tr.FontSize = 40;
            tr.Text = "Test string.";
            tr.TextColor = Color.DarkRed;
            tr.Transform.Position = new Vector2(-40, 70);

            testScene.AddEntity(textTestEntity);

            activeScene = testScene;
        }

        protected override void Update(GameTime gameTime)
        {
            InputManager.Update();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            testScene.Update();
            float x = MathF.Cos((float)gameTime.TotalGameTime.TotalSeconds);
            float y = MathF.Sin((float)gameTime.TotalGameTime.TotalSeconds);
            testTransform.Position = new Vector2(x, y) * 40;
            // testTransform.Position = new Vector2(100, 100);
            // testTransform.PointTowards(Vector2.Zero);
            testTransform.Rotation = x;
            // testTransform.Scale = new Vector2(MathF.Abs(x)+.25f, MathF.Abs(y)+.25f);

            for(int i = 0; i < 4; i++)
            {
                debugCorners[i].Position = boxToDebug.EffectivePoints[i];
            }

            if(InputManager.GetKey(Keys.Up)) {
                testCamera.Transform.Position += new Vector2(0, -0.1f);
            }

            if(InputManager.GetKey(Keys.Down)) {
                testCamera.Transform.Position += new Vector2(0, 0.1f);
            }

            if(InputManager.GetKey(Keys.Left)) {
                testCamera.Transform.Position += new Vector2(-0.1f, 0);
            }

            if(InputManager.GetKey(Keys.Right)) {
                testCamera.Transform.Position += new Vector2(0.1f, 0);
            }

            if(InputManager.GetKey(Keys.Q)) {
                testCamera.Zoom -= .01f;
            }

            if(InputManager.GetKey(Keys.E)) {
                testCamera.Zoom += .01f;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
