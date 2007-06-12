using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.SceneGraph;
using Dope.DDXX.Graphics;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;
using Dope.DDXX.Utility;
using Dope.DDXX.TextureBuilder;
using Dope.DDXX.MeshBuilder;

namespace TiVi
{
    public class TunnelFlight : BaseDemoEffect
    {
        private IScene scene;
        private CameraNode camera;
        private List<PointLightNode> lights = new List<PointLightNode>();
        private List<ModelNode> boxes = new List<ModelNode>();
        private ModelNode terrainModel;

        public TunnelFlight(float startTime, float endTime)
            : base(startTime, endTime)
        {
        }

        protected override void Initialize()
        {
            CreateStandardSceneAndCamera(out scene, out camera, 10);

            PerlinTurbulence noise = new PerlinTurbulence();
            noise.BaseFrequency = 30;
            Madd madd = new Madd(0.6f, 0.4f);
            madd.ConnectToInput(0, noise);
            TextureFactory.RegisterTexture("noise", TextureBuilder.Generate(madd, 128, 128, 1, Format.A8R8G8B8));

            CreateCircles();

            CreateLights();

            PerlinNoise generator = new PerlinNoise();
            generator.NumOctaves = 10;
            generator.BaseFrequency = 8;
            generator.Persistence = 0.5f;
            TerrainPrimitive terrain = new TerrainPrimitive();
            terrain.HeightMapGenerator = generator;
            terrain.HeightScale = 10.0f;
            terrain.Width = 50.0f;
            terrain.Height = 50.0f;
            terrain.HeightSegments = 25;
            terrain.WidthSegments = 25;
            terrain.Textured = true;
            MeshBuilder.SetDiffuseTexture("Default2", "BENEDETI.JPG");
            UvMapPlane uvMap = new UvMapPlane();
            uvMap.Input = terrain;
            uvMap.AlignToAxis = 1;
            uvMap.TileV = 4;
            uvMap.TileU = 4;
            IModel model = MeshBuilder.CreateModel(uvMap, "Default2");
            model.Mesh.ComputeNormals();
            model.Materials[0].AmbientColor = new ColorValue(0.1f, 0.1f, 0.1f);
            model.Materials[0].DiffuseColor = new ColorValue(0.6f, 0.6f, 0.6f);
            terrainModel = new ModelNode("Terrain", model,
                new EffectHandler(EffectFactory.CreateFromFile("TiVi.fxo"),
                delegate(int material) { return "Terrain"; }, model));
            scene.AddNode(terrainModel);
            terrainModel.WorldState.MoveUp(-8);
            terrainModel.WorldState.MoveForward(25);

            scene.AmbientColor = new ColorValue(0.4f, 0.4f, 0.4f, 0.4f);
        }

        private void CreateLights()
        {
            for (int i = 0; i < 2; i++)
            {
                PointLightNode light = new PointLightNode("");
                light.DiffuseColor = new ColorValue(0.3f + 0.7f * (1 - i), 0.3f + 0.7f * i, 1.0f, 1.0f);
                light.Position = new Vector3(0, 0, 0);
                scene.AddNode(light);
                lights.Add(light);
            }
        }

        private void CreateCircles()
        {
            ChamferBoxPrimitive primitive = new ChamferBoxPrimitive();
            primitive.Length = 2;
            primitive.Width = 1;
            primitive.Height = 0.2f;
            primitive.Fillet = 0.1f;
            primitive.FilletSegments = 8;
            UvMapPlane uvMap = new UvMapPlane();
            uvMap.Input = primitive;
            uvMap.AlignToAxis = 1;
            //uvMap.TileV = 2;

            MeshBuilder.SetDiffuseTexture("Default1", "square.tga");
            MeshBuilder.SetDiffuseColor("Default1", new ColorValue(0.8f, 0.8f, 0.8f, 0.8f));
            //MeshBuilder.SetAmbientColor("Default1", new ColorValue(0.5f, 0.5f, 0.5f, 0.8f));
            IModel model = MeshBuilder.CreateModel(uvMap, "Default1");
            EffectHandler effectHandler = new EffectHandler(EffectFactory.CreateFromFile("TiVi.fxo"),
                delegate(int material) { return "Terrain"; }, model);
            for (int j = 0; j < 8; j++)
            {
                for (int i = 0; i < 10; i++)
                {
                    ModelNode box = new ModelNode("Box", model, effectHandler);
                    scene.AddNode(box);
                    boxes.Add(box);
                }
            }
        }


        public override void Step()
        {
            for (int i = 0; i < 2; i++)
                lights[i].Position = new Vector3(
                    (float)Math.Sin(Time.StepTime * 0.5f + i) * 20,
                    (float)Math.Cos(Time.StepTime * 1.23f + i) * 10 + 10,
                    (float)Math.Sin(Time.StepTime + i) * 10 + 10);
            for (int j = 0; j < 8; j++)
            {
                for (int i = 0; i < 10; i++)
                {
                    boxes[j * 10 + i].WorldState.Reset();
                    boxes[j * 10 + i].WorldState.MoveForward(j * 2);
                    boxes[j * 10 + i].WorldState.Turn((float)Math.PI / 2);
                    //boxes[j * 10 + i].WorldState.Roll(Time.StepTime / 1); 
                    boxes[j * 10 + i].WorldState.Tilt(Time.StepTime / 2 + (float)Math.PI * 2 * i / 10);
                    boxes[j * 10 + i].WorldState.MoveUp(-4);
                }
            }
            scene.Step();
        }

        public override void Render()
        {
            scene.Render();
        }
    }
}
