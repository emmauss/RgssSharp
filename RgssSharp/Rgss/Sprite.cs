using System;
using IronRuby.Runtime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaColor = Microsoft.Xna.Framework.Color;

namespace RgssSharp.Rgss
{
	[RubyClass("Sprite", Inherits = typeof(Object))]
	public class Sprite : IRenderable
	{
		private bool _isDisposed;

		public Sprite(Viewport viewport = null)
		{
			Viewport = viewport;
			if (Viewport != null)
				Viewport.AddRender(this);
			else
				Graphics.AddRender(this);
		}

		public Viewport Viewport { get; } 

		public int X { get; set; } = 0;

		public int Y { get; set; } = 0;

		public int Ox { get; set; } = 0;

		public int Oy { get; set; } = 0;

		public float ZoomX { get; set; } = 1.0f;

		public float ZoomY { get; set; } = 1.0f;

		public int BushDepth { get; set; } = 0;

		public Bitmap Bitmap { get; set; }

		public Rect SrcRect { get; set; }

		public int Opacity { get; set; } = 255;

		public int Angle { get; set; } = 0;

		public bool Mirror { get; set; } = false;

		public Color Color { get; set; } = Color.CLEAR;

		public Tone Tone { get; set; } = Tone.NONE;

		#region IRenderable Members

		public int CompareTo(IRenderable other)
		{
			return Z.CompareTo(other.Z);
		}

		public int Z { get; set; } = 0;

		public bool Visible { get; set; } = true;

		public void Draw()
		{
			if (!Visible || Opacity == 0 || Bitmap == null)
				return;
			var vector = new Vector2(X - Ox, Y - Oy);
			var scale = new Vector2(ZoomX, ZoomY); // TODO: ???????????
			var effect = Mirror ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			if (BushDepth > 0)
			{

			}
			else
			{
				var color = GetRenderColor();
				Graphics.SpriteBatch.Draw(Bitmap, vector, SrcRect, color, Angle, Vector2.Zero, scale, effect, Z);
			}
		}

		public void Update()
		{

		}

		public bool IsDisposed()
		{
			return _isDisposed;
		}

		public void Dispose()
		{
			if (_isDisposed)
				return;
			if (Viewport != null)
				Viewport.RemoveRender(this);
			else
				Graphics.RemoveRender(this);
			Bitmap.Dispose();
			_isDisposed = true;
		}

		#endregion



		private XnaColor GetRenderColor()
		{
			// TODO: Implement blending color, tone, opacity, and flash color
			return new XnaColor(255, 255, 255, Opacity);
		}
	}
}