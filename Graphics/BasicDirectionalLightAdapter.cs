using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.Graphics
{
    public class BasicDirectionalLightAdapter : IBasicDirectionalLight
    {
        private BasicDirectionalLight light;

        public BasicDirectionalLightAdapter(BasicDirectionalLight light)
        {
            this.light = light;
        }

        #region IBasicDirectionalLight Members

        public Vector3 DiffuseColor
        {
            get
            {
                return light.DiffuseColor;
            }
            set
            {
                light.DiffuseColor = value;
            }
        }

        public Vector3 Direction
        {
            get
            {
                return light.Direction;
            }
            set
            {
                light.Direction = value;
            }
        }

        public bool Enabled
        {
            get
            {
                return light.Enabled;
            }
            set
            {
                light.Enabled = value;
            }
        }

        public Vector3 SpecularColor
        {
            get
            {
                return light.SpecularColor;
            }
            set
            {
                light.SpecularColor = value;
            }
        }

        #endregion
    }
}
