using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.SceneGraph;
using Dope.DDXX.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Dope.DDXX.Utility;
using Dope.DDXX.ModelBuilder;
using System.Reflection;

namespace EngineTest
{
    public class Morph3DEffect : BaseDemoEffect
    {
        private class Morph
        {
            private IModelDirector modelDirector;
            private IModelBuilder modelBuilder;
            private IModifier primitive;
            private List<IModifier> from;
            private List<IModifier> to;
            private ModelNode node;
            private IModifier chain;

            public Morph(IModifier primitive, List<IModifier> from, List<IModifier> to, IModelDirector modelDirector)
            {
                this.primitive = primitive;
                this.from = from;
                this.to = to;
                this.modelDirector = modelDirector;
                this.modelBuilder = modelDirector.ModelBuilder;
                this.to.InsertRange(0, from);
                this.from.AddRange(to);

                chain = primitive;
                foreach (IModifier modifier in this.from)
                {
                    ConstructorInfo constructor = modifier.GetType().GetConstructor(new Type[] { });
                    IModifier newModifier = constructor.Invoke(new object[] { }) as IModifier;
                    newModifier.ConnectToInput(0, chain);
                    chain = newModifier;
                }
            }

            public ModelNode ModelNode
            {
                get
                {
                    if (node == null)
                    {
                        modelBuilder.CreateMaterial("Material");
                        modelBuilder.SetDiffuseTexture("Material", "Noise256base1024");
                        modelBuilder.SetEffect("Material", "Content\\effects\\Morph");
                        IModel model = modelBuilder.CreateModel(chain, "Material");
                        //modelDirector.CreateBox(5, 5, 5);
                        //modelDirector.Scale(1, 3, 5);
                        //IModel model = modelDirector.Generate("Material");
                        node = new ModelNode("Morph", model, modelBuilder.GraphicsDevice);
                    }
                    return node;
                }
            }
        }

        private Morph morph;
        private ModelNode node;

        public Morph3DEffect(string name, float start, float end)
            : base(name, start, end)
        {
        }

        protected override void Initialize()
        {
            CreateStandardCamera(30);

            BoxPrimitive box = new BoxPrimitive();
            box.Width = 1;
            box.Height = 1;
            box.Length = 1;

            List<IModifier> primitives1 = new List<IModifier>();
            List<IModifier> primitives2 = new List<IModifier>();
            Scale scale = new Scale();
            scale.X = 2.0f;
            scale.Y = 3.0f;
            scale.Z = 4.0f;
            primitives1.Add(scale);
            morph = new Morph(box, primitives1, primitives2, ModelDirector);
            node = morph.ModelNode;
            Scene.AddNode(node);
        }

        public override void Step()
        {
            Mixer.ClearColor = Color.White;
            node.WorldState.Turn(Time.DeltaTime);
            node.WorldState.Tilt(Time.DeltaTime);
            Scene.Step();
        }

        public override void Render()
        {
            Scene.Render();
        }
    }
}
