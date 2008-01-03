using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.Graphics
{
    public class BasicEffectAdapter : EffectAdapter, IBasicEffect
    {
        private BasicEffect effect;

        public BasicEffectAdapter(BasicEffect effect)
            : base(effect)
        {
            this.effect = effect;
        }

        #region IBasicEffect Members

        public float Alpha
        {
            get
            {
                return effect.Alpha;
            }
            set
            {
                effect.Alpha = value;
            }
        }

        public Vector3 AmbientLightColor
        {
            get
            {
                return effect.AmbientLightColor;
            }
            set
            {
                effect.AmbientLightColor = value;
            }
        }

        public Vector3 DiffuseColor
        {
            get
            {
                return effect.DiffuseColor;
            }
            set
            {
                effect.DiffuseColor = value;
            }
        }

        public IBasicDirectionalLight DirectionalLight0
        {
            get { return new BasicDirectionalLightAdapter(effect.DirectionalLight0); }
        }

        public IBasicDirectionalLight DirectionalLight1
        {
            get { return new BasicDirectionalLightAdapter(effect.DirectionalLight1); }
        }

        public IBasicDirectionalLight DirectionalLight2
        {
            get { return new BasicDirectionalLightAdapter(effect.DirectionalLight2); }
        }

        public Vector3 EmissiveColor
        {
            get
            {
                return effect.EmissiveColor;
            }
            set
            {
                effect.EmissiveColor = value;
            }
        }

        public Vector3 FogColor
        {
            get
            {
                return effect.FogColor;
            }
            set
            {
                effect.FogColor = value;
            }
        }

        public bool FogEnabled
        {
            get
            {
                return effect.FogEnabled;
            }
            set
            {
                effect.FogEnabled = value;
            }
        }

        public float FogEnd
        {
            get
            {
                return effect.FogEnd;
            }
            set
            {
                effect.FogEnd = value;
            }
        }

        public float FogStart
        {
            get
            {
                return effect.FogStart;
            }
            set
            {
                effect.FogStart = value;
            }
        }

        public bool LightingEnabled
        {
            get
            {
                return effect.LightingEnabled;
            }
            set
            {
                effect.LightingEnabled = value;
            }
        }

        public bool PreferPerPixelLighting
        {
            get
            {
                return effect.PreferPerPixelLighting;
            }
            set
            {
                effect.PreferPerPixelLighting = value;
            }
        }

        public Matrix Projection
        {
            get
            {
                return effect.Projection;
            }
            set
            {
                effect.Projection = value;
            }
        }

        public Vector3 SpecularColor
        {
            get
            {
                return effect.SpecularColor;
            }
            set
            {
                effect.SpecularColor = value;
            }
        }

        public float SpecularPower
        {
            get
            {
                return effect.SpecularPower;
            }
            set
            {
                effect.SpecularPower = value;
            }
        }

        public ITexture2D Texture
        {
            get
            {
                return new Texture2DAdapter(effect.Texture);
            }
            set
            {
                effect.Texture = (value as Texture2DAdapter).DxTexture2D;
            }
        }

        public bool TextureEnabled
        {
            get
            {
                return effect.TextureEnabled;
            }
            set
            {
                effect.TextureEnabled = value;
            }
        }

        public bool VertexColorEnabled
        {
            get
            {
                return effect.VertexColorEnabled;
            }
            set
            {
                effect.VertexColorEnabled = value;
            }
        }

        public Matrix View
        {
            get
            {
                return effect.View;
            }
            set
            {
                effect.View = value;
            }
        }

        public Matrix World
        {
            get
            {
                return effect.World;
            }
            set
            {
                effect.World = value;
            }
        }

        public void EnableDefaultLighting()
        {
            effect.EnableDefaultLighting();
        }

        #endregion
    }
}
