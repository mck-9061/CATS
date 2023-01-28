﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using MultiCarrierManager.ApiTools;
using MultiCarrierManager.CATS;
using Newtonsoft.Json.Linq;
using Timer = System.Timers.Timer;

namespace MultiCarrierManager {
    public partial class Form1 : Form {
        private Array tabLayout;
        public Form1() {
            InitializeComponent();
            this.Shown += new System.EventHandler(this.Form1_Shown);
        }
        List<Tuple<string, JObject>> carriers = new List<Tuple<string, JObject>>();

        private WebClient client = new WebClient();
        

        private void Form1_Shown(object sender, EventArgs e) {
            TabPage template = tabControl1.TabPages[1];
            tabLayout = new Control[template.Controls.Count];
            template.Controls.CopyTo(tabLayout, 0);
            
            tabControl1.TabPages.Remove(template);
            
            init();
        }


        private void init() {
            if (!Directory.Exists("carriers")) Directory.CreateDirectory("carriers");

            string[] files = Directory.GetFiles("carriers");

            int carrierCount = 0;

            int i = -1;

            foreach (TabPage p in tabControl1.TabPages) {
                i++;
                if (i == 0) continue;
                tabControl1.TabPages.Remove(p);
            }
            
            carriers.Clear();
            
            listView1.Items.Clear();

            long totalWorth = 0;
            
            foreach (String file in files) {
                carrierCount++;
                string text = File.ReadAllText(file);
                JObject carrier = JObject.Parse(text);
                string name = ConvertHex(carrier["name"]["vanityName"].ToString());
                string cmdrName = file.Split('\\').Last().Replace(".json", "");
                
                carriers.Add(Tuple.Create(cmdrName, carrier));

                TabPage page = new TabPage();
                page.Text = name;
                
                tabControl1.TabPages.Add(page);
                
                JObject cmdr = JObject.Parse(File.ReadAllText("carriers/profiles/"+cmdrName+".json"));
                totalWorth += GetCommanderValue(cmdr, carrier);
            }

            if (carrierCount != 0) {
                label1.Text = carrierCount + " carrier(s) have been added.";
            }

            label2.Text = "Your total net worth: " + totalWorth.ToString("N0") + " CR";
        }

        private void label2_Click(object sender, EventArgs e) { }
        
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

        private void tabControl1_Selected(Object sender, TabControlEventArgs e) {
            if (e.TabPageIndex == 0) {
                Text = "ED Multi Carrier Manager";
                return;
            }
            e.TabPage.Controls.AddRange((Control[]) tabLayout);
            
            // Update controls with carrier's information
            JObject carrier = null;
            string owner = null;
            foreach (Tuple<string, JObject> t in carriers) {
                JObject c = t.Item2;
                string name = ConvertHex(c["name"]["vanityName"].ToString());
                if (name == e.TabPage.Text) {
                    carrier = c;
                    owner = t.Item1;
                    break;
                }
            }
            
            if (carrier == null) return;
            
            nameLabel1.Text = "Name: "+ConvertHex(carrier["name"]["vanityName"].ToString());
            callsignLabel1.Text = "Callsign: "+carrier["name"]["callsign"].ToString();
            systemLabel1.Text = "Current system: "+carrier["currentStarSystem"].ToString();
            creditsLabel1.Text = "Credits: " + long.Parse(carrier["balance"].ToString()).ToString("N0") + " CR";
            fuelLabel1.Text = "Fuel: " + carrier["fuel"].ToString() + "t";
            accessLabel1.Text = "Docking access: " + carrier["dockingAccess"].ToString();
            spaceLabel1.Text = "Free space: " + carrier["capacity"]["freeSpace"].ToString();

            Text = "ED Multi Carrier Manager - [" + carrier["name"]["callsign"].ToString() + "] " + ConvertHex(carrier["name"]["vanityName"].ToString()) + " - " + owner;
            
            
            // Available services
            foreach (ListViewItem item in servicesList1.Items) {
                string compare = item.Text.ToLower().Replace(" ", "");
                if (compare == "universalcartographics") compare = "exploration";
                
                ListViewItem.ListViewSubItem newItem = new ListViewItem.ListViewSubItem();
                newItem.Text = carrier["market"]["services"][compare].ToString();
                item.SubItems[1] = newItem;
            }
            
            
            // Cargo
            List<Tuple<string, int, long>> cargo = new List<Tuple<string, int, long>>();

            foreach (JToken token in carrier["cargo"]) {
                string name = token["locName"].ToString();
                int qty = int.Parse(token["qty"].ToString());
                long value = long.Parse(token["value"].ToString());

                Tuple<string, int, long> tuple = null;
                foreach (Tuple<string, int, long> t in cargo) {
                    if (name == t.Item1) {
                        tuple = t;
                        break;
                    }
                }

                if (tuple == null) {
                    tuple = Tuple.Create<string, int, long>(name, 0, 0);
                }
                else cargo.Remove(tuple);

                tuple = Tuple.Create(name, tuple.Item2 + qty, tuple.Item3 + value);
                cargo.Add(tuple);
            }
            
            // Display cargo
            commodityList1.Items.Clear();
            foreach (Tuple<string, int, long> tuple in cargo) {
                ListViewItem i = new ListViewItem();
                i.Text = tuple.Item1;

                ListViewItem.ListViewSubItem qty = new ListViewItem.ListViewSubItem();
                qty.Text = tuple.Item2.ToString("N0");
                
                ListViewItem.ListViewSubItem value = new ListViewItem.ListViewSubItem();
                value.Text = tuple.Item3.ToString("N0");

                i.SubItems.Add(qty);
                i.SubItems.Add(value);


                commodityList1.Items.Add(i);
            }
            
            
            // Sales
            sellList1.Items.Clear();
            foreach (JToken token in carrier["orders"]["commodities"]["sales"]) {
                string name = token["name"].ToString();
                string stock = token["stock"].ToString();
                string price = token["price"].ToString();
                string blackmarket = token["blackmarket"].ToString();


                ListViewItem item = new ListViewItem();
                item.Text = name;
                item.SubItems.Add(stock);
                item.SubItems.Add(price);
                item.SubItems.Add(blackmarket);

                sellList1.Items.Add(item);
            }
            
            // Purchases
            buyList1.Items.Clear();
            foreach (JToken token in carrier["orders"]["commodities"]["purchases"]) {
                string name = token["name"].ToString();
                string total = token["total"].ToString();
                string outstanding = token["outstanding"].ToString();
                string price = token["price"].ToString();
                string blackmarket = token["blackmarket"].ToString();


                long totalCredits = long.Parse(price) * long.Parse(total);


                ListViewItem item = new ListViewItem();
                item.Text = name;
                item.SubItems.Add(total);
                item.SubItems.Add(outstanding);
                item.SubItems.Add(price);
                item.SubItems.Add(totalCredits.ToString("N0"));
                item.SubItems.Add(blackmarket);

                buyList1.Items.Add(item);
            }
            
            
            
        }

