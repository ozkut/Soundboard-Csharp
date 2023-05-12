using System;
using System.IO;
using System.Windows.Forms;
using System.Linq;

namespace SoundBoard
{
    public partial class Form1 : Form
    {
        private NAudio.Wave.WaveOut output;
        private NAudio.Wave.Mp3FileReader sound;
        private NotifyIcon notifyIcon = new() { Visible = true, Icon = System.Drawing.SystemIcons.Application, ContextMenuStrip = new() };

        private readonly string myDir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\Sounds\";

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

        private sbyte[] keys;

        private System.ComponentModel.BindingList<string> items;


        public Form1() => InitializeComponent();

        private void Form1_Load(object sender, EventArgs e)
        {
            isRegisteringKey = false;
            output = new() { DeviceNumber = 2 };

            ScanForSounds(null, null);

            LoadKeybinds();

            for (int i = listBox.Items.Count; i > 0; i--)
            {
                UpdateUIElements(i - 1);
            }

            _ = notifyIcon.ContextMenuStrip.Items.Add("Show", null, ShowWindowClicked);
            _ = notifyIcon.ContextMenuStrip.Items.Add("Exit", System.Drawing.SystemIcons.Error.ToBitmap(), Exit);

            notifyIcon.BalloonTipClicked += ShowWindowClicked;
            notifyIcon.DoubleClick += ShowWindowClicked;
        }

        protected override void WndProc(ref Message key)
        {
            base.WndProc(ref key);
            if (key.Msg == 0x0312)
                PlaySound(key.WParam.ToInt32());
        }

        private void LoadKeybinds()//maybe make async?
        {
            string path = Path.Combine(myDir, "keybinds.json");
            if (!File.Exists(path)) 
                return;
            Vars.Key = System.Text.Json.JsonSerializer.Deserialize<sbyte[]>(File.ReadAllText(path));
            keys = Vars.Key.ToArray();
            for (int i = 0; i < keys.Length; i++)
            {
                Hotkey.Create(Handle, i, keys[i]);
            }
        }

        private async void SaveKeybinds()
        {
            Vars.Key = keys.ToArray();
            await File.WriteAllTextAsync(Path.Combine(myDir, "keybinds.json"), System.Text.Json.JsonSerializer.Serialize(Vars.Key));
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

            output.Volume = trackBar.Value / 100f;
            output.Play();
        }

        private void Exit(object sender, EventArgs e)
        {
            SaveKeybinds();
            Hotkey.Delete(Handle, keys.Length);
            notifyIcon.Visible = false;
            notifyIcon?.Dispose();
            output?.Dispose();
            sound?.Dispose();
            Environment.Exit(Environment.ExitCode);
        }

        private void ScanForSounds(object sender, EventArgs e)
        {
            if (!Directory.Exists(myDir))
                _ = Directory.CreateDirectory(myDir);

            soundFiles = Directory.GetFiles(myDir, "*.mp3");
            keys = new sbyte[soundFiles.Length];

            string[] names = new string[soundFiles.Length];
            for (int i = 0; i < soundFiles.Length; i++)
            {
                names[i] = soundFiles[i][myDir.Length..];
            }
            listBox.DataSource = items = new(names.ToList());
        }

        private void b_RegisterKey_KeyDown(object sender, KeyEventArgs e)
        {
            if (!isRegisteringKey) return;

            selectedIndex = listBox.SelectedIndex;
            keys[selectedIndex] = (sbyte)e.KeyCode;
            UpdateUIElements(selectedIndex);
            listBox.SelectedIndex = selectedIndex;
            Hotkey.Create(Handle, selectedIndex, keys[selectedIndex]);
            isRegisteringKey = false;
        }

        private void b_RegisterKey_Click(object sender, EventArgs e)
        {
            isRegisteringKey = !isRegisteringKey;
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

        private void listBox_MouseClick(object sender, MouseEventArgs e)
        {
            selectedIndex = listBox.SelectedIndex;
            UpdateUIElements(selectedIndex);
            if (isRegisteringKey) isRegisteringKey = false;
        }

        private void UpdateUIElements(int index)
        {
            string key = index > keys.Length - 1 ? Keys.None.ToString() : ((Keys)keys[index]).ToString();
            b_RegisterKey.Text = key;
            items[index] = $"{key} - {soundFiles[index][myDir.Length..]}";
        }
    }

    internal class Vars
    {
        public static sbyte[] Key { get; set; }
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