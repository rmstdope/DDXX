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
using Dope.DDXX.TextureBuilder;

namespace PoseidonTest
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
                SetupLogic setupLogic = new SetupLogic();
                SetupDialog setupDialog = new SetupDialog(setupLogic);

                setupDialog.ShowDialog();

                if (setupLogic.OK)
                {
                    DeviceDescription desc;

                    SetupFramework(setupLogic, out window, out executer, out desc);

                    FileUtility.SetLoadPaths("../../Data/",
                                             "../../../Effects/",
                                             "../../",
                                             "../../../EngineTest/Data");
                    DevicePrerequisits prerequisits = new DevicePrerequisits();

                    window.Initialize("PoseidonTest", desc, prerequisits);
                    executer.Initialize(D3DDriver.GetInstance().Device,
                        D3DDriver.GraphicsFactory, D3DDriver.TextureFactory,
                        new TextureBuilder(D3DDriver.TextureFactory),
                        "PoseidonTest.xml");
                    /* "dope-wanting_more-dhw2006-v2-320.mp3" */
                    //    new Assembly[] { Assembly.GetExecutingAssembly() }, 
                    //    "PoseidonTest.xml");
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
            float length = 65000.0f;
            PosseTestEffect posseEffect = new PosseTestEffect(0.0f, length);
            RealRenderPostEffect renderPostEffect = new RealRenderPostEffect(0.0f, length);
            renderPostEffect.SetPosseTestEffect(posseEffect);
            executer.Register(0, posseEffect);
            executer.Register(0, renderPostEffect);
            //MonochromePostEffect monochrome = new MonochromePostEffect(0.0f, length);
            //executer.Register(0, monochrome);
            //GlowPostEffect postEffect = new GlowPostEffect(0.0f, length);
            //postEffect.Luminance = 0.06f;
            //postEffect.Exposure = 0.1f;
            //postEffect.WhiteCutoff = 0.2f;
            //postEffect.BloomScale = 1.5f;
        }

        private static void SetupFramework(SetupLogic setup, out DemoWindow window, out DemoExecuter executer, out DeviceDescription desc)
        {
            DemoEffectTypes effectTypes = new DemoEffectTypes(new Assembly[] { 
                Assembly.GetExecutingAssembly(), 
                Assembly.GetAssembly(typeof(GlowPostEffect)),
                Assembly.GetAssembly(typeof(IGenerator))});
            desc = setup.DeviceDescription;
            window = new DemoWindow();
            executer = new DemoExecuter(new DemoFactory(),
                SoundDriver.GetInstance(), InputDriver.GetInstance(),
                new PostProcessor(), effectTypes);
        }

    }
}
