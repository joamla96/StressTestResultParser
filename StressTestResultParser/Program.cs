using System;
using System.IO;

namespace StressTestResultParser {
    class Program {

        private const int linesPerSection = 44;
        private const string fileName = "result.txt";
        private const string outputPrefix = "output_";

        static void Main(string[] args)
        {
            var file = File.ReadAllLines(fileName);
            var sections = file.Length / linesPerSection;

            var path = outputPrefix + DateTime.Now.Ticks + ".csv";

            File.AppendAllText(path, "Digits,Completed Requests,Concurrency,Time Taken,Time taken pr Request\n");

            int sectionStart = -1;
            foreach (var line in file)
            {
                sectionStart++;
                if (line != @"This is ApacheBench, Version 2.3 <$Revision: 1807734 $>")
                {
                    continue;
                }

                try
                {
                    decimal concurrency = 0;
                    decimal timeTaken = 0;
                    decimal completed = 0;
                    decimal failed = 0;
                    decimal timePerRequest = 0;

                    var tile = file[sectionStart];

                    var digitsString = file[sectionStart + 13];
                    var splita = digitsString.Split(':');
                    var splitb = splita[1].Split('=');
                    var digits = splitb[1];

                    var concurrencyString = file[sectionStart + 16];
                    var timeTakenString = file[sectionStart + 17];
                    var completedString = file[sectionStart + 18];
                    var failedString = file[sectionStart + 19];
                    var timePerRequestString = file[sectionStart + 23];
                    var requestPerSecondString = file[sectionStart + 22]

                    concurrency = StringParser(concurrencyString);
                    timeTaken = StringParser(timeTakenString);
                    completed = StringParser(completedString);
                    failed = StringParser(failedString);
                    timePerRequest = StringParser(timePerRequestString);
                    var rps = StringParser(requestPerSecondString);


                    string outputLine;
                    if (failed != 0)
                    {
                        outputLine = "na,na,na,na,na,na\n";
                    }
                    else
                    {
                        outputLine = string.Format("{0},{1},{2},{3},{4},{5}\n", digits, completed, concurrency, timeTaken, timePerRequest, rps);
                    }

                    File.AppendAllText(path, outputLine);
                } catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                    var outputLine = "na,na,na,na,na,na\n";
                    File.AppendAllText(path, outputLine);
                }
            }

            Console.WriteLine("\nComplete");
            Console.ReadLine();
        }

        private static decimal StringParser(string input)
        {
            var split = input.Split(':');

            var section = split[1].Trim();

            var sectionSplit = section.Split(' ');

            return decimal.Parse(sectionSplit[0]);
        }
    }
}
