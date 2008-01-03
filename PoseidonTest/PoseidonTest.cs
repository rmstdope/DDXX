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
                                             "../../");
                    DevicePrerequisits prerequisits = new DevicePrerequisits();

                    window.Initialize("PoseidonTest", desc, prerequisits);
                    executer.Initialize(D3DDriver.GetInstance().Device,
                        D3DDriver.GraphicsFactory, D3DDriver.TextureFactory,
                        D3DDriver.EffectFactory, new TextureBuilder(D3DDriver.TextureFactory),
                        "PoseidonTest.xml");

                    executer.Run();
                    window.CleanUp();
                }
            }
            catch (DDXXException exception)
            {
                Cursor.Show();
                if (DialogResult.Yes == MessageBox.Show(exception.ToString(), "It seems you are having problems...", MessageBoxButtons.YesNo, MessageBoxIcon.Error, MessageBoxDefaultButton.Button2))
                {
                    MessageBox.Show(exception.Callstack(), "Callstack");
                }
            }
            catch (Exception exception)
            {
                Cursor.Show();
                MessageBox.Show(exception.ToString(), "Demo Error");
            }
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
