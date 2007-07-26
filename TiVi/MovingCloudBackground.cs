using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.Graphics;
using Microsoft.DirectX.Direct3D;
using Dope.DDXX.TextureBuilder;
using Dope.DDXX.Utility;
using System.Drawing;
using Dope.DDXX.SceneGraph;
using Dope.DDXX.MeshBuilder;

namespace TiVi
{
    public class MovingCloudBackground : BaseDemoEffect
    {
        private IScene scene;
        private CameraNode camera;
        private ModelNode node;
        private ITexture texture;

        private Color skyColor;
        private Color cloudColor1;
        private Color cloudColor2;
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
        public Color SkyColor
        {
            get { return skyColor; }
            set { skyColor = value; SetEffectParametrers(); }
        }
        public Color CloudColor1
        {
            get { return cloudColor1; }
            set { cloudColor1 = value; SetEffectParametrers(); }
        }
        public Color CloudColor2
        {
            get { return cloudColor2; }
            set { cloudColor2 = value; SetEffectParametrers(); }
        }

        public MovingCloudBackground(string name, float startTime, float endTime)
            : base(name, startTime, endTime)
        {
            SetStepSize(GetTweakableNumber("Persistance"), 0.01f);
            skyColor = Color.FromArgb(50, 50, 100);
            cloudColor1 = Color.FromArgb(206, 206, 156);
            cloudColor2 = Color.FromArgb(206, 206, 156);
        }

        protected override void Initialize()
        {
            CreateStandardSceneAndCamera(out scene, out camera, Device.Viewport.Height / 2);
            camera.SetFOV((float)Math.PI / 2);

            MeshBuilder.SetDiffuseTexture("Default1", "square.tga");
            MeshDirector meshDirector = new MeshDirector(MeshBuilder);
            meshDirector.CreatePlane(Device.Viewport.Width, Device.Viewport.Height, 1, 1, true);
            meshDirector.UvRemap(0, 1f, 0, 1f);
            IModel model = meshDirector.Generate("Default1");
            node = CreateSimpleModelNode(model, "TiVi.fxo", "MovingCloudBackground");
            scene.AddNode(node);

            GeneratePerlinNoise();

        }

        private void SetEffectParametrers()
        {
            if (texture != null)
            {
                ColorValue v = ColorValue.FromColor(skyColor);
                EffectFactory.CreateFromFile("TiVi.fxo").SetValue(EffectHandle.FromString("AtmosphereSkyColor"),
                    new float[] { v.Red, v.Green, v.Blue, v.Alpha });
                v = ColorValue.FromColor(cloudColor1);
                EffectFactory.CreateFromFile("TiVi.fxo").SetValue(EffectHandle.FromString("AtmosphereCloudColor1"),
                    new float[] { v.Red, v.Green, v.Blue, v.Alpha });
                v = ColorValue.FromColor(cloudColor2);
                EffectFactory.CreateFromFile("TiVi.fxo").SetValue(EffectHandle.FromString("AtmosphereCloudColor2"),
                    new float[] { v.Red, v.Green, v.Blue, v.Alpha });
            }
        }

        private void GeneratePerlinNoise()
        {
            if (node != null)
            {
                TextureDirector textureDirector = new TextureDirector(TextureBuilder);
                textureDirector.CreatePerlinNoise(baseFrequency, numOctaves, persistance);
                textureDirector.Madd(1.0f, -0.4f);
                texture = textureDirector.Generate(256, 256, 1, Format.A8R8G8B8);
                node.Model.Materials[0].DiffuseTexture = texture;
            }
        }

        public override void Step()
        {
            float[] f = new float[] { 
                Time.StepTime * 0.035f, 
                Time.StepTime * 0.041f, 
                Time.StepTime * 0.024f, 
                Time.StepTime * 0.037f 
            };
            EffectFactory.CreateFromFile("TiVi.fxo").SetValue(EffectHandle.FromString("AtmosphereTime"), f);
            scene.Step();
        }

        public override void Render()
        {
            scene.Render();
        }
    }
}
