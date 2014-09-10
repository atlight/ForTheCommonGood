using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace ForTheCommonGood
{
    internal static class PlatformSpecific
    {
        // Windows

        [DllImport("user32.dll")]
        private static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, IntPtr dwExtraInfo);
        private const int MOUSEEVENTF_MOVE = 1;

        [DllImport("user32.dll")]
        private static extern IntPtr GetMessageExtraInfo();

        // Cross-platform functions

        internal static void FakeMouseMovement()
        {
            // Windows only
            if (Environment.OSVersion.Platform == PlatformID.Unix || Environment.OSVersion.Platform == PlatformID.MacOSX)
                return;
            mouse_event(MOUSEEVENTF_MOVE, 4, 0, 0, GetMessageExtraInfo());
            mouse_event(MOUSEEVENTF_MOVE, -4, 0, 0, GetMessageExtraInfo());
        }

        internal static bool IsMono()
        {
            return Type.GetType("Mono.Runtime") != null;
        }
    }
}
