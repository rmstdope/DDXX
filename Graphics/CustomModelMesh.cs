using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.Graphics
{
    public class CustomModelMesh
    {
        private ReadOnlyCollection<CustomModelMeshPart> parts;
        private GraphicsDevice graphicsDevice;

        public CustomModelMesh(GraphicsDevice graphicsDevice, CustomModelMeshPart[] partArray)
        {
            List<CustomModelMeshPart> list = new List<CustomModelMeshPart>(partArray);
            parts = new ReadOnlyCollection<CustomModelMeshPart>(list);
            this.graphicsDevice = graphicsDevice;
        }

        //public BoundingSphere BoundingSphere { get; }
        //public ModelEffectCollection Effects { get; }
        public ReadOnlyCollection<CustomModelMeshPart> MeshParts
        {
            get { return parts; }
        }
        //public string Name { get; }
        //public ModelBone ParentBone { get; }
        //public object Tag { get; set; }

        public void Draw()
        {
            foreach (CustomModelMeshPart part in parts)
            {
                graphicsDevice.Indices = part.IndexBuffer;
                graphicsDevice.SetVertexBuffer(part.VertexBuffer);
                foreach (EffectPass pass in part.MaterialHandler.Effect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    if (part.IndexBuffer == null)
                        graphicsDevice.DrawPrimitives(part.PrimitiveType, part.VertexOffset, part.PrimitiveCount);
                    else
                        graphicsDevice.DrawIndexedPrimitives(part.PrimitiveType, part.VertexOffset, 0, 
                            part.NumVertices, part.StartIndex, part.PrimitiveCount);
                }
            }
        }
    }
}
