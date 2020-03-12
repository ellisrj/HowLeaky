
using HowLeaky.CustomAttributes;
using HowLeaky.DataModels;
using System;
using System.Collections.Generic;

namespace HowLeaky
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> argsList = new List<string>(args);

            if(argsList.Contains("-h"))
            {
                PrintHelp();
                return;
            }

            Project p = null;
            int numberOfCores = 4;
            if (args.Length > 0)
            {
                //This will read the XML too...
                p = new Project(args[0].ToString());
            }

            //Move this to later so that the output path argument overrides the Batch Config
            //if (argsList.IndexOf("-O") > 0)
            //{
            //    p.OutputPath = args[argsList.IndexOf("-O") + 1].ToString();
            //}

            if (argsList.IndexOf("-P") > 0)
            {
                numberOfCores = int.Parse(args[argsList.IndexOf("-P") + 1].ToString());
            }

            if (argsList.Contains("-SQL"))
            {
                p.OutputType = OutputType.SQLiteOutput;
            }

            if (argsList.Contains("-CSV"))
            {
                p.OutputType = OutputType.CSVOutput;
            }

            if (argsList.Contains("-M"))
            {
                p.WriteMonthlyData = true;
            }

            if (argsList.Contains("-Y"))
            {
                p.WriteYearlyData = true;
            }

            if (argsList.Contains("-Q"))
            {
                p.QuietOutput = true;
            }

            if (argsList.IndexOf("-B") > 0)
            {
                p.BatchConfigIndex = int.Parse(args[argsList.IndexOf("-B") + 1].ToString());

                //So need to redo the stuff gleaned from XML here....
                //Now adjust the chosen simulations and required outputs as per any selected batch config

                if (p.BatchConfigIndex >= 0)
                {
                    foreach (OutputModels.OutputDataElement ODE in p.OutputDataElements)
                    {
                        if (!p.BatchConfigurations[p.BatchConfigIndex].TimeSeriesOutputs.Contains(ODE.Output.TimeSeriesOutputIdentifier))
                        {
                            ODE.IsSelected = false;
                        }
                    }

                    List<Simulation> simsToDo = new List<Simulation>();
                    foreach(Simulation sim in p.Simulations)
                    if (p.BatchConfigurations[p.BatchConfigIndex].Simulations.Contains(sim.Index))
                    {
                            simsToDo.Add(sim);
                    }


                    p.Simulations = simsToDo;


                    p.OutputPath = p.BatchConfigurations[p.BatchConfigIndex].path;
                }

            }

            //Putting this here makes th -O argument more important than the batch provided path
            if (argsList.IndexOf("-O") > 0)
            {
                p.OutputPath = args[argsList.IndexOf("-O") + 1].ToString();
            }

            p.RunSimulations(numberOfCores);

            Console.WriteLine("Press any key to exit when sims completed...\n");

            if (argsList.Contains("-WAIT"))
            {
                Console.ReadKey();
            } 
        }

        public static void PrintHelp()
        {
            Console.WriteLine("How Leaky Command Line Argumnets:");
            Console.WriteLine("   howleaky.exe <hlkfile.hlk:required>");
            Console.WriteLine("Optional Parameters:");
            Console.WriteLine("    -O <outputpath>        | The default path will be the same as the hlk file unlsess this is set");
            Console.WriteLine("    -P <No. of Processors> | The default is -1, which means HL will leave 1 processor spare. If you want to use specify the number of processors set this");
            Console.WriteLine("    -B <Idx Batch Config>  | The Index number of the Batch Configuration to use");
            Console.WriteLine("    -CSV                   | Output to CSV files. [Default output stream if none selected]");
            Console.WriteLine("    -SQL                   | Output to SQL file");
            Console.WriteLine("    -M                     | Output Monthly summaries (CSV option only) [Not set by Default]");
            Console.WriteLine("    -Y                     | Output Annual summaries (CSV option only) [Not set by Default]");
            Console.WriteLine("    -Q                     | Quiet mode [Not set by Default]");
            Console.WriteLine("");
        }
    }
}
