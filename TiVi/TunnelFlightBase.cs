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

        protected Interpolator<InterpolatedVector3> discInterpolator;
        protected Interpolator<InterpolatedVector3> cameraInterpolator;
        protected Interpolator<InterpolatedVector3> cameraTargetInterpolator;
        protected Interpolator<InterpolatedVector3> cameraUpInterpolator;
        private List<PointLightNode> lights = new List<PointLightNode>();

        public TunnelFlightBase(string name, float startTime, float endTime)
            : base(name, startTime, endTime)
        {
        }

        protected override void Initialize()
        {
            director = new MeshDirector(MeshBuilder);

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
            ((tiviNode as ModelNode).Model as SkinnedModel).SetAnimationSet(0, StartTime, 1);
            //tiviNode.WorldState.Scale(0.7f);
            mirrorNode = new MirrorNode(tiviNode);
            mirrorNode.Brightness = 0.4f;
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
        }

        private ModelNode CreateDisc(float outerRadius, float innerRadius, string technique)
        {
            MeshBuilder.SetDiffuseTexture("Default2", "noise");
            director.CreateDisc(outerRadius, innerRadius, 32);
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
            tiviNode.Position = discModel2.Position + new Vector3(0, 0, 0);
            stencilDisc.Position = discModel2.Position;
            mirrorNode.Position = new Vector3(0, tiviNode.Position.Y * 2, 0);

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
