using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NMock2;
using Dope.DDXX.Graphics;
using Dope.DDXX.Utility;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.IO;
using NMock2.Monitoring;

namespace Dope.DDXX.SceneGraph
{
    [TestFixture]
    public class ParticleSystemNodeTest : D3DMockTest
    {
        private SystemStub system;
        private IRenderableCamera camera;
        private IScene scene;
        private IMaterialHandler materialHandler;
        private Matrix view = Matrix.CreateRotationX(1);
        private Matrix proj = Matrix.CreateRotationX(2);
        private Color color = Color.AntiqueWhite;
        private IEffectTechnique technique;
        private IEffectPass pass;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            system = new SystemStub("FSS");
            camera = mockery.NewMock<IRenderableCamera>();
            scene = mockery.NewMock<IScene>();
            materialHandler = mockery.NewMock<IMaterialHandler>();
            technique = mockery.NewMock<IEffectTechnique>();
            pass = mockery.NewMock<IEffectPass>();

            Stub.On(scene).GetProperty("ActiveCamera").Will(Return.Value(camera));
            Stub.On(scene).GetProperty("AmbientColor").Will(Return.Value(color));
            Stub.On(camera).GetProperty("ViewMatrix").Will(Return.Value(view));
            Stub.On(camera).GetProperty("ProjectionMatrix").Will(Return.Value(proj));
            Stub.On(effect).GetProperty("CurrentTechnique").Will(Return.Value(technique));
            Stub.On(materialHandler).GetProperty("Effect").Will(Return.Value(effect));
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        [Test]
        public void Initialize0Of0()
        {
            // Setup
            ExpectVertexBuffer(0);
            ExpectSpawner(0);
            // Exercise SUT
            system.Initialize(device, graphicsFactory, 0);
            // Verify
            Assert.AreEqual(0, system.ActiveParticles);
        }

        [Test]
        public void Initialize2Of5()
        {
            // Setup
            ExpectVertexBuffer(5);
            ExpectSpawner(2);
            // Exercise SUT
            system.Initialize(device, graphicsFactory, 5);
            // Verify
            Assert.AreEqual(2, system.ActiveParticles);
        }

        [Test]
        public void Initialize20Of20()
        {
            // Setup
            ExpectVertexBuffer(20);
            ExpectSpawner(20);
            // Exercise SUT
            system.Initialize(device, graphicsFactory, 20);
            // Verify
            Assert.AreEqual(20, system.ActiveParticles);
        }

        [Test]
        public void StepAndKillAllButFour()
        {
            // Setup
            Initialize20Of20();
            List<float> vertexList = new List<float>();
            int i = 0;
            system.ShouldSpawnReturns = new bool[] { false };
            foreach (ISystemParticle<float> particle in system.SpawnedParticles)
            {
                if (i > 3)
                {
                    Stub.On(particle).Method("IsDead").Will(Return.Value(true));
                }
                else
                {
                    Stub.On(particle).Method("IsDead").Will(Return.Value(false));
                    Expect.Once.On(particle).Method("Step").Will(new VertexCopyAction(vertexList));
                }
                i++;
            }
            Expect.Once.On(vertexBuffer).Method("SetData").
                With(new MatchListAndArray(vertexList), Is.EqualTo(0), Is.EqualTo(4), Is.EqualTo(SetDataOptions.Discard));
            // Exercise SUT
            system.Step();
            // Verify SUT
            Assert.AreEqual(4, system.ActiveParticles);
        }

        [Test]
        public void StepNoCreate()
        {
            // Setup
            Initialize2Of5();
            List<float> vertexList = new List<float>();
            system.ShouldSpawnReturns = new bool[] { false };
            for (int i = 0; i < system.SpawnedParticles.Count; i++)
            {
                Stub.On(system.SpawnedParticles[i]).Method("IsDead").Will(Return.Value(false));
                Expect.Once.On(system.SpawnedParticles[i]).Method("Step").Will(new VertexCopyAction(vertexList));
            }
            Expect.Once.On(vertexBuffer).Method("SetData").
                With(new MatchListAndArray(vertexList), Is.EqualTo(0), Is.EqualTo(2), Is.EqualTo(SetDataOptions.Discard));
            // Exercise SUT
            system.Step();
            // Verify SUT
            Assert.AreEqual(2, system.ActiveParticles);
        }

