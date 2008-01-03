using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using Dope.DDXX.Graphics;
using Dope.DDXX.Input;
using Dope.DDXX.Sound;
using Dope.DDXX.TextureBuilder;
using Dope.DDXX.Utility;
using System.Reflection;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Dope.DDXX.DemoFramework
{
    public class DemoExecuter : IDemoEffectBuilder, IDemoRegistrator, IDemoTweakerContext, IDemoMixer
    {
        private ISoundDriver soundDriver;
        private ICue song;

        private IGraphicsDevice device;
        private IGraphicsFactory graphicsFactory;
        private ITextureFactory textureFactory;
        private ITextureBuilder textureBuilder;
        private IPostProcessor postProcessor;
        private IDemoTweaker tweaker;
        private ISpriteBatch spriteBatch;
        private IRenderTarget2D renderTarget;
        private IRenderTarget2D renderTargetNoMultiSampling;
        private IDepthStencilBuffer depthStencilBuffer;

        private IInputDriver inputDriver;
        private List<ITrack> tracks = new List<ITrack>();
        private List<IDemoTransition> transitions = new List<IDemoTransition>();

        private IDemoFactory demoFactory;
        private IDemoEffectTypes effectTypes;
        private TweakerSettings settings = new TweakerSettings();
        private DemoXMLReader xmlReader;
        private Color clearColor = new Color(0, 0, 0, 0);
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

        public void Initialize(IGraphicsDevice device, IGraphicsFactory graphicsFactory,
            ITextureFactory textureFactory, IEffectFactory effectFactory,
            ITextureBuilder textureBuilder, IDeviceParameters deviceParameters)
        {
            this.Initialize(device, graphicsFactory, textureFactory, effectFactory, textureBuilder, "", deviceParameters);
        }

        public void Initialize(IGraphicsDevice device, IGraphicsFactory graphicsFactory, 
            ITextureFactory textureFactory, IEffectFactory effectFactory,
            ITextureBuilder textureBuilder, string xmlFile, IDeviceParameters deviceParameters)
        {
            this.device = device;
            this.graphicsFactory = graphicsFactory;
            this.textureFactory = textureFactory;
            this.textureBuilder = textureBuilder;
            this.spriteBatch = graphicsFactory.CreateSpriteBatch();
            this.renderTarget = textureFactory.CreateFullsizeRenderTarget(deviceParameters.RenderTargetFormat, deviceParameters.MultiSampleType, 0);
            if (deviceParameters.MultiSampleType == MultiSampleType.None)
                this.renderTargetNoMultiSampling = this.renderTarget;
            else
                this.renderTargetNoMultiSampling = textureFactory.CreateFullsizeRenderTarget();
            this.depthStencilBuffer = textureFactory.CreateFullsizeDepthStencil(deviceParameters.DepthStencilFormat, deviceParameters.MultiSampleType);

            InitializeFromFile(xmlFile);

            InitializeSound();

            postProcessor.Initialize(device, graphicsFactory, textureFactory, effectFactory);

            foreach (ITrack track in tracks)
            {
                track.Initialize(graphicsFactory, device, textureFactory, effectFactory, textureBuilder, this, postProcessor);
            }
            foreach (IDemoTransition transition in transitions)
            {
                transition.Initialize(postProcessor);
            }

            tweaker.Initialize(this, graphicsFactory, textureFactory);

            Time.CurrentTime = StartTime;
        }

        private void InitializeSound()
        {
            if (songFilename != null && songFilename != "")
            {
                soundDriver.Initialize("Test");
                song = soundDriver.PlaySound(songFilename);
                //System.Diagnostics.Debug.WriteLine(song.GetVariable("Position"));
            }
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
            //System.Diagnostics.Debug.WriteLine(song.GetVariable("Position"));
            inputDriver.Step();
            soundDriver.Step();
            tweaker.HandleInput(inputDriver);

            foreach (ITrack track in tracks)
            {
                track.Step();
            }
        }

        public void CleanUp()
        {
            if (tweaker.ShouldSave() && xmlReader != null)
            {
                Update(xmlReader);
                xmlReader.Write();
            }
        }

        public bool IsRunning()
        {
            return !tweaker.Quit;
        }

        //private void SynchronizeSong()
        //{
        //    if (sound != null)
        //    {
        //        soundDriver.SetPosition(channel, Time.CurrentTime);
        //        //Time.CurrentTime = soundDriver.GetPosition(channel);
        //        //System.Diagnostics.Debug.Write("Syncronizing sound: SoundTime=" + soundDriver.GetPosition(channel));
        //        //System.Diagnostics.Debug.WriteLine(", ActualTime: " + Time.CurrentTime);
        //    }
        //}

#if (!XBOX)
        private int screenshotNum = 0;
#endif
        public void Render()
        {
            ITexture2D renderedTexture = RenderTracks();

#if (!XBOX)
            if (inputDriver.KeyPressedNoRepeat(Keys.F12))
                renderedTexture.Save("Screenshot" + screenshotNum++ + ".jpg", ImageFileFormat.Jpg);
#endif

            if (renderedTexture != null)
            {
                device.SetRenderTarget(0, null);
                spriteBatch.Begin(SpriteBlendMode.None, SpriteSortMode.Immediate, SaveStateMode.None);
                spriteBatch.Draw(renderedTexture, new Rectangle(0, 0, device.PresentationParameters.BackBufferWidth,
                    device.PresentationParameters.BackBufferHeight), Color.White);
                spriteBatch.End();
            }
 
            tweaker.Draw();
        }

        private ITexture2D RenderTracks()
        {
            IRenderTarget2D finalRenderTarget = null;
            if (tracks.Count != 0)
            {
                finalRenderTarget = GetActiveTrack().Render(device, renderTarget, renderTargetNoMultiSampling, depthStencilBuffer, clearColor);
                IDemoTransition transition = GetActiveTransition();
                if (transition != null)
                {
                    postProcessor.AllocateTexture(finalRenderTarget);
                    IRenderTarget2D finalRenderTarget2 = tracks[transition.DestinationTrack].Render(device, renderTarget, renderTargetNoMultiSampling, depthStencilBuffer, clearColor);
                    //postProcessor.AllocateTexture(finalRenderTarget2);
                    IRenderTarget2D newFinalRenderTarget = transition.Render(finalRenderTarget, finalRenderTarget2);
                    postProcessor.FreeTexture(finalRenderTarget);
                    //postProcessor.FreeTexture(finalRenderTarget2);
                    finalRenderTarget = newFinalRenderTarget;
                }
            }
            if (finalRenderTarget == null)
                return null;
            return finalRenderTarget.GetTexture();
        }

        private IDemoTransition GetActiveTransition()
        {
            foreach (IDemoTransition transition in transitions)
            {
                if (transition.StartTime <= Time.CurrentTime &&
                    transition.EndTime > Time.CurrentTime)
                    return transition;
            }
            return null;
        }

        private ITrack GetActiveTrack()
        {
            int trackNum = 0;
            foreach (IDemoTransition transition in transitions)
            {
                if (transition.EndTime <= Time.CurrentTime)
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
            IRegisterable effect = effectTypes.CreateInstance(className, effectName, startTime, endTime);
            if (!(effect is IDemoEffect))
                throw new DDXXException("Tried to add a demo effect " + effectName + " of class " + className +
                    "which was not of type IDemoEffect");
            IDemoEffect demoEffect = (IDemoEffect)effect;
            this.Register(effectTrack, demoEffect);
            lastAddedAsset = demoEffect;
        }

        public void AddPostEffect(string className, string effectName, int effectTrack, float startTime, float endTime)
        {
            IRegisterable effect = effectTypes.CreateInstance(className, effectName, startTime, endTime);
            if (!(effect is IDemoPostEffect))
                throw new DDXXException("Tried to add a demo post effect " + effectName + " of class " + className +
                    "which was not of type IDemoPostEffect");
            IDemoPostEffect postEffect = (IDemoPostEffect)effect;
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
            ITexture2D texture = textureBuilder.Generate(generators[generatorName], width, height, mipLevels, SurfaceFormat.Color);
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

        public void AddVector2Parameter(string name, Vector2 value, float stepSize)
        {
            AddParameter(name, value, stepSize);
        }

        public void AddVector3Parameter(string name, Vector3 value, float stepSize)
        {
            AddParameter(name, value, stepSize);
        }

        public void AddVector4Parameter(string name, Vector4 value, float stepSize)
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
                if (song != null)
                {
                    song.Resume();
                    //soundDriver.SetPosition(channel, Time.CurrentTime);
                    //soundDriver.ResumeChannel(channel);
                }
            }
            else
            {
                Time.Pause();
                if (song != null)
                {
                    song.Pause();
                    //soundDriver.PauseChannel(channel);
                }
            }
        }

        public void JumpInTime(float time)
        {
            Time.DeltaTime += time;
            Time.CurrentTime += time;
            if (Time.CurrentTime < StartTime)
                Time.CurrentTime = StartTime;
            if (Time.CurrentTime > EndTime)
                Time.CurrentTime = EndTime;
            //SynchronizeSong();
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
