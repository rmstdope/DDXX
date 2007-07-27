using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.SceneGraph;
using Microsoft.DirectX.Direct3D;
using Dope.DDXX.Graphics;
using Microsoft.DirectX;
using System.Drawing;
using Dope.DDXX.Utility;

namespace TiVi
{
    public class ThinkTank : BaseDemoEffect
    {
        private IScene scene;
        private CameraNode camera;
        private ILine line;
        private ITexture screenTexture;
        private IDemoEffect subEffect;

        private Vector3 upperLeft;
        private Vector3 upperRight;
        private Vector3 lowerLeft;
        private Vector3 center;
        private Vector3 normal;
        private LineNode lineNode;
        private Vector3 destinationPos;
        private Vector3 destinationUp;
        private Interpolator<InterpolatedVector3> cameraInterpolator;

        public ThinkTank(string name, float startTime, float endTime)
            : base(name, startTime, endTime)
        {
        }

        protected override void Initialize()
        {
            line = GraphicsFactory.CreateLine(Device);
            CreateStandardSceneAndCamera(out scene, out camera, 3);
            camera.WorldState.MoveUp(1.5f);

            XLoader.Load("Tivi-Sitting.X", EffectFactory.CreateFromFile("TiVi.fxo"),
                delegate(string name)
                {
                    if (name == "TiVi")
                    {
                        return delegate(int material)
                        {
                            if (material == 1)
                                return "TvScreen";
                            else
                                return "Solid";
                        };
                    }
                    else
                        return TechniqueChooser.MaterialPrefix("Terrain");
                });
            XLoader.AddToScene(scene);
            //scene.GetNodeByName("TiVi").WorldState.Position = new Vector3(0, 0.25f, 0);

            ExtractTiViInfo();

            cameraInterpolator = new Interpolator<InterpolatedVector3>();
            ClampedCubicSpline<InterpolatedVector3> spline = new ClampedCubicSpline<InterpolatedVector3>(
                new InterpolatedVector3(), new InterpolatedVector3());
            spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(0, new InterpolatedVector3(new Vector3(-2.0f, 3.0f, -2.0f))));
            //spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(1, new InterpolatedVector3(new Vector3(0.0f, 2.0f, -3.0f))));
            //spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(2, new InterpolatedVector3(new Vector3(1.0f, 1.0f, -3.0f))));
            spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(3, new InterpolatedVector3(new Vector3(2.0f, 1.0f, -2.0f))));
            spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(5, new InterpolatedVector3(destinationPos)));
            spline.Calculate();
            cameraInterpolator.AddSpline(spline);

            subEffect = new ChessScene("screeneffect", EndTime, EndTime);
            subEffect.Initialize(GraphicsFactory, EffectFactory, Device, Mixer, PostProcessor);
            screenTexture = TextureFactory.CreateFullsizeRenderTarget();
            (scene.GetNodeByName("TiVi") as ModelNode).Model.Materials[1].DiffuseTexture = screenTexture;

            Time.Pause();
            Time.CurrentTime = EndTime;
            subEffect.Step();
            using (ISurface original = Device.GetRenderTarget(0))
            {
                using (ISurface surface = screenTexture.GetSurfaceLevel(0))
                {
                    Device.SetRenderTarget(0, surface);
                    Device.BeginScene();
                    Device.Clear(ClearFlags.Target | ClearFlags.ZBuffer | ClearFlags.Stencil, Color.Black, 1, 0);
                    subEffect.Render();
                    Device.EndScene();
                    Device.SetRenderTarget(0, original);
                }
            }
            Time.Resume();
        }

        private void ExtractTiViInfo()
        {
            ModelNode tiviNode = scene.GetNodeByName("TiVi") as ModelNode;
            IMesh mesh = tiviNode.Model.Mesh;
            VertexElement[] elements = tiviNode.Model.Mesh.Declaration;
            VertexFormats format = tiviNode.Model.Mesh.VertexFormat;
            AttributeRange range = tiviNode.Model.Mesh.GetAttributeTable()[1];
            short[] indices;// = new short[range.FaceCount];
            indices = mesh.IndexBuffer.Lock(range.FaceStart * 3 * sizeof(short), typeof(short), LockFlags.ReadOnly, new int[] { range.FaceCount * 3 }) as short[];
            mesh.IndexBuffer.Unlock();
            TiViVertex[] vertices;
            vertices = mesh.VertexBuffer.Lock(0, typeof(TiViVertex), LockFlags.ReadOnly, new int[] { mesh.NumberVertices }) as TiViVertex[];
            mesh.VertexBuffer.Unlock();

            int i1 = 0;
            int i2 = 1;
            int i3 = 2;
            upperLeft = GetScreenPosition(new Vector2(0, 0), vertices[indices[i1]], vertices[indices[i2]], vertices[indices[i3]]);
            upperRight = GetScreenPosition(new Vector2(1, 0), vertices[indices[i1]], vertices[indices[i2]], vertices[indices[i3]]);
            lowerLeft = GetScreenPosition(new Vector2(0, 1), vertices[indices[i1]], vertices[indices[i2]], vertices[indices[i3]]);

            center = (lowerLeft + upperRight) * 0.5f;// new Vector3((upperLeft.mX + upperRight.mX) / 2.0f, (upperLeft.mY + lowerLeft.mY) / 2.0f, lowerLeft.mZ);
            normal = Vector3.Cross(upperRight - upperLeft, lowerLeft - upperLeft);
            normal.Normalize();

            //Matrix[] m = (tiviNode.Model as SkinnedModel).GetBoneMatrices(1);
            float oppositeLength = (upperLeft - lowerLeft).Length() / 2;
            float closeLength = oppositeLength / (float)Math.Tan(camera.GetFOV() / 2);
            //camera.Position = center + normal * closeLength * 1.0f;// new Vector3(1, 0, 0);
            destinationPos = center + normal * closeLength * 1.0f;
            //camera.LookAt(center, new Vector3(0, 1, 0));

            Vector3 right = (upperRight - upperLeft);
            right.Normalize();
            destinationUp = (upperLeft - lowerLeft);
            destinationUp.Normalize();
            //Matrix rot = new Matrix();
            //rot.M11 = right.X;
            //rot.M12 = right.Y;
            //rot.M13 = right.Z;
            //rot.M21 = up.X;
            //rot.M22 = up.Y;
            //rot.M23 = up.Z;
            //rot.M31 = -normal.X;
            //rot.M32 = -normal.Y;
            //rot.M33 = -normal.Z;
            //rot.M44 = 1;
            //camera.WorldState.Rotation = rot;

            lineNode = new LineNode("Line", line, upperLeft, upperRight, Color.Blue);
        }

        private Vector3 GetScreenPosition(Vector2 destUV, TiViVertex v1, TiViVertex v2, TiViVertex v3)
        {
            Vector3 p1 = v1.Position;
            Vector2 t1 = new Vector2(v1.U, v1.V);
            Vector3 p2 = v2.Position;
            Vector2 t2 = new Vector2(v2.U, v2.V);
            Vector3 p3 = v3.Position;
            Vector2 t3 = new Vector2(v3.U, v3.V);

            Vector2 tv1 = t2 - t1;
            Vector2 tv2 = t3 - t1;

            float c2 = (((t1.Y - destUV.Y) * tv1.X / tv1.Y) - (t1.X - destUV.X)) / (tv2.X - (tv2.Y * tv1.X) / tv1.Y);
            float c1 = (-(t1.X - destUV.X) - c2 * tv2.X) / tv1.X;

            Vector3 pv1 = p2 - p1;
            Vector3 pv2 = p3 - p1;

            return p1 + c1 * pv1 + c2 * pv2;
        }

        public override void Step()
        {
            camera.Position = cameraInterpolator.GetValue(Time.StepTime - StartTime);
            camera.LookAt(center, destinationUp);

            scene.Step();
        }

        public override void Render()
        {
            scene.Render();
            //Device.RenderState.ZBufferEnable = false;
            //lineNode.Render(scene);
        }
    }
}
