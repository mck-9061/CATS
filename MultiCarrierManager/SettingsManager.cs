using System;
using System.Drawing.Drawing2D;
using System.IO;

namespace MultiCarrierManager {
    public class SettingsManager {
        public string[] IniFile;
        public string FileName;
        
        public bool AutoPlot { get; private set; }
        public bool OpenToTraversal { get; private set; }
        public bool GetTritium { get; private set; }
        public bool DisableRefuel { get; private set; }

        public SettingsManager(string file) {
            FileName = file;
            IniFile = File.ReadAllLines(file);

            foreach (string line in IniFile) {
                if (line.StartsWith("auto-plot-jumps")) {
                    AutoPlot = Convert.ToBoolean(line.Split('=')[1]);
                } else if (line.StartsWith("open-to-traversal")) {
                    OpenToTraversal = Convert.ToBoolean(line.Split('=')[1]);
                } else if (line.StartsWith("get-tritium")) {
                    GetTritium = Convert.ToBoolean(line.Split('=')[1]);
                } else if (line.StartsWith("disable-refuel")) {
                    DisableRefuel = Convert.ToBoolean(line.Split('=')[1]);
                }
            }
        }

        private void ReplaceInArray(string setting, bool b) {
            int i = 0;
            foreach (string line in (string[]) IniFile.Clone()) {
                if (line.StartsWith(setting)) {
                    IniFile[i] = setting + "=" + b;
                }

                i++;
            }
            
            File.WriteAllLines(FileName, IniFile);
        }
        
        
        public void SetAutoPlot(bool b) {
            AutoPlot = b;
            ReplaceInArray("auto-plot-jumps", b);
        }
        
        public void SetOpenToTraversal(bool b) {
            OpenToTraversal = b;
            ReplaceInArray("open-to-traversal", b);
        }

        public void SetGetTritium(bool b) {
            GetTritium = b;
            ReplaceInArray("get-tritium", b);
        }
        
        
        public void SetDisableRefuel(bool b) {
            DisableRefuel = b;
            ReplaceInArray("disable-refuel", b);
        }
    }
}