using System;
using System.Collections.Generic;
using IronRuby.Runtime;
using Microsoft.Xna.Framework.Graphics;

namespace RgssSharp.Rgss
{
	public class Viewport : RenderTarget2D, IRenderable
	{
		internal List<IRenderable> PendingRenders { get; } = new List<IRenderable>();

		private Color _flashColor;
		private int _flashDuration;

		public bool Invaladited { get; set; } = true;

		public Rect Rect { get; } 

		public int Ox { get; set; } = 0;

		public int Oy { get; set; } = 0;

		public int Z { get; set; } = 0;

		public bool Visible { get; set; } = true;

		public Color Color { get; set; } = Color.CLEAR;

		public Tone Tone { get; set; } = Tone.NONE;

		public int Opacity { get; set; } = 255;

		public Viewport(int x, int y, int width, int height) : base(Graphics.Device, width, height)
		{
			Rect = new Rect(x, y, width, height);
			Graphics.PendingRenders.Add(this);
		}

		public Viewport(Rect rect) : base(Graphics.Device, rect.Width, rect.Height)
		{
			Rect = rect;
			Graphics.PendingRenders.Add(this);
		}

		public new void Dispose()
		{
			foreach (var render in PendingRenders)
				render.Dispose();
			PendingRenders.Clear();
			base.Dispose();
		}

		[RubyMethod("disposed?")]
		public new bool IsDisposed()
		{
			return base.IsDisposed;
		}

		public void Draw()
		{
			// Ideally some of this would have been placed in Update(), but RMXP specifies that
			// Viewports only need updated if they are flashing, so have to place elsewhere.
			if (Visible && Opacity > 0)
			{
				PendingRenders.RemoveAll(render => render.IsDisposed());
				PendingRenders.Sort();
				Graphics.Device.SetRenderTarget(this);
				foreach (var render in PendingRenders)
				{
					if (!render.Invaladited)
						continue;
					render.Draw();
				}
				if (_flashDuration > 0)
					Graphics.SpriteBatch.Draw(this, Rect, _flashColor);
			}
		}

		public void Update()
		{
			if (_flashDuration > 0)
				_flashDuration--;
		}

		public void Flash(Color color, int duration)
		{
			_flashColor = color;
			_flashDuration = duration;
		}

		public int CompareTo(IRenderable other)
		{
			return Z.CompareTo(other.Z);
		}
	}
}