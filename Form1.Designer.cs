namespace SoundBorad
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
            comboBox = new ComboBox();
            ((System.ComponentModel.ISupportInitialize)trackBar).BeginInit();
            SuspendLayout();
            // 
            // b_RegisterKey
            // 
            b_RegisterKey.Location = new Point(12, 126);
            b_RegisterKey.Name = "b_RegisterKey";
            b_RegisterKey.Size = new Size(75, 23);
            b_RegisterKey.TabIndex = 13;
            b_RegisterKey.UseVisualStyleBackColor = true;
            b_RegisterKey.Click += b_RegisterKey_Click;
            b_RegisterKey.KeyDown += b_RegisterKey_KeyDown;
            // 
            // l_registeringKey
            // 
            l_registeringKey.AutoSize = true;
            l_registeringKey.Location = new Point(93, 129);
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
            // 
            // cb_StopPrevSound
            // 
            cb_StopPrevSound.AutoSize = true;
            cb_StopPrevSound.Checked = true;
            cb_StopPrevSound.CheckState = CheckState.Checked;
            cb_StopPrevSound.Location = new Point(12, 99);
            cb_StopPrevSound.Name = "cb_StopPrevSound";
            cb_StopPrevSound.Size = new Size(282, 21);
            cb_StopPrevSound.TabIndex = 12;
            cb_StopPrevSound.Text = "Stop previous sound if new sound is played";
            cb_StopPrevSound.UseVisualStyleBackColor = true;
            // 
            // b_Scan
            // 
            b_Scan.Location = new Point(212, 23);
            b_Scan.Name = "b_Scan";
            b_Scan.Size = new Size(75, 23);
            b_Scan.TabIndex = 10;
            b_Scan.Text = "Scan";
            b_Scan.UseVisualStyleBackColor = true;
            // 
            // comboBox
            // 
            comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox.FormattingEnabled = true;
            comboBox.Location = new Point(12, 63);
            comboBox.Name = "comboBox";
            comboBox.Size = new Size(275, 25);
            comboBox.TabIndex = 11;
            comboBox.SelectedIndexChanged += comboBox_SelectedIndexChanged;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(299, 158);
            Controls.Add(b_RegisterKey);
            Controls.Add(l_registeringKey);
            Controls.Add(trackBar);
            Controls.Add(cb_StopPrevSound);
            Controls.Add(b_Scan);
            Controls.Add(comboBox);
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
        private TrackBar trackBar;
        private CheckBox cb_StopPrevSound;
        private Button b_Scan;
        private ComboBox comboBox;
    }
}