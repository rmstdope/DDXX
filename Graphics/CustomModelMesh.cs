using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.Graphics
{
    public class CustomModelMesh : IModelMesh
    {
        private ReadOnlyCollection<IModelMeshPart> parts;
        private IGraphicsDevice device;
        private IVertexBuffer vertexBuffer;
        private IIndexBuffer indexBuffer;
        private int vertexSizeInBytes;
        private IVertexDeclaration vertexDeclaration;
        private PrimitiveType primitiveType;

        public CustomModelMesh(IGraphicsDevice device, IVertexBuffer vertexBuffer,
            IIndexBuffer indexBuffer, int vertexSizeInBytes, IVertexDeclaration vertexDeclaration,
            PrimitiveType primitiveType, IModelMeshPart[] partArray)
        {
            List<IModelMeshPart> list = new List<IModelMeshPart>(partArray);
            parts = new ReadOnlyCollection<IModelMeshPart>(list);
            this.device = device;
            this.vertexBuffer = vertexBuffer;
            this.indexBuffer = indexBuffer;
            this.vertexSizeInBytes = vertexSizeInBytes;
            this.vertexDeclaration = vertexDeclaration;
            this.primitiveType = primitiveType;
        }

        public BoundingSphere BoundingSphere
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public ModelEffectCollectionAdapter Effects
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public IIndexBuffer IndexBuffer
        {
            get { return indexBuffer; }
        }

        public ReadOnlyCollection<IModelMeshPart> MeshParts
        {
            get { return parts; }
        }

        public string Name
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public ModelBone ParentBone
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public object Tag
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        public IVertexBuffer VertexBuffer
        {
            get { return vertexBuffer; }
        }

        public void Draw()
        {
            device.Indices = indexBuffer;
            device.Vertices[0].SetSource(vertexBuffer, 0, vertexSizeInBytes);
            device.VertexDeclaration = vertexDeclaration;
            foreach (IModelMeshPart part in parts)
            {
                part.Effect.Begin(SaveStateMode.SaveState);
                foreach (IEffectPass pass in part.Effect.CurrentTechnique.Passes)
                {
                    pass.Begin();
                    if (indexBuffer == null)
                        device.DrawPrimitives(primitiveType, part.BaseVertex, part.PrimitiveCount);
                    else
                        device.DrawIndexedPrimitives(primitiveType, part.BaseVertex, 0, 
                            part.NumVertices, part.StartIndex, part.PrimitiveCount);
                    pass.End();
                }
                part.Effect.End();
            }
        }

        public void Draw(SaveStateMode saveStateMode)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
