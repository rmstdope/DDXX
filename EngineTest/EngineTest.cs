using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using DemoFramework;

namespace EngineTest
{
    public class Runner
    {
        static void Main()
        {
            try
            {
                DemoWindow window = new DemoWindow();
                DemoExecuter executer = new DemoExecuter();
                window.Initialize(800, 600, "RolemasterTest");
                executer.Initialize();
                executer.Run();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }

        }

    }
}
