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
        private void DiscordForm_Shown(object sender, EventArgs e) {
            string[] lines = File.ReadAllLines("CATS\\photos.txt");
            foreach (string line in lines) {
                textBox1.Text += line + Environment.NewLine;
                images.Add(line);
            }
        }

        private void button1_Click(object sender, EventArgs e) {
            WebClient client = new WebClient();

            foreach (string imageURL in images) {
                byte[] data = client.DownloadData(imageURL + "?width=144&height=81");
                imageList1.Images.Add(Image.FromStream(new MemoryStream(data)));
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
    }
}