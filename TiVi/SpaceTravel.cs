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
using Dope.DDXX.TextureBuilder;

namespace TiVi
{
    public class SpaceTravel : BaseDemoEffect
    {
        private List<INode> spiralSystems = new List<INode>();
        private CameraNode camera;
        private IScene scene;
        private TiViMeshDirector tiviMeshDirector;
        private Interpolator<InterpolatedVector3> interpolator;

        public SpaceTravel(float startTime, float endTime)
            : base(startTime, endTime)
        {
        }

        private Vector4 circleCallback(Vector2 texCoord, Vector2 texelSize)
        {
            Vector2 centered = texCoord - new Vector2(0.5f, 0.5f);
            float distance = centered.Length();
            if (distance < 0.1f)
                return new Vector4(1, 1, 1, 1);
            else if (distance < 0.5f)
            {
                float scaled = (0.5f - distance) / 0.4f;
                return new Vector4(scaled, scaled, scaled, scaled);
            }
            return new Vector4(0, 0, 0, 0);
        }

        protected override void Initialize()
        {
            CreateStandardSceneAndCamera(out scene, out camera, 1000);
            camera.SetClippingPlanes(1, 10000);
            tiviMeshDirector = new TiViMeshDirector(MeshBuilder, new MeshDirector(MeshBuilder), EffectFactory, Device);

            CreateSpiralSystems();

            ClampedCubicSpline<InterpolatedVector3> spline = new ClampedCubicSpline<InterpolatedVector3>(new InterpolatedVector3(), new InterpolatedVector3());
            spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(10, new InterpolatedVector3(new Vector3(0, 0, 0))));
            spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(15, new InterpolatedVector3(new Vector3(0, 200, -200))));
            spline.Calculate();
            interpolator = new Interpolator<InterpolatedVector3>();
            interpolator.AddSpline(spline);

            scene.Validate();
        }

        private void CreateSpiralSystems()
        {
            ITexture circleTexture = TextureFactory.CreateFromFunction(64, 64, 0, Usage.None, Format.A8R8G8B8, Pool.Managed, circleCallback);
            spiralSystems.Add(CreateSpiralSystem("1", circleTexture, Color.FromArgb(100, 70, 70), -5));
            spiralSystems.Add(CreateSpiralSystem("2", circleTexture, Color.FromArgb(20, 20, 50), -5));
            spiralSystems.Add(new MirrorNode(spiralSystems[0]));// CreateSpiralSystem("3", circleTexture, Color.FromArgb(100, 70, 70), 5));
            spiralSystems.Add(new MirrorNode(spiralSystems[1]));// CreateSpiralSystem("4", circleTexture, Color.FromArgb(20, 20, 50), 5));
            scene.AddNode(spiralSystems[2]);
            scene.AddNode(spiralSystems[3]);
        }

        private ParticleSystemNode CreateSpiralSystem(string name, ITexture circleTexture, Color color, float startTime)
        {
            ParticleSystemNode system = new ParticleSystemNode(name);
            SpiralParticleSpawner spawner = new SpiralParticleSpawner(GraphicsFactory, Device, 50000);
            spawner.Color = color;
            spawner.ColorDistortion = 10;
            spawner.NextTime = startTime;
            spawner.VelocityY = 10;
            spawner.VelocityXZ = 30;
            spawner.PositionDistortion = 20;
            spawner.TimeBetweenSpawns = 0.004f;
            system.Initialize(spawner, Device, GraphicsFactory, EffectFactory, circleTexture);
            scene.AddNode(system);
            return system;
        }

        public override void Step()
        {
            StepCamera();

            StepSpiralSystems();

            scene.Step();
        }

        private void StepSpiralSystems()
        {
            float d = (Time.StepTime - StartTime) / (EndTime - StartTime);
            d = Math.Max(0, (d - 0.5f)) * 2;
            float deltaMul = 0.02f + (float)Math.Sin(Math.PI / 2 * d) * 0.5f;
            for (int i = 0; i <2; i++)
                if (spiralSystems.Count > i)
                    spiralSystems[i].WorldState.Turn(-Time.DeltaTime * deltaMul);

        }

        private void StepCamera()
        {
            float t = Time.StepTime / 4;
            camera.Position = new Vector3((float)Math.Sin(t), (float)Math.Cos(t), (float)Math.Cos(t)) * 1000;
            camera.LookAt(interpolator.GetValue(Time.StepTime - StartTime), new Vector3(0, 1, 0));

        }

        public override void Render()
        {
            scene.Render();
        }

    }
}
