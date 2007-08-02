using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.SceneGraph;
using Dope.DDXX.MeshBuilder;
using Dope.DDXX.Graphics;
using Microsoft.DirectX.Direct3D;

namespace TiVi
{
    public class ChessBoard
    {
        private const int NUM_SIGNS_X = 8;
        private const int NUM_SIGNS_Y = 8;
        private ModelNode[] signs = new ModelNode[NUM_SIGNS_X * NUM_SIGNS_Y];
        private ModelNode boardStencilNode;
        private DummyNode rootNode;

        public ChessBoard(IScene scene, MeshBuilder meshBuilder, IEffect effect, IDevice device, float scale, float reflective)
        {
            rootNode = new DummyNode("Root");
            meshBuilder.SetDiffuseTexture("Default1", "Square.tga");
            meshBuilder.SetReflectiveTexture("Default1", "rnl_cross.dds");
            meshBuilder.SetReflectiveFactor("Default1", reflective);
            MeshDirector meshDirector = new MeshDirector(meshBuilder);
            meshDirector.CreatePlane(1, 1, 1, 1, true);
            meshDirector.Rotate((float)Math.PI / 2, 0, 0);
            float c = 0;
            for (int y = 0; y < NUM_SIGNS_Y; y++)
            {
                for (int x = 0; x < NUM_SIGNS_X; x++)
                {
                    IModel model = meshDirector.Generate("Default1");
                    ModelNode node = new ModelNode("Board" + x + "_" + y, model, new EffectHandler(effect, TechniqueChooser.MaterialPrefix("TiViChessBoard"), model), device);
                    node.WorldState.MoveForward(-1.0f * (y - NUM_SIGNS_Y / 2));
                    node.WorldState.MoveRight(-1.0f * (x - NUM_SIGNS_X / 2));
                    float color = 0.0f + c * 0.5f;
                    node.Model.Materials[0].AmbientColor = new ColorValue(0.02f, 0.02f, 0.02f, 0.02f);
                    node.Model.Materials[0].DiffuseColor = new ColorValue(color, color, color, color);
                    if (c == 0)
                        node.Model.Materials[0].ReflectiveFactor = reflective;//0.7f;
                    else
                        node.Model.Materials[0].ReflectiveFactor = reflective / 7.0f;// 0.1f;
                    c = 1 - c;
                    signs[y * NUM_SIGNS_X + x] = node;
                    rootNode.AddChild(node);
                }
                c = 1 - c;
            }
            CreateStencilObject(meshBuilder, effect, device, scale);
            rootNode.WorldState.Scale(scale);
            scene.AddNode(rootNode);
        }

        private void CreateStencilObject(MeshBuilder meshBuilder, IEffect effect, IDevice device, float scale)
        {
            MeshDirector director = new MeshDirector(meshBuilder);
            director.CreatePlane(8, 8, 1, 1, false);
            director.Rotate((float)Math.PI / 2, 0, 0);
            director.Translate(0.5f, 0, 0.5f);
            director.Scale(scale);
            IModel model = director.Generate("Default1");
            boardStencilNode = new ModelNode("StencilNode", model, new EffectHandler(effect, TechniqueChooser.MaterialPrefix("StencilOnly"), model), device);
        }

        public void Render(IScene scene)
        {
            boardStencilNode.Render(scene);
        }
    }
}
