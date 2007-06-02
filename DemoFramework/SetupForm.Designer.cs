namespace Dope.DDXX.DemoFramework
{
    partial class SetupDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetupDialog));
            this.resolution = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.radio_4_3 = new System.Windows.Forms.RadioButton();
            this.radio_16_9 = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radio_16_10 = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.bit16fp = new System.Windows.Forms.RadioButton();
            this.bit16 = new System.Windows.Forms.RadioButton();
            this.bit32 = new System.Windows.Forms.RadioButton();
            this.bit32fp = new System.Windows.Forms.RadioButton();
            this.ok = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.effect3 = new System.Windows.Forms.Label();
            this.effect2 = new System.Windows.Forms.Label();
            this.effect1 = new System.Windows.Forms.Label();
            this.effectBar = new System.Windows.Forms.TrackBar();
            this.windowed = new System.Windows.Forms.CheckBox();
            this.refDriver = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.effectBar)).BeginInit();
            this.SuspendLayout();
            // 
            // resolution
            // 
            this.resolution.FormattingEnabled = true;
            this.resolution.Location = new System.Drawing.Point(20, 102);
            this.resolution.Margin = new System.Windows.Forms.Padding(4);
            this.resolution.Name = "resolution";
            this.resolution.Size = new System.Drawing.Size(351, 24);
            this.resolution.TabIndex = 3;
            this.resolution.SelectedIndexChanged += new System.EventHandler(this.resolution_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 82);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(110, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "a resolution of...";
            // 
            // radio_4_3
            // 
            this.radio_4_3.AutoSize = true;
            this.radio_4_3.Location = new System.Drawing.Point(8, 22);
            this.radio_4_3.Margin = new System.Windows.Forms.Padding(4);
            this.radio_4_3.Name = "radio_4_3";
            this.radio_4_3.Size = new System.Drawing.Size(49, 21);
            this.radio_4_3.TabIndex = 0;
            this.radio_4_3.TabStop = true;
            this.radio_4_3.Text = "4:3";
            this.radio_4_3.UseVisualStyleBackColor = true;
            this.radio_4_3.CheckedChanged += new System.EventHandler(this.radio_4_3_CheckedChanged);
            // 
            // radio_16_9
            // 
            this.radio_16_9.AutoSize = true;
            this.radio_16_9.Location = new System.Drawing.Point(129, 22);
            this.radio_16_9.Margin = new System.Windows.Forms.Padding(4);
            this.radio_16_9.Name = "radio_16_9";
            this.radio_16_9.Size = new System.Drawing.Size(57, 21);
            this.radio_16_9.TabIndex = 1;
            this.radio_16_9.TabStop = true;
            this.radio_16_9.Text = "16:9";
            this.radio_16_9.UseVisualStyleBackColor = true;
            this.radio_16_9.CheckedChanged += new System.EventHandler(this.radio_16_9_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radio_16_10);
            this.groupBox1.Controls.Add(this.radio_16_9);
            this.groupBox1.Controls.Add(this.radio_4_3);
            this.groupBox1.Location = new System.Drawing.Point(20, 15);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(352, 50);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "give me an aspect ratio of...";
            // 
            // radio_16_10
            // 
            this.radio_16_10.AutoSize = true;
            this.radio_16_10.Location = new System.Drawing.Point(267, 20);
            this.radio_16_10.Margin = new System.Windows.Forms.Padding(4);
            this.radio_16_10.Name = "radio_16_10";
            this.radio_16_10.Size = new System.Drawing.Size(65, 21);
            this.radio_16_10.TabIndex = 2;
            this.radio_16_10.TabStop = true;
            this.radio_16_10.Text = "16:10";
            this.radio_16_10.UseVisualStyleBackColor = true;
            this.radio_16_10.CheckedChanged += new System.EventHandler(this.radio_16_10_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.bit16fp);
            this.groupBox2.Controls.Add(this.bit16);
            this.groupBox2.Controls.Add(this.bit32);
            this.groupBox2.Controls.Add(this.bit32fp);
            this.groupBox2.Location = new System.Drawing.Point(20, 149);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox2.Size = new System.Drawing.Size(352, 52);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "and let the boxColor depth be...";
            // 
            // bit16fp
            // 
            this.bit16fp.AutoSize = true;
            this.bit16fp.Location = new System.Drawing.Point(175, 20);
            this.bit16fp.Margin = new System.Windows.Forms.Padding(4);
            this.bit16fp.Name = "bit16fp";
            this.bit16fp.Size = new System.Drawing.Size(66, 21);
            this.bit16fp.TabIndex = 3;
            this.bit16fp.TabStop = true;
            this.bit16fp.Text = "16 FP";
            this.bit16fp.UseVisualStyleBackColor = true;
            // 
            // bit16
            // 
            this.bit16.AutoSize = true;
            this.bit16.Location = new System.Drawing.Point(8, 20);
            this.bit16.Margin = new System.Windows.Forms.Padding(4);
            this.bit16.Name = "bit16";
            this.bit16.Size = new System.Drawing.Size(45, 21);
            this.bit16.TabIndex = 2;
            this.bit16.TabStop = true;
            this.bit16.Text = "16";
            this.bit16.UseVisualStyleBackColor = true;
            this.bit16.CheckedChanged += new System.EventHandler(this.bit16_CheckedChanged);
            // 
            // bit32
            // 
            this.bit32.AutoSize = true;
            this.bit32.Location = new System.Drawing.Point(91, 20);
            this.bit32.Margin = new System.Windows.Forms.Padding(4);
            this.bit32.Name = "bit32";
            this.bit32.Size = new System.Drawing.Size(45, 21);
            this.bit32.TabIndex = 1;
            this.bit32.TabStop = true;
            this.bit32.Text = "32";
            this.bit32.UseVisualStyleBackColor = true;
            // 
            // bit32fp
            // 
            this.bit32fp.AutoSize = true;
            this.bit32fp.Location = new System.Drawing.Point(267, 20);
            this.bit32fp.Margin = new System.Windows.Forms.Padding(4);
            this.bit32fp.Name = "bit32fp";
            this.bit32fp.Size = new System.Drawing.Size(66, 21);
            this.bit32fp.TabIndex = 0;
            this.bit32fp.TabStop = true;
            this.bit32fp.Text = "32 FP";
            this.bit32fp.UseVisualStyleBackColor = true;
            // 
            // ok
            // 
            this.ok.Location = new System.Drawing.Point(28, 423);
            this.ok.Margin = new System.Windows.Forms.Padding(4);
            this.ok.Name = "ok";
            this.ok.Size = new System.Drawing.Size(120, 36);
            this.ok.TabIndex = 0;
            this.ok.Text = "Experience";
            this.ok.UseVisualStyleBackColor = true;
            this.ok.Click += new System.EventHandler(this.ok_Click);
            // 
            // cancel
            // 
            this.cancel.Location = new System.Drawing.Point(240, 423);
            this.cancel.Margin = new System.Windows.Forms.Padding(4);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(116, 36);
            this.cancel.TabIndex = 1;
            this.cancel.Text = "Chicken Out";
            this.cancel.UseVisualStyleBackColor = true;
            this.cancel.Click += new System.EventHandler(this.cancel_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.effect3);
            this.groupBox3.Controls.Add(this.effect2);
            this.groupBox3.Controls.Add(this.effect1);
            this.groupBox3.Controls.Add(this.effectBar);
            this.groupBox3.Location = new System.Drawing.Point(19, 225);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox3.Size = new System.Drawing.Size(352, 107);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "and give me the following level of postEffect...";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(229, 85);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(111, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "(Fast Shader Model 3)";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(5, 85);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(114, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "(Slow Shader Model 2)";
            // 
            // effect3
            // 
            this.effect3.AutoSize = true;
            this.effect3.Location = new System.Drawing.Point(217, 66);
            this.effect3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.effect3.Name = "effect3";
            this.effect3.Size = new System.Drawing.Size(119, 17);
            this.effect3.TabIndex = 3;
            this.effect3.Text = "Hit me hard baby!";
            // 
            // effect2
            // 
            this.effect2.AutoSize = true;
            this.effect2.Location = new System.Drawing.Point(147, 65);
            this.effect2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.effect2.Name = "effect2";
            this.effect2.Size = new System.Drawing.Size(61, 17);
            this.effect2.TabIndex = 2;
            this.effect2.Text = "Average";
            // 
            // effect1
            // 
            this.effect1.AutoSize = true;
            this.effect1.Location = new System.Drawing.Point(5, 66);
            this.effect1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.effect1.Name = "effect1";
            this.effect1.Size = new System.Drawing.Size(72, 17);
            this.effect1.TabIndex = 1;
            this.effect1.Text = "Spare me!";
            // 
            // effectBar
            // 
            this.effectBar.LargeChange = 1;
            this.effectBar.Location = new System.Drawing.Point(9, 28);
            this.effectBar.Margin = new System.Windows.Forms.Padding(4);
            this.effectBar.Maximum = 2;
            this.effectBar.Name = "effectBar";
            this.effectBar.Size = new System.Drawing.Size(328, 56);
            this.effectBar.TabIndex = 0;
            // 
            // windowed
            // 
            this.windowed.AutoSize = true;
            this.windowed.Checked = true;
            this.windowed.CheckState = System.Windows.Forms.CheckState.Checked;
            this.windowed.Location = new System.Drawing.Point(69, 351);
            this.windowed.Margin = new System.Windows.Forms.Padding(4);
            this.windowed.Name = "windowed";
            this.windowed.Size = new System.Drawing.Size(257, 21);
            this.windowed.TabIndex = 6;
            this.windowed.Text = "I am too afraid to run in full screen...";
            this.windowed.UseVisualStyleBackColor = true;
            this.windowed.CheckedChanged += new System.EventHandler(this.windowed_CheckedChanged);
            // 
            // refDriver
            // 
            this.refDriver.AutoSize = true;
            this.refDriver.Location = new System.Drawing.Point(69, 379);
            this.refDriver.Margin = new System.Windows.Forms.Padding(4);
            this.refDriver.Name = "refDriver";
            this.refDriver.Size = new System.Drawing.Size(184, 21);
            this.refDriver.TabIndex = 7;
            this.refDriver.Text = "I only fancy REF driver...";
            this.refDriver.UseVisualStyleBackColor = true;
            // 
            // SetupDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ClientSize = new System.Drawing.Size(387, 486);
            this.Controls.Add(this.refDriver);
            this.Controls.Add(this.windowed);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.ok);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.resolution);
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SetupDialog";
            this.Text = "a Dope production -- setup";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.SetupDialog_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.effectBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox resolution;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton radio_4_3;
        private System.Windows.Forms.RadioButton radio_16_9;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radio_16_10;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button ok;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.RadioButton bit16;
        private System.Windows.Forms.RadioButton bit32;
        private System.Windows.Forms.RadioButton bit32fp;
        private System.Windows.Forms.RadioButton bit16fp;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label effect3;
        private System.Windows.Forms.Label effect2;
        private System.Windows.Forms.Label effect1;
        private System.Windows.Forms.TrackBar effectBar;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox windowed;
        private System.Windows.Forms.CheckBox refDriver;
    }
}