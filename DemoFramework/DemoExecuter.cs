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
using Dope.DDXX.TextureBuilder;

namespace Dope.DDXX.DemoFramework
{
    public class DemoExecuter : IDemoEffectBuilder, IDemoRegistrator, IDemoTweakerContext, IDemoMixer
    {
        private ISoundDriver soundDriver;
        private FMOD.Sound sound;
        private FMOD.Channel channel;

        private IDevice device;
        private IGraphicsFactory graphicsFactory;
        private ITextureFactory textureFactory;
        private ITextureBuilder textureBuilder;
        private ITexture backBuffer;
        private IPostProcessor postProcessor;
        private IDemoTweaker tweaker;

        private IInputDriver inputDriver;
        private List<ITrack> tracks = new List<ITrack>();
        private int activeTrack;

        private IDemoFactory demoFactory;
        private IDemoEffectTypes effectTypes;
        private TweakerSettings settings = new TweakerSettings();
        private DemoXMLReader xmlReader;
        private Color clearColor = Color.FromArgb(0, 10, 10, 10);//.DarkGray;//.Black;//DarkSlateBlue;
        private Dictionary<string, IGenerator> generators = new Dictionary<string,IGenerator>();

        private string songFilename;

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

        public Color BackgroundColor
        {
            set { clearColor = value; }
            get { return clearColor; }
        }

        public DemoExecuter(IDemoFactory demoFactory, ISoundDriver soundDriver, IInputDriver inputDriver, IPostProcessor postProcessor, IDemoEffectTypes effectTypes)
        {
            activeTrack = 0;
            this.demoFactory = demoFactory;
            this.soundDriver = soundDriver;
            this.inputDriver = inputDriver;
            this.postProcessor = postProcessor;
            this.effectTypes = effectTypes;
            tweaker = new DemoTweakerMain(this, new IDemoTweaker[] { new DemoTweakerDemo(settings), new DemoTweakerTrack(settings), new DemoTweakerEffect(settings) }, settings);
        }

        public void Initialize(IDevice device, IGraphicsFactory graphicsFactory,
            ITextureFactory textureFactory, ITextureBuilder textureBuilder)
        {
            this.Initialize(device, graphicsFactory, textureFactory, textureBuilder, "");
        }

        public void Initialize(IDevice device, IGraphicsFactory graphicsFactory, 
            ITextureFactory textureFactory, ITextureBuilder textureBuilder, string xmlFile)
        {
            this.device = device;
            this.graphicsFactory = graphicsFactory;
            this.textureFactory = textureFactory;
            this.textureBuilder = textureBuilder;

            //effectTypes.Initialize(assemblies);

            InitializeFromFile(xmlFile);

            InitializeGraphics();

            InitializeSound();

            postProcessor.Initialize(device);

            foreach (ITrack track in tracks)
            {
                track.Initialize(graphicsFactory, device, textureFactory, textureBuilder, this, postProcessor);
            }

            tweaker.Initialize(this);
        }

        private void InitializeGraphics()
        {
            backBuffer = textureFactory.CreateFullsizeRenderTarget(Format.A8R8G8B8);
        }

        private void InitializeSound()
        {
            soundDriver.Initialize();
            if (songFilename != null && songFilename != "")
                sound = soundDriver.CreateSound(songFilename);
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

        public void Step()
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
                if (tweaker.ShouldSave(inputDriver))
                {
                    Update(xmlReader);
                    xmlReader.Write();
                }
            }
        }

        private void SynchronizeSong()
        {
            if (sound != null)
            {
                soundDriver.SetPosition(channel, Time.CurrentTime);
                //Time.CurrentTime = soundDriver.GetPosition(channel);
                //System.Diagnostics.Debug.Write("Syncronizing sound: SoundTime=" + soundDriver.GetPosition(channel));
                //System.Diagnostics.Debug.WriteLine(", ActualTime: " + Time.CurrentTime);
            }
        }

        public void Render()
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

                device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, clearColor, 1.0f, 0);
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
        private object lastAddedAsset;

        public void AddEffect(string effectName, int effectTrack, float startTime, float endTime)
        {
            IDemoEffect effect = (IDemoEffect)effectTypes.CreateInstance(effectName, startTime, endTime);
            this.Register(effectTrack, effect);
            lastAddedAsset = effect;
        }

        public void AddPostEffect(string effectName, int effectTrack, float startTime, float endTime)
        {
            IDemoPostEffect effect = (IDemoPostEffect)effectTypes.CreateInstance(effectName, startTime, endTime);
            this.Register(effectTrack, effect);
            lastAddedAsset = effect;
        }

        public void AddTransition(string effectName, int destinationTrack)
        {
            // TODO Add transition support
            throw new Exception("The method or operation is not implemented.");
        }

        public void AddGenerator(string generatorName, string className)
        {
            IGenerator generator = effectTypes.CreateGenerator(className);
            generators.Add(generatorName, generator);
            lastAddedAsset = generator;
        }

        public void AddTexture(string textureName, string generatorName, int width, int height, int mipLevels)
        {
            if (!generators.ContainsKey(generatorName))
                throw new DDXXException("Generator " + generatorName + " has never been registered.");
            ITexture texture = textureBuilder.Generate(generators[generatorName], width, height, mipLevels, Format.A8R8G8B8);
            textureFactory.RegisterTexture(textureName, texture);
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
            effectTypes.SetProperty(lastAddedAsset, name, value);
            ITweakableContainer container = lastAddedAsset as ITweakableContainer;
            if (stepSize > 0)
                container.SetStepSize(container.GetTweakableNumber(name), stepSize);
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
            if (lastAddedAsset != null)
            {
                effectTypes.CallSetup(lastAddedAsset, name, parameters);
            }
        }

        public void AddGeneratorInput(int num, string generatorName)
        {
            if (!generators.ContainsKey(generatorName))
                throw new DDXXException("Generator " + generatorName + " has never been registered.");
            if (!(lastAddedAsset is IGenerator))
                throw new DDXXException("Can not call AddGeneratorInput if no Generator was just added.");

            IGenerator generator = lastAddedAsset as IGenerator;
            generator.ConnectToInput(num, generators[generatorName]);
        }

        public void SetSong(string filename)
        {
            songFilename = filename;
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



        #region IDemoMixer Members

        public Color ClearColor
        {
            get { return clearColor; }
            set { clearColor = value; }
        }

        #endregion
    }
}
