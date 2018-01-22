using System.Diagnostics;
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
		public bool Debug { get; set; }


		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
		


		public RgssPlayer()
		{
			
			Ruby.Initialize();
			Audio.Initialize();
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


		private Bitmap testBitmap, testBitmap2;
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

			testBitmap = new Bitmap("012-Lancer04.png");

			testBitmap2 = new Bitmap(500, 456);


			testBitmap2.StretchBlt(testBitmap2.Rect, testBitmap, testBitmap.Rect);


			/*
			var timer = new Stopwatch();
			var counter = 5000;
			timer.Start();
			do
			{

			} while (counter > 0);
			timer.Stop();
			var ms = timer.ElapsedMilliseconds / 5000f;
			*/


			//testBitmap.HueChange(60);
			//testBitmap.Font = new Rgss.Font("OptimusPrinceps", 18);

			//testBitmap.FillRect(8, 8, 96, 16, new Color(255, 32, 64, 240));
			//testBitmap.DrawText(0, 0, 128, 32, "New Game");


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
			spriteBatch.Draw(testBitmap2, testBitmap2.Rect, Microsoft.Xna.Framework.Color.White);
			spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
