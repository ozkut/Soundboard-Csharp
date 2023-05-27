﻿using System;
using NAudio.Wave;
using System.Windows.Forms;

namespace Soundboard
{
    internal static class GlobalVariables
    {
        public static readonly string soundDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\Sounds\";

        public static WaveOut output;
        public static Mp3FileReader sound;

        public static readonly NotifyIcon notifyIcon = new() { Visible = true, Icon = System.Drawing.SystemIcons.Application, ContextMenuStrip = new() };

        public static string[] soundFiles;
        public static string prevFileDir;

        public static bool isRegisteringKey_;

        public static int selectedIndex = 0;

        public static System.Collections.Generic.Dictionary<string, Keys> keys;

        public static System.ComponentModel.BindingList<string> items;
    }
}