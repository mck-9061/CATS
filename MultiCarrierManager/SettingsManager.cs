using System;
using System.IO;

namespace MultiCarrierManager
{
    public class SettingsManager
    {
        public string[] IniFile;
        public string FileName;

        public bool AutoPlot { get; private set; }
        public bool OpenToTraversal { get; private set; }
        public bool GetTritium { get; private set; }
        public bool DisableRefuel { get; private set; }
        public bool PowerSaving { get; private set; }
        public int RefuelMode { get; private set; }
        public bool SingleDiscordMessage { get; private set; }
        public bool DarkMode { get; private set; }
        public bool PreInteractionAlert { get; private set; }
        public int RefuelThreshold { get; private set; }

        public SettingsManager(string file)
        {
            FileName = file;
            IniFile = File.ReadAllLines(file);

            DarkMode = true;
            PreInteractionAlert = true;
            RefuelThreshold = 200;

            foreach (string line in IniFile)
            {
                if (line.StartsWith("auto-plot-jumps"))
                {
                    AutoPlot = Convert.ToBoolean(line.Split('=')[1]);
                }
                else if (line.StartsWith("open-to-traversal"))
                {
                    OpenToTraversal = Convert.ToBoolean(line.Split('=')[1]);
                }
                else if (line.StartsWith("get-tritium"))
                {
                    GetTritium = Convert.ToBoolean(line.Split('=')[1]);
                }
                else if (line.StartsWith("disable-refuel"))
                {
                    DisableRefuel = Convert.ToBoolean(line.Split('=')[1]);
                }
                else if (line.StartsWith("power-saving"))
                {
                    PowerSaving = Convert.ToBoolean(line.Split('=')[1]);
                }
                else if (line.StartsWith("squadron-carrier"))
                {
                    RefuelMode = Convert.ToInt32(line.Split('=')[1]);
                }
                else if (line.StartsWith("single-discord-message"))
                {
                    SingleDiscordMessage = Convert.ToBoolean(line.Split('=')[1]);
                }
                else if (line.StartsWith("dark-mode"))
                {
                    DarkMode = Convert.ToBoolean(line.Split('=')[1]);
                }
                else if (line.StartsWith("pre-interaction-alert"))
                {
                    PreInteractionAlert = Convert.ToBoolean(line.Split('=')[1]);
                }
                else if (line.StartsWith("refuel-threshold"))
                {
                    RefuelThreshold = Convert.ToInt32(line.Split('=')[1]);
                }
            }
        }

        private void ReplaceInArray(string setting, bool b)
        {
            int i = 0;
            foreach (string line in (string[])IniFile.Clone())
            {
                if (line.StartsWith(setting))
                {
                    IniFile[i] = setting + "=" + b;
                }

                i++;
            }

            File.WriteAllLines(FileName, IniFile);
        }
        
        private void ReplaceInArray(string setting, int i)
        {
            int j = 0;
            foreach (string line in (string[])IniFile.Clone())
            {
                if (line.StartsWith(setting))
                {
                    IniFile[j] = setting + "=" + Convert.ToString(i);
                }

                j++;
            }

            File.WriteAllLines(FileName, IniFile);
        }


        public void SetAutoPlot(bool b)
        {
            AutoPlot = b;
            ReplaceInArray("auto-plot-jumps", b);
        }

        public void SetOpenToTraversal(bool b)
        {
            OpenToTraversal = b;
            ReplaceInArray("open-to-traversal", b);
        }

        public void SetGetTritium(bool b)
        {
            GetTritium = b;
            ReplaceInArray("get-tritium", b);
        }


        public void SetDisableRefuel(bool b)
        {
            DisableRefuel = b;
            ReplaceInArray("disable-refuel", b);
        }

        public void SetPowerSaving(bool b)
        {
            PowerSaving = b;
            ReplaceInArray("power-saving", b);
        }

        public void SetRefuelMode(int i)
        {
            RefuelMode = i;
            ReplaceInArray("refuel-mode", i);
        }

        public void SetSingleDiscordMessage(bool b)
        {
            SingleDiscordMessage = b;
            ReplaceInArray("single-discord-message", b);
        }

        public void SetDarkMode(bool b)
        {
            DarkMode = b;
            ReplaceInArray("dark-mode", b);
        }

        public void SetPreInteractionAlert(bool b)
        {
            PreInteractionAlert = b;
            ReplaceInArray("pre-interaction-alert", b);
        }

        public void SetRefuelThreshold(int i)
        {
            RefuelThreshold = i;
            ReplaceInArray("refuel-threshold", i);
        }
    }
}
