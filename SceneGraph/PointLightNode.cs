using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;
using Dope.DDXX.Graphics;
using Microsoft.DirectX.Direct3D;
using System.Drawing;
using Dope.DDXX.Utility;

namespace Dope.DDXX.SceneGraph
{
    public class PointLightNode : LightNode
    {
        private float range;
        private IDevice device;
        private ISprite sprite;
        private ITexture texture;

        public PointLightNode(string name)
            : base(name)
        {
        }

        public float Range
        {
            get { return range; }
            set { range = value; }
        }

        public void SetRenderParameters(IDevice device, ISprite sprite, ITexture texture)
        {
            this.device = device;
            this.sprite = sprite;
            this.texture = texture;
        }

        protected override void SetLightStateNode(LightState state)
        {
            state.NewState(Position, new Vector3(), range, DiffuseColor, SpecularColor);
        }

        protected override void RenderNode(IScene scene)
        {
            if (sprite != null && texture != null && device != null)
            {
                sprite.Begin(SpriteFlags.AlphaBlend);
                float spriteSize = 5;
                DrawBillboard(scene, spriteSize);
                sprite.End();
            }
        }

        private void DrawBillboard(IScene scene, float spriteSize)
        {
            CameraNode c1 = scene.ActiveCamera as CameraNode;
            Vector3 pos = Position;
            Vector3 size = new Vector3(0, spriteSize, 0);
            CameraNode c2 = new CameraNode("");
            c2.WorldState.MoveForward(-(pos - c1.Position).Length());
            c2.SetFOV(c1.GetFOV());
            pos.TransformCoordinate(c1.ViewMatrix * c1.ProjectionMatrix);
            size.TransformCoordinate(c2.ViewMatrix * c2.ProjectionMatrix);
            float s = device.Viewport.Height * size.Y / (2 * texture.GetLevelDescription(0).Height);
            Vector3 posTranslation =
                new Vector3(((1 + pos.X) / 2) * device.Viewport.Width,
                            ((1 - pos.Y) / 2) * device.Viewport.Height,
                            0);
            Vector3 halfSizeTranslation = new Vector3(-texture.GetLevelDescription(0).Width * s / 2, -texture.GetLevelDescription(0).Height * s / 2, 0);
            System.Diagnostics.Debug.WriteLine(posTranslation.ToString());
            sprite.Transform =
                Matrix.Scaling(s, s, 1) *
                Matrix.Translation(posTranslation) *
                Matrix.Translation(halfSizeTranslation);

            device.RenderState.SourceBlend = Blend.One;
            device.RenderState.DestinationBlend = Blend.One;
            sprite.Draw(texture, new Vector3(), new Vector3(0, 0, pos.Z), DiffuseColor.ToArgb());
        }
    }
}
