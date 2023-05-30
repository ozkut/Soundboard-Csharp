using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using SoundBoard;
using static Soundboard.GlobalVariables;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Soundboard
{
    internal class Hotkey
    {
        private const string configFileName = "config.txt";

        [DllImport("user32.dll")]
        public static extern int RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);

        [DllImport("user32.dll")]
        public static extern int UnregisterHotKey(IntPtr hWnd, int id);

        internal static void CreateKey(IntPtr handle, int id, int key) => _ = RegisterHotKey(handle, id, 0, key);

        internal static void DeleteKeys(IntPtr handle, int numKeys_Or_id, bool deleteSingleKey = false)
        {
            for (int i = 0; i < numKeys_Or_id; i++)
            {
                if (deleteSingleKey)
                {
                    if (i == numKeys_Or_id - 1)
                        i = numKeys_Or_id;
                    else
                        continue;
                }
                _ = UnregisterHotKey(handle, i);
            }
        }

        internal async static void LoadKeys(Form1 form1)
        {
            string path = Path.Combine(soundDirectory, configFileName);
            if (!File.Exists(path)) 
                return;
            int index = 0;
            using StreamReader reader = new(path);
            while (!reader.EndOfStream)
            {
                #pragma warning disable CS8600
                string line = await reader.ReadLineAsync();

                if (line!.StartsWith("Volume:"))
                {
                    form1.trackBar.Value = int.Parse(File.ReadAllLines(path).Last().TrimStart("Volume: ".ToCharArray()));
                    form1.trackBar_Scroll();
                }

                else
                {
                    string[] parts = line.Split('|');
                    if (parts.Length != 2)
                        return;
                    string key = parts[0].Trim();
                    Keys value = (Keys)Enum.Parse(typeof(Keys), parts[1].Trim());
                    if (soundFiles.Contains(key))
                    {
                        if (!keys.ContainsKey(key))
                            keys.Add(key, value);
                        CreateKey(form1.Handle, Array.IndexOf(soundFiles, key), (int)keys[key]); 
                    }
                }
                index++;
            }
            reader.Close();
        }

        internal async static void SaveKeys(int trackBarValue)
        {
            using StreamWriter writer = new(Path.Combine(soundDirectory, configFileName));
            for (int i = 0; i < keys.Count; i++)
            {
                if (keys.Count > 0 && !keys.ContainsKey(soundFiles[i]))
                    keys.Add(soundFiles[i], Keys.None);
                await writer.WriteLineAsync($"{soundFiles[i]} | {keys[soundFiles[i]]}");
            }
            await writer.WriteLineAsync($"Volume: {trackBarValue}");
            writer.Close();
        }
    }
}