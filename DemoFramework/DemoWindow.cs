using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Reflection;
using Dope.DDXX.Graphics;
using Dope.DDXX.Utility;
using Dope.DDXX.Sound;
using Dope.DDXX.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Dope.DDXX.DemoFramework
{
    public class DemoWindow : Game
    {
        private GraphicsDeviceManager graphicsDeviceManager;
        private DemoExecuter executer;
        private IGraphicsFactory graphicsFactory;
        private string xmlFile;
        private DeviceParameters deviceParameters;

        public AspectRatio AspectRatio
        {
            get
            {
                return new AspectRatio(Window.ClientBounds.Width, Window.ClientBounds.Height); 
            }
        }

        public DemoWindow(string name, string xmlFile, Assembly[] assemblies, IDemoTweakerHandler tweakerHandler)
        //new DemoTweakerHandler(new TweakerSettings())
        {
            this.xmlFile = xmlFile;
            Window.Title = name;

            graphicsFactory = new GraphicsFactory(this, Services);

            DemoEffectTypes effectTypes = new DemoEffectTypes(assemblies);
            executer = new DemoExecuter(new DemoFactory(), new SoundFactory(graphicsFactory.ContentManager),
                InputDriver.GetInstance(), new PostProcessor(), effectTypes, 
                tweakerHandler);

            graphicsDeviceManager = graphicsFactory.GraphicsDeviceManager;
            graphicsDeviceManager.GraphicsProfile = GraphicsProfile.Reach;
            graphicsDeviceManager.PreparingDeviceSettings += new EventHandler<PreparingDeviceSettingsEventArgs>(graphics_PreparingDeviceSettings);
            //bool reach = GraphicsAdapter.DefaultAdapter.IsProfileSupported(GraphicsProfile.Reach);

            deviceParameters = new DeviceParameters(1280, 720, false, false, true, false);
        }

        void graphics_PreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
        {
#if !XBOX
            if (deviceParameters.FullScreen)
            {
                e.GraphicsDeviceInformation.PresentationParameters.IsFullScreen = true;
            }
            else
            {
                e.GraphicsDeviceInformation.PresentationParameters.IsFullScreen = false;
            }
            e.GraphicsDeviceInformation.GraphicsProfile = deviceParameters.GraphicsProfile;
            //if (deviceParameters.DeviceType == DeviceType.Reference)
            //{
            //    //e.GraphicsDeviceInformation.CreationOptions = CreateOptions.SoftwareVertexProcessing;
            //}
            e.GraphicsDeviceInformation.PresentationParameters.BackBufferWidth = deviceParameters.BackBufferWidth;
            e.GraphicsDeviceInformation.PresentationParameters.BackBufferHeight = deviceParameters.BackBufferHeight;
#endif
        }

        protected override void Initialize()
        {
            if (!GraphicsAdapter.DefaultAdapter.IsProfileSupported(GraphicsProfile.Reach))
                throw new DDXXException("Your graphics device does not seem to support shader model 2.");
            TextureFactory textureFactory = new TextureFactory(graphicsFactory);
            EffectFactory effectFactory = new EffectFactory(graphicsFactory);
            ModelFactory modelFactory = new ModelFactory(graphicsFactory, textureFactory);

            executer.Initialize(graphicsFactory, xmlFile, deviceParameters);

            if (AspectRatio.Ratios.RATIO_INVALID == new AspectRatio(Window.ClientBounds.Width, Window.ClientBounds.Height).Ratio)
                throw new DDXXException("Width and height of window does not match any valid aspect ratio.");

            base.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            Time.Step((float)gameTime.ElapsedGameTime.TotalSeconds);

            executer.Step();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            executer.Render();
            base.Draw(gameTime);

            if (!executer.IsRunning())
                Exit();
        }

        protected override void EndRun()
        {
            executer.CleanUp();
            base.EndRun();
        }

        public bool SetupDialog()
        {
#if WINDOWS
            SetupLogic logic = new SetupLogic();
            SetupForm setup = new SetupForm(logic);
            //setup.ShowDialog();
            //deviceParameters = logic.DeviceParameters;
            //return logic.OK;
            return true;
#else
            return true;
#endif
        }
    }
}
