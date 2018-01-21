using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;

namespace RgssSharp
{
	/// <summary>
	///     Wrapper for GDI text rendering functions<br />
	///     This class is not thread-safe as GDI function should be called from the UI thread.
	/// </summary>
	public sealed class RgssTextRenderer : IDisposable
	{
		/// <summary>
		///     Init.
		/// </summary>
		public RgssTextRenderer(Graphics g)
		{
			_g = g;

			var clip = _g.Clip.GetHrgn(_g);

			_hdc = _g.GetHdc();
			Gdi32.SetBkMode(_hdc, 1);

			Gdi32.SelectClipRgn(_hdc, clip);

			Gdi32.DeleteObject(clip);
		}

		#region IDisposable Members

		/// <summary>
		///     Release current HDC to be able to use <see cref="Graphics" /> methods.
		/// </summary>
		public void Dispose()
		{
			if (_hdc != IntPtr.Zero)
			{
				Gdi32.SelectClipRgn(_hdc, IntPtr.Zero);
				_g.ReleaseHdc(_hdc);
				_hdc = IntPtr.Zero;
			}
		}

		#endregion

		/// <summary>
		///     Measure the width and height of string <paramref name="str" /> when drawn on device context HDC
		///     using the given font <paramref name="font" />.
		/// </summary>
		/// <param name="str">the string to measure</param>
		/// <param name="font">the font to measure string with</param>
		/// <returns>the size of the string</returns>
		public Size MeasureString(string str, Font font)
		{
			SetFont(font);

			var size = new Size();
			Gdi32.GetTextExtentPoint32(_hdc, str, str.Length, ref size);
			return size;
		}

		/// <summary>
		///     Measure the width and height of string <paramref name="str" /> when drawn on device context HDC
		///     using the given font <paramref name="font" />.<br />
		///     Restrict the width of the string and get the number of characters able to fit in the restriction and
		///     the width those characters take.
		/// </summary>
		/// <param name="str">the string to measure</param>
		/// <param name="font">the font to measure string with</param>
		/// <param name="maxWidth">the max width to render the string in</param>
		/// <param name="charFit">the number of characters that will fit under <see cref="maxWidth" /> restriction</param>
		/// <param name="charFitWidth"></param>
		/// <returns>the size of the string</returns>
		public Size MeasureString(string str, Font font, float maxWidth, out int charFit, out int charFitWidth)
		{
			SetFont(font);

			var size = new Size();
			Gdi32.GetTextExtentExPoint(_hdc, str, str.Length, (int) Math.Round(maxWidth), _charFit, _charFitWidth, ref size);
			charFit = _charFit[0];
			charFitWidth = charFit > 0 ? _charFitWidth[charFit - 1] : 0;
			return size;
		}

		/// <summary>
		///     Draw the given string using the given font and foreground color at given location.
		/// </summary>
		/// <param name="str">the string to draw</param>
		/// <param name="font">the font to use to draw the string</param>
		/// <param name="color">the text color to set</param>
		/// <param name="point">the location to start string draw (top-left)</param>
		public void DrawString(String str, Font font, Color color, Point point)
		{
			SetFont(font);
			SetTextColor(color);

			Gdi32.TextOut(_hdc, point.X, point.Y, str, str.Length);
		}

		/// <summary>
		///     Draw the given string using the given font and foreground color at given location.<br />
		///     See http://msdn.microsoft.com/en-us/library/windows/desktop/dd162498(v=vs.85).aspx.
		/// </summary>
		/// <param name="str">the string to draw</param>
		/// <param name="font">the font to use to draw the string</param>
		/// <param name="color">the text color to set</param>
		/// <param name="rect">the rectangle in which the text is to be formatted</param>
		/// <param name="flags">The method of formatting the text</param>
		public void DrawString(String str, Font font, Color color, Rectangle rect, TextFormatFlags flags)
		{
			SetFont(font);
			SetTextColor(color);

			var rect2 = new Gdi32.Rect(rect);
			User32.DrawText(_hdc, str, str.Length, ref rect2, (uint) flags);
		}

		public void DrawTransparentText(string str, Font font, Color color, Rectangle rect)
		{
			var point = new Point(rect.X, rect.Y);
			var size = new Size(rect.Width, rect.Height);
			DrawTransparentText(str, font, color, point, size);
		}


