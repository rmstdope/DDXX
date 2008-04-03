using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.SceneGraph;
using Dope.DDXX.Graphics;
using Dope.DDXX.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Dope.DDXX.ModelBuilder;
using Dope.DDXX.TextureBuilder;

namespace EngineTest
{
    public class LineEffect : BaseDemoEffect
    {
        private CameraNode camera;
        private LineNode[] lines;
        private ISpline<InterpolatedVector3>[] splines;

        private const int NumLines = 50;

        private ITexture2D texture;
        public ITexture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }

        public LineEffect(string name, float start, float end)
            : base(name, start, end)
        {
            splines = new ISpline<InterpolatedVector3>[NumLines];
            lines = new LineNode[NumLines];
        }

        protected override void Initialize()
        {
            CreateStandardCamera(out camera, 15);

            for (int i = 0; i < NumLines; i++)
            {
                splines[i] = new SimpleCubicSpline<InterpolatedVector3>();
                AddKeyFrame(i, 0, new Vector3(0, 0, 0));
                AddKeyFrame(i, 1, new Vector3(4, 2, 0));
                AddKeyFrame(i, 2, new Vector3(-7, -5, 0));
                AddKeyFrame(i, 3, new Vector3(-2, 8, 0));
                AddKeyFrame(i, 4, new Vector3(4, -6, 0));
                splines[i].Calculate();
                IMaterialHandler material = new MaterialHandler(EffectFactory.CreateFromFile("Content\\effects\\ColorLine"), new EffectConverter());
                material.DiffuseColor = new Color(new Vector3((i + 1) / (float)NumLines, (i + 1) / (float)NumLines, (i + 1) / (float)NumLines));
                lines[i] = new LineNode("Line", GraphicsFactory,
                    material,
                    splines[i], 100);
                Scene.AddNode(lines[i]);
            }
        }

        private void AddKeyFrame(int i, float t, Vector3 vector)
        {
            vector += Rand.Vector3(0.2f);
            splines[i].AddKeyFrame(new KeyFrame<InterpolatedVector3>(t, new InterpolatedVector3(vector)));
        }


        public override void Step()
        {
            for (int i = 0; i < NumLines; i++)
            {
                lines[i].WorldState.Turn(Time.DeltaTime * 1.2f);
                lines[i].WorldState.Tilt(Time.DeltaTime * 0.5f);
                lines[i].EndTime = Time.CurrentTime;
            }
            Scene.Step();
        }

        public override void Render()
        {
            Scene.Render();
        }
    }
}
