using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using MultiCarrierManager.CATS;

namespace MultiCarrierManager {
    static class Program {
        public static SettingsManager settings;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        ///
        /// 
        [STAThread]
        static void Main() {
            settings = new SettingsManager("settings/settings.ini");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (settings.OpenToTraversal) {
                Application.Run(new CATSForm());
            } else {
                Application.Run(new Form1());
            }
            
        }
    }
}