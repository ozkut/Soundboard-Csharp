using System.Runtime.InteropServices;
using System.IO;
using NAudio.Wave;
using System.Reflection.Metadata;

namespace SoundBoard
{
    public partial class Form1 : Form
    {
        private WaveOut output;
        private Mp3FileReader sound;

        private string[] soundFiles, names;
        private string prevFileDir;

        private bool isRegisteringKey_;
        private bool isRegisteringKey
        {
            get { return isRegisteringKey_; }
            set
            {
                isRegisteringKey_ = value;
                l_registeringKey.Text = $"Registering Key: {isRegisteringKey_}";
            }
        }

        private sbyte selectedIndex = 0;

        private Keys[] keys;


        public Form1() => InitializeComponent();

        private void Form1_Load(object sender, EventArgs e)
        {
            isRegisteringKey = false;
            output = new() { DeviceNumber = 2 };

            ScanForSounds(null, null);

            NotifyIcon notifyIcon = new() { Visible = true, Icon = SystemIcons.Application, ContextMenuStrip = new() };
            notifyIcon.ContextMenuStrip.Items.Add("Show", null, ShowWindowClicked);
            notifyIcon.ContextMenuStrip.Items.Add("Exit", SystemIcons.Error.ToBitmap(), Exit);

            notifyIcon.BalloonTipClicked += ShowWindowClicked;
            notifyIcon.DoubleClick += ShowWindowClicked;
        }

        protected override void WndProc(ref Message key)
        {
            base.WndProc(ref key);
            if (key.Msg == 0x0312)
                PlaySound(key.WParam.ToInt32());
        }

        private void PlaySound(int id)
        {
            if (!cb_StopPrevSound.Checked || prevFileDir != soundFiles[id])
            {
                sound = new(soundFiles[id]);
                output.Init(sound);
                prevFileDir = soundFiles[id];
            }
            else
                sound.Seek(0, SeekOrigin.Begin);

            output.Volume = trackBar.Value / 100f;
            output.Play();
        }

        private void Exit(object sender, EventArgs e)
        {
            Hotkey.Delete(Handle, keys.Length);
            output?.Dispose();
            sound?.Dispose();
            Environment.Exit(Environment.ExitCode);
        }

        private void ScanForSounds(object? sender, EventArgs? e)
        {
            string myDir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\Sounds\";

            if (!Directory.Exists(myDir))
                Directory.CreateDirectory(myDir);

            soundFiles = Directory.GetFiles(myDir, "*.mp3");
            names = new string[soundFiles.Length];
            keys = new Keys[soundFiles.Length];

            listBox.Items.Clear();

            for (int i = 0; i < soundFiles.Length; i++)
            {
                names[i] = soundFiles[i][myDir.Length..];
                listBox.Items.Insert(i, names[i]);
                keys[i] = Keys.None;
            }
        }

        private void b_RegisterKey_KeyDown(object sender, KeyEventArgs e)
        {
            if (!isRegisteringKey) return;
            selectedIndex = (sbyte)listBox.SelectedIndex;
            keys[selectedIndex] = e.KeyCode;
            b_RegisterKey.Text = keys[selectedIndex].ToString();
            listBox.Items[selectedIndex] = $"{keys[selectedIndex]} - {names[selectedIndex]}";//fix
            listBox.SelectedIndex = selectedIndex;
            Hotkey.Create(Handle, selectedIndex, e.KeyCode);
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

        private void listBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            b_RegisterKey.Text = keys[selectedIndex].ToString();
            isRegisteringKey = false;
        }
    }

    class Hotkey
    {
        //[DllImport("user32.dll")]
        //public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        //[DllImport("user32.dll")]
        //public static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        public static extern int RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);
        [DllImport("user32.dll")]
        public static extern int UnregisterHotKey(IntPtr hWnd, int id);

        public static void Create(IntPtr hWnd, int id, Keys key)
        {
            //Delete(hWnd);
            _ = RegisterHotKey(hWnd, id, 0, (int)key);
        }

        public static void Delete(IntPtr hWnd, int numSounds)
        {
            for (int i = 0; i < numSounds; i++)
            {
                _ = UnregisterHotKey(hWnd, i);
            }
        }
    }
}