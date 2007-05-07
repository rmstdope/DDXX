using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

using Dope.DDXX.DemoEffects;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.Graphics;
using Dope.DDXX.Input;
using Dope.DDXX.Sound;
using Dope.DDXX.Utility;

namespace TiVi
{
    internal class TiViMain
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
                    window.Initialize("Engine Test", desc, prerequisits);
                    executer.Initialize(D3DDriver.GetInstance().Device,
                        D3DDriver.GraphicsFactory, D3DDriver.TextureFactory,
                        "", new Assembly[] { Assembly.GetExecutingAssembly(), Assembly.GetAssembly(typeof(GlowPostEffect)) }, "TiVi.xml");

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
            desc = setup.DeviceDescription;
            window = new DemoWindow();
            executer = new DemoExecuter(new DemoFactory(), 
                SoundDriver.GetInstance(), InputDriver.GetInstance(),
                new PostProcessor());
        }

    }
}
