using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Dope.DDXX.Utility;

namespace Dope.DDXX.Graphics
{
    public class EffectConverter : IEffectConverter
    {
        public void Convert(Effect oldEffect, Effect newEffect)
        {
            oldEffect.CurrentTechnique.Passes[0].Apply();
            foreach (EffectParameter fromParameter in oldEffect.Parameters)
            {
                EffectParameter toParameter = newEffect.Parameters[fromParameter.Name];
                if (toParameter != null)
                {
                    switch (fromParameter.ParameterClass)
                    {
                        case EffectParameterClass.Vector:
                            switch (toParameter.ColumnCount)
                            {
                                case 2:
                                    if (fromParameter.Elements.Count > 1)
                                    {
                                        toParameter.SetValue(fromParameter.GetValueVector2Array());
                                    }
                                    else
                                    {
                                        toParameter.SetValue(fromParameter.GetValueVector2());
                                    }
                                    break;
                                case 3:
                                    if (fromParameter.Elements.Count > 1)
                                    {
                                        toParameter.SetValue(fromParameter.GetValueVector3Array());
                                    }
                                    else
                                    {
                                        toParameter.SetValue(fromParameter.GetValueVector3());
                                    }
                                    break;
                                case 4:
                                    if (fromParameter.Elements.Count > 1)
                                    {
                                        toParameter.SetValue(fromParameter.GetValueVector4Array());
                                    }
                                    else
                                    {
                                        toParameter.SetValue(fromParameter.GetValueVector4());
                                    }
                                    break;
                                default:
                                    throw new DDXXException("Unhandled case!");
                            }
                            break;
                        case EffectParameterClass.Matrix:
                            if (fromParameter.Elements.Count > 1)
                            {
                                toParameter.SetValue(fromParameter.GetValueMatrixArray(Math.Min(toParameter.Elements.Count, fromParameter.Elements.Count)));
                            }
                            else
                            {
                                if (toParameter.Elements.Count > 1)
                                {
                                    toParameter.SetValue(new Microsoft.Xna.Framework.Matrix[] { fromParameter.GetValueMatrix() });
                                }
                                else
                                {
                                    toParameter.SetValue(fromParameter.GetValueMatrix());
                                }
                            }
                            break;
                        case EffectParameterClass.Scalar:
                            if (fromParameter.Elements.Count > 1)
                            {
                                toParameter.SetValue(fromParameter.GetValueSingleArray());
                            }
                            else
                            {
                                toParameter.SetValue(fromParameter.GetValueSingle());
                            }
                            break;
                        case EffectParameterClass.Object:
                            if (fromParameter.ParameterType == EffectParameterType.Texture2D)
                            {
                                toParameter.SetValue(fromParameter.GetValueTexture2D());
                            }
                            else if (fromParameter.ParameterType == EffectParameterType.TextureCube)
                            {
                                toParameter.SetValue(fromParameter.GetValueTextureCube());
                            }
                            else
                            {
                                throw new DDXXException("Unsupported effect parameter type");
                            }
                            break;
                        default:
                            throw new DDXXException("Unsupported effect parameter class");
                    }
                    //switch (fromParameter.ParameterType)
                    //{
                    //    case EffectParameterType.Single:
                    //        if (toParameter.ParameterClass == EffectParameterClass.Vector)
                    //        {
                    //            switch (toParameter.ColumnCount)
                    //            {
                    //                case 2:
                    //                    if (toParameter.Elements.Count > 1)
                    //                    {
                    //                        toParameter.SetValue(fromParameter.GetValueVector2Array());
                    //                    } else
                    //                    {
                    //                        toParameter.SetValue(fromParameter.GetValueVector2());
                    //                    }
                    //                    break;
                    //                case 3:
                    //                    if (toParameter.Elements.Count > 1)
                    //                    {
                    //                        toParameter.SetValue(fromParameter.GetValueVector3Array());
                    //                    }
                    //                    else
                    //                    {
                    //                        toParameter.SetValue(fromParameter.GetValueVector3());
                    //                    }
                    //                    break;
                    //                case 4:
                    //                    if (toParameter.Elements.Count > 1)
                    //                    {
                    //                        toParameter.SetValue(fromParameter.GetValueVector4Array());
                    //                    }
                    //                    else
                    //                    {
                    //                        toParameter.SetValue(fromParameter.GetValueVector4());
                    //                    }
                    //                    break;
                    //                default:
                    //                    throw new DDXXException("Unhandled case!");
                    //            }
                    //        }
                    //        else
                    //        {
                    //            if (toParameter.Elements.Count > 1)
                    //            {
                    //                toParameter.SetValue(fromParameter.GetValueSingleArray());
                    //            }
                    //            else
                    //            {
                    //                toParameter.SetValue(fromParameter.GetValueSingle());
                    //            }
                    //        }
                    //        break;
                    //    case EffectParameterType.Texture2D:
                    //        toParameter.SetValue(fromParameter.GetValueTexture2D());
                    //        break;
                    //    case EffectParameterType.TextureCube:
                    //        toParameter.SetValue(fromParameter.GetValueTextureCube());
                    //        break;
                    //    default:
                    //        throw new DDXXException("Invalid type.");
                    //}
                }
            }
        }
    }
}
