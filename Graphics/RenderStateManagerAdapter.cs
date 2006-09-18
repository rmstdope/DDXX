using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;
using System.Drawing;

namespace Dope.DDXX.Graphics
{
    public class RenderStateManagerAdapter : IRenderStateManager
    {
        private RenderStateManager renderStateManager;

        public RenderStateManagerAdapter(RenderStateManager renderStateManager)
        {
            this.renderStateManager = renderStateManager;
        }

        #region IRenderStateManager Members

        public float AdaptiveTessellateW
        {
            get
            {
                return renderStateManager.AdaptiveTessellateW;
            }
            set
            {
                renderStateManager.AdaptiveTessellateW = value;
            }
        }

        public float AdaptiveTessellateX
        {
            get
            {
                return renderStateManager.AdaptiveTessellateX;
            }
            set
            {
                renderStateManager.AdaptiveTessellateX = value;
            }
        }

        public float AdaptiveTessellateY
        {
            get
            {
                return renderStateManager.AdaptiveTessellateY;
            }
            set
            {
                renderStateManager.AdaptiveTessellateY = value;
            }
        }

        public float AdaptiveTessellateZ
        {
            get
            {
                return renderStateManager.AdaptiveTessellateZ;
            }
            set
            {
                renderStateManager.AdaptiveTessellateZ = value;
            }
        }

        public bool AlphaBlendEnable
        {
            get
            {
                return renderStateManager.AlphaBlendEnable;
            }
            set
            {
                renderStateManager.AlphaBlendEnable = value;
            }
        }

        public BlendOperation AlphaBlendOperation
        {
            get
            {
                return renderStateManager.AlphaBlendOperation;
            }
            set
            {
                renderStateManager.AlphaBlendOperation = value;
            }
        }

        public Blend AlphaDestinationBlend
        {
            get
            {
                return renderStateManager.AlphaDestinationBlend;
            }
            set
            {
                renderStateManager.AlphaDestinationBlend = value;
            }
        }

        public Compare AlphaFunction
        {
            get
            {
                return renderStateManager.AlphaFunction;
            }
            set
            {
                renderStateManager.AlphaFunction = value;
            }
        }

        public Blend AlphaSourceBlend
        {
            get
            {
                return renderStateManager.AlphaSourceBlend;
            }
            set
            {
                renderStateManager.AlphaSourceBlend = value;
            }
        }

        public bool AlphaTestEnable
        {
            get
            {
                return renderStateManager.AlphaTestEnable;
            }
            set
            {
                renderStateManager.AlphaTestEnable = value;
            }
        }

        public Color Ambient
        {
            get
            {
                return renderStateManager.Ambient;
            }
            set
            {
                renderStateManager.Ambient = value;
            }
        }

        public int AmbientColor
        {
            get
            {
                return renderStateManager.AmbientColor;
            }
            set
            {
                renderStateManager.AmbientColor = value;
            }
        }

        public ColorSource AmbientMaterialSource
        {
            get
            {
                return renderStateManager.AmbientMaterialSource;
            }
            set
            {
                renderStateManager.AmbientMaterialSource = value;
            }
        }

        public bool AntiAliasedLineEnable
        {
            get
            {
                return renderStateManager.AntiAliasedLineEnable;
            }
            set
            {
                renderStateManager.AntiAliasedLineEnable = value;
            }
        }

        public Color BlendFactor
        {
            get
            {
                return renderStateManager.BlendFactor;
            }
            set
            {
                renderStateManager.BlendFactor = value;
            }
        }

        public int BlendFactorColor
        {
            get
            {
                return renderStateManager.BlendFactorColor;
            }
            set
            {
                renderStateManager.BlendFactorColor = value;
            }
        }

        public BlendOperation BlendOperation
        {
            get
            {
                return renderStateManager.BlendOperation;
            }
            set
            {
                renderStateManager.BlendOperation = value;
            }
        }

        public bool Clipping
        {
            get
            {
                return renderStateManager.Clipping;
            }
            set
            {
                renderStateManager.Clipping = value;
            }
        }

        public bool ColorVertex
        {
            get
            {
                return renderStateManager.ColorVertex;
            }
            set
            {
                renderStateManager.ColorVertex = value;
            }
        }

