using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.Graphics
{
    public interface IEffectParameter
    {
        // Summary:
        //     Gets the collection of EffectAnnotation objects for this parameter.
        //
        // Returns:
        //     The collection of EffectAnnotation objects.
        ICollectionAdapter<IEffectAnnotation> Annotations { get; }
        //
        // Summary:
        //     Gets the number of columns in the parameter description.
        //
        // Returns:
        //     The number of columns in the parameter description.
        int ColumnCount { get; }
        //
        // Summary:
        //     Gets the collection of effect parameters.
        //
        // Returns:
        //     The collection of effect parameters.
        ICollectionAdapter<IEffectParameter> Elements { get; }
        //
        // Summary:
        //     Gets the name of the parameter.
        //
        // Returns:
        //     The name of the parameter.
        string Name { get; }
        //
        // Summary:
        //     Gets the class of the parameter.
        //
        // Returns:
        //     The parameter class.
        EffectParameterClass ParameterClass { get; }
        //
        // Summary:
        //     Gets the type of the parameter.
        //
        // Returns:
        //     The parameter type.
        EffectParameterType ParameterType { get; }
        //
        // Summary:
        //     Gets the number of rows in the parameter description.
        //
        // Returns:
        //     The number of rows in the parameter description.
        int RowCount { get; }
        //
        // Summary:
        //     Gets the semantic meaning, or usage, of the parameter.
        //
        // Returns:
        //     The semantic meaning of the parameter.
        string Semantic { get; }
        //
        // Summary:
        //     Gets the collection of structure members.
        //
        // Returns:
        //     The collection of structure members.
        ICollectionAdapter<IEffectParameter> StructureMembers { get; }
        //
        // Summary:
        //     Gets the value of the EffectParameter as a System.Boolean.
        //
        // Returns:
        //     The value of the EffectParameter as a System.Boolean.
        bool GetValueBoolean();
        //
        // Summary:
        //     Gets the value of the EffectParameter as an array of System.Boolean.
        //
        // Parameters:
        //   count:
        //     The number of elements in the array.
        //
        // Returns:
        //     The value of the EffectParameter as an array of System.Boolean.
        bool[] GetValueBooleanArray(int count);
        //
        // Summary:
        //     Gets the value of the EffectParameter as an System.Int32.
        //
        // Returns:
        //     Gets the value of the EffectParameter as an System.Int32.
        int GetValueInt32();
        //
        // Summary:
        //     Gets the value of the EffectParameter as an array of System.Int32.
        //
        // Parameters:
        //   count:
        //     The number of elements in the array.
        //
        // Returns:
        //     The value of the EffectParameter as an array of System.Int32.
        int[] GetValueInt32Array(int count);
        //
        // Summary:
        //     Gets the value of the EffectParameter as a Framework.Matrix.
        //
        // Returns:
        //     The value of the EffectParameter as a Framework.Matrix.
        Matrix GetValueMatrix();
        //
        // Summary:
        //     Gets the value of the EffectParameter as an array of Framework.Matrix.
        //
        // Parameters:
        //   count:
        //     The number of elements in the array.
        //
        // Returns:
        //     The value of the EffectParameter as an array of Framework.Matrix.
        Matrix[] GetValueMatrixArray(int count);
        //
        // Summary:
        //     Gets the value of the EffectParameter as a Framework.Matrix transpose.
        //
        // Returns:
        //     The value of the EffectParameter as a Framework.Matrix transpose.
        Matrix GetValueMatrixTranspose();
        //
        // Summary:
        //     Gets the value of the EffectParameter as an array of Framework.Matrix transpose.
        //
        // Parameters:
        //   count:
        //     The number of elements in the array.
        //
        // Returns:
        //     The value of the EffectParameter as an array of Framework.Matrix transpose.
        Matrix[] GetValueMatrixTransposeArray(int count);
        //
        // Summary:
        //     Gets the value of the EffectParameter as a Framework.Quaternion.
        //
        // Returns:
        //     The value of the EffectParameter as a Framework.Quaternion.
        Quaternion GetValueQuaternion();
        //
        // Summary:
        //     Gets the value of the EffectParameter as an array of Framework.Quaternion.
        //
        // Parameters:
        //   count:
        //     The number of elements in the array.
        //
        // Returns:
        //     The value of the EffectParameter as an array of Framework.Quaternion.
        Quaternion[] GetValueQuaternionArray(int count);
        //
        // Summary:
        //     Gets the value of the EffectParameter as a System.Single.
        //
        // Returns:
        //     The value of the EffectParameter as a System.Single.
        float GetValueSingle();
        //
        // Summary:
        //     Gets the value of the EffectParameter as an array of System.Single.
        //
        // Parameters:
        //   count:
        //     The number of elements in the array.
        //
        // Returns:
        //     The value of the EffectParameter as an array of System.Single.
        float[] GetValueSingleArray(int count);
        //
        // Summary:
        //     Gets the value of the EffectParameter as an System.String.
        //
        // Returns:
        //     The value of the EffectParameter as an System.String.
        string GetValueString();
        //
        // Summary:
        //     Gets the value of the EffectParameter as a Texture2D.
        //
        // Returns:
        //     The value of the EffectParameter as a Texture2D.
        ITexture2D GetValueTexture2D();
        //
        // Summary:
        //     Gets the value of the EffectParameter as a Texture3D.
        //
        // Returns:
        //     The value of the EffectParameter as a Texture3D.
        ITexture3D GetValueTexture3D();
        //
        // Summary:
        //     Gets the value of the EffectParameter as a TextureCube.
        //
        // Returns:
        //     The value of the EffectParameter as a TextureCube.
        ITextureCube GetValueTextureCube();
        //
        // Summary:
        //     Gets the value of the EffectParameter as a Framework.Vector2.
        //
        // Returns:
        //     The value of the EffectParameter as a Framework.Vector2.
        Vector2 GetValueVector2();
        //
        // Summary:
        //     Gets the value of the EffectParameter as an array of Framework.Vector2.
        //
        // Parameters:
        //   count:
        //     The number of elements in the array.
        //
        // Returns:
        //     The value of the EffectParameter as an array of Framework.Vector2.
        Vector2[] GetValueVector2Array(int count);
        //
        // Summary:
        //     Gets the value of the EffectParameter as a Framework.Vector3.
        //
        // Returns:
        //     The value of the EffectParameter as a Framework.Vector3.
        Vector3 GetValueVector3();
        //
        // Summary:
        //     Gets the value of the EffectParameter as an array of Framework.Vector3.
        //
        // Parameters:
        //   count:
        //     The number of elements in the array.
        //
        // Returns:
        //     The value of the EffectParameter as an array of Framework.Vector3.
        Vector3[] GetValueVector3Array(int count);
        //
        // Summary:
        //     Gets the value of the EffectParameter as a Framework.Vector4.
        //
        // Returns:
        //     The value of the EffectParameter as a Framework.Vector4.
        Vector4 GetValueVector4();
        //
        // Summary:
        //     Gets the value of the EffectParameter as an array of Framework.Vector4.
        //
        // Parameters:
        //   count:
        //     The number of elements in the array.
        //
        // Returns:
        //     The value of the EffectParameter as an array of Framework.Vector4.
        Vector4[] GetValueVector4Array(int count);
        //
        // Summary:
        //     Sets the range of an array to pass to the device.
        //
        // Parameters:
        //   start:
        //     The start index.
        //
        //   end:
        //     The stop index.
        void SetArrayRange(int start, int end);
        //
        // Summary:
        //     Sets the value of the EffectParameter.
        //
        // Parameters:
        //   value:
        //     The value to assign to the EffectParameter.
        void SetValue(bool value);
        //
        // Summary:
        //     Sets the value of the EffectParameter.
        //
        // Parameters:
        //   value:
        //     The value to assign to the EffectParameter.
        void SetValue(bool[] value);
        //
        // Summary:
        //     Sets the value of the EffectParameter.
        //
        // Parameters:
        //   value:
        //     The value to assign to the EffectParameter.
        void SetValue(float value);
        //
        // Summary:
        //     Sets the value of the EffectParameter.
        //
        // Parameters:
        //   value:
        //     The value to assign to the EffectParameter.
        void SetValue(float[] value);
        //
        // Summary:
        //     Sets the value of the EffectParameter.
        //
        // Parameters:
        //   value:
        //     The value to assign to the EffectParameter.
        void SetValue(int value);
        //
        // Summary:
        //     Sets the value of the EffectParameter.
        //
        // Parameters:
        //   value:
        //     The value to assign to the EffectParameter.
        void SetValue(int[] value);
        //
        // Summary:
        //     Sets the value of the EffectParameter.
        //
        // Parameters:
        //   value:
        //     The value to assign to the EffectParameter.
        void SetValue(Matrix value);
        //
        // Summary:
        //     Sets the value of the EffectParameter.
        //
        // Parameters:
        //   value:
        //     The value to assign to the EffectParameter.
        void SetValue(Matrix[] value);
        //
        // Summary:
        //     Sets the value of the EffectParameter.
        //
        // Parameters:
        //   value:
        //     The value to assign to the EffectParameter.
        void SetValue(Quaternion value);
        //
        // Summary:
        //     Sets the value of the EffectParameter.
        //
        // Parameters:
        //   value:
        //     The value to assign to the EffectParameter.
        void SetValue(Quaternion[] value);
        //
        // Summary:
        //     Sets the value of the EffectParameter.
        //
        // Parameters:
        //   value:
        //     The value to assign to the EffectParameter.
        void SetValue(string value);
        //
        // Summary:
        //     Sets the value of the EffectParameter.
        //
        // Parameters:
        //   value:
        //     The value to assign to the EffectParameter.
        void SetValue(ITexture value);
        //
        // Summary:
        //     Sets the value of the EffectParameter.
        //
        // Parameters:
        //   value:
        //     The value to assign to the EffectParameter.
        void SetValue(Vector2 value);
        //
        // Summary:
        //     Sets the value of the EffectParameter.
        //
        // Parameters:
        //   value:
        //     The value to assign to the EffectParameter.
        void SetValue(Vector2[] value);
        //
        // Summary:
        //     Sets the value of the EffectParameter.
        //
        // Parameters:
        //   value:
        //     The value to assign to the EffectParameter.
        void SetValue(Vector3 value);
        //
        // Summary:
        //     Sets the value of the EffectParameter.
        //
        // Parameters:
        //   value:
        //     The value to assign to the EffectParameter.
        void SetValue(Vector3[] value);
        //
        // Summary:
        //     Sets the value of the EffectParameter.
        //
        // Parameters:
        //   value:
        //     The value to assign to the EffectParameter.
        void SetValue(Vector4 value);
        //
        // Summary:
        //     Sets the value of the EffectParameter.
        //
        // Parameters:
        //   value:
        //     The value to assign to the EffectParameter.
        void SetValue(Vector4[] value);
        //
        // Summary:
        //     Sets the value of the EffectParameter to the transpose of a Framework.Matrix.
        //
        // Parameters:
        //   value:
        //     The value.
        void SetValueTranspose(Matrix value);
        //
        // Summary:
        //     Sets the value of the EffectParameter to the transpose of a Framework.Matrix.
        //
        // Parameters:
        //   value:
        //     The value.
        void SetValueTranspose(Matrix[] value);
    }
}
