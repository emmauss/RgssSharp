using System;
using System.Collections.Generic;
using IronRuby.Runtime;
using Microsoft.Xna.Framework.Graphics;

namespace RgssSharp.Rgss
{
	public class Viewport : RenderTarget2D, IRenderable
	{
		internal List<IRenderable> PendingRenders { get; } = new List<IRenderable>();


		private Color _flashColor = new Color();
		private int _flashDuration = 0;

		public Rect Rect { get; }

		public int Ox { get; set; }

		public int Oy { get; set; }

		public int Z { get; set; }

		public bool Visible { get; set; }

		public Color Color { get; set; }

		public Tone Tone { get; set; }


		public Viewport(int x, int y, int width, int height) : base(Graphics.Device, width, height)
		{
			Rect = new Rect(x, y, width, height);
			Visible = true;
		}

		public Viewport(Rect rect) : base(Graphics.Device, rect.Width, rect.Height)
		{
			Rect = rect;
			Visible = true;
		}

		[RubyMethod("disposed?")]
		public new bool IsDisposed()
		{
			return base.IsDisposed;
		}

		public void Draw()
		{
			foreach (var render in PendingRenders)
				render.Draw();
			PendingRenders.Clear();
		}

		public void Update()
		{
			if (_flashDuration <= 0) 
				return;

			// TODO: Implement flash
			_flashDuration--;
			PendingRenders.Sort();
			Graphics.PendingRenders.Add(this);
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