using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;

namespace Dope.DDXX.Graphics
{
    public class BaseTextureAdapter : IBaseTexture
    {
        protected BaseTexture baseTexture;

        protected BaseTextureAdapter(BaseTexture texture)
        {
            baseTexture = texture;
        }

        public BaseTexture BaseTextureDX 
        { 
            get 
            { 
                return baseTexture; 
            } 
        }

        #region IBaseTexture Members

        public IDevice Device
        {
            get { return new DeviceAdapter(baseTexture.Device); }
        }

        public int Priority
        {
            get
            {
                return baseTexture.Priority;
            }
            set
            {
                baseTexture.Priority = value;
            }
        }

        public ResourceType Type
        {
            get { return baseTexture.Type; }
        }

        public void Dispose()
        {
            baseTexture.Dispose();
        }

        public void PreLoad()
        {
            baseTexture.PreLoad();
        }

        public int SetPriority(int priorityNew)
        {
            return baseTexture.SetPriority(priorityNew);
        }

        public TextureFilter AutoGenerateFilterType
        {
            get
            {
                return baseTexture.AutoGenerateFilterType;
            }
            set
            {
                baseTexture.AutoGenerateFilterType = value;
            }
        }

        public int LevelCount
        {
            get { return baseTexture.LevelCount; }
        }

        public int LevelOfDetail
        {
            get
            {
                return baseTexture.LevelOfDetail;
            }
            set
            {
                baseTexture.LevelOfDetail = value;
            }
        }

        public void GenerateMipSubLevels()
        {
            baseTexture.GenerateMipSubLevels();
        }

        public int SetLevelOfDetail(int lodNew)
        {
            return baseTexture.SetLevelOfDetail(lodNew);
        }

        public void Save(string destFile, ImageFileFormat destFormat)
        {
            TextureLoader.Save(destFile, destFormat, baseTexture);
        }

        #endregion
    }
}
