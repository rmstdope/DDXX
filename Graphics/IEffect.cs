using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Dope.DDXX.Graphics
{
    public interface IEffect
    {
        Device Device { get; }
        bool Disposed { get; }
        EffectPool Pool { get; }
        EffectStateManager StateManager { get; set; }
        EffectHandle Technique { get; set; }
        void ApplyParameterBlock(EffectHandle parameterBlock);
        int Begin(FX flags);
        void BeginParameterBlock();
        void BeginPass(int passNumber);
        Effect Clone(Device dev);
        void CommitChanges();
        void DeleteParameterBlock(EffectHandle parameterBlock);
        string Disassemble(bool enableColorCode);
        void Dispose();
        void End();
        EffectHandle EndParameterBlock();
        void EndPass();
        EffectHandle FindNextValidTechnique(EffectHandle technique);
        bool IsParameterUsed(EffectHandle parameter, EffectHandle technique);
        bool IsTechniqueValid(EffectHandle technique);
        bool IsTechniqueValid(EffectHandle technique, out int returnValue);
        void OnLostDevice();
        void OnResetDevice();
        void SetArrayRange(EffectHandle parameter, int start, int end);
        void SetRawValue(EffectHandle parameter, GraphicsStream data, int byteOffset);
        void ValidateTechnique(EffectHandle technique);
    }
}
