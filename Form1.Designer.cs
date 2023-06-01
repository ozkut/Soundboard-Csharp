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
            cb_hearPlayedSound = new CheckBox();
            l_Sounds = new Label();
            ((System.ComponentModel.ISupportInitialize)trackBar).BeginInit();
            SuspendLayout();
            // 
            // b_RegisterKey
            // 
            b_RegisterKey.Location = new Point(12, 134);
            b_RegisterKey.Name = "b_RegisterKey";
            b_RegisterKey.Size = new Size(160, 25);
            b_RegisterKey.TabIndex = 13;
            b_RegisterKey.UseVisualStyleBackColor = true;
            b_RegisterKey.Click += b_RegisterKey_Click;
            b_RegisterKey.KeyDown += b_RegisterKey_KeyDown;
            // 
            // l_registeringKey
            // 
            l_registeringKey.AutoSize = true;
            l_registeringKey.Location = new Point(12, 114);
            l_registeringKey.Name = "l_registeringKey";
            l_registeringKey.Size = new Size(106, 17);
            l_registeringKey.TabIndex = 14;
            l_registeringKey.Text = "Registering Key: ";
            // 
            // trackBar
            // 
            trackBar.Location = new Point(12, 66);
            trackBar.Maximum = 100;
            trackBar.Name = "trackBar";
            trackBar.Size = new Size(162, 45);
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
            cb_StopPrevSound.Location = new Point(12, 165);
            cb_StopPrevSound.Name = "cb_StopPrevSound";
            cb_StopPrevSound.Size = new Size(102, 21);
            cb_StopPrevSound.TabIndex = 12;
            cb_StopPrevSound.Text = "Disable echo";
            cb_StopPrevSound.UseVisualStyleBackColor = true;
            // 
            // b_Scan
            // 
            b_Scan.Location = new Point(12, 219);
            b_Scan.Name = "b_Scan";
            b_Scan.Size = new Size(160, 37);
            b_Scan.TabIndex = 10;
            b_Scan.Text = "Scan sounds folder";
            b_Scan.UseVisualStyleBackColor = true;
            b_Scan.Click += ScanForSounds;
            // 
            // listBox
            // 
            listBox.FormattingEnabled = true;
            listBox.HorizontalScrollbar = true;
            listBox.ItemHeight = 17;
            listBox.Location = new Point(178, 46);
            listBox.Name = "listBox";
            listBox.Size = new Size(331, 242);
            listBox.TabIndex = 15;
            listBox.MouseClick += listBox_MouseClick;
            // 
            // b_Exit
            // 
            b_Exit.Location = new Point(12, 262);
            b_Exit.Name = "b_Exit";
            b_Exit.Size = new Size(58, 26);
            b_Exit.TabIndex = 16;
            b_Exit.Text = "Exit";
            b_Exit.UseVisualStyleBackColor = true;
            b_Exit.Click += Exit;
            // 
            // l_Volume
            // 
            l_Volume.AutoSize = true;
            l_Volume.Location = new Point(12, 46);
            l_Volume.Name = "l_Volume";
            l_Volume.Size = new Size(58, 17);
            l_Volume.TabIndex = 17;
            l_Volume.Text = "Volume: ";
            // 
            // b_DeleteConf
            // 
            b_DeleteConf.Location = new Point(76, 262);
            b_DeleteConf.Name = "b_DeleteConf";
            b_DeleteConf.Size = new Size(96, 26);
            b_DeleteConf.TabIndex = 18;
            b_DeleteConf.Text = "Delete config";
            b_DeleteConf.UseVisualStyleBackColor = true;
            b_DeleteConf.Click += b_DeleteConf_Click;
            // 
            // soundDevices_ComboBox
            // 
            soundDevices_ComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            soundDevices_ComboBox.FormattingEnabled = true;
            soundDevices_ComboBox.Location = new Point(120, 12);
            soundDevices_ComboBox.Name = "soundDevices_ComboBox";
            soundDevices_ComboBox.Size = new Size(389, 25);
            soundDevices_ComboBox.TabIndex = 19;
            // 
            // l_SoundDev
            // 
            l_SoundDev.AutoSize = true;
            l_SoundDev.Location = new Point(12, 15);
            l_SoundDev.Name = "l_SoundDev";
            l_SoundDev.Size = new Size(102, 17);
            l_SoundDev.TabIndex = 20;
            l_SoundDev.Text = "Playback device:";
            // 
            // cb_hearPlayedSound
            // 
            cb_hearPlayedSound.AutoSize = true;
            cb_hearPlayedSound.Checked = true;
            cb_hearPlayedSound.CheckState = CheckState.Checked;
            cb_hearPlayedSound.Location = new Point(12, 192);
            cb_hearPlayedSound.Name = "cb_hearPlayedSound";
            cb_hearPlayedSound.Size = new Size(138, 21);
            cb_hearPlayedSound.TabIndex = 21;
            cb_hearPlayedSound.Text = "Hear played sound";
            cb_hearPlayedSound.UseVisualStyleBackColor = true;
            cb_hearPlayedSound.CheckedChanged += cb_hearPlayedSound_CheckedChanged;
            // 
            // l_Sounds
            // 
            l_Sounds.AutoSize = true;
            l_Sounds.Location = new Point(120, 46);
            l_Sounds.Name = "l_Sounds";
            l_Sounds.Size = new Size(54, 17);
            l_Sounds.TabIndex = 22;
            l_Sounds.Text = "Sounds:";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(521, 304);
            Controls.Add(l_Sounds);
            Controls.Add(cb_hearPlayedSound);
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
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Soundboard";
            FormClosing += Form1_FormClosing;
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)trackBar).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button b_RegisterKey;
        private Label l_registeringKey;
        private Button b_Scan;
        private Button b_Exit;
        private Button b_DeleteConf;
        private ComboBox soundDevices_ComboBox;
        private Label l_SoundDev;
        internal TrackBar trackBar;
        internal Label l_Volume;
        internal ListBox listBox;
        private Label l_Sounds;
        internal CheckBox cb_StopPrevSound;
        internal CheckBox cb_hearPlayedSound;
    }
}