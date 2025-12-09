using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MultiCarrierManager
{
    public static class FlashWindow
    {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool FlashWindowEx(ref FLASHWINFO pwfi);

        [StructLayout(LayoutKind.Sequential)]
        private struct FLASHWINFO
        {
            public uint cbSize;
            public IntPtr hwnd;
            public uint dwFlags;
            public uint uCount;
            public uint dwTimeout;
        }

        private const uint FLASHW_STOP = 0;
        private const uint FLASHW_CAPTION = 1;
        private const uint FLASHW_TRAY = 2;
        private const uint FLASHW_ALL = 3;
        private const uint FLASHW_TIMER = 4;
        private const uint FLASHW_TIMERNOFG = 12;

        public static bool Flash(Form form, uint count = 0)
        {
            if (form == null || form.IsDisposed) return false;

            FLASHWINFO fi = new FLASHWINFO();
            fi.cbSize = (uint)Marshal.SizeOf(fi);
            fi.hwnd = form.Handle;
            fi.dwFlags = FLASHW_ALL | FLASHW_TIMERNOFG;
            fi.uCount = count;
            fi.dwTimeout = 0;
            return FlashWindowEx(ref fi);
        }

        public static bool FlashUntilFocused(Form form)
        {
            return Flash(form, 0);
        }

        public static bool Stop(Form form)
        {
            if (form == null || form.IsDisposed) return false;

            FLASHWINFO fi = new FLASHWINFO();
            fi.cbSize = (uint)Marshal.SizeOf(fi);
            fi.hwnd = form.Handle;
            fi.dwFlags = FLASHW_STOP;
            fi.uCount = 0;
            fi.dwTimeout = 0;
            return FlashWindowEx(ref fi);
        }
    }
}
