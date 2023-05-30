using System;
using NAudio.Wave;
using System.Windows.Forms;
using System.Linq;

namespace Soundboard
{
    #pragma warning disable CS8618 //Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    internal static class GlobalVariables
    {
        internal static readonly string soundDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\Sounds\";

        internal static DirectSoundOut output;
        internal static Mp3FileReader sound;
        internal static WaveChannel32 waveChannel;
        internal static Guid[] Guids = new Guid[DirectSoundOut.Devices.Count()];

        internal static readonly NotifyIcon notifyIcon = new() 
        { 
            Visible = true,
            Icon = System.Drawing.SystemIcons.Application,
            ContextMenuStrip = new()
        };

        internal static string[] soundFiles;
        internal static string[] prevSoundFiles;
        internal static string prevFileDir;

        internal static bool isRegisteringKey_;

        internal static int selectedIndex = 0;
        internal static int prevSoundIndex = -1;

        internal static System.Collections.Generic.Dictionary<string,Keys> keys;

        internal static System.ComponentModel.BindingList<string> items;
    }
}