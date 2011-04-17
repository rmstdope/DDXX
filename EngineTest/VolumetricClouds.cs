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
        private Effect frontEffect;
        private Effect backEffect;
        private RenderTarget2D renderTarget;

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

            renderTarget = new RenderTarget2D(GraphicsDevice, BackbufferWidth, BackbufferHeight, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.DiscardContents);

            ModelDirector.CreateBox(1, 1, 1);
            //ModelDirector.Translate(0.5f, 0.5f, 0.5f);
            ModelBuilder.SetEffect("Default", "Content\\effects\\VolumetricRayCastingBack");
            CustomModel model = ModelDirector.Generate("Default");
            volumeCube = new ModelNode("Volume cube", model, GraphicsDevice);
            Scene.AddNode(volumeCube);
        }

        public override void Step()
        {
            volumeCube.Model.Meshes[0].MeshParts[0].MaterialHandler.Effect = backEffect;
            volumeCube.RasterizerState = RasterizerState.CullClockwise;
            volumeCube.WorldState.Turn(Time.DeltaTime);
            volumeCube.WorldState.Roll(Time.DeltaTime / 2.345f);

            RenderTargetBinding[] originalRenderTarget = GraphicsDevice.GetRenderTargets();
            GraphicsDevice.SetRenderTarget(renderTarget);
            Scene.Render();
            if (originalRenderTarget.Length >= 1)
            {
                GraphicsDevice.SetRenderTarget(originalRenderTarget[0].RenderTarget as RenderTarget2D);
            }
            else
            {
                GraphicsDevice.SetRenderTarget(null);
            }
        }

        public override void Render()
        {
            volumeCube.Model.Meshes[0].MeshParts[0].MaterialHandler.Effect = frontEffect;
            volumeCube.Model.Meshes[0].MeshParts[0].MaterialHandler.DiffuseTexture = renderTarget;
            volumeCube.RasterizerState = RasterizerState.CullCounterClockwise;
            Scene.Render();
        }
    }
}
