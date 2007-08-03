using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.MeshBuilder;
using Dope.DDXX.Graphics;
using Dope.DDXX.SceneGraph;
using Dope.DDXX.Utility;
using Microsoft.DirectX;
using System.Drawing;
using Microsoft.DirectX.Direct3D;

namespace TiVi
{
    public class EndScene : BaseDemoEffect
    {
        private class Diamond
        {
            public float StartTime;
            public ModelNode Node;
            public float Speed;
        }

        private IScene scene;
        private CameraNode camera;
        private INode film;
        private ITexture filmTexture;
        private List<ITexture> textures = new List<ITexture>();
        private PointLightNode light;
        private float cameraDist;
        private float rollFactor;
        private float turnFactor;
        private float tiltFactor;
        private List<Diamond> diamonds = new List<Diamond>();

        public float CameraDist
        {
            get { return cameraDist; }
            set { cameraDist = value; }
        }

        public float RollFactor
        {
            get { return rollFactor; }
            set { rollFactor = value; }
        }

        public float TurnFactor
        {
            get { return turnFactor; }
            set { turnFactor = value; }
        }

        public float TiltFactor
        {
            get { return tiltFactor; }
            set { tiltFactor = value; }
        }

        public EndScene(string name, float startTime, float endTime)
            : base(name, startTime, endTime)
        {
            SetStepSize(GetTweakableNumber("RollFactor"), 0.01f);
            SetStepSize(GetTweakableNumber("TurnFactor"), 0.01f);
            SetStepSize(GetTweakableNumber("TiltFactor"), 0.01f);
            SetStepSize(GetTweakableNumber("CameraDist"), 0.05f);
            rollFactor = 0.3f;
            turnFactor = 0.2f;
            tiltFactor = 0.2f;
        }

        protected override void Initialize()
        {
            for (int i = 1; i < 31; i++)
            {
                textures.Add(TextureFactory.CreateFromFile("Screenshot" + i + ".jpg"));
            }

            CreateStandardSceneAndCamera(out scene, out camera, 2.5f);
            MeshBuilder.SetDiffuseTexture("Default1", "marble.jpg");
            MeshDirector director = new MeshDirector(MeshBuilder);
            director.CreatePlane(1, 9.0f / 16.0f, 1, 1, true);
            film = new DummyNode("Film");
            for (int i = -49; i < 50; i++)
            {
                IModel model = director.Generate("Default1");
                model.Materials[0].Diffuse = Color.White;
                model.Materials[0].DiffuseTexture = textures[Rand.Int(0, textures.Count)];
                ModelNode node = CreateSimpleModelNode(model, "TiVi.fxo", "EndFilm");
                node.WorldState.MoveUp(i);
                node.WorldState.Roll(-(float)Math.PI / 2);
                film.AddChild(node);
            }

            scene.AddNode(film);

            light = new PointLightNode("");
            light.Position = new Vector3(0, 0, -2);
            scene.AddNode(light);

            filmTexture = TextureFactory.CreateFromFile("filmroll.dds");
            IEffect effect = EffectFactory.CreateFromFile("TiVi.fxo");
            effect.SetValue(EffectHandle.FromString("FilmRollTexture"), filmTexture);

            Step();
            TiViMeshDirector tiviDirector = new TiViMeshDirector(MeshBuilder, new MeshDirector(MeshBuilder), EffectFactory, Device);
            ModelNode firstDiamond = tiviDirector.CreateDiamondNode(0.15f);
            for (int i = 0; i < 80; i++)
            {
                Diamond diamond = new Diamond();
                diamond.Node = new ModelNode("", firstDiamond.Model, firstDiamond.EffectHandler, Device);
                diamond.StartTime = 100;
                scene.AddNode(diamond.Node);
                diamonds.Add(diamond);
                diamond.Node.Position +=
                    film.WorldState.Forward * Rand.Float(0.1f, 0.3f) +
                    film.WorldState.Right * Rand.Float(-1, 1) +
                    film.WorldState.Up * Rand.Float(-3, 3);
                diamond.Node.WorldState.Turn(Rand.Float(0, 2 * (float)Math.PI));
                if (Rand.Int(0, 15) == 5)
                    diamond.Node.Position -= film.WorldState.Forward * 0.5f;
                diamond.Speed = Rand.Float(0.25f, 0.45f);
            }
        }

        public override void Step()
        {
            film.WorldState.Reset();
            film.WorldState.Roll((float)Math.PI * rollFactor);
            film.WorldState.Tilt((float)Math.PI * tiltFactor);
            film.WorldState.Turn((float)Math.PI * -turnFactor);
            film.WorldState.MoveUp(40);
            film.WorldState.MoveUp(-(Time.StepTime - StartTime) * 0.2f);
            film.WorldState.MoveRight(0.2f - (Time.StepTime - StartTime) * 0.01f);
            scene.Step();
            camera.WorldState.ResetPosition();
            camera.WorldState.MoveForward(-cameraDist);

            foreach (Diamond diamond in diamonds)
            {
                diamond.Node.WorldState.Turn(Time.DeltaTime * 0.4f);
                diamond.Node.WorldState.Position -= film.WorldState.Up * Time.DeltaTime * diamond.Speed;
                Vector3 pos = (diamond.Node.WorldState.Position - 3 * film.WorldState.Up);
                if (pos.Length() > 4)
                {
                    diamond.Node.WorldState.Position += film.WorldState.Up * 7;
                    diamond.StartTime = Time.StepTime;
                }
                //float d = Time.StepTime - diamond.StartTime;
                //d = (float)Math.Min(1, d);
                //diamond.Node.Model.Materials[0].AmbientColor = new ColorValue(0.2f, 0.2f, 0.2f, 1.0f * d);
                //diamond.Node.Model.Materials[0].DiffuseColor = new ColorValue(0.5f, 0.5f, 1.0f, 1.0f * d);
                //diamond.Node.Model.Materials[0].SpecularColor = new ColorValue(1.0f, 1.0f, 1.0f, 1.0f * d);
            }

            //light.Position = new Vector3(
            //    (float)Math.Sin(Time.StepTime),
            //    0,
            //    (float)Math.Cos(Time.StepTime)) * 5;
        }

        public override void Render()
        {
            scene.Render();
        }

    }
}
