using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.Graphics
{
    public interface IEffectAnnotation
    {
        // Summary:
        //     Gets the number of columns in this effect annotation.
        //
        // Returns:
        //     The number of columns in this effect annotation.
        int ColumnCount { get; }
        //
        // Summary:
        //     Gets the name of the effect annotation.
        //
        // Returns:
        //     The name of the effect annotation.
        string Name { get; }
        //
        // Summary:
        //     Gets the parameter class of this effect annotation.
        //
        // Returns:
        //     The parameter class of this effect annotation.
        EffectParameterClass ParameterClass { get; }
        //
        // Summary:
        //     Gets the parameter type of this effect annotation.
        //
        // Returns:
        //     The parameter type of this effect annotation.
        EffectParameterType ParameterType { get; }
        //
        // Summary:
        //     Gets the row count of this effect annotation.
        //
        // Returns:
        //     The row count of this effect annotation.
        int RowCount { get; }
        //
        // Summary:
        //     Gets the semantic of this effect annotation.
        //
        // Returns:
        //     The semantic of this effect annotation.
        string Semantic { get; }
        //
        // Summary:
        //     Gets the value of the EffectAnnotation as a System.Boolean.
        //
        // Returns:
        //     The value of the EffectAnnotation as a System.Boolean.
        bool GetValueBoolean();
        //
        // Summary:
        //     Gets the value of the EffectAnnotation as a System.Int32.
        //
        // Returns:
        //     The value of the EffectAnnotation as a System.Int32.
        int GetValueInt32();
        //
        // Summary:
        //     Gets the value of the EffectAnnotation as a System.Int32.
        //
        // Returns:
        //     The value of the EffectAnnotation as a System.Int32.
        Matrix GetValueMatrix();
        //
        // Summary:
        //     Gets the value of the EffectAnnotation as a System.Single.
        //
        // Returns:
        //     The value of the EffectAnnotation as a System.Single.
        float GetValueSingle();
        //
        // Summary:
        //     Gets the value of the EffectAnnotation as a System.String.
        //
        // Returns:
        //     The value of the EffectAnnotation as a System.String.
        string GetValueString();
        //
        // Summary:
        //     Gets the value of the EffectAnnotation as a Framework.Vector2.
        //
        // Returns:
        //     The value of the EffectAnnotation as a Framework.Vector2.
        Vector2 GetValueVector2();
        //
        // Summary:
        //     Gets the value of the EffectAnnotation as a Framework.Vector3.
        //
        // Returns:
        //     The value of the EffectAnnotation as a Framework.Vector3.
        Vector3 GetValueVector3();
        //
        // Summary:
        //     Gets the value of the EffectAnnotation as a Framework.Vector4.
        //
        // Returns:
        //     The value of the EffectAnnotation as a Framework.Vector4.
        Vector4 GetValueVector4();
    }
}
