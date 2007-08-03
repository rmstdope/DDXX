using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.SceneGraph;
using Dope.DDXX.MeshBuilder;
using Dope.DDXX.Graphics;
using Microsoft.DirectX.Direct3D;
using Dope.DDXX.Utility;
using Microsoft.DirectX;

namespace TiVi
{
    public abstract class TunnelFlightBase : BaseDemoEffect
    {
        protected struct Brick
        {
            public ModelNode Model;
            public float StartTime;
            public Brick(ModelNode model, float startTime)
            {
                Model = model;
                StartTime = startTime;
            }
        }

        protected struct TunnleRing
        {
            public DummyNode RingCenter;
            public float RotDirection;
        }

        private const int NUM_LIGHTS = 2;

        protected MeshDirector director;
        protected IScene scene;
        protected CameraNode camera;
        private ModelNode discModel;
        private ModelNode discModel2;
        protected ModelNode tiviNode;
        private ModelNode stencilDisc;
        private MirrorNode mirrorNode;
        private ModelNode whiteDisc1;
        private ModelNode whiteDisc2;
        private float timeScale;
        private float timeDelta;

        protected Interpolator<InterpolatedVector3> discInterpolator;
        protected Interpolator<InterpolatedVector3> cameraInterpolator;
        protected Interpolator<InterpolatedVector3> cameraTargetInterpolator;
        protected Interpolator<InterpolatedVector3> cameraUpInterpolator;
        private List<PointLightNode> lights = new List<PointLightNode>();

        public float TimeScale
        {
            get { return timeScale; }
            set 
            { 
                timeScale = value; 
                if (tiviNode != null)
                    ((tiviNode as ModelNode).Model as SkinnedModel).SetAnimationSet(0, StartTime + timeDelta, timeScale);
            }
        }

        public float TimeDelta
        {
            get { return timeDelta; }
            set 
            { 
                timeDelta = value;
                if (tiviNode != null)
                    ((tiviNode as ModelNode).Model as SkinnedModel).SetAnimationSet(0, StartTime + timeDelta, timeScale);
            }
        }

        public TunnelFlightBase(string name, float startTime, float endTime)
            : base(name, startTime, endTime)
        {
            timeScale = 1.348f;
            SetStepSize(GetTweakableNumber("TimeScale"), 0.001f);
            SetStepSize(GetTweakableNumber("TimeDelta"), 0.001f);
        }

        protected override void Initialize()
        {
            director = new MeshDirector(MeshBuilder);

            CreateStandardSceneAndCamera(out scene, out camera, 10);

            CreateDiscs();
            CreateTiVi();

            InitializeSpecific();

            CreateLights();

            Device.BeginScene();
            Render();
            Device.EndScene();
        }

        protected abstract void InitializeSpecific();

        public override void Step()
        {
            tiviNode.Step(scene.ActiveCamera);

            StepDiscs();
            StepLights();

            StepSpecific();
        }

        protected abstract void StepSpecific();

        public override void Render()
        {
            stencilDisc.Render(scene);
            mirrorNode.Render(scene);
            tiviNode.Render(scene);

            RenderSpecific();

            if (discModel != null)
            {
                discModel2.Render(scene);
                discModel.Render(scene);
            }
        }

        protected abstract void RenderSpecific();

        private void CreateLights()
        {
            for (int i = 0; i < NUM_LIGHTS; i++)
            {
                PointLightNode light = new PointLightNode("");
                light.DiffuseColor = new ColorValue(0.3f + 0.7f * (1 - i), 0.3f + 0.7f * i, 1.0f, 1.0f);
                light.Position = new Vector3(0, 0, 0);
                light.Range = 0.00001f;
                scene.AddNode(light);
                lights.Add(light);
            }
        }

