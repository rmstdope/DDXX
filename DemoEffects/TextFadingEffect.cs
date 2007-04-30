using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.Graphics;
using Microsoft.DirectX.Direct3D;

namespace Dope.DDXX.DemoEffects
{
    public class TextFadingEffect : BaseDemoEffect
    {
        private ISprite sprite;
        private IFont font;

        public TextFadingEffect(float startTime, float endTime)
            : base(startTime, endTime)
        {
        }

        protected override void Initialize()
        {
            FontDescription description = new FontDescription();
            description.FaceName = "Arial";
            description.Height = 25;

            font = GraphicsFactory.CreateFont(Device, description);
            sprite = GraphicsFactory.CreateSprite(Device);
        }

        public override void Step()
        {
        }

        public override void Render()
        {
        }
    }
}
