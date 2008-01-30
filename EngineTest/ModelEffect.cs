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
        private IModel model;
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

            TextureDirector tDirector = new TextureDirector(TextureFactory);
            tDirector.FromFile("Content\\textures\\BENEDETI2");
            tDirector.Madd(0.7f, 0);
            tDirector.NormalMap();
            tDirector.Madd(1, 1);
            tDirector.Madd(0.5f, 0);
            ITexture2D normalMap = tDirector.Generate(256, 256, 1, SurfaceFormat.Color);

            model = ModelFactory.FromFile("Content/models/ChamferBox2", "Content/effects/NormalMapping");
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


            //IEffect effect = model.Meshes[0].MeshParts[0].Effect;
            //IEffect clone = effect.Clone(effect.GraphicsDevice);
            //effect.Parameters["Texture"].SetValue((ITexture2D)null);
            
            //model = ModelFactory.FromFile("Content/models/ChamferBox", "Content/effects/NormalMapping");
            //genNode = new ModelNode("Test Model", model, GraphicsDevice);
            //scene.AddNode(genNode);

            ModelDirector mDirector = new ModelDirector(ModelBuilder);
            //ModelBuilder.GetMaterial("Default").Effect = EffectFactory.CreateFromFile("Content\\effects\\NormalMapping");
            ModelBuilder.SetDiffuseTexture("Default", "Content\\textures\\BENEDETI2");
            ModelBuilder.SetNormalTexture("Default", normalMap);
            ModelBuilder.SetAmbientColor("Default", Color.Chocolate);
            ModelBuilder.SetDiffuseColor("Default", Color.Chocolate);
            ModelBuilder.SetSpecularColor("Default", Color.White);
            ModelBuilder.SetSpecularPower("Default", 16);
            ModelBuilder.SetShininess("Default", 1.5f);
            ModelBuilder.SetEffect("Default", "Content\\effects\\NormalMapping");
            mDirector.CreateChamferBox(10, 10, 10, 1, 4);
            mDirector.UvMapSphere();
            IModel genModel = mDirector.Generate("Default");

            genNode = new ModelNode("Generated", genModel, GraphicsDevice);
            scene.AddNode(genNode);
            // Create lights
            //DirectionalLightNode light = new DirectionalLightNode("x");
            //light.DiffuseColor = Color.Blue;
            //light.SpecularColor = Color.LightGray;
            //light.Direction = new Vector3(1, -1, -1);
            //scene.AddNode(light);

            PointLightNode light2 = new PointLightNode("x");
            light2.Color = Color.CornflowerBlue;
            light2.Position = new Vector3(20, 0, 20);
            scene.AddNode(light2);

            // Create camera
            camera = new CameraNode("Test Camera");
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
