using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IronRuby.Builtins;
using IronRuby.Runtime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RgssSharp.Rgss
{
	[RubyModule("Graphics", DefineIn = typeof(RubyModule))]
	public static class Graphics
	{
		private static Bitmap _screenSample;
		private static Bitmap _transitionBitmap;
		private static int _transitionVague;
		private static bool _isTransitioning;
		private static int _transitionCount;
		private static bool _isFrozen;
		private static List<IRenderable> _renderables;
		private static int _frameRate;

		internal static GraphicsDeviceManager DeviceManager { get; private set; }

		internal static GraphicsDevice Device { get; private set; }

		internal static SpriteBatch SpriteBatch { get; private set; }

		public static int FrameRate
		{
			get { return _frameRate; }
			set { _frameRate = value.Clamp(10, 120); }
		}

		public static long FrameCount { get; set; } = 0;

		public static void Initialize(ref GraphicsDeviceManager manager, ref SpriteBatch spriteBatch)
		{
			DeviceManager = manager;
			DeviceManager.PreferredBackBufferFormat = SurfaceFormat.Color;
			DeviceManager.PreferredDepthStencilFormat = DepthFormat.None;
			Device = DeviceManager.GraphicsDevice;
			SpriteBatch = spriteBatch;
			_renderables = new List<IRenderable>();
			_transitionCount = 0;
			_isFrozen = false;
			_transitionBitmap = null;
			_isTransitioning = false;
		}

		[RubyMethod("update")]
		public static void Update()
		{
			
		}

		[RubyMethod("freeze")]
		public static void Freeze()
		{


		}

		[RubyMethod("transition")]
		public static void Transition(int duration, string filename = null, int vague = 40)
		{
			if (!_isFrozen)
				return;
			_transitionCount = duration;
			// TODO: Have Bitmap loaded through cache
			_transitionVague = vague;
			_isTransitioning = true;
			_isFrozen = false;
		}

		[RubyMethod("frame_reset")]
		public static void FrameReset()
		{

		}

		private static void Draw(GameTime time)
		{
			if (_isFrozen)
				return;
			if (_isTransitioning)
			{
				// TODO: Implement transition logic
				_transitionCount--;
				if (_transitionCount <= 0)
					_isTransitioning = false;
			}
			SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
			foreach (var render in _renderables)
				render.Draw();
			SpriteBatch.End();
		}

		internal static void AddRender(IRenderable sprite)
		{
			_renderables.Add(sprite);
			_renderables.Sort();
		}

		internal static void RemoveRender(IRenderable sprite)
		{
			_renderables.Remove(sprite);
		}
	}
}
