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
            Program.settings.SetAutoPlot(checkBox2.Checked);
            Program.settings.SetOpenToTraversal(checkBox3.Checked);

            if (checkBox4.Checked && Directory.GetFiles("carriers").Length == 0) {
                MessageBox.Show("You must have at least one carrier set up in the admin interface to use the Tritium requirements feature.");
            } else Program.settings.SetGetTritium(checkBox4.Checked);

            if (checkBox1.Checked && !Program.settings.PowerSaving) {
                MessageBox.Show(
                    "For power saving mode to work, make sure to set your Steam launch options properly. In Steam, right-click Elite and select Properties. Type /autorun in the text box at the bottom.");
            }
            
            
            Program.settings.SetDisableRefuel(checkBox5.Checked);
            Program.settings.SetPowerSaving(checkBox1.Checked);
            
            

            Close();
        }

        
        private void OptionsForm_Load(object sender, EventArgs e) {
            checkBox2.Checked = Program.settings.AutoPlot;
            checkBox3.Checked = Program.settings.OpenToTraversal;
            checkBox4.Checked = Program.settings.GetTritium;
            checkBox5.Checked = Program.settings.DisableRefuel;
            checkBox1.Checked = Program.settings.PowerSaving;
        }
    }
}