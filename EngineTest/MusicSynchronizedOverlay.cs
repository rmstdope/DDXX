using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Dope.DDXX.SceneGraph;
using Dope.DDXX.Utility;
using Dope.DDXX.MidiProcessorLib;

namespace EngineTest
{
    public class MusicSynchronizedOverlay : BaseDemoEffect
    {
        private class MusicPlane
        {
            const float maxTransparency = 0.2f;
            const float fadeIn = 0.05f;
            const float fadeOut = 0.8f;
            public ModelNode ModelNode;
            private float startTime;
            public MusicPlane(ModelNode modelNode, float startTime)
            {
                this.startTime = startTime;
                ModelNode = modelNode;
            }
            public void SetColor()
            {
                float time = Time.CurrentTime - startTime;
                float delta;
                if (time <= fadeIn)
                {
                    delta = time / fadeIn;
                }
                else
                {
                    delta = 1.0f - ((time - fadeIn) / fadeOut);
                }
                ModelNode.Model.Meshes[0].MeshParts[0].MaterialHandler.Transparency = delta* maxTransparency;
            }
            public bool HasEnded
            {
                get { return Time.CurrentTime > startTime + fadeIn + fadeOut; }
            }
        }

        private List<MusicPlane> planes;
        private List<ModelNode> unusedNodes;
        private CameraNode camera;
        private ISpriteBatch spriteBatch;

        public MusicSynchronizedOverlay(string name, float start, float end)
            : base(name, start, end)
        {
        }

        protected override void Initialize()
        {
            planes = new List<MusicPlane>();
            unusedNodes = new List<ModelNode>();
            CreateStandardCamera(out camera, 10);
            spriteBatch = GraphicsFactory.CreateSpriteBatch();

            for (int i = 0; i < 10; i++)
            {
                string material = "TransparentBlue" + i;
                ModelBuilder.CreateMaterial(material);
                ModelBuilder.SetAmbientColor(material, new Color(50, 20, 160));
                ModelBuilder.SetEffect(material, "Content\\effects\\TransparentColor");
                ModelDirector.CreatePlane(1, 200, 1, 1);
                ModelDirector.Rotate(MathHelper.PiOver2 * 0.95f, 0, 0);
                ModelDirector.Translate(0, 2, 0);
                IModel model = ModelDirector.Generate(material);
                ModelNode modelNode = new ModelNode("x", model, GraphicsDevice);
                unusedNodes.Add(modelNode);
            }
        }

        int index = 0;
        public override void Step()
        {
            const int TrackNum = 9;
            CompiledMidi.CompiledMidiTrack track = Mixer.CompiledMidi.Tracks[TrackNum];
            if (index < track.NotesAndTimes.Length &&
                Time.CurrentTime > track.NotesAndTimes[index] && 
                unusedNodes.Count > 0)
            {
                MusicPlane plane = new MusicPlane(unusedNodes[0], track.NotesAndTimes[index]);
                index += 2;
                unusedNodes.RemoveAt(0);
                plane.ModelNode.WorldState.Reset();
                plane.ModelNode.WorldState.Roll(Rand.Float(MathHelper.TwoPi));
                Scene.AddNode(plane.ModelNode);
                planes.Add(plane);
            }
            planes.RemoveAll(delegate(MusicPlane plane) 
            {
                if (plane.HasEnded)
                {
                    unusedNodes.Add(plane.ModelNode);
                    Scene.RemoveNode(plane.ModelNode);
                    return true;
                }
                return false;
            });
            planes.ForEach(delegate(MusicPlane plane) 
            { 
                plane.SetColor(); 
            });
            Scene.Step();
        }

        public override void Render()
        {
            Scene.Render();
        }
    }
}
