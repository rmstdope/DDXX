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
        private ISpriteBatch spriteBatch;
        private ITexture2D texture1;
        private ITexture2D texture2;
        private ITexture2D texture3;
        private ITexture2D texture4;
        private CameraNode camera;
        private IModel cylinder;
        private IModel plane;
        private ITexture2D lightTexture;
        private ITexture2D brickTexture;
        //private IModelNode node;

        public ComicEffect(string name, float startTime, float endTime)
            : base(name, startTime, endTime)
        {
        }

        protected override void Initialize()
        {
            CreateStandardCamera(out camera, 3);
            CreateLightTexture();
            CreateCylinder();
            CreatePlane();

            for (int y = 0; y < 10; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    IModelNode node = new ModelNode("Cylinder", cylinder, GraphicsDevice);
                    node.WorldState.Position = new Vector3(x - 5, 0.5f, y - 5);
                    Scene.AddNode(node);

                    node = new ModelNode("Plane", plane, GraphicsDevice);
                    node.WorldState.Position = new Vector3(x - 5, 1, y - 5);
                    Scene.AddNode(node);
                }
            }
            spriteBatch = GraphicsFactory.CreateSpriteBatch();
            texture1 = TextureFactory.CreateFromName("Circle");
            texture2 = TextureFactory.CreateFromName("Turbulence256");
            texture3 = TextureFactory.CreateFromName("OldMarble256");
            texture4 = TextureFactory.CreateFromName("NewMarble256");
        }

        private void CreateLightTexture()
        {
            lightTexture = TextureFactory.CreateFromName("LightMap");
            brickTexture = TextureFactory.CreateFromName("Square");
            //TextureDirector.CreateSquare(0.9f);
            //TextureDirector.CreateBrushNoise(3);
            //TextureDirector.ModulateColor(new Vector4(0.2f, 0.2f, 0.2f, 0.2f));
            //TextureDirector.Subtract();
            //brickTexture = TextureDirector.Generate("Square", 64, 64, 1, SurfaceFormat.Color);
        }

        private void CreatePlane()
        {
            ModelBuilder.CreateMaterial("LightMap");
            ModelBuilder.SetEffect("LightMap", "Content\\effects\\LightMap2");
            ModelBuilder.SetDiffuseTexture("LightMap", brickTexture);
            ModelBuilder.SetNormalTexture("LightMap", lightTexture);
            ModelDirector.CreatePlane(1.0f, 1.0f, 1, 1);
            ModelDirector.Rotate(MathHelper.PiOver2, 0, 0);
            plane = ModelDirector.Generate("LightMap");
        }

        private void CreateCylinder()
        {
            ModelBuilder.CreateMaterial("Reflective");
            ModelBuilder.SetEffect("Reflective", "Content\\effects\\LightMap");
            ModelBuilder.SetDiffuseTexture("Reflective", lightTexture);
            ModelDirector.CreateCylinder(0.05f, 8, 1.0f, 1, false, 1, 1);
            cylinder = ModelDirector.Generate("Reflective");
        }

        public override void Step()
        {
            camera.WorldState.Reset();
            camera.WorldState.Turn(Time.CurrentTime / 2);
            camera.WorldState.MoveBackward(8);
            camera.WorldState.MoveUp(0.4f);
            //node.WorldState.Turn(Time.DeltaTime);
            //node.WorldState.Tilt(0.32f * Time.DeltaTime);
            //node.WorldState.Roll(0.86f * Time.DeltaTime);
            Scene.Step();
        }

        public override void Render()
        {
            Scene.Render();
            spriteBatch.Begin(SpriteBlendMode.None);
            spriteBatch.Draw(texture1, new Rectangle(0, 0, 256, 256), Color.White);
            spriteBatch.Draw(texture2, new Rectangle(256, 0, 256, 256), Color.White);
            spriteBatch.Draw(texture3, new Rectangle(0, 256, 256, 256), Color.White);
            spriteBatch.Draw(texture4, new Rectangle(256, 256, 256, 256), Color.White);
            spriteBatch.End();
        }
    }
}