        public ColorWriteEnable ColorWriteEnable
        {
            get
            {
                return renderStateManager.ColorWriteEnable;
            }
            set
            {
                renderStateManager.ColorWriteEnable = value;
            }
        }

        public ColorWriteEnable ColorWriteEnable1
        {
            get
            {
                return renderStateManager.ColorWriteEnable1;
            }
            set
            {
                renderStateManager.ColorWriteEnable1 = value;
            }
        }

        public ColorWriteEnable ColorWriteEnable2
        {
            get
            {
                return renderStateManager.ColorWriteEnable2;
            }
            set
            {
                renderStateManager.ColorWriteEnable2 = value;
            }
        }

        public ColorWriteEnable ColorWriteEnable3
        {
            get
            {
                return renderStateManager.ColorWriteEnable3;
            }
            set
            {
                renderStateManager.ColorWriteEnable3 = value;
            }
        }

        public StencilOperation CounterClockwiseStencilFail
        {
            get
            {
                return renderStateManager.CounterClockwiseStencilFail;
            }
            set
            {
                renderStateManager.CounterClockwiseStencilFail = value;
            }
        }

        public Compare CounterClockwiseStencilFunction
        {
            get
            {
                return renderStateManager.CounterClockwiseStencilFunction;
            }
            set
            {
                renderStateManager.CounterClockwiseStencilFunction = value;
            }
        }

        public StencilOperation CounterClockwiseStencilPass
        {
            get
            {
                return renderStateManager.CounterClockwiseStencilPass;
            }
            set
            {
                renderStateManager.CounterClockwiseStencilPass = value;
            }
        }

        public StencilOperation CounterClockwiseStencilZBufferFail
        {
            get
            {
                return renderStateManager.CounterClockwiseStencilZBufferFail;
            }
            set
            {
                renderStateManager.CounterClockwiseStencilZBufferFail = value;
            }
        }

        public Cull CullMode
        {
            get
            {
                return renderStateManager.CullMode;
            }
            set
            {
                renderStateManager.CullMode = value;
            }
        }

        public bool DebugMonitorTokenEnabled
        {
            get
            {
                return renderStateManager.DebugMonitorTokenEnabled;
            }
            set
            {
                renderStateManager.DebugMonitorTokenEnabled = value;
            }
        }

        public float DepthBias
        {
            get
            {
                return renderStateManager.DepthBias;
            }
            set
            {
                renderStateManager.DepthBias = value;
            }
        }

        public Blend DestinationBlend
        {
            get
            {
                return renderStateManager.DestinationBlend;
            }
            set
            {
                renderStateManager.DestinationBlend = value;
            }
        }

        public ColorSource DiffuseMaterialSource
        {
            get
            {
                return renderStateManager.DiffuseMaterialSource;
            }
            set
            {
                renderStateManager.DiffuseMaterialSource = value;
            }
        }

        public bool DitherEnable
        {
            get
            {
                return renderStateManager.DitherEnable;
            }
            set
            {
                renderStateManager.DitherEnable = value;
            }
        }

        public ColorSource EmissiveMaterialSource
        {
            get
            {
                return renderStateManager.EmissiveMaterialSource;
            }
            set
            {
                renderStateManager.EmissiveMaterialSource = value;
            }
        }

        public bool EnableAdaptiveTessellation
        {
            get
            {
                return renderStateManager.EnableAdaptiveTessellation;
            }
            set
            {
                renderStateManager.EnableAdaptiveTessellation = value;
            }
        }

        public FillMode FillMode
        {
            get
            {
                return renderStateManager.FillMode;
            }
            set
            {
                renderStateManager.FillMode = value;
            }
        }

        public Color FogColor
        {
            get
            {
                return renderStateManager.FogColor;
            }
            set
            {
                renderStateManager.FogColor = value;
            }
        }

        public int FogColorValue
        {
            get
            {
                return renderStateManager.FogColorValue;
            }
            set
            {
                renderStateManager.FogColorValue = value;
            }
        }

        public float FogDensity
        {
            get
            {
                return renderStateManager.FogDensity;
            }
            set
            {
                renderStateManager.FogDensity = value;
            }
        }

