using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.Graphics;
using Dope.DDXX.Physics;
using Dope.DDXX.SceneGraph;
using Dope.DDXX.Utility;
using Dope.DDXX.ParticleSystems;
using Dope.DDXX.Graphics;
using Dope.DDXX.MeshBuilder;

namespace EngineTest
{
    public class TimeSyncEffect : BaseDemoEffect
    {
        private float lingerTime = 1.0f;
        float[] notesAndTimes = new float[] {
            3.465007f, 78f, 3.906175f, 80f, 4.145141f, 76f, 4.788511f, 80f, 5.183724f, 78f, 5.652465f, 76f, 5.873049f, 73f, 6.847295f, 73f, 7.040306f, 81f, 7.490665f, 81f, 7.90426f, 81f, 8.529248f, 81f, 8.749832f, 81f, 9.236955f, 80f, 9.439157f, 78f, 10.37664f, 76f, 10.59722f, 78f, 11.04758f, 80f, 11.26817f, 76f, 11.90234f, 80f, 12.31594f, 78f, 12.73873f, 76f, 12.9685f, 73f, 13.92437f, 73f, 14.13576f, 76f, 14.54016f, 73f, 14.95376f, 76f, 15.6247f, 73f, 15.85448f, 76f, 16.17616f, 73f, 16.70924f, 76f, 17.58238f, 78f, 17.78458f, 78f, 18.23494f, 80f, 18.47391f, 76f, 19.42058f, 80f, 19.62278f, 78f, 19.97204f, 76f, 20.20182f, 73f, 21.06577f, 73f, 21.26797f, 81f, 21.68157f, 81f, 22.07678f, 81f, 22.6742f, 81f, 22.88559f, 81f, 23.28999f, 80f, 23.51977f, 78f, 24.4021f, 76f, 24.62269f, 78f, 25.03628f, 80f, 25.25687f, 76f, 25.96458f, 80f, 26.38736f, 78f, 26.80096f, 76f, 26.99397f, 73f, 27.93145f, 73f, 28.15203f, 76f, 28.60239f, 73f, 29.0068f, 76f, 29.45716f, 73f, 29.87994f, 78f, 30.15567f, 78f, 30.68875f, 78f, 74.30043f, 78f, 74.67802f,80f, 74.97747f, 76f, 75.26392f, 80f, 75.56989f, 78f, 75.84982f, 76f, 76.00606f, 73f, 76.58545f, 73f, 76.74169f, 76f, 77.06718f, 73f, 77.41873f, 76f, 77.70517f, 81f, 78.03066f, 81f, 78.34315f, 80f, 78.52542f, 78f, 79.07227f, 78f, 79.23502f, 78f, 79.52797f, 80f, 79.87299f, 76f, 80.17245f, 80f, 80.50446f, 78f, 80.81694f, 76f, 80.99272f, 73f, 81.57211f, 73f, 81.73486f, 76f, 82.03432f, 73f, 82.38586f, 76f, 82.67229f, 76f, 83.03034f, 76f, 83.31679f, 78f, 83.44698f, 78f, 84.07195f, 78f, 84.20866f, 78f, 84.53416f, 80f, 84.87267f, 76f, 85.19817f, 80f, 85.51066f, 78f, 85.80361f, 76f, 85.95334f, 73f, 86.59782f, 73f, 86.81265f, 81f, 87.07306f, 81f, 87.39205f, 81f, 87.70453f, 81f, 88.04305f, 81f, 88.34251f, 80f, 88.49874f, 78f, 89.07813f, 78f, 89.21485f, 78f, 89.54034f, 80f, 89.81377f, 76f, 90.18483f, 80f, 90.52986f, 78f, 90.81631f, 76f, 90.95953f, 73f, 91.56496f, 73f, 91.69515f, 81f, 92.0467f, 81f, 92.38522f, 81f, 92.71072f, 81f, 93.0232f, 81f, 93.32265f, 80f, 93.4789f, 78f, 104.2985f, 78f, 104.585f, 80f, 104.7998f, 76f, 105.1969f, 80f, 105.4898f, 78f, 105.7828f, 76f, 105.9455f, 73f, 106.3947f, 73f, 106.7007f, 76f, 107.0197f, 73f, 107.3582f, 76f, 107.6772f, 73f, 107.9897f, 76f, 108.2826f, 73f,108.4389f, 76f, 108.8165f, 78f, 109.2852f, 78f, 109.5977f, 80f, 109.9101f, 76f, 110.1836f, 80f, 110.5025f, 78f, 110.802f, 76f, 110.9582f, 73f, 111.5507f, 73f, 111.6613f, 81f, 112.0259f, 81f, 112.41f, 81f, 112.742f, 81f, 113.048f, 81f, 113.3539f, 80f, 113.5102f, 78f, 114.0961f, 76f, 114.2393f, 76f, 114.5648f, 76f, 114.8968f, 76f, 115.2093f, 76f, 115.5218f, 76f, 115.8407f, 73f, 115.9905f, 73f, 116.5503f, 73f, 116.7066f, 76f, 117.0256f, 76f, 117.3641f, 76f, 117.6701f, 76f, 118.0151f, 76f, 118.3276f, 78f, 118.4578f, 78f, 119.0957f, 78f, 119.2455f, 78f, 119.5514f, 80f, 119.8769f, 76f, 120.2024f, 80f, 120.5019f, 78f, 120.8144f, 76f, 120.9511f, 73f, 121.5956f, 73f, 121.7128f, 81f, 122.0448f, 81f, 122.4028f, 81f, 122.7218f, 81f, 123.0343f, 81f, 123.3272f, 80f, 123.49f, 78f, 145.5133f, 78f, 145.943f, 80f, 146.1773f, 76f, 146.4638f, 80f, 146.7893f, 78f, 147.0627f, 76f, 147.2124f, 73f, 147.6291f, 73f, 148.0197f, 76f, 148.3257f, 73f, 148.6642f, 76f, 149.0027f, 81f, 149.2891f, 81f, 149.5821f, 80f, 149.7644f, 78f, 150.1485f, 76f, 150.5325f, 78f, 150.858f, 80f, 151.0273f, 76f, 151.4765f, 80f, 151.7694f, 78f, 152.0363f, 76f, 152.2316f, 73f, 152.6092f, 73f, 153.0324f, 81f, 153.2993f, 81f, 153.6443f, 81f, 154.087f, 81f, 154.2693f, 81f, 154.5492f, 80f, 154.7119f, 78f, 155.057f, 76f, 155.4411f, 78f, 155.7991f, 80f, 156.0856f, 76f, 156.6194f, 76f, 156.7822f, 76f, 157.049f, 73f, 157.1988f, 73f, 157.9279f, 76f, 158.2339f, 76f, 158.5984f, 76f, 158.963f, 76f, 159.282f, 76f, 159.5684f, 78f, 159.7116f, 78f, 160.5124f, 78f, 160.8248f, 80f, 160.9746f, 76f, 161.4563f, 80f, 161.7688f, 78f, 162.0422f, 76f, 162.1854f, 73f, 162.7778f, 81f, 162.9081f, 81f, 163.2661f, 81f, 163.5981f, 81f, 163.9301f, 81f, 164.2491f, 81f, 164.5551f, 80f, 164.7178f, 78f, 167.8036f, 81f, 167.9338f, 81f, 168.2723f, 81f, 168.5978f, 81f, 168.9298f, 81f, 169.2292f, 81f, 169.5678f, 80f, 169.7305f, 78f, 177.8876f, 83f, 178.0178f, 83f, 178.3563f, 83f, 178.6492f, 83f, 178.9552f, 83f, 179.2612f, 83f, 179.5867f, 80f, 179.7494f, 78f, 187.9911f, 78f, 188.0041f, 78f, 188.4545f, 80f, 188.7302f, 76f, 189.2908f, 80f, 189.7228f, 78f, 190.2007f, 76f, 190.4305f, 73f, 191.6989f, 76f, 192.0941f, 73f, 192.4985f, 76f, 192.9213f, 73f, 193.3165f, 76f, 193.7668f, 73f, 194.0426f, 76f, 194.5021f, 78f, 195.0352f, 78f, 195.4948f, 80f, 195.8808f, 76f, 196.3403f, 80f, 196.8183f, 78f, 197.2502f, 76f, 197.5168f, 73f, 198.5646f, 81f, 198.9874f, 81f,199.4653f, 81f, 199.9156f, 81f, 200.1914f, 81f, 200.7888f, 80f, 201.2207f, 76f, 201.6527f, 76f, 202.1307f, 78f, 202.5994f, 80f, 202.8476f, 76f, 203.445f, 80f, 203.8861f, 78f, 204.3089f, 76f, 204.603f, 73f, 205.1269f, 73f, 205.614f, 76f, 206.1104f, 73f, 206.5515f, 76f, 206.9835f, 73f, 207.4155f, 76f, 207.7188f, 73f, 208.1875f, 76f, 208.6655f, 78f, 209.1802f, 78f, 209.6765f, 80f, 209.9706f, 76f, 210.5588f, 80f, 210.9908f, 78f, 211.386f, 76f, 211.6342f, 73f, 212.1856f, 73f, 212.6544f, 76f, 213.1507f, 73f, 213.5827f, 76f, 213.8676f, 73f, 214.3455f, 78f, 214.6304f, 78f, 215.1176f, 78f, 215.7425f, 76f, 
        };
        private float timeAdd = 0.0f;

