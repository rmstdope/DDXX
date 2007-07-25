using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;

namespace Dope.DDXX.Graphics
{
    public abstract class ModelBase : IModel
    {
        private ModelMaterial[] materials;
        private Cull cullMode;
        private bool useStencil;
        private int stencilReference;

        protected ModelBase()
        {
            cullMode = Cull.CounterClockwise;
            useStencil = false;
            stencilReference = 1;
        }

        protected ModelMaterial[] CreateModelMaterials(ITextureFactory textureFactory, ExtendedMaterial[] extendedMaterials)
        {
            ModelMaterial[] modelMaterials = new ModelMaterial[extendedMaterials.Length];
            for (int i = 0; i < extendedMaterials.Length; i++)
            {
                if (extendedMaterials[i].TextureFilename == null ||
                    extendedMaterials[i].TextureFilename == "")
                    modelMaterials[i] = new ModelMaterial(extendedMaterials[i].Material3D);
                else
                    modelMaterials[i] = new ModelMaterial(extendedMaterials[i].Material3D, textureFactory.CreateFromFile(extendedMaterials[i].TextureFilename));
                modelMaterials[i].Ambient = modelMaterials[i].Diffuse;
            }
            return modelMaterials;
        }

        #region IModel Members

        public Cull CullMode
        {
            get { return cullMode; }
            set { cullMode = value; }
        }

        public bool UseStencil
        {
            get { return useStencil; }
            set { useStencil = value; }
        }

        public int StencilReference
        {
            get { return stencilReference; }
            set { stencilReference = value; }
        }

        public abstract IMesh Mesh
        { get; set; }

        public ModelMaterial[] Materials
        {
            get { return materials; }
            set { materials = value; }
        }

        public virtual bool IsSkinned()
        { 
            return false; 
        }

        public void Render(IDevice device, IEffectHandler effectHandler, ColorValue ambient, Matrix world, Matrix view, Matrix projection)
        {
            effectHandler.SetNodeConstants(world, view, projection);
            device.RenderState.CullMode = cullMode;
            device.RenderState.StencilEnable = useStencil;
            device.RenderState.ReferenceStencil = stencilReference;
            device.RenderState.StencilFunction = Compare.Equal;
            for (int j = 0; j < Materials.Length; j++)
            {
                HandleSkin(effectHandler, j);
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

        protected abstract void HandleSkin(IEffectHandler effectHandler, int materialNum);

        public abstract void Step();

        public abstract IModel Clone();

        #endregion
    }
}
