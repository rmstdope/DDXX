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
    public partial class SetupDialog : Form, ISetupDialog
    {
        private SetupLogic logic;

        public SetupDialog(SetupLogic logic)
        {
            logic.Dialog = this;
            this.logic = logic;
            InitializeComponent();

            bit16fp.Enabled = false;
            bit32fp.Enabled = false;
        }

        public bool REF
        {
            get { return refDriver.Checked; }
        }

        public bool Windowed
        {
            get { return windowed.Checked; }
        }

        public string SelectedResolution 
        {
            get { return (string)resolution.SelectedItem; }  
        }

        public bool Checked16Bit 
        {
            set { bit16.Checked = value; } 
            get { return bit16.Checked; }
        }
        public bool Checked32Bit 
        {
            set { bit32.Checked = value; }
            get { return bit32.Checked; } 
        }
        public bool Enable16Bit 
        {
            set { bit16.Enabled = value; }
            get { return bit16.Enabled; } 
}
        public bool Enable32Bit 
        {
            set { bit32.Enabled = value; }
            get { return bit32.Enabled; } 
        }

        public bool EnableRadio4_3 { set { radio_4_3.Enabled = value; } }
        public bool EnableRadio16_9 { set { radio_16_9.Enabled = value; } }
        public bool EnableRadio16_10 { set { radio_16_10.Enabled = value; } }
        public bool CheckedRadio3_4 { set { radio_4_3.Checked = value; } }

        public string[] Resolution 
        {
            set
            {
                resolution.Items.Clear();
                resolution.Items.AddRange(value);
                resolution.SelectedIndex = 0;
                int i = 0;
                foreach (string s in value)
                {
                    if (s.Length < 8)
                        resolution.SelectedIndex = i;
                    i++;
                }
            }
        }

        private void ok_Click(object sender, EventArgs e)
        {
            logic.OK = true;
            Close();
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void SetupDialog_Load(object sender, EventArgs e)
        {
            logic.Initialize();

            // Set different colors depending on computer speed
            effect1.ForeColor = Color.Firebrick;
            effect2.ForeColor = Color.DarkGreen;
            effect3.ForeColor = Color.Firebrick;
            effectBar.Value = 1;
        }

        private void radio_4_3_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
                logic.UpdateResolution(AspectRatio.Ratios.RATIO_4_3);
        }

        private void radio_16_9_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
                logic.UpdateResolution(AspectRatio.Ratios.RATIO_16_9);
        }

        private void radio_16_10_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
                logic.UpdateResolution(AspectRatio.Ratios.RATIO_16_10);
        }

        private void resolution_SelectedIndexChanged(object sender, EventArgs e)
        {
            logic.ResolutionChanged();
        }

        private void bit16_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void windowed_CheckedChanged(object sender, EventArgs e)
        {
        }

    }
}