using System;
using System.Linq;
using RgssSharp.Win32;

namespace RgssSharp
{
#if WINDOWS 
	internal static class Program
	{
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static void Main(string[] args)
        {
			
	        System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(true);
            using (var game = new RgssPlayer())
            {
	            ParseArgs(args, game);
				game.Run();
            }
        }

	    private static void ParseArgs(string[] args, RgssPlayer game)
	    {
			string[] debugArgs = { "-d", "-debug", "-t", "-test" };
			game.Debug = args.Any(arg => debugArgs.Contains(arg));
		    if ((game.Debug && args.Contains("-noconsole")) || args.Contains("-console"))
		    {
			    Kernel32.AllocConsole();
			    Kernel32.SetConsoleTitle(game.Window.Title + " - Console");
			    var handle = Kernel32.GetConsoleWindow();
			    User32.SetWindowPos(handle, IntPtr.Zero, 32, 32, 640, 480, SetWindowPosFlags.ShowWindow);
		    }
			   
	    }
    }
#endif
}

