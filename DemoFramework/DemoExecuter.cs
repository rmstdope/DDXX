using System;
using System.Collections.Generic;
using System.Collections;
using System.Drawing;
using System.Text;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX.DirectInput;
using Dope.DDXX.Graphics;
using FMOD;
using Dope.DDXX.Input;
using Dope.DDXX.Sound;
using Dope.DDXX.Utility;

namespace Dope.DDXX.DemoFramework
{
    public class DemoExecuter
    {
        SoundDriver soundDriver;
        FMOD.Sound sound;
        FMOD.Channel channel;

        IDevice device;
        ITexture backBuffer;

        InputDriver inputDriver;

        List<Track> tracks = new List<Track>();
        int activeTrack = 0;

        public float StartTime
        {
            get
            {
                return 0.0f;
            }
        }

        public float EndTime
        {
            get
            {
                float t = 0.0f;
                foreach (Track track in tracks)
                {
                    foreach (IDemoEffect effect in track.Effects)
                    {
                        if (effect.EndTime > t)
                            t = effect.EndTime;
                    }
                }
                return t;
            }
        }

        public int NumTracks
        {
            get
            {
                return tracks.Count;
            }
        }

        public DemoExecuter()
        {
        }

        public void Initialize(string song)
        {
            InitializeGraphics();

            InitializeSound(song);

            InitializeInput();

            foreach (Track track in tracks)
            {
                foreach (IDemoEffect effect in track.Effects)
                {
                    effect.Initialize();
                }
            }
        }

        private void InitializeInput()
        {
            inputDriver = InputDriver.GetInstance();
        }

        private void InitializeGraphics()
        {
            device = D3DDriver.GetInstance().GetDevice();
            backBuffer = D3DDriver.Factory.CreateTexture(device, device.PresentationParameters.BackBufferWidth, device.PresentationParameters.BackBufferHeight, 1, Usage.RenderTarget, device.PresentationParameters.BackBufferFormat, Pool.Default);
        }

        private void InitializeSound(string song)
        {
            soundDriver = SoundDriver.GetInstance();
            soundDriver.Initialize();
            if (song != null && song != "")
                sound = soundDriver.CreateSound(song);
        }

        public void Register(int track, IDemoEffect effect)
        {
            while (NumTracks <= track)
            {
                tracks.Add(new Track());
            }
            tracks[track].Register(effect);
        }


        internal void Step()
        {
            Time.Step();

            foreach (Track track in tracks)
            {
                foreach (IDemoEffect effect in track.GetEffects(Time.StepTime))
                {
                    effect.Step();
                }
            }
        }

        public void Run()
        {
            Time.Initialize();

            if (sound != null)
            {
                channel = soundDriver.PlaySound(sound);
            }

            while (Time.StepTime <= EndTime + 2.0f && !inputDriver.KeyPressed(Key.Escape))
            {
                Step();

                Render();
            }
        }

        internal void Render()
        {
            RenderActiveTrack();

            ISurface source = backBuffer.GetSurfaceLevel(0);
            ISurface destination = device.GetRenderTarget(0);
            device.StretchRectangle(source, new Rectangle(0, 0, source.Description.Width, source.Description.Height),
                                    destination, new Rectangle(0, 0, destination.Description.Width, destination.Description.Height),
                                    TextureFilter.None);
            
            device.Present();
        }

        private void RenderActiveTrack()
        {
            ISurface originalTarget = device.GetRenderTarget(0);
            device.SetRenderTarget(0, backBuffer.GetSurfaceLevel(0));
            device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, System.Drawing.Color.Blue, 1.0f, 0);

            if (tracks.Count != 0)
            {
                IDemoEffect[] activeEffects = tracks[activeTrack].GetEffects(Time.StepTime);

                if (activeEffects.Length != 0)
                {
                    device.BeginScene();
                    foreach (IDemoEffect effect in activeEffects)
                    {
                        effect.Render();
                    }
                    device.EndScene();
                }
            }

            device.SetRenderTarget(0, originalTarget);
        }
    }
}