        public bool FogEnable
        {
            get
            {
                return renderStateManager.FogEnable;
            }
            set
            {
                renderStateManager.FogEnable = value;
            }
        }

        public float FogEnd
        {
            get
            {
                return renderStateManager.FogEnd;
            }
            set
            {
                renderStateManager.FogEnd = value;
            }
        }

        public float FogStart
        {
            get
            {
                return renderStateManager.FogStart;
            }
            set
            {
                renderStateManager.FogStart = value;
            }
        }

        public FogMode FogTableMode
        {
            get
            {
                return renderStateManager.FogTableMode;
            }
            set
            {
                renderStateManager.FogTableMode = value;
            }
        }

        public FogMode FogVertexMode
        {
            get
            {
                return renderStateManager.FogVertexMode;
            }
            set
            {
                renderStateManager.FogVertexMode = value;
            }
        }

        public bool IndexedVertexBlendEnable
        {
            get
            {
                return renderStateManager.IndexedVertexBlendEnable;
            }
            set
            {
                renderStateManager.IndexedVertexBlendEnable = value;
            }
        }

        public bool LastPixel
        {
            get
            {
                return renderStateManager.LastPixel;
            }
            set
            {
                renderStateManager.LastPixel = value;
            }
        }

        public bool Lighting
        {
            get
            {
                return renderStateManager.Lighting;
            }
            set
            {
                renderStateManager.Lighting = value;
            }
        }

        public bool LocalViewer
        {
            get
            {
                return renderStateManager.LocalViewer;
            }
            set
            {
                renderStateManager.LocalViewer = value;
            }
        }

        public float MaxTessellationLevel
        {
            get
            {
                return renderStateManager.MaxTessellationLevel;
            }
            set
            {
                renderStateManager.MaxTessellationLevel = value;
            }
        }

        public float MinTessellationLevel
        {
            get
            {
                return renderStateManager.MinTessellationLevel;
            }
            set
            {
                renderStateManager.MinTessellationLevel = value;
            }
        }

        public bool MultiSampleAntiAlias
        {
            get
            {
                return renderStateManager.MultiSampleAntiAlias;
            }
            set
            {
                renderStateManager.MultiSampleAntiAlias = value;
            }
        }

        public int MultiSampleMask
        {
            get
            {
                return renderStateManager.MultiSampleMask;
            }
            set
            {
                renderStateManager.MultiSampleMask = value;
            }
        }

        public DegreeType NormalDegree
        {
            get
            {
                return renderStateManager.NormalDegree;
            }
            set
            {
                renderStateManager.NormalDegree = value;
            }
        }

        public bool NormalizeNormals
        {
            get
            {
                return renderStateManager.NormalizeNormals;
            }
            set
            {
                renderStateManager.NormalizeNormals = value;
            }
        }

        public PatchEdge PatchEdgeStyle
        {
            get
            {
                return renderStateManager.PatchEdgeStyle;
            }
            set
            {
                renderStateManager.PatchEdgeStyle = value;
            }
        }

        public float PointScaleA
        {
            get
            {
                return renderStateManager.PointScaleA;
            }
            set
            {
                renderStateManager.PointScaleA = value;
            }
        }

        public float PointScaleB
        {
            get
            {
                return renderStateManager.PointScaleB;
            }
            set
            {
                renderStateManager.PointScaleB = value;
            }
        }

        public float PointScaleC
        {
            get
            {
                return renderStateManager.PointScaleC;
            }
            set
            {
                renderStateManager.PointScaleC = value;
            }
        }

        public bool PointScaleEnable
        {
            get
            {
                return renderStateManager.PointScaleEnable;
            }
            set
            {
                renderStateManager.PointScaleEnable = value;
            }
        }

        public float PointSize
        {
            get
            {
                return renderStateManager.PointSize;
            }
            set
            {
                renderStateManager.PointSize = value;
            }
        }

        public float PointSizeMax
        {
            get
            {
                return renderStateManager.PointSizeMax;
            }
            set
            {
                renderStateManager.PointSizeMax = value;
            }
        }

        public float PointSizeMin
        {
            get
            {
                return renderStateManager.PointSizeMin;
            }
            set
            {
                renderStateManager.PointSizeMin = value;
            }
        }

