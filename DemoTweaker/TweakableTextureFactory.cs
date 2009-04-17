using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Xml;
using Microsoft.Xna.Framework.Input;
using Dope.DDXX.UserInterface;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.Utility;
using Dope.DDXX.TextureBuilder;
using System.Reflection;

namespace Dope.DDXX.DemoTweaker
{
    public class TweakableTextureFactory : TweakableObjectBase<ITextureFactory>
    {
        private IMenuControl<Type> menuControl;

        public TweakableTextureFactory(ITextureFactory target, ITweakableFactory factory)
            : base(target, factory)
        {
        }

        public override int NumVisableVariables
        {
            get { return 3; }
        }

        protected override int NumSpecificVariables
        {
            get { return TextureParameters.Count; }
        }

        protected override ITweakable GetSpecificVariable(int index)
        {
            return Factory.CreateTweakableObject(TextureParameters[index]);
        }

        protected override void ParseSpecficXmlNode(XmlNode node)
        {
            if (node.Name != "Texture")
                throw new DDXXException("Unknown node \"" + node.Name + "\" in TextureFactory");
            RegisterTexture(node);
        }

        protected override void WriteSpecificXmlNode(XmlDocument xmlDocument, XmlNode node)
        {
            Texture2DParameters parameter = TextureParameters.Find(delegate(Texture2DParameters param) { return param.Name == GetStringAttribute(node, "name"); });
            if (parameter != null)
            {
                UpdateTexture(xmlDocument, node, parameter);
            }
        }

        protected override void WriteNewNodes(XmlDocument xmlDocument, XmlNode node)
        {
            foreach (Texture2DParameters parameter in TextureParameters)
            {
                bool found = false;
                foreach (XmlNode child in node.ChildNodes)
                {
                    if (child.Name == "Texture" && GetStringAttribute(child, "name") == parameter.Name)
                        found = true;
                }
                if (!found)
                {
                    XmlElement newNode = AddNewChild(xmlDocument, node, "Texture");
                    AddAttribute(xmlDocument, newNode, "name", parameter.Name);
                    AddAttribute(xmlDocument, newNode, "width", parameter.Texture.Width.ToString());
                    AddAttribute(xmlDocument, newNode, "height", parameter.Texture.Height.ToString());
                    AddAttribute(xmlDocument, newNode, "miplevels", parameter.Texture.LevelCount > 1 ? "0" : "1");
                    UpdateTexture(xmlDocument, newNode, parameter);
                }
            }
        }

        private void UpdateTexture(XmlDocument xmlDocument, XmlNode node, Texture2DParameters parameter)
        {
            while (node.FirstChild != null)
                node.RemoveChild(node.FirstChild);
            List<ITextureGenerator> generators = new List<ITextureGenerator>();
            AddGenerator(parameter.Generator, generators);
            generators.Reverse();
            foreach (ITextureGenerator generator in generators)
            {
                XmlElement newNode = AddNewChild(xmlDocument, node, "Generator");
                AddAttribute(xmlDocument, newNode, "class", generator.GetType().Name);
                Factory.CreateTweakableObject(generator).WriteToXmlFile(xmlDocument, newNode);
            }
        }

        private void AddGenerator(ITextureGenerator generator, List<ITextureGenerator> generators)
        {
            generators.Add(generator);
            for (int i = 0; i < generator.NumInputPins; i++)
                AddGenerator(generator.GetInput(i), generators);
        }

        private void RegisterTexture(XmlNode node)
        {
            TextureDirector director = new TextureDirector(Target);
            foreach (XmlNode child in node.ChildNodes)
            {
                if (!(child is XmlElement))
                    continue;
                if (child.Name != "Generator")
                    throw new DDXXException("Unknown node \"" + child.Name + "\" in Texture");
                ITextureGenerator generator = Factory.EffectTypes.CreateGenerator(GetStringAttribute(child, "class"));
                director.AddGenerator(generator);
                ITweakable tweakableGenerator = Factory.CreateTweakableObject(generator);
                tweakableGenerator.ReadFromXmlFile(child);
            }
            director.Generate(GetStringAttribute(node, "name"), GetIntAttribute(node, "width"),
                GetIntAttribute(node, "height"), GetIntAttribute(node, "miplevels"), SurfaceFormat.Color);
        }

        public override void CreateControl(TweakerStatus status, int index, float y, ITweakerSettings settings)
        {
            float height = status.VariableSpacing * 0.9f;
            if (index == status.Selection)
                new BoxControl(new Vector4(0, y, 1, height), settings.Alpha, settings.SelectedColor, status.RootControl);
            new TextControl("TextureFactory", new Vector4(0, y, 0.45f, height), Positioning.Right | Positioning.VerticalCenter, settings.TextAlpha, Color.White, status.RootControl);

            new TextControl("<ITextureFactory>",
                new Vector4(0.55f, y, 0.45f, height), Positioning.Center | Positioning.VerticalCenter,
                settings.TextAlpha, Color.White, status.RootControl);
        }

        private List<Texture2DParameters> TextureParameters
        {
            get
            {
                return Target.Texture2DParameters.FindAll(delegate(Texture2DParameters param) { return param.IsGenerated; });
            }
        }

        public override void Regenerate(TweakerStatus status)
        {
            TextureParameters[status.Selection].Regenerate();
        }

        public override IMenuControl InsertNew(TweakerStatus status, IDrawResources drawResources)
        {
            List<Type> generators = EnumerateGenerators(0);
            generators.Sort(delegate(Type t1, Type t2) { return t1.Name.CompareTo(t2.Name); });
            menuControl = Factory.CreateMenuControl<Type>();
            for (int i = 0; i < generators.Count; i++)
                menuControl.AddOption(generators[i].Name, generators[i]);
            menuControl.Title = "Select Base Generator";
            return menuControl;
        }

        public override IMenuControl ChoiceMade(TweakerStatus status, int index)
        {
            Type type = menuControl.Action;
            ITextureGenerator newGenerator = createGenerator(type);
            string newName = "Texture - " + Rand.Int(0, 65535);
            TextureDirector director = new TextureDirector(Target);
            director.AddGenerator(newGenerator);
            director.Generate(newName, 64, 64, 1, SurfaceFormat.Color);
            int newIndex = TextureParameters.FindIndex(delegate(Texture2DParameters param)
            {
                return param.Name == newName;
            });
            status.Selection = newIndex;
            return null;
        }

        private ITextureGenerator createGenerator(Type type)
        {
            ConstructorInfo constructor = type.GetConstructor(new Type[] { });
            return constructor.Invoke(new object[] { }) as ITextureGenerator;
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

    }
}
