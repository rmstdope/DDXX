using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Dope.DDXX.DemoEffects;
using Dope.DDXX.TextureBuilder;
using Dope.DDXX.Utility;
using Dope.DDXX.DemoFramework;
#if (!XBOX)
using System.Windows.Forms;
#endif

namespace DFM2007Invitro
{
    public class Invitro
    {
        static void Main(string[] args)
        {
            Assembly[] assemblies = new Assembly[] { 
                Assembly.GetExecutingAssembly(),
                typeof(GlowPostEffect).Assembly,
                typeof(TextureBuilder).Assembly };
            FileUtility.SetLoadPaths(new string[] { "./", "../../../", "Content/xml/" });
            DemoWindow window = new DemoWindow("Invitro", "DFM2007.xml", assemblies);
            try
            {
                if (window.SetupDialog())
                    window.Run();
            }
            catch (Exception exception)
            {
#if (!XBOX)
                Cursor.Show();
                MessageBox.Show(exception.ToString(), "It seems you are having problems...");
#endif
            }
        }
    }
}