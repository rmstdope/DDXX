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

namespace Dope.DDXX.DemoFramework
{
    public abstract class BaseDemoEffect : Registerable, IDemoEffect
    {
        private int drawOrder;
        private IGraphicsFactory graphicsFactory;
        private IEffectFactory effectFactory;
        private ITextureFactory textureFactory;
        private IModelFactory modelFactory;
        private ModelBuilder.ModelBuilder modelBuilder;
        private ModelDirector modelDirector;
        private TextureDirector textureDirector;
        private IDemoMixer mixer;
        private IPostProcessor postProcessor;
        private IScene scene;

        protected BaseDemoEffect(string name, float startTime, float endTime)
            : base(name, startTime, endTime)
        {
            drawOrder = 0;
        }

        protected IGraphicsFactory GraphicsFactory
        {
            get { return graphicsFactory; }
        }

        protected IGraphicsDevice GraphicsDevice
        {
            get { return graphicsFactory.GraphicsDeviceManager.GraphicsDevice; }
        }

        protected IEffectFactory EffectFactory
        {
            get { return effectFactory; }
        }

        protected IModelFactory ModelFactory
        {
            // Lazy creation
            get
            {
                if (modelFactory == null)
                    modelFactory = new ModelFactory(GraphicsDevice, GraphicsFactory, TextureFactory);
                return modelFactory;
            }
        }

        protected ITextureFactory TextureFactory
        {
            get { return textureFactory; }
        }

        protected IDemoMixer Mixer
        {
            get { return mixer; }
        }

        protected IPostProcessor PostProcessor 
        {
            get { return postProcessor; }
        }

        protected ModelBuilder.ModelBuilder ModelBuilder
        {
            // Lazy creation
            get
            {
                if (modelBuilder == null)
                    modelBuilder = new ModelBuilder.ModelBuilder(GraphicsDevice, GraphicsFactory, TextureFactory, EffectFactory, EffectFactory.CreateFromFile("Content\\effects\\DefaultEffect"));
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

        protected IScene Scene
        {
            get { return scene; }
        }

        public int DrawOrder
        {
            set { drawOrder = value; }
            get { return drawOrder; }
        }

        protected void CreateStandardSceneAndCamera(out IScene scene, out CameraNode camera, float distance)
        {
            scene = new Scene();
            scene.AmbientColor = new Color(255, 255, 255, 255);
            camera = new CameraNode("Standard Camera");
            camera.WorldState.MoveForward(-distance);
            scene.AddNode(camera);
            scene.ActiveCamera = camera;
        }

        protected ModelNode CreateSimpleModelNode(IModel model, string effectFileName, string techniqueName)
        {
            return CreateSimpleModelNode(model, effectFileName, techniqueName, EffectFactory);
        }

        public static ModelNode CreateSimpleModelNode(IModel model, string effectFileName, string techniqueName, 
            IEffectFactory effectFactory)
        {
            throw new Exception("Not implemented");
            //IEffectHandler effectHandler = new EffectHandler(effectFactory.CreateFromFile(effectFileName),
            //    delegate(int material) { return techniqueName; }, model);
            //return new ModelNode("", model, effectHandler, device);
        }

        protected float EffectTime
        {
            get { return Time.CurrentTime - StartTime; }
        }

        protected abstract void Initialize();

        #region IDemoEffect Members

        public abstract void Step();

        public abstract void Render();

        public void Initialize(IGraphicsFactory graphicsFactory, IEffectFactory effectFactory, 
            ITextureFactory textureFactory, IDemoMixer mixer, IPostProcessor postProcessor)
        {
            CameraNode camera;
            this.graphicsFactory = graphicsFactory;
            this.effectFactory = effectFactory;
            this.mixer = mixer;
            this.postProcessor = postProcessor;
            this.textureFactory = textureFactory;
            CreateStandardSceneAndCamera(out scene, out camera, 10);

            Initialize();
        }

        #endregion

    }
}
