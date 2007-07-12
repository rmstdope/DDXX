using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.SceneGraph;
using Dope.DDXX.MeshBuilder;
using Dope.DDXX.Graphics;
using Dope.DDXX.Utility;
using Dope.DDXX.TextureBuilder;
using Microsoft.DirectX.Direct3D;
using System.Drawing;
using Dope.DDXX.ParticleSystems;
using Microsoft.DirectX;

namespace TiVi
{
    public class TestEffect : BaseDemoEffect
    {
        private ISprite sprite;
        private ITexture texture;
        //private ParticleSystemNode system;
        private IScene scene;
        private CameraNode camera;
        private ModelNode node;

        private int baseFrequency = 16;
        private int numOctaves = 6;
        private float persistance = 0.5f;

        public float Persistance
        {
            get { return persistance; }
            set { persistance = value; GeneratePerlinNoise(); }
        }
        public int NumOctaves
        {
            get { return numOctaves; }
            set { numOctaves = value; GeneratePerlinNoise(); }
        }
        public int BaseFrequency
        {
            get { return baseFrequency; }
            set { baseFrequency = value; GeneratePerlinNoise(); }
        }

        public TestEffect(string name, float startTime, float endTime)
            : base(name, startTime, endTime)
        {
            SetStepSize(GetTweakableNumber("Persistance"), 0.01f);
        }

        protected override void Initialize()
        {
            CreateStandardSceneAndCamera(out scene, out camera, 40);
            sprite = GraphicsFactory.CreateSprite(Device);

            MeshBuilder.SetDiffuseTexture("Default1", "square.tga");
            MeshDirector meshDirector = new MeshDirector(MeshBuilder);
            meshDirector.CreatePlane(40, 40, 1, 1, true);
            meshDirector.UvRemap(0, 1f, 0, 1f);
            IModel model = meshDirector.Generate("Default1");
            node = CreateSimpleModelNode(model, "TiVi.fxo", "Atmosphere");
            scene.AddNode(node);

            GraphicsStream stream = ShaderLoader.CompileShaderFromFile("Imaginations.psh", "CreateCloudTexture", null, "tx_1_0", ShaderFlags.None);
            ITexture tex = GraphicsFactory.CreateTexture(Device, 256, 256, 1, Usage.None, Format.A8R8G8B8, Pool.Managed);
            TextureLoader.FillTexture((Texture)((tex as TextureAdapter).BaseTextureDX), new TextureShader(stream));
            tex.Save("cloud.dds", ImageFileFormat.Dds);
            //node.Model.Materials[0].DiffuseTexture = tex;

            GeneratePerlinNoise();

            //system = new ParticleSystemNode("");
            //CloudParticleSpawner spawner = new CloudParticleSpawner(GraphicsFactory, Device, 10);
            //system.Initialize(spawner, Device, GraphicsFactory, EffectFactory, texture);
            //scene.AddNode(system);
        }

        private void GeneratePerlinNoise()
        {
            if (node != null)
            {
                TextureDirector textureDirector = new TextureDirector(TextureBuilder);
                //textureDirector.CreateCircle(0.2f, 0.5f);
                textureDirector.CreatePerlinNoise(baseFrequency, numOctaves, persistance);
                textureDirector.Madd(1.0f, -0.3f);
                //textureDirector.Madd(3.2f, 0.0f);
                //textureDirector.Modulate();
                texture = textureDirector.Generate(256, 256, 1, Format.A8R8G8B8);
                node.Model.Materials[0].DiffuseTexture = texture;
            }
        }

        public override void Step()
        {
            float[] f = new float[] { 
                Time.StepTime * 0.025f, 
                Time.StepTime * 0.031f, 
                Time.StepTime * 0.014f, 
                Time.StepTime * 0.027f 
            };
            EffectFactory.CreateFromFile("TiVi.fxo").SetValue(EffectHandle.FromString("AtmosphereTime"), f);
            //camera.WorldState.MoveForward(Time.DeltaTime * 3.0f);
            scene.Step();
        }

        public override void Render()
        {
            scene.Render();

            sprite.Begin(SpriteFlags.None);
            sprite.Draw2D(texture, Rectangle.Empty, SizeF.Empty, new PointF(512, 0), Color.White);
            sprite.End();
        }
    }
}
