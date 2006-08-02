using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using DemoFramework;
using Microsoft.DirectX.Direct3D;
using Direct3D;
using Utility;

namespace EngineTest
{
    public class Runner
    {
        static void Main()
        {
            try
            {
                // Run setup form
                SetupDialog setup = new SetupDialog();

                setup.ShowDialog();

                if (setup.OK)
                {
                    // Setup desciptor
                    DeviceDescription desc = setup.DeviceDescription;
                    DemoWindow window = new DemoWindow(new D3DFactory());
                    DemoExecuter executer = new DemoExecuter();
                    window.Initialize("RolemasterTest", desc);
                    executer.Initialize("test.mp3");
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

    }
}
