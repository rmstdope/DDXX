using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.Physics;
using Microsoft.DirectX;
using Dope.DDXX.Graphics;
using System.Drawing;
using Dope.DDXX.SceneGraph;
using Dope.DDXX.MeshBuilder;
using Dope.DDXX.Utility;
using Microsoft.DirectX.Direct3D;

namespace TiVi
{
    public class PhysicalCubes : BaseDemoEffect
    {
        private class YPosConstraint : IConstraint
        {
            private const float epsilon = 0.005f;
            private float yPos;
            private IPhysicalParticle particle;
            
            public YPosConstraint(float yPos, IPhysicalParticle particle)
            {
                this.particle = particle;
                this.yPos = yPos;
            }

            public ConstraintPriority Priority
            {
                get { return ConstraintPriority.DummyPriority; }
            }

            public void Satisfy()
            {
                if (particle.Position.Y < yPos - epsilon)
                {
                    Vector3 oldPos = particle.OldPosition;
                    float yDiff = particle.Position.Y - particle.OldPosition.Y;
                    oldPos.Y = particle.Position.Y;
                    //particle.OldPosition = oldPos;
                    //particle.Position = new Vector3(particle.Position.X, oldPos.Y - yDiff, particle.Position.Z);
                    particle.Position = new Vector3(particle.Position.X, yPos, particle.Position.Z);
                }
            }
        }

        private class PhysicalCube
        {
            public ModelNode model;
            public MirrorNode mirror;
            public IBody body;
            public bool floorContact;
            public Vector3 up;
            public Vector3 forward;
            public Vector3 right;
        }

        private ILine line;
        private IScene scene;
        private CameraNode camera;
        private List<PhysicalCube> cubes = new List<PhysicalCube>();
        private ChessBoard chessBoard;

        public PhysicalCubes(string name, float start, float end)
            : base(name, start, end)
        {
        }

        private Vector4 celMapCallback(Vector2 texCoord, Vector2 texelSize)
        {
            if (texCoord.X < 0.3f)
                return new Vector4(0.0f, 0.0f, 0.0f, 1.0f);
            else if (texCoord.X < 0.6f)
                return new Vector4(0.0f, 0.0f, 0.2f, 1.0f);
            else if (texCoord.X < 0.9f)
                return new Vector4(0.1f, 0.1f, 0.4f, 1.0f);
            return new Vector4(0.8f, 0.8f, 0.8f, 1);
        }

        protected override void Initialize()
        {
            ITexture celTexture = TextureFactory.CreateFromFunction(64, 1, 0, Usage.None, Format.A8R8G8B8, Pool.Managed, celMapCallback);

            cubes.Clear();

            string[] text1 = new string[] {
                "***** ***** **** ",
                "*   * *     *   *",
                "***** ***** *   *",
                "*   *     * *   *", 
                "*   * ***** **** ",
            };
            string[] text2 = new string[] {
                "**   **  ***  ***  *",
                "* * *  * *  * *    * ",
                "* * *  * ***  ***  * ",
                "* * *  * *    *      ",
                "**   **  *    ***  * ",
            };
            string[] text;
            if (Rand.Int(-1, 1) == 0)
                text = text1;
            else
                text = text2;

            const float epsilon = 0.01f;
            CreateStandardSceneAndCamera(out scene, out camera, 30);
            camera.WorldState.MoveUp(20);
            line = GraphicsFactory.CreateLine(Device);

            int yLength = text.Length;
            for (int y = 0; y < yLength; y++)
            {
                int xLength = text[y].Length;
                for (int x = 0; x < xLength; x++)
                {
                    if (text[y][x] == '*')
                    {
                        Vector3 pos = new Vector3((x - xLength / 2) * 2.4f, 20 - y * 2.4f, 0);
                        cubes.Add(CreateBox(pos, celTexture));
                        foreach (IPhysicalParticle particle in cubes[cubes.Count - 1].body.Particles)
                        {
                            particle.Position += new Vector3(
                                0, 0, Rand.Float(0, epsilon));
                        }
                    }
                }
            }

            CreateLights();
            //for (int i = 0; i < 2; i++)
            //{
            //    DirectionalLightNode light = new DirectionalLightNode("Light" + i);
            //    scene.AddNode(light);
            //    float x = 0;// i - 0.5f;
            //    float y = 0;// Rand.Float(-1, 1);
            //    float z = (i * 2) - 1;// Rand.Float(-1, 0);
            //    light.Direction = new Vector3(x, y, z);
            //}

            //MeshDirector director = new MeshDirector(MeshBuilder);
            //director.CreatePlane(100, 100, 1, 1, true);
            //director.Rotate((float)Math.PI / 2, 0, 0);
            //MeshBuilder.SetDiffuseTexture("Default1", "marble.jpg");
            //IModel model = director.Generate("Default1");
            //scene.AddNode(CreateSimpleModelNode(model, "TiVi.fxo", "Alpha"));

            chessBoard = new ChessBoard(scene, MeshBuilder, EffectFactory.CreateFromFile("TiVi.fxo"), Device, 10);
        }

