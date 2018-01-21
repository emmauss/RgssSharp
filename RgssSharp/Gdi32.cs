using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace RgssSharp
{			
	internal static class Gdi32
	{
		public const string LIBRARY = "gdi32.dll";

		/// <summary>
		/// <para>Specifies that only the process that called the <see cref="AddFontResourceEx"/> function can use this font.</para>
		/// <para>When the font name matches a public font, the private font will be chosen. When the process terminates, 
		/// the system will remove all fonts installed by the process with the <b>AddFontResourceEx</b> function.</para>
		/// </summary>
		/// <seealso cref="AddFontResourceEx"/>
		public const uint FR_PRIVATE = 0x10;

		/// <summary>
		/// Specifies that no process, including the process that called the <see cref="AddFontResourceEx"/> function, can enumerate this font.
		/// </summary>
		/// <seealso cref="AddFontResourceEx"/>
		public const uint FR_NOT_ENUM = 0x20;

		/// <summary>
		/// <para>The <b>AddFontResourceEx</b> function adds the font resource from the specified file to the system.</para> 
		/// <para>Fonts added with the <b>AddFontResourceEx</b> function can be marked as private and not enumerable.</para>
		/// </summary>
		/// <param name="lpszFilename">A pointer to a null-terminated character string that contains a valid font file name.</param>
		/// <param name="fl">
		/// The characteristics of the font to be added to the system.
		/// See <see cref="FR_PRIVATE"/> and <see cref="FR_NOT_ENUM"/>.
		/// </param>
		/// <param name="pdv">Reserved. Must be zero.</param>
		/// <returns>
		/// <para>If the function succeeds, the return value specifies the number of fonts added.</para>
		/// <para>If the function fails, the return value is zero. No extended error information is available.</para>
		/// </returns>
		/// <remarks>
		/// <para>This function allows a process to use fonts without allowing other processes access to the fonts.</para>
		/// <para>When an application no longer needs a font resource it loaded by calling the <b>AddFontResourceEx</b> function, it must remove the resource by calling the <see cref="RemoveFontResourceEx"/> function.</para>
		/// <para>This function installs the font only for the current session. When the system restarts, the font will not be present. To have the font installed even after restarting the system, the font must be listed in the registry.</para>
		/// </remarks>
		/// <seealso cref="RemoveFontResourceEx(string, uint, IntPtr)"/>
		/// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/dd183327(v=vs.85).aspx">AddFontResourceEx (MSDN)</seealso>
		/// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/dd162923(v=vs.85).aspx">RemoveFontResourceEx (MSDN)</seealso>
		[DllImport(LIBRARY)]
		public static extern int AddFontResourceEx(string lpszFilename, uint fl, IntPtr pdv);

		/// <summary>
		/// Removes the fonts in the specified file from the system font table.
		/// </summary>
		/// <param name="lpszFilename">A pointer to a null-terminated string that names a font resource file.</param>
		/// <param name="fl">
		/// The characteristics of the font to be removed from the system. 
		/// In order for the font to be removed, the flags used must be the same as when the font was added with the <see cref="AddFontResourceEx"/> function. 
		/// See <see cref="FR_PRIVATE"/> and <see cref="FR_NOT_ENUM"/>.
		/// </param>
		/// <param name="pdv">Reserved. Must be zero.</param>
		/// <returns>
		/// <para>If the function succeeds, the return value is nonzero.</para>
		/// <para>If the function fails, the return value is zero. No extended error information is available.</para>
		/// </returns>
		/// <seealso cref="AddFontResourceEx(string, uint, IntPtr)"/>
		/// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/dd183327(v=vs.85).aspx">AddFontResourceEx (MSDN)</seealso>
		/// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/dd162923(v=vs.85).aspx">RemoveFontResourceEx (MSDN)</seealso>
		[DllImport(LIBRARY)]
		public static extern bool RemoveFontResourceEx(string lpszFilename, uint fl, IntPtr pdv);

		[DllImport(LIBRARY)]

		public static extern IntPtr AddFontMemResourceEx(byte[] pbFont, int cbFont, IntPtr pdv, out uint pcFonts);

		[DllImport(LIBRARY)]
		public static extern int SelectClipRgn(IntPtr hdc, IntPtr hrgn);

		[DllImport(LIBRARY)]
		public static extern bool DeleteObject(IntPtr hObject);

		[DllImport(LIBRARY, ExactSpelling = true, SetLastError = true)]
		public static extern bool DeleteDC(IntPtr hdc);

		[DllImport(LIBRARY)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BitBlt(IntPtr hdc, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc,
			int nXSrc, int nYSrc, uint dwRop);

		[DllImport(LIBRARY, EntryPoint = "GdiAlphaBlend")]
		public static extern bool AlphaBlend(IntPtr hdcDest, int nXOriginDest, int nYOriginDest, int nWidthDest,
			int nHeightDest, IntPtr hdcSrc, int nXOriginSrc, int nYOriginSrc, int nWidthSrc, int nHeightSrc,
			BlendFunction blendFunction);

		[DllImport(LIBRARY, ExactSpelling = true, SetLastError = true)]
		public static extern IntPtr CreateCompatibleDC(IntPtr hdc);

		[DllImport(LIBRARY)]
		public static extern IntPtr CreateDIBSection(IntPtr hdc, [In] ref BitMapInfo pbmi, uint iUsage, out IntPtr ppvBits,
			IntPtr hSection, uint dwOffset);

		[DllImport(LIBRARY)]
		public static extern int SetBkMode(IntPtr hdc, int mode);

		[DllImport(LIBRARY)]
		public static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiObj);

		[DllImport(LIBRARY)]
		public static extern int SetTextColor(IntPtr hdc, int color);

		[DllImport(LIBRARY, EntryPoint = "GetTextExtentPoint32W")]
		public static extern int GetTextExtentPoint32(IntPtr hdc, [MarshalAs(UnmanagedType.LPWStr)] string str, int len,
			ref Size size);

		[DllImport(LIBRARY, EntryPoint = "GetTextExtentExPointW")]
		public static extern bool GetTextExtentExPoint(IntPtr hDc, [MarshalAs(UnmanagedType.LPWStr)] string str, int nLength,
			int nMaxExtent, int[] lpnFit, int[] alpDx, ref Size size);

		[DllImport(LIBRARY, EntryPoint = "TextOutW")]
		public static extern bool TextOut(IntPtr hdc, int x, int y, [MarshalAs(UnmanagedType.LPWStr)] string str, int len);

		public struct Rect
		{
			public int Left { get; set; }

			public int Top { get; set; }

			public int Right { get; set; }

			public int Bottom { get; set; }

			public Rect(int left, int top, int bottom, int right)
			{
				Left = left;
				Top = top;
				Bottom = bottom;
				Right = right;
			}

			public Rect(Rectangle rect)
			{
				Left = rect.Left;
				Top = rect.Top;
				Bottom = rect.Bottom;
				Right = rect.Right;
			}
		}


		[StructLayout(LayoutKind.Sequential)]
		public struct BlendFunction
		{
			public byte BlendOp;
			public byte BlendFlags;
			public byte SourceConstantAlpha;
			public byte AlphaFormat;

			public BlendFunction(byte alpha)
			{
				BlendOp = 0;
				BlendFlags = 0;
				AlphaFormat = 0;
				SourceConstantAlpha = alpha;
			}
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct BitMapInfo
		{
			public int biSize;
			public int biWidth;
			public int biHeight;
			public short biPlanes;
			public short biBitCount;
			public int biCompression;
			public int biSizeImage;
			public int biXPelsPerMeter;
			public int biYPelsPerMeter;
			public int biClrUsed;
			public int biClrImportant;
			public byte bmiColors_rgbBlue;
			public byte bmiColors_rgbGreen;
			public byte bmiColors_rgbRed;
			public byte bmiColors_rgbReserved;
		}
	}

	/// <summary>
	/// See http://msdn.microsoft.com/en-us/library/windows/desktop/dd162498(v=vs.85).aspx
	/// </summary>
	[Flags]
	public enum TextFormatFlags : uint
	{
		Default = 0x00000000,
		Center = 0x00000001,
		Right = 0x00000002,
		VCenter = 0x00000004,
		Bottom = 0x00000008,
		WordBreak = 0x00000010,
		SingleLine = 0x00000020,
		ExpandTabs = 0x00000040,
		TabStop = 0x00000080,
		NoClip = 0x00000100,
		ExternalLeading = 0x00000200,
		CalcRect = 0x00000400,
		NoPrefix = 0x00000800,
		Internal = 0x00001000,
		EditControl = 0x00002000,
		PathEllipsis = 0x00004000,
		EndEllipsis = 0x00008000,
		ModifyString = 0x00010000,
		RtlReading = 0x00020000,
		WordEllipsis = 0x00040000,
		NoFullWidthCharBreak = 0x00080000,
		HidePrefix = 0x00100000,
		ProfixOnly = 0x00200000,
	}
}

