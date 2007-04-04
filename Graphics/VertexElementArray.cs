using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Microsoft.DirectX.Direct3D;
using Dope.DDXX.Utility;

namespace Dope.DDXX.Graphics
{
    public class VertexElementArray
    {
        VertexElement[] vertexElements;

        public VertexElement[] VertexElements
        {
            get { return vertexElements; }
            set { vertexElements = value; }
        }

        public VertexElementArray()
        {
            vertexElements = new VertexElement[1] { VertexElement.VertexDeclarationEnd };
        }

        public VertexElementArray(VertexElement[] elements)
        {
            vertexElements = elements;
            for (int i = 0; i < vertexElements.Length; i++)
            {
                if (vertexElements[i].DeclarationType == DeclarationType.Unused)
                {
                    Array.Resize(ref vertexElements, i + 1);
                    break;
                }
            }
        }

        public void AddPositions()
        {
            AddElement(DeclarationType.Float3, DeclarationUsage.Position, 0);
        }

        public void AddTransformedPositions()
        {
            AddElement(DeclarationType.Float4, DeclarationUsage.PositionTransformed, 0);
        }

        public void AddNormals()
        {
            AddElement(DeclarationType.Float3, DeclarationUsage.Normal, 0);
        }

        public void AddTangents()
        {
            if (!HasTexCoords(0))
                throw new DDXXException("Tangents can not be added to vertex declaration if no texture coordinates (0) exists.");
            AddElement(DeclarationType.Float3, DeclarationUsage.Tangent, 0);
        }

        public void AddBiNormals()
        {
            if (!HasTangents())
                throw new DDXXException("Tangents can not be added to vertex declaration if no texture coordinates (0) exists.");
            AddElement(DeclarationType.Float3, DeclarationUsage.BiNormal, 0);
        }

        private bool HasTangents()
        {
            for (int i = 0; i < vertexElements.Length; i++)
            {
                if (vertexElements[i].DeclarationUsage == DeclarationUsage.Tangent)
                    return true;
            }
            return false;
        }

        public bool HasTexCoords(int index)
        {
            for (int i = 0; i < vertexElements.Length; i++)
            {
                if (vertexElements[i].DeclarationUsage == DeclarationUsage.TextureCoordinate &&
                    vertexElements[i].UsageIndex == index)
                    return true;
            }
            return false;
        }

        public void AddTexCoords(byte index, byte numFloats)
        {
            switch (numFloats)
            {
                case 1:
                    AddElement(DeclarationType.Float1, DeclarationUsage.TextureCoordinate, index);
                    break;
                case 2:
                    AddElement(DeclarationType.Float2, DeclarationUsage.TextureCoordinate, index);
                    break;
                case 3:
                    AddElement(DeclarationType.Float3, DeclarationUsage.TextureCoordinate, index);
                    break;
                case 4:
                    AddElement(DeclarationType.Float4, DeclarationUsage.TextureCoordinate, index);
                    break;
                default:
                    throw new DDXXException("numFloats must be between one and four. It is " + numFloats);
            }
        }

        public void AddElement(DeclarationType type, DeclarationUsage usage, byte index)
        {
            Array.Resize(ref vertexElements, vertexElements.Length + 1);
            vertexElements[vertexElements.Length - 1] = new VertexElement(0, 0, type, DeclarationMethod.Default, usage, index);
            Array.Sort(vertexElements, delegate(VertexElement e1, VertexElement e2) 
            {
                if (e1.DeclarationType == DeclarationType.Unused)
                    return 1;
                if (e2.DeclarationType == DeclarationType.Unused)
                    return -1;
                if (e1.DeclarationUsage < e2.DeclarationUsage)
                    return -1;
                else if (e1.DeclarationUsage > e2.DeclarationUsage)
                    return 1;
                else
                {
                    if (e1.UsageIndex < e2.UsageIndex)
                        return -1;
                    if (e1.UsageIndex > e2.UsageIndex)
                        return 1;
                    return 0;
                }
            });
            SetOffsets();
        }

        public void SetOffsets()
        {
            short offset = 0;
            for (int i = 0; i < vertexElements.Length - 1; i++)
            {
                vertexElements[i].Offset = offset;
                switch (vertexElements[i].DeclarationType)
                {
                    case DeclarationType.Float1:
                        offset += 4;
                        break;
                    case DeclarationType.Float2:
                        offset += 8;
                        break;
                    case DeclarationType.Float3:
                        offset += 12;
                        break;
                    case DeclarationType.Float4:
                        offset += 16;
                        break;
                    case DeclarationType.Color:
                        offset += 4;
                        break;
                    default:
                        throw new DDXXException("DeclarationType not supported! Add it!");
                }
            }
        }

    }
}
