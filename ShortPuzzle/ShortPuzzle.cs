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
using Dope.DDXX.Sound;
using Dope.DDXX.Input;

namespace ShortPuzzle
{
    class ShortPuzzle
    {
        static void Main()
        {
            DemoWindow window;
            DemoExecuter executer;

            try
            {
                FileUtility.SetLoadPaths(new string[] { "../../../Effects/", "../../../../Short Puzzle Data/" });

                // Run setup form
                SetupLogic setupLogic = new SetupLogic();
                SetupDialog setupDialog = new SetupDialog(setupLogic);

                setupDialog.ShowDialog();

                if (setupLogic.OK)
                {
                    DeviceDescription desc;

                    SetupFramework(setupLogic, out window, out executer, out desc);

                    RegisterEffects(executer);

                    DevicePrerequisits prerequisits = new DevicePrerequisits();
                    prerequisits.ShaderModel = 2;

                    window.Initialize("Short Puzzle", desc, prerequisits);
                    executer.Initialize("scanner_of_dope-woo-192.mp3");//test.mp3");
                    executer.Run();
                    window.CleanUp();
                }
            }
            catch (DDXXException exception)
            {
                exception.PresentInMessageBox();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
            }

        }

        private static void RegisterEffects(DemoExecuter executer)
        {
            float length = 65000.0f;
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
                        throw new DDXXException("Couldn'maxTime find constructor (float,float) in " + effect.FullName);
                    IDemoEffect demoEffect = (IDemoEffect)constrInfo.Invoke(new object[] { 0.0f, length });
                    if (demoEffect == null)
                        throw new DDXXException("Couldn'maxTime create instance of " + effect.FullName);
                    executer.Register(0, demoEffect);
                }
            }
            MonochromePostEffect monochrome = new MonochromePostEffect(0.0f, length);
            //executer.Register(0, monochrome);
            GlowPostEffect postEffect = new GlowPostEffect(0.0f, length);
            postEffect.Luminance = 0.06f;
            postEffect.Exposure = 0.1f;
            postEffect.WhiteCutoff = 0.2f;
            postEffect.BloomScale = 1.5f;
            executer.Register(0, postEffect);
        }

        private static void SetupFramework(SetupLogic setup, out DemoWindow window, out DemoExecuter executer, out DeviceDescription desc)
        {
            desc = setup.DeviceDescription;
            window = new DemoWindow();
            executer = new DemoExecuter(D3DDriver.GetInstance().Device,
                D3DDriver.GraphicsFactory, D3DDriver.TextureFactory,
                SoundDriver.GetInstance(), InputDriver.GetInstance(),
                new PostProcessor());
        }
    }
}
