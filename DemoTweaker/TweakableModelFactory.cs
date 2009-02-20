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
using Dope.DDXX.ModelBuilder;
using Dope.DDXX.Utility;

namespace Dope.DDXX.DemoTweaker
{
    public class TweakableModelFactory : TweakableObjectBase<IModelFactory>
    {
        public TweakableModelFactory(IModelFactory target, ITweakableFactory factory)
            : base(target, factory)
        {
        }

        public override int NumVisableVariables
        {
            get { return 5; }
        }

        protected override int NumSpecificVariables
        {
            get { return ModelParameters.Count; }
        }

        protected override ITweakable GetSpecificVariable(int index)
        {
            return Factory.CreateTweakableObject(ModelParameters[index]);
        }

        protected override void ParseSpecficXmlNode(XmlNode node)
        {
            if (node.Name != "Model")
                throw new DDXXException("Unknown node \"" + node.Name + "\" in ModelFactory");
            RegisterModel(node);
        }

        protected override void WriteSpecificXmlNode(XmlDocument xmlDocument, XmlNode node)
        {
            ModelParameters parameter = ModelParameters.Find(delegate(ModelParameters param) { return param.Name == GetStringAttribute(node, "name"); });
            if (parameter != null)
            {
                UpdateModel(xmlDocument, node, parameter);
            }
        }

        protected override void WriteNewNodes(XmlDocument xmlDocument, XmlNode node)
        {
            foreach (ModelParameters parameter in ModelParameters)
            {
                bool found = false;
                foreach (XmlNode child in node.ChildNodes)
                {
                    if (child.Name == "Model" && GetStringAttribute(child, "name") == parameter.Name)
                        found = true;
                }
                if (!found)
                {
                    XmlElement newNode = AddNewChild(xmlDocument, node, "Model");
                    AddAttribute(xmlDocument, newNode, "name", parameter.Name);
                    //AddAttribute(xmlDocument, newNode, "width", parameter.Model.Width.ToString());
                    //AddAttribute(xmlDocument, newNode, "height", parameter.Model.Height.ToString());
                    //AddAttribute(xmlDocument, newNode, "miplevels", parameter.Model.LevelCount > 1 ? "0" : "1");
                    UpdateModel(xmlDocument, newNode, parameter);
                }
            }
        }

        private void UpdateModel(XmlDocument xmlDocument, XmlNode node, ModelParameters parameter)
        {
            while (node.FirstChild != null)
                node.RemoveChild(node.FirstChild);
            List<IModifier> generators = new List<IModifier>();
            //AddGenerator(parameter.Generator, generators);
            generators.Reverse();
            foreach (IModifier generator in generators)
            {
                XmlElement newNode = AddNewChild(xmlDocument, node, "Generator");
                AddAttribute(xmlDocument, newNode, "class", generator.GetType().Name);
                Factory.CreateTweakableObject(generator).WriteToXmlFile(xmlDocument, newNode);
            }
        }

        private void AddGenerator(IModifier generator, List<IModifier> generators)
        {
            //generators.Add(generator);
            //for (int i = 0; i < generator.NumInputPins; i++)
            //    AddGenerator(generator.GetInput(i), generators);
        }

        private void RegisterModel(XmlNode node)
        {
            //ModelDirector director = new ModelDirector(Target);
            //foreach (XmlNode child in node.ChildNodes)
            //{
            //    if (!(child is XmlElement))
            //        continue;
            //    if (child.Name != "Generator")
            //        throw new DDXXException("Unknown node \"" + child.Name + "\" in Model");
            //    IModifier generator = Factory.EffectTypes.CreateGenerator(GetStringAttribute(child, "class"));
            //    director.AddGenerator(generator);
            //    ITweakable tweakableGenerator = Factory.CreateTweakableObject(generator);
            //    tweakableGenerator.ReadFromXmlFile(child);
            //}
            //director.Generate(GetStringAttribute(node, "name"), GetIntAttribute(node, "width"),
            //    GetIntAttribute(node, "height"), GetIntAttribute(node, "miplevels"), SurfaceFormat.Color);
        }

        public override void CreateControl(TweakerStatus status, int index, float y, ITweakerSettings settings)
        {
            float height = status.VariableSpacing * 0.9f;
            if (index == status.Selection)
                new BoxControl(new Vector4(0, y, 1, height), settings.Alpha, settings.SelectedColor, status.RootControl);
            new TextControl("ModelFactory", new Vector4(0, y, 0.45f, height), Positioning.Right | Positioning.VerticalCenter, settings.TextAlpha, Color.White, status.RootControl);

            new TextControl("<IModelFactory>",
                new Vector4(0.55f, y, 0.45f, height), Positioning.Center | Positioning.VerticalCenter,
                settings.TextAlpha, Color.White, status.RootControl);
        }

        private List<ModelParameters> ModelParameters
        {
            get
            {
                return Target.ModelParameters.FindAll(delegate(ModelParameters param) { return param.IsGenerated; });
            }
        }

        public override void Regenerate(TweakerStatus status)
        {
            //ModelParameters[status.Selection].Regenerate();
        }

    }
}
