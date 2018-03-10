using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace LightingDemo
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class LightingDemo : Game
    {

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D background;
        Texture2D blackSquare;
        Texture2D ground;
        Texture2D torch;
        Texture2D lightmask;

        RenderTarget2D mainScene;
        RenderTarget2D lightMask;

        Effect lightingEffect;

        List<Vector2> torchPositions;

        Vector2 mousePosition = Vector2.Zero;

        const int LIGHTOFFSET = 115;

        public LightingDemo()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {

            torchPositions = new List<Vector2>
            {
                new Vector2(40, 420),
                new Vector2(340, 420),
                new Vector2(640, 420)
            };

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            background = Content.Load<Texture2D>("images\\bg5");
            blackSquare = Content.Load<Texture2D>("images\\blacksquare");
            ground = Content.Load<Texture2D>("images\\ground");
            torch = Content.Load<Texture2D>("images\\torch");
            lightmask = Content.Load<Texture2D>("images\\lightmask");

            lightingEffect = Content.Load<Effect>("effects\\lighting");

            var pp = GraphicsDevice.PresentationParameters;
            mainScene = new RenderTarget2D(GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight);
            lightMask = new RenderTarget2D(GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight);
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            var mouse = Mouse.GetState();
            mousePosition = new Vector2(mouse.X, mouse.Y);

            base.Update(gameTime);
        }

        private void DrawMain(GameTime gameTime)
        {

            GraphicsDevice.SetRenderTarget(mainScene);
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            // Draws a background image.
            spriteBatch.Draw(background, new Vector2(-300, -200), Color.White);

            // Lays out some torches based on torch positions and mouse
            foreach (var position in torchPositions)
            {
                spriteBatch.Draw(torch, position, Color.White);
            }
            spriteBatch.Draw(torch, mousePosition, Color.White);

            // Lay out some ground.
            for (var idx = 0; idx < 25; idx++)
            {
                spriteBatch.Draw(ground, new Vector2(32 * idx, 450), Color.White);
            }

            spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);
        }

        private void DrawLightMask(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(lightMask);
            GraphicsDevice.Clear(Color.Black);

            // Create a Black Background
            spriteBatch.Begin();
            spriteBatch.Draw(blackSquare, new Vector2(0, 0), new Rectangle(0, 0, 800, 800), Color.White);
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);

            // Draw out lightmasks based on torch positions.
            foreach (var position in torchPositions)
            {
                var new_pos = new Vector2(position.X - LIGHTOFFSET, position.Y - LIGHTOFFSET);
                spriteBatch.Draw(lightmask, new_pos, Color.White);
            }
            spriteBatch.Draw(lightmask, new Vector2(mousePosition.X - LIGHTOFFSET, mousePosition.Y - LIGHTOFFSET), Color.White);

            spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);
        }

        protected override void Draw(GameTime gameTime)
        {
            DrawMain(gameTime);
            DrawLightMask(gameTime);

            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            lightingEffect.Parameters["lightMask"].SetValue(lightMask);
            lightingEffect.CurrentTechnique.Passes[0].Apply();
            spriteBatch.Draw(mainScene, new Vector2(0, 0), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}