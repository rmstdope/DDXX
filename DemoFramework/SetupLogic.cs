using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;
using Dope.DDXX.Graphics;

namespace Dope.DDXX.DemoFramework
{
    public class SetupLogic
    {
        private ISetupDialog dialog;
        private D3DDriver driver;

        public SetupLogic()
        {
            driver = D3DDriver.GetInstance();
        }

        public ISetupDialog Dialog
        {
            set { dialog = value; }
        }

        public int ResolutionWidth
        {
            get { return Int32.Parse(dialog.SelectedResolution.Split('x')[0]); }
        }

        public int ResolutionHeight
        {
            get { return Int32.Parse(dialog.SelectedResolution.Split('x')[1]); }
        }

        public Format ColorFormat
        {
            get
            {
                DisplayMode[] modes = driver.GetDisplayModes(0,
                    delegate(DisplayMode mode)
                    {
                        if (mode.Width == ResolutionWidth && mode.Height == ResolutionHeight)
                        {
                            switch (mode.Format)
                            {
                                case Format.R5G6B5:
                                    if (dialog.Bit16)
                                        return true;
                                    break;
                                case Format.A8B8G8R8:
                                case Format.X8B8G8R8:
                                case Format.X8R8G8B8:
                                case Format.A8R8G8B8:
                                case Format.A2R10G10B10:
                                    if (dialog.Bit32)
                                        return true;
                                    break;
                                //case Format.A16B16G16R16F:
                                //    if (dialog.Bit16FP)
                                //        return true;
                                //    break;
                                //case Format.A32B32G32R32F:
                                //    if (dialog.Bit32FP)
                                //        return true;
                                //    break;
                            }
                        }
                        return false;
                    });
                if (modes.Length == 0)
                    return Format.Unknown;
                return modes[0].Format;
            }
        }


    }
}
