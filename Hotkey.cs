using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using SoundBoard;

namespace Soundboard
{
    internal class Hotkey
    {
        [DllImport("user32.dll")]
        public static extern int RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);

        [DllImport("user32.dll")]
        public static extern int UnregisterHotKey(IntPtr hWnd, int id);

        public static void Create(IntPtr hWnd, int id, int key) => _ = RegisterHotKey(hWnd, id, 0, key);

        public static void Delete(IntPtr hWnd, int numSounds)
        {
            for (int i = 0; i < numSounds; i++)
            {
                _ = UnregisterHotKey(hWnd, i);
            }
        }

        public async static void LoadKeys(Form1 form)
        {
            string path = Path.Combine(form.soundDirectory, "keybinds.json");
            if (!File.Exists(path)) return;

            using StreamReader reader = new(path);
            while (!reader.EndOfStream)
            {
                string line = await reader.ReadLineAsync();

                if (line.StartsWith("Volume:"))
                {
                    form.trackBar.Value = int.Parse(File.ReadAllLines(path).Last().TrimStart("Volume: ".ToCharArray()));
                    form.trackBar_Scroll(null, null);
                }

                else
                {
                    string[] parts = line.Split('|');
                    if (parts.Length == 2)
                    {
                        string key = parts[0].Trim();
                        Keys value = (Keys)Enum.Parse(typeof(Keys), parts[1].Trim());
                        if (!form.keys.ContainsKey(key) || !form.keys.ContainsValue(value))
                            form.keys.Add(key, value);
                    }
                }
            }
            reader.Close();
        }

        public async static void SaveKeys(Form1 form)
        {
            using StreamWriter writer = new(Path.Combine(form.soundDirectory, "keybinds.json"));
            for (int i = 0; i < form.keys.Count; i++)
            {
                if (!form.keys.ContainsKey(form.soundFiles[i]))
                    form.keys.Add(form.soundFiles[i], Keys.None);
                await writer.WriteLineAsync($"{form.soundFiles[i]} | {form.keys[form.soundFiles[i]]}");
            }
            await writer.WriteLineAsync($"Volume: {form.trackBar.Value}");
            writer.Close();
        }
    }
}