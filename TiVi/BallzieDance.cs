using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.SceneGraph;
using Microsoft.DirectX;
using Dope.DDXX.Graphics;

namespace TiVi
{
    public class BallzieDance : BaseDemoEffect
    {
        private IScene scene;
        private CameraNode camera;
        private ChessBoard chessBoard;
        private MirrorNode mirror;

        public BallzieDance(string name, float startTime, float endTime)
            : base(name, startTime, endTime)
        {
        }

        protected override void Initialize()
        {
            IEffect effect = EffectFactory.CreateFromFile("TiVi.fxo");

            CreateStandardSceneAndCamera(out scene, out camera, 5);
            camera.WorldState.MoveUp(1);
            chessBoard = new ChessBoard(scene, MeshBuilder, effect, Device, 1, 0.1f);

            PointLightNode light = new PointLightNode("");
            light.Position = new Vector3(0, 1, 0);
            scene.AddNode(light);

            XLoader.Load("Ballzie-Dance.x", effect, TechniqueChooser.MeshPrefix("BallzieReflective"));
            XLoader.AddToScene(scene);
            mirror = new MirrorNode(scene.GetNodeByName("Ballzie"));
        }

        public override void Step()
        {
            scene.Step();
        }

        public override void Render()
        {
            chessBoard.Render(scene);
            mirror.Render(scene);
            scene.Render();
        }
    }
}
