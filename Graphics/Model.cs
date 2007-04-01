using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Dope.DDXX.Utility;

namespace Dope.DDXX.Graphics
{
    public class Model : ModelBase
    {
        private IMesh mesh;

        public override IMesh Mesh
        {
            get { return mesh; }
        }

        public Model(IMesh mesh)
        {
            this.mesh = mesh;
            Materials = new ModelMaterial[] { new ModelMaterial(new Material()) };
        }

        public Model(IMesh mesh, ModelMaterial[] materials)
        {
            this.mesh = mesh;
            Materials = materials;
        }

        public Model(IMesh mesh, ITextureFactory textureFactory, ExtendedMaterial[] extendedMaterials)
        {
            this.mesh = mesh;
            Materials = CreateModelMaterials(textureFactory, extendedMaterials);
        }

        public override void Draw(IEffectHandler effectHandler, ColorValue ambient, Matrix world, Matrix view, Matrix projection)
        {
            effectHandler.SetNodeConstants(world, view, projection);
            for (int j = 0; j < Materials.Length; j++)
            {
                effectHandler.SetMaterialConstants(ambient, Materials[j], j);
                int passes = effectHandler.Effect.Begin(FX.None);
                for (int i = 0; i < passes; i++)
                {
                    effectHandler.Effect.BeginPass(i);
                    Mesh.DrawSubset(j);
                    effectHandler.Effect.EndPass();
                }
                effectHandler.Effect.End();
            }
        }

        public override void Step() { }
    }
}