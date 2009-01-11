#if!(XBOX)
namespace Dope.DDXX.DemoFramework
{
    partial class SetupForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetupForm));
            this.resolution = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.radio_4_3 = new System.Windows.Forms.RadioButton();
            this.radio_16_9 = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radio_16_10 = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.hal = new System.Windows.Forms.RadioButton();
            this.reference = new System.Windows.Forms.RadioButton();
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
            this.multisampling = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.effectBar)).BeginInit();
            this.SuspendLayout();
            // 
            // resolution
            // 
            this.resolution.FormattingEnabled = true;
            this.resolution.Location = new System.Drawing.Point(15, 83);
            this.resolution.Name = "resolution";
            this.resolution.Size = new System.Drawing.Size(264, 21);
            this.resolution.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 67);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "a resolution of...";
            // 
            // radio_4_3
            // 
            this.radio_4_3.AutoSize = true;
            this.radio_4_3.Location = new System.Drawing.Point(6, 18);
            this.radio_4_3.Name = "radio_4_3";
            this.radio_4_3.Size = new System.Drawing.Size(40, 17);
            this.radio_4_3.TabIndex = 0;
            this.radio_4_3.TabStop = true;
            this.radio_4_3.Text = "4:3";
            this.radio_4_3.UseVisualStyleBackColor = true;
            this.radio_4_3.CheckedChanged += new System.EventHandler(this.radio_4_3_CheckedChanged);
            // 
            // radio_16_9
            // 
            this.radio_16_9.AutoSize = true;
            this.radio_16_9.Location = new System.Drawing.Point(97, 18);
            this.radio_16_9.Name = "radio_16_9";
            this.radio_16_9.Size = new System.Drawing.Size(46, 17);
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
            this.groupBox1.Location = new System.Drawing.Point(15, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(264, 41);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "give me an aspect ratio of...";
            // 
            // radio_16_10
            // 
            this.radio_16_10.AutoSize = true;
            this.radio_16_10.Location = new System.Drawing.Point(200, 16);
            this.radio_16_10.Name = "radio_16_10";
            this.radio_16_10.Size = new System.Drawing.Size(52, 17);
            this.radio_16_10.TabIndex = 2;
            this.radio_16_10.TabStop = true;
            this.radio_16_10.Text = "16:10";
            this.radio_16_10.UseVisualStyleBackColor = true;
            this.radio_16_10.CheckedChanged += new System.EventHandler(this.radio_16_10_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.hal);
            this.groupBox2.Controls.Add(this.reference);
            this.groupBox2.Location = new System.Drawing.Point(15, 121);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(264, 42);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "and use the following driver...";
            // 
            // hal
            // 
            this.hal.AutoSize = true;
            this.hal.Location = new System.Drawing.Point(6, 16);
            this.hal.Name = "hal";
            this.hal.Size = new System.Drawing.Size(46, 17);
            this.hal.TabIndex = 2;
            this.hal.TabStop = true;
            this.hal.Text = "HAL";
            this.hal.UseVisualStyleBackColor = true;
            // 
            // reference
            // 
            this.reference.AutoSize = true;
            this.reference.Location = new System.Drawing.Point(68, 16);
            this.reference.Name = "reference";
            this.reference.Size = new System.Drawing.Size(75, 17);
            this.reference.TabIndex = 1;
            this.reference.TabStop = true;
            this.reference.Text = "Reference";
            this.reference.UseVisualStyleBackColor = true;
            // 
            // ok
            // 
            this.ok.Location = new System.Drawing.Point(21, 344);
            this.ok.Name = "ok";
            this.ok.Size = new System.Drawing.Size(90, 29);
            this.ok.TabIndex = 0;
            this.ok.Text = "Experience";
            this.ok.UseVisualStyleBackColor = true;
            this.ok.Click += new System.EventHandler(this.ok_Click);
            // 
            // cancel
            // 
            this.cancel.Location = new System.Drawing.Point(180, 344);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(87, 29);
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
            this.groupBox3.Location = new System.Drawing.Point(14, 183);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(264, 87);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "and give me the following level of postEffect...";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(172, 69);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(81, 9);
            this.label6.TabIndex = 5;
            this.label6.Text = "(Fast Shader Model 3)";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(4, 69);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(83, 9);
            this.label5.TabIndex = 4;
            this.label5.Text = "(Slow Shader Model 2)";
            // 
            // effect3
            // 
            this.effect3.AutoSize = true;
            this.effect3.Location = new System.Drawing.Point(163, 54);
            this.effect3.Name = "effect3";
            this.effect3.Size = new System.Drawing.Size(90, 13);
            this.effect3.TabIndex = 3;
            this.effect3.Text = "Hit me hard baby!";
            // 
            // effect2
            // 
            this.effect2.AutoSize = true;
            this.effect2.Location = new System.Drawing.Point(110, 53);
            this.effect2.Name = "effect2";
            this.effect2.Size = new System.Drawing.Size(47, 13);
            this.effect2.TabIndex = 2;
            this.effect2.Text = "Average";
            // 
            // effect1
            // 
            this.effect1.AutoSize = true;
            this.effect1.Location = new System.Drawing.Point(4, 54);
            this.effect1.Name = "effect1";
            this.effect1.Size = new System.Drawing.Size(55, 13);
            this.effect1.TabIndex = 1;
            this.effect1.Text = "Spare me!";
            // 
            // effectBar
            // 
            this.effectBar.LargeChange = 1;
            this.effectBar.Location = new System.Drawing.Point(7, 23);
            this.effectBar.Maximum = 2;
            this.effectBar.Name = "effectBar";
            this.effectBar.Size = new System.Drawing.Size(246, 45);
            this.effectBar.TabIndex = 0;
            // 
            // windowed
            // 
            this.windowed.AutoSize = true;
            this.windowed.Checked = true;
            this.windowed.CheckState = System.Windows.Forms.CheckState.Checked;
            this.windowed.Location = new System.Drawing.Point(52, 285);
            this.windowed.Name = "windowed";
            this.windowed.Size = new System.Drawing.Size(194, 17);
            this.windowed.TabIndex = 6;
            this.windowed.Text = "I am too afraid to run in full screen...";
            this.windowed.UseVisualStyleBackColor = true;
            // 
            // multisampling
            // 
            this.multisampling.AutoSize = true;
            this.multisampling.Checked = true;
            this.multisampling.CheckState = System.Windows.Forms.CheckState.Checked;
            this.multisampling.Location = new System.Drawing.Point(52, 308);
            this.multisampling.Name = "multisampling";
            this.multisampling.Size = new System.Drawing.Size(165, 17);
            this.multisampling.TabIndex = 7;
            this.multisampling.Text = "Yes! Multisample is da thing...";
            this.multisampling.UseVisualStyleBackColor = true;
            // 
            // SetupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ClientSize = new System.Drawing.Size(290, 395);
            this.Controls.Add(this.multisampling);
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
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SetupForm";
            this.Text = "a Dope production -- setup";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.SetupForm_Load);
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
        private System.Windows.Forms.RadioButton hal;
        private System.Windows.Forms.RadioButton reference;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label effect3;
        private System.Windows.Forms.Label effect2;
        private System.Windows.Forms.Label effect1;
        private System.Windows.Forms.TrackBar effectBar;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox windowed;
        private System.Windows.Forms.CheckBox multisampling;
    }
}
#endif