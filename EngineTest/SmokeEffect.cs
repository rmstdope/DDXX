using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.TextureBuilder;
using Microsoft.Xna.Framework.Graphics;
using Dope.DDXX.Graphics;
using Dope.DDXX.SceneGraph;
using Microsoft.Xna.Framework;
using Dope.DDXX.Utility;
using Dope.DDXX.ModelBuilder;
using System.Diagnostics;
using NUnit.Framework;
using Microsoft.Xna.Framework.Input;

namespace EngineTest
{
    public class Grid
    {
        float[] values;
        int N;
        public Grid(int n)
        {
            N = n;
            int size = (N + 2) * (N + 2);
            values = new float[size];
            for (int i = 0; i < size; i++)
                values[i] = 0.0f;
        }

        public float this[int x, int y]
        {
            set { values[(N + 2) * y + x] = value; }
            get { return values[(N+2)*y + x]; }
        }

        public int Size 
        {
            get { return N; }
        }

        public void AddMul(float dt, Grid other)
        {
            Debug.Assert(other.Size == Size);
            for(int i=0; i<=Size+1; i++) {
                for(int j=0; j<=Size+1; j++) {
                    this[i,j] += dt*other[i,j];
                }
            }
        }

        internal void Set(float v)
        {
            for (int i = 0; i <= Size + 1; i++)
            {
                for (int j = 0; j <= Size + 1; j++)
                {
                    this[i, j] = v;
                }
            }
        }
    }

    public class SmokeSimulator
    {
        Grid u;
        Grid v;
        Grid u_prev;
        Grid v_prev;
        Grid dens;
        Grid dens_prev;
        int N;
        float visc;
        float diff;
        float sourceDens = 100.0f;
        float force = 5.0f;
        int oldX;
        int oldY;

        public SmokeSimulator(int n, float viscosity, float diffusion)
        {
            N = n;
            visc = viscosity;
            diff = diffusion;
            u = new Grid(N);
            v = new Grid(N);
            u_prev = new Grid(N);
            v_prev = new Grid(N);
            dens = new Grid(N);
            dens_prev = new Grid(N);
        }

        public void AddSource(Grid x, Grid s, float dt)
        {
            x.AddMul(dt, s);
        }

        public void Diffuse(int b, Grid x, Grid x0, float diff, float dt)
        {
            float a = dt * diff * N * N;

            for (int k = 0; k < 20; k++)
            {
                for (int i = 1; i <= N; i++)
                {
                    for (int j = 1; j <= N; j++)
                    {
                        x[i, j] = (x0[i, j] + a * (x[i - 1, j] + 
                                                   x[i + 1, j] +
                                                   x[i, j - 1] + 
                                                   x[i, j + 1])) / (1 + 4 * a);
                    }
                }
                SetBoundary(b, x);
            }
        }

        public void Advect(int b, Grid d, Grid d0,
                           Grid u, Grid v, float dt)
        {
            float dt0 = dt * N;
            for (int i = 1; i <= N; i++)
            {
                for (int j = 1; j <= N; j++)
                {
                    float x = i - dt0 * u[i, j];
                    float y = j - dt0 * v[i, j];
                    Limit(ref x);
                    int i0 = (int)x;
                    int i1 = i0 + 1;
                    Limit(ref y);
                    int j0 = (int)y;
                    int j1 = j0 + 1;
                    float s1 = x - i0;
                    float s0 = 1 - s1;
                    float t1 = y - j0;
                    float t0 = 1 - t1;
                    d[i, j] = s0 * (t0 * d0[i0, j0] + t1 * d0[i0, j1]) +
                              s1 * (t0 * d0[i1, j0] + t1 * d0[i1, j1]);
                }
            }
            SetBoundary(b, d);
        }

        public void DensityStep(Grid x, Grid x0,
                                Grid u, Grid v,
                                float diff, float dt)
        {
            AddSource(x, x0, dt);
            Swap(ref x0, ref x);
            Diffuse(0, x, x0, diff, dt);
            Swap(ref x0, ref x);
            Advect(0, x, x0, u, v, dt);
        }

