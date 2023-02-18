using System;
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
            Program.settings.SetGetTritium(checkBox4.Checked);
            Close();
        }
    }
}