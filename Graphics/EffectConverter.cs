using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Dope.DDXX.Utility;

namespace Dope.DDXX.Graphics
{
    public class EffectConverter : IEffectConverter
    {
        public void Convert(IEffect oldEffect, IEffect newEffect)
        {
            foreach (IEffectParameter fromParameter in oldEffect.Parameters)
            {
                IEffectParameter toParameter = newEffect.Parameters[fromParameter.Name];
                if (toParameter != null)
                {
                    switch (fromParameter.ParameterType)
                    {
                        case EffectParameterType.Single:
                            toParameter.SetValue(fromParameter.GetValueSingleArray(fromParameter.ColumnCount * fromParameter.RowCount));
                            break;
                        case EffectParameterType.Texture2D:
                            toParameter.SetValue(fromParameter.GetValueTexture2D());
                            break;
                        case EffectParameterType.TextureCube:
                            toParameter.SetValue(fromParameter.GetValueTextureCube());
                            break;
                        case EffectParameterType.Sampler2D:
                            break;
                        default:
                            throw new DDXXException("Invalid type.");
                    }
                }
            }
        }
    }
}
