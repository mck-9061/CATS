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
            LogLines.Add(l);
            File.WriteAllLines(LogFile, LogLines.ToArray());
        }

        public void Upload() {
            // todo: remove url before commit
            var client = new RestClient("");
            var request = new RestRequest("upload", Method.Post);
            request.AddFile("uploads", LogFile);
            var response = client.Execute(request);
            //Console.WriteLine(response.Content);
        }
    }
}