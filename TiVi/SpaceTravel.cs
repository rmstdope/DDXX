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

namespace EngineTest
{
    public class SpaceTravel : BaseDemoEffect
    {
        private List<ParticleSystemNode> spiralSystems = new List<ParticleSystemNode>();
        private CameraNode camera;
        private IScene scene;

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
            CreateStandardSceneAndCamera(out scene, out camera, 10);
            camera.SetClippingPlanes(1, 10000);

            CreateSpiralSystems();

            scene.Validate();
        }

        private void CreateSpiralSystems()
        {
            ITexture circleTexture = TextureFactory.CreateFromFunction(64, 64, 0, Usage.None, Format.A8R8G8B8, Pool.Managed, circleCallback);
            spiralSystems.Add(CreateSpiralSystem("1", circleTexture, Color.FromArgb(100, 70, 70), -5));
            spiralSystems.Add(CreateSpiralSystem("2", circleTexture, Color.FromArgb(20, 20, 50), -5));
            spiralSystems.Add(CreateSpiralSystem("3", circleTexture, Color.FromArgb(100, 70, 70), 5));
            spiralSystems.Add(CreateSpiralSystem("4", circleTexture, Color.FromArgb(20, 20, 50), 5));
            spiralSystems[2].WorldState.Scaling = new Vector3(-1, -1, -1);
            spiralSystems[3].WorldState.Scaling = new Vector3(-1, -1, -1);
        }

        private ParticleSystemNode CreateSpiralSystem(string name, ITexture circleTexture, Color color, float startTime)
        {
            ParticleSystemNode system = new ParticleSystemNode(name);
            SpiralParticleSpawner spawner = new SpiralParticleSpawner(GraphicsFactory, Device, 50000);
            spawner.Color = color;
            spawner.ColorDistortion = 10;
            spawner.NextTime = startTime;
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
            foreach (ParticleSystemNode system in spiralSystems)
                system.WorldState.Turn(-Time.DeltaTime * deltaMul);

        }

        private void StepCamera()
        {
            float t = Time.StepTime / 4;
            camera.Position = new Vector3((float)Math.Sin(t), (float)Math.Cos(t), (float)Math.Cos(t)) * 500;
            camera.LookAt(new Vector3(0, 0, 0), new Vector3(0, 1, 0));
        }

        public override void Render()
        {
            scene.Render();
        }

    }
}
