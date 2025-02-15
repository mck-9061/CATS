using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace MultiCarrierManager.CATS
{
    public class CatSitter
    {
        // Why rewrite my shit python script when I can just call it from c#? I'm sure this will cause no issues at all in the future!
        // also fuck your naming schemes I think the class name is cute

        private TextBox output;
        private CATSForm form;
        private Process process;
        private Label countdownLabel;
        private Label etaLabel;
        public bool isRunning { get; set; }

        public CatSitter(TextBox output, CATSForm form, Label countdownLabel, Label etaLabel)
        {
            this.output = output;
            this.countdownLabel = countdownLabel;
            this.form = form;
            this.etaLabel = etaLabel;
        }

        public string finalSystem { get; set; }
        private string nextSystem;


        public void run_cmd()
        {
            output.Text = "";
            process = new Process();
            process.StartInfo.FileName = "CATS\\pyinstaller\\TraversalSystem\\TraversalSystem.exe";
            process.StartInfo.WorkingDirectory = "CATS";
            // The old way of executing the Python script, using a Python installation to execute instead of compiling
            // process.StartInfo.FileName = "CATS\\Python39\\python.exe";
            // process.StartInfo.WorkingDirectory = "CATS";
            // process.StartInfo.Arguments = "-u main.py";
            if (!Program.settings.AutoPlot) process.StartInfo.Arguments += " --manual";
            else process.StartInfo.Arguments += " --auto";
            // I cba refactoring - not actually ocr any more
            process.StartInfo.Arguments += " --ocr";
            if (Program.settings.DisableRefuel) process.StartInfo.Arguments += " --nofuel";
            else process.StartInfo.Arguments += " --fuel";
            if (Program.settings.PowerSaving) process.StartInfo.Arguments += " --power-saving";
            else process.StartInfo.Arguments += " --no-ps";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            // Monitor the process for exit
            process.EnableRaisingEvents = true;
            process.Exited += new EventHandler((s, e) =>
            {
                form.stopButton_Click(s, e);
                Program.logger.Log("TraversalExitCode=" + process.ExitCode);
            });
            process.OutputDataReceived += new DataReceivedEventHandler((s2, e2) =>
            {
                // First check if the output is a number, in which case pass it to the countdown clock instead of the console
                try
                {
                    int remaining = Convert.ToInt32(e2.Data);

                    countdownLabel.Text = "Current jump: " + TimeSpan.FromSeconds(remaining).ToString(@"hh\:mm\:ss");
                }
                catch (FormatException)
                {
                    try
                    {
                        string line = e2.Data;
                        output.AppendText(line + Environment.NewLine);
                        if (!line.StartsWith("journal_directory=")) Program.logger.LogCats(line);

                        switch (line)
                        {
                            case "Beginning in 5...":
                                form.Text = $"CATS | En route to {finalSystem} | Next stop: {nextSystem} | Starting up...";
                                break;
                            case string s when s.StartsWith("Next stop"):
                                nextSystem = line.Split(':')[1].Remove(0, 1);
                                form.Text = $"CATS | En route to {finalSystem} | Next stop: {nextSystem} | Navigating menus...";
                                break;
                            case string s when s.StartsWith("Navigation complete"):
                                form.Text = $"CATS | En route to {finalSystem} | Next stop: {nextSystem} | Counting down...";
                                break;
                            case "Jumping!":
                                form.Text = $"CATS | En route to {finalSystem} | Next stop: {nextSystem} | In hyperspace...";
                                break;
                            case "Jump complete!":
                                form.Text = $"CATS | En route to {finalSystem} | Next stop: {nextSystem} | Cooling down...";
                                break;
                            case "Restocking tritium...":
                                form.Text = $"CATS | En route to {finalSystem} | Next stop: {nextSystem} | Restocking tritium...";
                                break;
                            case "Tritium successfully refuelled":
                                form.Text = $"CATS | En route to {finalSystem} | Next stop: {nextSystem} | Cooling down...";
                                break;
                            case "Route complete!":
                                form.Text = "Carrier Administration and Traversal System (CATS)";
                                break;
                            case string s when s.StartsWith("ETA:"):
                                etaLabel.Text = line;
                                break;
                            case string s when s.StartsWith("alert:"):
                                string alert = line.Split(':')[1];
                                MessageBox.Show(alert, "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                break;
                        }
                    }
                    catch (Exception)
                    {
                        Program.logger.LogOutput("Exception while writing to console, possible force CATS process kill");
                    }
                }
            });
            process.Start();
            process.BeginOutputReadLine();
            Program.logger.LogOutput("Traversal System script started");
        }

        public void close()
        {
            try
            {
                process.Kill();
                process.Close();
            }
            catch (Exception) { }
        }
    }
}
