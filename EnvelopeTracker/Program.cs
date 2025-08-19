using System;
using System.IO;

namespace EnvelopeTracker
{
    internal class Program
    {
        #region VARIABLES
        public static string AppDirectory = Environment.GetEnvironmentVariable("onedriveconsumer") + "\\documents\\EnvelopeTracker\\";  //path to save and read from
        public static string EnvSave = AppDirectory + "Envelopes.csv";  //save file location
        public static bool[] IsFilled = new bool[101];  //envelope status 
        public static int TotalSaved, FilledCount;
        #endregion

        static void Main()
        {
            Console.Title = "100 Envelopes Savings Tracker";  //console window title
            Console.ForegroundColor = ConsoleColor.White;  //text color for console

            LoadFile();
            GetStats(false);
            
            while (true)
            {
                MainMenu(true);
            }
        }

        static void CheckEnvelopes()  //prints out empty and full envelopes
        {
            switch (FilledCount)
            {
                case 0:
                    Console.WriteLine("All envelopes are empty.  Start Saving!  \n");
                    return;
                    break;
                case 100:
                    Console.WriteLine("All envelopes are filled, Great Job!\n");
                    return;
                    break;
            }

            Console.WriteLine("\n\n100 Envelope Savings Challenge - Envelope Status\n");
            int Printed = 0;  //print output in rows of 10
            Console.WriteLine("FILLED ENVELOPES:  ");
            for (int i = 1; i < 101; i++)  //loop IsFilled Array, True = filled
            {
                if (IsFilled[i] == true )
                {
                    Console.Write(i.ToString().PadLeft(3) + "  ");
                    Printed++;
                    if (Printed % 10 == 0)
                    {
                        Printed = 0;
                        Console.WriteLine();
                    }
                }
            }

            Printed = 0;  
            Console.WriteLine("\n\nEMPTY ENVELOPES:");

            for (int i = 1; i < 101; i++)
            {
                if (IsFilled[i] == false )
                {
                    Console.Write(i.ToString().PadLeft(3) + "  ");
                    Printed++;
                    if ( (Printed % 10) == 0)
                    {
                        Printed = 0;
                        Console.WriteLine();
                    }
                }
            }
        }

        static void SaveToFile()  //save status to file
        {
            Directory.CreateDirectory(AppDirectory);  //make sure directory exists before writing
            StreamWriter SW = new System.IO.StreamWriter(EnvSave);  
            
            for (int i = 1;i <101; i++)
            {
                SW.WriteLine(i + "," + IsFilled[i]);  //write to file, csv format
            }
            SW.Close();  //slose file
        }

        static void GetStats(bool Show = false)  //show total saved and filled envelopes
        {
            FilledCount = 0; TotalSaved =0;  //clear variables
            for (int i = 1;i < 101; i++)
            {
                if ( IsFilled[i] == true)
                {
                    FilledCount++;  
                    TotalSaved += i;
                }
            }

            if (Show)  //if stats are to be shown, not just calculated
            {
                Console.WriteLine("\n\nSTATISTICS:");  //write to screen 
                Console.WriteLine("         Total Saved:  $" + TotalSaved.ToString("n2"));
                Console.WriteLine("    Envelopes Filled:  " + FilledCount.ToString() + " of 100");
            }
        }

        static void MainMenu(bool Show = false)
        {
            if (Show)  //if menu options are desired
            {
                Console.WriteLine("\n\n100 Envelope Savings Challenge - Main Menu\n\n");
                Console.WriteLine("1 - Check Envelope Status");
                Console.WriteLine("2 - Mark  Envelope as Filled");
                Console.WriteLine("3 - Savings Stats");
                Console.WriteLine("0 - Exit");

                int MenuItem = GetNumber("\n", 0, 3);  //get number 0 to 3

                switch(MenuItem)  //run functions based on number 
                {
                    case 0:  //exit 
                        SaveToFile(); 
                        Environment.Exit(0);
                        break;
                    case 1:  //envelope status
                        CheckEnvelopes();
                        break;
                    case 2:  //mark as filled
                        MarkFilled();
                        break;
                    case 3:  //get stats 
                        GetStats(true);
                        break;
                    default:  //all other input 
                        MainMenu(false);  //try again, no options display 
                        break;
                }
            }
        }

        static void MarkFilled()
        {
            int EnvChosen;

            Console.WriteLine("\n\n100 Envelope Savings Challenge - Mark Envelope as Filled\n");

            while (true)
            {
                EnvChosen = GetNumber("Chooes an envolope from 1 to 100:  ", 1, 100);

                if (IsFilled[EnvChosen] == true)
                {
                    Console.WriteLine("Envelope #" + EnvChosen + " has already been filled.");
                    MarkFilled();
                }
                else
                {
                    IsFilled[EnvChosen] = true;
                    GetStats(false);
                    Console.WriteLine("Ok, Envelope #" + EnvChosen + " has been marked as filled.");
                    Console.WriteLine("Be sure to put $" + EnvChosen + " in the appropriate place.");
                    MainMenu(true);
                    return;
                }
            }
        }

        static int GetNumber(String Prompt, int Low, int High)  //get a number from thee user
        {
            string line; int rtn = 0;  //line read and number returned
            while (true)  //loop until valid input
            {
                Console.Write(Prompt);  //display prompt message before getting input
                line = Console.ReadLine();  //store input

                if (int.TryParse(line, out rtn))  //if string can convert to double
                {
                    // Successfully parsed, exit the loop
                    break;
                }
                else
                {
                    // Invalid input, prompt the user again
                    System.Media.SystemSounds.Asterisk.Play();  //play a sound 
                }
            }

            if (rtn < Low || rtn > High)  //bounds check
            {
                System.Media.SystemSounds.Asterisk.Play();
                GetNumber(Prompt, Low, High);
            }

            return rtn;  //return number, all checks passed
        }

        static void LoadFile()
        {
            if ( ! File.Exists(EnvSave) )
            {
                return;
            }

            string line; int EnvNumber; bool EnvStatus;  //holds complete line, number and status 
            StreamReader CsvRip = new StreamReader
            (EnvSave);  //open a file

            for (int i = 1; i<101; i++)
            {
                line = CsvRip.ReadLine();  //read a line from file
                string[] subs = line.Split(',');  //split string by ,
                EnvNumber = Convert.ToInt16(subs[0]);
                EnvStatus = Convert.ToBoolean(subs[1]);
                IsFilled[EnvNumber] = EnvStatus;
            }
            CsvRip.Close();
        }
    }  //end class
}  //end namespace
