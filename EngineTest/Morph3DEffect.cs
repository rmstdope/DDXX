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
using Microsoft.Xna.Framework;

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
            private IModel model;

            public Morph(IModifier primitive, List<IModifier> from, List<IModifier> to, IModelDirector modelDirector)
            {
                this.primitive = primitive;
                this.from = new List<IModifier>(from);
                this.to = new List<IModifier>(to);
                this.modelDirector = modelDirector;
                this.modelBuilder = modelDirector.ModelBuilder;

                int i = 0;
                foreach (IModifier modifier in from)
                {
                    ConstructorInfo constructor = modifier.GetType().GetConstructor(new Type[] { });
                    IModifier newModifier = constructor.Invoke(new object[] { }) as IModifier;
                    this.to.Insert(i, newModifier);
                    i++;
                }
                foreach (IModifier modifier in to)
                {
                    ConstructorInfo constructor = modifier.GetType().GetConstructor(new Type[] { });
                    IModifier newModifier = constructor.Invoke(new object[] { }) as IModifier;
                    this.from.Add(newModifier);
                }

                chain = primitive;
                foreach (IModifier modifier in this.from)
                {
                    ConstructorInfo constructor = modifier.GetType().GetConstructor(new Type[] { });
                    IModifier newModifier = constructor.Invoke(new object[] { }) as IModifier;
                    newModifier.ConnectToInput(0, chain);
                    chain = newModifier;
                }
                from.Reverse();
                to.Reverse();
            }

            public void Step(float delta)
            {
                IModifier modifier = chain;
                for (int i = 0; i < from.Count; i++)
                {
                    PropertyInfo[] properties = from[i].GetType().GetProperties();
                    foreach (PropertyInfo property in properties)
                    {
                        if (property.CanRead && property.CanWrite && property.PropertyType == typeof(float))
                        {
                            float start = (float)property.GetGetMethod().Invoke(from[i], new object[] { });
                            float end = (float)property.GetGetMethod().Invoke(to[i], new object[] { });
                            float value = start + (end - start) * delta;
                            property.GetSetMethod().Invoke(modifier, new object[] { value });
                        }
                    }
                    modifier = modifier.GetInputModifier(0);
                }

                IPrimitive primitive = chain.Generate();
                VertexPositionTangentTexture[] newVertices = new VertexPositionTangentTexture[primitive.Vertices.Length];
                for (int i = 0; i < primitive.Vertices.Length; i++)
                    newVertices[i] = new VertexPositionTangentTexture(primitive.Vertices[i].Position, primitive.Vertices[i].Normal, primitive.Vertices[i].Tangent, primitive.Vertices[i].BiNormal, new Vector2(primitive.Vertices[i].U, primitive.Vertices[i].V));
                model.Meshes[0].VertexBuffer.SetData<VertexPositionTangentTexture>(newVertices);
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
                        model = modelBuilder.CreateModel(chain, "Material");
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
            morph.Step((Time.CurrentTime - StartTime) / 10.0f);
            //Mixer.ClearColor = Color.White;
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
