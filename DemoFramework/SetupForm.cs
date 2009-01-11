#if !(XBOX)
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Dope.DDXX.Graphics;
using Dope.DDXX.Utility;

namespace Dope.DDXX.DemoFramework
{
    public partial class SetupForm : Form, ISetupDialog
    {
        private SetupLogic logic;

        public SetupForm(SetupLogic logic)
        {
            logic.Dialog = this;
            this.logic = logic;
            InitializeComponent();
        }

        public bool Reference
        {
            get { return reference.Checked; }
            set { reference.Checked = value; hal.Checked = !value; }
        }

        public bool Windowed
        {
            get { return windowed.Checked; }
            set { windowed.Checked = value; }
        }

        public bool Multisampling
        {
            get { return multisampling.Checked; }
            set { multisampling.Checked = value; }
        }

        public string SelectedResolution 
        {
            get { return (string)resolution.SelectedItem; }  
        }

        public bool EnableRadio4_3 { set { radio_4_3.Enabled = value; } }
        public bool EnableRadio16_9 { set { radio_16_9.Enabled = value; } }
        public bool EnableRadio16_10 { set { radio_16_10.Enabled = value; } }
        public bool CheckedRadio4_3 { set { radio_4_3.Checked = value; } }
        public bool CheckedRadio16_9 { set { radio_16_9.Checked = value; } }
        public bool CheckedRadio16_10 { set { radio_16_10.Checked = value; } }

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

        private void SetupForm_Load(object sender, EventArgs e)
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

    }
}
#endif