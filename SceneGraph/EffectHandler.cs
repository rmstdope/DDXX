using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;

using Dope.DDXX.Graphics;
using Dope.DDXX.Utility;

namespace Dope.DDXX.SceneGraph
{
    public class EffectHandler : IEffectHandler
    {
        private IEffect effect;
        private EffectHandle technique;
        private EffectHandle worldT;
        private EffectHandle worldViewProjT;

        public EffectHandler(IEffect effect)
        {
            this.effect = effect;
            technique = effect.FindNextValidTechnique(null);
            worldT = effect.GetParameter(null, "WorldT");
            worldViewProjT = effect.GetParameter(null, "WorldViewProjectionT");

            if (worldT == null || worldViewProjT == null)
                throw new DDXXException("Invalid effect. Not all mandatory parameters are present.");
        }

        #region IEffectHandler Members

        public IEffect Effect
        {
            get { return effect; }
        }

        public EffectHandle Technique
        {
            get { return technique; }
        }

        public void SetMeshConstants(IRenderableScene scene, INode node)
        {
            effect.SetValueTranspose(worldT, node.WorldMatrix);
            effect.SetValueTranspose(worldViewProjT, node.WorldMatrix * scene.ActiveCamera.ViewMatrix * scene.ActiveCamera.ProjectionMatrix);
        }

        #endregion
    }
}
