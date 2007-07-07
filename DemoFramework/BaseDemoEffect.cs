using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;
using Dope.DDXX.SceneGraph;
using Dope.DDXX.Physics;
using Dope.DDXX.MeshBuilder;
using Dope.DDXX.TextureBuilder;
using Microsoft.DirectX.Direct3D;

namespace Dope.DDXX.DemoFramework
{
    public abstract class BaseDemoEffect : TweakableContainer, IDemoEffect
    {
        private int drawOrder;
        private float startTime;
        private float endTime;
        private IGraphicsFactory graphicsFactory;
        private IEffectFactory effectFactory;
        private IDevice device;
        private IXLoader xLoader;
        private MeshBuilder.MeshBuilder meshBuilder;
        private TextureBuilder.TextureBuilder textureBuilder;
        private IDemoMixer mixer;

        protected BaseDemoEffect(float startTime, float endTime)
        {
            StartTime = startTime;
            EndTime = endTime;
            drawOrder = 0;
        }

        protected IDevice Device
        {
            get { return device; }
        }

        protected IGraphicsFactory GraphicsFactory
        {
            get { return graphicsFactory; }
        }

        protected IEffectFactory EffectFactory
        {
            get { return effectFactory; }
        }

        protected IModelFactory ModelFactory
        {
            get { return D3DDriver.ModelFactory; }
        }

        protected ITextureFactory TextureFactory
        {
            get { return D3DDriver.TextureFactory; }
        }

        protected IXLoader XLoader
        {
            get { return xLoader; }
        }

        protected IDemoMixer Mixer
        {
            get { return mixer; }
        }

        protected MeshBuilder.MeshBuilder MeshBuilder
        {
            // Lazy creation
            get
            {
                if (meshBuilder == null)
                    meshBuilder = new MeshBuilder.MeshBuilder(GraphicsFactory, TextureFactory, Device);
                return meshBuilder;
            }
        }

        protected TextureBuilder.TextureBuilder TextureBuilder
        {
            // Lazy creation
            get
            {
                if (textureBuilder == null)
                    textureBuilder = new TextureBuilder.TextureBuilder(TextureFactory);
                return textureBuilder;
            }
        }

        public int DrawOrder
        {
            set { drawOrder = value; }
            get { return drawOrder; }
        }

        protected void CreateStandardSceneAndCamera(out IScene scene, out CameraNode camera, float distance)
        {
            scene = new Scene(EffectFactory);
            scene.AmbientColor = new ColorValue(1.0f, 1.0f, 1.0f);
            camera = new CameraNode("Standard Camera");
            camera.WorldState.MoveForward(-distance);
            scene.AddNode(camera);
            scene.ActiveCamera = camera;
        }

        protected ModelNode CreateSimpleModelNode(IModel model, string effectFileName, string techniqueName)
        {
            IEffectHandler effectHandler = new EffectHandler(EffectFactory.CreateFromFile(effectFileName),
                delegate(int material) { return techniqueName; }, model);
            return new ModelNode("", model, effectHandler, Device);
        }

        protected abstract void Initialize();

        #region IDemoEffect Members

        public float StartTime
        {
            get { return startTime; }
            set { startTime = value; StartTimeUpdated();  }
        }

        public float EndTime
        {
            get { return endTime; }
            set { endTime = value; EndTimeUpdated();  }
        }

        public abstract void Step();

        public abstract void Render();

        public virtual void StartTimeUpdated()
        {
        }

        public virtual void EndTimeUpdated()
        {
        }

        public void Initialize(IGraphicsFactory graphicsFactory, IEffectFactory effectFactory, IDevice device, IDemoMixer mixer)
        {
            this.graphicsFactory = graphicsFactory;
            this.effectFactory = effectFactory;
            this.device = device;
            this.mixer = mixer;

            xLoader = new XLoader(GraphicsFactory, new NodeFactory(Device, TextureFactory), Device);
            
            Initialize();
        }

        #endregion

    }
}
