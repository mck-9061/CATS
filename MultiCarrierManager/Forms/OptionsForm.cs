using System;
using System.IO;
using System.Windows.Forms;

namespace MultiCarrierManager
{
    public partial class OptionsForm : Form
    {
        public OptionsForm()
        {
            InitializeComponent();
            DarkTheme.ApplyTheme(this);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked && !Program.settings.PowerSaving)
            {
                MessageBox.Show("For power saving mode to work, make sure to set your Steam launch options properly. In Steam, right-click Elite and select Properties. Type /autorun in the text box at the bottom.");
            }
            Program.settings.SetPowerSaving(checkBox1.Checked);

            Program.settings.SetAutoPlot(checkBox2.Checked);
            Program.settings.SetOpenToTraversal(checkBox3.Checked);

            if (checkBox4.Checked && Directory.GetFiles("carriers").Length == 0)
            {
                MessageBox.Show("You must have at least one carrier set up in the admin interface to use the Tritium requirements feature.");
            }
            else Program.settings.SetGetTritium(checkBox4.Checked);

            Program.settings.SetDisableRefuel(checkBox5.Checked);
            Program.settings.SetRefuelMode(comboBox1.SelectedIndex);
            Program.settings.SetRefuelThreshold(trackBarRefuelThreshold.Value);

            bool darkModeChanged = checkBoxDarkMode.Checked != Program.settings.DarkMode;
            Program.settings.SetDarkMode(checkBoxDarkMode.Checked);
            Program.settings.SetPreInteractionAlert(checkBoxPreInteractionAlert.Checked);

            if (darkModeChanged)
            {
                MessageBox.Show("Please restart CATS for the theme change to take effect.", "Theme Changed", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            Close();
        }


        private void OptionsForm_Load(object sender, EventArgs e)
        {
            checkBox2.Checked = Program.settings.AutoPlot;
            checkBox3.Checked = Program.settings.OpenToTraversal;
            checkBox4.Checked = Program.settings.GetTritium;
            checkBox5.Checked = Program.settings.DisableRefuel;
            checkBox1.Checked = Program.settings.PowerSaving;
            comboBox1.SelectedIndex = Program.settings.RefuelMode;
            checkBoxDarkMode.Checked = Program.settings.DarkMode;
            checkBoxPreInteractionAlert.Checked = Program.settings.PreInteractionAlert;
            trackBarRefuelThreshold.Value = Program.settings.RefuelThreshold;
            labelRefuelThresholdValue.Text = Program.settings.RefuelThreshold.ToString();
            UpdateRefuelThresholdEnabled();
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            UpdateRefuelThresholdEnabled();
        }

        private void trackBarRefuelThreshold_Scroll(object sender, EventArgs e)
        {
            labelRefuelThresholdValue.Text = trackBarRefuelThreshold.Value.ToString();
        }

        private void UpdateRefuelThresholdEnabled()
        {
            bool enabled = !checkBox5.Checked;
            trackBarRefuelThreshold.Enabled = enabled;
            labelRefuelThreshold.Enabled = enabled;
            labelRefuelThresholdValue.Enabled = enabled;
        }
    }
}