		/// <summary>
		///     Special draw logic to draw transparent text using GDI.<br />
		///     1. Create in-memory DC<br />
		///     2. Copy background to in-memory DC<br />
		///     3. Draw the text to in-memory DC<br />
		///     4. Copy the in-memory DC to the proper location with alpha blend<br />
		/// </summary>
		public void DrawTransparentText(string str, Font font, Color color, Point point, Size size)
		{
			// Create a memory DC so we can work off-screen
			var memoryHdc = Gdi32.CreateCompatibleDC(_hdc);
			Gdi32.SetBkMode(memoryHdc, 1);

			// Create a device-independent bitmap and select it into our DC
			var info = new Gdi32.BitMapInfo();
			info.biSize = Marshal.SizeOf(info);
			info.biWidth = size.Width;
			info.biHeight = -size.Height;
			info.biPlanes = 1;
			info.biBitCount = 32;
			info.biCompression = 0; // BI_RGB
			// ReSharper disable once UnusedVariable
			var dib = Gdi32.CreateDIBSection(_hdc, ref info, 0, out var ppvBits, IntPtr.Zero, 0);
			Gdi32.SelectObject(memoryHdc, dib);

			try
			{
				// copy target background to memory HDC so when copied back it will have the proper background
				Gdi32.BitBlt(memoryHdc, 0, 0, size.Width, size.Height, _hdc, point.X, point.Y, 0x00CC0020);

				// Create and select font
				Gdi32.SelectObject(memoryHdc, GetCachedHFont(font));
				Gdi32.SetTextColor(memoryHdc, ((color.B & 0xFF) << 16) | ((color.G & 0xFF) << 8) | color.R);

				// Draw text to memory HDC
				Gdi32.TextOut(memoryHdc, 0, 0, str, str.Length);

				// copy from memory HDC to normal HDC with alpha blend so achieve the transparent text
				Gdi32.AlphaBlend(_hdc, point.X, point.Y, size.Width, size.Height, memoryHdc, 0, 0, size.Width, size.Height,
					new Gdi32.BlendFunction(color.A));
			}
			finally
			{
				Gdi32.DeleteObject(dib);
				Gdi32.DeleteDC(memoryHdc);
			}
		}

		/// <summary>
		///     Set a resource (e.g. a font) for the specified device context.
		/// </summary>
		private void SetFont(Font font)
		{
			Gdi32.SelectObject(_hdc, GetCachedHFont(font));
		}

		/// <summary>
		///     Get cached unmanaged font handle for given font.<br />
		/// </summary>
		/// <param name="font">the font to get unmanaged font handle for</param>
		/// <returns>handle to unmanaged font</returns>
		private static IntPtr GetCachedHFont(Font font)
		{
			var hfont = IntPtr.Zero;
			if (_fontsCache.TryGetValue(font.Name, out var dic1))
			{
				if (dic1.TryGetValue(font.Size, out var dic2))
					dic2.TryGetValue(font.Style, out hfont);
				else
					dic1[font.Size] = new Dictionary<FontStyle, IntPtr>();
			}
			else
			{
				_fontsCache[font.Name] = new Dictionary<float, Dictionary<FontStyle, IntPtr>>
				{
					[font.Size] = new Dictionary<FontStyle, IntPtr>() 

				};
			}

			if (hfont == IntPtr.Zero) _fontsCache[font.Name][font.Size][font.Style] = hfont = font.ToHfont();

			return hfont;
		}

		/// <summary>
		///     Set the text color of the device context.
		/// </summary>
		private void SetTextColor(Color color)
		{
			var rgb = ((color.B & 0xFF) << 16) | ((color.G & 0xFF) << 8) | color.R;
			Gdi32.SetTextColor(_hdc, rgb);
		}

		#region Fields and Consts

		/// <summary>
		///     used for <see cref="MeasureString(string,System.Drawing.Font,float,out int,out int)" /> calculation.
		/// </summary>
		private static readonly int[] _charFit = new int[1];

		/// <summary>
		///     used for <see cref="MeasureString(string,System.Drawing.Font,float,out int,out int)" /> calculation.
		/// </summary>
		private static readonly int[] _charFitWidth = new int[1000];

		/// <summary>
		///     cache of all the font used not to create same font again and again
		/// </summary>
		private static readonly Dictionary<string, Dictionary<float, Dictionary<FontStyle, IntPtr>>> _fontsCache =
			new Dictionary<string, Dictionary<float, Dictionary<FontStyle, IntPtr>>>(StringComparer.InvariantCultureIgnoreCase);

		/// <summary>
		///     The wrapped WinForms graphics object
		/// </summary>
		private readonly Graphics _g;

		/// <summary>
		///     the initialized HDC used
		/// </summary>
		private IntPtr _hdc;

		#endregion
	}
}