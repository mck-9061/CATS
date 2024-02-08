using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using MultiCarrierManager.CATS;

namespace MultiCarrierManager {
    public partial class CATSForm : Form {
        private CatSitter cats;
        public CATSForm() {
            InitializeComponent();
            this.Shown += new System.EventHandler(this.CATSForm_Shown);
            this.Closed += new System.EventHandler(this.CATSForm_Closed);
        }

        private void CATSForm_Shown(object sender, EventArgs e) {
            cats = new CatSitter(textBox1, this, label1, label4);
            loadFromRouteFile();
            loadSettingsFile();
            populateComboBox();
            Program.logger.Log("TraversalFormLoaded");
        }


        private void CATSForm_Closed(object sender, EventArgs e) {
            cats.close();
        }

        private void label6_Click(object sender, EventArgs e) {
            
        }

        private void label7_Click(object sender, EventArgs e) {
            
        }

        private void populateComboBox() {
            ComboBox box = comboBox1;
            box.Items.Clear();

            box.Items.Add("Before first system in route");
            foreach (string line in textBox2.Text.Split(Environment.NewLine.ToCharArray())) {
                if (line == "" || line == Environment.NewLine) continue;
                box.Items.Add(line);
            }

            box.SelectedIndex = 0;

            if (File.Exists("CATS\\save.txt")) {
                try {
                    int index = Convert.ToInt32(File.ReadAllText("CATS\\save.txt"));
                    if (index == 0) File.Delete("CATS\\save.txt");
                    else box.SelectedIndex = index;
                } catch (Exception) {
                    File.Delete("CATS\\save.txt");
                    box.SelectedIndex = 0;
                }
            }
        }

        private void runButton_Click(object sender, EventArgs e) {
            if (!cats.isRunning) {
                Program.logger.Log("TraversalRunning");
                cats.finalSystem = (string) comboBox1.Items[comboBox1.Items.Count - 1];
                Program.logger.Log("Destination:"+cats.finalSystem);
                
                cats.isRunning = true;
                stopButton.Enabled = true;
                runButton.Enabled = false;
                loadButton.Enabled = false;
                importButton.Enabled = false;
                button2.Enabled = false;
                cats.run_cmd();
            }
        }

        private void stopButton_Click(object sender, EventArgs e) {
            if (cats.isRunning) {
                Program.logger.Log("TraversalStopped");
                cats.isRunning = false;
                stopButton.Enabled = false;
                runButton.Enabled = true;
                loadButton.Enabled = true;
                importButton.Enabled = true;
                button2.Enabled = true;
                cats.close();

                Text = "Carrier Administration and Traversal System (CATS)";
            }
        }

        private void loadFromRouteFile() {
            TextBox box = textBox2;
            textBox2.Text = "";
            string[] lines = File.ReadAllLines("CATS\\route.txt");

            foreach (string l in lines) {
                box.Text = box.Text + l + Environment.NewLine;
            }
            
            Program.logger.Log("RouteLoaded");
        }

        private string webhook;

        private void loadSettingsFile() {
            string[] lines = File.ReadAllLines("CATS\\settings.txt");
            textBox3.Text = "";
            textBox5.Text = "";
            
            foreach (string line in lines) {
                if (line.StartsWith("webhook_url")) {
                    webhook = line.Replace("webhook_url=", "");
                } else if (line.StartsWith("journal_directory")) {
                    textBox5.Text = line.Replace("journal_directory=", "");
                } else if (line.StartsWith("tritium_slot")) {
                    textBox3.Text = line.Replace("tritium_slot=", "");
                }
            }
            
            Program.logger.Log("TraversalSettingsLoaded");
        }

        private void loadButton_Click(object sender, EventArgs e) {
            using (OpenFileDialog dialog = new OpenFileDialog()) {
                dialog.InitialDirectory = Path.Combine(Directory.GetCurrentDirectory(), "CATS\\Default Routes\\");
                dialog.RestoreDirectory = true;
                if (dialog.ShowDialog() == DialogResult.OK) {
                    try {
                        var filePath = dialog.FileName;
                        string[] lines = System.IO.File.ReadAllLines(filePath);
                        File.WriteAllText("CATS\\route.txt", String.Join(Environment.NewLine, lines));
                        loadFromRouteFile();
                    }
                    catch (SecurityException ex) {
                        MessageBox.Show($"Security error.\n\nError message: {ex.Message}\n\n" +
                                        $"Details:\n\n{ex.StackTrace}");
                    }
                }
            }
        }

        private void save() {
            string journal = textBox5.Text;
            string slot = textBox3.Text;
            string route = textBox2.Text;
            int index = comboBox1.SelectedIndex;
            
            string[] oldLines = File.ReadAllLines("CATS\\settings.txt");
            foreach (string line in oldLines) {
                if (line.StartsWith("webhook_url")) {
                    webhook = line.Replace("webhook_url=", "");
                }
            }
            
            string[] lines = new[] { "webhook_url="+webhook, "journal_directory="+journal, "tritium_slot="+slot, "route_file=route.txt" };
            File.WriteAllLines("CATS\\settings.txt", lines);
            File.WriteAllText("CATS\\route.txt", route);
            File.WriteAllText("CATS\\save.txt", Convert.ToString(index));
            
            populateComboBox();
        }

        // Save settings
        private void button1_Click(object sender, EventArgs e) {
            
        }

        private void importButton_Click(object sender, EventArgs e) {
            OpenFileDialog dialog = new OpenFileDialog() {
                Filter = "CSV Files (*.csv)|*.csv"
            };
            if (dialog.ShowDialog() == DialogResult.OK) {
                try
                {
                    var filePath = dialog.FileName;
                    string[] lines = System.IO.File.ReadAllLines(filePath);
                    List<string> systems = new List<string>();

                    int i = 0;
                    
                    foreach (string line in lines) {
                        i++;
                        if (i < 3) continue;
                        systems.Add(line.Split('"')[1]);
                    }
                    
                    
                    
                    File.WriteAllText("CATS\\route.txt", String.Join(Environment.NewLine, systems.ToArray()));
                    
                    loadFromRouteFile();
                }
                catch (SecurityException ex)
                {
                    MessageBox.Show($"Security error.\n\nError message: {ex.Message}\n\n" +
                                    $"Details:\n\n{ex.StackTrace}");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e) {
            if (!SpanshRouteForm.isOpen) {
                Hide();
                SpanshRouteForm.isOpen = true;
                SpanshRouteForm form = new SpanshRouteForm();
                form.ShowDialog();
                Close();
            }
        }

        private void button3_Click(object sender, EventArgs e) {
            Hide();
            Form1 form = new Form1();
            form.ShowDialog();
            Close();
        }

        private void CATSForm_FormClosing(object sender, FormClosingEventArgs e) {
            if (e.CloseReason == CloseReason.UserClosing) {
                Program.logger.Log("End");
                Program.logger.Upload();
            }
        }

        private void OptionsButton_Click(object sender, EventArgs e) {
            OptionsForm form = new OptionsForm();
            form.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e) {
            AboutForm form = new AboutForm();
            form.ShowDialog();
        }

        private void CATSForm_Load(object sender, EventArgs e) {
            
        }

        private void button5_Click(object sender, EventArgs e) {
            DiscordForm form = new DiscordForm();
            form.ShowDialog();
        }

        private void textBox3_Leave(object sender, EventArgs e) {
            save();
        }
    }
}