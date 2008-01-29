using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using Dope.DDXX.Graphics;
using Dope.DDXX.Input;
using Dope.DDXX.Utility;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Reflection;

namespace Dope.DDXX.DemoFramework
{
    public class DemoTweakerHandler : IDemoTweakerHandler, ITweakableFactory
    {
        private Stack<IDemoTweaker> tweakerStack;
        private IDemoTweaker firstTweaker;
        private IDemoTweakerContext context;
        private IUserInterface userInterface;
        private IDemoRegistrator registrator;
        private ITweakerSettings settings;
        private BaseControl mainWindow;
        private bool visable;
        private bool saveNeeded;
        private bool saveDone;
        private bool shouldSave;
        private bool exiting;
        private string xmlFileName;
        private XmlDocument xmlDocument;
        private List<KeyValuePair<Type, Type>> typeTweakableMapping;

        public object IdentifierToChild() { return 0; }
        public void IdentifierFromParent(object id) { }

        public IDemoTweaker Tweaker
        {
            get { if (tweakerStack.Count == 0) return null; return tweakerStack.Peek(); }
        }

        public bool Enabled
        {
            get { return tweakerStack.Count > 0; }
        }

        public bool Quit 
        {
            get { return Exiting && (!saveNeeded || saveDone); } 
        }

        public bool Exiting
        {
            get { return exiting; }
        }

        public ITweakableFactory Factory
        {
            get { return this; }
        }

        public DemoTweakerHandler(IDemoTweakerContext context, ITweakerSettings settings)
        {
            exiting = false;
            tweakerStack = new Stack<IDemoTweaker>();
            saveNeeded = false;
            saveDone = false;
            shouldSave = true;
            this.context = context;
            this.settings = settings;
            visable = true;

            typeTweakableMapping = new List<KeyValuePair<Type, Type>>();
            typeTweakableMapping.Add(new KeyValuePair<Type, Type>(typeof(bool), typeof(TweakableBoolean)));
            typeTweakableMapping.Add(new KeyValuePair<Type, Type>(typeof(Color), typeof(TweakableColor)));
            typeTweakableMapping.Add(new KeyValuePair<Type, Type>(typeof(IDemoRegistrator), typeof(TweakableDemo)));
            typeTweakableMapping.Add(new KeyValuePair<Type, Type>(typeof(int), typeof(TweakableInt32)));
            typeTweakableMapping.Add(new KeyValuePair<Type, Type>(typeof(IRegisterable), typeof(TweakableRegisterable)));
            typeTweakableMapping.Add(new KeyValuePair<Type, Type>(typeof(ITexture2D), typeof(TweakableTextureValue)));
            typeTweakableMapping.Add(new KeyValuePair<Type, Type>(typeof(Texture2DParameters), typeof(TweakableTexture2DParameters)));
            typeTweakableMapping.Add(new KeyValuePair<Type, Type>(typeof(float), typeof(TweakableSingle)));
            typeTweakableMapping.Add(new KeyValuePair<Type, Type>(typeof(string), typeof(TweakableString)));
            typeTweakableMapping.Add(new KeyValuePair<Type, Type>(typeof(Vector2), typeof(TweakableVector2)));
            typeTweakableMapping.Add(new KeyValuePair<Type, Type>(typeof(Vector3), typeof(TweakableVector3)));
            typeTweakableMapping.Add(new KeyValuePair<Type, Type>(typeof(Vector4), typeof(TweakableVector4)));
            typeTweakableMapping.Add(new KeyValuePair<Type, Type>(typeof(ITextureFactory), typeof(TweakableTextureFactory)));
        }

        public virtual void Initialize(IDemoRegistrator registrator, IUserInterface userInterface, ITweakable demoTweakable)
        {
            this.firstTweaker = new DemoTweaker(settings, demoTweakable);
            this.registrator = registrator;
            this.userInterface = userInterface;

            CreateBaseControls();

            firstTweaker.Initialize(registrator, userInterface);
        }

        private void CreateBaseControls()
        {
            mainWindow = new BoxControl(new Vector4(0.05f, 0.05f, 0.90f, 0.90f), 0, Color.Black, null);

            BaseControl titleWindow = new BoxControl(new Vector4(0, 0, 1, 0.05f),
                settings.Alpha, settings.TitleColor, mainWindow);
            int seconds = (int)Time.CurrentTime;
            int hundreds = (int)((Time.CurrentTime - seconds) * 100);
            string titleString = "DDXX Tweaker - " + seconds.ToString("D3") + "." + hundreds.ToString("D2");
            BaseControl titleText = new TextControl(titleString, new Vector4(0, 0, 1, 1), TextFormatting.Center | TextFormatting.VerticalCenter,
                settings.TextAlpha, Color.White, titleWindow);
        }