        private void CreateTiVi()
        {
            XLoader.Load("Tivi-Dance.X", EffectFactory.CreateFromFile("TiVi.fxo"),
                delegate(string name)
                {
                    return delegate(int material)
                    {
                        if (material == 1)
                            return GetScreenTechnique();
                        else
                            return "TiViReflective";
                    };
                });
            tiviNode = XLoader.GetNodeHierarchy()[0].Children[1] as ModelNode;
            tiviNode.Model.Materials[0].ReflectiveTexture = TextureFactory.CreateCubeFromFile("rnl_cross.dds");
            tiviNode.Model.Materials[0].DiffuseTexture = TextureFactory.CreateFromFile("marble.jpg");
            tiviNode.Model.Materials[0].AmbientColor = new ColorValue(0.5f, 0.5f, 0.5f, 0.5f);
            tiviNode.Model.Materials[0].DiffuseColor = new ColorValue(0.5f, 0.5f, 0.5f, 0.5f);
            tiviNode.Model.Materials[0].ReflectiveFactor = 0.2f;
            ((tiviNode as ModelNode).Model as SkinnedModel).SetAnimationSet(0, StartTime + timeDelta, timeScale);
            //tiviNode.WorldState.Scale(0.7f);
            mirrorNode = new MirrorNode(tiviNode);
            mirrorNode.Brightness = 0.6f;
        }

        protected abstract string GetScreenTechnique();

        private void CreateDiscs()
        {
            const float outerRadius = 1.0f;
            const float innerRadius = 0.9f;
            const float torusRadius = 0.01f;
            stencilDisc = CreateDisc(outerRadius * 0.95f, 0, "StencilOnly");
            discModel = CreateDisc(outerRadius, innerRadius, "AlphaTest");
            discModel.AddChild(CreateTorus(outerRadius, torusRadius, "Terrain"));
            discModel.AddChild(CreateTorus(innerRadius, torusRadius, "Terrain"));
            discModel2 = CreateDisc(outerRadius * 0.95f, 0, "AlphaTest");
            discModel2.AddChild(CreateTorus(outerRadius * 0.95f, torusRadius, "Terrain"));

            whiteDisc1 = CreateDisc(outerRadius + torusRadius * 20, outerRadius + torusRadius * 5, "NoTextureAmbient");
            whiteDisc1.Model.CullMode = Cull.None;
            whiteDisc1.Model.Materials[0].AmbientColor = new ColorValue(0.8f, 0.8f, 1.0f);
            whiteDisc1.Model.Materials[0].DiffuseColor = new ColorValue(0.9f, 0.9f, 0.9f);
            //discModel2.AddChild(whiteDisc1);
            whiteDisc2 = CreateDisc(outerRadius + torusRadius * 20, outerRadius + torusRadius * 5, "NoTextureAmbient");
            whiteDisc2.Model.CullMode = Cull.None;
            whiteDisc2.Model.Materials[0].AmbientColor = new ColorValue(1.0f, 0.9f, 0.9f);
            //whiteDisc2.Model.Materials[0].DiffuseColor = new ColorValue(2.9f, 2.9f, 2.9f);
            //discModel2.AddChild(whiteDisc2);
            scene.AddNode(whiteDisc1);
            scene.AddNode(whiteDisc2);
        }

        private ModelNode CreateDisc(float outerRadius, float innerRadius, string technique)
        {
            MeshBuilder.SetDiffuseTexture("Default2", "noise");
            director.CreateDisc(outerRadius, innerRadius, 32);
            IModel model = director.Generate("Default2");
            model.Materials[0].AmbientColor = new ColorValue(0.2f, 0.2f, 0.2f);
            model.Materials[0].DiffuseColor = new ColorValue(0.6f, 0.6f, 0.6f);
            return new ModelNode("Terrain", model,
                new EffectHandler(EffectFactory.CreateFromFile("TiVi.fxo"),
                delegate(int material) { return technique; }, model), Device);
        }

