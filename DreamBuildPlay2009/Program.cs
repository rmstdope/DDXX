using System;
using Dope.DDXX.GameFramework;
using Dope.DDXX.Graphics;
using Dope.DDXX.Input;

namespace Dope.DDXX.DreamBuildPlay2009
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            //graphicsFactory.SetScreen(1280, 800, true);
            using (GameMain game = new GameMain(new GameExecuter(), new SpriteMove()))
            {
                game.Run();
            }
        }
    }
}

