using System;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.Input;
using Dope.DDXX.Sound;
using Microsoft.Xna.Framework.Input;
using Dope.DDXX.Utility;
using System.Reflection;
using Dope.DDXX.DemoEffects;
using Dope.DDXX.TextureBuilder;

namespace EngineTest
{
    static class EngineTest
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            Assembly[] assemblies = new Assembly[] { 
                Assembly.GetExecutingAssembly(),
                typeof(GlowPostEffect).Assembly,
                typeof(TextureBuilder).Assembly };
            FileUtility.SetLoadPaths(new string[] { "./", "../../../", "xml/" });
            DemoWindow window = new DemoWindow("Pelle", "EngineTest.xml", assemblies);
            if (window.SetupDialog())
                window.Run();
        }
    }
}

