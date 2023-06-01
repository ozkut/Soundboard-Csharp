using System;
using NAudio.Wave;
using System.Windows.Forms;
using System.Linq;
using System.Collections.Generic;

namespace Soundboard
{
    #pragma warning disable CS8618 //Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    internal static class GlobalVariables
    {
        internal const string configFileName = "config.txt";
        internal static readonly string soundDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\Sounds\";

        internal static DirectSoundOut output, defaultOutput;
        internal static MediaFoundationReader sound, defaultSound;
        internal static WaveChannel32 waveChannel, defaultWaveChannel;

        internal static Guid[] Guids = new Guid[DirectSoundOut.Devices.Count()];
        internal static Guid defaultGuid;

        internal static string[] soundFiles;
        internal static string prevFileDir;

        internal static bool isRegisteringKey_;

        internal static int prevSoundIndex = -1;
        internal static int currentSoundId;

        internal static readonly NotifyIcon notifyIcon = new() 
        {
            Visible = true,
            Icon = System.Drawing.SystemIcons.Application,
            Text = "Sound Board",
            ContextMenuStrip = new()
        };

        internal static readonly HashSet<string> supportedFormats = new() { ".mp3", ".wav", ".wma", ".wmv", ".aac", ".ogg" };

        internal static Dictionary<string,Keys> keys;

        internal static System.ComponentModel.BindingList<string> items;
    }
}