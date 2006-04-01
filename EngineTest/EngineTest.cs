using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Direct3D;

namespace EngineTest
{
    public class MainWindow : System.Windows.Forms.Form
    {
        public MainWindow()
        {
            this.Size = new System.Drawing.Size(800, 600);
            this.Text = "Engine Test";
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.Opaque, true);
        }

        public bool InitializeGraphics()
        {
            try
            {
                DeviceDescription desc;
                desc.windowed = true;
                desc.useDepth = true;
                desc.useStencil = true;
                desc.deviceType = DeviceType.Hardware;
                D3DDriver driver = D3DDriver.GetInstance();
                driver.Init(this, desc);                
                return true;
            }
            catch (DirectXException exception)
            {
                MessageBox.Show("Error: " + exception.ErrorString);
                return false;
            }
        }

        static void Main()
        {
            MainWindow window = new MainWindow();

            if (window.InitializeGraphics())
            {
                window.Cursor.Dispose();
                window.Show();

                try
                {
                    Device device = D3DDriver.GetInstance().GetDevice();
                    //mesh = Mesh.FromFile("../../Data/airplane 2.x", MeshFlags.SystemMemory, device);
                }
                catch (DirectXException e)
                {
                    MessageBox.Show(e.Message + " : " + e.ErrorString);
                }

                // While the form is still valid, render and process messages
                while (window.Created)
                {
                    window.Render();
                    Application.DoEvents();
                }
            }
            Application.Exit();
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            this.Render();
        }

        protected override void OnKeyPress(System.Windows.Forms.KeyPressEventArgs e)
        {
            if ((int)(byte)e.KeyChar == (int)System.Windows.Forms.Keys.Escape)
                this.Close();
        }

        private void Render()
        {
            Device device = D3DDriver.GetInstance().GetDevice();

            CustomVertex.TransformedColored[] vertices = new CustomVertex.TransformedColored[3];

            vertices[0].Position = new Vector4(150f, 100f, 0f, 1f);
            vertices[0].Color = Color.Red.ToArgb();
            vertices[1].Position = new Vector4(this.Width / 2 + 100f, 100f, 0f, 1f);
            vertices[1].Color = Color.Green.ToArgb();
            vertices[2].Position = new Vector4(250f, 300f, 0f, 1f);
            vertices[2].Color = Color.Yellow.ToArgb(); 

            //Clear the backbuffer to a blue color (ARGB = 000000ff)
            device.Clear(ClearFlags.Target, System.Drawing.Color.Blue, 1.0f, 0);

            //Begin the scene
            device.BeginScene();

            device.VertexFormat = CustomVertex.TransformedColored.Format;
            device.DrawUserPrimitives(PrimitiveType.TriangleList, 1, vertices);

            device.EndScene();
            device.Present();

            Invalidate();
        }
    }
}
