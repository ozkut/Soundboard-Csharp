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

        protected override void WndProc(ref Message message)
        {
            base.WndProc(ref message);
            if (message.Msg == 0x0312)
                PlaySound(currentSoundId = message.WParam.ToInt32());
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            isRegisteringKey = false;

            GetAudioDevices();
            trackBar_Scroll();
            ScanForSounds();
            LoadKeys(this);

            for (int i = listBox.Items.Count; i > 0; i--) { UpdateUIElements(i - 1); }

            _ = notifyIcon.ContextMenuStrip.Items.Add("Show", null, ShowWindowClicked!);
            _ = notifyIcon.ContextMenuStrip.Items.Add("Exit", null/*System.Drawing.SystemIcons.Error.ToBitmap()*/, Exit!);

            notifyIcon.BalloonTipClicked += ShowWindowClicked!;
            notifyIcon.DoubleClick += ShowWindowClicked!;
        }

        /// <summary>
        /// Gets all audio devices and adds them to their comboBox. 
        /// Temporarily using WASAPI to get the default device as the DirectSound default device GUID is wrong.
        /// </summary>
        private void GetAudioDevices()
        {
            bool deviceFound = false;
            string defaultDevice = new MMDeviceEnumerator().GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia).FriendlyName;
            DirectSoundDeviceInfo[] devices = DirectSoundOut.Devices.ToArray();
            for (int i = 1; i < devices.Length; i++)
            {
                soundDevices_ComboBox.Items.Add($"{i - 1}: {devices[i].Description}");
                Guids[i - 1] = devices[i].Guid;
                if (!deviceFound)
                {
                    bool isDefaultDevice = devices[i].Description == defaultDevice;
                    deviceFound = devices[i].Description == "CABLE Input (VB-Audio Virtual Cable)";//directsound's default playback device guid is wrong so i had to use the wasapi method to get the default playback device
                    if (deviceFound || isDefaultDevice)
                    {
                        soundDevices_ComboBox.SelectedIndex = i - 1;
                        if (isDefaultDevice)
                            defaultGuid = devices[i].Guid;
                    }
                }
            }
        }

        /// <summary>
        /// Scans the "sounds" folder on the desktop and (if they were added to the config) loads their appropriate keys.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScanForSounds(object sender = null!, EventArgs e = null!)
        {
            if (keys != null)
                DeleteKeys(Handle, keys.Count);
            _ = Directory.CreateDirectory(soundDirectory);
            soundFiles = Directory.GetFiles(soundDirectory).Where(file => supportedFormats.Contains(Path.GetExtension(file).ToLower())).ToArray();
            keys = new(soundFiles.Length);
            listBox.DataSource = items = new(soundFiles.ToList());
            LoadKeys(this);
            for (int i = soundFiles.Length; i > 0; i--) { UpdateUIElements(i - 1); }
        }

        /// <summary>
        /// Initializes the default playback device
        /// </summary>
        /// <param name="id"></param>
        private void InitializeDefaultDevicePlayback(int id)
        {
            if (!cb_hearPlayedSound.Checked)
                return;
            defaultOutput = new(defaultGuid);
            defaultSound = new(soundFiles[id]);
            defaultWaveChannel = new(defaultSound, trackBar.Value / 100f, 0);
            defaultOutput.Init(defaultWaveChannel);
        }

        /// <summary>
        /// Plays the sound with the corresponding id or seeks to the start of the sound if it has been played previously.
        /// </summary>
        /// <param name="id"></param>
        private void PlaySound(int id)
        {
            if (!cb_StopPrevSound.Checked || prevFileDir != soundFiles[id] || prevSoundIndex != soundDevices_ComboBox.SelectedIndex)
            {
                output = new(Guids[soundDevices_ComboBox.SelectedIndex]);
                sound = new(soundFiles[id]);
                waveChannel = new(sound, trackBar.Value / 100f, 0);
                output.Init(waveChannel);

                InitializeDefaultDevicePlayback(id);

                prevFileDir = soundFiles[id];
                prevSoundIndex = soundDevices_ComboBox.SelectedIndex;
            }

            else
            {
                _ = sound.Seek(0, SeekOrigin.Begin);
                if (defaultSound != null && cb_hearPlayedSound.Checked)
                    _ = defaultSound.Seek(0, SeekOrigin.Begin);
            }

            output.Play();
            if (cb_hearPlayedSound.Checked)
                defaultOutput.Play();
        }

        private void cb_hearPlayedSound_CheckedChanged(object sender, EventArgs e)
        {
            if (output == null)
                return;

            if (output.PlaybackState == PlaybackState.Stopped || !cb_hearPlayedSound.Checked)
                defaultOutput.Stop();

            else if (output.PlaybackState == PlaybackState.Playing)
            {
                InitializeDefaultDevicePlayback(currentSoundId);
                defaultSound.Position = sound.Position;
                defaultOutput.Play();
            }
        }

        private void b_RegisterKey_Click(object sender, EventArgs e) => isRegisteringKey = !isRegisteringKey;

        private void b_RegisterKey_KeyDown(object sender, KeyEventArgs e)
        {
            if (!isRegisteringKey || soundFiles.Length <= 0)
                return;
            keys[soundFiles[listBox.SelectedIndex]] = e.KeyCode;
            listBox.SelectedIndex = listBox.SelectedIndex;
            UpdateUIElements(listBox.SelectedIndex);
            DeleteKeys(Handle, listBox.SelectedIndex, true);
            CreateKey(Handle, listBox.SelectedIndex, (int)keys[soundFiles[listBox.SelectedIndex]]);
            SaveKeys(this);
            isRegisteringKey = false;
        }

        private void listBox_MouseClick(object sender, MouseEventArgs e)
        {
            UpdateUIElements(listBox.SelectedIndex);
            if (isRegisteringKey)
                isRegisteringKey = false;
        }

        /// <summary>
        /// Updates the Listbox entries if sounds were added or removed.
        /// </summary>
        /// <param name="index"></param>
        private void UpdateUIElements(int index)
        {
            if (index >= soundFiles.Length && items.Count > 0)
                return;
            string key = b_RegisterKey.Text = soundFiles[index] == string.Empty || !keys.ContainsKey(soundFiles[index]) ? Keys.None.ToString() : keys[soundFiles[index]].ToString();
            items[index] = $"{key} - {Path.GetFileNameWithoutExtension(soundFiles[index])}";
        }

        /// <summary>
        /// Updates the Trackbar and its label.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal void trackBar_Scroll(object sender = null!, EventArgs e = null!)
        {
            if (waveChannel != null)
            {
                waveChannel.Volume = trackBar.Value / 100f;
                if (defaultWaveChannel != null)
                    defaultWaveChannel.Volume = trackBar.Value / 100f;
            }
            l_Volume.Text = $"Volume: {trackBar.Value}%";
        }

        private void trackBar_MouseUp(object sender, MouseEventArgs e) => SaveKeys(this);

        private void b_DeleteConf_Click(object sender, EventArgs e)
        {
            File.Delete(Path.Combine(soundDirectory, configName));
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

        /// <summary>
        /// Exits the program.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Exit(object sender, EventArgs e)
        {
            SaveKeys(this);
            DeleteKeys(Handle, keys.Count);

            notifyIcon.Visible = false;
            notifyIcon?.Dispose();

            output?.Dispose();
            sound?.Dispose();
            waveChannel?.Dispose();

            defaultOutput?.Dispose();
            defaultSound?.Dispose();
            defaultWaveChannel?.Dispose();

            Environment.Exit(Environment.ExitCode);
        }

    }
}