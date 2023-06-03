using System;
using System.Runtime.InteropServices;

namespace Soundboard
{
    internal static class Hotkey
    {
        [DllImport("user32.dll")]
        private static extern int RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);

        [DllImport("user32.dll")]
        private static extern int UnregisterHotKey(IntPtr hWnd, int id);

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
    }
}