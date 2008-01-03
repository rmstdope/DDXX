using Microsoft.Xna.Framework;
using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.Graphics
{
    public struct VertexPositionNormal
    {
        //
        // Summary:
        //     The vertex position.
        [SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public Vector3 Position;
        // Summary:
        //     The vertex normal.
        [SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public Vector3 Normal;
        //
        // Summary:
        //     An array of three vertex elements describing the position and normal
        //     of this vertex.
        [SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public static readonly VertexElement[] VertexElements = new VertexElement[] 
        {
            new VertexElement(0, 0, VertexElementFormat.Vector3, VertexElementMethod.Default, VertexElementUsage.Position, 0),
            new VertexElement(0, 12, VertexElementFormat.Vector3, VertexElementMethod.Default, VertexElementUsage.Normal, 0)
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
        public VertexPositionNormal(Vector3 position, Vector3 normal)
        {
            Position = position;
            Normal = normal;
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
        public static bool operator !=(VertexPositionNormal left, VertexPositionNormal right)
        {
            return left.Normal != right.Normal || left.Position != right.Position;
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
        public static bool operator ==(VertexPositionNormal left, VertexPositionNormal right)
        {
            return left.Normal == right.Normal && left.Position == right.Position;
        }

        // Summary:
        //     Gets the size of the VertexPositionNormal class.
        //
        // Returns:
        //     The size of the vertex, in bytes.
        public static int SizeInBytes { get { return 2 * 3 * sizeof(float); } }

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
            if (!(obj is VertexPositionNormal))
                return false;
            return this == (VertexPositionNormal)obj;
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
            return "Position: " + Position + ", Normal: " + Normal;
        }
    }
}
