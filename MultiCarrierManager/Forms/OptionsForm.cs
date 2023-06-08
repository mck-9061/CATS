using System;
using System.IO;
using System.Windows.Forms;

namespace MultiCarrierManager {
    public partial class OptionsForm : Form {
        public OptionsForm() {
            InitializeComponent();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e) {
            
        }

        private void button1_Click(object sender, EventArgs e) {
            Program.settings.SetUsageStats(!checkBox1.Checked);
            Program.settings.SetAutoPlot(checkBox2.Checked);
            Program.settings.SetOpenToTraversal(checkBox3.Checked);

            if (checkBox4.Checked && Directory.GetFiles("carriers").Length == 0) {
                MessageBox.Show("You must have at least one carrier set up in the admin interface to use the Tritium requirements feature.");
            } else Program.settings.SetGetTritium(checkBox4.Checked);
            
            
            Program.settings.SetDisableRefuel(checkBox5.Checked);

            if (checkBox6.Checked && !Program.settings.DisableOCR) {
                DialogResult result = MessageBox.Show(
                    "IMPORTANT - By disabling OCR, CATS will assume every jump will take exactly 15 minutes and 10 seconds."+Environment.NewLine+Environment.NewLine
                    +"During peak server load, jump times can be much longer than this."+Environment.NewLine+Environment.NewLine
                    +"Please only disable OCR if you know what you're doing or if CATS is consistently failing to read the jump time."+Environment.NewLine+Environment.NewLine
                    +"Are you sure you want to continue?", "Please read carefully", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result.Equals(DialogResult.Yes)) {
                    Program.settings.SetDisableOCR(checkBox6.Checked);
                }
            } else Program.settings.SetDisableOCR(checkBox6.Checked);
            
            Close();
        }

        private void OptionsForm_Load(object sender, EventArgs e) {
            checkBox1.Checked = !Program.settings.UsageStats;
            checkBox2.Checked = Program.settings.AutoPlot;
            checkBox3.Checked = Program.settings.OpenToTraversal;
            checkBox4.Checked = Program.settings.GetTritium;
            checkBox5.Checked = Program.settings.DisableRefuel;
            checkBox6.Checked = Program.settings.DisableOCR;
        }
    }
}