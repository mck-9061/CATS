using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace MultiCarrierManager {
    public partial class AboutForm : Form {
        public AboutForm() {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) {
            System.Diagnostics.Process.Start("https://github.com/mck-9061/CATS/issues");
        }

        private int num = 0;

        private void label1_Click(object sender, EventArgs e) {
            num++;

            if (num == 5) {
                label1.Text = "LGBT rights are human rights. Cry about it, tories";
            }
        }
    }
}