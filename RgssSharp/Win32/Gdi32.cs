using System;
using System.Runtime.InteropServices;

namespace RgssSharp.Win32
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
	}
}