        private bool canSend = true;
        private void onTimedEvent(Object source, ElapsedEventArgs e) {
            canSend = true;
        }

        private void refreshButton_Click(object sender, EventArgs e) {
            if (!canSend) {
                statusLabel.Text = "Please don't spam the API. Give it a few minutes and try again";
                return;
            }
            
            
            statusLabel.Text = "Updating...";
            foreach (Tuple<string, JObject> carrier in carriers) {
                string cmdr = carrier.Item1;
                
                OAuth2 auth = OAuth2.Load(cmdr);
                
                if (auth == null || !auth.Refresh())
                {
                    statusLabel.Text = "Please re-authorize " + cmdr;
                    var req = OAuth2.Authorize();
                    Console.WriteLine(req.AuthURL);
                    auth = req.GetAuth();
                }
            
                var capi = new CAPI(auth);
                JObject c = null;
                JObject p = null;
                
                
                try {
                    c = capi.GetCarrier();
                    p = capi.GetProfile();
                }
                catch (WebException exception) {
                    statusLabel.Text = "Please re-authorize " + cmdr;
                    var req = OAuth2.Authorize();
                    Console.WriteLine(req.AuthURL);
                    auth = req.GetAuth();
                    
                    capi = new CAPI(auth);
                    c = capi.GetCarrier();
                }
                
                
                auth.Save(cmdr);
                
                File.WriteAllText("carriers/"+cmdr+".json", c.ToString(Newtonsoft.Json.Formatting.Indented));
                File.WriteAllText("carriers/profiles/"+cmdr+".json", p.ToString(Newtonsoft.Json.Formatting.Indented));
                statusLabel.Text = "Updated " + cmdr + "...";
            }

            statusLabel.Text = "Refreshing...";
            init();

            canSend = false;
            Timer timer = new Timer(300000);
            timer.Elapsed += onTimedEvent;
            timer.AutoReset = false;
            timer.Enabled = true;


            statusLabel.Text = "Done";
        }

