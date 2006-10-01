using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;
using Dope.DDXX.Utility;

namespace Dope.DDXX.Graphics
{
    public class DevicePrerequisits : IPrerequisits
    {
        private int shaderModel;
        private Version vertexShaderVersion;
        private Version pixelShaderVersion;

        public int ShaderModel
        {
            get { return shaderModel; }
            set { shaderModel = value; }
        }
        public Version VertexShaderVersion
        {
            get { return vertexShaderVersion; }
            set { vertexShaderVersion = value; }
        }
        public Version PixelShaderVersion
        {
            get { return pixelShaderVersion; }
            set { pixelShaderVersion = value; }
        }

        public DevicePrerequisits()
        {
            ShaderModel = 0;
            VertexShaderVersion = new Version(0, 0);
            PixelShaderVersion = new Version(0, 0);
        }

        public void CheckPrerequisits(int adapter, DeviceType deviceType)
        {
            Caps caps = D3DDriver.Factory.Manager.GetDeviceCaps(adapter, deviceType);

            if (caps.VertexShaderVersion < new Version(ShaderModel, 0) ||
                caps.PixelShaderVersion < new Version(ShaderModel, 0))
                throw new DDXXException("Shader Model is too low. Expected " + ShaderModel + " but found " + caps.PixelShaderVersion.Major + ".");
            if (caps.VertexShaderVersion < VertexShaderVersion)
                throw new DDXXException("Vertex shader version is too low. Expected " + VertexShaderVersion + " but found " + caps.VertexShaderVersion + ".");
            if (caps.PixelShaderVersion < PixelShaderVersion)
                throw new DDXXException("Pixel shader version is too low. Expected " + PixelShaderVersion + " but found " + caps.PixelShaderVersion + ".");
        }
    }
}