        [Test]
        public void StepAllCreated()
        {
            // Setup
            Initialize20Of20();
            List<float> vertexList = new List<float>();
            for (int i = 0; i < system.SpawnedParticles.Count; i++)
            {
                Stub.On(system.SpawnedParticles[i]).Method("IsDead").Will(Return.Value(false));
                Expect.Once.On(system.SpawnedParticles[i]).Method("Step").Will(new VertexCopyAction(vertexList));
            }
            Expect.Once.On(vertexBuffer).Method("SetData").
                With(new MatchListAndArray(vertexList), Is.EqualTo(0), Is.EqualTo(20), Is.EqualTo(SetDataOptions.Discard));
            // Exercise SUT
            system.Step();
            // Verify
            Assert.AreEqual(20, system.ActiveParticles);
        }

        [Test]
        public void StepCreateOne()
        {
            // Setup
            Initialize2Of5();
            List<float> vertexList = new List<float>();
            system.ShouldSpawnReturns = new bool[] { true, false };
            system.SpawnedParticles.Add(mockery.NewMock<ISystemParticle<float>>());
            for (int i = 0; i < system.SpawnedParticles.Count; i++)
            {
                Stub.On(system.SpawnedParticles[i]).Method("IsDead").Will(Return.Value(false));
                Expect.Once.On(system.SpawnedParticles[i]).Method("Step").Will(new VertexCopyAction(vertexList));
            }
            Expect.Once.On(vertexBuffer).Method("SetData").
                With(new MatchListAndArray(vertexList), Is.EqualTo(0), Is.EqualTo(3), Is.EqualTo(SetDataOptions.Discard));
            // Exercise SUT
            system.Step();
            // Verify
            Assert.AreEqual(3, system.ActiveParticles);
        }

        [Test]
        public void StepCreateThree()
        {
            // Setup
            Initialize2Of5();
            List<float> vertexList = new List<float>();
            system.ShouldSpawnReturns = new bool[] { true, true, true };
            for (int i = 0; i < 3; i++)
                system.SpawnedParticles.Add(mockery.NewMock<ISystemParticle<float>>());
            for (int i = 0; i < system.SpawnedParticles.Count; i++)
            {
                Stub.On(system.SpawnedParticles[i]).Method("IsDead").Will(Return.Value(false));
                Expect.Once.On(system.SpawnedParticles[i]).Method("Step").Will(new VertexCopyAction(vertexList));
            }
            Expect.Once.On(vertexBuffer).Method("SetData").
                With(new MatchListAndArray(vertexList), Is.EqualTo(0), Is.EqualTo(5), Is.EqualTo(SetDataOptions.Discard));
            // Exercise SUT
            system.Step();
            // Verify
            Assert.AreEqual(5, system.ActiveParticles);
        }

        [Test]
        public void DrawNoParticles()
        {
            // Setup
            Initialize0Of0();
            // Exercise SUT
            system.Render(scene);
        }

        [Test]
        public void Draw2Particles()
        {
            // Setup
            Initialize2Of5();
            system.Material = materialHandler;
            Expect.Once.On(materialHandler).Method("SetupRendering").
                With(Is.EqualTo(system.WorldMatrix), Is.EqualTo(view), Is.EqualTo(proj), Is.EqualTo(color), Is.Anything);
            Expect.Once.On(vertexStream).Method("SetSource").With(vertexBuffer, 0, 42);
            Expect.Once.On(device).SetProperty("VertexDeclaration").To(vertexDeclaration);
            ExpectForeachPass(technique, new IEffectPass[] { pass });
            Expect.Once.On(effect).Method("Begin");
            Expect.Once.On(pass).Method("Begin");
            Expect.Once.On(device).Method("DrawPrimitives").With(PrimitiveType.PointList, 0, 2);
            Expect.Once.On(pass).Method("End");
            Expect.Once.On(effect).Method("End");
            // Exercise SUT
            system.Render(scene);
        }

