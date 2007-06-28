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
        private Scene scene;
        private ModelNode node;
        MeshBuilder meshBuilder;
        private float xAngle;
        private float yAngle;
        private float zAngle;

        public ModelEffect(float start, float end)
            : base(start, end)
        {
        }

        protected override void Initialize()
        {
            meshBuilder = new MeshBuilder(D3DDriver.GraphicsFactory, D3DDriver.TextureFactory,
                D3DDriver.GetInstance().Device);
            scene = new Scene();

            CreatePlane();

            // Create camera
            CameraNode camera = new CameraNode("Test Camera");
            camera.WorldState.MoveForward(-10);
            scene.AddNode(camera);
            scene.ActiveCamera = camera;

            scene.AmbientColor = new ColorValue(1.0f, 1.0f, 1.0f, 1.0f);
        }

        private void CreatePlane()
        {
            ModelMaterial material = new ModelMaterial(new Material());
            material.DiffuseTexture = TextureFactory.CreateFromFile("wings.bmp");
            //meshBuilder.CreateSphere("Box", 3, 4, 2);
//            meshBuilder.CreatePlane("Box", 3, 3, 6, 6, true);
//            meshBuilder.AssignMaterial("Box", "Default1");
            meshBuilder.SetDiffuseTexture("Default1", "wings.bmp");
            //meshBuilder.SetReflectiveTexture("Default1", "rnl_cross.dds");
            //meshBuilder.SetReflectiveFactor("Default1", 0.02f);
            IModel boxModel = null;// meshBuilder.CreateModel("Box");
            boxModel.Materials[0].DiffuseColor = ColorValue.FromColor(Color.Gray);
            boxModel.Materials[0].AmbientColor = ColorValue.FromColor(Color.MediumAquamarine);
            node = new ModelNode("Box", boxModel,
                new EffectHandler(EffectFactory.CreateFromFile("../../Effects/PosseTest.fxo"),
                TechniqueChooser.MaterialPrefix("Tex"), boxModel));
            scene.AddNode(node);
            node.WorldState.MoveUp(1f);
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
            //Microsoft.DirectX.Matrix rot = node.WorldState.Rotation;
            //rot.X = XAngle;
            //rot.Y = YAngle;
            //rot.Z = ZAngle;
            //node.WorldState.Rotation = rot;
            scene.Step();
        }

        public override void Render()
        {
            scene.Render();
        }
    }
}
