using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.TextureBuilder;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.MeshBuilder;
using Dope.DDXX.SceneGraph;
using Dope.DDXX.Graphics;
using Microsoft.DirectX.Direct3D;
using Dope.DDXX.Utility;

namespace EngineTest
{
    public class TerrainFlyover : BaseDemoEffect
    {
        private ModelNode terrainModel = null;
        private CameraNode camera;
        private IScene scene;

        public TerrainFlyover(string name, float startTime, float endTime)
            : base(name, startTime, endTime)
        {
        }

        protected override void Initialize()
        {
            CreateStandardSceneAndCamera(out scene, out camera, -3);

            //PerlinNoise generator = new PerlinNoise();
            //generator.NumOctaves = 10;
            //generator.BaseFrequency = 8;
            //generator.Persistence = 0.5f;
            //MeshBuilder builder = new MeshBuilder(GraphicsFactory, TextureFactory, Device);
            //builder.CreateTerrain("Terrain", generator, 10.0f, 50.0f, 50.0f, 50, 50, true);
            //builder.AssignMaterial("Terrain", "Default1");
            //builder.SetDiffuseTexture("Default1", "square.tga");
            //IModel model = builder.CreateModel("Terrain");
            //model.Mesh.ComputeNormals();
            //model.Materials[0].AmbientColor = new ColorValue(0.1f, 0.1f, 0.1f);
            //model.Materials[0].DiffuseColor = new ColorValue(0.6f, 0.6f, 0.6f);
            //terrainModel = new ModelNode("Terrain", model,
            //    new EffectHandler(EffectFactory.CreateFromFile("Test.fxo"),
            //    delegate(int material) { return "Terrain"; }, model));
            //scene.AddNode(terrainModel);
            //terrainModel.WorldState.MoveUp(-7);
            //terrainModel.WorldState.MoveForward(30);
        }

        public override void Step()
        {
            terrainModel.WorldState.Turn(Time.DeltaTime / 2);
            scene.Step();
        }

        public override void Render()
        {
            scene.Render();
        }
    }
}
