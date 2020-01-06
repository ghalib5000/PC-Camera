using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PC_Camera
{
    class Logger
    {
        //static string date = DateTime.Now.ToString("yyyy_MM_dd");
        private readonly string logloc = "";// @"C:\Windows\Temp\Log_" + date + ".log";
        public Logger(string location, string date)
        {
            string t = "Logging started at " + date + Environment.NewLine;// DateTime.Now + Environment.NewLine;
            logloc = location;
           // Console.ForegroundColor = ConsoleColor.White;
           // Console.WriteLine(t);
            File.AppendAllText(logloc, t);
            
        }
        public void Information(string info)
        {
            string data = "IMFORMATION: Work " + info + " Done at " + DateTime.Now + Environment.NewLine;
          //  Console.ForegroundColor = ConsoleColor.White;
           // Console.WriteLine(data);
            File.AppendAllText(logloc, data);
        }
        public void Error(Exception error)
        {
            string data = "ERROR: Error " + error.ToString() + " found  at " + DateTime.Now + Environment.NewLine;
           // Console.ForegroundColor = ConsoleColor.DarkRed;
           // Console.WriteLine(data);
            File.AppendAllText(logloc, data);
            throw (error);
        }
        public void Information(string info, string current_date, bool check)
        {
            string data = "IMFORMATION: Work " + info + " Done at " + current_date + Environment.NewLine;
            //   Console.ForegroundColor = ConsoleColor.White;
            //  Console.WriteLine(data);
            File.AppendAllText(logloc, data);
        }
        public void Information(string info, string current_date)
        {
            string data = "IMFORMATION: " + info +" ..."+current_date + Environment.NewLine;
            //   Console.ForegroundColor = ConsoleColor.White;
            //  Console.WriteLine(data);
            File.AppendAllText(logloc, data);
        }
        public void Error(Exception error, string current_date)
        {
            string data = "ERROR: Error " + error.ToString() + " found  at " + current_date + Environment.NewLine;
           // Console.ForegroundColor = ConsoleColor.DarkRed;
          //  Console.WriteLine(data);
            File.AppendAllText(logloc, data);
            throw (error);
        }
        public bool Exsists()
        {
          return  File.Exists(logloc);
        }

    }
}
