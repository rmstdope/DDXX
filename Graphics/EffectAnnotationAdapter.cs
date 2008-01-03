using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.Graphics
{
    public class EffectAnnotationAdapter : IEffectAnnotation
    {
        private EffectAnnotation annotation;

        public EffectAnnotationAdapter(EffectAnnotation annotation)
        {
            this.annotation = annotation;
        }

        #region IEffectAnnotation Members

        public int ColumnCount
        {
            get { return annotation.ColumnCount; }
        }

        public string Name
        {
            get { return annotation.Name; }
        }

        public EffectParameterClass ParameterClass
        {
            get { return annotation.ParameterClass; }
        }

        public EffectParameterType ParameterType
        {
            get { return annotation.ParameterType; }
        }

        public int RowCount
        {
            get { return annotation.RowCount; }
        }

        public string Semantic
        {
            get { return annotation.Semantic; }
        }

        public bool GetValueBoolean()
        {
            return annotation.GetValueBoolean();
        }

        public int GetValueInt32()
        {
            return annotation.GetValueInt32();
        }

        public Matrix GetValueMatrix()
        {
            return annotation.GetValueMatrix();
        }

        public float GetValueSingle()
        {
            return annotation.GetValueSingle();
        }

        public string GetValueString()
        {
            return annotation.GetValueString();
        }

        public Vector2 GetValueVector2()
        {
            return annotation.GetValueVector2();
        }

        public Vector3 GetValueVector3()
        {
            return annotation.GetValueVector3();
        }

        public Vector4 GetValueVector4()
        {
            return annotation.GetValueVector4();
        }

        #endregion
    }
}
