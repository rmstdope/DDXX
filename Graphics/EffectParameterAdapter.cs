using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.Graphics
{
    public class EffectParameterAdapter : IEffectParameter
    {
        private EffectParameter parameter;

        public EffectParameterAdapter(EffectParameter parameter)
        {
            this.parameter = parameter;
        }

        public EffectParameter DxEffectParameter { get { return parameter; } }

        #region IEffectParameter Members

        public ICollectionAdapter<IEffectAnnotation> Annotations
        {
            get { return new EffectAnnotationCollectionAdapter(parameter.Annotations); }
        }

        public int ColumnCount
        {
            get { return parameter.ColumnCount; }
        }

        public ICollectionAdapter<IEffectParameter> Elements
        {
            get { return new EffectParameterCollectionAdapter(parameter.Elements); }
        }

        public string Name
        {
            get { return parameter.Name; }
        }

        public EffectParameterClass ParameterClass
        {
            get { return parameter.ParameterClass; }
        }

        public EffectParameterType ParameterType
        {
            get { return parameter.ParameterType; }
        }

        public int RowCount
        {
            get { return parameter.RowCount; }
        }

        public string Semantic
        {
            get { return parameter.Semantic; }
        }

        public ICollectionAdapter<IEffectParameter> StructureMembers
        {
            get { return new EffectParameterCollectionAdapter(parameter.StructureMembers); }
        }

        public bool GetValueBoolean()
        {
            return parameter.GetValueBoolean();
        }

        public bool[] GetValueBooleanArray(int count)
        {
            return parameter.GetValueBooleanArray(count);
        }

        public int GetValueInt32()
        {
            return parameter.GetValueInt32();
        }

        public int[] GetValueInt32Array(int count)
        {
            return parameter.GetValueInt32Array(count);
        }

        public Matrix GetValueMatrix()
        {
            return parameter.GetValueMatrix();
        }

        public Matrix[] GetValueMatrixArray(int count)
        {
            return parameter.GetValueMatrixArray(count);
        }

        public Matrix GetValueMatrixTranspose()
        {
            return parameter.GetValueMatrixTranspose();
        }

        public Matrix[] GetValueMatrixTransposeArray(int count)
        {
            return parameter.GetValueMatrixTransposeArray(count);
        }

        public Quaternion GetValueQuaternion()
        {
            return parameter.GetValueQuaternion();
        }

        public Quaternion[] GetValueQuaternionArray(int count)
        {
            return parameter.GetValueQuaternionArray(count);
        }

        public float GetValueSingle()
        {
            return parameter.GetValueSingle();
        }

        public float[] GetValueSingleArray(int count)
        {
            return parameter.GetValueSingleArray(count);
        }

        public string GetValueString()
        {
            return parameter.GetValueString();
        }

        public ITexture2D GetValueTexture2D()
        {
            return new Texture2DAdapter(parameter.GetValueTexture2D());
        }

        public ITexture3D GetValueTexture3D()
        {
            return new Texture3DAdapter(parameter.GetValueTexture3D());
        }

        public ITextureCube GetValueTextureCube()
        {
            return new TextureCubeAdapter(parameter.GetValueTextureCube());
        }

        public Vector2 GetValueVector2()
        {
            return parameter.GetValueVector2();
        }

        public Vector2[] GetValueVector2Array(int count)
        {
            return parameter.GetValueVector2Array(count);
        }

        public Vector3 GetValueVector3()
        {
            return parameter.GetValueVector3();
        }

        public Vector3[] GetValueVector3Array(int count)
        {
            return parameter.GetValueVector3Array(count);
        }

        public Vector4 GetValueVector4()
        {
            return parameter.GetValueVector4();
        }

        public Vector4[] GetValueVector4Array(int count)
        {
            return parameter.GetValueVector4Array(count);
        }

        public void SetArrayRange(int start, int end)
        {
            parameter.SetArrayRange(start, end);
        }

        public void SetValue(bool value)
        {
            parameter.SetValue(value);
        }

        public void SetValue(bool[] value)
        {
            parameter.SetValue(value);
        }

        public void SetValue(float value)
        {
            parameter.SetValue(value);
        }

        public void SetValue(float[] value)
        {
            parameter.SetValue(value);
        }

        public void SetValue(int value)
        {
            parameter.SetValue(value);
        }

        public void SetValue(int[] value)
        {
            parameter.SetValue(value);
        }

        public void SetValue(Matrix value)
        {
            parameter.SetValue(value);
        }

        public void SetValue(Matrix[] value)
        {
            parameter.SetValue(value);
        }

        public void SetValue(Quaternion value)
        {
            parameter.SetValue(value);
        }

        public void SetValue(Quaternion[] value)
        {
            parameter.SetValue(value);
        }

        public void SetValue(string value)
        {
            parameter.SetValue(value);
        }

        public void SetValue(ITexture value)
        {
            if (value == null)
                parameter.SetValue((Texture2D)null);
            else
                parameter.SetValue((value as TextureAdapter).DxTexture);
        }

        public void SetValue(Vector2 value)
        {
            parameter.SetValue(value);
        }

        public void SetValue(Vector2[] value)
        {
            parameter.SetValue(value);
        }

        public void SetValue(Vector3 value)
        {
            parameter.SetValue(value);
        }

        public void SetValue(Vector3[] value)
        {
            parameter.SetValue(value);
        }

        public void SetValue(Vector4 value)
        {
            parameter.SetValue(value);
        }

        public void SetValue(Vector4[] value)
        {
            parameter.SetValue(value);
        }

        public void SetValueTranspose(Matrix value)
        {
            parameter.SetValue(value);
        }

        public void SetValueTranspose(Matrix[] value)
        {
            parameter.SetValue(value);
        }

        #endregion
    }
}
