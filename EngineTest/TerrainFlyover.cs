using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.TextureBuilder;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.ModelBuilder;
using Dope.DDXX.SceneGraph;
using Dope.DDXX.Graphics;
using Dope.DDXX.Utility;
using Microsoft.Xna.Framework.Graphics;

namespace EngineTest
{
    public class TerrainFlyover : BaseDemoEffect
    {
        private ModelNode terrainModel = null;
        private CameraNode camera;

        public TerrainFlyover(string name, float startTime, float endTime)
            : base(name, startTime, endTime)
        {
        }

        protected override void Initialize()
        {
            CreateStandardCamera(out camera, -3);

            PerlinNoise generator = new PerlinNoise();
            generator.NumOctaves = 10;
            generator.BaseFrequency = 8;
            generator.Persistence = 0.5f;
            ModelDirector.CreateTerrain(generator, 10.0f, 50.0f, 50.0f, 50, 50, true);
            ModelBuilder.CreateMaterial("Terrain");
            ModelBuilder.SetDiffuseTexture("Terrain", "OldMarble256");
            terrainModel = new ModelNode("Terrain", ModelDirector.Generate("Terrain"), GraphicsDevice);
            Scene.AddNode(terrainModel);
            terrainModel.WorldState.MoveUp(-7);
            terrainModel.WorldState.MoveForward(30);
            terrainModel.DepthStencilState = DepthStencilState.Default;
        }

        public override void Step()
        {
            terrainModel.WorldState.Turn(Time.DeltaTime / 2);
            Scene.Step();
        }

        public override void Render()
        {
            Scene.Render();
        }
    }
}
