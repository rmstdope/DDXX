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

        private EffectAdapter(Effect effect)
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

        #endregion
    }
}
