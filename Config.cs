using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using SoundBoard;
using static Soundboard.GlobalVariables;

namespace Soundboard
{
    internal static class Config
    {
        internal async static void LoadValues(Form1 form1)
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

                if (line!.StartsWith(Environment.GetFolderPath(Environment.SpecialFolder.Desktop)))
                {
                    string[] parts = line.Split('|');

                    if (parts.Length != 2)
                        return;

                    string key = parts[0].Trim();
                    if (soundFiles.Contains(key))
                    {
                        if (!keys.ContainsKey(key))
                            keys.Add(key, (Keys)Enum.Parse(typeof(Keys), parts[1].Trim())/*<=Value*/);
                        Hotkey.CreateKey(form1.Handle, Array.IndexOf(soundFiles, key), (int)keys[key]);
                    }
                }

                else if (line!.StartsWith("Volume:"))
                {
                    form1.trackBar.Value = int.Parse(line.TrimStart("Volume: ".ToCharArray()));
                    form1.trackBar_Scroll();
                }

                else if (line!.StartsWith("Disable echo:"))
                    form1.cb_StopPrevSound.Checked = bool.Parse(line.TrimStart("Disable echo: ".ToCharArray()));

                else if (line!.StartsWith("Enable hearing played sound:"))
                    form1.cb_hearPlayedSound.Checked = bool.Parse(line.TrimStart("Enable hearing played sound: ".ToCharArray()));

                else if (line!.StartsWith("Show file extensions:"))
                    form1.cb_ShowExtension.Checked = bool.Parse(line.TrimStart("Show file extensions: ".ToCharArray()));

                index++;
            }
            reader.Close();
        }

        internal async static void SaveValues(Form1 form1)
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
            await writer.WriteLineAsync($"Show file extensions: {form1.cb_ShowExtension.Checked}");
            writer.Close();
        }
    }
}