using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using RestSharp;

namespace MultiCarrierManager {
    public class Logger {
        private string LogFile;
        private List<string> LogLines;

        public Logger() {
            Random r = new Random();
            LogLines = new List<string>();
            if (!Directory.Exists("logs")) Directory.CreateDirectory("logs");
            LogFile = "logs\\" + DateTime.Now.ToString("dd-MM-yy-hh-mm-ss-") + r.Next(100000) + ".txt";
            File.Create(LogFile).Close();
        }

        public void Log(string l) {
            LogLines.Add("[Event] [" + DateTime.Now.ToString("dd-MM-yy hh:mm:ss") + "] " + l);
            File.WriteAllLines(LogFile, LogLines.ToArray());
        }

        public void LogOutput(string l) {
            Console.WriteLine(l);
            LogLines.Add("[Program Output] [" + DateTime.Now.ToString("dd-MM-yy hh:mm:ss") + "] " + l);
            File.WriteAllLines(LogFile, LogLines.ToArray());
        }
        
        public void LogError(string l) {
            LogLines.Add("[Error] [" + DateTime.Now.ToString("dd-MM-yy hh:mm:ss") + "] " + l);
            File.WriteAllLines(LogFile, LogLines.ToArray());
        }
        
        public void LogCats(string l) {
            LogLines.Add("[CATS Output] [" + DateTime.Now.ToString("dd-MM-yy hh:mm:ss") + "] " + l);
            File.WriteAllLines(LogFile, LogLines.ToArray());
        }
        
        public void Upload() {
            // TODO: remove url before commit
            var client = new RestClient("");
            var request = new RestRequest("upload", Method.Post);
            request.AddFile("uploads", LogFile);
            var response = client.Execute(request);
            //Console.WriteLine(response.Content);
        }
    }
}