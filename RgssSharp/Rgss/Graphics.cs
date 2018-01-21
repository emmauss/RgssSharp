using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RgssSharp.Rgss
{
	public static class Graphics
	{
		internal static List<IRenderable> PendingRenders { get; } = new List<IRenderable>();

		public static GraphicsDeviceManager DeviceManager { get; private set; }

		public static GraphicsDevice Device { get; private set; }

		public static SpriteBatch SpriteBatch { get; private set; }

		public static List<string> PrivateFonts { get; } = new List<string>();


		public static void Initialize(ref GraphicsDeviceManager manager, ref SpriteBatch spriteBatch)
		{
			DeviceManager = manager;
			DeviceManager.PreferredBackBufferFormat = SurfaceFormat.Color;
			DeviceManager.PreferredDepthStencilFormat = DepthFormat.None;
			Device = DeviceManager.GraphicsDevice;
			SpriteBatch = spriteBatch;
		}

		public static void Update()
		{
			PendingRenders.Sort();
			Draw();
		}

		private static void Draw()
		{
			SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
			foreach (var render in PendingRenders)
				render.Draw();
			SpriteBatch.End();
		}
	}
}