        private ModelNode CreateDiscStretched(float outerRadius, float innerRadius, float stretch, string technique)
        {
            MeshBuilder.SetDiffuseTexture("Default2", "noise");
            director.CreateDisc(outerRadius, innerRadius, 32);
            director.Scale(1, 1, stretch);
            IModel model = director.Generate("Default2");
            model.Materials[0].AmbientColor = new ColorValue(0.1f, 0.1f, 0.1f);
            model.Materials[0].DiffuseColor = new ColorValue(0.5f, 0.5f, 0.5f);
            return new ModelNode("Terrain", model,
                new EffectHandler(EffectFactory.CreateFromFile("TiVi.fxo"),
                delegate(int material) { return technique; }, model), Device);
        }

        private ModelNode CreateTorus(float outerRadius, float torusRadius, string technique)
        {
            MeshBuilder.SetDiffuseTexture("Default2", "noise");
            director.CreateTorus(torusRadius, outerRadius, 32, 32);
            IModel model = director.Generate("Default2");
            model.Materials[0].AmbientColor = new ColorValue(0.1f, 0.1f, 0.1f);
            model.Materials[0].DiffuseColor = new ColorValue(0.9f, 0.9f, 0.9f);
            return new ModelNode("Torus", model,
                new EffectHandler(EffectFactory.CreateFromFile("TiVi.fxo"),
                delegate(int material) { return technique; }, model), Device);
        }

        private ModelNode CreateTorusStretched(float outerRadius, float torusRadius, float stretch, string technique)
        {
            MeshBuilder.SetDiffuseTexture("Default2", "noise");
            director.CreateTorus(torusRadius, outerRadius, 32, 32);
            director.Scale(1, 1, stretch);
            IModel model = director.Generate("Default2");
            model.Materials[0].AmbientColor = new ColorValue(0.1f, 0.1f, 0.1f);
            model.Materials[0].DiffuseColor = new ColorValue(0.9f, 0.9f, 0.9f);
            return new ModelNode("Torus", model,
                new EffectHandler(EffectFactory.CreateFromFile("TiVi.fxo"),
                delegate(int material) { return technique; }, model), Device);
        }

        private void StepDiscs()
        {
            float t = Time.StepTime - StartTime;
            if (discModel == null)
                return;
            discModel.WorldState.Reset();
            discModel2.WorldState.Reset();
            discModel.WorldState.Position = discInterpolator.GetValue(t);
            discModel2.WorldState.Position = discInterpolator.GetValue(t);
            discModel.WorldState.MoveUp(0.40f);
            tiviNode.Position = discModel2.Position + new Vector3(0, 0.04f, 0);
            stencilDisc.Position = discModel2.Position;
            mirrorNode.Position = new Vector3(0, (tiviNode.Position.Y - 0.04f) * 2, 0);
            whiteDisc1.WorldState.Reset();
            whiteDisc1.Position = discModel.Position;
            whiteDisc1.WorldState.Turn(t * 0.7f);
            whiteDisc1.WorldState.Tilt(t);
            whiteDisc1.WorldState.Scale(new Vector3(1, 1, 2.0f + 0.5f * (float)Math.Sin(t * 0.8f)));
            whiteDisc2.WorldState.Reset();
            whiteDisc2.Position = discModel.Position;
            whiteDisc2.WorldState.Turn(t * 0.8f + 2);
            whiteDisc2.WorldState.Tilt(t * 0.9f + 1.5f);
            whiteDisc2.WorldState.Scale(new Vector3(1, 1, 2.0f + 0.5f * (float)Math.Sin(t)));

            discModel.WorldState.Turn(t * 2);
            discModel.WorldState.Tilt(0.3f);
        }

        private void StepLights()
        {
            float t = Time.StepTime - StartTime;
            for (int i = 0; i < NUM_LIGHTS; i++)
            {
                lights[i].Position = discInterpolator.GetValue(t) + new Vector3(
                    (float)Math.Sin(t * 0.5f + i) * 2,
                    (float)Math.Cos(t * 1.23f + i) * 1 + 1,
                    (float)Math.Sin(t + i) * 2);
            }
        }

    }
}
