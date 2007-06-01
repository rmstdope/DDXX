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
using Dope.DDXX.Graphics.Skinning;
using Dope.DDXX.MeshBuilder;

namespace EngineTest
{
    public class DofCubesEffect : BaseDemoEffect
    {
        private IScene scene;
        private CameraNode camera;
        private ModelNode boxNode;
        private ITexture celTexture;

        public DofCubesEffect(float startTime, float endTime)
            : base(startTime, endTime)
        {
        }

        private Vector4 celMapCallback(Vector2 texCoord, Vector2 texelSize)
        {
            if (texCoord.X < 0.5f)
                return new Vector4(0.4f, 0.2f, 0.2f, 1.0f);
            else if (texCoord.X < 0.7f)
                return new Vector4(0.6f, 0.3f, 0.3f, 1.0f);
            else if (texCoord.X < 0.9f)
                return new Vector4(0.8f, 0.4f, 0.4f, 1.0f);
            return new Vector4(1, 1, 1, 1);
        }

        protected override void Initialize()
        {
            CreateStandardSceneAndCamera(out scene, out camera, 5.0f);
            celTexture = TextureFactory.CreateFromFunction(64, 1, 0, Usage.None, Format.A8R8G8B8, Pool.Managed, celMapCallback);

            MeshBuilder.CreateChamferBox("Box", 0.5f, 0.5f, 0.5f, 0.1f, 4);
            MeshBuilder.AssignMaterial("Box", "Default1");
            MeshBuilder.SetDiffuseTexture("Default1", "red glass.jpg");
            IModel model = MeshBuilder.CreateModel("Box");
            model.Materials[0].DiffuseColor = new ColorValue(0.6f, 0.6f, 0.6f);
            model.Materials[0].DiffuseTexture = celTexture;
            boxNode = new ModelNode("Box", model,
                new EffectHandler(EffectFactory.CreateFromFile("Test.fxo"),
                delegate(int material) { return "CelWithDoF"; }, model));
            scene.AddNode(boxNode);

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
            boxNode.WorldState.Roll(Time.DeltaTime);
            boxNode.WorldState.Turn(Time.DeltaTime);
            scene.Step();
        }

        public override void Render()
        {
            scene.Render();
        }
    }
}
