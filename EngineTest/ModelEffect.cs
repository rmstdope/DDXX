using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.SceneGraph;
using Dope.DDXX.Graphics;
using Dope.DDXX.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Dope.DDXX.ModelBuilder;
using Dope.DDXX.TextureBuilder;

namespace EngineTest
{
    public class ModelEffect : BaseDemoEffect
    {
        private Scene scene;
        private CustomModel model;
        private CameraNode camera;
        private ModelNode node;
        private MirrorNode mirror;
        private ModelNode genNode;

        public ModelEffect(string name, float start, float end)
            : base(name, start, end)
        {
        }

        protected override void Initialize()
        {
            scene = new Scene();
            scene.AmbientColor = new Color(180,180,180);

            TextureDirector textureDirector = new TextureDirector(TextureFactory);
            textureDirector.FromFile("Content\\textures\\BENEDETI2");
            textureDirector.Madd(0.7f, 0);
            textureDirector.NormalMap();
            textureDirector.Madd(1, 1);
            textureDirector.Madd(0.5f, 0);
            Texture2D normalMap = textureDirector.Generate("NormalBENEDETI2", 256, 256, false, SurfaceFormat.Color);

            model = ModelFactory.CreateFromName("Content/models/ChamferBox2", "Content/effects/NormalMapping");
            model.Meshes[0].MeshParts[0].MaterialHandler.AmbientColor = Color.Chocolate;
            model.Meshes[0].MeshParts[0].MaterialHandler.DiffuseColor = Color.Chocolate;
            model.Meshes[0].MeshParts[0].MaterialHandler.SpecularColor = Color.White;
            model.Meshes[0].MeshParts[0].MaterialHandler.SpecularPower = 16;
            model.Meshes[0].MeshParts[0].MaterialHandler.Shininess = 1.5f;
            model.Meshes[0].MeshParts[0].MaterialHandler.NormalTexture = normalMap;

            node = new ModelNode("Test Model", model, GraphicsDevice);
            node.WorldState.MoveUp(15);
            scene.AddNode(node);
            mirror = new MirrorNode(node);
            mirror.Brightness = 0.5f;
            scene.AddNode(mirror);


            ModelDirector modelDirector = new ModelDirector(ModelBuilder);
            //ModelBuilder.GetMaterial("Default").Effect = EffectFactory.CreateFromFile("Content\\effects\\NormalMapping");
            ModelBuilder.SetDiffuseTexture("Default", "Content\\textures\\BENEDETI2");
            ModelBuilder.SetNormalTexture("Default", normalMap);
            ModelBuilder.SetAmbientColor("Default", Color.Chocolate);
            ModelBuilder.SetDiffuseColor("Default", Color.Chocolate);
            ModelBuilder.SetSpecularColor("Default", Color.White);
            ModelBuilder.SetSpecularPower("Default", 16);
            ModelBuilder.SetShininess("Default", 1.5f);
            ModelBuilder.SetEffect("Default", "Content\\effects\\NormalMapping");
            modelDirector.CreateChamferBox(10, 10, 10, 1, 4);
            modelDirector.UvMapSphere();
            CustomModel genModel = modelDirector.Generate("Default");

            genNode = new ModelNode("Generated", genModel, GraphicsDevice);
            scene.AddNode(genNode);
            // Create lights
            //DirectionalLightNode light = new DirectionalLightNode("x");
            //light.DiffuseColor = Color.Blue;
            //light.SpecularColor = Color.LightGray;
            //light.Direction = new Vector3(1, -1, -1);
            //scene.AddNode(light);

            PointLightNode light2 = new PointLightNode("x");
            light2.DiffuseColor = Color.CornflowerBlue;
            light2.Position = new Vector3(20, 0, 20);
            scene.AddNode(light2);

            // Create camera
            camera = new CameraNode("Test Camera", 
                (float)GraphicsDevice.PresentationParameters.BackBufferWidth /
                (float)GraphicsDevice.PresentationParameters.BackBufferHeight);
            camera.WorldState.MoveBackward(60);
            scene.AddNode(camera);
            scene.ActiveCamera = camera;
        }


        public override void Step()
        {
            //Mixer.ClearColor = Color.AntiqueWhite;

            node.WorldState.ResetRotation();
            node.WorldState.Roll(Time.CurrentTime * 0.7f);
            node.WorldState.Turn(Time.CurrentTime * 1.2f);

            genNode.WorldState.ResetRotation();
            genNode.WorldState.Roll(Time.CurrentTime * 0.7f);
            genNode.WorldState.Turn(Time.CurrentTime * 1.2f);

            scene.Step();
        }

        public override void Render()
        {
            scene.Render();
        }
    }
}
