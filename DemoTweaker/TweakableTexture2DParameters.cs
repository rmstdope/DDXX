using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Xml;
using Microsoft.Xna.Framework.Input;
using Dope.DDXX.UserInterface;
using System.Reflection;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.Utility;
using Dope.DDXX.TextureBuilder;

namespace Dope.DDXX.DemoTweaker
{
    public class TweakableTexture2DParameters : TweakableObjectBase<Texture2DParameters>
    {
        private List<ITextureGenerator> generators = new List<ITextureGenerator>();
        private ITweakable[] cachedChildren;
        private bool insertAfter;
        private IMenuControl<Type> menuControl;

        public TweakableTexture2DParameters(Texture2DParameters target, ITweakableFactory factory)
            : base(target, factory)
        {
            Reinitialize();
        }

        private void Reinitialize()
        {
            generators.Clear();
            ListGenerators(Target.Generator);
            generators.Reverse();
            cachedChildren = new ITweakable[generators.Count];
        }

        private void ListGenerators(ITextureGenerator generator)
        {
            generators.Add(generator);
            for (int i = 0; i < generator.NumInputPins; i++)
                ListGenerators(generator.GetInput(i));
        }

        public override int NumVisableVariables
        {
            get { return 5; }
        }

        protected override int NumSpecificVariables
        {
            get { return generators.Count; }
        }

        protected override ITweakable GetSpecificVariable(int index)
        {
            if (cachedChildren[index] == null)
                cachedChildren[index] = Factory.CreateTweakableObject(generators[index]);
            return cachedChildren[index];
        }

        protected override void ParseSpecficXmlNode(XmlNode node)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        protected override void WriteSpecificXmlNode(XmlDocument xmlDocument, XmlNode node)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void CreateControl(TweakerStatus status, int index, float y, ITweakerSettings settings)
        {
            float height = status.VariableSpacing * 0.9f;
            if (index == status.Selection)
                new BoxControl(new Vector4(0, y, 1, height), settings.Alpha, settings.SelectedColor, status.RootControl);
            new TextControl(Target.Name + " (<ITexture2D>)", new Vector4(0, y, 0.45f, height), Positioning.Right | Positioning.VerticalCenter, settings.TextAlpha, Color.White, status.RootControl);

            new BoxControl(new Vector4(0.55f + 0.225f - height / 2, y, height / 2, height), 255, Target.Texture, status.RootControl);
        }

        public override IMenuControl InsertNew(TweakerStatus status, IDrawResources drawResources, bool after)
        {
            List<Type> generators = EnumerateGenerators(1);//ValidateAndGetStackSize());
            menuControl = Factory.CreateMenuControl<Type>();
            for (int i = 0; i < generators.Count; i++)
                menuControl.AddOption(generators[i].Name, generators[i]);
            menuControl.Title = "Select Generator";
            //insertAfter = after;
            return menuControl;
        }

        public override void ChoiceMade(TweakerStatus status, int index)
        {
            Type type = menuControl.Action;
            ITextureGenerator newGenerator = createGenerator(type);
            ITextureGenerator generator = generators[status.Selection];
            foreach (ITextureGenerator heystack in generators)
            {
                for (int i = 0; i < heystack.NumInputPins; i++)
                    if (heystack.GetInput(i) == generator)
                        heystack.ConnectToInput(i, newGenerator);
            }
            newGenerator.ConnectToInput(0, generator);
            Reinitialize();
        }

        private int ValidateAndGetStackSize()
        {
            int size = 0;
            foreach (ITextureGenerator generator in generators)
            {
                size -= generator.NumInputPins;
                if (size < 0)
                    throw new DDXXException("Invalid ITextureGenerator stack. Will not render.");
                size++;
            }
            return size;
        }

        private List<Type> EnumerateGenerators(int numPins)
        {
            List<Type> generators = new List<Type>();
            foreach (Type type in typeof(Constant).Assembly.GetTypes())
            {
                if (type.GetInterface("ITextureGenerator") != null &&
                    !type.IsAbstract && type.IsPublic)
                {
                    ITextureGenerator generator = createGenerator(type);
                    if (generator.NumInputPins == numPins)
                        generators.Add(type);
                }
            }
            return generators;
        }

        private ITextureGenerator createGenerator(Type type)
        {
            ConstructorInfo constructor = type.GetConstructor(new Type[] { });
            return constructor.Invoke(new object[] { }) as ITextureGenerator;
        }

        public override void Regenerate(TweakerStatus status)
        {
            cachedChildren = new ITweakable[generators.Count];
        }
    }
}
