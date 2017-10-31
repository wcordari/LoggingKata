using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
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

            var lines = File.ReadAllLines(path);

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

            var locations = lines.Select(line => parser.Parse(line))
                .OrderBy(loc => loc.Location.Longitude)
                .ThenBy(loc => loc.Location.Latitude)
                .ToArray();

          
            ITrackable a = null;
            ITrackable b = null;
            double distance = 0;

            Console.WriteLine("Starting foreach on: " + locations.Length);

            foreach (var locA in locations)
            {
                Logger.Info("Compare all locations");
                var origin = new Coordinate
                {
                    Latitude = locA.Location.Latitude,
                    Longitude = locA.Location.Longitude
                };
                foreach (var locB in locations)
                {
                    Logger.Debug("Checking origin against destination location");
                    var destination = new Coordinate
                    {
                        Latitude = locB.Location.Latitude,
                        Longitude = locB.Location.Longitude 
                    };

                    Logger.Info("getting distance in miles");
                    var nDistance = GeoCalculator.GetDistance(origin, destination);

                    if (!(nDistance > distance)) {continue;}
                    Logger.Info("found the next further apart");
                    a = locA;
                    b = locB;
                    distance = nDistance;
                }
            }
            if (a == null || b == null)
            {
                Logger.Error("failed to find the furthest locations");
                Console.WriteLine("Couldnt find the locations furthest apart");
                Console.ReadLine();
                return;
            }
            Console.WriteLine($"The two Taco Bells that are furthest apart are: {a.Name} and {b.Name} {distance} miles apart");
            Console.ReadLine();
        }
    }
}