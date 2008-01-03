using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.TextureBuilder;
using Microsoft.Xna.Framework.Graphics;
using Dope.DDXX.Graphics;
using Dope.DDXX.SceneGraph;
using Microsoft.Xna.Framework;
using Dope.DDXX.Utility;
using Dope.DDXX.ModelBuilder;

namespace DFM2007Invitro
{
    public class GridEffect : BaseDemoEffect
    {
        private IScene scene;
        private CameraNode camera;
        //private DummyNode gridNode;
        //private ModelNode terrain;
        private List<ModelNode> tubes = new List<ModelNode>();

        public GridEffect(string name, float startTime, float endTime)
            : base(name, startTime, endTime)
        {
        }

        protected override void Initialize()
        {
            CreateStandardSceneAndCamera(out scene, out camera, 50);

            List<VertexPositionColor> list = new List<VertexPositionColor>();
            for (int i = -5; i < 6; i++)
            {
                list.Add(new VertexPositionColor(new Vector3(-5, i, 0), new Color(200, 200, 200)));
                list.Add(new VertexPositionColor(new Vector3(5, i, 0), new Color(200, 200, 200)));
                list.Add(new VertexPositionColor(new Vector3(i, -5, 0), new Color(200, 200, 200)));
                list.Add(new VertexPositionColor(new Vector3(i, 5, 0), new Color(200, 200, 200)));
            }

            IVertexBuffer vertexBuffer = GraphicsFactory.CreateVertexBuffer(typeof(VertexPositionColor), list.Count, BufferUsage.None);
            vertexBuffer.SetData(list.ToArray());

            CustomModelMeshPart part = new CustomModelMeshPart(EffectFactory.CreateFromFile("Content\\effects\\ColorLine"), 0, 0, 0, list.Count / 2);
            CustomModelMesh mesh = new CustomModelMesh(GraphicsDevice, vertexBuffer, null, VertexPositionColor.SizeInBytes, GraphicsFactory.CreateVertexDeclaration(VertexPositionColor.VertexElements), PrimitiveType.LineList, new IModelMeshPart[] { part });
            CustomModel model = new CustomModel(mesh);

            //ModelNode node;
            //gridNode = new DummyNode("Grids");

            //node = new ModelNode("", model, GraphicsDevice);
            //node.WorldState.Turn(Math.PI / 2);
            //node.WorldState.MoveForward(6);
            //gridNode.AddChild(node);

            //node = new ModelNode("", model, GraphicsDevice);
            //node.WorldState.Turn(-Math.PI / 2);
            //node.WorldState.MoveForward(6);
            //gridNode.AddChild(node);

            //node = new ModelNode("", model, GraphicsDevice);
            //node.WorldState.MoveForward(6);
            //gridNode.AddChild(node);

            //scene.AddNode(gridNode);

            TextureDirector textureDirector = new TextureDirector(TextureBuilder, TextureFactory);
            textureDirector.CreatePerlinNoise(4, 6, 0.5f);

            ModelBuilder.SetDiffuseTexture("Default", textureDirector.Generate(64, 64, 0, SurfaceFormat.Color));
            ModelBuilder.SetDiffuseColor("Default", new Color(250, 250, 250));
            ModelBuilder.SetAmbientColor("Default", new Color(50, 50, 50));
            ModelBuilder.SetEffect("Default", "Content\\effects\\Cylinders");
            ModelDirector director = new ModelDirector(ModelBuilder);

            const float radius = 0.8f;
            const float distance = radius * 1.6f;
            float r = 10.0f;
            for (int i = 0; i < 7; i++)
            {
                director.CreateTorus(radius / 2, r, 8, 64);
                //director.CreateTube(r, r + radius, 1.0f, 90, 1);
                director.Rotate(Math.PI / 2, 0, 0);
                IModel tubeModel = director.Generate("Default");
                ModelNode tube = new ModelNode("x", tubeModel, GraphicsDevice);
                if (tubes.Count == 0)
                    scene.AddNode(tube);
                else
                    tubes[tubes.Count - 1].AddChild(tube);
                tubes.Add(tube);
                r -= distance;
            }
            tubes[0].WorldState.MoveDelta(new Vector3(-17, 10, 0));

            // Add Light
            PointLightNode light = new PointLightNode("");
            light.WorldState.MoveDelta(new Vector3(-17, 10, 0));
            scene.AddNode(light);

            //ModelBuilder.SetEffect("Default", "Content\\effects\\Terrain");
            //PerlinNoise noise = new PerlinNoise();
            //noise.BaseFrequency = 8;
            //director.CreateTerrain(noise, 40, 200, 200, 30, 30, true);
            //terrain = new ModelNode("Terrain", director.Generate("Default"), GraphicsDevice);
            //terrain.WorldState.MoveDown(25);
            //scene.AddNode(terrain);
        }

        public override void Step()
        {
            bool turn = false;
            for (int i = 1; i < tubes.Count; i++)
            {
                if (turn)
                    tubes[i].WorldState.Turn(Time.DeltaTime / (0.7f - i * 0.02f) );
                else
                    tubes[i].WorldState.Tilt(Time.DeltaTime / (0.7f - i * 0.02f));
                turn = !turn;
            }
            scene.Step();
        }

        public override void Render()
        {
            scene.Render();
        }
    }
}
