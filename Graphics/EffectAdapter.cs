using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Dope.DDXX.Graphics
{
    public class EffectAdapter : IEffect
    {
        private Effect effect;

        public EffectAdapter(Effect effect)
        {
            this.effect = effect;
        }

        public static EffectAdapter FromFile(Device device, string sourceDataFile, Include includeFile, string skipConstants, ShaderFlags flags, EffectPool pool)
        {
            return new EffectAdapter(Effect.FromFile(device, sourceDataFile, includeFile, skipConstants, flags, pool));
        }
        public static EffectAdapter FromFile(Device device, string sourceDataFile, Include includeFile, string skipConstants, ShaderFlags flags, EffectPool pool, out string compilationErrors)
        {
            return new EffectAdapter(Effect.FromFile(device, sourceDataFile, includeFile, skipConstants, flags, pool, out compilationErrors));
        }
        public static EffectAdapter FromFile(Device device, string sourceDataFile, Macro[] preprocessorDefines, Include includeFile, string skipConstants, ShaderFlags flags, EffectPool pool, out string compilationErrors)
        {
            return new EffectAdapter(Effect.FromFile(device, sourceDataFile, preprocessorDefines, includeFile, skipConstants, flags, pool, out compilationErrors));
        }
        public static EffectAdapter FromStream(Device device, Stream data, Include includeFile, string skipConstants, ShaderFlags flags, EffectPool pool)
        {
            return new EffectAdapter(Effect.FromStream(device, data, includeFile, skipConstants, flags, pool));
        }
        public static EffectAdapter FromStream(Device device, Stream data, Include includeFile, string skipConstants, ShaderFlags flags, EffectPool pool, out string compilationErrors)
        {
            return new EffectAdapter(Effect.FromStream(device, data, includeFile, skipConstants, flags, pool, out compilationErrors));
        }
        public static EffectAdapter FromStream(Device device, Stream data, Macro[] preprocessorDefines, Include includeFile, string skipConstants, ShaderFlags flags, EffectPool pool, out string compilationErrors)
        {
            return new EffectAdapter(Effect.FromStream(device, data, preprocessorDefines, includeFile, skipConstants, flags, pool, out compilationErrors));
        }
        public static EffectAdapter FromString(Device device, string sourceData, Include includeFile, string skipConstants, ShaderFlags flags, EffectPool pool)
        {
            return new EffectAdapter(Effect.FromString(device, sourceData, includeFile, skipConstants, flags, pool));
        }
        public static EffectAdapter FromString(Device device, string sourceData, Include includeFile, string skipConstants, ShaderFlags flags, EffectPool pool, out string compilationErrors)
        {
            return new EffectAdapter(Effect.FromString(device, sourceData, includeFile, skipConstants, flags, pool, out compilationErrors));
        }
        public static EffectAdapter FromString(Device device, string sourceData, Macro[] preprocessorDefines, Include includeFile, string skipConstants, ShaderFlags flags, EffectPool pool, out string compilationErrors)
        {
            return new EffectAdapter(Effect.FromString(device, sourceData, preprocessorDefines, includeFile, skipConstants, flags, pool, out compilationErrors));
        }
        
        #region IEffect Members

        public Device Device
        {
            get { return effect.Device; }
        }

        public bool Disposed
        {
            get { return effect.Disposed; }
        }

        public EffectPool Pool
        {
            get { return effect.Pool; }
        }

        public EffectStateManager StateManager
        {
            get
            {
                return effect.StateManager;
            }
            set
            {
                effect.StateManager = value;
            }
        }

        public EffectHandle Technique
        {
            get
            {
                return effect.Technique;
            }
            set
            {
                effect.Technique = value;
            }
        }

        public void ApplyParameterBlock(EffectHandle parameterBlock)
        {
            effect.ApplyParameterBlock(parameterBlock);
        }

        public int Begin(FX flags)
        {
            return effect.Begin(flags);
        }

        public void BeginParameterBlock()
        {
            effect.BeginParameterBlock();
        }

        public void BeginPass(int passNumber)
        {
            effect.BeginPass(passNumber);
        }

        public Effect Clone(Device dev)
        {
            return effect.Clone(dev);
        }

        public void CommitChanges()
        {
            effect.CommitChanges();
        }

        public void DeleteParameterBlock(EffectHandle parameterBlock)
        {
            effect.DeleteParameterBlock(parameterBlock);
        }

        public string Disassemble(bool enableColorCode)
        {
            return effect.Disassemble(enableColorCode);
        }

        public void Dispose()
        {
            effect.Dispose();
        }

        public void End()
        {
            effect.End();
        }

        public EffectHandle EndParameterBlock()
        {
            return effect.EndParameterBlock();
        }

        public void EndPass()
        {
            effect.EndPass();
        }

        public EffectHandle FindNextValidTechnique(EffectHandle technique)
        {
            return effect.FindNextValidTechnique(technique);
        }

        public bool IsParameterUsed(EffectHandle parameter, EffectHandle technique)
        {
            return effect.IsParameterUsed(parameter, technique);
        }

        public bool IsTechniqueValid(EffectHandle technique)
        {
            return effect.IsTechniqueValid(technique);
        }

        public bool IsTechniqueValid(EffectHandle technique, out int returnValue)
        {
            return effect.IsTechniqueValid(technique, out returnValue);
        }

        public void OnLostDevice()
        {
            effect.OnLostDevice();
        }

        public void OnResetDevice()
        {
            effect.OnResetDevice();
        }

        public void SetArrayRange(EffectHandle parameter, int start, int end)
        {
            effect.SetArrayRange(parameter, start, end);
        }

        public void SetRawValue(EffectHandle parameter, GraphicsStream data, int byteOffset)
        {
            effect.SetRawValue(parameter, data, byteOffset);
        }

        public void ValidateTechnique(EffectHandle technique)
        {
            effect.ValidateTechnique(technique);
        }

        public int Description_Parameters 
        {
            get { return effect.Description.Parameters; }
        }

        public EffectDescription Description
        {
            get { return effect.Description; }
        }

        public EffectHandle GetAnnotation(EffectHandle technique, int index)
        {
            return effect.GetAnnotation(technique, index);
        }

        public EffectHandle GetAnnotation(EffectHandle technique, string name)
        {
            return effect.GetAnnotation(technique, name);
        }

        public EffectHandle GetFunction(int index)
        {
            return effect.GetFunction(index);
        }

        public EffectHandle GetFunction(string name)
        {
            return effect.GetFunction(name);
        }

        public FunctionDescription GetFunctionDescription(EffectHandle shader)
        {
            return effect.GetFunctionDescription(shader);
        }

        public EffectHandle GetParameter(EffectHandle constant, int index)
        {
            return effect.GetParameter(constant, index);
        }

        public EffectHandle GetParameter(EffectHandle constant, string name)
        {
            return effect.GetParameter(constant, name);
        }

        public EffectHandle GetParameterBySemantic(EffectHandle constant, string name)
        {
            return effect.GetParameterBySemantic(constant, name);
        }

        public ParameterDescription GetParameterDescription(EffectHandle parameter)
        {
            return effect.GetParameterDescription(parameter);
        }

        public int GetParameterDescription_Elements(EffectHandle parameter)
        {
            return effect.GetParameterDescription(parameter).Elements;
        }

        public EffectHandle GetParameterElement(EffectHandle constant, int index)
        {
            return effect.GetParameterElement(constant, index);
        }

        public EffectHandle GetPass(EffectHandle technique, int index)
        {
            return effect.GetPass(technique, index);
        }

        public EffectHandle GetPass(EffectHandle technique, string name)
        {
            return effect.GetPass(technique, name);
        }

        public PassDescription GetPassDescription(EffectHandle pass)
        {
            return effect.GetPassDescription(pass);
        }

        public EffectHandle GetTechnique(int index)
        {
            return effect.GetTechnique(index);
        }

        public EffectHandle GetTechnique(string name)
        {
            return effect.GetTechnique(name);
        }

        public string GetTechniqueName(EffectHandle technique)
        {
            TechniqueDescription desc;
            desc = effect.GetTechniqueDescription(technique);
            return desc.Name;
        }

        public GraphicsStream GetValue(EffectHandle parameter, int numberBytes)
        {
            return effect.GetValue(parameter, numberBytes);
        }

        public bool GetValueBoolean(EffectHandle parameter)
        {
            return effect.GetValueBoolean(parameter);
        }

        public bool[] GetValueBooleanArray(EffectHandle parameter, int count)
        {
            return effect.GetValueBooleanArray(parameter, count);
        }

        public ColorValue GetValueColor(EffectHandle parameter)
        {
            return effect.GetValueColor(parameter);
        }

        public ColorValue[] GetValueColorArray(EffectHandle parameter, int count)
        {
            return effect.GetValueColorArray(parameter, count);
        }

        public float GetValueFloat(EffectHandle parameter)
        {
            return effect.GetValueFloat(parameter);
        }

        public float[] GetValueFloatArray(EffectHandle parameter, int count)
        {
            return effect.GetValueFloatArray(parameter, count);
        }

        public int GetValueInteger(EffectHandle parameter)
        {
            return effect.GetValueInteger(parameter);
        }

        public int[] GetValueIntegerArray(EffectHandle parameter, int count)
        {
            return effect.GetValueIntegerArray(parameter, count);
        }

        public Matrix GetValueMatrix(EffectHandle parameter)
        {
            return effect.GetValueMatrix(parameter);
        }

        public Matrix[] GetValueMatrixArray(EffectHandle parameter, int count)
        {
            return effect.GetValueMatrixArray(parameter, count);
        }

        public Matrix GetValueMatrixTranspose(EffectHandle parameter)
        {
            return effect.GetValueMatrixTranspose(parameter);
        }

        public Matrix[] GetValueMatrixTransposeArray(EffectHandle parameter, int count)
        {
            return effect.GetValueMatrixTransposeArray(parameter, count);
        }

        public PixelShader GetValuePixelShader(EffectHandle parameter)
        {
            return effect.GetValuePixelShader(parameter);
        }

        public string GetValueString(EffectHandle parameter)
        {
            return effect.GetValueString(parameter);
        }

        public ITexture GetValueTexture(EffectHandle parameter)
        {
            return new TextureAdapter(effect.GetValueTexture(parameter));
        }

        public Vector4 GetValueVector(EffectHandle parameter)
        {
            return effect.GetValueVector(parameter);
        }

        public Vector4[] GetValueVectorArray(EffectHandle parameter, int count)
        {
            return effect.GetValueVectorArray(parameter, count);
        }

        public VertexShader GetValueVertexShader(EffectHandle parameter)
        {
            return effect.GetValueVertexShader(parameter);
        }

        public void SetValue(EffectHandle parameter, ITexture texture)
        {
            if (texture == null)
                effect.SetValue(parameter, (BaseTexture)null);
            else
                effect.SetValue(parameter, ((TextureAdapter)texture).TextureDX);
        }

        public void SetValue(EffectHandle parameter, bool b)
        {
            effect.SetValue(parameter, b);
        }

        public void SetValue(EffectHandle parameter, bool[] b)
        {
            effect.SetValue(parameter, b);
        }

        public void SetValue(EffectHandle parameter, ColorValue color)
        {
            effect.SetValue(parameter, color);
        }

        public void SetValue(EffectHandle parameter, ColorValue[] color)
        {
            effect.SetValue(parameter, color);
        }

        public void SetValue(EffectHandle parameter, float f)
        {
            effect.SetValue(parameter, f);
        }

        public void SetValue(EffectHandle parameter, float[] f)
        {
            effect.SetValue(parameter, f);
        }

        public void SetValue(EffectHandle parameter, GraphicsStream data)
        {
            effect.SetValue(parameter, data);
        }

        public void SetValue(EffectHandle parameter, int n)
        {
            effect.SetValue(parameter, n);
        }

        public void SetValue(EffectHandle parameter, int[] n)
        {
            effect.SetValue(parameter, n);
        }

        public void SetValue(EffectHandle parameter, Matrix matrix)
        {
            effect.SetValue(parameter, matrix);
        }

        public void SetValue(EffectHandle parameter, Matrix[] matrix)
        {
            effect.SetValue(parameter, matrix);
        }

        public void SetValue(EffectHandle parameter, string str)
        {
            effect.SetValue(parameter, str);
        }

        public void SetValue(EffectHandle parameter, Vector4 vector)
        {
            effect.SetValue(parameter, vector);
        }

        public void SetValue(EffectHandle parameter, Vector4[] vector)
        {
            effect.SetValue(parameter, vector);
        }

        public void SetValueTranspose(EffectHandle parameter, Matrix matrix)
        {
            effect.SetValue(parameter, matrix);
        }

        public void SetValueTranspose(EffectHandle parameter, Matrix[] matrix)
        {
            effect.SetValue(parameter, matrix);
        }

        #endregion
    }
}
