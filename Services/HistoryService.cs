using System;
using System.Collections.Generic;
using System.IO;

namespace NetworkAnalyzer.Services
{
    public class HistoryService
    {
        private readonly string _historyFile = "UrlHistory.txt";

        public void SaveUrlHistory(string url)
        {
            try
            {
                File.AppendAllText(_historyFile, DateTime.Now + " - " + url + Environment.NewLine);
            }
            catch
            {
                
            }
        }

        public List<string> GetUrlHistory()
        {
            if (File.Exists(_historyFile))
            {
                return new List<string>(File.ReadAllLines(_historyFile));
            }
            return new List<string>();
        }
    }
}
