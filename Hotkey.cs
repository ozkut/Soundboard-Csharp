using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using SoundBoard;
using static Soundboard.GlobalVariables;

namespace Soundboard
{
    internal class Hotkey
    {
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

                if (line!.StartsWith("Show file extensions:"))
                    form1.cb_ShowExtention.Checked = bool.Parse(line.TrimStart("Show file extensions: ".ToCharArray()));

                else if (line!.StartsWith("Disable echo:"))
                    form1.cb_StopPrevSound.Checked = bool.Parse(line.TrimStart("Disable echo: ".ToCharArray()));

                else if (line!.StartsWith("Enable hearing played sound:"))
                    form1.cb_hearPlayedSound.Checked = bool.Parse(line.TrimStart("Enable hearing played sound: ".ToCharArray()));

                else if (line!.StartsWith("Volume:"))
                {
                    form1.trackBar.Value = int.Parse(line.TrimStart("Volume: ".ToCharArray()));
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

        internal async static void SaveKeys(Form1 form1)
        {
            using StreamWriter writer = new(Path.Combine(soundDirectory, configFileName));
            for (int i = 0; i < keys.Count; i++)
            {
                if (keys.Count > 0 && !keys.ContainsKey(soundFiles[i]))
                    keys.Add(soundFiles[i], Keys.None);
                await writer.WriteLineAsync($"{soundFiles[i]} | {keys[soundFiles[i]]}");
            }
            await writer.WriteLineAsync($"Volume: {form1.trackBar.Value}");
            await writer.WriteLineAsync($"Disable echo: {form1.cb_StopPrevSound.Checked}");
            await writer.WriteLineAsync($"Enable hearing played sound: {form1.cb_hearPlayedSound.Checked}");
            await writer.WriteLineAsync($"Show file extensions: {form1.cb_ShowExtention.Checked}");
            writer.Close();
        }
    }
}