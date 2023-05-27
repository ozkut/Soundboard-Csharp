using System;
using System.IO;
using System.Windows.Forms;
using System.Linq;
using NAudio.Wave;
using NAudio.CoreAudioApi;
using static Soundboard.Hotkey;
using static Soundboard.GlobalVariables;

namespace SoundBoard
{
    public partial class Form1 : Form
    {
        private bool isRegisteringKey
        {
            get => isRegisteringKey_;
            set
            {
                isRegisteringKey_ = value;
                l_registeringKey.Text = $"Registering Key: {isRegisteringKey_}";
            }
        }

        public Form1() => InitializeComponent();

        protected override void WndProc(ref Message msg)
        {
            base.WndProc(ref msg);
            if (msg.Msg == 0x0312)
                PlaySound(msg.WParam.ToInt32());
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            bool deviceFound = false;
            int i = -1;

            foreach (DirectSoundDeviceInfo dev in DirectSoundOut.Devices)
            {
                if (i > -1)
                {
                    soundDevices_ComboBox.Items.Add($"{i} - {dev.Description}");
                    Guids[i] = dev.Guid;

                    if (!deviceFound)
                    {
                        deviceFound = dev.Description == "CABLE Input (VB-Audio Virtual Cable)";

                        if (deviceFound || dev.Description == new MMDeviceEnumerator().GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia).FriendlyName)//directsound's default playback device guid is wrong so i had to use the wasapi method to get the default playback device
                            soundDevices_ComboBox.SelectedIndex = i;
                    }
                }
                i++;
            }

            isRegisteringKey = false;
            prevSoundIndex = soundDevices_ComboBox.SelectedIndex;
            output = new(Guids[soundDevices_ComboBox.SelectedIndex]);

            CreateShortcut(System.Reflection.Assembly.GetEntryAssembly().GetName().Name);
            trackBar_Scroll(null, null);
            ScanForSounds(null, null);
            LoadKeys(this);
            for (int j = listBox.Items.Count; j > 0; j--) { UpdateUIElements(j - 1); }

            _ = notifyIcon.ContextMenuStrip.Items.Add("Show", null, ShowWindowClicked);
            _ = notifyIcon.ContextMenuStrip.Items.Add("Exit", null/*System.Drawing.SystemIcons.Error.ToBitmap()*/, Exit);

            notifyIcon.BalloonTipClicked += ShowWindowClicked;
            notifyIcon.DoubleClick += ShowWindowClicked;
        }

        private static void CreateShortcut(string shortcutName)
        {
            string shortcutLocation = $"{Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)}\\{shortcutName}.url";
            if (File.Exists(shortcutLocation))
                return;
            using StreamWriter writer = new(shortcutLocation);
            string app = System.Reflection.Assembly.GetEntryAssembly().Location.Replace(".dll", ".exe");
            writer.WriteLine("[InternetShortcut]");
            writer.WriteLine("URL=file:///" + app);
            writer.WriteLine("IconIndex=0");
            string icon = app.Replace('\\', '/');
            writer.WriteLine("IconFile=" + icon);
        }

        private void ScanForSounds(object sender, EventArgs e)
        {
            _ = Directory.CreateDirectory(soundDirectory);
            soundFiles = Directory.GetFiles(soundDirectory, "*.mp3");
            keys = new(soundFiles.Length);
            listBox.DataSource = items = new(soundFiles.ToList());

            DeleteKeys(Handle, items.Count);
            LoadKeys(this);

            for (int i = soundFiles.Length; i > 0; i--) { UpdateUIElements(i - 1); }
        }

        private void PlaySound(int id)
        {
            if (!cb_StopPrevSound.Checked || prevFileDir != soundFiles[id] || prevSoundIndex != soundDevices_ComboBox.SelectedIndex)
            {
                sound = new(soundFiles[id]);
                output = new(Guids[soundDevices_ComboBox.SelectedIndex]);
                output.Init(sound);
                prevFileDir = soundFiles[id];
                prevSoundIndex = soundDevices_ComboBox.SelectedIndex;
            }
            else
                _ = sound.Seek(0, SeekOrigin.Begin);

            trackBar_Scroll(null, null);
            output.Play();
        }

        private void soundDevices_ComboBox_SelectedValueChanged(object sender, EventArgs e)
        {

            //MessageBox.Show(soundDevices_ComboBox.SelectedIndex.ToString());
        }

        private void soundDevices_ComboBox_SelectedIndexChanged(object sender, EventArgs e) { }

        private void b_RegisterKey_Click(object sender, EventArgs e) => isRegisteringKey = !isRegisteringKey;

        private void b_RegisterKey_KeyDown(object sender, KeyEventArgs e)
        {
            if (!isRegisteringKey) return;
            selectedIndex = listBox.SelectedIndex;
            keys[soundFiles[selectedIndex]] = e.KeyCode;
            UpdateUIElements(selectedIndex);
            listBox.SelectedIndex = selectedIndex;
            CreateKey(Handle, selectedIndex, (int)keys[soundFiles[selectedIndex]]);
            SaveKeys(trackBar.Value);
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
            if (index >= soundFiles.Length) return;
            string key = !keys.ContainsKey(soundFiles[index]) ? Keys.None.ToString() : keys[soundFiles[index]].ToString();
            b_RegisterKey.Text = key;
            items[index] = $"{key} - {Path.GetFileNameWithoutExtension(soundFiles[index][soundDirectory.Length..])}";
        }

        public void trackBar_Scroll(object sender, EventArgs e)
        {
            output.Volume = trackBar.Value / 100f;
            l_Volume.Text = $"Volume: {trackBar.Value}%";
        }

        private void trackBar_MouseUp(object sender, MouseEventArgs e) => SaveKeys(trackBar.Value);

        private void b_DeleteConf_Click(object sender, EventArgs e)
        {
            File.Delete(Path.Combine(soundDirectory, "keybinds.json"));
            DeleteKeys(Handle, soundFiles.Length);
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


        private void Exit(object sender, EventArgs e)
        {
            SaveKeys(trackBar.Value);
            DeleteKeys(Handle, keys.Count);
            notifyIcon.Visible = false;
            notifyIcon?.Dispose();
            output?.Dispose();
            sound?.Dispose();
            Environment.Exit(Environment.ExitCode);
        }
    }
}