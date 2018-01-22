using IronRuby.Runtime;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Windows.Forms;
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

		public Bitmap(string filename) : this(File.OpenRead(filename))
		{
			
		}

		public Bitmap(Stream stream) : this(FromStream(Graphics.Device, stream))
		{
			stream.Dispose();
		}

		public Bitmap(Texture2D other) : base(Graphics.Device, other.Width, other.Height)
		{
			var pixels = new XnaColor[other.Width * other.Height];
			other.GetData(pixels);
			SetData(pixels);
			Rect = new Rect(0, 0, other.Width, other.Height);
		}

		public Bitmap(SD.Bitmap bitmap) : base(Graphics.Device, bitmap.Width, bitmap.Height)
		{
			Rect = new Rect(0, 0, bitmap.Width, bitmap.Height);
			var data = bitmap.LockBits(Rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
			var pixels = new uint[data.Width * data.Height];
			unsafe
			{
				unchecked
				{
					// Bitmap data is stored in different order, need to swap red and blue bytes
					var srcPtr = (uint*)data.Scan0;
					for (var i = 0; i < pixels.Length; i++)
					{
						// ReSharper disable once PossibleNullReferenceException
						pixels[i] = (srcPtr[i] & 0x000000FF) << 16 | (srcPtr[i] & 0x0000FF00) |
							        (srcPtr[i] & 0x00FF0000) >> 16 | (srcPtr[i] & 0xFF000000);
					}
				}
			}
			bitmap.UnlockBits(data);
			SetData(pixels);
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
			var bytes = new byte[Width * Height * 4];
			GetData(bytes, 0, bytes.Length);
			RotateHue(ref bytes, hue);
			SetData(bytes, 0, bytes.Length);
		}

		public void DrawText(int x, int y, int width, int height, string text, TextAlign align = TextAlign.Left, bool wrap = false)
		{
			var rect = new Rect(x, y, Math.Min(Width - x, width), Math.Min(Height - y, height));
			DrawText(rect, text, align, wrap);
		}

		public void DrawText(Rect rect, string text, TextAlign align = TextAlign.Left, bool wrap = false)
		{
			// Correct out of range values
			rect.Width = Math.Min(Width - rect.X, rect.Width);
			rect.Height = Math.Min(Height - rect.Y, rect.Height);
			// Create new bitmap with its allotted memory pinned
			using (var bmp = new PinnedBitmap(rect.Width, rect.Height))
			{
				var flags = GetTextFormatFlags(align, wrap);
				// Copy source pixels over to memory section
				GetData(0, rect, bmp.Bits, 0, bmp.Bits.Length);
				using (var gfx = SD.Graphics.FromImage(bmp))
				{
					gfx.TextRenderingHint = TextRenderingHint.AntiAlias;
					TextRenderer.DrawText(gfx, text, Font, rect, Font.Color, flags);
				}
				// Set the altered bits back to the source
				SetData(0, rect, bmp.Bits, 0, bmp.Bits.Length);
			}
		}

		private TextFormatFlags GetTextFormatFlags(TextAlign align, bool wrapping)
		{
			var flags = TextFormatFlags.NoPrefix | (wrapping ? TextFormatFlags.WordBreak : TextFormatFlags.SingleLine);
			switch (align)
			{
				case TextAlign.Left:
					flags |= TextFormatFlags.Left;
					break;
				case TextAlign.Center:
					flags |= TextFormatFlags.HorizontalCenter;
					break;
				case TextAlign.Right:
					flags |= TextFormatFlags.Right;
					break;
			}
			return flags;
		}

		public int TextSize(string text)
		{
			return TextRenderer.MeasureText(text, Font).Width;
		}

		public static void RotateHue(ref byte[] bytes, int degrees)
		{
			unchecked
			{
				var radians = Math.PI * degrees / 180.0;
				var cos = Math.Cos(radians);
				var sin = Math.Sin(radians);

				var a00 = 0.213 + cos * 0.787 - sin * 0.213;
				var a01 = 0.213 - cos * 0.213 + sin * 0.143;
				var a02 = 0.213 - cos * 0.213 - sin * 0.787;
				var a10 = 0.715 - cos * 0.715 - sin * 0.715;
				var a11 = 0.715 + cos * 0.285 + sin * 0.140;
				var a12 = 0.715 - cos * 0.715 + sin * 0.715;
				var a20 = 0.072 - cos * 0.072 + sin * 0.928;
				var a21 = 0.072 - cos * 0.072 - sin * 0.283;
				var a22 = 0.072 + cos * 0.928 + sin * 0.072;

				for (var i = 0; i < bytes.Length; i += 4)
				{
					double r = bytes[i];
					double g = bytes[i + 1];
					double b = bytes[i + 2];

					var rr = Math.Max(0.0, Math.Min(255.0, r * a00 + g * a10 + b * a20));
					var gr = Math.Max(0.0, Math.Min(255.0, r * a01 + g * a11 + b * a21));
					var br = Math.Max(0.0, Math.Min(255.0, r * a02 + g * a12 + b * a22));

					bytes[i] = (byte)rr;
					bytes[i + 1] = (byte)gr;
					bytes[i + 2] = (byte)br;
				}
			}
		}
	}
}