using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeadFirst__ExcusesManagement
{
    class Excuse
    {
        public string Description { get; set; }
        public string Results { get; set; }
        public DateTime LastUsed { get; set; }
        public string ExcusePath { get; set; }

        public Excuse() { }

        public Excuse(string path) { OpenFile(path); }

        public Excuse(string[] filePaths, Random random)
        {
            int index = random.Next(filePaths.Length);
            OpenFile(filePaths[index]);
            ExcusePath = filePaths[index];
        }

        public void OpenFile(string path)
        {
            string[] lines = File.ReadAllLines(path);
            Description = lines[0];
            Results = lines[1];
            LastUsed = Convert.ToDateTime(lines[2]);
        }

        public void Save(string path, string text)
        {
            File.WriteAllText(path, text);
        }
    }
}
