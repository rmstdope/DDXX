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
        private SoundDriver soundDriver;
        private FMOD.Sound sound;
        private FMOD.Channel channel;

        private IDevice device;
        private ITexture backBuffer;
        private PostProcessor postProcessor;

        private InputDriver inputDriver;

        private List<Track> tracks = new List<Track>();
        private int activeTrack = 0;

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
                    foreach (IDemoPostEffect postEffect in track.PostEffects)
                    {
                        if (postEffect.EndTime > t)
                            t = postEffect.EndTime;
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
            postProcessor = new PostProcessor();
        }

        public void Initialize(string song)
        {
            InitializeGraphics();

            InitializeSound(song);

            InitializeInput();

            postProcessor.Initialize(device);

            foreach (Track track in tracks)
            {
                foreach (IDemoEffect effect in track.Effects)
                {
                    effect.Initialize();
                }
                foreach (IDemoPostEffect postEffect in track.PostEffects)
                {
                    postEffect.Initialize(postProcessor);
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
            backBuffer = D3DDriver.TextureFactory.CreateFullsizeRenderTarget();
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

        public void Register(int track, IDemoPostEffect postEffect)
        {
            while (NumTracks <= track)
            {
                tracks.Add(new Track());
            }
            tracks[track].Register(postEffect);
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

            //postProcessor.DebugWriteAllTextures();

            ISurface source = postProcessor.OutputTexture.GetSurfaceLevel(0);
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
            device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, System.Drawing.Color.Black, 1.0f, 0);
            postProcessor.StartFrame(backBuffer);

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

                IDemoPostEffect[] activePostEffects = tracks[activeTrack].GetPostEffects(Time.StepTime);
                foreach (IDemoPostEffect postEffect in activePostEffects)
                {
                    postEffect.Render();
                }
            }

            device.SetRenderTarget(0, originalTarget);
        }
    }
}
