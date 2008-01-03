using System;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.Graphics
{
    public class RenderStateAdapter : IRenderState
    {
        private RenderState renderState;

        public RenderStateAdapter(RenderState renderState)
        {
            this.renderState = renderState;
        }

        #region IRenderState Members

        public bool AlphaBlendEnable
        {
            get
            {
                return renderState.AlphaBlendEnable;
            }
            set
            {
                renderState.AlphaBlendEnable = value;
            }
        }

        public BlendFunction AlphaBlendOperation
        {
            get
            {
                return renderState.AlphaBlendOperation;
            }
            set
            {
                renderState.AlphaBlendOperation = value;
            }
        }

        public Blend AlphaDestinationBlend
        {
            get
            {
                return renderState.AlphaDestinationBlend;
            }
            set
            {
                renderState.AlphaDestinationBlend = value;
            }
        }

        public CompareFunction AlphaFunction
        {
            get
            {
                return renderState.AlphaFunction;
            }
            set
            {
                renderState.AlphaFunction = value;
            }
        }

        public Blend AlphaSourceBlend
        {
            get
            {
                return renderState.AlphaSourceBlend;
            }
            set
            {
                renderState.AlphaSourceBlend = value;
            }
        }

        public bool AlphaTestEnable
        {
            get
            {
                return renderState.AlphaTestEnable;
            }
            set
            {
                renderState.AlphaTestEnable = value;
            }
        }

        public Color BlendFactor
        {
            get
            {
                return renderState.BlendFactor;
            }
            set
            {
                renderState.BlendFactor = value;
            }
        }

        public BlendFunction BlendFunction
        {
            get
            {
                return renderState.BlendFunction;
            }
            set
            {
                renderState.BlendFunction = value;
            }
        }

        public ColorWriteChannels ColorWriteChannels
        {
            get
            {
                return renderState.ColorWriteChannels;
            }
            set
            {
                renderState.ColorWriteChannels = value;
            }
        }

        public ColorWriteChannels ColorWriteChannels1
        {
            get
            {
                return renderState.ColorWriteChannels1;
            }
            set
            {
                renderState.ColorWriteChannels1 = value;
            }
        }

        public ColorWriteChannels ColorWriteChannels2
        {
            get
            {
                return renderState.ColorWriteChannels2;
            }
            set
            {
                renderState.ColorWriteChannels2 = value;
            }
        }

        public ColorWriteChannels ColorWriteChannels3
        {
            get
            {
                return renderState.ColorWriteChannels3;
            }
            set
            {
                renderState.ColorWriteChannels3 = value;
            }
        }

        public StencilOperation CounterClockwiseStencilDepthBufferFail
        {
            get
            {
                return renderState.CounterClockwiseStencilDepthBufferFail;
            }
            set
            {
                renderState.CounterClockwiseStencilDepthBufferFail = value;
            }
        }

        public StencilOperation CounterClockwiseStencilFail
        {
            get
            {
                return renderState.CounterClockwiseStencilFail;
            }
            set
            {
                renderState.CounterClockwiseStencilFail = value;
            }
        }

        public CompareFunction CounterClockwiseStencilFunction
        {
            get
            {
                return renderState.CounterClockwiseStencilFunction;
            }
            set
            {
                renderState.CounterClockwiseStencilFunction = value;
            }
        }

        public StencilOperation CounterClockwiseStencilPass
        {
            get
            {
                return renderState.CounterClockwiseStencilPass;
            }
            set
            {
                renderState.CounterClockwiseStencilPass = value;
            }
        }

        public CullMode CullMode
        {
            get
            {
                return renderState.CullMode;
            }
            set
            {
                renderState.CullMode = value;
            }
        }

        public float DepthBias
        {
            get
            {
                return renderState.DepthBias;
            }
            set
            {
                renderState.DepthBias = value;
            }
        }

        public bool DepthBufferEnable
        {
            get
            {
                return renderState.DepthBufferEnable;
            }
            set
            {
                renderState.DepthBufferEnable = value;
            }
        }

        public CompareFunction DepthBufferFunction
        {
            get
            {
                return renderState.DepthBufferFunction;
            }
            set
            {
                renderState.DepthBufferFunction = value;
            }
        }

        public bool DepthBufferWriteEnable
        {
            get
            {
                return renderState.DepthBufferWriteEnable;
            }
            set
            {
                renderState.DepthBufferWriteEnable = value;
            }
        }

        public Blend DestinationBlend
        {
            get
            {
                return renderState.DestinationBlend;
            }
            set
            {
                renderState.DestinationBlend = value;
            }
        }

        public FillMode FillMode
        {
            get
            {
                return renderState.FillMode;
            }
            set
            {
                renderState.FillMode = value;
            }
        }

#if (!XBOX)
        public Color FogColor
        {
            get
            {
                return renderState.FogColor;
            }
            set
            {
                renderState.FogColor = value;
            }
        }

        public float FogDensity
        {
            get
            {
                return renderState.FogDensity;
            }
            set
            {
                renderState.FogDensity = value;
            }
        }

        public bool FogEnable
        {
            get
            {
                return renderState.FogEnable;
            }
            set
            {
                renderState.FogEnable = value;
            }
        }

        public float FogEnd
        {
            get
            {
                return renderState.FogEnd;
            }
            set
            {
                renderState.FogEnd = value;
            }
        }

        public float FogStart
        {
            get
            {
                return renderState.FogStart;
            }
            set
            {
                renderState.FogStart = value;
            }
        }

        public FogMode FogTableMode
        {
            get
            {
                return renderState.FogTableMode;
            }
            set
            {
                renderState.FogTableMode = value;
            }
        }

        public FogMode FogVertexMode
        {
            get
            {
                return renderState.FogVertexMode;
            }
            set
            {
                renderState.FogVertexMode = value;
            }
        }
#endif

        public bool MultiSampleAntiAlias
        {
            get
            {
                return renderState.MultiSampleAntiAlias;
            }
            set
            {
                renderState.MultiSampleAntiAlias = value;
            }
        }

        public int MultiSampleMask
        {
            get
            {
                return renderState.MultiSampleMask;
            }
            set
            {
                renderState.MultiSampleMask = value;
            }
        }

        public float PointSize
        {
            get
            {
                return renderState.PointSize;
            }
            set
            {
                renderState.PointSize = value;
            }
        }

        public float PointSizeMax
        {
            get
            {
                return renderState.PointSizeMax;
            }
            set
            {
                renderState.PointSizeMax = value;
            }
        }

        public float PointSizeMin
        {
            get
            {
                return renderState.PointSizeMin;
            }
            set
            {
                renderState.PointSizeMin = value;
            }
        }

        public bool PointSpriteEnable
        {
            get
            {
                return renderState.PointSpriteEnable;
            }
            set
            {
                renderState.PointSpriteEnable = value;
            }
        }

#if (!XBOX)
        public bool RangeFogEnable
        {
            get
            {
                return renderState.RangeFogEnable;
            }
            set
            {
                renderState.RangeFogEnable = value;
            }
        }
#endif

        public int ReferenceAlpha
        {
            get
            {
                return renderState.ReferenceAlpha;
            }
            set
            {
                renderState.ReferenceAlpha = value;
            }
        }

        public int ReferenceStencil
        {
            get
            {
                return renderState.ReferenceStencil;
            }
            set
            {
                renderState.ReferenceStencil = value;
            }
        }

        public bool ScissorTestEnable
        {
            get
            {
                return renderState.ScissorTestEnable;
            }
            set
            {
                renderState.ScissorTestEnable = value;
            }
        }

        public bool SeparateAlphaBlendEnabled
        {
            get
            {
                return renderState.SeparateAlphaBlendEnabled;
            }
            set
            {
                renderState.SeparateAlphaBlendEnabled = value;
            }
        }

        public float SlopeScaleDepthBias
        {
            get
            {
                return renderState.SlopeScaleDepthBias;
            }
            set
            {
                renderState.SlopeScaleDepthBias = value;
            }
        }

        public Blend SourceBlend
        {
            get
            {
                return renderState.SourceBlend;
            }
            set
            {
                renderState.SourceBlend = value;
            }
        }

        public StencilOperation StencilDepthBufferFail
        {
            get
            {
                return renderState.StencilDepthBufferFail;
            }
            set
            {
                renderState.StencilDepthBufferFail = value;
            }
        }

        public bool StencilEnable
        {
            get
            {
                return renderState.StencilEnable;
            }
            set
            {
                renderState.StencilEnable = value;
            }
        }

        public StencilOperation StencilFail
        {
            get
            {
                return renderState.StencilFail;
            }
            set
            {
                renderState.StencilFail = value;
            }
        }

        public CompareFunction StencilFunction
        {
            get
            {
                return renderState.StencilFunction;
            }
            set
            {
                renderState.StencilFunction = value;
            }
        }

        public int StencilMask
        {
            get
            {
                return renderState.StencilMask;
            }
            set
            {
                renderState.StencilMask = value;
            }
        }

        public StencilOperation StencilPass
        {
            get
            {
                return renderState.StencilPass;
            }
            set
            {
                renderState.StencilPass = value;
            }
        }

        public int StencilWriteMask
        {
            get
            {
                return renderState.StencilWriteMask;
            }
            set
            {
                renderState.StencilWriteMask = value;
            }
        }

        public bool TwoSidedStencilMode
        {
            get
            {
                return renderState.TwoSidedStencilMode;
            }
            set
            {
                renderState.TwoSidedStencilMode = value;
            }
        }

        public TextureWrapCoordinates Wrap0
        {
            get
            {
                return renderState.Wrap0;
            }
            set
            {
                renderState.Wrap0 = value;
            }
        }

        public TextureWrapCoordinates Wrap1
        {
            get
            {
                return renderState.Wrap1;
            }
            set
            {
                renderState.Wrap1 = value;
            }
        }

        public TextureWrapCoordinates Wrap10
        {
            get
            {
                return renderState.Wrap10;
            }
            set
            {
                renderState.Wrap10 = value;
            }
        }

        public TextureWrapCoordinates Wrap11
        {
            get
            {
                return renderState.Wrap11;
            }
            set
            {
                renderState.Wrap11 = value;
            }
        }

        public TextureWrapCoordinates Wrap12
        {
            get
            {
                return renderState.Wrap12;
            }
            set
            {
                renderState.Wrap12 = value;
            }
        }

        public TextureWrapCoordinates Wrap13
        {
            get
            {
                return renderState.Wrap13;
            }
            set
            {
                renderState.Wrap13 = value;
            }
        }

        public TextureWrapCoordinates Wrap14
        {
            get
            {
                return renderState.Wrap14;
            }
            set
            {
                renderState.Wrap14 = value;
            }
        }

        public TextureWrapCoordinates Wrap15
        {
            get
            {
                return renderState.Wrap15;
            }
            set
            {
                renderState.Wrap15 = value;
            }
        }

        public TextureWrapCoordinates Wrap2
        {
            get
            {
                return renderState.Wrap2;
            }
            set
            {
                renderState.Wrap2 = value;
            }
        }

        public TextureWrapCoordinates Wrap3
        {
            get
            {
                return renderState.Wrap3;
            }
            set
            {
                renderState.Wrap3 = value;
            }
        }

        public TextureWrapCoordinates Wrap4
        {
            get
            {
                return renderState.Wrap4;
            }
            set
            {
                renderState.Wrap4 = value;
            }
        }

        public TextureWrapCoordinates Wrap5
        {
            get
            {
                return renderState.Wrap5;
            }
            set
            {
                renderState.Wrap5 = value;
            }
        }

        public TextureWrapCoordinates Wrap6
        {
            get
            {
                return renderState.Wrap6;
            }
            set
            {
                renderState.Wrap6 = value;
            }
        }

        public TextureWrapCoordinates Wrap7
        {
            get
            {
                return renderState.Wrap7;
            }
            set
            {
                renderState.Wrap7 = value;
            }
        }

        public TextureWrapCoordinates Wrap8
        {
            get
            {
                return renderState.Wrap8;
            }
            set
            {
                renderState.Wrap8 = value;
            }
        }

        public TextureWrapCoordinates Wrap9
        {
            get
            {
                return renderState.Wrap9;
            }
            set
            {
                renderState.Wrap9 = value;
            }
        }

        #endregion
    }
}
