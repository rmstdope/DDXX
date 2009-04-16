using System;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.Input;
using Dope.DDXX.Sound;
using Microsoft.Xna.Framework.Input;
using Dope.DDXX.Utility;
using System.Reflection;
using Dope.DDXX.DemoEffects;
using Dope.DDXX.TextureBuilder;
using Microsoft.Xna.Framework.Audio;
using Dope.DDXX.DemoTweaker;

namespace EngineTest
{
    static class EngineTest
    {
        static void Main(string[] args)
        {
#if (!DEBUG)
            try
            {
#endif
                Assembly[] assemblies = new Assembly[] { 
                    Assembly.GetExecutingAssembly(),
                    typeof(GlowPostEffect).Assembly,
                    typeof(TextureDirector).Assembly };
                FileUtility.SetLoadPaths(new string[] { "./", "../../../xml/" });
                DemoWindow window = new DemoWindow("Pelle", "EngineTest.xml", assemblies, new DemoTweakerHandler(new TweakerSettings()));

                if (window.SetupDialog())
                    window.Run();
#if (!DEBUG)
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.ToString(), "Exception was Thrown");
            }
        }
#endif
    }
}

