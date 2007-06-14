using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;
using Dope.DDXX.Graphics;
using Dope.DDXX.Utility;
using System.Collections;

namespace Dope.DDXX.DemoFramework
{
    public class SetupLogic
    {
        private ISetupDialog dialog;
        private D3DDriver driver;
        private bool ok;

        public SetupLogic()
        {
            driver = D3DDriver.GetInstance();
        }

        public bool OK
        {
            get { return ok; }
            set { ok = value; }
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
                                    if (dialog.Checked16Bit)
                                        return true;
                                    break;
                                case Format.A8B8G8R8:
                                case Format.X8B8G8R8:
                                case Format.X8R8G8B8:
                                case Format.A8R8G8B8:
                                case Format.A2R10G10B10:
                                    if (dialog.Checked32Bit)
                                        return true;
                                    break;
                            }
                        }
                        return false;
                    });
                if (modes.Length == 0)
                    return Format.Unknown;
                return modes[0].Format;
            }
        }

        public DeviceDescription DeviceDescription
        {
            get
            {
                DeviceDescription desc = new DeviceDescription();
                if (dialog.REF)
                    desc.deviceType = DeviceType.Reference;
                else
                    desc.deviceType = DeviceType.Hardware;
                desc.windowed = dialog.Windowed;
                desc.colorFormat = ColorFormat;
                desc.width = ResolutionWidth;
                desc.height = ResolutionHeight;
                desc.useDepth = true;
                return desc;
            }
        }

        public void UpdateResolution(AspectRatio.Ratios ratio)
        {
            int i;
            DisplayMode[] modes = GetDisplayModes(ratio);
            Array.Sort(modes, CompareDM);

            string[] values = new string[modes.Length];
            int n = 0;
            foreach (DisplayMode m in modes)
            {
                values[n++] = m.Width + "x" + m.Height;
            }

            ArrayList noDups = new ArrayList();
            for (i = 0; i < values.Length; i++)
            {
                if (!noDups.Contains(values[i].Trim()))
                {
                    noDups.Add(values[i].Trim());
                }
            }
            string[] uniqueValues = new string[noDups.Count];
            noDups.CopyTo(uniqueValues);
            dialog.Resolution = uniqueValues;
        }

        public DisplayMode[] GetDisplayModes(AspectRatio.Ratios ratio)
        {
            return driver.GetDisplayModes(0, delegate(DisplayMode mode) { if (new AspectRatio(mode.Width, mode.Height).Ratio == ratio) return true; else return false; });
        }
        
        private static int CompareDM(DisplayMode m1, DisplayMode m2)
        {
            if (m1.Height == m2.Height)
            {
                if (m1.Width == m2.Width)
                    return 0;
                else if (m1.Width > m2.Width)
                    return 1;
                else
                    return -1;
            }
            else if (m1.Height > m2.Height)
                return 1;
            else
                return -1;
        }

        private static bool EqualsDM(DisplayMode m1, DisplayMode m2)
        {
            return m1.Height == m2.Height && m1.Width == m2.Width;
        }


        public void ResolutionChanged()
        {
            string[] t = dialog.SelectedResolution.Split('x');
            DisplayMode[] modes = driver.GetDisplayModes(0, delegate(DisplayMode mode) { if (mode.Width.ToString() == t[0] && mode.Height.ToString() == t[1]) return true; else return false; });
            dialog.Enable16Bit = false;
            dialog.Enable32Bit = false;
            foreach (DisplayMode m in modes)
            {
                switch (m.Format)
                {
                    case Format.R5G6B5:
                        dialog.Enable16Bit = true;
                        break;
                    case Format.A8B8G8R8:
                    case Format.X8B8G8R8:
                    case Format.X8R8G8B8:
                    case Format.A8R8G8B8:
                    case Format.A2R10G10B10:
                        dialog.Enable32Bit = true;
                        break;
                    default:
                        //MessageBox.Show("Unknown format : " + m.Format.ToString());
                        break;
                }
            }

            if (dialog.Enable32Bit)
                dialog.Checked32Bit = true;
            else
                dialog.Checked16Bit = true;
        }

        public void Initialize()
        {
            DisplayMode[] modes;

            modes = GetDisplayModes(AspectRatio.Ratios.RATIO_4_3);
            if (modes.Length == 0)
                dialog.EnableRadio4_3 = false;

            modes = GetDisplayModes(AspectRatio.Ratios.RATIO_16_9);
            if (modes.Length == 0)
                dialog.EnableRadio16_9 = false;

            modes = GetDisplayModes(AspectRatio.Ratios.RATIO_16_10);
            if (modes.Length == 0)
                dialog.EnableRadio16_10 = false;

            dialog.CheckedRadio3_4 = true;

        }
    }
}
