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
        private IPostProcessor postProcessor;
        private IDemoTweaker tweaker;

        private IInputDriver inputDriver;
        private List<ITrack> tracks = new List<ITrack>();
        private List<IDemoTransition> transitions = new List<IDemoTransition>();

        private IDemoFactory demoFactory;
        private IDemoEffectTypes effectTypes;
        private TweakerSettings settings = new TweakerSettings();
        private DemoXMLReader xmlReader;
        private Color clearColor = Color.FromArgb(0, 0, 0, 0);//50, 50, 50);//10, 10, 10);
        private Dictionary<string, IGenerator> generators = new Dictionary<string,IGenerator>();

        private string songFilename;

        public float StartTime
        {
            get
            {
                float minTime = float.MaxValue;
                foreach (ITrack track in tracks)
                {
                    if (track.StartTime < minTime)
                        minTime = track.StartTime;
                }
                if (minTime == float.MaxValue)
                    minTime = 0;
                return minTime;
            }
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

        public DemoExecuter(IDemoFactory demoFactory, ISoundDriver soundDriver, IInputDriver inputDriver, IPostProcessor postProcessor, IDemoEffectTypes effectTypes)
        {
            this.demoFactory = demoFactory;
            this.soundDriver = soundDriver;
            this.inputDriver = inputDriver;
            this.postProcessor = postProcessor;
            this.effectTypes = effectTypes;
            tweaker = new DemoTweakerMain(this, new IDemoTweaker[] { new DemoTweakerDemo(settings), new DemoTweakerTrack(settings), new DemoTweakerEffect(settings) }, settings);
        }

        public void Initialize(IDevice device, IGraphicsFactory graphicsFactory,
            ITextureFactory textureFactory, IEffectFactory effectFactory,
            ITextureBuilder textureBuilder)
        {
            this.Initialize(device, graphicsFactory, textureFactory, effectFactory, textureBuilder, "");
        }

        public void Initialize(IDevice device, IGraphicsFactory graphicsFactory, 
            ITextureFactory textureFactory, IEffectFactory effectFactory, 
            ITextureBuilder textureBuilder, string xmlFile)
        {
            this.device = device;
            this.graphicsFactory = graphicsFactory;
            this.textureFactory = textureFactory;
            this.textureBuilder = textureBuilder;

            Time.Initialize();

            InitializeFromFile(xmlFile);

            InitializeSound();

            postProcessor.Initialize(device, textureFactory, effectFactory);

            foreach (ITrack track in tracks)
            {
                track.Initialize(graphicsFactory, device, textureFactory, effectFactory, textureBuilder, this, postProcessor);
            }
            foreach (IDemoTransition transition in transitions)
            {
                transition.Initialize(device, postProcessor);
            }

            tweaker.Initialize(this);
        }

        private void InitializeSound()
        {
            soundDriver.Initialize();
            if (songFilename != null && songFilename != "")
                sound = soundDriver.CreateSound(songFilename);
        }

        public void Register(int track, IDemoEffect effect)
        {
            foreach (ITrack testTrack in tracks)
            {
                if (testTrack.IsEffectRegistered(effect.Name, effect.GetType()))
                    throw new DDXXException("Can not have two Effects of class " + effect.GetType().Name +
                        "and name " + effect.Name + ".");
            }
            while (NumTracks <= track)
            {
                tracks.Add(demoFactory.CreateTrack());
            }
            tracks[track].Register(effect);
        }

        public void Register(int track, IDemoPostEffect postEffect)
        {
            foreach (ITrack testTrack in tracks)
            {
                if (testTrack.IsPostEffectRegistered(postEffect.Name, postEffect.GetType()))
                    throw new DDXXException("Can not have two PostEffects of class " + postEffect.GetType().Name + 
                        "and name " + postEffect.Name + ".");
            }
            while (NumTracks <= track)
            {
                tracks.Add(demoFactory.CreateTrack());
            }
            tracks[track].Register(postEffect);
        }

        public void Register(IDemoTransition transition)
        {
            foreach (IDemoTransition compare in transitions)
            {
                if (compare.StartTime < transition.EndTime &&
                    compare.EndTime > transition.StartTime)
                    throw new DDXXException("Can not have overlapping transitions.");
            }
            foreach (IDemoTransition compare in transitions)
            {
                if (compare.Name == transition.Name &&
                    compare.GetType() == transition.GetType())
                    throw new DDXXException("Can not have two Transitions of class " + transition.GetType().Name +
                        "and name " + transition.Name + ".");
            }

            transitions.Add(transition);
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
            Time.CurrentTime = StartTime;

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

        private int screenshotNum = 0;
        public void Render()
        {
            ITexture renderedTexture = RenderTracks();

            if (inputDriver.KeyPressedNoRepeat(Key.F12))
                renderedTexture.Save("Screenshot" + screenshotNum++ + ".jpg", ImageFileFormat.Jpg);

            if (renderedTexture != null)
            {
                using (ISurface source = renderedTexture.GetSurfaceLevel(0))
                {
                    using (ISurface destination = device.GetRenderTarget(0))
                    {
                        device.StretchRectangle(source, new Rectangle(0, 0, source.Description.Width, source.Description.Height),
                                                destination, new Rectangle(0, 0, destination.Description.Width, destination.Description.Height),
                                                TextureFilter.None);
                    }
                }
            }

            tweaker.Draw();

            device.Present();
        }

        private ITexture RenderTracks()
        {
            ITexture finalTexture = null;
            if (tracks.Count != 0)
            {
                finalTexture = GetActiveTrack().Render(device, postProcessor.GetTemporaryTextures(1, false)[0], clearColor);
                IDemoTransition transition = GetActiveTransition();
                if (transition != null)
                {
                    postProcessor.AllocateTexture(finalTexture);
                    ITexture finalTexture2 = tracks[transition.DestinationTrack].Render(device, postProcessor.GetTemporaryTextures(1, false)[0], clearColor);
                    postProcessor.AllocateTexture(finalTexture2);
                    ITexture newFinalTexture = transition.Render(finalTexture, finalTexture2);
                    postProcessor.FreeTexture(finalTexture);
                    postProcessor.FreeTexture(finalTexture2);
                    finalTexture = newFinalTexture;
                }
            }
            return finalTexture;
        }

        private IDemoTransition GetActiveTransition()
        {
            foreach (IDemoTransition transition in transitions)
            {
                if (transition.StartTime <= Time.StepTime &&
                    transition.EndTime > Time.StepTime)
                    return transition;
            }
            return null;
        }

        private ITrack GetActiveTrack()
        {
            int trackNum = 0;
            foreach (IDemoTransition transition in transitions)
            {
                if (transition.EndTime <= Time.StepTime)
                    trackNum = transition.DestinationTrack;
            }
            return tracks[trackNum];
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

        public void AddEffect(string className, string effectName, int effectTrack, float startTime, float endTime)
        {
            IDemoEffect effect = (IDemoEffect)effectTypes.CreateInstance(className, effectName, startTime, endTime);
            this.Register(effectTrack, effect);
            lastAddedAsset = effect;
        }

        public void AddPostEffect(string className, string effectName, int effectTrack, float startTime, float endTime)
        {
            IDemoPostEffect postEffect = (IDemoPostEffect)effectTypes.CreateInstance(className, effectName, startTime, endTime);
            this.Register(effectTrack, postEffect);
            lastAddedAsset = postEffect;
        }

        public void AddTransition(string className, string effectName, int destinationTrack, float startTime, float endTime)
        {
            IDemoTransition transition = (IDemoTransition)effectTypes.CreateInstance(className, effectName, startTime, endTime);
            transition.DestinationTrack = destinationTrack;
            this.Register(transition);
            lastAddedAsset = transition;
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
