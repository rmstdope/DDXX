using Microsoft.Xna.Framework;
using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.Graphics
{
    public struct VertexPositionColorPoint
    {
        //
        // Summary:
        //     The vertex position.
        [SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public Vector3 Position;
        // Summary:
        //     The vertex point size.
        [SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public float PointSize;
        // Summary:
        //     The vertex color.
        [SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public Color Color;
        //
        // Summary:
        //     An array of three vertex elements describing the position, color and point size
        //     of this vertex.
        [SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public static readonly VertexElement[] VertexElements = new VertexElement[] 
        {
            new VertexElement(0, 0, VertexElementFormat.Vector3, VertexElementMethod.Default, VertexElementUsage.Position, 0),
            new VertexElement(0, 12, VertexElementFormat.Single, VertexElementMethod.Default, VertexElementUsage.PointSize, 0),
            new VertexElement(0, 16, VertexElementFormat.Color, VertexElementMethod.Default, VertexElementUsage.Color, 0),
        };
        //
        // Summary:
        //     Initializes a new instance of the VertexPositionColorSize class.
        //
        // Parameters:
        //   position:
        //     Position of the vertex.
        //
        //   color:
        //     The vertex color.
        public VertexPositionColorPoint(Vector3 position, Color color, float size)
        {
            Position = position;
            Color = color;
            PointSize = size;
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
        public static bool operator !=(VertexPositionColorPoint left, VertexPositionColorPoint right)
        {
            return left.PointSize != right.PointSize || left.Color != right.Color || left.Position != right.Position;
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
        public static bool operator ==(VertexPositionColorPoint left, VertexPositionColorPoint right)
        {
            return left.PointSize == right.PointSize && left.Color == right.Color && left.Position == right.Position;
        }

        // Summary:
        //     Gets the size of the VertexPositionColorPoint class.
        //
        // Returns:
        //     The size of the vertex, in bytes.
        public static int SizeInBytes { get { return (3 + 1 + 1) * sizeof(float); } }

        // Summary:
        //     Returns a value that indicates whether the current instance is equal to a
        //     specified object.
        //
        // Parameters:
        //   obj:
        //     The System.Object to compare with the current VertexPositionColorPoint.
        //
        // Returns:
        //     true if the objects are the same; false otherwise.
        public override bool Equals(object obj)
        {
            if (!(obj is VertexPositionColorPoint))
                return false;
            return this == (VertexPositionColorPoint)obj;
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
            return "Position: " + Position + ", Color: " + Color + ", PointSize: " + PointSize;
        }
    }
}
