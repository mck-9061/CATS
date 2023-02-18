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
            Close();
        }

        private void OptionsForm_Load(object sender, EventArgs e) {
            checkBox1.Checked = !Program.settings.UsageStats;
            checkBox2.Checked = Program.settings.AutoPlot;
            checkBox3.Checked = Program.settings.OpenToTraversal;
            checkBox4.Checked = Program.settings.GetTritium;
        }
    }
}