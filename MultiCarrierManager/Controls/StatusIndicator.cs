using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace MultiCarrierManager.Controls
{
    public class StatusIndicator : Panel
    {
        private Timer animationTimer;
        private int pulsePhase;
        private bool isRunning;
        private string statusText = "Ready";
        private StatusState currentState = StatusState.Idle;

        public enum StatusState
        {
            Idle,
            Starting,
            Waiting,
            Plotting,
            Jumping,
            CoolingDown,
            Refueling,
            Complete
        }

        public StatusIndicator()
        {
            DoubleBuffered = true;
            Height = 60;

            animationTimer = new Timer();
            animationTimer.Interval = 50;
            animationTimer.Tick += AnimationTimer_Tick;
        }

        public bool IsRunning
        {
            get => isRunning;
            set
            {
                isRunning = value;
                if (value)
                {
                    animationTimer.Start();
                }
                else
                {
                    animationTimer.Stop();
                    currentState = StatusState.Idle;
                    statusText = "Ready";
                }
                Invalidate();
            }
        }

        public void SetState(StatusState state, string text = null)
        {
            currentState = state;
            statusText = text ?? GetDefaultText(state);
            Invalidate();
        }

        private string GetDefaultText(StatusState state)
        {
            switch (state)
            {
                case StatusState.Starting: return "Starting...";
                case StatusState.Waiting: return "Waiting for jump";
                case StatusState.Plotting: return "Plotting course";
                case StatusState.Jumping: return "In hyperspace";
                case StatusState.CoolingDown: return "Cooling down";
                case StatusState.Refueling: return "Refueling";
                case StatusState.Complete: return "Route complete";
                default: return "Ready";
            }
        }

        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            pulsePhase = (pulsePhase + 1) % 60;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            Color bgColor = DarkTheme.IsEnabled ? DarkTheme.BackgroundMedium : SystemColors.Control;
            Color borderColor = DarkTheme.IsEnabled ? DarkTheme.BorderColor : SystemColors.ControlDark;
            Color textColor = DarkTheme.IsEnabled ? DarkTheme.TextPrimary : SystemColors.ControlText;

            using (SolidBrush bgBrush = new SolidBrush(bgColor))
            {
                g.FillRectangle(bgBrush, ClientRectangle);
            }

            using (Pen borderPen = new Pen(borderColor))
            {
                g.DrawRectangle(borderPen, 0, 0, Width - 1, Height - 1);
            }

            int indicatorSize = 16;
            int indicatorX = 15;
            int indicatorY = (Height - indicatorSize) / 2;

            Color indicatorColor = GetIndicatorColor();

            if (isRunning)
            {
                float pulseScale = 1.0f + (float)Math.Sin(pulsePhase * Math.PI / 30) * 0.15f;
                int pulseSize = (int)(indicatorSize * pulseScale);
                int pulseOffset = (indicatorSize - pulseSize) / 2;

                Color glowColor = Color.FromArgb(80, indicatorColor);
                int glowSize = (int)(indicatorSize * 1.5);
                int glowOffset = (indicatorSize - glowSize) / 2;

                using (SolidBrush glowBrush = new SolidBrush(glowColor))
                {
                    g.FillEllipse(glowBrush, indicatorX + glowOffset, indicatorY + glowOffset, glowSize, glowSize);
                }

                using (SolidBrush indicatorBrush = new SolidBrush(indicatorColor))
                {
                    g.FillEllipse(indicatorBrush, indicatorX + pulseOffset, indicatorY + pulseOffset, pulseSize, pulseSize);
                }

                Color highlightColor = Color.FromArgb(150, Color.White);
                using (SolidBrush highlightBrush = new SolidBrush(highlightColor))
                {
                    g.FillEllipse(highlightBrush, indicatorX + pulseOffset + 3, indicatorY + pulseOffset + 2, pulseSize / 3, pulseSize / 3);
                }
            }
            else
            {
                using (SolidBrush indicatorBrush = new SolidBrush(indicatorColor))
                {
                    g.FillEllipse(indicatorBrush, indicatorX, indicatorY, indicatorSize, indicatorSize);
                }
            }

            using (Font statusFont = new Font("Segoe UI", 11, FontStyle.Bold))
            using (SolidBrush textBrush = new SolidBrush(textColor))
            {
                g.DrawString(statusText, statusFont, textBrush, indicatorX + indicatorSize + 12, indicatorY - 2);
            }

            if (isRunning)
            {
                string activityText = GetActivityText();
                using (Font activityFont = new Font("Segoe UI", 8))
                using (SolidBrush activityBrush = new SolidBrush(DarkTheme.IsEnabled ? DarkTheme.TextSecondary : SystemColors.GrayText))
                {
                    g.DrawString(activityText, activityFont, activityBrush, indicatorX + indicatorSize + 12, indicatorY + 18);
                }
            }
        }

        private Color GetIndicatorColor()
        {
            switch (currentState)
            {
                case StatusState.Starting:
                case StatusState.Plotting:
                    return DarkTheme.AccentOrange;
                case StatusState.Waiting:
                    return DarkTheme.AccentBlue;
                case StatusState.Jumping:
                    return Color.FromArgb(138, 43, 226);
                case StatusState.CoolingDown:
                    return Color.FromArgb(255, 200, 0);
                case StatusState.Refueling:
                    return Color.FromArgb(0, 191, 255);
                case StatusState.Complete:
                    return DarkTheme.SuccessGreen;
                default:
                    return Color.Gray;
            }
        }

        private string GetActivityText()
        {
            switch (currentState)
            {
                case StatusState.Starting:
                    return "Initializing automation...";
                case StatusState.Waiting:
                    int dots = (pulsePhase / 15) % 4;
                    return "Monitoring jump timer" + new string('.', dots);
                case StatusState.Plotting:
                    return "Navigating carrier menus...";
                case StatusState.Jumping:
                    return "Carrier in hyperspace...";
                case StatusState.CoolingDown:
                    return "Waiting for cooldown...";
                case StatusState.Refueling:
                    return "Restocking tritium...";
                case StatusState.Complete:
                    return "Destination reached!";
                default:
                    return "Click Run CATS to begin";
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                animationTimer?.Stop();
                animationTimer?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
