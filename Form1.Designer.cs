using System.Drawing;
using System.Windows.Forms;

namespace SoundBoard
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            b_RegisterKey = new Button();
            l_registeringKey = new Label();
            trackBar = new TrackBar();
            cb_StopPrevSound = new CheckBox();
            b_Scan = new Button();
            listBox = new ListBox();
            b_Exit = new Button();
            l_Volume = new Label();
            b_DeleteConf = new Button();
            soundDevices_ComboBox = new ComboBox();
            l_SoundDev = new Label();
            ((System.ComponentModel.ISupportInitialize)trackBar).BeginInit();
            SuspendLayout();
            // 
            // b_RegisterKey
            // 
            b_RegisterKey.Location = new Point(12, 97);
            b_RegisterKey.Name = "b_RegisterKey";
            b_RegisterKey.Size = new Size(102, 23);
            b_RegisterKey.TabIndex = 13;
            b_RegisterKey.UseVisualStyleBackColor = true;
            b_RegisterKey.Click += b_RegisterKey_Click;
            b_RegisterKey.KeyDown += b_RegisterKey_KeyDown;
            // 
            // l_registeringKey
            // 
            l_registeringKey.AutoSize = true;
            l_registeringKey.Location = new Point(12, 77);
            l_registeringKey.Name = "l_registeringKey";
            l_registeringKey.Size = new Size(106, 17);
            l_registeringKey.TabIndex = 14;
            l_registeringKey.Text = "Registering Key: ";
            // 
            // trackBar
            // 
            trackBar.Location = new Point(12, 12);
            trackBar.Maximum = 100;
            trackBar.Name = "trackBar";
            trackBar.Size = new Size(194, 45);
            trackBar.TabIndex = 9;
            trackBar.TickFrequency = 10;
            trackBar.TickStyle = TickStyle.Both;
            trackBar.Value = 100;
            trackBar.Scroll += trackBar_Scroll;
            trackBar.MouseUp += trackBar_MouseUp;
            // 
            // cb_StopPrevSound
            // 
            cb_StopPrevSound.AutoSize = true;
            cb_StopPrevSound.Checked = true;
            cb_StopPrevSound.CheckState = CheckState.Checked;
            cb_StopPrevSound.Location = new Point(12, 174);
            cb_StopPrevSound.Name = "cb_StopPrevSound";
            cb_StopPrevSound.Size = new Size(102, 21);
            cb_StopPrevSound.TabIndex = 12;
            cb_StopPrevSound.Text = "Disable echo";
            cb_StopPrevSound.UseVisualStyleBackColor = true;
            // 
            // b_Scan
            // 
            b_Scan.Location = new Point(120, 97);
            b_Scan.Name = "b_Scan";
            b_Scan.Size = new Size(86, 23);
            b_Scan.TabIndex = 10;
            b_Scan.Text = "Scan";
            b_Scan.UseVisualStyleBackColor = true;
            b_Scan.Click += ScanForSounds;
            // 
            // listBox
            // 
            listBox.FormattingEnabled = true;
            listBox.HorizontalScrollbar = true;
            listBox.ItemHeight = 17;
            listBox.Location = new Point(212, 12);
            listBox.Name = "listBox";
            listBox.Size = new Size(385, 208);
            listBox.TabIndex = 15;
            listBox.MouseClick += listBox_MouseClick;
            // 
            // b_Exit
            // 
            b_Exit.Location = new Point(12, 201);
            b_Exit.Name = "b_Exit";
            b_Exit.Size = new Size(75, 24);
            b_Exit.TabIndex = 16;
            b_Exit.Text = "Exit";
            b_Exit.UseVisualStyleBackColor = true;
            b_Exit.Click += Exit;
            // 
            // l_Volume
            // 
            l_Volume.AutoSize = true;
            l_Volume.Location = new Point(12, 60);
            l_Volume.Name = "l_Volume";
            l_Volume.Size = new Size(58, 17);
            l_Volume.TabIndex = 17;
            l_Volume.Text = "Volume: ";
            // 
            // b_DeleteConf
            // 
            b_DeleteConf.Location = new Point(93, 200);
            b_DeleteConf.Name = "b_DeleteConf";
            b_DeleteConf.Size = new Size(113, 25);
            b_DeleteConf.TabIndex = 18;
            b_DeleteConf.Text = "Delete config";
            b_DeleteConf.UseVisualStyleBackColor = true;
            b_DeleteConf.Click += b_DeleteConf_Click;
            // 
            // soundDevices_ComboBox
            // 
            soundDevices_ComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            soundDevices_ComboBox.FormattingEnabled = true;
            soundDevices_ComboBox.Location = new Point(12, 143);
            soundDevices_ComboBox.Name = "soundDevices_ComboBox";
            soundDevices_ComboBox.Size = new Size(194, 25);
            soundDevices_ComboBox.TabIndex = 19;
            soundDevices_ComboBox.SelectedIndexChanged += soundDevices_ComboBox_SelectedIndexChanged;
            soundDevices_ComboBox.SelectedValueChanged += soundDevices_ComboBox_SelectedValueChanged;
            // 
            // l_SoundDev
            // 
            l_SoundDev.AutoSize = true;
            l_SoundDev.Location = new Point(12, 123);
            l_SoundDev.Name = "l_SoundDev";
            l_SoundDev.Size = new Size(102, 17);
            l_SoundDev.TabIndex = 20;
            l_SoundDev.Text = "Playback device:";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(609, 236);
            Controls.Add(l_SoundDev);
            Controls.Add(soundDevices_ComboBox);
            Controls.Add(b_DeleteConf);
            Controls.Add(l_Volume);
            Controls.Add(b_Exit);
            Controls.Add(listBox);
            Controls.Add(b_RegisterKey);
            Controls.Add(l_registeringKey);
            Controls.Add(trackBar);
            Controls.Add(cb_StopPrevSound);
            Controls.Add(b_Scan);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "Form1";
            Text = "Form1";
            FormClosing += Form1_FormClosing;
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)trackBar).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button b_RegisterKey;
        private Label l_registeringKey;
        private CheckBox cb_StopPrevSound;
        private Button b_Scan;
        private ListBox listBox;
        private Button b_Exit;
        private Label l_Volume;
        private Button b_DeleteConf;
        public TrackBar trackBar;
        private ComboBox soundDevices_ComboBox;
        private Label l_SoundDev;
    }
}