        public bool PointSpriteEnable
        {
            get
            {
                return renderStateManager.PointSpriteEnable;
            }
            set
            {
                renderStateManager.PointSpriteEnable = value;
            }
        }

        public DegreeType PositionDegree
        {
            get
            {
                return renderStateManager.PositionDegree;
            }
            set
            {
                renderStateManager.PositionDegree = value;
            }
        }

        public bool RangeFogEnable
        {
            get
            {
                return renderStateManager.RangeFogEnable;
            }
            set
            {
                renderStateManager.RangeFogEnable = value;
            }
        }

        public int ReferenceAlpha
        {
            get
            {
                return renderStateManager.ReferenceAlpha;
            }
            set
            {
                renderStateManager.ReferenceAlpha = value;
            }
        }

        public int ReferenceStencil
        {
            get
            {
                return renderStateManager.ReferenceStencil;
            }
            set
            {
                renderStateManager.ReferenceStencil = value;
            }
        }

        public bool ScissorTestEnable
        {
            get
            {
                return renderStateManager.ScissorTestEnable;
            }
            set
            {
                renderStateManager.ScissorTestEnable = value;
            }
        }

        public bool SeparateAlphaBlendEnabled
        {
            get
            {
                return renderStateManager.SeparateAlphaBlendEnabled;
            }
            set
            {
                renderStateManager.SeparateAlphaBlendEnabled = value;
            }
        }

        public ShadeMode ShadeMode
        {
            get
            {
                return renderStateManager.ShadeMode;
            }
            set
            {
                renderStateManager.ShadeMode = value;
            }
        }

        public float SlopeScaleDepthBias
        {
            get
            {
                return renderStateManager.SlopeScaleDepthBias;
            }
            set
            {
                renderStateManager.SlopeScaleDepthBias = value;
            }
        }

        public Blend SourceBlend
        {
            get
            {
                return renderStateManager.SourceBlend;
            }
            set
            {
                renderStateManager.SourceBlend = value;
            }
        }

        public bool SpecularEnable
        {
            get
            {
                return renderStateManager.SpecularEnable;
            }
            set
            {
                renderStateManager.SpecularEnable = value;
            }
        }

        public ColorSource SpecularMaterialSource
        {
            get
            {
                return renderStateManager.SpecularMaterialSource;
            }
            set
            {
                renderStateManager.SpecularMaterialSource = value;
            }
        }

        public bool SrgbWriteEnable
        {
            get
            {
                return renderStateManager.SrgbWriteEnable;
            }
            set
            {
                renderStateManager.SrgbWriteEnable = value;
            }
        }

        public bool StencilEnable
        {
            get
            {
                return renderStateManager.StencilEnable;
            }
            set
            {
                renderStateManager.StencilEnable = value;
            }
        }

        public StencilOperation StencilFail
        {
            get
            {
                return renderStateManager.StencilFail;
            }
            set
            {
                renderStateManager.StencilFail = value;
            }
        }

        public Compare StencilFunction
        {
            get
            {
                return renderStateManager.StencilFunction;
            }
            set
            {
                renderStateManager.StencilFunction = value;
            }
        }

        public int StencilMask
        {
            get
            {
                return renderStateManager.StencilMask;
            }
            set
            {
                renderStateManager.StencilMask = value;
            }
        }

        public StencilOperation StencilPass
        {
            get
            {
                return renderStateManager.StencilPass;
            }
            set
            {
                renderStateManager.StencilPass = value;
            }
        }

        public int StencilWriteMask
        {
            get
            {
                return renderStateManager.StencilWriteMask;
            }
            set
            {
                renderStateManager.StencilWriteMask = value;
            }
        }

        public StencilOperation StencilZBufferFail
        {
            get
            {
                return renderStateManager.StencilZBufferFail;
            }
            set
            {
                renderStateManager.StencilZBufferFail = value;
            }
        }

        public int TextureFactor
        {
            get
            {
                return renderStateManager.TextureFactor;
            }
            set
            {
                renderStateManager.TextureFactor = value;
            }
        }

        public float TweenFactor
        {
            get
            {
                return renderStateManager.TweenFactor;
            }
            set
            {
                renderStateManager.TweenFactor = value;
            }
        }

