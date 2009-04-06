using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.SceneGraph;
using Dope.DDXX.Graphics;
using Dope.DDXX.Utility;
using Dope.DDXX.ModelBuilder;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TiVi
{
    public class ComicEffect : BaseDemoEffect
    {
        private CameraNode camera;
        private IModel cylinder;
        private ITexture2D lightTexture;
        private IModelNode node;

        public ComicEffect(string name, float startTime, float endTime)
            : base(name, startTime, endTime)
        {
        }

        protected override void Initialize()
        {
            CreateStandardCamera(out camera, 3);
            CreateLightTexture();
            CreateCylinder();

            node = new ModelNode("Cylinder", cylinder, GraphicsDevice);
            Scene.AddNode(node);
        }

        private void CreateLightTexture()
        {
            lightTexture = TextureFactory.CreateFromName("LightMap");
        }

        private void CreateCylinder()
        {
            ModelBuilder.CreateMaterial("SolidColor");
            ModelBuilder.SetDiffuseTexture("SolidColor", lightTexture);

            ModelBuilder.CreateMaterial("Reflective");
            ModelBuilder.SetEffect("Reflective", "Content\\effects\\LightMap");
            ModelBuilder.SetDiffuseTexture("Reflective", lightTexture);
            //ModelDirector.CreateSphere(1.0f, 8);//.CreateChamferBox(1, 1, 1, 0.1f, 5);
            ModelDirector.CreateCylinder(0.05f, 8, 1.0f, 1, false, 1, 1);
            cylinder = ModelDirector.Generate("Reflective");
        }

        public override void Step()
        {
            node.WorldState.Turn(Time.DeltaTime);
            node.WorldState.Tilt(0.32f * Time.DeltaTime);
            node.WorldState.Roll(0.86f * Time.DeltaTime);
            Scene.Step();
        }

        public override void Render()
        {
            Scene.Render();
        }
    }
}
