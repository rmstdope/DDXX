using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.Graphics
{
    public struct VertexPositionTangentTexture
    {
        // Summary:
        //     The vertex position.
        public Vector3 Position;
        // Summary:
        //     The vertex normal.
        public Vector3 Normal;
        // Summary:
        //     The vertex tangent.
        public Vector3 Tangent;
        // Summary:
        //     The vertex binormal.
        public Vector3 Binormal;
        //
        // Summary:
        //     The texture coordinates.
        public Vector2 TextureCoordinate;
        //
        // Summary:
        //     An array of three vertex elements describing the position and normal
        //     of this vertex.
        public static readonly VertexElement[] VertexElements = new VertexElement[] 
        {
            new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
            new VertexElement(12, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0),
            new VertexElement(24, VertexElementFormat.Vector3, VertexElementUsage.Tangent, 0),
            new VertexElement(36, VertexElementFormat.Vector3, VertexElementUsage.Binormal, 0),
            new VertexElement(48, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0),
        };
        //
        // Summary:
        //     Initializes a new instance of the VertexPositionNormal class.
        //
        // Parameters:
        //   position:
        //     Position of the vertex.
        //
        //   normal:
        //     The vertex normal.
        public VertexPositionTangentTexture(Vector3 position, Vector3 normal, Vector3 tangent, Vector3 binormal, Vector2 textureCoordinate)
        {
            Position = position;
            Normal = normal;
            Tangent = tangent;
            Binormal = binormal;
            TextureCoordinate = textureCoordinate;
        }
        // Summary:
        //     Compares two objects to determine whether they are different.
        //
        // Parameters:
        //   left:
        //     Object to the left of the inequality operator.
        //
        //   right:
        //     Object to the right of the inequality operator.
        //
        // Returns:
        //     true if the objects are different; false otherwise.
        public static bool operator !=(VertexPositionTangentTexture left, VertexPositionTangentTexture right)
        {
            return left.Normal != right.Normal || left.Position != right.Position || left.TextureCoordinate != right.TextureCoordinate || left.Tangent != right.Tangent || left.Binormal != right.Binormal;
        }
        //
        // Summary:
        //     Compares two objects to determine whether they are the same.
        //
        // Parameters:
        //   left:
        //     Object to the left of the equality operator.
        //
        //   right:
        //     Object to the right of the equality operator.
        //
        // Returns:
        //     true if the objects are the same; false otherwise.
        public static bool operator ==(VertexPositionTangentTexture left, VertexPositionTangentTexture right)
        {
            return left.Normal == right.Normal && left.Position == right.Position && left.TextureCoordinate == right.TextureCoordinate && left.Tangent == right.Tangent && left.Binormal == right.Binormal;
        }
        // Summary:
        //     Gets the size of the VertexPositionNormal class.
        //
        // Returns:
        //     The size of the vertex, in bytes.
        public static int SizeInBytes { get { return (3 + 3 + 3 + 3 + 2) * sizeof(float); } }

        // Summary:
        //     Returns a value that indicates whether the current instance is equal to a
        //     specified object.
        //
        // Parameters:
        //   obj:
        //     The System.Object to compare with the current VertexPositionNormal.
        //
        // Returns:
        //     true if the objects are the same; false otherwise.
        public override bool Equals(object obj)
        {
            if (!(obj is VertexPositionTangentTexture))
                return false;
            return this == (VertexPositionTangentTexture)obj;
        }
        //
        // Summary:
        //     Gets the hash code for this instance.
        //
        // Returns:
        //     Hash code for this object.
        public override int GetHashCode()
        {
            return (int)Position.X;
        }
        //
        // Summary:
        //     Retrieves a string representation of this object.
        //
        // Returns:
        //     String representation of this object.
        public override string ToString()
        {
            return "Position: " + Position + ", Normal: " + Normal + ", Tangent: " + Tangent + ", Binormal: " + Binormal + ", TextureCoordinate: " + TextureCoordinate;
        }
    }
}