        public bool TwoSidedStencilMode
        {
            get
            {
                return renderStateManager.TwoSidedStencilMode;
            }
            set
            {
                renderStateManager.TwoSidedStencilMode = value;
            }
        }

        public bool UseWBuffer
        {
            get
            {
                return renderStateManager.UseWBuffer;
            }
            set
            {
                renderStateManager.UseWBuffer = value;
            }
        }

        public VertexBlend VertexBlend
        {
            get
            {
                return renderStateManager.VertexBlend;
            }
            set
            {
                renderStateManager.VertexBlend = value;
            }
        }

        public WrapCoordinates Wrap0
        {
            get
            {
                return renderStateManager.Wrap0;
            }
            set
            {
                renderStateManager.Wrap0 = value;
            }
        }

        public WrapCoordinates Wrap1
        {
            get
            {
                return renderStateManager.Wrap1;
            }
            set
            {
                renderStateManager.Wrap1 = value;
            }
        }

        public WrapCoordinates Wrap10
        {
            get
            {
                return renderStateManager.Wrap10;
            }
            set
            {
                renderStateManager.Wrap10 = value;
            }
        }

        public WrapCoordinates Wrap11
        {
            get
            {
                return renderStateManager.Wrap11;
            }
            set
            {
                renderStateManager.Wrap11 = value;
            }
        }

        public WrapCoordinates Wrap12
        {
            get
            {
                return renderStateManager.Wrap12;
            }
            set
            {
                renderStateManager.Wrap12 = value;
            }
        }

        public WrapCoordinates Wrap13
        {
            get
            {
                return renderStateManager.Wrap13;
            }
            set
            {
                renderStateManager.Wrap13 = value;
            }
        }

        public WrapCoordinates Wrap14
        {
            get
            {
                return renderStateManager.Wrap14;
            }
            set
            {
                renderStateManager.Wrap14 = value;
            }
        }

        public WrapCoordinates Wrap15
        {
            get
            {
                return renderStateManager.Wrap15;
            }
            set
            {
                renderStateManager.Wrap15 = value;
            }
        }

        public WrapCoordinates Wrap2
        {
            get
            {
                return renderStateManager.Wrap2;
            }
            set
            {
                renderStateManager.Wrap2 = value;
            }
        }

        public WrapCoordinates Wrap3
        {
            get
            {
                return renderStateManager.Wrap3;
            }
            set
            {
                renderStateManager.Wrap3 = value;
            }
        }

        public WrapCoordinates Wrap4
        {
            get
            {
                return renderStateManager.Wrap4;
            }
            set
            {
                renderStateManager.Wrap4 = value;
            }
        }

        public WrapCoordinates Wrap5
        {
            get
            {
                return renderStateManager.Wrap5;
            }
            set
            {
                renderStateManager.Wrap5 = value;
            }
        }

        public WrapCoordinates Wrap6
        {
            get
            {
                return renderStateManager.Wrap6;
            }
            set
            {
                renderStateManager.Wrap6 = value;
            }
        }

        public WrapCoordinates Wrap7
        {
            get
            {
                return renderStateManager.Wrap7;
            }
            set
            {
                renderStateManager.Wrap7 = value;
            }
        }

        public WrapCoordinates Wrap8
        {
            get
            {
                return renderStateManager.Wrap8;
            }
            set
            {
                renderStateManager.Wrap8 = value;
            }
        }

        public WrapCoordinates Wrap9
        {
            get
            {
                return renderStateManager.Wrap9;
            }
            set
            {
                renderStateManager.Wrap9 = value;
            }
        }

        public bool ZBufferEnable
        {
            get
            {
                return renderStateManager.ZBufferEnable;
            }
            set
            {
                renderStateManager.ZBufferEnable = value;
            }
        }

        public Compare ZBufferFunction
        {
            get
            {
                return renderStateManager.ZBufferFunction;
            }
            set
            {
                renderStateManager.ZBufferFunction = value;
            }
        }

        public bool ZBufferWriteEnable
        {
            get
            {
                return renderStateManager.ZBufferWriteEnable;
            }
            set
            {
                renderStateManager.ZBufferWriteEnable = value;
            }
        }

        #endregion
    }
}
