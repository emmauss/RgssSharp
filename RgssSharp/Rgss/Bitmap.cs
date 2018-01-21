using System;
using System.IO;
using System.Linq;
using IronRuby.Runtime;
using Microsoft.Xna.Framework.Graphics;
using SD = System.Drawing;
using XnaColor = Microsoft.Xna.Framework.Color;
using XnaPoint = Microsoft.Xna.Framework.Point;
using XnaRect = Microsoft.Xna.Framework.Rectangle;


namespace RgssSharp.Rgss
{
	public class Bitmap : RenderTarget2D, IDisposable
	{
		#region TextAlign enum

		public enum TextAlign
		{
			Left = 0x00,
			Center = 0x01,
			Right = 0x02
		}

		#endregion

		private XnaRect _pixel = new XnaRect(0, 0, 1, 1);

		public Bitmap(int width, int height) : base(Graphics.Device, width, height)
		{
			if (width < 1)
				throw new ArgumentOutOfRangeException(nameof(width), width, "Bitmap width must be greater than 0.");
			if (height < 1)
				throw new ArgumentOutOfRangeException(nameof(height), height, "Bitmap height must be greater than 0.");
			Rect = new Rect(0, 0, width, height);
		}

		public Bitmap(string filename) : this(SD.Image.FromFile(filename))
		{
		}

		public Bitmap(SD.Image bitmap) : base(Graphics.Device, bitmap.Width, bitmap.Height)
		{
			Rect = new Rect(0, 0, bitmap.Width, bitmap.Height);
		}

		public Bitmap(Stream stream) : this(SD.Image.FromStream(stream))
		{
		}

		public Rect Rect { get; }

		public Font Font { get; set; } = new Font(Font.DefaultName);

		#region IDisposable Members

		public new void Dispose()
		{
			Font.Dispose();
			base.Dispose();
		}

		#endregion

		[RubyMethod("disposed?")]
		public new bool IsDisposed()
		{
			return base.IsDisposed;
		}

		public void Blt(int x, int y, Bitmap srcBitmap, Rect srcRect, int opacity = 255)
		{
			var data = new XnaColor[srcRect.Width * srcRect.Height];
			srcBitmap.GetData(0, srcRect, data, 0, data.Length);
			SetData(0, new XnaRect(x, y, srcRect.Width, srcRect.Height), data, 0, data.Length);
		}

		public void StretchBlt(Rect destRect, Bitmap srcBitmap, Rect srcRect, int opacity = 255)
		{
			Graphics.Device.SetRenderTarget(this);
			Graphics.SpriteBatch.Begin();
			Graphics.SpriteBatch.Draw(srcBitmap, destRect, srcRect, new XnaColor(255, 255, 255, opacity));
			Graphics.SpriteBatch.End();
			Graphics.Device.SetRenderTarget(null);
		}

		public void FillRect(int x, int y, int width, int height, Color color)
		{
			FillRect(new Rect(x, y, width, height), color);
		}

		public void FillRect(Rect rect, Color color)
		{
			var data = Enumerable.Repeat<XnaColor>(color, rect.Width * rect.Height).ToArray();
			SetData(0, rect, data, 0, data.Length);
		}

		public void Clear()
		{
			var color = new XnaColor(0, 0, 0, 0);
			var data = Enumerable.Repeat(color, Width * Height).ToArray();
			SetData(data);
		}

		public Color GetPixel(int x, int y)
		{
			_pixel.Location = new XnaPoint(x, y);
			var pixelColor = new XnaColor[1];
			GetData(0, _pixel, pixelColor, 0, 1);
			return (Color) pixelColor[0];
		}

		public void SetPixel(int x, int y, Color color)
		{
			_pixel.Location = new XnaPoint(x, y);
			var pixelColor = new XnaColor[] { color };
			SetData(0, _pixel, pixelColor, 0, 1);
		}

		public void HueChange(int hue)
		{
			// TODO: Implement
		}

		public void DrawText(int x, int y, int width, int height, string text, TextAlign align = TextAlign.Left)
		{
			DrawText(new Rect(x, y, width, height), text, align);
		}

		public void DrawText(Rect rect, string text, TextAlign align = TextAlign.Left)
		{
			using (var bmp = new PinnedBitmap(rect.Width, rect.Height))
			{
				using (var gfx = SD.Graphics.FromImage(bmp))
				{
					using (var renderer = new NativeTextRenderer(gfx))
					{
						var flags = TextFormatFlags.SingleLine;
						if (align == TextAlign.Center)
							flags |= TextFormatFlags.Center;
						else if (align == TextAlign.Right)
							flags |= TextFormatFlags.Right;
						renderer.DrawString(text, Font, Font.Color, rect, flags);
					}
				}
				SetData(bmp.Bits);
			}
		}

		public int TextSize(string text)
		{
			// TODO: Implement
			return 0;
		}
	}
}