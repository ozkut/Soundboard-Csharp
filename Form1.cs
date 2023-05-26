using System;
using System.IO;
using System.Windows.Forms;
using System.Linq;
using System.Collections.Generic;
using static Soundboard.Hotkey;

namespace SoundBoard
{
    public partial class Form1 : Form
    {
        private NAudio.Wave.WaveOut output;
        private NAudio.Wave.Mp3FileReader sound;

        private readonly NotifyIcon notifyIcon = new() { Visible = true, Icon = System.Drawing.SystemIcons.Application, ContextMenuStrip = new() };

        public readonly string soundDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\Sounds\";

        public string[] soundFiles;
        private string prevFileDir;

        private bool isRegisteringKey_;
        private bool isRegisteringKey
        {
            get => isRegisteringKey_;
            set
            {
                isRegisteringKey_ = value;
                l_registeringKey.Text = $"Registering Key: {isRegisteringKey_}";
            }
        }

        private int selectedIndex = 0;

        public Dictionary<string, Keys> keys;

        private System.ComponentModel.BindingList<string> items;

        public Form1() => InitializeComponent();

        protected override void WndProc(ref Message msg)
        {
            base.WndProc(ref msg);
            if (msg.Msg == 0x0312)
                PlaySound(msg.WParam.ToInt32());
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            isRegisteringKey = false;
            output = new() { DeviceNumber = 2 };

            trackBar_Scroll(null, null);
            ScanForSounds(null, null);
            LoadKeys(this);
            for (int i = listBox.Items.Count; i > 0; i--) { UpdateUIElements(i - 1); }

            _ = notifyIcon.ContextMenuStrip.Items.Add("Show", null, ShowWindowClicked);
            _ = notifyIcon.ContextMenuStrip.Items.Add("Exit", System.Drawing.SystemIcons.Error.ToBitmap(), Exit);

            notifyIcon.BalloonTipClicked += ShowWindowClicked;
            notifyIcon.DoubleClick += ShowWindowClicked;
        }

        private void ScanForSounds(object sender, EventArgs e)//fix
        {
            _ = Directory.CreateDirectory(soundDirectory);
            soundFiles = Directory.GetFiles(soundDirectory, "*.mp3");
            keys = new(soundFiles.Length);
            listBox.DataSource = items = new(soundFiles.ToList());

            Delete(Handle, items.Count);
            LoadKeys(this);

            for (int i = keys.Count; i > 0; i--)
            {
                UpdateUIElements(i - 1);
            }
        }

        private void PlaySound(int id)
        {
            if (!cb_StopPrevSound.Checked || prevFileDir != soundFiles[id])
            {
                sound = new(soundFiles[id]);
                output.Init(sound);
                prevFileDir = soundFiles[id];
            }
            else _ = sound.Seek(0, SeekOrigin.Begin);

            trackBar_Scroll(null, null);
            output.Play();
        }

        private void b_RegisterKey_KeyDown(object sender, KeyEventArgs e)
        {
            if (!isRegisteringKey) return;
            selectedIndex = listBox.SelectedIndex;
            keys[soundFiles[selectedIndex]] = e.KeyCode;
            UpdateUIElements(selectedIndex);
            listBox.SelectedIndex = selectedIndex;
            Create(Handle, selectedIndex, (int)keys[soundFiles[selectedIndex]]);
            SaveKeys(this);
            isRegisteringKey = false;
        }

        private void listBox_MouseClick(object sender, MouseEventArgs e)
        {
            selectedIndex = listBox.SelectedIndex;
            UpdateUIElements(selectedIndex);
            if (isRegisteringKey) isRegisteringKey = false;
        }

        private void UpdateUIElements(int index)
        {
            string key = !keys.ContainsKey(soundFiles[index]) ? Keys.None.ToString() : keys[soundFiles[index]].ToString();
            b_RegisterKey.Text = key;
            items[index] = $"{key} - {Path.GetFileNameWithoutExtension(soundFiles[index][soundDirectory.Length..])}";
        }
        private void b_RegisterKey_Click(object sender, EventArgs e) => isRegisteringKey = !isRegisteringKey;

        public void trackBar_Scroll(object sender, EventArgs e)
        {
            output.Volume = trackBar.Value / 100f;
            l_Volume.Text = $"Volume: {trackBar.Value}%";
        }

        private void b_DeleteConf_Click(object sender, EventArgs e)
        {
            File.Delete(Path.Combine(soundDirectory, "keybinds.json"));
            Delete(Handle, soundFiles.Length);
            Application.Restart();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Hide();
            WindowState = FormWindowState.Minimized;
            e.Cancel = true;
        }

        private void ShowWindowClicked(object sender, EventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
        }

        private void trackBar_MouseUp(object sender, MouseEventArgs e) => SaveKeys(this);

        private void Exit(object sender, EventArgs e)
        {
            SaveKeys(this);
            Delete(Handle, keys.Count);
            notifyIcon.Visible = false;
            notifyIcon?.Dispose();
            output?.Dispose();
            sound?.Dispose();
            Environment.Exit(Environment.ExitCode);
        }
    }
}