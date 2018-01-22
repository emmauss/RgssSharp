using System;
using System.Runtime.InteropServices;

namespace RgssSharp.Win32
{
	internal static class Kernel32
	{
		public const string LIBRARY = "kernel32.dll";

		[DllImport(LIBRARY)]
		public static extern bool AllocConsole();

		[DllImport(LIBRARY)]
		public static extern bool FreeConsole();

		[DllImport(LIBRARY)]
		public static extern bool SetConsoleTitle(string lpConsoleTitle);

		[DllImport(LIBRARY)]
		public static extern IntPtr GetConsoleWindow();

		[DllImport(LIBRARY, EntryPoint = "RtlMoveMemory")]
		public static extern void CopyMemory(IntPtr dest, IntPtr src, uint length);

		[DllImport(LIBRARY, EntryPoint = "RtlMoveMemory")]
		public static extern unsafe void CopyMemory(void* dest, void* src, int length);


	}
}
