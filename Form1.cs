using System;
using System.IO;
using System.Windows.Forms;
using System.Linq;
using System.Collections.Generic;

namespace SoundBoard
{
    public partial class Form1 : Form
    {
        private NAudio.Wave.WaveOut output;
        private NAudio.Wave.Mp3FileReader sound;

        private readonly NotifyIcon notifyIcon = new() { Visible = true, Icon = System.Drawing.SystemIcons.Application, ContextMenuStrip = new() };

        private readonly string soundDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\Sounds\";

        private string[] soundFiles;
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

        private Dictionary<string, Keys> keys;

        private System.ComponentModel.BindingList<string> items;

        public Form1() => InitializeComponent();

        private void Form1_Load(object sender, EventArgs e)
        {
            isRegisteringKey = false;
            output = new() { DeviceNumber = 2 };

            trackBar_Scroll(null, null);
            ScanForSounds(null, null);
            LoadKeybinds();
            for (int i = listBox.Items.Count; i > 0; i--) { UpdateUIElements(i - 1); }

            _ = notifyIcon.ContextMenuStrip.Items.Add("Show", null, ShowWindowClicked);
            _ = notifyIcon.ContextMenuStrip.Items.Add("Exit", System.Drawing.SystemIcons.Error.ToBitmap(), Exit);

            notifyIcon.BalloonTipClicked += ShowWindowClicked;
            notifyIcon.DoubleClick += ShowWindowClicked;
        }

        protected override void WndProc(ref Message msg)
        {
            base.WndProc(ref msg);
            if (msg.Msg == 0x0312)
                PlaySound(msg.WParam.ToInt32());
        }

        private void LoadKeybinds()//maybe make async?
        {
            string path = Path.Combine(soundDirectory, "keybinds.json");
            if (!File.Exists(path)) return;

            string lastLine = File.ReadAllLines(path).Last();
            trackBar.Value = int.Parse(lastLine);
            trackBar_Scroll(null, null);

            Vars.Save = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText(path).TrimEnd(lastLine.ToCharArray()));
            for (int i = 0; i < Vars.Save.Count; i++)
            {
                if (i >= soundFiles.Length)
                    break;

                if (!Vars.Save.ContainsKey(soundFiles[i]))
                    Vars.Save.Add(soundFiles[i], Keys.None.ToString());

                keys[soundFiles[i]] = (Keys)Enum.Parse(typeof(Keys), Vars.Save[soundFiles[i]]);
                if (keys.ContainsKey(soundFiles[i]))
                {
                    Hotkey.Create(Handle, i, (int)keys[soundFiles[i]]);
                    UpdateUIElements(i);
                }
            }
        }

        private async void SaveKeybinds()
        {
            Vars.Save ??= new(keys.Count);
            for (int i = 0; i < keys.Count; i++)
            {
                Vars.Save[soundFiles[i]] = keys[soundFiles[i]].ToString();
            }
            await File.WriteAllTextAsync(Path.Combine(soundDirectory, "keybinds.json"), System.Text.Json.JsonSerializer.Serialize(Vars.Save) + $"\n{trackBar.Value}");
        }

        private void PlaySound(int id)
        {
            if (!cb_StopPrevSound.Checked || prevFileDir != soundFiles[id])//maybe make an mp3filereader array?
            {
                sound = new(soundFiles[id]);
                output.Init(sound);
                prevFileDir = soundFiles[id];
            }
            else
                _ = sound.Seek(0, SeekOrigin.Begin);

            trackBar_Scroll(null, null);
            output.Play();
        }


        private void ScanForSounds(object sender, EventArgs e)
        {
            if (!Directory.Exists(soundDirectory))
                _ = Directory.CreateDirectory(soundDirectory);

            soundFiles = Directory.GetFiles(soundDirectory, "*.mp3");
            keys = new(soundFiles.Length);

            string[] names = new string[soundFiles.Length];
            for (int i = 0; i < soundFiles.Length; i++)
            {
                names[i] = soundFiles[i][soundDirectory.Length..];
            }
            listBox.DataSource = items = new(names.ToList());
            Hotkey.Delete(Handle, items.Count);
            LoadKeybinds();
            for (int i = keys.Count; i > 0; i--)
            {
                UpdateUIElements(i - 1);
            }
        }

        private void b_RegisterKey_KeyDown(object sender, KeyEventArgs e)
        {
            if (!isRegisteringKey) return;
            selectedIndex = listBox.SelectedIndex;
            keys[soundFiles[selectedIndex]] = e.KeyCode;
            UpdateUIElements(selectedIndex);
            listBox.SelectedIndex = selectedIndex;
            Hotkey.Create(Handle, selectedIndex, (int)keys[soundFiles[selectedIndex]]);
            SaveKeybinds();
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
            items[index] = $"{key} - {soundFiles[index][soundDirectory.Length..]}";
        }
        private void b_RegisterKey_Click(object sender, EventArgs e)
        {
            isRegisteringKey = !isRegisteringKey;
        }

        private void trackBar_Scroll(object sender, EventArgs e)
        {
            output.Volume = trackBar.Value / 100f;
            l_Volume.Text = $"Volume: {trackBar.Value}%";
        }

        private void b_DeleteConf_Click(object sender, EventArgs e)
        {
            File.Delete(Path.Combine(soundDirectory, "keybinds.json"));
            Hotkey.Delete(Handle, soundFiles.Length);
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
        private void trackBar_MouseUp(object sender, MouseEventArgs e) => SaveKeybinds();

        private void Exit(object sender, EventArgs e)
        {
            SaveKeybinds();
            Hotkey.Delete(Handle, keys.Count);
            notifyIcon.Visible = false;
            notifyIcon?.Dispose();
            output?.Dispose();
            sound?.Dispose();
            Environment.Exit(Environment.ExitCode);
        }
    }

    internal class Vars
    {
        public static Dictionary<string, string> Save { get; set; }
    }

    internal class Hotkey
    {
        //[DllImport("user32.dll")]
        //public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        //[DllImport("user32.dll")]
        //public static extern bool ReleaseCapture();

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int UnregisterHotKey(IntPtr hWnd, int id);

        public static void Create(IntPtr hWnd, int id, int key) => _ = RegisterHotKey(hWnd, id, 0, key);

        public static void Delete(IntPtr hWnd, int numSounds)
        {
            for (int i = 0; i < numSounds; i++)
            {
                _ = UnregisterHotKey(hWnd, i);
            }
        }
    }
}