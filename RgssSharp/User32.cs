using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace RgssSharp
{
	internal static class User32
	{
		[DllImport("user32.dll", EntryPoint = "DrawTextW")]
		public static extern int DrawText(IntPtr hdc, [MarshalAs(UnmanagedType.LPWStr)] string str, int len,
			ref Gdi32.Rect rect,
			uint uFormat);

	}
}
