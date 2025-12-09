using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace MultiCarrierManager
{
    public class FocusWatchdog : IDisposable
    {
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

        private Timer watchTimer;
        private bool isMonitoring;
        private bool isPaused;
        private bool hadFocusOnce;
        private string lastState;
        private NotifyIcon notifyIcon;
        private Form parentForm;

        public event EventHandler<FocusLostEventArgs> FocusLost;
        public event EventHandler FocusRestored;

        public bool IsPaused => isPaused;
        public string LastState => lastState;

        public FocusWatchdog(Form parent)
        {
            parentForm = parent;
            watchTimer = new Timer();
            watchTimer.Interval = 500;
            watchTimer.Tick += WatchTimer_Tick;

            notifyIcon = new NotifyIcon();
            notifyIcon.Icon = parent.Icon;
            notifyIcon.BalloonTipTitle = "CATS - Focus Lost";
            notifyIcon.BalloonTipIcon = ToolTipIcon.Warning;
            notifyIcon.Click += NotifyIcon_Click;
        }

        public void StartMonitoring(string currentState = null)
        {
            lastState = currentState;
            isMonitoring = true;
            isPaused = false;
            hadFocusOnce = IsEliteFocused();
            watchTimer.Start();
        }

        public void StopMonitoring()
        {
            isMonitoring = false;
            isPaused = false;
            watchTimer.Stop();
            notifyIcon.Visible = false;
        }

        public void UpdateState(string state)
        {
            lastState = state;
        }

        public void Resume()
        {
            if (isPaused && IsEliteFocused())
            {
                isPaused = false;
                notifyIcon.Visible = false;
                FocusRestored?.Invoke(this, EventArgs.Empty);
            }
        }

        private void WatchTimer_Tick(object sender, EventArgs e)
        {
            if (!isMonitoring) return;

            bool eliteFocused = IsEliteFocused();

            if (eliteFocused)
            {
                hadFocusOnce = true;
            }

            if (!isPaused && !eliteFocused && hadFocusOnce)
            {
                isPaused = true;
                OnFocusLost();
            }
            else if (isPaused && eliteFocused)
            {
                Resume();
            }
        }

        private bool IsEliteFocused()
        {
            IntPtr hwnd = GetForegroundWindow();
            if (hwnd == IntPtr.Zero) return false;

            StringBuilder windowTitle = new StringBuilder(256);
            GetWindowText(hwnd, windowTitle, 256);
            string title = windowTitle.ToString();

            return title.Contains("Elite - Dangerous") ||
                   title.Contains("Elite Dangerous");
        }

        private void OnFocusLost()
        {
            string stateMessage = string.IsNullOrEmpty(lastState)
                ? "Navigation was in progress"
                : $"Paused during: {lastState}";

            notifyIcon.Visible = true;
            notifyIcon.BalloonTipText = $"{stateMessage}\n\nClick here or tab back to Elite Dangerous to resume.";
            notifyIcon.ShowBalloonTip(10000);

            System.Media.SystemSounds.Exclamation.Play();

            if (parentForm.WindowState == FormWindowState.Minimized)
            {
                FlashWindow.Flash(parentForm);
            }

            FocusLost?.Invoke(this, new FocusLostEventArgs(lastState));
        }

        private void NotifyIcon_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "CATS has paused because Elite Dangerous lost focus.\n\n" +
                $"Last state: {lastState ?? "Unknown"}\n\n" +
                "Please tab back into Elite Dangerous to resume navigation.",
                "CATS Paused",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        public void Dispose()
        {
            watchTimer?.Stop();
            watchTimer?.Dispose();
            notifyIcon?.Dispose();
        }
    }

    public class FocusLostEventArgs : EventArgs
    {
        public string LastState { get; }

        public FocusLostEventArgs(string lastState)
        {
            LastState = lastState;
        }
    }
}
