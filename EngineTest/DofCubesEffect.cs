using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.Graphics;
using Dope.DDXX.Physics;
using Dope.DDXX.SceneGraph;
using Dope.DDXX.Utility;
using Dope.DDXX.ParticleSystems;
using Dope.DDXX.MeshBuilder;

namespace EngineTest
{
    public class DofCubesEffect : BaseDemoEffect
    {
        private const int NUM_LIGHTS = 2;
        private IScene scene;
        private CameraNode camera;
        private ModelNode boxNode = null;
        private ITexture celTexture;
        private List<DirectionalLightNode> lights = new List<DirectionalLightNode>();
        private IEffect boxEffect = null;

        public DofCubesEffect(string name, float startTime, float endTime)
            : base(name, startTime, endTime)
        {
        }

        private Vector4 celMapCallback(Vector2 texCoord, Vector2 texelSize)
        {
            if (texCoord.X < 0.3f)
                return new Vector4(0.0f, 0.0f, 0.0f, 1.0f);
            else if (texCoord.X < 0.6f)
                return new Vector4(0.0f, 0.0f, 0.2f, 1.0f);
            //else if (texCoord.X < 0.9f)
                return new Vector4(0.1f, 0.1f, 0.4f, 1.0f);
            //return new Vector4(0.1f, 0.3f, 0.4f, 1);
        }

        protected override void Initialize()
        {
            CreateStandardSceneAndCamera(out scene, out camera, 5.0f);
            celTexture = TextureFactory.CreateFromFunction(64, 1, 0, Usage.None, Format.A8R8G8B8, Pool.Managed, celMapCallback);

            //MeshBuilder.CreateChamferBox("Box", 0.40001f, 0.40001f, 0.40001f, 0.2f, 12);
            //MeshBuilder.AssignMaterial("Box", "Default1");
            //MeshBuilder.SetDiffuseTexture("Default1", "red glass.jpg");
            //IModel model = MeshBuilder.CreateModel("Box");
            //model.Materials[0].AmbientColor = new ColorValue(0.1f, 0.1f, 0.6f);
            //model.Materials[0].DiffuseTexture = celTexture;
            //boxEffect = EffectFactory.CreateFromFile("Test.fxo");
            //boxNode = new ModelNode("Box", model,
            //    new EffectHandler(boxEffect,
            //    delegate(int material) { return "CelWithDoF"; }, model));
            scene.AddNode(boxNode);

            for (int i = 0; i < NUM_LIGHTS; i++)
            {
                lights.Add(new DirectionalLightNode("Light" + i));
                scene.AddNode(lights[i]);
                float x = i - 0.5f;
                float y = Rand.Float(-1, 1);
                float z = Rand.Float(-1, 0);
                lights[i].Direction =new Vector3(x, y, z);
            }
            scene.Validate();
        }

        public override void StartTimeUpdated()
        {
        }

        public override void EndTimeUpdated()
        {
        }

        public override void Step()
        {
            Vector3 dir = new Vector3();
            float phi = Time.StepTime;
            float theta = Time.StepTime * 2.123f;
            dir.X = (float)(Math.Cos(theta) * Math.Sin(phi));
            dir.Y = (float)(Math.Sin(theta) * Math.Sin(phi));
            dir.Z = (float)(Math.Cos(phi));

            boxNode.WorldState.Roll(Time.DeltaTime * 1.2f);
            boxNode.WorldState.Turn(Time.DeltaTime);
            boxNode.WorldState.Position =
                new Vector3((float)Math.Sin(Time.StepTime),
                0.0f, (float)Math.Cos(Time.StepTime));

            scene.Step();
        }

        public override void Render()
        {
            float add = (float)(Math.Sin(Time.CurrentTime) + 1) / 2;
            boxEffect.SetValue(EffectHandle.FromString("ChamferAdd"), 
                add);
            float scale = add + 0.2f;
            scale = 0.2f / scale;
            boxNode.WorldState.Scaling = new Vector3(scale, scale, scale);
            scene.Render();
        }
    }
}
