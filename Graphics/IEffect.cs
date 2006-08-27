using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Dope.DDXX.Graphics
{
    public interface IEffect
    {
        // Summary:
        //     Retrieves an effect description.
        EffectDescription Description { get; }
        // Summary:
        //     Retrieves the device associated with an effect.
        Device Device { get; }
        //
        // Summary:
        //     Gets a value that indicates whether the object is disposed.
        bool Disposed { get; }
        //
        // Summary:
        //     Retrieves an effect pool object that represents the pool of shared parameters.
        EffectPool Pool { get; }
        //
        // Summary:
        //     Retrieves or sets the Microsoft.DirectX.Direct3D.EffectStateManager object
        //     for this effect.
        EffectStateManager StateManager { get; set; }
        //
        // Summary:
        //     Retreives and sets the active technique.
        EffectHandle Technique { get; set; }
        // Summary:
        //     Assigns a state value to each effect parameter in a parameter block.
        //
        // Parameters:
        //   parameterBlock:
        //     An Microsoft.DirectX.Direct3D.EffectHandle object that is returned by Microsoft.DirectX.Direct3D.Effect.EndParameterBlock().
        void ApplyParameterBlock(EffectHandle parameterBlock);
        //
        // Summary:
        //     Begins the application of an effect technique.
        //
        // Parameters:
        //   flags:
        //     One or more Microsoft.DirectX.Direct3D.FX enumerated values that determine
        //     whether state modified by an effect will be saved and restored. The default
        //     value, 0, specifies that Microsoft.DirectX.Direct3D.Effect.Begin(Microsoft.DirectX.Direct3D.FX)
        //     and Microsoft.DirectX.Direct3D.Effect.End() will save and restore all state
        //     modified by the effect (including pixel and vertex shader constants). Microsoft.DirectX.Direct3D.FX.DoNotSaveShaderState
        //     indicates that shader device state will not be saved or restored; Microsoft.DirectX.Direct3D.FX.DoNotSaveState
        //     indicates that device state will not be saved or restored.
        //
        // Returns:
        //     An integer value that indicates the number of passes needed to render the
        //     current effect technique.
        int Begin(FX flags);
        //
        // Summary:
        //     Captures parameter effect state changes.
        void BeginParameterBlock();
        //
        // Summary:
        //     Applies the state settings for the specified pass of a technique.
        //
        // Parameters:
        //   passNumber:
        //     An integer that identifies the pass to apply. This value is returned by Microsoft.DirectX.Direct3D.Effect.Begin(Microsoft.DirectX.Direct3D.FX),
        //     which must be called before rendering an effect.
        void BeginPass(int passNumber);
        //
        // Summary:
        //     Creates a clone of an effect.
        //
        // Parameters:
        //   dev:
        //     The Microsoft.DirectX.Direct3D.Device associated with the effect.
        //
        // Returns:
        //     An Microsoft.DirectX.Direct3D.Effect object that contains the cloned effect.
        Effect Clone(Device dev);
        //
        // Summary:
        //     Propagates the state change that occurs inside of an active pass to the device
        //     before rendering.
        void CommitChanges();
        //
        // Summary:
        //     Deletes an effect parameter block.
        //
        // Parameters:
        //   parameterBlock:
        //     Handle to the parameter block to delete. This is the handle returned by Microsoft.DirectX.Direct3D.Effect.Microsoft.DirectX.Direct3D.Effect.EndParameterBlock().
        void DeleteParameterBlock(EffectHandle parameterBlock);
        //
        // Summary:
        //     Disassembles an effect.
        //
        // Parameters:
        //   enableColorCode:
        //     Set to true to enable color coding to make the disassembly easier to read.
        //     Set to false for no color coding.
        //
        // Returns:
        //     A string that contains the disassembled effect.
        string Disassemble(bool enableColorCode);
        //
        // Summary:
        //     Immediately releases the unmanaged resources used by the Microsoft.DirectX.Direct3D.Effect
        //     object.
        void Dispose();
        //
        // Summary:
        //     Ends the application of the current effect technique.
        void End();
        //
        // Summary:
        //     Stops the capturing of effect parameter state changes.
        //
        // Returns:
        //     An Microsoft.DirectX.Direct3D.EffectHandle for the parameter state block.
        EffectHandle EndParameterBlock();
        //
        // Summary:
        //     Signals the end of an effect pass.
        void EndPass();
        //
        // Summary:
        //     Searches for the next valid technique, starting at the technique after the
        //     one specified.
        //
        // Parameters:
        //   technique:
        //     Unique identifier of a technique. For more information, see Microsoft.DirectX.Direct3D.EffectHandle.
        //     Specify null for this parameter to find the first valid technique.
        //
        // Returns:
        //     An Microsoft.DirectX.Direct3D.EffectHandle object that identifies the next
        //     technique, or null if the current technique is the last.
        EffectHandle FindNextValidTechnique(EffectHandle technique);
        //
        // Summary:
        //     Retrieves an annotation.
        //
        // Parameters:
        //   technique:
        //     An Microsoft.DirectX.Direct3D.EffectHandle object that represents a technique,
        //     pass, or top-level parameter.
        //
        //   index:
        //     Annotation index.
        //
        // Returns:
        //     A Microsoft.DirectX.Direct3D.EffectHandle object that specifies the annotation,
        //     or null if the annotation is not found.
        EffectHandle GetAnnotation(EffectHandle technique, int index);
        //
        // Summary:
        //     Retrieves an annotation.
        //
        // Parameters:
        //   technique:
        //     An Microsoft.DirectX.Direct3D.EffectHandle object that represents a technique,
        //     pass, or top-level parameter.
        //
        //   name:
        //     Annotation name.
        //
        // Returns:
        //     A Microsoft.DirectX.Direct3D.EffectHandle object that specifies the annotation,
        //     or null if the annotation is not found.
        EffectHandle GetAnnotation(EffectHandle technique, string name);
        //
        // Summary:
        //     Retrieves a function.
        //
        // Parameters:
        //   index:
        //     Index of the function.
        //
        // Returns:
        //     Specified Microsoft.DirectX.Direct3D.EffectHandle associated with the function,
        //     or null if it is not found.
        EffectHandle GetFunction(int index);
        //
        // Summary:
        //     Retrieves a function.
        //
        // Parameters:
        //   name:
        //     String that contains the name of the function.
        //
        // Returns:
        //     Specified Microsoft.DirectX.Direct3D.EffectHandle associated with the function,
        //     or null if it is not found.
        EffectHandle GetFunction(string name);
        //
        // Summary:
        //     Retrieves a function description.
        //
        // Parameters:
        //   shader:
        //     An Microsoft.DirectX.Direct3D.EffectHandle object that indicates the function
        //     form from which to retrieve the description.
        //
        // Returns:
        //     A Microsoft.DirectX.Direct3D.FunctionDescription structure that describes
        //     the function.
        FunctionDescription GetFunctionDescription(EffectHandle shader);
        //
        // Summary:
        //     Retrieves a top-level parameter or a structure member parameter.
        //
        // Parameters:
        //   constant:
        //     An Microsoft.DirectX.Direct3D.EffectHandle object that indicates the parameter,
        //     or null for top-level parameters.
        //
        //   index:
        //     Parameter index.
        //
        // Returns:
        //     An Microsoft.DirectX.Direct3D.EffectHandle of the specified parameter, or
        //     null if the parameter is not found.
        EffectHandle GetParameter(EffectHandle constant, int index);
        //
        // Summary:
        //     Retrieves a top-level parameter or a structure member parameter.
        //
        // Parameters:
        //   constant:
        //     An Microsoft.DirectX.Direct3D.EffectHandle object that indicates the parameter,
        //     or null for top-level parameters.
        //
        //   name:
        //     String that contains the parameter name.
        //
        // Returns:
        //     An Microsoft.DirectX.Direct3D.EffectHandle of the specified parameter, or
        //     null if the parameter is not found.
        EffectHandle GetParameter(EffectHandle constant, string name);
        //
        // Summary:
        //     Retrieves the handle of a top-level parameter or structure member parameter
        //     by looking up its semantic.
        //
        // Parameters:
        //   constant:
        //     The Microsoft.DirectX.Direct3D.EffectHandle of the parameter, or null for
        //     top-level parameters.
        //
        //   name:
        //     String that contains the semantic name.
        //
        // Returns:
        //     The Microsoft.DirectX.Direct3D.EffectHandle of the first parameter that matches
        //     the specified semantic, or null if the semantic is not found.
        EffectHandle GetParameterBySemantic(EffectHandle constant, string name);
        //
        // Summary:
        //     Retrieves a parameter or annotation description.
        //
        // Parameters:
        //   parameter:
        //     An Microsoft.DirectX.Direct3D.EffectHandle that indicates the parameter or
        //     annotation handle.
        //
        // Returns:
        //     Description of the specified parameter or annotation. For more information,
        //     see Microsoft.DirectX.Direct3D.ParameterDescription.
        ParameterDescription GetParameterDescription(EffectHandle parameter);
        //
        // Summary:
        //     Retrieves the Microsoft.DirectX.Direct3D.EffectHandle of an array element
        //     parameter.
        //
        // Parameters:
        //   constant:
        //     The Microsoft.DirectX.Direct3D.EffectHandle associated with the array.
        //
        //   index:
        //     Array element index.
        //
        // Returns:
        //     Handle of the specified parameter, or null if either Microsoft.DirectX.Direct3D.BaseEffect.GetParameterElement(Microsoft.DirectX.Direct3D.EffectHandle,System.Int32)
        //     or Microsoft.DirectX.Direct3D.BaseEffect.GetParameterElement(Microsoft.DirectX.Direct3D.EffectHandle,System.Int32)
        //     is invalid.
        EffectHandle GetParameterElement(EffectHandle constant, int index);
        //
        // Summary:
        //     Retrieves the Microsoft.DirectX.Direct3D.EffectHandle of a pass.
        //
        // Parameters:
        //   technique:
        //     The Microsoft.DirectX.Direct3D.EffectHandle of the parent technique.
        //
        //   index:
        //     Index for the pass.
        //
        // Returns:
        //     The Microsoft.DirectX.Direct3D.EffectHandle of the specified pass inside
        //     the specified technique, or null if the index is invalid.
        EffectHandle GetPass(EffectHandle technique, int index);
        //
        // Summary:
        //     Retrieves the Microsoft.DirectX.Direct3D.EffectHandle of a pass.
        //
        // Parameters:
        //   technique:
        //     The Microsoft.DirectX.Direct3D.EffectHandle of the parent technique.
        //
        //   name:
        //     String that contains the name of the pass.
        //
        // Returns:
        //     The Microsoft.DirectX.Direct3D.EffectHandle of the specified pass inside
        //     the specified technique, or null if the index is invalid.
        EffectHandle GetPass(EffectHandle technique, string name);
        //
        // Summary:
        //     Retrieves a pass description.
        //
        // Parameters:
        //   pass:
        //     Pass Microsoft.DirectX.Direct3D.EffectHandle.
        //
        // Returns:
        //     A Microsoft.DirectX.Direct3D.PassDescription of the specified pass.
        PassDescription GetPassDescription(EffectHandle pass);
        //
        // Summary:
        //     Retrieves the Microsoft.DirectX.Direct3D.EffectHandle of a technique.
        //
        // Parameters:
        //   index:
        //     Technique index.
        //
        // Returns:
        //     The Microsoft.DirectX.Direct3D.EffectHandle of the first technique that has
        //     the specified name, or null if the name is not found.
        EffectHandle GetTechnique(int index);
        //
        // Summary:
        //     Retrieves the Microsoft.DirectX.Direct3D.EffectHandle of a technique.
        //
        // Parameters:
        //   name:
        //     String that contains the technique name.
        //
        // Returns:
        //     The Microsoft.DirectX.Direct3D.EffectHandle of the first technique that has
        //     the specified name, or null if the name is not found.
        EffectHandle GetTechnique(string name);
        //
        // Summary:
        //     Retrieves a technique description.
        //
        // Parameters:
        //   technique:
        //     Technique Microsoft.DirectX.Direct3D.EffectHandle.
        //
        // Returns:
        //     A Microsoft.DirectX.Direct3D.TechniqueDescription of the specified technique.
        TechniqueDescription GetTechniqueDescription(EffectHandle technique);
        //
        // Summary:
        //     Retrieves the value of an arbitrary parameter or annotation, including simple
        //     types, structures, arrays, strings, shaders, and textures.
        //
        // Parameters:
        //   parameter:
        //     Unique Microsoft.DirectX.Direct3D.EffectHandle identifier.
        //
        //   numberBytes:
        //     Number of bytes in the Microsoft.DirectX.GraphicsStream. To skip size validation,
        //     pass in 0, provided the Microsoft.DirectX.GraphicsStream is large enough
        //     to contain the entire parameter.
        //
        // Returns:
        //     A Microsoft.DirectX.GraphicsStream object that contains the value.
        GraphicsStream GetValue(EffectHandle parameter, int numberBytes);
        //
        // Summary:
        //     Retrieves a Boolean value.
        //
        // Parameters:
        //   parameter:
        //     Unique Microsoft.DirectX.Direct3D.EffectHandle identifier.
        //
        // Returns:
        //     Boolean value.
        bool GetValueBoolean(EffectHandle parameter);
        //
        // Summary:
        //     Retrieves an array of Boolean values.
        //
        // Parameters:
        //   parameter:
        //     Unique Microsoft.DirectX.Direct3D.EffectHandle identifier.
        //
        //   count:
        //     Number of Boolean values in the returned array.
        //
        // Returns:
        //     Array of Boolean values.
        bool[] GetValueBooleanArray(EffectHandle parameter, int count);
        //
        // Summary:
        //     Retrieves a Microsoft.DirectX.Direct3D.ColorValue structure.
        //
        // Parameters:
        //   parameter:
        //     An Microsoft.DirectX.Direct3D.EffectHandle associated with the color value.
        //
        // Returns:
        //     A Microsoft.DirectX.Direct3D.ColorValue structure.
        ColorValue GetValueColor(EffectHandle parameter);
        //
        // Summary:
        //     Retrieves an array of Microsoft.DirectX.Direct3D.ColorValue structures.
        //
        // Parameters:
        //   parameter:
        //     An Microsoft.DirectX.Direct3D.EffectHandle associated with the color value
        //     array.
        //
        //   count:
        //     Number of values in the returned array.
        //
        // Returns:
        //     Array of Microsoft.DirectX.Direct3D.ColorValue structures.
        ColorValue[] GetValueColorArray(EffectHandle parameter, int count);
        //
        // Summary:
        //     Retrieves a floating-point value.
        //
        // Parameters:
        //   parameter:
        //     Unique Microsoft.DirectX.Direct3D.EffectHandle identifier.
        //
        // Returns:
        //     A System.Single value.
        float GetValueFloat(EffectHandle parameter);
        //
        // Summary:
        //     Retrieves an array of floating-point values.
        //
        // Parameters:
        //   parameter:
        //     Unique Microsoft.DirectX.Direct3D.EffectHandle identifier.
        //
        //   count:
        //     Number of floating-point values in the returned array.
        //
        // Returns:
        //     Array of System.Single values.
        float[] GetValueFloatArray(EffectHandle parameter, int count);
        //
        // Summary:
        //     Retrieves an integer.
        //
        // Parameters:
        //   parameter:
        //     Unique Microsoft.DirectX.Direct3D.EffectHandle identifier.
        //
        // Returns:
        //     Integer.
        int GetValueInteger(EffectHandle parameter);
        //
        // Summary:
        //     Retrieves an array of integers.
        //
        // Parameters:
        //   parameter:
        //     Unique Microsoft.DirectX.Direct3D.EffectHandle identifier.
        //
        //   count:
        //     Number of integers in the returned array.
        //
        // Returns:
        //     Array of integers.
        int[] GetValueIntegerArray(EffectHandle parameter, int count);
        //
        // Summary:
        //     Retrieves a nontransposed matrix.
        //
        // Parameters:
        //   parameter:
        //     Unique Microsoft.DirectX.Direct3D.EffectHandle identifier.
        //
        // Returns:
        //     Nontransposed Microsoft.DirectX.Matrix.
        Matrix GetValueMatrix(EffectHandle parameter);
        //
        // Summary:
        //     Retrieves an array of nontransposed matrices.
        //
        // Parameters:
        //   parameter:
        //     Unique Microsoft.DirectX.Direct3D.EffectHandle identifier.
        //
        //   count:
        //     Number of matrices in the returned array.
        //
        // Returns:
        //     Nontransposed Microsoft.DirectX.Matrix.
        Matrix[] GetValueMatrixArray(EffectHandle parameter, int count);
        //
        // Summary:
        //     Retrieves a transposed matrix.
        //
        // Parameters:
        //   parameter:
        //     Unique Microsoft.DirectX.Direct3D.EffectHandle identifier.
        //
        // Returns:
        //     Transposed Microsoft.DirectX.Matrix.
        Matrix GetValueMatrixTranspose(EffectHandle parameter);
        //
        // Summary:
        //     Retrieves an array of transposed matrices.
        //
        // Parameters:
        //   parameter:
        //     Unique Microsoft.DirectX.Direct3D.EffectHandle identifier.
        //
        //   count:
        //     Number of matrices in the returned array.
        //
        // Returns:
        //     Array of transposed Microsoft.DirectX.Matrix.
        Matrix[] GetValueMatrixTransposeArray(EffectHandle parameter, int count);
        //
        // Summary:
        //     Retrieves a pixel shader.
        //
        // Parameters:
        //   parameter:
        //     Unique Microsoft.DirectX.Direct3D.EffectHandle identifier.
        //
        // Returns:
        //     A Microsoft.DirectX.Direct3D.PixelShader object.
        PixelShader GetValuePixelShader(EffectHandle parameter);
        //
        // Summary:
        //     Retrieves a string.
        //
        // Parameters:
        //   parameter:
        //     Unique Microsoft.DirectX.Direct3D.EffectHandle identifier.
        //
        // Returns:
        //     String identified by the effect handle Microsoft.DirectX.Direct3D.BaseEffect.GetValueString(Microsoft.DirectX.Direct3D.EffectHandle).
        string GetValueString(EffectHandle parameter);
        //
        // Summary:
        //     Retrieves a texture.
        //
        // Parameters:
        //   parameter:
        //     Unique Microsoft.DirectX.Direct3D.EffectHandle identifier.
        //
        // Returns:
        //     A Microsoft.DirectX.Direct3D.Texture object.
        ITexture GetValueTexture(EffectHandle parameter);
        //
        // Summary:
        //     Retrieves a vector.
        //
        // Parameters:
        //   parameter:
        //     Unique Microsoft.DirectX.Direct3D.EffectHandle identifier.
        //
        // Returns:
        //     A Microsoft.DirectX.Vector4 object.
        Vector4 GetValueVector(EffectHandle parameter);
        //
        // Summary:
        //     Retrieves an array of vectors.
        //
        // Parameters:
        //   parameter:
        //     Unique Microsoft.DirectX.Direct3D.EffectHandle identifier.
        //
        //   count:
        //     Number of vectors in the returned array.
        //
        // Returns:
        //     Array of Microsoft.DirectX.Vector4 objects.
        Vector4[] GetValueVectorArray(EffectHandle parameter, int count);
        //
        // Summary:
        //     Retrieves a vertex shader.
        //
        // Parameters:
        //   parameter:
        //     Unique Microsoft.DirectX.Direct3D.EffectHandle identifier.
        //
        // Returns:
        //     A Microsoft.DirectX.Direct3D.VertexShader object.
        VertexShader GetValueVertexShader(EffectHandle parameter);
        //
        // Summary:
        //     Determines whether a parameter is used by a technique.
        //
        // Parameters:
        //   parameter:
        //     An Microsoft.DirectX.Direct3D.EffectHandle object that acts as a unique identifier
        //     for the parameter.
        //
        //   technique:
        //     An Microsoft.DirectX.Direct3D.EffectHandle object that acts as a unique identifier
        //     for the technique.
        //
        // Returns:
        //     Value that is true if the parameter is being used, or false if it is not
        //     being used.
        bool IsParameterUsed(EffectHandle parameter, EffectHandle technique);
        //
        // Summary:
        //     Validates a technique.
        //
        // Parameters:
        //   technique:
        //     The Microsoft.DirectX.Direct3D.EffectHandle of a technique.
        //
        // Returns:
        //     A value that is set to true, if the technique is valid; otherwise, true.
        bool IsTechniqueValid(EffectHandle technique);
        //
        // Summary:
        //     Validates a technique.
        //
        // Parameters:
        //   technique:
        //     The Microsoft.DirectX.Direct3D.EffectHandle of a technique.
        //
        //   returnValue:
        //     Return code for validation.
        //
        // Returns:
        //     A value that is set to true, if the technique is valid; otherwise, true.
        bool IsTechniqueValid(EffectHandle technique, out int returnValue);
        //
        // Summary:
        //     Releases all references to video memory resources and deletes all state blocks.
        void OnLostDevice();
        //
        // Summary:
        //     Should be called after a device is reset and before any other methods are
        //     called, if Microsoft.DirectX.Direct3D.Device.IsUsingEventHandlers is set
        //     to false.
        void OnResetDevice();
        //
        // Summary:
        //     Sets the range of an array to pass to the device.
        //
        // Parameters:
        //   parameter:
        //     An Microsoft.DirectX.Direct3D.EffectHandle object that represents the effect.
        //
        //   start:
        //     The array starting index.
        //
        //   end:
        //     The array ending index.
        void SetArrayRange(EffectHandle parameter, int start, int end);
        void SetRawValue(EffectHandle parameter, GraphicsStream data, int byteOffset);
        //
        // Summary:
        //     Sets the value of an arbitrary parameter or annotation, including simple
        //     types, structures, arrays, strings, shaders, and textures.
        //
        // Parameters:
        //   parameter:
        //     Unique Microsoft.DirectX.Direct3D.EffectHandle identifier.
        //
        //   texture:
        //     A Microsoft.DirectX.Direct3D.BaseTexture object to set.
        void SetValue(EffectHandle parameter, ITexture texture);
        //
        // Summary:
        //     Sets the value of an arbitrary parameter or annotation, including simple
        //     types, structures, arrays, strings, shaders, and textures.
        //
        // Parameters:
        //   parameter:
        //     Unique Microsoft.DirectX.Direct3D.EffectHandle identifier.
        //
        //   b:
        //     A System.Boolean value to set.
        void SetValue(EffectHandle parameter, bool b);
        //
        // Summary:
        //     Sets the value of an arbitrary parameter or annotation, including simple
        //     types, structures, arrays, strings, shaders, and textures.
        //
        // Parameters:
        //   parameter:
        //     Unique Microsoft.DirectX.Direct3D.EffectHandle identifier.
        //
        //   b:
        //     Array of System.Boolean values to set.
        void SetValue(EffectHandle parameter, bool[] b);
        //
        // Summary:
        //     Sets the value of an arbitrary parameter or annotation, including simple
        //     types, structures, arrays, strings, shaders, and textures.
        //
        // Parameters:
        //   parameter:
        //     Unique Microsoft.DirectX.Direct3D.EffectHandle identifier.
        //
        //   color:
        //     A Microsoft.DirectX.Direct3D.ColorValue structure to set.
        void SetValue(EffectHandle parameter, ColorValue color);
        //
        // Summary:
        //     Sets the value of an arbitrary parameter or annotation, including simple
        //     types, structures, arrays, strings, shaders, and textures.
        //
        // Parameters:
        //   parameter:
        //     Unique Microsoft.DirectX.Direct3D.EffectHandle identifier.
        //
        //   color:
        //     Array of Microsoft.DirectX.Direct3D.ColorValue structures to set.
        void SetValue(EffectHandle parameter, ColorValue[] color);
        //
        // Summary:
        //     Sets the value of an arbitrary parameter or annotation, including simple
        //     types, structures, arrays, strings, shaders, and textures.
        //
        // Parameters:
        //   parameter:
        //     Unique Microsoft.DirectX.Direct3D.EffectHandle identifier.
        //
        //   f:
        //     A System.Single value to set.
        void SetValue(EffectHandle parameter, float f);
        //
        // Summary:
        //     Sets the value of an arbitrary parameter or annotation, including simple
        //     types, structures, arrays, strings, shaders, and textures.
        //
        // Parameters:
        //   parameter:
        //     Unique Microsoft.DirectX.Direct3D.EffectHandle identifier.
        //
        //   f:
        //     Array of System.Single values to set.
        void SetValue(EffectHandle parameter, float[] f);
        //
        // Summary:
        //     Sets the value of an arbitrary parameter or annotation, including simple
        //     types, structures, arrays, strings, shaders, and textures.
        //
        // Parameters:
        //   parameter:
        //     Unique Microsoft.DirectX.Direct3D.EffectHandle identifier.
        //
        //   data:
        //     A Microsoft.DirectX.GraphicsStream object that contains the data to set.
        void SetValue(EffectHandle parameter, GraphicsStream data);
        //
        // Summary:
        //     Sets the value of an arbitrary parameter or annotation, including simple
        //     types, structures, arrays, strings, shaders, and textures.
        //
        // Parameters:
        //   parameter:
        //     Unique Microsoft.DirectX.Direct3D.EffectHandle identifier.
        //
        //   n:
        //     An System.Int32 value to set.
        void SetValue(EffectHandle parameter, int n);
        //
        // Summary:
        //     Sets the value of an arbitrary parameter or annotation, including simple
        //     types, structures, arrays, strings, shaders, and textures.
        //
        // Parameters:
        //   parameter:
        //     Unique Microsoft.DirectX.Direct3D.EffectHandle identifier.
        //
        //   n:
        //     Array of System.Int32 values to set.
        void SetValue(EffectHandle parameter, int[] n);
        //
        // Summary:
        //     Sets the value of an arbitrary parameter or annotation, including simple
        //     types, structures, arrays, strings, shaders, and textures.
        //
        // Parameters:
        //   parameter:
        //     Unique Microsoft.DirectX.Direct3D.EffectHandle identifier.
        //
        //   matrix:
        //     A Microsoft.DirectX.Matrix structure to set.
        void SetValue(EffectHandle parameter, Matrix matrix);
        //
        // Summary:
        //     Sets the value of an arbitrary parameter or annotation, including simple
        //     types, structures, arrays, strings, shaders, and textures.
        //
        // Parameters:
        //   parameter:
        //     Unique Microsoft.DirectX.Direct3D.EffectHandle identifier.
        //
        //   matrix:
        //     Array of Microsoft.DirectX.Matrix structures to set.
        void SetValue(EffectHandle parameter, Matrix[] matrix);
        //
        // Summary:
        //     Sets the value of an arbitrary parameter or annotation, including simple
        //     types, structures, arrays, strings, shaders, and textures.
        //
        // Parameters:
        //   parameter:
        //     Unique Microsoft.DirectX.Direct3D.EffectHandle identifier.
        //
        //   str:
        //     A System.String to set.
        void SetValue(EffectHandle parameter, string str);
        //
        // Summary:
        //     Sets the value of an arbitrary parameter or annotation, including simple
        //     types, structures, arrays, strings, shaders, and textures.
        //
        // Parameters:
        //   parameter:
        //     Unique Microsoft.DirectX.Direct3D.EffectHandle identifier.
        //
        //   vector:
        //     A Microsoft.DirectX.Vector4 structure to set.
        void SetValue(EffectHandle parameter, Vector4 vector);
        //
        // Summary:
        //     Sets the value of an arbitrary parameter or annotation, including simple
        //     types, structures, arrays, strings, shaders, and textures.
        //
        // Parameters:
        //   parameter:
        //     Unique Microsoft.DirectX.Direct3D.EffectHandle identifier.
        //
        //   vector:
        //     Array of Microsoft.DirectX.Vector4 structures to set.
        void SetValue(EffectHandle parameter, Vector4[] vector);
        //
        // Summary:
        //     Sets a transposed matrix.
        //
        // Parameters:
        //   parameter:
        //     Unique Microsoft.DirectX.Direct3D.EffectHandle identifier.
        //
        //   matrix:
        //     A Microsoft.DirectX.Matrix structure to set.
        void SetValueTranspose(EffectHandle parameter, Matrix matrix);
        //
        // Summary:
        //     Sets a transposed matrix.
        //
        // Parameters:
        //   parameter:
        //     Unique Microsoft.DirectX.Direct3D.EffectHandle identifier.
        //
        //   matrix:
        //     Array of Microsoft.DirectX.Matrix structures to set.
        void SetValueTranspose(EffectHandle parameter, Matrix[] matrix);
        //
        // Summary:
        //     Validates a technique.
        //
        // Parameters:
        //   technique:
        //     An Microsoft.DirectX.Direct3D.EffectHandle object used as a unique identifier
        //     for the effect.
        void ValidateTechnique(EffectHandle technique);
    }
}
