using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.SceneGraph;
using Dope.DDXX.MeshBuilder;
using Dope.DDXX.Graphics;
using System.Drawing;
using Microsoft.DirectX.Direct3D;
using Dope.DDXX.Utility;

namespace TiVi {
    class DiscoFever : BaseDemoEffect {
        private IScene scene;
        private CameraNode camera;
        private INode room;
        private ModelNode[, ,] squares;
        private int redOffset;
        private int greenOffset;
        private int blueOffset;

        public DiscoFever(string name, float startTime, float endTime)
            : base(name, startTime, endTime) {
        }

        const int Nx = 8;
        const int Ny = 8;
        const int Nz = 8;
        const float sx = 1.2f;
        const float sy = 1.2f;
        const float sz = 1.2f;

        protected override void Initialize() {
            CreateStandardSceneAndCamera(out scene, out camera, 8.5f);
            MeshDirector director = new MeshDirector(MeshBuilder);
            director.CreatePlane(1, 1, 1, 1, true);
            MeshBuilder.SetDiffuseTexture("Default1", "SquareNoAlpha.dds");
            IModel model = director.Generate("Default1");
            const float pihalf = (float)(Math.PI / 2.0);
            room = new DummyNode("Room node");
            squares = new ModelNode[Nx, Ny, Nz];
            for (int j = 0; j < Ny; j++) {
                for (int i = 0; i < Nx; i++) {
                    AddDiscoPlane(model, i, j, Nz - 1, 0, 0, 0, 0.5f, 0.5f, 1.0f);
                    AddDiscoPlane(model, i, j, 0, 0, 0, 2 * pihalf, 0.5f, 0.5f, 0.0f);
                }
            }
            for (int j = 0; j < Ny; j++) {
                for (int k = 0; k < Nz; k++) {
                    AddDiscoPlane(model, Nx - 1, j, k, 0, 0, pihalf, 1.0f, 0.5f, 0.5f);
                    AddDiscoPlane(model, 0, j, k, 0, 0, -pihalf, 0.0f, 0.5f, 0.5f);
                }
            }
            for (int i = 0; i < Nx; i++) {
                for (int k = 0; k < Nz; k++) {
                    AddDiscoPlane(model, i, Ny - 1, k, 0, -pihalf, 0, 0.5f, 1.0f, 0.5f);
                    AddDiscoPlane(model, i, 0, k, 0, pihalf, 0, 0.5f, 0.0f, 0.5f);
                }
            }
            scene.AddNode(room);
            //Mixer.ClearColor = Color.Blue;
        }

        private void AddDiscoPlane(IModel model, int i, int j, int k,
            float roll, float tilt, float turn,
            float dx, float dy, float dz) {
            ModelNode modelNode;
            modelNode = CreateSimpleModelNode(model.Clone(), "TiVi.fxo", "Simple");
            modelNode.WorldState.MoveRight(-((Nx * sx) / 2.0f) + i * sx + dx);
            modelNode.WorldState.MoveUp(-((Ny * sy) / 2.0f) + j * sy + dy);
            modelNode.WorldState.MoveForward(-((Nz * sz) / 2.0f) + k * sz + dz);
            modelNode.WorldState.Roll(roll);
            modelNode.WorldState.Tilt(tilt);
            modelNode.WorldState.Turn(turn);
            room.AddChild(modelNode);
            squares[i, j, k] = modelNode;
            SetModelColor(i, j, k);
        }

        private void SetModelColor(int i, int j, int k) {
            int r = Ramp((int)(redOffset + 2*i * (256 / Nx) + j * (256 / Ny) + k * (256 / Nz)), 256);
            int g = Ramp((int)(greenOffset + i * (256 / Nx) + 2*j * (256 / Ny) + k * (256 / Nz)), 256);
            int b = Ramp((int)(blueOffset + i * (256 / Nx) + j * (256 / Ny) + 2*k * (256 / Nz)), 256);
            Color color = Color.FromArgb(r, g, b);
            IModel model = squares[i, j, k].Model;
            model.Materials[0].AmbientColor = ColorValue.FromColor(color);
        }

        private static int Ramp(int i, int N) {
            i = i % N;
            if (i >= N / 2) {
                i -= N / 2;
                return N - 1 - (2 * i);
            } else
                return 2 * i;
        }

        public override void Step() {
            //Mixer.ClearColor = Color.Blue;
            scene.Step();
            room.WorldState.Reset();
            room.WorldState.Turn(0.5f * Time.StepTime);
            room.WorldState.Roll(0.2f * Time.StepTime);
            room.WorldState.Tilt(0.3f * Time.StepTime);
            redOffset = (int)(Time.StepTime * 180) % 256;
            greenOffset = (int)(Time.StepTime * 210) % 256;
            blueOffset = (int)(Time.StepTime * 150) % 256;
            for (int j = 0; j < Ny; j++)
            {
                for (int i = 0; i < Nx; i++) {
                    SetModelColor(i, j, Nz - 1);
                    SetModelColor(i, j, 0);
                }
            }
            for (int j = 0; j < Ny; j++) {
                for (int k = 0; k < Nz; k++) {
                    SetModelColor(Nx - 1, j, k);
                    SetModelColor(0, j, k);
                }
            }
            for (int i = 0; i < Nx; i++) {
                for (int k = 0; k < Nz; k++) {
                    SetModelColor(i, Ny - 1, k);
                    SetModelColor(i, 0, k);
                }
            }
        }

        public override void Render() {
            scene.Render();
        }
    }
}
