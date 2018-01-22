using System;
using System.Collections.Generic;
using IronRuby.Runtime;
using Microsoft.Xna.Framework.Graphics;

namespace RgssSharp.Rgss
{
	[RubyClass("Viewport", Inherits = typeof(Object))]
	public class Viewport : RenderTarget2D, IRenderable
	{
		private List<IRenderable> _pendingRenders;
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
			Graphics.AddRender(this);
		}

		public Viewport(Rect rect) : base(Graphics.Device, rect.Width, rect.Height)
		{
			Rect = rect;
			Graphics.AddRender(this);
		}

		public new void Dispose()
		{
			foreach (var render in _pendingRenders)
				render.Dispose();
			_pendingRenders.Clear();
			Graphics.RemoveRender(this);
			base.Dispose();
		}

		[RubyMethod("disposed?")]
		public new bool IsDisposed()
		{
			return base.IsDisposed;
		}

		public void Draw()
		{
			if (!Visible || Opacity <= 0) 
				return;
			Graphics.Device.SetRenderTarget(this);
			foreach (var render in _pendingRenders)
				render.Draw();
			if (_flashDuration > 0)
				Graphics.SpriteBatch.Draw(this, Rect, _flashColor);
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

		internal void AddRender(IRenderable sprite)
		{
			_pendingRenders.Add(sprite);
			_pendingRenders.Sort();
		}

		internal void RemoveRender(IRenderable sprite)
		{
			_pendingRenders.Remove(sprite);
		}
	}
}