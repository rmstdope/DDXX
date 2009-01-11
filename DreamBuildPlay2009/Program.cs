using System;
using GameFramework;
using Dope.DDXX.Graphics;
using Dope.DDXX.Input;

namespace DreamBuildPlay2009
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            //graphicsFactory.SetScreen(1280, 800, true);
            using (GameMain game = new GameMain(new GameExecuter()))
            {
                game.Run();
            }
        }
    }
}