        private void CreateLights()
        {
            PointLightNode[] lights = new PointLightNode[2];
            lights[0] = new PointLightNode("");
            lights[0].Position = new Vector3(-20, 8, 0);
            lights[0].DiffuseColor = new ColorValue(1.0f, 0.7f, 0.7f, 1.0f);
            lights[0].Range = 0.0003f;
            scene.AddNode(lights[0]);
            lights[1] = new PointLightNode("");
            lights[1].Position = new Vector3(20, 8, 0);
            lights[1].DiffuseColor = new ColorValue(0.7f, 0.7f, 1.0f, 1.0f);
            lights[1].Range = 0.0003f;
            scene.AddNode(lights[1]);
        }
        private PhysicalCube CreateBox(Vector3 pos, ITexture texture)
        {
            PhysicalCube cube = new PhysicalCube();
            const float stiffness = 1.0f;
            const float dragCoefficient = 0.05f;
            IBody body = new Body();
            body.AddParticle(new PhysicalParticle(new Vector3(-1, 1, 1) + pos, 1, dragCoefficient));
            body.AddParticle(new PhysicalParticle(new Vector3(1, 1, 1) + pos, 1, dragCoefficient));
            body.AddParticle(new PhysicalParticle(new Vector3(1, -1, 1) + pos, 1, dragCoefficient));
            body.AddParticle(new PhysicalParticle(new Vector3(-1, -1, 1) + pos, 1, dragCoefficient)); // forward
            body.AddParticle(new PhysicalParticle(new Vector3(-1, 1, -1) + pos, 1, dragCoefficient)); // up
            body.AddParticle(new PhysicalParticle(new Vector3(1, 1, -1) + pos, 1, dragCoefficient));
            body.AddParticle(new PhysicalParticle(new Vector3(1, -1, -1) + pos, 1, dragCoefficient)); // right
            body.AddParticle(new PhysicalParticle(new Vector3(-1, -1, -1) + pos, 1, dragCoefficient)); // origo

            body.AddConstraint(new StickConstraint(body.Particles[0], body.Particles[1], 2, stiffness));
            body.AddConstraint(new StickConstraint(body.Particles[1], body.Particles[2], 2, stiffness));
            body.AddConstraint(new StickConstraint(body.Particles[2], body.Particles[3], 2, stiffness));
            body.AddConstraint(new StickConstraint(body.Particles[3], body.Particles[0], 2, stiffness));

            body.AddConstraint(new StickConstraint(body.Particles[4], body.Particles[5], 2, stiffness));
            body.AddConstraint(new StickConstraint(body.Particles[5], body.Particles[6], 2, stiffness));
            body.AddConstraint(new StickConstraint(body.Particles[6], body.Particles[7], 2, stiffness));
            body.AddConstraint(new StickConstraint(body.Particles[7], body.Particles[4], 2, stiffness));

            body.AddConstraint(new StickConstraint(body.Particles[0], body.Particles[4], 2, stiffness));
            body.AddConstraint(new StickConstraint(body.Particles[1], body.Particles[5], 2, stiffness));
            body.AddConstraint(new StickConstraint(body.Particles[2], body.Particles[6], 2, stiffness));
            body.AddConstraint(new StickConstraint(body.Particles[3], body.Particles[7], 2, stiffness));

            body.AddConstraint(new StickConstraint(body.Particles[0], body.Particles[6], (float)Math.Sqrt(12)));
            body.AddConstraint(new StickConstraint(body.Particles[1], body.Particles[7], (float)Math.Sqrt(12)));
            body.AddConstraint(new StickConstraint(body.Particles[2], body.Particles[4], (float)Math.Sqrt(12)));
            body.AddConstraint(new StickConstraint(body.Particles[3], body.Particles[5], (float)Math.Sqrt(12)));

            foreach (IPhysicalParticle particle in body.Particles)
                body.AddConstraint(new YPosConstraint(0, particle));

            body.Gravity = new Vector3(0, -100.0f, 0);

            MeshDirector director = new MeshDirector(MeshBuilder);
            director.CreateChamferBox(2, 2, 2, 0.6f, 4);
            IModel model = director.Generate("Default1");
            model.Mesh.ComputeNormals();
            //model.Materials[0].DiffuseTexture = texture;
            model.Materials[0].DiffuseTexture = TextureFactory.CreateFromFile("marble.jpg");
            //model.Materials[0].AmbientColor = new ColorValue(0.1f, 0.1f, 0.6f);
            model.Materials[0].AmbientColor = new ColorValue(0.3f, 0.3f, 0.3f, 0.3f);
            model.Materials[0].DiffuseColor = new ColorValue(0.8f, 0.8f, 0.8f, 0.8f);
            model.Materials[0].ReflectiveFactor = 0.4f;
            model.Materials[0].ReflectiveTexture = TextureFactory.CreateCubeFromFile("rnl_cross.dds");
            //cube.model = CreateSimpleModelNode(model, "TiVi.fxo", "CelWithDoF");
            cube.model = CreateSimpleModelNode(model, "TiVi.fxo", "TiviChessPiece");
            cube.model.Position = pos;
            cube.body = body;
            cube.floorContact = false;
            cube.mirror = new MirrorNode(cube.model);
            cube.mirror.Brightness = 0.2f;
            return cube;
        }

