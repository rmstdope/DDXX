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
using System.Reflection;
using System.IO;

namespace Dope.DDXX.DemoFramework
{
    public class DemoExecuter : IDemoEffectBuilder, IDemoRegistrator
    {
        private ISoundDriver soundDriver;
        private FMOD.Sound sound;
        private FMOD.Channel channel;

        private IDevice device;
        private ITexture backBuffer;
        private IPostProcessor postProcessor;
        private IDemoTweaker tweaker;

        private IInputDriver inputDriver;
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

        public List<Track> Tracks
        {
            get { return tracks; }
        }

        public IDemoEffect[] Effects(int track)
        {
            return tracks[track].Effects;
        }

        public IDemoPostEffect[] PostEffects(int track)
        {
            return tracks[track].PostEffects;
        }

        public IDemoTweaker Tweaker
        {
            set { tweaker = value; }
        }

        public DemoExecuter(ISoundDriver soundDriver, IInputDriver inputDriver, IPostProcessor postProcessor)
        {
            this.soundDriver = soundDriver;
            this.inputDriver = inputDriver;
            this.postProcessor = postProcessor;
            tweaker = new DemoTweakerMain(new IDemoTweaker[] { new DemoTweakerDemo(), new DemoTweakerTrack() , new DemoTweakerEffect() });
        }

        private DemoEffectTypes effectTypes = new DemoEffectTypes();

        public void Initialize(string song)
        {
            this.Initialize(song, new Assembly[] { Assembly.GetCallingAssembly() }, "");
        }

        public void Initialize(string song, string xmlFile)
        {
            this.Initialize(song, new Assembly[] { Assembly.GetCallingAssembly() }, xmlFile);
        }

        public void Initialize(string song, Assembly[] assemblies, string xmlFile)
        {
            effectTypes.Initialize(assemblies);

            InitializeFromFile(xmlFile);

            InitializeGraphics();

            InitializeSound(song);

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

            tweaker.Initialize(this);
        }

        private void InitializeGraphics()
        {
            device = D3DDriver.GetInstance().Device;
            backBuffer = D3DDriver.TextureFactory.CreateFullsizeRenderTarget();
        }

        private void InitializeSound(string song)
        {
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

            if (inputDriver.KeyPressedNoRepeat(Key.Space))
            {
                if (Time.IsPaused())
                {
                    Time.Resume();
                    soundDriver.SetPosition(channel, Time.CurrentTime);
                    soundDriver.ResumeChannel(channel);
                }
                else
                {
                    Time.Pause();
                    soundDriver.PauseChannel(channel);
                }
            }
            tweaker.HandleInput(inputDriver);
        }

        public void Run()
        {
            Time.Initialize();

            if (sound != null)
            {
                channel = soundDriver.PlaySound(sound);
                soundDriver.SetPosition(channel, Time.CurrentTime);
            }

            while (Time.StepTime <= EndTime + 2.0f && !tweaker.Quit)
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

            tweaker.Draw();

            device.SetRenderTarget(0, originalTarget);
        }


        private void InitializeFromFile(string xmlFile)
        {
            if (xmlFile != "")
            {
                DemoXMLReader xmlReader = new DemoXMLReader(this);
                xmlReader.Read(xmlFile);
            }
        }

        #region IDemoEffectBuilder Members
        private IRegisterable lastAddedEffect;

        public void AddEffect(string effectName, int effectTrack, float startTime, float endTime)
        {
            IDemoEffect effect = (IDemoEffect)effectTypes.CreateInstance(effectName, startTime, endTime);
            this.Register(effectTrack, effect);
            lastAddedEffect = effect;
        }

        public void AddPostEffect(string effectName, int effectTrack, float startTime, float endTime)
        {
            IDemoPostEffect effect = (IDemoPostEffect)effectTypes.CreateInstance(effectName, startTime, endTime);
            this.Register(effectTrack, effect);
            lastAddedEffect = effect;
        }

        public void AddTransition(string effectName, int destinationTrack)
        {
            // TODO Add transition support
        }

        public void AddFloatParameter(string name, float value)
        {
            AddParameter(name, value);
        }

        public void AddIntParameter(string name, int value)
        {
            AddParameter(name, value);
        }

        private void AddParameter(string name, object value)
        {
            if (lastAddedEffect != null)
            {
                effectTypes.SetProperty(lastAddedEffect, name, value);
            }
        }

        public void AddStringParameter(string name, string value)
        {
            AddParameter(name, value);
        }

        public void AddVector3Parameter(string name, Vector3 value)
        {
            AddParameter(name, value);
        }

        public void AddColorParameter(string name, Color value)
        {
            AddParameter(name, value);
        }

        public void AddSetupCall(string name, List<object> parameters)
        {
            if (lastAddedEffect != null)
            {
                effectTypes.CallSetup(lastAddedEffect, name, parameters);
            }
        }

        #endregion
    }
}
