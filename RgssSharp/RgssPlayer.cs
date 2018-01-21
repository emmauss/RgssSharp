using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RgssSharp.Rgss;
using Color = RgssSharp.Rgss.Color;

namespace RgssSharp
{
	/// <summary>
	/// This is the main type for your game
	/// </summary>
	public class RgssPlayer : Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
		


		public RgssPlayer()
		{
			
			Ruby.Initialize();
			graphics = new GraphicsDeviceManager(this);
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{


			base.Initialize();
		}


		private Bitmap testBitmap;
		private Sprite sprite;

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch(GraphicsDevice);
			Graphics.Initialize(ref graphics, ref spriteBatch);
			Font.LoadFonts();
			testBitmap = new Bitmap(128, 32);


	
			testBitmap.FillRect(0, 0, 128, 32, new Color(32, 32, 64));
			testBitmap.DrawText(0, 0, 128, 32, "Optimus");


			// TODO: use this.Content to load your game content here
		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// all content.
		/// </summary>
		protected override void UnloadContent()
		{

		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			// Allows the game to exit
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
				Exit();


			//Rgss.Graphics.Update();
			Input.Update();

			// TODO: Add your update logic here

			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			
			GraphicsDevice.Clear(Microsoft.Xna.Framework.Color.SlateBlue);
			spriteBatch.Begin();
			spriteBatch.Draw(testBitmap, new Rectangle(0, 0, 128, 32), Microsoft.Xna.Framework.Color.White);
			spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
