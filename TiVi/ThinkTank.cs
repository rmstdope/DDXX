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

        private Interpolator<InterpolatedVector3> cameraInterpolator;

        private TiVi tivi;

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
                                return "TiViReflective";
                        };
                    }
                    else
                        return TechniqueChooser.MaterialPrefix("Terrain");
                });
            XLoader.AddToScene(scene);
            ModelNode tiviNode = scene.GetNodeByName("TiVi") as ModelNode;
            tivi = new TiVi(tiviNode, camera, StartTime);
            tiviNode.Model.Materials[0].ReflectiveTexture = TextureFactory.CreateCubeFromFile("rnl_cross.dds");
            tiviNode.Model.Materials[0].DiffuseTexture = TextureFactory.CreateFromFile("marble.jpg");
            tiviNode.Model.Materials[0].AmbientColor = new ColorValue(0.5f, 0.5f, 0.5f, 0.5f);
            tiviNode.Model.Materials[0].DiffuseColor = new ColorValue(0.5f, 0.5f, 0.5f, 0.5f);
            tiviNode.Model.Materials[0].ReflectiveFactor = 0.2f;

            cameraInterpolator = new Interpolator<InterpolatedVector3>();
            ClampedCubicSpline<InterpolatedVector3> spline = new ClampedCubicSpline<InterpolatedVector3>(
                new InterpolatedVector3(), new InterpolatedVector3());
            spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(0, new InterpolatedVector3(new Vector3(-2.0f, 3.0f, -2.0f))));
            //spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(1, new InterpolatedVector3(new Vector3(0.0f, 2.0f, -3.0f))));
            //spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(2, new InterpolatedVector3(new Vector3(1.0f, 1.0f, -3.0f))));
            spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(3, new InterpolatedVector3(new Vector3(2.0f, 1.0f, -2.0f))));
            spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(5, new InterpolatedVector3(tivi.DestinationPos)));
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

        public override void Step()
        {
            camera.Position = cameraInterpolator.GetValue(Time.StepTime - StartTime);
            camera.LookAt(tivi.Center, tivi.DestinationUp);

            scene.Step();
        }

        public override void Render()
        {
            scene.Render();
        }
    }
}
