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
        private IMenuControl<Type> menuControl;
        private Type combineType;

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
            new TextControl(Target.Name + " (<Texture2D>)", new Vector4(0, y, 0.45f, height), Positioning.Right | Positioning.VerticalCenter, settings.TextAlpha, Color.White, status.RootControl);
            new BoxControl(new Vector4(0.55f + 0.225f - height / 2, y, -1, height), 255, Target.Texture, status.RootControl);
        }

        public override IMenuControl InsertNew(TweakerStatus status, IDrawResources drawResources)
        {
            menuControl = Factory.CreateMenuControl<Type>();
            List<Type> list = new List<Type>(EnumerateGenerators(1));
            list.Sort(delegate(Type t1, Type t2) { return t1.Name.CompareTo(t2.Name); });
            foreach (Type generator in list)
                menuControl.AddOption(generator.Name, generator);
            list = new List<Type>(EnumerateGenerators(2));
            list.Sort(delegate(Type t1, Type t2) { return t1.Name.CompareTo(t2.Name); });
            foreach (Type generator in list)
                menuControl.AddOption(generator.Name + " with ...", generator);
            menuControl.Title = "Select Generator To Add";
            return menuControl;
        }

        public override IMenuControl ChoiceMade(TweakerStatus status, int index)
        {
            if (GeneratorHasNumPins(menuControl.Action, 2))
            {
                combineType = menuControl.Action;
                menuControl = Factory.CreateMenuControl<Type>();
                foreach (Type generator in EnumerateGenerators(0))
                    menuControl.AddOption(generator.Name, generator);
                menuControl.Title = "Select Secondary Input";
                return menuControl;
            }
            ITextureGenerator newGenerator = createGenerator(menuControl.Action);
            if (GeneratorHasNumPins(menuControl.Action, 0))
            {
                ITextureGenerator tempGenerator = createGenerator(combineType);
                tempGenerator.ConnectToInput(1, newGenerator);
                newGenerator = tempGenerator;
            }
            ConnectGeneratorAfter(generators[status.Selection], newGenerator);
            Reinitialize();
            return null;
        }

        public override void Delete(TweakerStatus status)
        {
            if (generators[status.Selection].NumInputPins == 1)
            {
                ITextureGenerator before = generators[status.Selection].GetInput(0);
                ITextureGenerator after = generators[status.Selection].Output;
                if (after != null)
                    after.ConnectToInput(after.GetInputIndex(generators[status.Selection]), before);
                else
                {
                    before.Output = null;
                    Target.Generator = before;
                    status.Selection--;
                }
                Reinitialize();
            }
        }

        private void ConnectGeneratorAfter(ITextureGenerator oldGenerator, ITextureGenerator newGenerator)
        {
            if (oldGenerator.Output != null)
                oldGenerator.Output.ConnectToInput(oldGenerator.Output.GetInputIndex(oldGenerator), newGenerator);
            newGenerator.ConnectToInput(0, oldGenerator);
            if (Target.Generator == oldGenerator)
                Target.Generator = newGenerator;
        }

        private IEnumerable<Type> EnumerateGenerators(int numPins)
        {
            foreach (Type type in typeof(Constant).Assembly.GetTypes())
            {
                if (type.GetInterface("ITextureGenerator") != null &&
                    !type.IsAbstract && type.IsPublic &&
                    GeneratorHasNumPins(type, numPins))
                {
                    yield return type;
                }
            }
        }

        private bool GeneratorHasNumPins(Type type, int numPins)
        {
            return (numPins == createGenerator(type).NumInputPins);
        }

        private ITextureGenerator createGenerator(Type type)
        {
            return type.GetConstructor(new Type[] { }).Invoke(new object[] { }) as ITextureGenerator;
        }

        public override void Regenerate(TweakerStatus status)
        {
            cachedChildren = new ITweakable[generators.Count];
        }
    }
}