        [Test]
        public void Draw20Particles()
        {
            // Setup
            Initialize20Of20();
            system.Material = materialHandler;
            Expect.Once.On(materialHandler).Method("SetupRendering").
                With(Is.EqualTo(system.WorldMatrix), Is.EqualTo(view), Is.EqualTo(proj), Is.EqualTo(color), Is.Anything);
            Expect.Once.On(vertexStream).Method("SetSource").With(vertexBuffer, 0, 42);
            Expect.Once.On(device).SetProperty("VertexDeclaration").To(vertexDeclaration);
            ExpectForeachPass(technique, new IEffectPass[] { pass });
            Expect.Once.On(effect).Method("Begin");
            Expect.Once.On(pass).Method("Begin");
            Expect.Once.On(device).Method("DrawPrimitives").With(PrimitiveType.PointList, 0, 20);
            Expect.Once.On(pass).Method("End");
            Expect.Once.On(effect).Method("End");
            // Exercise SUT
            system.Render(scene);
        }

        [Test]
        public void SphereRadius0()
        {
            for (int i = 0; i < 1000; i++)
            {
                // Exercise SUT
                Vector3 pos = system.GetSpherePosition(0);
                // Verify
                Assert.GreaterOrEqual(0, pos.Length());
            }
        }

        [Test]
        public void SphereRadius1()
        {
            for (int i = 0; i < 1000; i++)
            {
                // Exercise SUT
                Vector3 pos = system.GetSpherePosition(1);
                // Verify
                Assert.GreaterOrEqual(1, pos.Length());
            }
        }

        private void ExpectSpawner(int numInitial)
        {
            system.NumInitialSpawnsReturn = numInitial;
            for (int i = 0; i < numInitial; i++)
            {
                system.SpawnedParticles.Add(mockery.NewMock<ISystemParticle<float>>());
            }
        }

        private void ExpectVertexBuffer(int num)
        {
            Expect.Once.On(graphicsFactory).Method("CreateVertexBuffer").
                With(typeof(float), num, ResourceUsage.WriteOnly | ResourceUsage.Dynamic, ResourceManagementMode.Manual).
                Will(Return.Value(vertexBuffer));
            Expect.Once.On(graphicsFactory).Method("CreateVertexDeclaration").
                With(new VertexElement[] { new VertexElement(0, 1, VertexElementFormat.Byte4, VertexElementMethod.LookUpPresampled, VertexElementUsage.TessellateFactor, 2) }).
                Will(Return.Value(vertexDeclaration));
        }

        private class VertexCopyAction : IAction
        {
            private List<float> list;

            public VertexCopyAction(List<float> list)
            {
                this.list = list;
            }

            public void Invoke(Invocation invocation)
            {
                float vertex = (float)(invocation.Parameters[0]);
                list.Add(vertex);
            }

            public void DescribeTo(TextWriter writer)
            {
            }
        }

        private class MatchListAndArray : Matcher
        {
            private List<float> list;

            public MatchListAndArray(List<float> list)
            {
                this.list = list;
            }

            public override void DescribeTo(TextWriter writer)
            {
            }

            public override bool Matches(object o)
            {
                if (!(o is float[]))
                    return false;
                float[] array = (o as float[]);
                if (array.Length < list.Count)
                    return false;
                for (int i = 0; i < list.Count; i++)
                    if (array[i] != list[i])
                        return false;
                return true;
            }
        }

    }

    public class SystemStub : ParticleSystemNode<float>
    {
        public List<ISystemParticle<float>> SpawnedParticles;
        public int NumInitialSpawnsReturn;
        private int spawnCalled;
        public bool[] ShouldSpawnReturns;
        private int shouldSpawnCounter;

        public SystemStub(string name)
            : base(name)
        {
            SpawnedParticles = new List<ISystemParticle<float>>();
            spawnCalled = 0;
            shouldSpawnCounter = 0;
        }

        protected override int NumInitialSpawns
        {
            get { return NumInitialSpawnsReturn; }
        }

        protected override ISystemParticle<float> Spawn()
        {
            return SpawnedParticles[spawnCalled++];
        }

        protected override bool ShouldSpawn()
        {
            return ShouldSpawnReturns[shouldSpawnCounter++];
        }

        protected override IMaterialHandler CreateDefaultMaterial(IGraphicsFactory graphicsFactory)
        {
            return null;
        }

        public Vector3 GetSpherePosition(float radius)
        {
            return RandomPositionInSphere(radius);
        }

        protected override int VertexSizeInBytes
        {
            get { return 42; }
        }

        protected override VertexElement[] VertexElements
        {
            get { return new VertexElement[] { new VertexElement(0, 1, VertexElementFormat.Byte4, VertexElementMethod.LookUpPresampled, VertexElementUsage.TessellateFactor, 2) }; }
        }
    }

}
