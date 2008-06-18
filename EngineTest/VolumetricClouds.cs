using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.Graphics;
using Dope.DDXX.SceneGraph;
using Microsoft.Xna.Framework.Graphics;
using Dope.DDXX.Utility;

namespace EngineTest
{
    public class VolumetricClouds : BaseDemoEffect
    {
        private ModelNode volumeCube;
        private IEffect frontEffect;
        private IEffect backEffect;
        private IRenderTarget2D renderTarget;

        public VolumetricClouds(string name, float start, float end)
            : base(name, start, end)
        {
        }

        protected override void Initialize()
        {
            frontEffect = EffectFactory.CreateFromFile("Content\\effects\\VolumetricRayCastingFront");
            backEffect = EffectFactory.CreateFromFile("Content\\effects\\VolumetricRayCastingBack");

            CameraNode camera;
            CreateStandardCamera(out camera, 5);

            renderTarget = GraphicsFactory.CreateRenderTarget2D(BackbufferWidth, BackbufferHeight, 1, SurfaceFormat.Color, MultiSampleType.None, 0);

            ModelDirector.CreateBox(1, 1, 1);
            //ModelDirector.Translate(0.5f, 0.5f, 0.5f);
            ModelBuilder.SetEffect("Default", "Content\\effects\\VolumetricRayCastingBack");
            IModel model = ModelDirector.Generate("Default");
            volumeCube = new ModelNode("Volume cube", model, GraphicsDevice);
            Scene.AddNode(volumeCube);
        }

        public override void Step()
        {
            volumeCube.Model.Meshes[0].MeshParts[0].Effect = backEffect;
            volumeCube.CullMode = CullMode.CullClockwiseFace;
            volumeCube.WorldState.Turn(Time.DeltaTime);
            volumeCube.WorldState.Roll(Time.DeltaTime / 2.345f);

            IRenderTarget2D originalRenderTarget = GraphicsDevice.GetRenderTarget(0) as IRenderTarget2D;
            GraphicsDevice.SetRenderTarget(0, renderTarget);
            Scene.Render();
            GraphicsDevice.SetRenderTarget(0, originalRenderTarget);
        }

        public override void Render()
        {
            volumeCube.Model.Meshes[0].MeshParts[0].Effect = frontEffect;
            volumeCube.Model.Meshes[0].MeshParts[0].MaterialHandler.DiffuseTexture = renderTarget.GetTexture();
            volumeCube.CullMode = CullMode.CullCounterClockwiseFace;
            Scene.Render();
        }
    }
}
