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

                    //Viewport viewport = D3DDriver.GetInstance().Device.Viewport;
                    //float ratio = (10.0f / 16.0f) * (viewport.Width / (float)viewport.Height);
                    //TextureBuilder.TextureBuilder builder = new TextureBuilder.TextureBuilder(D3DDriver.TextureFactory);
                    //IGenerator rect = new RoundedRectangle(new Vector2(1.0f, ratio), new Vector2(0.5f, 0.5f), 0.05f);
                    //OverlayPostEffect overlay = new OverlayPostEffect(0, 1000.0f);
                    //overlay.AddNoise = true;
                    //overlay.Texture = builder.Generate(rect, viewport.Width, viewport.Height, 1, Format.A8R8G8B8);
                    //executer.Register(0, overlay);

                    executer.Initialize(D3DDriver.GetInstance().Device,
                        D3DDriver.GraphicsFactory, D3DDriver.TextureFactory,
                        D3DDriver.EffectFactory, new TextureBuilder(D3DDriver.TextureFactory),
                        "EngineTest.xml");
//new Assembly[] { Assembly.GetExecutingAssembly(), 
//                            Assembly.GetAssembly(typeof(GlowPostEffect))},
                        
                    executer.Run();
                    window.CleanUp();
                }
            }
            catch (DDXXException exception)
            {
                for (int i = 0; i < 2; i++)
                {
                    Cursor.Show();
                    if (DialogResult.Yes == MessageBox.Show(exception.ToString(), "It seems you are having problems...", MessageBoxButtons.YesNo, MessageBoxIcon.Error, MessageBoxDefaultButton.Button2))
                    {
                        MessageBox.Show(exception.Callstack(), "Callstack");
                    }
                }
            }
            catch (Exception exception)
            {
                for (int i = 0; i < 2; i++)
                {
                    Cursor.Show();
                    MessageBox.Show(exception.ToString(), "Demo Error");
                }
            }

        }

        //private static void RegisterEffects(DemoExecuter executer)
        //{
        //    //TestEffect effect = new TestEffect(0.0f, 10.0f);
        //    //executer.Register(0, effect);
        //    //executer.Register(5, effect);
        //    //executer.Register(0, new EmptyEffect(1.0f, 3.0f));
        //    //executer.Register(0, new EmptyEffect(5.0f, 8.0f));

        //    //executer.Register(0, new EmptyEffect(2.0f, 5.0f));
        //    //executer.Register(0, new EmptyEffect(6.0f, 9.0f));
        //    SpinningBackgroundEffect effect2 = new SpinningBackgroundEffect(0.0f, 10.0f);
        //    effect2.AddTextureLayer(new SpinningBackgroundEffect.TextureLayer("BlurBackground.jpg", 35.0f, Color.Beige, 0.2f));
        //    effect2.AddTextureLayer(new SpinningBackgroundEffect.TextureLayer("BlurBackground.jpg", -44.0f, Color.Coral, 0.2f));
        //    executer.Register(0, effect2);
        //    float length = 65000.0f;
        //    MonochromePostEffect monochrome = new MonochromePostEffect(0.0f, length);
        //    executer.Register(0, monochrome);
        //    GlowPostEffect postEffect = new GlowPostEffect(0.0f, length);
        //    postEffect.Luminance = 0.06f;
        //    postEffect.Exposure = 0.1f;
        //    postEffect.WhiteCutoff = 0.2f;
        //    postEffect.BloomScale = 1.5f;
        //    executer.Register(0, postEffect);
        //}

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
