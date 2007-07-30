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
            public bool active;
        }

        //private ILine line;
        private IScene scene;
        private CameraNode camera;
        private List<PhysicalCube> cubes = new List<PhysicalCube>();
        private ChessBoard chessBoard;
        protected Interpolator<InterpolatedVector3> cameraInterpolator;
        protected Interpolator<InterpolatedVector3> cameraTargetInterpolator;
        protected Interpolator<InterpolatedVector3> focalTargetInterpolator;
        private IEffect effect;
        private float focalDistance;
        private float hyperfocalDistance;
        private float loopTime;

        public float FocalDistance
        {
            get { return focalDistance; }
            set { focalDistance = value; }
        }

        public float HyperfocalDistance
        {
            get { return hyperfocalDistance; }
            set { hyperfocalDistance = value; }
        }

        public PhysicalCubes(string name, float start, float end)
            : base(name, start, end)
        {
            focalDistance = 40;
            hyperfocalDistance = 0.1f;
            SetStepSize(GetTweakableNumber("HyperfocalDistance"), 0.01f);
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
            //ITexture celTexture = TextureFactory.CreateFromFunction(64, 1, 0, Usage.None, Format.A8R8G8B8, Pool.Managed, celMapCallback);

            CreateStandardSceneAndCamera(out scene, out camera, 30);
            camera.SetFOV((float)Math.PI * 0.3f);

            cubes.Clear();
            for (int i = 0; i < GetMaxNumCubes(); i++)
                cubes.Add(CreateBox(new Vector3(1000, 0, 0)));
            ResetText(0);

            CreateLights();
            CreateSplines();

            effect = EffectFactory.CreateFromFile("TiVi.fxo");
            chessBoard = new ChessBoard(scene, MeshBuilder, effect, Device, 10);
        }

        private void CreateSplines()
        {
            loopTime = 8;

            cameraInterpolator = new Interpolator<InterpolatedVector3>();
            ClampedCubicSpline<InterpolatedVector3> spline = new ClampedCubicSpline<InterpolatedVector3>(new InterpolatedVector3(), new InterpolatedVector3());
            spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(0, new InterpolatedVector3(new Vector3(-60, 20, -50.0f))));
            spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(3, new InterpolatedVector3(new Vector3(0, 20, -55.0f))));
            spline.Calculate();
            cameraInterpolator.AddSpline(spline);
            spline = new ClampedCubicSpline<InterpolatedVector3>(new InterpolatedVector3(), new InterpolatedVector3());
            spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(4, new InterpolatedVector3(new Vector3(0, 20, -55.0f))));
            spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(5, new InterpolatedVector3(new Vector3(0, 4, 0.0f))));
            spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(6, new InterpolatedVector3(new Vector3(-10, 6, 5.0f))));
            spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(7, new InterpolatedVector3(new Vector3(-60, 20, -50.0f))));
            spline.Calculate();
            cameraInterpolator.AddSpline(spline);

            cameraTargetInterpolator = new Interpolator<InterpolatedVector3>();
            spline = new ClampedCubicSpline<InterpolatedVector3>(new InterpolatedVector3(), new InterpolatedVector3());
            spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(0, new InterpolatedVector3(new Vector3(0.0f, 10.0f, 0.0f))));
            spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(4, new InterpolatedVector3(new Vector3(0.0f, 6.0f, 0.01f))));
            spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(5, new InterpolatedVector3(new Vector3(10, 4, 30.0f))));
            spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(6, new InterpolatedVector3(new Vector3(10, 5, 30.0f))));
            spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(7, new InterpolatedVector3(new Vector3(0.0f, 10.0f, 0.0f))));
            spline.Calculate();
            cameraTargetInterpolator.AddSpline(spline);

            focalTargetInterpolator = new Interpolator<InterpolatedVector3>();
            spline = new ClampedCubicSpline<InterpolatedVector3>(new InterpolatedVector3(), new InterpolatedVector3());
            //spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(0, new InterpolatedVector3(new Vector3(-30, 20, -39.0f))));
            //spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(0, new InterpolatedVector3(new Vector3(0.0f, 15.0f, 0.0f))));
            spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(6, new InterpolatedVector3(new Vector3(0.0f, 15.0f, 0.0f))));
            spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(7, new InterpolatedVector3(new Vector3(-30, 20, -39.0f))));
            spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(8, new InterpolatedVector3(new Vector3(0.0f, 15.0f, 0.0f))));
            spline.Calculate();
            focalTargetInterpolator.AddSpline(spline);
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
        private PhysicalCube CreateBox(Vector3 pos)
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
            //director.CreateChamferBox(2, 2, 2, 0.6f, 4);
            director.CreateBox(2, 2, 2);
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
            cube.active = false;
            cube.mirror = new MirrorNode(cube.model);
            cube.mirror.Brightness = 0.4f;
            return cube;
        }

        public override void Step()
        {
            Vector3 origo;
            //float t = Time.StepTime * 0.1f;

            float t = (Time.StepTime - StartTime) % loopTime;
            int loop = (int)((Time.StepTime - StartTime) / loopTime);
            if (t > loopTime - 2)
                ResetText(loop + 1);
            else if (t > 2.5f)
            {
                foreach (PhysicalCube cube in cubes)
                    if (cube.active)
                        cube.body.Step();
            }
            foreach (PhysicalCube cube in cubes)
            {
                GetBodyBase(cube, out origo);
                UpdateNode(cube, origo);
            }
            foreach (PhysicalCube cube in cubes)
            {
                LimitNodeToFloor(cube);
                CheckColidingCubes(cube);
                UpdateBody(cube);
            }

            camera.WorldState.Position = cameraInterpolator.GetValue(t);
            camera.LookAt(cameraTargetInterpolator.GetValue(t), new Vector3(0, 1, 0));

            //camera.Position = new Vector3((float)Math.Sin(t), 0.3f, (float)Math.Cos(t)) * 60;
            //camera.LookAt(new Vector3(), new Vector3(0, 1, 0));
            Vector3 position = cameraInterpolator.GetValue(t);
            Vector3 target = focalTargetInterpolator.GetValue(t);
            Vector3 dir = target - position;
            float dist = dir.Length();
            dir.Normalize();
            Vector4 vec = new Vector4(0, 0, 1, -dist);
            //Vector4 vec = new Vector4(dir.X, dir.Y, dir.Z, -dist);
            effect.SetValue(EffectHandle.FromString("FocalPlane"), vec);
            effect.SetValue(EffectHandle.FromString("HyperfocalDistance"), hyperfocalDistance);
            
            scene.Step();
        }

        private void ResetText(int num)
        {
            string text = GetText(num);
            float xLength = GetXLength(text);
            float yLength = GetYLength(text);
            const float epsilon = 0.01f;
            int cubeNum = 0;
            int yPos = 0;
            int xPos = 0;
            const float cubeDist = 2.4f;
            for (int j = 0; j < text.Length; j++)
            {
                if (text[j] == 'x')
                {
                    xPos = 0;
                    yPos++;
                }
                else
                {
                    if (text[j] == '*')
                    {
                        Vector3 pos = new Vector3((xPos - xLength / 2), (yLength - yPos + 2), 0);
                        pos *= cubeDist;
                        List<IPhysicalParticle> particles = cubes[cubeNum].body.Particles;
                        foreach (IPhysicalParticle particle in particles)
                        {
                            particle.Reset();
                        }
                        particles[0].Position = new Vector3(-1, 1, 1) + pos;
                        particles[1].Position = new Vector3(1, 1, 1) + pos;
                        particles[2].Position = new Vector3(1, -1, 1) + pos;
                        particles[3].Position = new Vector3(-1, -1, 1) + pos;
                        particles[4].Position = new Vector3(-1, 1, -1) + pos;
                        particles[5].Position = new Vector3(1, 1, -1) + pos;
                        particles[6].Position = new Vector3(1, -1, -1) + pos;
                        particles[7].Position = new Vector3(-1, -1, -1) + pos;
                        foreach (IPhysicalParticle particle in particles)
                        {
                            particle.OldPosition = particle.Position;
                            particle.Position += new Vector3(0, 0, Rand.Float(0, epsilon));
                            particle.DragCoefficient = 0.05f;
                        }
                        cubes[cubeNum].floorContact = false;
                        cubes[cubeNum].active = true;
                        cubeNum++;
                    }
                    xPos++;
                }
            }
        }

        private float GetYLength(string text)
        {
            float yNum = 0;
            for (int j = 0; j < text.Length; j++)
            {
                if (text[j] == 'x')
                    yNum++;
            }
            return yNum;
        }

        private float GetXLength(string text)
        {
            float xMax = 0;
            int xPos = 0;
            int yPos = 0;
            for (int j = 0; j < text.Length; j++)
            {
                if (text[j] == 'x')
                {
                    if (xPos > xMax)
                        xMax = xPos;
                    xPos = 0;
                    yPos++;
                }
                else
                    xPos++;
            }
            return xMax;
        }

        private string[] allTexts = new string[] {
            "***** ***** **** x" +
            "*   * *     *   *x" +
            "***** ***** *   *x" +
            "*   *     * *   *x" +
            "*   * ***** **** x" +
            "xxxx",

            "    *** * * ***x" +
            "    * * * *  * x" +
            "    * * * *  * x" +
            "    * * * *  * x" +
            "    *** ***  * x" +
            "x" +
            "**  **  *** *** * *x" +
            "* * * * *   * * * *x" +
            "**  **  *** *** ** x" +
            "* * * * *   * * * *x" +
            "**  * * *** * * * *x",
                
            "***** ***** *****x" +
            "*       *   *    x" +
            "*****   *   *****x" +
            "    *   *       *x" +
            "*****   *   *****x" +
            "xxxx",

            "  *** *** *** ** x" +
            "  *   * * * * * *x" +
            "  *** *** **  ** x" +
            "  *   * * * * * *x" +
            "  *   * * * * ** x" +
            "x" +
            "*** *** * * *** * *x" +
            "* * * * * * *   * *x" +
            "**  *** * * *   ***x" +
            "* * * * * * *   * *x" +
            "* * * * *** *** * *",

            "***** ****  *    x" +
            "  *   *   * *    x" +
            "  *   ****  *    x" +
            "  *   *   * *    x" +
            "  *   ****  *****x" +
            "xxxx",

            };

        private string GetText(int num)
        {
            return allTexts[num];
        }

        private int GetMaxNumCubes()
        {
            int max = 0;
            for (int i = 0; i < allTexts.Length; i++)
            {
                int num = 0;
                for (int j = 0; j < allTexts[i].Length; j++)
                {
                    if (allTexts[i][j] == '*')
                        num++;
                }
                if (num > max)
                    max = num;
            }
            return max;
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
            //chessBoard.Render(scene);
            foreach (PhysicalCube cube in cubes)
            {
                //DrawLineCube(cube);
                cube.model.Render(scene);
                cube.mirror.Render(scene);
            }
            scene.Render();
        }

        private void DrawLineCube(PhysicalCube cube)
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
        }
    }
}
