using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;
using Dope.DDXX.Utility;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.DemoFramework
{
    public class SetupLogic
    {
        private ISetupDialog dialog;
        private bool ok;

        public SetupLogic()
        {
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

        public DeviceParameters DeviceParameters
        {
            get { return new DeviceParameters(ResolutionWidth, ResolutionHeight, !dialog.Windowed, dialog.Reference, dialog.Multisampling, false); }
        }

        public int ResolutionWidth
        {
            get { return Int32.Parse(dialog.SelectedResolution.Split('x')[0]); }
        }

        public int ResolutionHeight
        {
            get { return Int32.Parse(dialog.SelectedResolution.Split('x')[1]); }
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
            List<DisplayMode> list = new List<DisplayMode>();
            foreach (DisplayMode mode in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes)
            {
                if (new AspectRatio(mode.Width, mode.Height).Ratio == ratio)
                {
                    list.Add(mode);
                }
            }
            return list.ToArray();
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

        public void Initialize()
        {
            DisplayMode[] modes;

            modes = GetDisplayModes(AspectRatio.Ratios.RATIO_4_3);
            if (modes.Length == 0)
                dialog.EnableRadio4_3 = false;
            else
                dialog.CheckedRadio4_3 = true;

            modes = GetDisplayModes(AspectRatio.Ratios.RATIO_16_10);
            if (modes.Length == 0)
                dialog.EnableRadio16_10 = false;
            else
                dialog.CheckedRadio16_10 = true;

            modes = GetDisplayModes(AspectRatio.Ratios.RATIO_16_9);
            if (modes.Length == 0)
                dialog.EnableRadio16_9 = false;
            else
                dialog.CheckedRadio16_9 = true;

            dialog.Reference = false;
            dialog.Windowed = true;
        }
    }
}
