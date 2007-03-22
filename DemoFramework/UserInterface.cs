using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;
using System.Drawing;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;

namespace Dope.DDXX.DemoFramework
{
    public class UserInterface : IUserInterface
    {
        private ILine line;
        private ISprite sprite;
        private ITexture whiteTexture;
        private IFont font;

        public UserInterface()
        {
        }

        public void Initialize()
        {
            IDevice device = D3DDriver.GetInstance().Device;

            line = D3DDriver.GraphicsFactory.CreateLine(device);
            line.Width = 1.0f;
            line.Antialias = false;
            
            sprite = D3DDriver.GraphicsFactory.CreateSprite(device);

            font = D3DDriver.GraphicsFactory.CreateFont(device, 16,  0, FontWeight.Bold,  1, false,  CharacterSet.Default, Precision.Default, FontQuality.ClearType, PitchAndFamily.DefaultPitch | PitchAndFamily.FamilyDoNotCare, "Times New Roman");
            
            whiteTexture = D3DDriver.GraphicsFactory.CreateTexture(device, 1, 1, 1, Usage.RenderTarget, Format.X8R8G8B8, Pool.Default);
            device.ColorFill(whiteTexture.GetSurfaceLevel(0), new Rectangle(0, 0, 1, 1), Color.White);
        }

        public void DrawControl(BaseControl control)
        {
            control.DrawControl(sprite, line, font, whiteTexture);
        }

    }
}
