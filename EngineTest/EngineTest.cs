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

                    RegisterEffects(executer);

                    window.Initialize("RolemasterTest", desc);
                    executer.Initialize("");//test.mp3");
                    executer.Run();
                    window.CleanUp();
                }
            }
            catch (DDXXException exception)
            {
                MessageBox.Show(exception.ToString());
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
            }

        }

        private static void RegisterEffects(DemoExecuter executer)
        {
            //TestEffect postEffect = new TestEffect(0.0f, 10.0f);
            //executer.Register(0, postEffect);
            Assembly assembly = Assembly.GetExecutingAssembly();
            foreach (Type t in assembly.GetTypes())
            {
                TypeFilter filter = new TypeFilter(delegate(Type ty, object comp)
                {
                    if (ty.FullName == (string)comp)
                        return true;
                    else
                        return false;
                });
                Type[] interfaces = t.FindInterfaces(filter, "Dope.DDXX.DemoFramework.IDemoEffect");
                if (interfaces.Length > 0)
                {
                    Type effect = t;
                    Type[] constrArgs = new Type[] { typeof(float), typeof(float) };
                    ConstructorInfo constrInfo = effect.GetConstructor(constrArgs);
                    if (constrInfo == null)
                        throw new DDXXException("Couldn't find constructor (float,float) in " + effect.FullName);
                    IDemoEffect demoEffect = (IDemoEffect)constrInfo.Invoke(new object[] { 0.0f, 10.0f });
                    if (demoEffect == null)
                        throw new DDXXException("Couldn't create instance of " + effect.FullName);
                    executer.Register(0, demoEffect);
                }
            }
            MonochromePostEffect monochrome = new MonochromePostEffect(0.0f, 10.0f);
            executer.Register(0, monochrome);
            GlowPostEffect postEffect = new GlowPostEffect(0.0f, 10.0f);
            postEffect.Luminance = 0.06f;
            postEffect.Exposure = 0.3f;
            postEffect.WhiteCutoff = 0.1f;
            postEffect.BloomScale = 1.5f;
            executer.Register(0, postEffect);
        }

        private static void SetupFramework(SetupDialog setup, out DemoWindow window, out DemoExecuter executer, out DeviceDescription desc)
        {
            desc = setup.DeviceDescription;
            window = new DemoWindow();
            executer = new DemoExecuter();
        }

    }
}