        public IDemoTweaker HandleInput(IInputDriver inputDriver)
        {
            if (Exiting)
            {
                HandleExitInput(inputDriver);
                return null;
            }
            if (Enabled)
            {
                IDemoTweaker newTweaker = Tweaker.HandleInput(inputDriver);
                if (newTweaker != null)
                    tweakerStack.Push(newTweaker);
            }

            if (inputDriver.RightPressedNoRepeat())
            {
                context.JumpInTime(5.0f);
            }

            if (inputDriver.LeftPressedNoRepeat())
            {
                context.JumpInTime(-5.0f);
            }

            if (inputDriver.PausePressedNoRepeat())
            {
                context.TogglePause();
            }

            if (inputDriver.OkPressedNoRepeat())
            {
                saveNeeded = true;
                if (Tweaker == null)
                    tweakerStack.Push(firstTweaker);
            }

            if (inputDriver.BackPressedNoRepeat())
            {
                if (tweakerStack.Count > 0)
                    tweakerStack.Pop();
                else
                    exiting = true;
            }

            if (inputDriver.KeyPressedNoRepeat(Keys.F1))
                visable = !visable;

            if (inputDriver.KeyPressedNoRepeat(Keys.F2))
                settings.SetTransparency(Transparency.Low);
            if (inputDriver.KeyPressedNoRepeat(Keys.F3))
                settings.SetTransparency(Transparency.Medium);
            if (inputDriver.KeyPressedNoRepeat(Keys.F4))
                settings.SetTransparency(Transparency.High);

            if (inputDriver.KeyPressedNoRepeat(Keys.F5))
                settings.NextColorSchema();
            if (inputDriver.KeyPressedNoRepeat(Keys.F6))
                settings.PreviousColorSchema();

            return null;
        }

        private void HandleExitInput(IInputDriver inputDriver)
        {
            if (inputDriver.RightPressedNoRepeat())
                shouldSave = false;
            if (inputDriver.LeftPressedNoRepeat())
                shouldSave = true;
            if (inputDriver.OkPressedNoRepeat())
                saveDone = true;
        }

        public void Draw()
        {
            if (Exiting)
            {
                HandleExitDraw();
                return;
            }
            if (!Enabled)
                return;

            if (visable)
            {
                Tweaker.Draw();
            }
        }

        private void HandleExitDraw()
        {
            BoxControl tweakableWindow = new BoxControl(new Vector4(0, 0.05f, 1, 0.95f),
                settings.Alpha, settings.TimeColor, mainWindow);
            new TextControl("Should old XML file be overwritten?", new Vector4(0, 0, 1, 0.90f), TextFormatting.VerticalCenter | TextFormatting.Center, 255, Color.White, tweakableWindow);
            Color yesColor = Color.DarkGray;
            Color noColor = Color.DarkGray;
            if (shouldSave)
                yesColor = Color.White;
            else
                noColor = Color.White;
            new TextControl("Yes>>>", new Vector4(0, 0, 0.5f, 1), TextFormatting.Right | TextFormatting.VerticalCenter, 255, yesColor, tweakableWindow);
            new TextControl("<<<No", new Vector4(0.5f, 0, 0.5f, 1), TextFormatting.Left | TextFormatting.VerticalCenter, 255, noColor, tweakableWindow);

            userInterface.DrawControl(mainWindow);
        }

        public bool ShouldSave()
        {
            return shouldSave && saveNeeded;
        }

        public void ReadFromXmlFile(string xmlFileName)
        {
            this.xmlFileName = FileUtility.FilePath(xmlFileName);
            xmlDocument = new XmlDocument();
            xmlDocument.PreserveWhitespace = true;
            using (Stream inputStream = new FileStream(this.xmlFileName, FileMode.Open))
            {
                xmlDocument.Load(inputStream);
                firstTweaker.ReadFromXmlFile(xmlDocument.DocumentElement);
            }
        }

        public void WriteToXmlFile()
        {
            firstTweaker.WriteToXmlFile(xmlDocument, xmlDocument.DocumentElement);

            using (Stream outStream = new FileStream(FileUtility.FilePath("..\\..\\..\\" + xmlFileName), FileMode.Create))
                xmlDocument.Save(outStream);
        }

        #region ITweakableFactory Members

        public ITweakableProperty CreateTweakableValue(PropertyInfo property, object target)
        {
            foreach (KeyValuePair<Type, Type> pair in typeTweakableMapping)
            {
                if (pair.Key == property.PropertyType)
                {
                    ConstructorInfo constructor = pair.Value.GetConstructor(new Type[] { typeof(PropertyInfo), typeof(object) });
                    if (constructor != null)
                        return constructor.Invoke(new object[] { property, target }) as ITweakableProperty;
                    //object propertyTarget = property.GetGetMethod().Invoke(target, null);
                    //if (propertyTarget != null)
                    //    return CreateTweakableObject(propertyTarget);
                }
            }
            return null;
        }

        public ITweakable CreateTweakableObject(object target)
        {
            foreach (KeyValuePair<Type, Type> pair in typeTweakableMapping)
            {
                if (pair.Key == target.GetType() || target.GetType().GetInterface(pair.Key.Name) != null)
                {
                    ConstructorInfo constructor = pair.Value.GetConstructor(new Type[] { pair.Key, typeof(ITweakableFactory) });
                    if (constructor != null)
                        return constructor.Invoke(new object[] { target, this }) as ITweakable;
                }
            }
            return null;
        }

        public ITextureFactory TextureFactory
        {
            get { return registrator.TextureFactory; }
        }

        #endregion
    }
}
