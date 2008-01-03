using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.SceneGraph;
using Dope.DDXX.Graphics;
using Dope.DDXX.Utility;
using Dope.DDXX.MeshBuilder;
using System.Drawing;
using Microsoft.DirectX.Direct3D;

namespace EngineTest
{
    public class ModelEffect : BaseDemoEffect
    {
        private ModelNode node;
        private float xAngle;
        private float yAngle;
        private float zAngle;
        private CameraNode camera;
        private IScene scene;

        public ModelEffect(string name, float start, float end)
            : base(name, start, end)
        {
        }

        protected override void Initialize()
        {
            CreateStandardSceneAndCamera(out scene, out camera, 10);

            MeshBuilder.SetDiffuseTexture("Default1", "wings.bmp");
            MeshDirector meshDirector = new MeshDirector(MeshBuilder);
            meshDirector.CreateBox(2, 2, 2);
            IModel model = meshDirector.Generate("Default1");
            node = CreateSimpleModelNode(model, "PosseTest.fxo", "Tex");
            scene.AddNode(node);
            scene.AmbientColor = new ColorValue(1.0f, 1.0f, 1.0f, 1.0f);
            scene.Validate();


            //scene = new Scene();

            //IModel model = ModelFactory.FromFile("airplane 2.x", ModelOptions.EnsureTangents);
            //IEffect effect = EffectFactory.CreateFromFile("Test.fxo");
            //EffectHandler effectHandler = new EffectHandler(effect,
            //    delegate(int material) { return "Transparent"; }, model);
            //node = new ModelNode("Test Model", model, effectHandler, Device);
            //scene.AddNode(node);

            //// Create camera
            //CameraNode camera = new CameraNode("Test Camera");
            //camera.WorldState.MoveForward(-10);
            //scene.AddNode(camera);
            //scene.ActiveCamera = camera;
        }

        public float XAngle
        {
            set { xAngle = value; }
            get { return xAngle; }
        }

        public float YAngle
        {
            set { yAngle = value; }
            get { return yAngle; }
        }

        public float ZAngle
        {
            set { zAngle = value; }
            get { return zAngle; }
        }

        public override void Step()
        {
            Microsoft.DirectX.Matrix rot = node.WorldState.Rotation;
            rot.RotateX(xAngle);
            rot.RotateY(yAngle);
            rot.RotateZ(zAngle);
            node.WorldState.Rotation = rot;
            scene.Step();
        }

        public override void Render()
        {
            scene.Render();
        }
    }
}