        public override void Step()
        {
            Vector3 origo;
            float t = Time.StepTime * 0.1f;

            if (Time.StepTime - StartTime < 2.0f)
                ;//Initialize();
            else
            {
                foreach (PhysicalCube cube in cubes)
                {
                    cube.body.Step();
                    GetBodyBase(cube, out origo);
                    UpdateNode(cube, origo);
                }
                foreach (PhysicalCube cube in cubes)
                {
                    LimitNodeToFloor(cube);
                    CheckColidingCubes(cube);
                    UpdateBody(cube);
                }
            }

            camera.Position = new Vector3((float)Math.Sin(t), 0.3f, (float)Math.Cos(t)) * 60;
            camera.LookAt(new Vector3(), new Vector3(0, 1, 0));
            scene.Step();
        }

        private void CheckColidingCubes(PhysicalCube cube)
        {
            foreach (PhysicalCube secondCube in cubes)
            {
                if (cube != secondCube)
                {
                    Vector3 deltaVector = cube.model.Position - secondCube.model.Position;
                    float originalDistance = deltaVector.Length();
                    float deltaDistance = (2.4f - originalDistance);
                    if (deltaDistance > 0)
                    {
                        if (cube.floorContact || !secondCube.floorContact)
                        {
                            float delta = deltaDistance / (originalDistance * 2);
                            cube.model.Position += delta * deltaVector * 0.6f;
                            secondCube.model.Position -= delta * deltaVector * 1.4f;
                        }
                        else if (!cube.floorContact || secondCube.floorContact)
                        {
                            float delta = deltaDistance / (originalDistance * 2);
                            cube.model.Position += delta * deltaVector * 1.4f;
                            secondCube.model.Position -= delta * deltaVector * 0.6f;
                        }
                        else
                        {
                            float delta = deltaDistance / (originalDistance * 2);
                            cube.model.Position += delta * deltaVector;
                            secondCube.model.Position -= delta * deltaVector;
                        }
                    }
                }
            }
        }

