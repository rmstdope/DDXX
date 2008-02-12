using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.SceneGraph;
using Dope.DDXX.Graphics;
using Dope.DDXX.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Dope.DDXX.ModelBuilder;
using Dope.DDXX.TextureBuilder;

namespace EngineTest
{
    public class LineEffect : BaseDemoEffect
    {
        private IScene scene;
        private IModel model;
        private CameraNode camera;
        private ModelNode node;
        private MirrorNode mirror;
        private ModelNode genNode;
        private UserPrimitive<VertexPositionTexture> primitive;
        private MaterialHandler material;

        public LineEffect(string name, float start, float end)
            : base(name, start, end)
        {
        }

        protected override void Initialize()
        {
            CreateStandardSceneAndCamera(out scene, out camera, 15);

            material = new MaterialHandler(EffectFactory.CreateFromFile("Content\\effects\\DefaultEffect"), new EffectConverter());
            material.DiffuseTexture = TextureFactory.WhiteTexture;

            primitive = new UserPrimitive<VertexPositionTexture>(GraphicsFactory.CreateVertexDeclaration(VertexPositionTexture.VertexElements), material, PrimitiveType.LineStrip, 3);
        }


        public override void Step()
        {
            //Mixer.ClearColor = Color.AntiqueWhite;
            scene.Step();
        }

        public override void Render()
        {
            scene.Render();
            material.SetupRendering(new Matrix[] { Matrix.Identity }, scene.ActiveCamera.ViewMatrix,
                scene.ActiveCamera.ProjectionMatrix, scene.AmbientColor, new LightState());
            GraphicsDevice.RenderState.CullMode = CullMode.None;
            primitive.Begin();
            primitive.AddVertex(new VertexPositionTexture(new Vector3(0, 0, 0), new Vector2(0, 0)));
            primitive.AddVertex(new VertexPositionTexture(new Vector3(10, 0, 0), new Vector2(1, 0)));
            primitive.AddVertex(new VertexPositionTexture(new Vector3(10, -10, 0), new Vector2(1, 1)));
            primitive.End();
        }
    }
}