        public void VelocityStep(Grid u, Grid v,
                                 Grid u0, Grid v0,
                                 float visc, float dt)
        {
            AddSource(u, u0, dt);
            AddSource(v, v0, dt);
            Swap(ref u0, ref u);
            Diffuse(1, u, u0, visc, dt);
            Swap(ref v0, ref v);
            Diffuse(2, v, v0, visc, dt);
            Project(u, v, u0, v0);
            Swap(ref u0, ref u);
            Swap(ref v0, ref v);
            Advect(1, u, u0, u0, v0, dt);
            Advect(2, v, v0, u0, v0, dt);
            Project(u, v, u0, v0);
        }

        private void Project(Grid u, Grid v, Grid p, Grid div)
        {
            float h = 1.0f / N;
            for (int i = 1; i <= N; i++)
            {
                for (int j = 1; j <= N; j++)
                {
                    div[i, j] = -0.5f * h * (u[i + 1, j] - u[i - 1, j] +
                                             v[i, j + 1] - v[i, j - 1]);
                    p[i, j] = 0;
                }
            }
            SetBoundary(0, div);
            SetBoundary(0, p);

            for (int k = 0; k < 20; k++)
            {
                for (int i = 1; i <= N; i++)
                {
                    for (int j = 1; j <= N; j++)
                    {
                        p[i, j] = (div[i, j] + p[i - 1, j] + p[i + 1, j] +
                                               p[i, j - 1] + p[i, j + 1]) / 4;
                    }
                }
                SetBoundary(0, p);
            }
            for (int i = 1; i <= N; i++)
            {
                for (int j = 1; j <= N; j++)
                {
                    u[i, j] -= 0.5f * (p[i + 1, j] - p[i - 1, j]) / h;
                    v[i, j] -= 0.5f * (p[i, j + 1] - p[i, j - 1]) / h;
                }
            }
            SetBoundary(1, u);
            SetBoundary(2, v);
        }

        internal static void Swap(ref Grid x0, ref Grid x)
        {
            Grid tmp = x0;
            x0 = x;
            x = tmp;
        }

        private void Limit(ref float x)
        {
            if (x < 0.5f)
                x = 0.5f;
            if (x > N + 0.5f)
                x = N + 0.5f;
        }

        private void SetBoundary(int b, Grid x)
        {
            for (int i = 1; i <= N; i++)
            {
                x[0, i] = b == 1 ? -x[1, i] : x[1, i];
                x[N + 1, i] = b == 1 ? -x[N, i] : x[N, i];
                x[i, 0] = b == 2 ? -x[i, 1] : x[i, 1];
                x[i, N + 1] = b == 2 ? -x[i, N] : x[i, N];

                if (b == 0)
                {
                    x[0, i] = 0;
                    x[N + 1, i] = 0;
                    x[i, 0] = 0;
                    x[i, N + 1] = 0;
                }
            }
            x[0, 0] = 0.5f * (x[1, 0] + x[0, 1]);
            x[0, N + 1] = 0.5f * (x[1, N + 1] + x[0, N]);
            x[N + 1, 0] = 0.5f * (x[N, 0] + x[N + 1, 1]);
            x[N + 1, N + 1] = 0.5f * (x[N, N + 1] + x[N + 1, N]);
        }

        public void Step(float dt)
        {
            GetFromUI();
            u_prev[1, N / 2 - 1] = force;
            u_prev[1, N / 2] = force;
            u_prev[1, N / 2 + 1] = force;
            dens_prev[N / 5, N / 2] = sourceDens;
            VelocityStep(u, v, u_prev, v_prev, visc, dt);
            DensityStep(dens, dens_prev, u, v, diff, dt);
        }

        private void GetFromUI()
        {
            dens_prev.Set(0.0f);
            u_prev.Set(0.0f);
            v_prev.Set(0.0f);
            // get dens_prev, u_prev, v_prev
            MouseState mouseState = Mouse.GetState();
            int x = 1 + mouseState.X % N;
            int y = 1 + mouseState.Y % N;
            if (x < 1)
                x = 1;
            if (y < 1)
                y = 1;
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                u_prev[x, y] = force * (x - oldX);
                v_prev[x, y] = force * (oldY - y);
            }
            if (mouseState.RightButton == ButtonState.Pressed)
            {
                dens_prev[x, y] = sourceDens;
            }
            oldX = x;
            oldY = y;
        }

