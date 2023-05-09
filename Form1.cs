using System.Runtime.InteropServices;
using System.IO;
using NAudio.Wave;
using System.Reflection.Metadata;

namespace SoundBorad
{
    public partial class Form1 : Form
    {
        private WaveOut output;
        private Mp3FileReader sound;

        private string[] soundFiles;
        private string prevFileDir;

        private bool isRegisteringKey = false;

        private const Keys defaultKey = Keys.NumPad5;
        private List<Keys> keys;

        public Form1() => InitializeComponent();

        private void Form1_Load(object sender, EventArgs e)
        {
            output = new() { DeviceNumber = 2 };
            ScanForSounds();

            l_registeringKey.Text = "Registering Key: " + isRegisteringKey.ToString();

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
            Hotkey.Delete(Handle, keys.Count);
            output?.Dispose();
            sound?.Dispose();
            Environment.Exit(Environment.ExitCode);
        }

        private void b_Scan_Click(object sender, EventArgs e) => ScanForSounds();

        private void ScanForSounds()
        {
            string myDir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\Sounds\";

            if (!Directory.Exists(myDir))
                Directory.CreateDirectory(myDir);

            soundFiles = Directory.GetFiles(myDir, "*.mp3");
            keys = new(soundFiles.Length);

            for (int i = 0; i < soundFiles.Length; i++)
            {
                comboBox.Items.Add(soundFiles[i][myDir.Length..]);//same as substring?
                keys.Insert(i, defaultKey);
            }

            comboBox.SelectedIndex = 0;
        }

        private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            b_RegisterKey.Text = keys[comboBox.SelectedIndex].ToString();
        }

        private void b_RegisterKey_KeyDown(object sender, KeyEventArgs e)
        {
            if (!isRegisteringKey) return;
            keys[comboBox.SelectedIndex] = e.KeyCode;
            b_RegisterKey.Text = keys[comboBox.SelectedIndex].ToString();
            Hotkey.Create(Handle, comboBox.SelectedIndex, e.KeyCode);
        }

        private void b_RegisterKey_Click(object sender, EventArgs e)
        {
            isRegisteringKey = !isRegisteringKey;
            l_registeringKey.Text = "Registering Key: " + isRegisteringKey.ToString();
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