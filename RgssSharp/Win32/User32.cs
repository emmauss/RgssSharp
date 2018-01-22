using System;
using System.Runtime.InteropServices;

namespace RgssSharp.Win32
{
	internal static class User32
	{
		public const string LIBRARY = "user32.dll";


		[DllImport(LIBRARY)]
		public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, SetWindowPosFlags uFlags);
	}
}
