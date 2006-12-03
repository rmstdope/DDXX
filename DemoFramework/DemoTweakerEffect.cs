using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Utility;
using Dope.DDXX.Input;
using System.Drawing;
using Microsoft.DirectX.Direct3D;
using Dope.DDXX.Graphics;
using Microsoft.DirectX.DirectInput;

namespace Dope.DDXX.DemoFramework
{
    public class DemoTweakerEffect : IDemoTweaker
    {
        private IUserInterface userInterface;
        private IDemoRegistrator registrator;
        private ITweakableContainer currentContainer;

        private BoxControl mainWindow;
        private BoxControl titleWindow;
        private TextControl titleText;
        private BoxControl tweakableWindow;

        private float alpha = 0.4f;
        private float textAlpha = 0.6f;
        private Color titleColor = Color.Aquamarine;
        private Color timeColor = Color.BurlyWood;

        private int currentVariable;

        public ITweakableContainer CurrentContainer
        {
            get { return currentContainer; }
            set
            {
                if (value != currentContainer)
                    currentVariable = 0;
                currentContainer = value;
            }
        }

        public int CurrentVariable
        {
            get { return currentVariable; }
        }

        public IUserInterface UserInterface
        {
            set { userInterface = value; }
        }

        public DemoTweakerEffect()
        {
            userInterface = new UserInterface();
            currentVariable = 0;
        }

        public void KeyUp()
        {
            currentVariable--;
            if (currentVariable == -1)
                currentVariable++;
        }

        public void KeyDown()
        {
            currentVariable++;
            if (currentVariable == currentContainer.GetNumTweakables())
                currentVariable--;
        }

        #region IDemoTweaker Members

        public bool Quit
        {
            get { return false; }
        }

        public void Initialize(IDemoRegistrator registrator)
        {
            this.registrator = registrator;

            userInterface.Initialize();

            CreateControls();
        }

        private void CreateControls()
        {
            mainWindow = new BoxControl(new RectangleF(0.05f, 0.05f, 0.90f, 0.90f), 0.0f, Color.Black, null);

            titleWindow = new BoxControl(new RectangleF(0.0f, 0.0f, 1.0f, 0.05f), alpha, titleColor, mainWindow);
            titleText = new TextControl("DDXX Tweaker", new RectangleF(0.0f, 0.0f, 1.0f, 1.0f), DrawTextFormat.Center | DrawTextFormat.VerticalCenter, textAlpha, Color.White, titleWindow);

            tweakableWindow = new BoxControl(new RectangleF(0.0f, 0.05f, 1.0f, 0.95f), alpha, timeColor, mainWindow);
        }

        public void Draw()
        {
            D3DDriver.GetInstance().GetDevice().BeginScene();
            userInterface.DrawControl(mainWindow);
            D3DDriver.GetInstance().GetDevice().EndScene();
        }

        public void HandleInput(IInputDriver inputDriver)
        {
            if (inputDriver.KeyPressedNoRepeat(Key.UpArrow))
                KeyUp();
            if (inputDriver.KeyPressedNoRepeat(Key.DownArrow))
                KeyDown();
        }

        public object IdentifierToChild()
        {
            return 0;
        }

        public void IdentifierFromParent(object id)
        {
            if ((ITweakableContainer)id == null)
                throw new DDXXException("Incorrect type received from parent.");
            CurrentContainer = (ITweakableContainer)id;
        }

        #endregion
    }
}
