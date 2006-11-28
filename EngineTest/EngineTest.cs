using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Dope.DDXX.DemoFramework;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System.Reflection;
using System.Text;
using Dope.DDXX.Graphics;
using Dope.DDXX.Utility;
using Dope.DDXX.DemoEffects;

namespace EngineTest
{
    public class Runner
    {

        static void Main()
        {
            DemoWindow window;
            DemoExecuter executer;

            try
            {

                // Run setup form
                SetupDialog setup = new SetupDialog();

                setup.ShowDialog();

                if (setup.OK)
                {
                    DeviceDescription desc;

                    SetupFramework(setup, out window, out executer, out desc);

                    //RegisterEffects(executer);
                    
                    FileUtility.SetLoadPaths("../../Data/", 
                                             "../../../Effects/",
                                             "../../");
                    DevicePrerequisits prerequisits = new DevicePrerequisits();

                    window.Initialize("Engine Test", desc, prerequisits);
                    executer.Initialize("", new Assembly[] {Assembly.GetExecutingAssembly(), Assembly.GetAssembly(typeof(GlowPostEffect)) }, "EngineTest.xml");

                    executer.Run();
                    window.CleanUp();
                }
            }
            catch (DDXXException exception)
            {
                if (DialogResult.Yes == MessageBox.Show(exception.ToString(), "It seems you are having problems...", MessageBoxButtons.YesNo, MessageBoxIcon.Error, MessageBoxDefaultButton.Button2))
                {
                    MessageBox.Show(exception.Callstack(), "Callstack"); 
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString(), "Demo Error");
            }

        }

        private static void RegisterEffects(DemoExecuter executer)
        {
            //TestEffect effect = new TestEffect(0.0f, 10.0f);
            //executer.Register(0, effect);
            //executer.Register(5, effect);
            //executer.Register(0, new EmptyEffect(1.0f, 3.0f));
            //executer.Register(0, new EmptyEffect(5.0f, 8.0f));

            //executer.Register(0, new EmptyEffect(2.0f, 5.0f));
            //executer.Register(0, new EmptyEffect(6.0f, 9.0f));
            SpinningBackgroundEffect effect2 = new SpinningBackgroundEffect(0.0f, 10.0f);
            effect2.AddTextureLayer(new SpinningBackgroundEffect.TextureLayer("BlurBackground.jpg", 35.0f, Color.Beige, 0.2f));
            effect2.AddTextureLayer(new SpinningBackgroundEffect.TextureLayer("BlurBackground.jpg", -44.0f, Color.Coral, 0.2f));
            executer.Register(0, effect2);
            float length = 65000.0f;
            MonochromePostEffect monochrome = new MonochromePostEffect(0.0f, length);
            executer.Register(0, monochrome);
            GlowPostEffect postEffect = new GlowPostEffect(0.0f, length);
            postEffect.Luminance = 0.06f;
            postEffect.Exposure = 0.1f;
            postEffect.WhiteCutoff = 0.2f;
            postEffect.BloomScale = 1.5f;
            executer.Register(0, postEffect);
        }

        private static void SetupFramework(SetupDialog setup, out DemoWindow window, out DemoExecuter executer, out DeviceDescription desc)
        {
            desc = setup.DeviceDescription;
            window = new DemoWindow();
            executer = new DemoExecuter(new PostProcessor());
        }

    }
}
