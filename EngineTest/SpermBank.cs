using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.SceneGraph;
using Microsoft.Xna.Framework.Graphics;
using Dope.DDXX.Utility;
using Microsoft.Xna.Framework;
using Dope.DDXX.Graphics;
using Dope.DDXX.ParticleSystems;

namespace EngineTest
{
    public class SpermBank : BaseDemoEffect
    {
        private const float TailLength = 6.0f;
        private CameraNode camera;
        private List<ModelNode> sperms = new List<ModelNode>();

        public SpermBank(string name, float start, float end)
            : base(name, start, end)
        {
        }

        protected override void Initialize()
        {
            CreateStandardCamera(out camera, 6);
            InitializeSperms();
            Scene.AmbientColor = new Color(30, 30, 30);
        }

        private void InitializeSperms()
        {
            TextureDirector.CreatePerlinNoise(1, 6, 0.5f);
            ModelBuilder.SetDiffuseTexture("Default", TextureDirector.Generate("Noise64", 64, 64, 0, SurfaceFormat.Color));
            //ModelBuilder.SetDiffuseTexture("Default", TextureFactory.CreateFromFile("Content\\textures\\CARPTBLU"));
            ModelBuilder.SetAmbientColor("Default", Color.Red);
            ModelBuilder.SetDiffuseColor("Default", Color.Red);
            ModelBuilder.SetSpecularColor("Default", Color.White);
            ModelBuilder.SetShininess("Default", 0.5f);
            ModelBuilder.SetSpecularPower("Default", 8);
            ModelBuilder.SetEffect("Default", "Content\\effects\\BloodCell");
            ModelDirector.CreateSphere(1, 32);
            ModelDirector.Scale(1.8f, 1, 1);
            IModel model = ModelDirector.Generate("Default");
            for (int i = 0; i < 1; i++)
            {
                ModelNode head = new ModelNode("Cell", model, GraphicsDevice);
                ModelDirector.CreateCylinder(0.1f, 8, TailLength, 16, true, 1, 1);
                ModelDirector.Rotate(0, 0, Math.PI / 2);
                ModelDirector.Translate(-TailLength / 2 - 1.6f, 0, 0);
                ModelDirector.Amplitude(tailFunction);
                model = ModelDirector.Generate("Default");
                ModelNode tail = new ModelNode("Cell", model, GraphicsDevice);
                head.AddChild(tail);
                head.WorldState.Position = new Vector3(0, 0, -3);// Rand.Vector3(-3, 3);
                sperms.Add(head);
                Scene.AddNode(head);
            }
        }

        private Vector3 tailFunction(Vector3 pos)
        {
            float delta = 1 - ((pos.X + 1.6f + TailLength) / TailLength);
            //System.Diagnostics.Debug.WriteLine(delta);
            float f = (float)Math.Cos(Math.PI * 0.4f * delta);
            return new Vector3(1, f, f);
        }

        public override void Step()
        {
            Mixer.SetClearColor(0, Color.Coral);
            foreach (ModelNode cell in sperms)
            {
                //cell.WorldState.Turn(Time.DeltaTime * 0.8f);
                //cell.WorldState.Tilt(Time.DeltaTime * 0.94f);
                cell.WorldState.Position += new Vector3(Time.DeltaTime * 1.4f, 0, 0);
                if (cell.WorldState.Position.X > 8)
                    cell.WorldState.Position -= new Vector3(16, 0, 0);
            }
            Scene.Step();
        }

        public override void Render()
        {
            Scene.Render();
        }
    }
}
