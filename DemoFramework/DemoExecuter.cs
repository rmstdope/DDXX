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
    public class DemoExecuter : IDemoEffectBuilder, IDemoRegistrator, IDemoTweakerContext
    {
        private ISoundDriver soundDriver;
        private FMOD.Sound sound;
        private FMOD.Channel channel;

        private IDevice device;
        private IGraphicsFactory graphicsFactory;
        private ITextureFactory textureFactory;
        private ITexture backBuffer;
        private IPostProcessor postProcessor;
        private IDemoTweaker tweaker;

        private IInputDriver inputDriver;
        private List<ITrack> tracks = new List<ITrack>();
        private int activeTrack;

        private IDemoFactory demoFactory;
        private DemoEffectTypes effectTypes = new DemoEffectTypes();
        private TweakerSettings settings = new TweakerSettings();
        private DemoXMLReader xmlReader;

        public float StartTime
        {
            get { return 0.0f; }
        }

        public float EndTime
        {
            get
            {
                float maxTime = 0.0f;
                foreach (ITrack track in tracks)
                {
                    if (track.EndTime > maxTime)
                        maxTime = track.EndTime;
                }
                return maxTime;
            }
        }

        public int NumTracks
        {
            get { return tracks.Count; }
        }

        public List<ITrack> Tracks
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

        public DemoExecuter(IDemoFactory demoFactory, ISoundDriver soundDriver, IInputDriver inputDriver, IPostProcessor postProcessor)
        {
            activeTrack = 0;
            this.demoFactory = demoFactory;
            this.soundDriver = soundDriver;
            this.inputDriver = inputDriver;
            this.postProcessor = postProcessor;
            tweaker = new DemoTweakerMain(this, new IDemoTweaker[] { new DemoTweakerDemo(settings), new DemoTweakerTrack(settings), new DemoTweakerEffect(settings) }, settings);
        }

        public void Initialize(IDevice device, IGraphicsFactory graphicsFactory, ITextureFactory textureFactory, string song)
        {
            this.Initialize(device, graphicsFactory, textureFactory, song, 
                new Assembly[] { Assembly.GetCallingAssembly() }, "");
        }

        public void Initialize(IDevice device, IGraphicsFactory graphicsFactory, ITextureFactory textureFactory, string song, string xmlFile)
        {
            this.Initialize(device, graphicsFactory, textureFactory, song, 
                new Assembly[] { Assembly.GetCallingAssembly() }, xmlFile);
        }

        public void Initialize(IDevice device, IGraphicsFactory graphicsFactory, 
            ITextureFactory textureFactory, string song, Assembly[] assemblies, 
            string xmlFile)
        {
            this.device = device;
            this.graphicsFactory = graphicsFactory;
            this.textureFactory = textureFactory;

            effectTypes.Initialize(assemblies);

            InitializeFromFile(xmlFile);

            InitializeGraphics();

            InitializeSound(song);

            postProcessor.Initialize(device);

            foreach (ITrack track in tracks)
            {
                track.Initialize(graphicsFactory, device, postProcessor);
            }

            tweaker.Initialize(this);
        }

        private void InitializeGraphics()
        {
            backBuffer = textureFactory.CreateFullsizeRenderTarget();
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
                tracks.Add(demoFactory.CreateTrack());
            }
            tracks[track].Register(effect);
        }

        public void Register(int track, IDemoPostEffect postEffect)
        {
            while (NumTracks <= track)
            {
                tracks.Add(demoFactory.CreateTrack());
            }
            tracks[track].Register(postEffect);
        }

        internal void Step()
        {
            Time.Step();

            foreach (ITrack track in tracks)
            {
                track.Step();
            }

            tweaker.HandleInput(inputDriver);
        }

        public void Run()
        {
            Time.Initialize();

            if (sound != null)
            {
                channel = soundDriver.PlaySound(sound);
                SynchronizeSong();
            }

            while (Time.StepTime <= EndTime + 2.0f && !tweaker.Quit)
            {
                Step();

                Render();
            }

            if (xmlReader != null)
            {
                Update(xmlReader);
                xmlReader.Write("new.xml");
            }
        }

        private void SynchronizeSong()
        {
            if (sound != null)
                soundDriver.SetPosition(channel, Time.CurrentTime);
        }

        internal void Render()
        {
            RenderActiveTrack();

            //postProcessor.DebugWriteAllTextures();

            using (ISurface source = postProcessor.OutputTexture.GetSurfaceLevel(0))
            {
                using (ISurface destination = device.GetRenderTarget(0))
                {
                    device.StretchRectangle(source, new Rectangle(0, 0, source.Description.Width, source.Description.Height),
                                            destination, new Rectangle(0, 0, destination.Description.Width, destination.Description.Height),
                                            TextureFilter.None);
                }
            }

            device.Present();
        }

        private void RenderActiveTrack()
        {
            using (ISurface originalTarget = device.GetRenderTarget(0))
            {
                using (ISurface currentRenderTarget = backBuffer.GetSurfaceLevel(0))
                    device.SetRenderTarget(0, currentRenderTarget);

                device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, System.Drawing.Color.Black, 1.0f, 0);
                postProcessor.StartFrame(backBuffer);

                if (tracks.Count != 0)
                    tracks[activeTrack].Render(device);

                tweaker.Draw();

                device.SetRenderTarget(0, originalTarget);
            }
        }


        private void InitializeFromFile(string xmlFile)
        {
            if (xmlFile != "")
            {
                xmlReader = new DemoXMLReader(this);
                xmlReader.Read(xmlFile);
            }
        }

        public void Update(IEffectChangeListener effectChangeListener)
        {
            foreach (ITrack track in tracks)
            {
                track.UpdateListener(effectChangeListener);
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

        public void AddFloatParameter(string name, float value, float stepSize)
        {
            AddParameter(name, value, stepSize);
        }

        public void AddIntParameter(string name, int value, float stepSize)
        {
            AddParameter(name, value, stepSize);
        }

        private void AddParameter(string name, object value, float stepSize)
        {
            if (lastAddedEffect != null)
            {
                effectTypes.SetProperty(lastAddedEffect, name, value);
                ITweakableContainer container = lastAddedEffect as ITweakableContainer;
                container.SetStepSize(container.GetTweakableNumber(name), stepSize);
            }
        }

        public void AddStringParameter(string name, string value)
        {
            AddParameter(name, value, 0);
        }

        public void AddVector3Parameter(string name, Vector3 value, float stepSize)
        {
            AddParameter(name, value, stepSize);
        }

        public void AddColorParameter(string name, Color value)
        {
            AddParameter(name, value, 0);
        }

        public void AddBoolParameter(string name, bool value)
        {
            AddParameter(name, value, 0);
        }

        public void AddSetupCall(string name, List<object> parameters)
        {
            if (lastAddedEffect != null)
            {
                effectTypes.CallSetup(lastAddedEffect, name, parameters);
            }
        }

        #endregion

        #region IDemoTweakerContext Members

        public void TogglePause()
        {
            if (Time.IsPaused())
            {
                Time.Resume();
                if (sound != null)
                {
                    soundDriver.SetPosition(channel, Time.CurrentTime);
                    soundDriver.ResumeChannel(channel);
                }
            }
            else
            {
                Time.Pause();
                if (sound != null)
                {
                    soundDriver.PauseChannel(channel);
                }
            }
        }

        public void JumpInTime(float time)
        {
            Time.CurrentTime += time;
            if (Time.CurrentTime < StartTime)
                Time.CurrentTime = StartTime;
            if (Time.CurrentTime > EndTime)
                Time.CurrentTime = EndTime;
            SynchronizeSong();
        }

        #endregion

    }
}
