using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using DemoFramework;
using Microsoft.DirectX.Direct3D;
using Graphics;
using Utility;

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
            TestEffect effect = new TestEffect(0.0f, 10.0f);
            executer.Register(0, effect);
        }

        private static void SetupFramework(SetupDialog setup, out DemoWindow window, out DemoExecuter executer, out DeviceDescription desc)
        {
            desc = setup.DeviceDescription;
            window = new DemoWindow();
            executer = new DemoExecuter();
        }

    }
}
