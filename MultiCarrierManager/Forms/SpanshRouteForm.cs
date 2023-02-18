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
            
            var values = new Dictionary<string, string>
            {
                { "source", textBox1.Text },
                { "destinations", textBox2.Text },
                // todo: change this to the carrier's used capacity minus tritium in market
                { "capacity_used", "0" },
                { "calculate_starting_fuel", "1" }
            };

            var content = new FormUrlEncodedContent(values);

            Console.WriteLine("Sending...");
            var response = await client.PostAsync("https://spansh.co.uk/api/fleetcarrier/route", content);
            Console.WriteLine(response.StatusCode.ToString());
            var responseString = await response.Content.ReadAsStringAsync();
            Console.WriteLine(responseString);

            label1.Text = "Waiting for route...";


            // Send requests for route
            do {
                System.Threading.Thread.Sleep(3000);
                Console.WriteLine(responseString.Split('"')[3]);
                response = await client.GetAsync("https://spansh.co.uk/api/results/" + responseString.Split('"')[3]);

                responseString = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseString);
            } while (responseString.Split('"')[1] != "error" && (responseString.Split('"')[7] == "started" || responseString.Split('"')[7] == "unstarted"));

            if (responseString.Split('"')[1] == "error") {
                label1.Text = "Invalid systems!";
                button1.Enabled = true;
                return;
            }

            JObject parsed = JObject.Parse(responseString);
            JToken jumps = parsed["result"]["jumps"];
            List<string> destinations = new List<string>();
            
            
            Console.WriteLine(jumps[0]["tritium_in_market"].ToString());
            
            foreach (JToken jump in jumps) {
                destinations.Add(jump["name"].ToString());
            }

            destinations.Remove(destinations[0]);
            
            File.WriteAllLines("CATS\\route.txt", destinations.ToArray());

            Close();
        }

        private void SpanshRouteForm_Load(object sender, EventArgs e) {
            if (Program.settings.GetTritium) {
                label2.Text = "Select carrier for Tritium requirements:";
                comboBox1.Enabled = true;
                
                // Populate combo box with carriers
                string[] files = Directory.GetFiles("carriers");
                foreach (string file in files) {
                    string text = File.ReadAllText(file);
                    JObject carrier = JObject.Parse(text);
                    string name = ConvertHex(carrier["name"]["vanityName"].ToString());
                    comboBox1.Items.Add(name);
                }
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
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            return string.Empty;
        }
    }
}