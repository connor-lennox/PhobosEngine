using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PhobosEngine.Input;

namespace PhobosEngine
{
    public class PhobosGame : Game
    {
        // TODO: should this *really* be static?
        public static Vector2 GameResolution {get; private set;}

        protected GraphicsDeviceManager graphicsManager;
        private SpriteBatch spriteBatch;

        protected Scene activeScene = null;

        private int resolutionX;
        private int resolutionY;
        private RenderTarget2D renderTarget;

        // Default resolution of 640x480
        public PhobosGame() : this(640, 480) {}

        public PhobosGame(int resX, int resY)
        {
            resolutionX = resX;
            resolutionY = resY;

            GameResolution = new Vector2(resolutionX, resolutionY);

            graphicsManager = new GraphicsDeviceManager(this);
        }

        protected override void Initialize()
        {
            ResourceDatabase.Init(GraphicsDevice);
            spriteBatch = new SpriteBatch(GraphicsDevice);
            renderTarget = new RenderTarget2D(GraphicsDevice, resolutionX, resolutionY,
                                                false, GraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24);
            base.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            InputManager.Update();
            Time.DeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            activeScene?.Update();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            if(activeScene != null)
            {
                GraphicsDevice.SetRenderTarget(renderTarget);

                GraphicsDevice.Clear(activeScene.BackgroundColor);
                activeScene.Draw(spriteBatch);

                GraphicsDevice.SetRenderTarget(null);

                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
                spriteBatch.Draw(renderTarget, GraphicsDevice.Viewport.Bounds, Color.White);
                spriteBatch.End();
            }

            base.Draw(gameTime);
        }
    }
}