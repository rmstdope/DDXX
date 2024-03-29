using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;
using Dope.DDXX.SceneGraph;
using Dope.DDXX.Physics;
using Dope.DDXX.TextureBuilder;
using Microsoft.Xna.Framework.Graphics;
using Dope.DDXX.ModelBuilder;
using Dope.DDXX.Utility;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.DemoFramework
{
    public abstract class BaseDemoEffect : Registerable, IDemoEffect
    {
        private int drawOrder;
        private IGraphicsFactory graphicsFactory;
        private ModelBuilder.ModelBuilder modelBuilder;
        private ModelDirector modelDirector;
        private TextureDirector textureDirector;
        private IDemoMixer mixer;
        private PostProcessor postProcessor;
        private IScene scene;

        protected BaseDemoEffect(string name, float startTime, float endTime)
            : base(name, startTime, endTime)
        {
            drawOrder = 0;
            scene = new Scene();
            scene.AmbientColor = new Color(255, 255, 255, 255);
        }

        protected IGraphicsFactory GraphicsFactory
        {
            get { return graphicsFactory; }
        }

        protected GraphicsDevice GraphicsDevice
        {
            get { return graphicsFactory.GraphicsDevice; }
        }

        protected EffectFactory EffectFactory
        {
            get { return graphicsFactory.EffectFactory; }
        }

        protected ModelFactory ModelFactory
        {
            get { return graphicsFactory.ModelFactory; }
        }

        protected TextureFactory TextureFactory
        {
            get { return graphicsFactory.TextureFactory; }
        }

        protected IDemoMixer Mixer
        {
            get { return mixer; }
        }

        protected PostProcessor PostProcessor 
        {
            get { return postProcessor; }
        }

        protected ModelBuilder.ModelBuilder ModelBuilder
        {
            // Lazy creation
            get
            {
                if (modelBuilder == null)
                    modelBuilder = new ModelBuilder.ModelBuilder(GraphicsFactory, TextureFactory, EffectFactory, new BasicEffect(GraphicsDevice));
                return modelBuilder;
            }
        }

        protected TextureDirector TextureDirector
        {
            // Lazy creation
            get
            {
                if (textureDirector == null)
                    textureDirector = new TextureDirector(TextureFactory);
                return textureDirector;
            }
        }

        protected ModelDirector ModelDirector
        {
            // Lazy creation
            get
            {
                if (modelDirector == null)
                    modelDirector = new ModelDirector(ModelBuilder);
                return modelDirector;
            }
        }

        public IScene Scene
        {
            get { return scene; }
        }

        public int DrawOrder
        {
            set { drawOrder = value; }
            get { return drawOrder; }
        }

        protected void CreateStandardCamera(out CameraNode camera, float distance)
        {
            camera = new CameraNode("Standard Camera", 
                (float)GraphicsDevice.PresentationParameters.BackBufferWidth /
                (float)GraphicsDevice.PresentationParameters.BackBufferHeight);
            camera.WorldState.MoveForward(-distance);
            scene.AddNode(camera);
            scene.ActiveCamera = camera;
        }

        protected void CreateStandardCamera(float distance)
        {
            CameraNode camera = new CameraNode("Standard Camera", 
                (float)GraphicsDevice.PresentationParameters.BackBufferWidth /
                (float)GraphicsDevice.PresentationParameters.BackBufferHeight);
            camera.WorldState.MoveBackward(distance);
            scene.AddNode(camera);
            scene.ActiveCamera = camera;
        }

        protected float EffectTime
        {
            get { return Time.CurrentTime - StartTime; }
        }

        protected abstract void Initialize();

        protected int BackbufferWidth
        {
            get
            {
                return GraphicsDevice.PresentationParameters.BackBufferWidth;
            }
        }

        protected int BackbufferHeight
        {
            get
            {
                return GraphicsDevice.PresentationParameters.BackBufferHeight;
            }
        }

        #region IDemoEffect Members

        public abstract void Step();

        public abstract void Render();

        public void Initialize(IGraphicsFactory graphicsFactory, IDemoMixer mixer, PostProcessor postProcessor)
        {
            this.graphicsFactory = graphicsFactory;
            this.mixer = mixer;
            this.postProcessor = postProcessor;

            Initialize();
        }

        #endregion

    }
}
