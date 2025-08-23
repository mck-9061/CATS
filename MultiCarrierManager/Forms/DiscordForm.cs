using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Mime;
using System.Windows.Forms;

namespace MultiCarrierManager {
    public partial class DiscordForm : Form {
        public DiscordForm() {
            InitializeComponent();
        }

        private void DiscordForm_Load(object sender, EventArgs e) {
            
        }

        private List<String> images = new List<string>();
        private string[] optionsLines;
        
        private void DiscordForm_Shown(object sender, EventArgs e) {
            string[] lines = File.ReadAllLines("CATS\\photos.txt");
            foreach (string line in lines) {
                textBox1.Text += line + Environment.NewLine;
                images.Add(line);
            }
            
            optionsLines = File.ReadAllLines("CATS\\settings.txt");
            foreach (string line in optionsLines) {
                if (line.StartsWith("webhook_url=")) textBox2.Text = line.Replace("webhook_url=", "");
            }
            
            singleMessageCheckBox.Checked = Program.settings.SingleDiscordMessage;
        }

        private void button1_Click(object sender, EventArgs e) {
            WebClient client = new WebClient();

            foreach (string imageURL in textBox1.Lines) {
                try {
                    byte[] data = client.DownloadData(imageURL + "?width=144&height=81");
                    imageList1.Images.Add(Image.FromStream(new MemoryStream(data)));
                } catch (Exception ex) {}
            }

            listView1.Items.Clear();
            listView1.LargeImageList = imageList1;

            int i = 0;
            foreach (string imageURL in images) {
                ListViewItem item = new ListViewItem("image", i);
                listView1.Items.Add(item);
                i++;
            }
        }

        private void label2_Click(object sender, EventArgs e) {
            
        }

        private void button2_Click(object sender, EventArgs e) {
            File.WriteAllText("CATS\\photos.txt", textBox1.Text);
            List<String> toWrite = new List<string>();
            
            foreach (string line in optionsLines) {
                if (line.StartsWith("webhook_url")) {
                    toWrite.Add("webhook_url="+textBox2.Text);
                } else toWrite.Add(line);
            }
            
            File.WriteAllLines("CATS\\settings.txt", toWrite);
            
            Program.settings.SetSingleDiscordMessage(singleMessageCheckBox.Checked);
            
            Close();
        }
    }
}