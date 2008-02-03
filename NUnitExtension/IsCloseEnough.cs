using System;
using System.Collections.Generic;
using System.Text;
using NMock2;
using Microsoft.Xna.Framework;

namespace NUnitExtension
{
    public class IsCloseEnough : Matcher
    {
        private object expected;
        private float tolerance;

        public IsCloseEnough(object expected, float tolerance)
        {
            this.expected = expected;
            this.tolerance = tolerance;
        }

        public override void DescribeTo(System.IO.TextWriter writer)
        {
        }

        public override bool Matches(object o)
        {
            if (expected is Vector2 && o is Vector2)
            {
                Vector2 expectedVector = (Vector2)expected;
                Vector2 actualVector = (Vector2)o;
                return Math.Abs(expectedVector.X - actualVector.X) <= tolerance &&
                    Math.Abs(expectedVector.Y - actualVector.Y) <= tolerance;
            }
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
