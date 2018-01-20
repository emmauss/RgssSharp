using IronRuby.Runtime;
using Microsoft.Xna.Framework;
using SD = System.Drawing;

namespace RgssSharp.Rgss
{
	public struct Rect
	{
		private Rectangle _rect;

		public int X
		{
			get => _rect.X;
			set => _rect.X = value;
		}

		public int Y
		{
			get => _rect.Y;
			set => _rect.Y = value;
		}

		public int Width
		{
			get => _rect.Width;
			set => _rect.Width = value;
		}

		public int Height
		{
			get => _rect.Height;
			set => _rect.Height = value;
		}

		public int Top { get => _rect.Top; }

		public int Bottom { get => _rect.Bottom; }

		public int Left { get => _rect.Left; }

		public int Right { get => _rect.Right; }

		public int CenterX{ get => _rect.Center.X; }

		public int CenterY{ get => _rect.Center.Y; }

		public Rect(int x, int y, int width, int height)
		{
			_rect = new Rectangle(x, y, width, height);
		}

		[RubyMethod("intersects?")]
		public bool Intersects(Rect other)
		{
			return _rect.Intersects(other._rect);
		}

		public static implicit operator Rectangle(Rect rect)
		{
			return rect._rect;
		}

		public static implicit operator SD.RectangleF(Rect rect)
		{
			return new SD.RectangleF(rect.X, rect.Y, rect.Width, rect.Height);
		}

		public static implicit operator SD.Rectangle(Rect rect)
		{
			return new SD.Rectangle(rect.X, rect.Y, rect.Width, rect.Height);
		}

		public static explicit operator Rect(Rectangle rectangle)
		{
			return new Rect(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
		}
	}
}