        public float TimeAdd
        {
            get { return timeAdd; }
            set { timeAdd = value; }
        }

        private ITexture circleTexture;
        private ISprite circleSprite;

        public TimeSyncEffect(float startTimef, float endTime)
            : base(startTimef, endTime)
        {
        }

        private Vector4 circleCallback(Vector2 texCoordf, Vector2 texelSize)
        {
            //Vector2 centered = texCoord - new Vector2(0.5ff, 0.5f);
            //float distance = centered.Length();
            //if (distance < 0.5f)
                return new Vector4(1f, 1f, 1f, 1);
            //return new Vector4(0f, 0f, 0f, 0);
        }

        protected override void Initialize()
        {
            circleTexture = TextureFactory.CreateFromFile("square.tga"); //.CreateFromFunction(512f, 512f, 0f, Usage.Nonef, Format.A8R8G8B8f, Pool.Managedf, circleCallback);
            circleSprite = GraphicsFactory.CreateSprite(Device);
        }

        public override void StartTimeUpdated()
        {
        }

        public override void EndTimeUpdated()
        {
        }

        public override void Step()
        {
        }

        public override void Render()
        {
            Viewport viewport = Device.Viewport;

            circleSprite.Begin(SpriteFlags.AlphaBlend);
            for (int i = 0; i < notesAndTimes.Length; i += 2)
            {
                float time = Time.StepTime + timeAdd;
                if (time > notesAndTimes[i] && time < notesAndTimes[i] + lingerTime)
                {
                    float delta = (time - notesAndTimes[i]) / lingerTime;
                    delta = 1 - delta;
                    int relNote = (int)notesAndTimes[i + 1] - 70;
                    circleSprite.Draw2D(circleTexture, Rectangle.Empty, new SizeF(40f, viewport.Height),
                        new PointF(40 + 40 * relNote, 0), Color.FromArgb((int)(70 * delta), Color.Firebrick));
                }
            }
            circleSprite.End();
        }

    }
}