        public float Viscosity
        {
            get { return visc; }
            set { visc = value; }
        }

        public float Diffusion
        {
            get { return diff; }
            set { diff = value; }
        }

        public float SourceDensity
        {
            get { return sourceDens; }
            set { sourceDens = value; }
        }

        public float Force
        {
            get { return force; }
            set { force = value; }
        }

        public Grid Density
        {
            get { return dens_prev; }
        }
        public Grid VelocityU
        {
            get { return u_prev; }
        }
        public Grid VelocityV
        {
            get { return v_prev; }
        }
    }

    public class SmokeEffect : BaseDemoEffect
    {
        private ISpriteBatch sprite;
        private ITexture2D texture;
        private uint[] colorData;
        private SmokeSimulator simulator;
        int dotX;
        int dotY;
        const int N = 50;

        public SmokeEffect(string name, float startTime, float endTime)
            : base(name, startTime, endTime)
        {
            simulator = new SmokeSimulator(N, 0.0f, 0.0f);
        }

        public float Viscosity
        {
            get { return simulator.Viscosity; }
            set { simulator.Viscosity = value; }
        }

        public float Diffusion
        {
            get { return simulator.Diffusion; }
            set { simulator.Diffusion = value; }
        }

        public float SourceDensity
        {
            get { return simulator.SourceDensity; }
            set { simulator.SourceDensity = value; }
        }

        public float Force
        {
            get { return simulator.Force; }
            set { simulator.Force = value; }
        }

        protected override void Initialize()
        {
            sprite = GraphicsFactory.CreateSpriteBatch();
            texture = TextureFactory.CreateFromFunction(N, N, 1, TextureUsage.None, SurfaceFormat.Color,
                delegate(Vector2 pos, Vector2 size) { return new Vector4(0.0f, 0.0f, 0.0f, 0.0f); });
            colorData = new uint[texture.Width * texture.Height];
            DrawTexture();            
        }

        protected void DrawTexture()
        {
            for (int y = 0; y < texture.Height; y++)
            {
                for (int x = 0; x < texture.Width; x++)
                {
                    float densityr = simulator.VelocityU[x + 1, y + 1];
                    float densityg = simulator.VelocityV[x + 1, y + 1];
                    float densityb = simulator.Density[x + 1, y + 1];
                    Clamp(ref densityr);
                    Clamp(ref densityg);
                    Clamp(ref densityb);
                    uint r = (uint)(densityr * 255);
                    uint g = (uint)(densityg * 255);
                    uint b = (uint)(densityb * 255);
                    colorData[y * texture.Height + x] = 0xFF000000 | 
                                                        (uint)((r << 16) & 0xFF0000) |
                                                        (uint)((g << 8) & 0xFF00) |
                                                        (uint)(b & 0xFF);
                }
            }
            //colorData[dotY * texture.Height + dotX] = 0xFFFFFFFF;
            texture.SetData<uint>(colorData);
        }

        private void Clamp(ref float d)
        {
            if (d < 0)
                d = 0.0f;
            if (d > 1.0f)
                d = 1.0f;
        }

        public override void Step()
        {
            //simulator.Density[dotX, dotY] = 1.0f;
            //simulator.VelocityU[dotX, dotY] = 0.02f*dotX;
            //simulator.VelocityV[dotX, dotY] = -0.03f*dotY;
            simulator.Step(0.20f);
            DrawTexture();
            dotX++;
            dotY++;
            if (dotX >= N)
                dotX = 0;
            if (dotY >= N)
                dotY = 0;
        }

        public override void Render()
        {
            sprite.Begin();
            GraphicsDevice.RenderState.DepthBufferEnable = false;
            {
                int sWidth = GraphicsDevice.PresentationParameters.BackBufferWidth;
                int sHeight = GraphicsDevice.PresentationParameters.BackBufferHeight;
                int tWidth = texture.Width;
                int tHeight = texture.Height;
                sprite.Draw(texture, new Rectangle(0, 0, sWidth, sHeight), Color.White);
            }
            sprite.End();
        }
    }
}
