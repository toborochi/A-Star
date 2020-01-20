using Microsoft.Xna.Framework;
using System;

namespace AStar
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (var game = new Game1())
            {
                game.Window.Title = "Behaviour";
                game.Run();
            }
        }
    }
#endif
}
