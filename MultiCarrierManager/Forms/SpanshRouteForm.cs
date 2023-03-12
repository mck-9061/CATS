using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace MultiCarrierManager {
    public partial class SpanshRouteForm : Form {
        public static bool isOpen = false;
        private static readonly HttpClient client = new HttpClient();
        
        public SpanshRouteForm() {
            InitializeComponent();
        }

        private void SpanshRouteForm_FormClosing_1(object sender, FormClosingEventArgs e) {
            Hide();
            isOpen = false;
            CATSForm form = new CATSForm();
            form.ShowDialog();
        }

        private async void button1_Click(object sender, EventArgs e) {
            button1.Enabled = false;
            label1.Text = "Sending request...";
            
            int usedCapacity = 0;
            int tritInMarket = 0;

            if (Program.settings.GetTritium) {
                string[] files = Directory.GetFiles("carriers");
                foreach (string file in files) {
                    string text = File.ReadAllText(file);
                    JObject carrier = JObject.Parse(text);
                    string name = ConvertHex(carrier["name"]["vanityName"].ToString()).Trim(Environment.NewLine.ToCharArray());

                    if (name == comboBox1.SelectedItem.ToString()) {
                        foreach (JToken j in carrier["cargo"]) {
                            if (j["commodity"].ToString() == "Tritium") {
                                tritInMarket += Convert.ToInt32(j["qty"]);
                            }
                        }

                        usedCapacity = 25000 - Convert.ToInt32(carrier["capacity"]["freeSpace"]) - tritInMarket;
                        
                        tritInMarket += Convert.ToInt32(carrier["fuel"]);
                        Program.logger.LogOutput(Convert.ToString(tritInMarket));
                    }
                }
            }
            
            var values = new Dictionary<string, string>
            {
                { "source", textBox1.Text },
                { "destinations", textBox2.Text },
                { "capacity_used", usedCapacity.ToString() },
                { "calculate_starting_fuel", "1" }
            };

            var content = new FormUrlEncodedContent(values);

            Program.logger.LogOutput("Sending...");
            var response = await client.PostAsync("https://spansh.co.uk/api/fleetcarrier/route", content);
            Program.logger.LogOutput(response.StatusCode.ToString());
            var responseString = await response.Content.ReadAsStringAsync();
            Program.logger.LogOutput(responseString);

            label1.Text = "Waiting for route...";


            // Send requests for route
            do {
                System.Threading.Thread.Sleep(3000);
                Program.logger.LogOutput(responseString.Split('"')[3]);
                response = await client.GetAsync("https://spansh.co.uk/api/results/" + responseString.Split('"')[3]);

                responseString = await response.Content.ReadAsStringAsync();
                Program.logger.LogOutput(responseString);
            } while (responseString.Split('"')[1] != "error" && (responseString.Split('"')[7] == "started" || responseString.Split('"')[7] == "unstarted"));

            if (responseString.Split('"')[1] == "error") {
                label1.Text = "Invalid systems!";
                button1.Enabled = true;
                return;
            }

            JObject parsed = JObject.Parse(responseString);
            JToken jumps = parsed["result"]["jumps"];
            List<string> destinations = new List<string>();


            int needed = Convert.ToInt32(jumps[0]["tritium_in_market"].ToString()) + Convert.ToInt32(jumps[0]["fuel_in_tank"].ToString());

            if (Program.settings.GetTritium) {
                if (needed > tritInMarket) {
                    MessageBox.Show("Warning: You need " + needed + " Tritium in the carrier to complete this route. You only have " + tritInMarket + " Tritium in the carrier.", "Tritium");
                } else {
                    MessageBox.Show("You have enough Tritium in the carrier to complete this route. You need " + needed + " Tritium in the carrier. You have " + tritInMarket + " Tritium in the carrier.", "Tritium");
                }
            }
            
            foreach (JToken jump in jumps) {
                destinations.Add(jump["name"].ToString());
            }

            destinations.Remove(destinations[0]);
            
            //File.WriteAllLines("CATS\\route.txt", destinations.ToArray());
            // Write to file without newline at the end
            File.WriteAllText("CATS\\route.txt", String.Join(Environment.NewLine, destinations.ToArray()));

            Close();
        }

        private void SpanshRouteForm_Load(object sender, EventArgs e) {
            if (Program.settings.GetTritium) {
                label2.Text = "Select carrier for Tritium requirements:";
                label3.Text = "Note: You may want to update your carrier's stats in the admin panel first.";
                comboBox1.Enabled = true;
                
                // Populate combo box with carriers
                string[] files = Directory.GetFiles("carriers");
                foreach (string file in files) {
                    string text = File.ReadAllText(file);
                    JObject carrier = JObject.Parse(text);
                    string name = ConvertHex(carrier["name"]["vanityName"].ToString());
                    comboBox1.Items.Add(name);
                }
                
                comboBox1.SelectedIndex = 0;
            } else {
                label2.Text = "Turn on Tritium requirements in settings to select carrier.";
                comboBox1.Enabled = false;
            }
        }
        
        public string ConvertHex(String hexString)
        {
            try
            {
                string ascii = string.Empty;

                for (int i = 0; i < hexString.Length; i += 2)
                {
                    String hs = string.Empty;

                    hs   = hexString.Substring(i,2);
                    uint decval =   System.Convert.ToUInt32(hs, 16);
                    char character = System.Convert.ToChar(decval);
                    ascii += character;

                }

                return ascii;
            }
            catch (Exception ex) { Program.logger.LogOutput(ex.Message); }

            return string.Empty;
        }
    }
}