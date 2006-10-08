using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Dope.DDXX.Graphics;
using Dope.DDXX.Utility;

namespace Dope.DDXX.DemoFramework
{
    public partial class SetupDialog : Form
    {
        D3DDriver driver;
        bool start;

        public SetupDialog()
        {
            driver = D3DDriver.GetInstance();
            InitializeComponent();
        }

        enum ColorDepth
        {
            COLOR_DEPTH_16 = 0,
            COLOR_DEPTH_32,
            COLOR_DEPTH_16F,
            COLOR_DEPTH_32F,
            NUM_COLOR_DEPTHS
        }

        public bool REF
        {
            get { return refDriver.Checked; }
        }

        public bool OK
        {
            get { return start; }
        }

        public bool Windowed
        {
            get { return windowed.Checked; }
        }

        public int ResolutionWidth
        {
            get { return Int32.Parse(((string)resolution.SelectedItem).Split('x')[0]); }
        }

        public int ResolutionHeight
        {
            get { return Int32.Parse(((string)resolution.SelectedItem).Split('x')[1]); }
        }

        public Format ColorFormat
        {
            get
            {
                DisplayMode[] modes = driver.GetDisplayModes(0, 
                    delegate(DisplayMode mode) {
                        if (mode.Width == ResolutionWidth && mode.Height == ResolutionHeight)
                        {
                            switch (mode.Format)
                            {
                                case Format.R5G6B5:
                                    if (bit16.Checked)
                                        return true;
                                    break;
                                case Format.A8B8G8R8:
                                case Format.X8B8G8R8:
                                case Format.X8R8G8B8:
                                case Format.A8R8G8B8:
                                case Format.A2R10G10B10:
                                    if (bit32.Checked)
                                        return true;
                                    break;
                                case Format.A16B16G16R16F:
                                    if (bit16fp.Checked)
                                        return true;
                                    break;
                                case Format.A32B32G32R32F:
                                    if (bit32fp.Checked)
                                        return true;
                                    break;
                            }
                        }
                        return false;
                    });
                return modes[0].Format;
            }
        }

        public DeviceDescription DeviceDescription
        {
            get
            {
                DeviceDescription desc = new DeviceDescription();
                if (REF)
                    desc.deviceType = DeviceType.Reference;
                else
                    desc.deviceType = DeviceType.Hardware;
                desc.windowed = Windowed;
                desc.colorFormat = ColorFormat;
                desc.width = ResolutionWidth;
                desc.height = ResolutionHeight;
                desc.useDepth = true;
                return desc;
            }
        }

        private void UpdateResolution(RadioButton button, AspectRatio.Ratios ratio)
        {
            if (button.Checked)
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

                resolution.Items.Clear();
                resolution.Items.AddRange(uniqueValues);
                resolution.SelectedIndex = 0;
                i = 0;
                foreach (string s in uniqueValues)
                {
                    if (s.Length < 8)
                        resolution.SelectedIndex = i;
                    i++;
                }
            }
        }

        private DisplayMode[] GetDisplayModes(AspectRatio.Ratios ratio)
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

        private void ok_Click(object sender, EventArgs e)
        {
            start = true;
            Close();
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void SetupDialog_Load(object sender, EventArgs e)
        {
            DisplayMode[] modes;

            modes = GetDisplayModes(AspectRatio.Ratios.RATIO_4_3);
            if (modes.Length == 0)
                radio_4_3.Enabled = false;

            modes = GetDisplayModes(AspectRatio.Ratios.RATIO_16_9);
            if (modes.Length == 0)
                radio_16_9.Enabled = false;

            modes = GetDisplayModes(AspectRatio.Ratios.RATIO_16_10);
            if (modes.Length == 0)
                radio_16_10.Enabled = false;

            radio_4_3.Checked = true;

            // Set different colors depending on computer speed
            effect1.ForeColor = Color.Firebrick;
            effect2.ForeColor = Color.DarkGreen;
            effect3.ForeColor = Color.Firebrick;
            effectBar.Value = 1;
        }

        private void radio_4_3_CheckedChanged(object sender, EventArgs e)
        {
            UpdateResolution((RadioButton)sender, AspectRatio.Ratios.RATIO_4_3);
        }

        private void radio_16_9_CheckedChanged(object sender, EventArgs e)
        {
            UpdateResolution((RadioButton)sender, AspectRatio.Ratios.RATIO_16_9);
        }

        private void radio_16_10_CheckedChanged(object sender, EventArgs e)
        {
            UpdateResolution((RadioButton)sender, AspectRatio.Ratios.RATIO_16_10);
        }

        private void resolution_SelectedIndexChanged(object sender, EventArgs e)
        {
            string[] t = ((string)resolution.SelectedItem).Split('x');
            DisplayMode[] modes = driver.GetDisplayModes(0, delegate(DisplayMode mode) { if (mode.Width.ToString() == t[0] && mode.Height.ToString() == t[1]) return true; else return false; });
            bit16.Enabled = false;
            bit32.Enabled = false;
            bit16fp.Enabled = false;
            bit32fp.Enabled = false;
            foreach (DisplayMode m in modes)
            {
                switch (m.Format)
                {
                    case Format.R5G6B5:
                        bit16.Enabled = true;
                        break;
                    case Format.A8B8G8R8:
                    case Format.X8B8G8R8:
                    case Format.X8R8G8B8:
                    case Format.A8R8G8B8:
                    case Format.A2R10G10B10:
                        bit32.Enabled = true;
                        break;
                    case Format.A16B16G16R16F:
                        bit16fp.Enabled = true;
                        break;
                    case Format.A32B32G32R32F:
                        bit32fp.Enabled = true;
                        break;
                    default:
                        //MessageBox.Show("Unknown format : " + m.Format.ToString());
                        break;
                }
            }

            if (bit32.Enabled)
                bit32.Checked = true;
            else
                bit16.Checked = true;
        }

        private void bit16_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void windowed_CheckedChanged(object sender, EventArgs e)
        {

        }

    }
}