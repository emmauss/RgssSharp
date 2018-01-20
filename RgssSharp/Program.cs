using System;

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
            using (RgssPlayer game = new RgssPlayer())
            {
                game.Run();
            }
        }
    }
#endif
}

