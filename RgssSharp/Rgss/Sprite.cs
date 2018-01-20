using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaColor = Microsoft.Xna.Framework.Color;

namespace RgssSharp.Rgss
{
	public class Sprite : IRenderable
	{
		#region IRenderable Members

		public int CompareTo(IRenderable other)
		{
			return Z.CompareTo(other.Z);
		}

		public int Z { get; set; }

		public bool Visible { get; set; }

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

		public void Dispose()
		{
			Bitmap.Dispose();
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


		public Viewport Viewport { get; }


		public int X { get; set; }

		public int Y { get; set; }

		public int Ox { get; set; }

		public int Oy { get; set; }

		public float ZoomX { get; set; }

		public float ZoomY { get; set; }

		public int BushDepth { get; set; }

		public Bitmap Bitmap { get; set; }

		public Rect SrcRect { get; set; }

		public int Angle { get; set; }

		public bool Mirror { get; set; }

		public Sprite(Viewport viewport = null)
		{
			Viewport = viewport;
		}

		private XnaColor GetRenderColor()
		{
			return new XnaColor(255, 255, 255, 255);
		}
	}
}