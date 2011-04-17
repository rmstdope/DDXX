using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.TextureBuilder;
using Microsoft.Xna.Framework.Graphics;
using Dope.DDXX.Graphics;
using Dope.DDXX.SceneGraph;
using Microsoft.Xna.Framework;
using Dope.DDXX.Utility;
using Dope.DDXX.ModelBuilder;

namespace DFM2007Invitro
{
    public class CubeSystem : BaseDemoEffect
    {
        private CameraNode camera;
        private NaturalCubicSpline<InterpolatedVector3> spline = 
            new NaturalCubicSpline<InterpolatedVector3>();
        private PointLightNode light;
        //private FloaterSystemNode floaterSystem;

        public CubeSystem(string name, float startTime, float endTime)
            : base(name, startTime, endTime)
        {
        }

        protected override void Initialize()
        {
            CreateStandardCamera(out camera, 5);

            spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(0.0f, new InterpolatedVector3(new Vector3(0, 1.02f, 0))));
            spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(4.0f, new InterpolatedVector3(new Vector3(2, 1.04f, -1))));
            spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(7.0f, new InterpolatedVector3(new Vector3(-3, 0.0f, 1))));
            spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(10.0f, new InterpolatedVector3(new Vector3(0, 1.02f, 0))));
            spline.Calculate();

            ModelBuilder.SetEffect("Default", "Content\\effects\\NormalMapping");
            ModelBuilder.SetDiffuseTexture("Default", TextureFactory.CreateFromName("Noise256"));
            ModelBuilder.SetNormalTexture("Default", TextureFactory.CreateFromName("NormalNoise256"));
            ModelBuilder.SetDiffuseColor("Default", new Color(60, 90, 250));
            ModelBuilder.SetAmbientColor("Default", new Color(60, 90, 250));
            ModelBuilder.SetSpecularColor("Default", new Color(200, 200, 200));
            ModelBuilder.SetSpecularPower("Default", 32);
            ModelBuilder.SetShininess("Default", 1.0f);
            for (int z = -5; z < 6; z++)
            {
                for (int x = -5; x < 6; x++)
                {
                    ModelDirector.CreateChamferBox(1, 1, 1, 0.1f, 4);
                    ModelDirector.UvMapBox();
                    CustomModel chamferBoxModel = ModelDirector.Generate("Default");
                    ModelNode box = new ModelNode("Chamfer", chamferBoxModel, GraphicsDevice);
                    box.WorldState.MoveRight(x * 3f);
                    box.WorldState.MoveForward(z * 3f);
                    Scene.AddNode(box);
                }
            }

            // Add Light
            light = new PointLightNode("");
            light.WorldState.Position = new Vector3(0, 2, -5);
            Scene.AddNode(light);

            //floaterSystem = new FloaterSystemNode("FloaterSystem", 8, 0.02f, 6.0f);
            //floaterSystem.Initialize(GraphicsDevice, GraphicsFactory, 2000);
            //floaterSystem.Material.DiffuseTexture = TextureFactory.CreateFromName("Circle64");
            //Scene.AddNode(floaterSystem);

            //TextureDirector.CreateCircle(0.2f, 0.5f);
            //SpiralSystemNode spiralSystem = new SpiralSystemNode("ps", 0.05f);
            //spiralSystem.Initialize(GraphicsDevice, GraphicsFactory, 10000);
            //spiralSystem.Material.DiffuseTexture = TextureFactory.CreateFromName("Circle64");
            //spiralSystem.WorldState.MoveUp(1.5f);
            //spiralSystem.WorldState.MoveForward(1.5f);
            //Scene.AddNode(spiralSystem);
        }

        public override void Step()
        {
            //if (floaterSystem != null)
            //{
            //    floaterSystem.WorldState.Turn(Time.DeltaTime * 0.4054f);
            //    floaterSystem.WorldState.Roll(Time.DeltaTime * 0.1765f);
            //    floaterSystem.WorldState.Tilt(Time.DeltaTime * 0.2543f);
            //    floaterSystem.WorldState.Position = new Vector3(2 * (float)Math.Sin(Time.CurrentTime),
            //                                                    2 * (float)Math.Cos(Time.CurrentTime),
            //                                                    2 * (float)Math.Cos(Time.CurrentTime)) + camera.Position;
            //}
            camera.WorldState.Position = spline.GetValue(Time.CurrentTime % 10);
            light.WorldState.Position = camera.WorldState.Position;
            Scene.Step();
        }

        public override void Render()
        {
            Scene.Render();
        }
    }
}