        private void LimitNodeToFloor(PhysicalCube cube)
        {
            Vector3 origo = cube.model.Position;
            float y = 0;
            y = Math.Min(y, (origo - cube.right + cube.up + cube.forward).Y);
            y = Math.Min(y, (origo + cube.right + cube.up + cube.forward).Y);
            y = Math.Min(y, (origo + cube.right - cube.up + cube.forward).Y);
            y = Math.Min(y, (origo - cube.right - cube.up + cube.forward).Y);
            y = Math.Min(y, (origo - cube.right + cube.up - cube.forward).Y);
            y = Math.Min(y, (origo + cube.right + cube.up - cube.forward).Y);
            y = Math.Min(y, (origo + cube.right - cube.up - cube.forward).Y);
            y = Math.Min(y, (origo - cube.right - cube.up - cube.forward).Y);
            if (y < 0)
            {
                cube.floorContact = true;
                foreach (IPhysicalParticle particle in cube.body.Particles)
                    particle.DragCoefficient = 0.3f;
                origo.Y -= (y + 0);
            }
            cube.model.Position = origo;
        }

        private void UpdateBody(PhysicalCube cube)
        {
            Vector3 origo = cube.model.Position;
            cube.body.Particles[0].Position = origo - cube.right + cube.up + cube.forward;
            cube.body.Particles[1].Position = origo + cube.right + cube.up + cube.forward;
            cube.body.Particles[2].Position = origo + cube.right - cube.up + cube.forward;
            cube.body.Particles[3].Position = origo - cube.right - cube.up + cube.forward;
            cube.body.Particles[4].Position = origo - cube.right + cube.up - cube.forward;
            cube.body.Particles[5].Position = origo + cube.right + cube.up - cube.forward;
            cube.body.Particles[6].Position = origo + cube.right - cube.up - cube.forward;
            cube.body.Particles[7].Position = origo - cube.right - cube.up - cube.forward;
        }

        private static void GetBodyBase(PhysicalCube cube, out Vector3 origo)
        {
            origo = cube.body.Particles[7].Position;
            IPhysicalParticle upParticle = cube.body.Particles[4];
            IPhysicalParticle rightParticle = cube.body.Particles[6];
            cube.right = rightParticle.Position - origo;
            cube.up = upParticle.Position - origo;
            cube.forward = Vector3.Cross(cube.right, cube.up);
            cube.up = Vector3.Cross(cube.forward, cube.right);
            cube.forward.Normalize();
            cube.up.Normalize();
            cube.right.Normalize();

            //origo = new Vector3();
            //foreach (IPhysicalParticle particle in body.Particles)
            //    origo += particle.Position;
            //origo.Scale(1/8.0f);
        }

        private static void UpdateNode(PhysicalCube cube, Vector3 origo)
        {
            Matrix rot = new Matrix();
            rot.M11 = cube.right.X;
            rot.M12 = cube.right.Y;
            rot.M13 = cube.right.Z;
            rot.M21 = cube.up.X;
            rot.M22 = cube.up.Y;
            rot.M23 = cube.up.Z;
            rot.M31 = cube.forward.X;
            rot.M32 = cube.forward.Y;
            rot.M33 = cube.forward.Z;
            rot.M44 = 1;
            cube.model.WorldState.Rotation = rot;
            cube.model.WorldState.Position = origo + cube.up + cube.forward + cube.right;
        }

        public override void Render()
        {
            scene.SetEffectParameters();
            chessBoard.Render(scene);
            foreach (PhysicalCube cube in cubes)
            {
                //Matrix transform = camera.ViewMatrix * camera.ProjectionMatrix;
                //Vector3[] vecs = new Vector3[] { new Vector3(0, 0, 0), new Vector3(10, 0, 0) };
                //Vector3[] vertices = new Vector3[cube.body.Particles.Count];
                //for (int i = 0; i < vertices.Length; i++)
                //    vertices[i] = cube.body.Particles[i].Position;
                //line.Width = 1;
                //line.Begin();
                //foreach (IConstraint constraint in cube.body.Constraints)
                //{
                //    if (constraint is StickConstraint)
                //    {
                //        StickConstraint stick = constraint as StickConstraint;
                //        line.DrawTransform(new Vector3[] { stick.Particle1.Position, stick.Particle2.Position }, transform, Color.White);
                //    }
                //}
                //line.End();
                //cube.model.EffectHandler.Techniques[0] = EffectHandle.FromString("CelWithDoF");
                cube.model.Render(scene);
                //cube.model.EffectHandler.Techniques[0] = EffectHandle.FromString("CelWithDoFMirrored");
                cube.mirror.Render(scene);
            }
            scene.Render();
        }
    }
}
