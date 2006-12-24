using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.Graphics;
using Dope.DDXX.Physics;
using Dope.DDXX.SceneGraph;
using Dope.DDXX.Utility;
using System.Diagnostics;
using System.Drawing;

namespace PoseidonTest
{
    public class RealRenderPostEffect : BaseDemoPostEffect
    {
        private Scene scene;
        private PosseTestEffect demoEffect;

        public RealRenderPostEffect(float startTime, float endTime)
            : base(startTime, endTime)
        {
        }

        public void SetPosseTestEffect(PosseTestEffect posseEffect)
        {
            this.demoEffect = posseEffect;
        }

        public override void Render()
        {
            TextureID tempTextureID = TextureID.FULLSIZE_TEXTURE_1;
            if (PostProcessor.OutputTextureID == TextureID.FULLSIZE_TEXTURE_1)
            {
                tempTextureID = TextureID.FULLSIZE_TEXTURE_2;
            }
            //ITexture tempTexture = PostProcessor.GetTexture(tempTextureID);
            
            //GraphicsStream stream = surface.LockRectangle(LockFlags.ReadOnly);
            //ITexture renderData = D3DDriver.TextureFactory.CreateFullsizeRenderTarget();

            //demoEffect.GetDevice().GetRenderTargetData(PostProcessor.OutputTexture.GetSurfaceLevel(0),
            //    renderData.GetSurfaceLevel(0));
            //ISurface surface = renderData.GetSurfaceLevel(0);
            //SurfaceDescription description = surface.Description;

            demoEffect.SourceTexture = PostProcessor.OutputTexture;

            PostProcessor.SetBlendParameters(BlendOperation.Add, Blend.One, Blend.Zero, Color.Black);
            PostProcessor.Process("Monochrome", PostProcessor.OutputTextureID, tempTextureID);
            
                ISurface oldTarget = demoEffect.GetDevice().GetRenderTarget(0);
            //demoEffect.GetDevice().SetRenderTarget(0, tempTexture.GetSurfaceLevel(0));
            demoEffect.GetDevice().SetRenderTarget(0, PostProcessor.OutputTexture.GetSurfaceLevel(0));
            demoEffect.GetDevice().Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.Brown, 1000, 0);
            demoEffect.GetDevice().BeginScene();
            demoEffect.RealRender();
            demoEffect.GetDevice().EndScene();
            demoEffect.GetDevice().SetRenderTarget(0, oldTarget);

        }

    }

    public class PosseTestEffect : BaseDemoEffect
    {
        private ModelNode virtualModelNode;
        private ModelNode modelNode;
        private Scene scene;
        private Scene virtualScene;
        private ITexture targetTexture;
        private IEffect effect;
        private EffectHandle sourceTextureHandle;

        public PosseTestEffect(float startTime, float endTime)
            : base(startTime, endTime)
        {
            scene = new Scene();
            virtualScene = new Scene();
        }

        public ITexture SourceTexture
        {
            set
            {
                effect.SetValue(sourceTextureHandle, value);
            }
        }

        public override void Initialize()
        {
            base.Initialize();

            InitializeVirtualScene();

            sourceTextureHandle = effect.GetParameter(null, "PosseSourceTexture");

            InitializeScene();
        }

        private void InitializeVirtualScene()
        {
            ModelMaterial material = new ModelMaterial(new Material());
            //material.DiffuseTexture = TextureFactory.CreateFromFile("wings.bmp");
            // targetTexture = TextureFactory.CreateFullsizeRenderTarget();
            material.DiffuseColor = ColorValue.FromColor(Color.Blue);
            material.Ambient = Color.SteelBlue;
            IModel model = ModelFactory.CreateBox(50, 50, 50);
            model.Materials = new ModelMaterial[] { material };

            effect = EffectFactory.CreateFromFile("../../Effects/PosseTest.fxo");
            EffectHandler effectHandler = new EffectHandler(effect, "NoTex");
            virtualModelNode = new ModelNode("Mesh", model, effectHandler);
            virtualScene.AddNode(virtualModelNode);
            virtualScene.AmbientColor = new ColorValue(1.0f, 1.0f, 1.0f, 1.0f);
            //Light dxLight = new Light();
            //dxLight.DiffuseColor = new ColorValue(1.0f, 1.0f, 1.0f, 1.0f);
            //dxLight.Type = LightType.Point;
            //light = new LightNode("Point Light", dxLight);
            //scene.AddNode(light);

            CameraNode camera = new CameraNode("Camera");
            camera.WorldState.MoveForward(-150.0f);
            virtualScene.AddNode(camera);
            virtualScene.ActiveCamera = camera;
        }

        private void InitializeScene()
        {
            ModelMaterial material = new ModelMaterial(new Material());
            material.DiffuseTexture = TextureFactory.CreateFromFile("wings.bmp");
            material.DiffuseColor = ColorValue.FromColor(Color.Blue);
            material.Ambient = Color.SteelBlue;
            SourceTexture = material.DiffuseTexture;
            IModel model = TexturedBox(ModelFactory.CreateBox(100, 100, 1), new ModelMaterial[] { material });
            IEffect effect = EffectFactory.CreateFromFile("../../Effects/PosseTest.fxo");
            EffectHandler effectHandler = new EffectHandler(effect, "Tex");
            modelNode = new ModelNode("Mesh", model, effectHandler);
            modelNode.WorldState.Tilt(4.0f);
            scene.AddNode(modelNode);
            scene.AmbientColor = new ColorValue(1.0f, 1.0f, 1.0f, 1.0f);
            CameraNode camera = new CameraNode("Camera");
            camera.WorldState.MoveForward(-150.0f);
            scene.AddNode(camera);
            scene.ActiveCamera = camera;
        }

        private IMesh CreateBox()
        {
            IMesh mesh;
            short[] arrayIndices = new short[12];
            CustomVertex.PositionNormal[] arrayVertices = new CustomVertex.PositionNormal[8];
            mesh = D3DDriver.Factory.CreateMesh(4, 8, MeshFlags.Managed, CustomVertex.PositionNormal.Format, Device);

            arrayVertices[0] = new CustomVertex.PositionNormal(-10, -10, -10, 0, 0, -1);
            arrayVertices[1] = new CustomVertex.PositionNormal(10, -10, -10, 0, 0, -1);
            arrayVertices[2] = new CustomVertex.PositionNormal(10, 10, -10, 0, 0, -1);
            arrayVertices[3] = new CustomVertex.PositionNormal(-10, 10, -10, 0, 0, -1);
            arrayVertices[4] = new CustomVertex.PositionNormal(10, -10, 10, 0, 0, 1);
            arrayVertices[5] = new CustomVertex.PositionNormal(-10, -10, 10, 0, 0, 1);
            arrayVertices[6] = new CustomVertex.PositionNormal(-10, 10, 10, 0, 0, 1);
            arrayVertices[7] = new CustomVertex.PositionNormal(10, 10, 10, 0, 0, 1);
            int i = 0;
            arrayIndices[i++] = 0;
            arrayIndices[i++] = 3;
            arrayIndices[i++] = 1;
            arrayIndices[i++] = 1;
            arrayIndices[i++] = 3;
            arrayIndices[i++] = 2;
            arrayIndices[i++] = 4;
            arrayIndices[i++] = 7;
            arrayIndices[i++] = 5;
            arrayIndices[i++] = 5;
            arrayIndices[i++] = 7;
            arrayIndices[i++] = 6;
            AttributeRange attributeRange = new AttributeRange();
            attributeRange.AttributeId = 0;
            attributeRange.FaceCount = 4;
            attributeRange.FaceStart = 0;
            attributeRange.VertexStart = 0;
            attributeRange.VertexCount = 8;

            mesh.VertexBuffer.SetData(arrayVertices, 0, LockFlags.None);
            mesh.IndexBuffer.SetData(arrayIndices, 0, LockFlags.None);
            mesh.SetAttributeTable(new AttributeRange[] { attributeRange });

            return mesh;
        }

        private Model TexturedBox(IModel model, ModelMaterial[] materials)
        {
            IMesh mesh = model.Mesh;
            Debug.Assert(mesh.NumberFaces == 12 && mesh.NumberVertices == 24);
            IMesh texturedMesh = mesh.Clone(mesh.Options.Value,
                VertexFormats.Position | VertexFormats.Normal |
                VertexFormats.Texture0 | VertexFormats.Texture1,
                new DeviceAdapter(mesh.Device));
            SetBoxTextureCoordinates(texturedMesh);
            return new Model(texturedMesh, materials);
        }

        private void SetBoxTextureCoordinates(IMesh texturedMesh)
        {
            using (VertexBuffer vb = texturedMesh.VertexBuffer)
            {
                CustomVertex.PositionNormalTextured[] verts =
                    (CustomVertex.PositionNormalTextured[])
                    vb.Lock(0, typeof(CustomVertex.PositionNormalTextured), LockFlags.None, texturedMesh.NumberVertices);
                try
                {
                    for (int i = 0; i < verts.Length; i += 4)
                    {
                        verts[i + 0].Tu = 0.0f;
                        verts[i + 0].Tv = 0.0f;
                        verts[i + 1].Tu = 1.0f;
                        verts[i + 1].Tv = 0.0f;
                        verts[i + 2].Tu = 1.0f;
                        verts[i + 2].Tv = 1.0f;
                        verts[i + 3].Tu = 0.0f;
                        verts[i + 3].Tv = 1.0f;
                    }
                }
                finally
                {
                    vb.Unlock();
                }
            }
        }

        public override void StartTimeUpdated()
        {
        }

        public override void EndTimeUpdated()
        {
        }

        public override void Step()
        {
            scene.Step();
            virtualScene.Step();
            virtualModelNode.WorldState.Roll(Time.DeltaTime * 0.5f);
            virtualModelNode.WorldState.Turn(Time.DeltaTime * 0.3f);
            //mesh.WorldState.Turn(Time.DeltaTime * 0.5f);
            //light.WorldState.Position = new Vector3(500.0f * (float)Math.Sin(Time.StepTime),
            //                                        0.0f,
            //                                        500.0f * (float)Math.Cos(Time.StepTime));
        }

        public override void Render()
        {
            virtualScene.Render();
        }

        public void RealRender()
        {
            scene.Render();
        }

        public IDevice GetDevice()
        {
            return Device;
        }
    }
}