        private void AddCarrierButton_Click(object sender, EventArgs e) {
            statusLabel.Text = "Please login to Frontier...";

            OAuth2 auth = null;
            var req = OAuth2.Authorize();
            Console.WriteLine(req.AuthURL);
            auth = req.GetAuth();
            
            var capi = new CAPI(auth);
            var profile = capi.GetProfile();

            string cmdr_name = profile["commander"]["name"].ToString();
            auth.Save(cmdr_name);

            var c = capi.GetCarrier();
            var p = capi.GetProfile();
            File.WriteAllText("carriers/"+cmdr_name+".json", c.ToString(Newtonsoft.Json.Formatting.Indented));
            File.WriteAllText("carriers/profiles/"+cmdr_name+".json", p.ToString(Newtonsoft.Json.Formatting.Indented));
            
            // Send usage stats
            // TODO: URL
            string url = "";

            var data = new NameValueCollection();

            string name = cmdr_name;
            if (!Program.settings.UsageStats) name = "Anonymous";
            
            data["content"] = "New commander is using CATS: " + name;

            var response = client.UploadValues(url, "POST", data);
            Console.WriteLine(Encoding.UTF8.GetString(response));
            
            init();
            statusLabel.Text = "Done";
        }
        

        private void catsButton_Click(object sender, EventArgs e) {
            Hide();
            CATSForm form = new CATSForm();
            form.ShowDialog();
            Close();
        }

        private long GetCommanderValue(JObject cmdr, JObject carrier) {
            ListViewItem item = new ListViewItem();
            Console.WriteLine(cmdr["commander"]["name"].ToString());
            item.Text = cmdr["commander"]["name"].ToString();
            
            // Start at 5bil (carrier cost)
            long value = 5000000000;
            
            // Add cmdr's liquid credits and carrier's liquid credits
            value += long.Parse(cmdr["commander"]["credits"].ToString());
            item.SubItems.Add(long.Parse(cmdr["commander"]["credits"].ToString()).ToString("N0"));
            value += long.Parse(carrier["balance"].ToString());
            item.SubItems.Add(long.Parse(carrier["balance"].ToString()).ToString("N0"));

            long shipValue = 0;
            // Add value of stored ships
            foreach (JToken token in cmdr["ships"]) {
                //Console.Out.WriteLine(token.ToString());

                try {
                    // why the fuck are some of them different formats???
                    // frontier?? what in the name of actual fuck are you doing??????????
                    // qwedfwasdfsdfsfwsdfdsfsdfvsdvsdvsdvsdv
                    
                    if (long.Parse(token["value"]["total"].ToString()) < 0) continue;

                    value += long.Parse(token["value"]["total"].ToString());
                    shipValue += long.Parse(token["value"]["total"].ToString());
                }
                catch (Exception e) {
                    foreach (JToken t in token) {
                        // this is seriously fucking stupid
                        if (long.Parse(t["value"]["total"].ToString()) < 0) continue;
                        
                        value += long.Parse(t["value"]["total"].ToString());
                        shipValue += long.Parse(t["value"]["total"].ToString());
                        long.Parse(t["value"]["total"].ToString());
                    }
                }
            }
            
            item.SubItems.Add(shipValue.ToString("N0"));

            long cargoValue = 0;
            
            // Add value of carrier cargo
            value += long.Parse(carrier["marketFinances"]["cargoTotalValue"].ToString());
            cargoValue += long.Parse(carrier["marketFinances"]["cargoTotalValue"].ToString());
            value += long.Parse(carrier["blackmarketFinances"]["cargoTotalValue"].ToString());
            cargoValue += long.Parse(carrier["blackmarketFinances"]["cargoTotalValue"].ToString());
            
            item.SubItems.Add(cargoValue.ToString("N0"));


            long carrierValue = 5000000000;
            // Add value of carrier services
            foreach (JToken token in carrier["servicesCrew"]) {
                try {
                    value += long.Parse(token["crewMember"]["hiringPrice"].ToString());
                    carrierValue += long.Parse(token["crewMember"]["hiringPrice"].ToString());
                }
                catch (Exception e) {
                    foreach (JToken t in token) {
                        // again, really fucking stupid
                        value += long.Parse(t["crewMember"]["hiringPrice"].ToString());
                        carrierValue += long.Parse(t["crewMember"]["hiringPrice"].ToString());
                    }
                }
            }

            item.SubItems.Add(carrierValue.ToString("N0"));
            item.SubItems.Add(value.ToString("N0"));

            listView1.Items.Add(item);
            

            return value;
        }

        private void button1_Click(object sender, EventArgs e) {
            DialogResult result = MessageBox.Show(
                $"CATS sends usage statistics to the developer by default, which you can opt out of " +
                $"if you want. {Environment.NewLine}{Environment.NewLine}Do you want to opt out?", "Privacy",
                MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes) {
                MessageBox.Show(
                    "You have opted out of usage statistics. You can opt in again by opening this again and clicking No.");
                Program.settings.SetUsageStats(false);
            }
            else {
                Program.settings.SetUsageStats(true);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e) {
            
        }
    }
}