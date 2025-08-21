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

            if (checkBox6.Checked && !Program.settings.EfficientRefueling)
            {
                MessageBox.Show("Ensure that your current ship's cargo hold is full of Tritium before beginning.");
            }
            Program.settings.SetEfficientRefueling(checkBox6.Checked);
            Program.settings.SetSquadronCarrier(checkBox7.Checked);

            Close();
        }


        private void OptionsForm_Load(object sender, EventArgs e)
        {
            checkBox2.Checked = Program.settings.AutoPlot;
            checkBox3.Checked = Program.settings.OpenToTraversal;
            checkBox4.Checked = Program.settings.GetTritium;
            checkBox5.Checked = Program.settings.DisableRefuel;
            checkBox1.Checked = Program.settings.PowerSaving;
            checkBox6.Checked = Program.settings.EfficientRefueling;
            checkBox7.Checked = Program.settings.SquadronCarrier;
        }
    }
}
