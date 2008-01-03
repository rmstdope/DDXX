using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Diagnostics.CodeAnalysis;

namespace Dope.DDXX.Graphics
{
    public struct VertexTransformedTexture
    {
        // Summary:
        //     The vertex position.
        [SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public Vector4 Position;
        //
        // Summary:
        //     The texture coordinates.
        [SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public Vector2 TextureCoordinate;
        //
        // Summary:
        //     An array of two vertex elements describing the position, followed by the
        //     texture coordinate, of this vertex.
        [SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public static readonly VertexElement[] VertexElements = new VertexElement[] 
        {
            new VertexElement(0, 0, VertexElementFormat.Vector4, VertexElementMethod.Default, VertexElementUsage.Position, 1),
            new VertexElement(0, 16, VertexElementFormat.Vector2, VertexElementMethod.Default, VertexElementUsage.TextureCoordinate, 0)
        };

        //
        // Summary:
        //     Initializes a new instance of the VertexPositionTexture class.
        //
        // Parameters:
        //   position:
        //     Position of the vertex.
        //
        //   textureCoordinate:
        //     Texture coordinate of the vertex.
        public VertexTransformedTexture(Vector4 position, Vector2 textureCoordinate)
        {
            Position = position;
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
        public static bool operator !=(VertexTransformedTexture left, VertexTransformedTexture right)
        {
            return (left.Position != right.Position || left.TextureCoordinate != right.TextureCoordinate);
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
        public static bool operator ==(VertexTransformedTexture left, VertexTransformedTexture right)
        {
            return (left.Position == right.Position && left.TextureCoordinate == right.TextureCoordinate);
        }

        // Summary:
        //     Gets the size of the VertexPositionTexture class.
        //
        // Returns:
        //     The size of the vertex, in bytes.
        public static int SizeInBytes 
        {
            get { return 4 * 2 + 4 * 4; }
        }

        // Summary:
        //     Returns a value that indicates whether the current instance is equal to a
        //     specified object.
        //
        // Parameters:
        //   obj:
        //     The System.Object to compare with the current VertexPositionTexture.
        //
        // Returns:
        //     true if the objects are the same; false otherwise.
        public override bool Equals(object obj)
        {
            if (!(obj is VertexTransformedTexture))
                return false;
            VertexTransformedTexture other = (VertexTransformedTexture)obj;
            return other == this;
        }

        //
        // Summary:
        //     Gets the hash code for this instance.
        //
        // Returns:
        //     Hash code for this object.
        public override int GetHashCode()
        {
            return Position.GetHashCode() + TextureCoordinate.GetHashCode();
        }

        //
        // Summary:
        //     Retrieves a string representation of this object.
        //
        // Returns:
        //     String representation of this object.
        public override string ToString()
        {
            return "Position: " + Position.ToString() + ", TextureCoordinate: " + TextureCoordinate.ToString();
        }
    }
}
