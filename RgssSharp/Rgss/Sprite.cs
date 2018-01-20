using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaColor = Microsoft.Xna.Framework.Color;

namespace RgssSharp.Rgss
{
	public class Sprite : IRenderable
	{
		public Sprite(Viewport viewport = null)
		{
			Viewport = viewport;
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

		public bool Invaladited { get; set; }

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
			var vector = new Vector2(X - Ox, Y - Oy);
			var scale = new Vector2(ZoomX, ZoomY); // TODO: ???????????
			var effect = Mirror ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			if (BushDepth > 0)
			{
			}
			else
			{
				var color = GetRenderColor();
				Graphics.SpriteBatch.Draw(Bitmap, vector, SrcRect, color, Angle, Vector2.Zero, scale, effect, 0);
			}
		}

		public void Update()
		{
			if (Viewport != null)
				Viewport.PendingRenders.Add(this);
			else
				Graphics.PendingRenders.Add(this);
		}

		public bool IsDisposed()
		{
			throw new NotImplementedException();
		}

		void IDisposable.Dispose()
		{
			throw new NotImplementedException();
		}

		#endregion

		public void Dispose()
		{
			Bitmap.Dispose();
		}

		private XnaColor GetRenderColor()
		{
			return new XnaColor(255, 255, 255, 255);
		}
	}
}