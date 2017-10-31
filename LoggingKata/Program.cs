using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using System.IO;
using Geolocation;

namespace LoggingKata
{
    class Program
    {
        //Why do you think we use ILog?
        private static readonly ILog Logger =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static void Main(string[] args)
        {
            
            var path = Environment.CurrentDirectory + "\\Taco_Bell-US-AL-Alabama.csv";
            if (path.Length == 0)
            {
                Console.WriteLine("You must provide a filename as an argument");
                Logger.Fatal("Cannot import without filename specified as an argument");
                return;
            }

            Logger.Info("Log initialized");
            Logger.Info("Grabbing from your path: " + path);

            var lines = File.ReadAllLines(args[0]);

            switch (lines.Length)
            {
                case 0:
                    Logger.Error("No location to check. Must have at least one location.");
                    break;
                case 1:
                    Logger.Warn("only one location provided. Must have two to perform a check.");
                    break;
            }

            var parser = new TacoParser();
            Logger.Debug("Initialized our parser");

            var locations = lines.Select(line => parser.Parse(line)).ToList();

            //TODO:  Find the two TacoBells in Alabama that are the furthurest from one another.
            //HINT:  You'll need two nested forloops
            foreach (var line in lines)
            {
                double distance = GeoCalculator.GetDistance
            }
        }
    }
}