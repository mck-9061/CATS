using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using MultiCarrierManager.CATS;
using Newtonsoft.Json;
using RestSharp;

namespace MultiCarrierManager {
    static class Program {
        public static SettingsManager settings;
        public static Logger logger;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        ///
        /// 
        [STAThread]
        static void Main() {
            settings = new SettingsManager("settings/settings.ini");
            logger = new Logger();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            logger.Log("Start");
            
            // Check for updates by checking the latest github release
            try {
                var client = new RestClient("https://api.github.com/repos/");
                var request = new RestRequest("mck-9061/CATS/releases/latest");
                var response = client.Execute(request);
                dynamic json = JsonConvert.DeserializeObject(response.Content);
                string tag = json.tag_name;

                List<String> allowedReleases = new List<string>();
                allowedReleases.Add("1.1.1");
                allowedReleases.Add("1.2");
                
                if (!allowedReleases.Contains(tag)) {
                    MessageBox.Show("Update available: " + tag + ". Please download the latest version from GitHub.");
                }
            } catch (Exception e) {
                logger.Log("Error checking for updates: " + e.Message);
            }




            if (settings.OpenToTraversal) {
                Application.Run(new CATSForm());
            } else {
                Application.Run(new Form1());
            }
            
        }
    }
}