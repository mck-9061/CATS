using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace MultiCarrierManager
{
    public class AlertOverlay : Form
    {
        private Timer fadeTimer;
        private Timer pulseTimer;
        private Timer countdownTimer;
        private float opacity = 0f;
        private bool fadingIn = true;
        private int pulsePhase = 0;
        private string alertTitle;
        private string alertMessage;
        private Action onDismiss;
        private int remainingSeconds;
        private int autoCloseSeconds;

        public AlertOverlay(string title, string message, int displaySeconds = 5, Action onDismissCallback = null)
        {
            alertTitle = title;
            alertMessage = message;
            autoCloseSeconds = displaySeconds;
            remainingSeconds = displaySeconds;
            onDismiss = onDismissCallback;

            InitializeForm();
            InitializeTimers();
        }

        private void InitializeForm()
        {
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.Manual;
            ShowInTaskbar = false;
            TopMost = true;
            BackColor = Color.Black;
            Opacity = 0;
            DoubleBuffered = true;

            Screen activeScreen = Screen.FromPoint(Cursor.Position);
            Bounds = activeScreen.Bounds;

            KeyPreview = true;
            KeyDown += (s, e) => Dismiss();
            MouseDown += (s, e) => Dismiss();
        }

        private void InitializeTimers()
        {
            fadeTimer = new Timer { Interval = 16 };
            fadeTimer.Tick += FadeTimer_Tick;

            pulseTimer = new Timer { Interval = 50 };
            pulseTimer.Tick += PulseTimer_Tick;

            countdownTimer = new Timer { Interval = 1000 };
            countdownTimer.Tick += CountdownTimer_Tick;
        }

        private void CountdownTimer_Tick(object sender, EventArgs e)
        {
            remainingSeconds--;
            Invalidate();
            if (remainingSeconds <= 0)
            {
                countdownTimer.Stop();
                Dismiss();
            }
        }

        public void ShowAlert()
        {
            Show();
            fadeTimer.Start();
            pulseTimer.Start();
            countdownTimer.Start();
        }

        private void FadeTimer_Tick(object sender, EventArgs e)
        {
            if (fadingIn)
            {
                opacity += 0.08f;
                if (opacity >= 0.85f)
                {
                    opacity = 0.85f;
                    fadingIn = false;
                    fadeTimer.Stop();
                }
            }
            else
            {
                opacity -= 0.05f;
                if (opacity <= 0)
                {
                    opacity = 0;
                    fadeTimer.Stop();
                    Close();
                    return;
                }
            }
            Opacity = opacity;
        }

        private void PulseTimer_Tick(object sender, EventArgs e)
        {
            pulsePhase = (pulsePhase + 1) % 60;
            Invalidate();
        }

        private bool isDismissing = false;

        private void Dismiss()
        {
            if (isDismissing)
            {
                return;
            }
            isDismissing = true;

            countdownTimer.Stop();
            pulseTimer.Stop();

            onDismiss?.Invoke();

            fadingIn = false;
            fadeTimer.Start();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            float pulse = (float)(Math.Sin(pulsePhase * Math.PI / 30) * 0.3 + 0.7);
            Color borderColor = Color.FromArgb((int)(255 * pulse), 255, 140, 0);

            int boxWidth = 500;
            int boxHeight = 230;
            int boxX = (Width - boxWidth) / 2;
            int boxY = (Height - boxHeight) / 2;

            Rectangle boxRect = new Rectangle(boxX, boxY, boxWidth, boxHeight);

            using (SolidBrush bgBrush = new SolidBrush(Color.FromArgb(40, 40, 45)))
            {
                g.FillRectangle(bgBrush, boxRect);
            }

            for (int i = 3; i >= 1; i--)
            {
                using (Pen glowPen = new Pen(Color.FromArgb((int)(60 * pulse / i), borderColor), 2 + i * 2))
                {
                    g.DrawRectangle(glowPen, boxX - i * 2, boxY - i * 2, boxWidth + i * 4, boxHeight + i * 4);
                }
            }

            using (Pen borderPen = new Pen(borderColor, 3))
            {
                g.DrawRectangle(borderPen, boxRect);
            }

            int warningSize = 50;
            int warningX = boxX + (boxWidth - warningSize) / 2;
            int warningY = boxY + 20;

            using (Pen warningPen = new Pen(borderColor, 3))
            {
                Point[] triangle = new Point[]
                {
                    new Point(warningX + warningSize / 2, warningY),
                    new Point(warningX, warningY + warningSize),
                    new Point(warningX + warningSize, warningY + warningSize)
                };
                g.DrawPolygon(warningPen, triangle);

                using (Font exclamationFont = new Font("Segoe UI", 24, FontStyle.Bold))
                using (SolidBrush exclamationBrush = new SolidBrush(borderColor))
                {
                    StringFormat sf = new StringFormat
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center
                    };
                    g.DrawString("!", exclamationFont, exclamationBrush,
                        new RectangleF(warningX, warningY + 5, warningSize, warningSize), sf);
                }
            }

            using (Font titleFont = new Font("Segoe UI", 18, FontStyle.Bold))
            using (SolidBrush titleBrush = new SolidBrush(Color.White))
            {
                StringFormat sf = new StringFormat { Alignment = StringAlignment.Center };
                g.DrawString(alertTitle, titleFont, titleBrush,
                    new RectangleF(boxX, boxY + 80, boxWidth, 30), sf);
            }

            using (Font messageFont = new Font("Segoe UI", 12))
            using (SolidBrush messageBrush = new SolidBrush(Color.FromArgb(200, 200, 200)))
            {
                StringFormat sf = new StringFormat { Alignment = StringAlignment.Center };
                g.DrawString(alertMessage, messageFont, messageBrush,
                    new RectangleF(boxX + 20, boxY + 115, boxWidth - 40, 50), sf);
            }

            using (Font countdownFont = new Font("Segoe UI", 14, FontStyle.Bold))
            using (SolidBrush countdownBrush = new SolidBrush(borderColor))
            {
                StringFormat sf = new StringFormat { Alignment = StringAlignment.Center };
                g.DrawString($"{remainingSeconds}s", countdownFont, countdownBrush,
                    new RectangleF(boxX, boxY + boxHeight - 60, boxWidth, 25), sf);
            }

            using (Font dismissFont = new Font("Segoe UI", 9))
            using (SolidBrush dismissBrush = new SolidBrush(Color.FromArgb(120, 120, 120)))
            {
                StringFormat sf = new StringFormat { Alignment = StringAlignment.Center };
                g.DrawString("Press any key or click to dismiss", dismissFont, dismissBrush,
                    new RectangleF(boxX, boxY + boxHeight - 25, boxWidth, 20), sf);
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            fadeTimer?.Stop();
            fadeTimer?.Dispose();
            pulseTimer?.Stop();
            pulseTimer?.Dispose();
            countdownTimer?.Stop();
            countdownTimer?.Dispose();
            base.OnFormClosing(e);
        }

        
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            Activate();
            BringToFront();
            Focus();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            Dismiss();
            return true;
        }
    }
}
