using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace Dope.DDXX.NUnitExtension
{
    public class XnaConstraint : Constraint
    {
        private object expected; 

        public XnaConstraint(object expected)
        {
            this.expected = expected;
        }

        public bool Matches(object actual, bool x)
        {
            this.actual = actual;
            if (expected is Vector3 && actual is Vector3)
            {
                Vector3 aVec = (Vector3)actual;
                Vector3 eVec = (Vector3)expected;
                return (Math.Abs(eVec.X - aVec.X) <= (float)tolerance &&
                        Math.Abs(eVec.Y - aVec.Y) <= (float)tolerance &&
                        Math.Abs(eVec.Z - aVec.Z) <= (float)tolerance);
            }
            return false;
        }

        public override bool Matches(object actual)
        {
            if (tolerance == null)
                tolerance = 0.0f;
            this.actual = actual;
            if (expected is Array && actual is Array)
                return Matches(actual as Array, expected as Array);
            return Matches(new object[] { actual }, new object[] { expected });
        }

        public bool Matches(Array actual, Array expected)
        {
            if (actual.Length == expected.Length)
            {
                for (int i = 0; i < actual.Length; i++)
                {
                    if (expected.GetValue(i) is Matrix && actual.GetValue(i) is Matrix)
                    {
                        if (!Matches((Matrix)actual.GetValue(i), (Matrix)expected.GetValue(i)))
                        {
                            return false;
                        }
                    }
                    else if (expected.GetValue(i) is Vector2 && actual.GetValue(i) is Vector2)
                    {
                        if (!Matches((Vector2)actual.GetValue(i), (Vector2)expected.GetValue(i)))
                        {
                            return false;
                        }
                    }
                    else if (expected.GetValue(i) is Vector3 && actual.GetValue(i) is Vector3)
                    {
                        if (!Matches((Vector3)actual.GetValue(i), (Vector3)expected.GetValue(i)))
                        {
                            return false;
                        }
                    }
                    else if (expected.GetValue(i) is Vector4 && actual.GetValue(i) is Vector4)
                    {
                        if (!Matches((Vector4)actual.GetValue(i), (Vector4)expected.GetValue(i)))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

        public bool Matches(Matrix actual, Matrix expected)
        {
            if (Math.Abs(expected.M11 - actual.M11) <= (float)tolerance &&
                Math.Abs(expected.M12 - actual.M12) <= (float)tolerance &&
                Math.Abs(expected.M13 - actual.M13) <= (float)tolerance &&
                Math.Abs(expected.M14 - actual.M14) <= (float)tolerance &&
                Math.Abs(expected.M21 - actual.M21) <= (float)tolerance &&
                Math.Abs(expected.M22 - actual.M22) <= (float)tolerance &&
                Math.Abs(expected.M23 - actual.M23) <= (float)tolerance &&
                Math.Abs(expected.M24 - actual.M24) <= (float)tolerance &&
                Math.Abs(expected.M31 - actual.M31) <= (float)tolerance &&
                Math.Abs(expected.M32 - actual.M32) <= (float)tolerance &&
                Math.Abs(expected.M33 - actual.M33) <= (float)tolerance &&
                Math.Abs(expected.M34 - actual.M34) <= (float)tolerance &&
                Math.Abs(expected.M41 - actual.M41) <= (float)tolerance &&
                Math.Abs(expected.M42 - actual.M42) <= (float)tolerance &&
                Math.Abs(expected.M43 - actual.M43) <= (float)tolerance &&
                Math.Abs(expected.M44 - actual.M44) <= (float)tolerance)
            {
                return true;
            }
            return false;
        }

        public bool Matches(Vector2 actual, Vector2 expected)
        {
            return Matches(new Vector3(actual, 0), new Vector3(actual, 0));
        }

        public bool Matches(Vector3 actual, Vector3 expected)
        {
            return Matches(new Vector4(actual, 0), new Vector4(actual, 0));
        }

        public bool Matches(Vector4 actual, Vector4 expected)
        {
            if (Math.Abs(expected.X - actual.X) <= (float)tolerance &&
                Math.Abs(expected.Y - actual.Y) <= (float)tolerance &&
                Math.Abs(expected.Z - actual.Z) <= (float)tolerance &&
                Math.Abs(expected.W - actual.W) <= (float)tolerance)
            {
                return true;
            }
            return false;
        }

        public override void WriteDescriptionTo(MessageWriter writer)
        {
            if ((float)tolerance > 0.0f)
            {
                writer.WriteExpectedValue(expected);
                writer.Write("(tolerance " + tolerance + ")");
            }
            else
            {
                writer.WriteExpectedValue(expected);
            }
        }

        public override void WriteActualValueTo(MessageWriter writer)
        {
            writer.WriteActualValue(actual);
        }

    }
}
