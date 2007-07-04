using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.MeshBuilder;
using Dope.DDXX.SceneGraph;
using Microsoft.DirectX.Direct3D;
using Dope.DDXX.Graphics;

namespace TiVi
{
    public class TiViMeshDirector
    {
        private MeshDirector director;
        private MeshBuilder builder;
        private IEffectFactory effectFactory;
        private IDevice device;

        void Initialize(MeshBuilder builder, MeshDirector director, IEffectFactory effectFactory, IDevice device)
        {
            this.builder = builder;
            this.director = director;
            this.effectFactory = effectFactory;
            this.device = device;
        }

        ModelNode CreateDiamondNode()
        {
            builder.SetAmbientColor("Default1", new ColorValue(0.3f, 0.3f, 0.3f, 1.0f));
            builder.SetDiffuseColor("Default1", new ColorValue(0.3f, 0.3f, 0.3f, 1.0f));
            builder.SetDiffuseTexture("Default1", "square.tga");
            director.CreateChamferBox(1, 1, 0.4f, 0.2f, 4);
            director.UvMapPlane(1, 1, 1);
            director.Rotate((float)Math.PI / 2, 0, 0);
            director.Rotate(0, 0, (float)Math.PI / 4);
            director.Scale(0.2f, 0.3f, 0.3f);
            IModel model = director.Generate("Default1");
            model.Mesh.ComputeNormals();
            EffectHandler handler = new EffectHandler(effectFactory.CreateFromFile("TiVi.fxo"),
                delegate(int material) { return "Diamond"; }, model);
            ModelNode node = new ModelNode("", model, handler, device);
            return node;
        }
    }
}
