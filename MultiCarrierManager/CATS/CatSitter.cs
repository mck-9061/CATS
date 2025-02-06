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
            process.StartInfo.FileName = "CATS\\main.exe";
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

                        if (line == "Beginning in 5...")
                        {
                            form.Text = $"CATS | En route to {finalSystem} | Next stop: {nextSystem} | Starting up...";
                        }
                        else if (line.StartsWith("Next stop"))
                        {
                            nextSystem = line.Split(':')[1].Remove(0, 1);
                            form.Text = $"CATS | En route to {finalSystem} | Next stop: {nextSystem} | Navigating menus...";
                        }
                        else if (line.StartsWith("Navigation complete"))
                        {
                            form.Text = $"CATS | En route to {finalSystem} | Next stop: {nextSystem} | Counting down...";
                        }
                        else if (line == "Jumping!")
                        {
                            form.Text = $"CATS | En route to {finalSystem} | Next stop: {nextSystem} | In hyperspace...";
                        }
                        else if (line == "Jump complete!")
                        {
                            form.Text = $"CATS | En route to {finalSystem} | Next stop: {nextSystem} | Cooling down...";
                        }
                        else if (line == "Restocking tritium...")
                        {
                            form.Text =
                                $"CATS | En route to {finalSystem} | Next stop: {nextSystem} | Restocking tritium...";
                        }
                        else if (line == "Tritium successfully refuelled")
                        {
                            form.Text = $"CATS | En route to {finalSystem} | Next stop: {nextSystem} | Cooling down...";
                        }
                        else if (line == "Route complete!")
                        {
                            form.Text = "Carrier Administration and Traversal System (CATS)";
                        }
                        else if (line.StartsWith("ETA:"))
                        {
                            etaLabel.Text = line;
                        }
                        else if (line.StartsWith("alert:"))
                        {
                            string alert = line.Split(':')[1];
                            MessageBox.Show(alert, "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
