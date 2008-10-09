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

namespace EngineTest
{
    public class GridEffect : BaseDemoEffect
    {
        private CameraNode camera;
        private DummyNode gridNode;
        private ModelNode terrain;
        private List<ModelNode> tubes = new List<ModelNode>();

        public GridEffect(string name, float startTime, float endTime)
            : base(name, startTime, endTime)
        {
        }

        protected override void Initialize()
        {
            CreateStandardCamera(out camera, 40);

            List<VertexPositionColor> list = new List<VertexPositionColor>();
            for (int i = -5; i < 6; i++)
            {
                list.Add(new VertexPositionColor(new Vector3(-5, i, 0), new Color(200, 200, 200)));
                list.Add(new VertexPositionColor(new Vector3( 5, i, 0), new Color(200, 200, 200)));
                list.Add(new VertexPositionColor(new Vector3(i, -5, 0), new Color(200, 200, 200)));
                list.Add(new VertexPositionColor(new Vector3(i,  5, 0), new Color(200, 200, 200)));
            }

            IVertexBuffer vertexBuffer = GraphicsFactory.CreateVertexBuffer(typeof(VertexPositionColor), list.Count, BufferUsage.None);
            vertexBuffer.SetData(list.ToArray());

            CustomModelMeshPart part = new CustomModelMeshPart(new MaterialHandler(EffectFactory.CreateFromFile("Content\\effects\\ColorLine"), new EffectConverter()), 0, 0, 0, list.Count / 2);
            CustomModelMesh mesh = new CustomModelMesh(GraphicsDevice, vertexBuffer, null, VertexPositionColor.SizeInBytes, GraphicsFactory.CreateVertexDeclaration(VertexPositionColor.VertexElements), PrimitiveType.LineList, new IModelMeshPart[] { part });
            CustomModel model = new CustomModel(mesh);

            ModelNode node;
            gridNode = new DummyNode("Grids");
            
            node = new ModelNode("", model, GraphicsDevice);
            node.WorldState.Turn(Math.PI / 2);
            node.WorldState.MoveForward(6);
            gridNode.AddChild(node);

            node = new ModelNode("", model, GraphicsDevice);
            node.WorldState.Turn(-Math.PI / 2);
            node.WorldState.MoveForward(6);
            gridNode.AddChild(node);

            node = new ModelNode("", model, GraphicsDevice);
            node.WorldState.MoveForward(6);
            gridNode.AddChild(node);
            
            Scene.AddNode(gridNode);

            ModelBuilder.SetDiffuseTexture("Default", "Content\\textures\\BENEDETI2");
            ModelDirector director = new ModelDirector(ModelBuilder);

            float r = 4.0f;
            for (int i = 0; i < 4; i++)
            {
                director.CreateTube(r, r + 0.3f, 1.0f, 32, 1);
                director.Rotate(Math.PI / 2, 0, 0);
                IModel tubeModel = director.Generate("Default");
                ModelNode tube = new ModelNode("x", tubeModel, GraphicsDevice);
                if (tubes.Count == 0)
                    Scene.AddNode(tube);
                else
                    tubes[tubes.Count - 1].AddChild(tube);
                tubes.Add(tube);
                r -= 0.5f;
            }

            // Add Light
            PointLightNode light = new PointLightNode("");
            light.WorldState.MoveUp(5);
            Scene.AddNode(light);

            ModelBuilder.SetEffect("Default", "Content\\effects\\Terrain");
            PerlinNoise noise = new PerlinNoise();
            noise.BaseFrequency = 8;
            director.CreateTerrain(noise, 40, 200, 200, 30, 30, true);
            terrain = new ModelNode("Terrain", director.Generate("Default"), GraphicsDevice);
            terrain.WorldState.MoveDown(25);
            Scene.AddNode(terrain);
        }

        public override void Step()
        {
            bool turn = false;
            foreach (ModelNode tube in tubes)
            {
                if (turn)
                    tube.WorldState.Turn(Time.DeltaTime / 0.7f);
                else
                    tube.WorldState.Tilt(Time.DeltaTime / 0.7f);
                turn = !turn;
            }
            Scene.Step();
        }

        public override void Render()
        {
            Scene.Render();
        }
    }
}
