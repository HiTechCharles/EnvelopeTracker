using System;
using System.Data;
using System.Diagnostics.SymbolStore;
using System.IO;

namespace EnvelopeTracker
{
    internal class Program
    {
        #region VARIABLES
        public static string AppDirectory = Environment.GetEnvironmentVariable("onedriveconsumer") + "\\documents\\EnvelopeTracker\\";  //path to save and read from
        public static string EnvSave = AppDirectory + "Envelopes.csv";  //save file location
        public static bool[] IsFilled = new bool[101];  //envelope status 
        public static int FilledCount;  //number of envelopes filled
        #endregion

        static void Main()  //entrypoint of program
        {
            Console.Title = "100 Envelopes Savings Tracker";  //console window title
            Console.ForegroundColor = ConsoleColor.White;  //text color for console

            LoadFile();  //load envelope file and store it in IsFilled[]
            
            while (true)  //makes sure menu is shown after functions exit
            {
                MainMenu(true);  //true means show the options
            }
        }

        static void CheckEnvelopes()  //prints out empty and full envelopes
        {
            switch (FilledCount)  //based on number of envelopes filled,
            {
                case 0:  //none
                    Console.WriteLine("All envelopes are empty.  Start Saving!  \n");
                    return;
                    break;
                case 100:  //all filled 
                    Console.WriteLine("All envelopes are filled, Great Job!\n");
                    return;  //exit function
                    break;
            }

            Console.WriteLine("\n100 Envelope Savings Challenge - Envelope Status\n");  //menu header
            ShowFilled();  //show filled envelopes 
            ShowEmpty();   //display empty ones
            Console.WriteLine();
        }

        static void ShowFilled()  //show filled envelopes
        {
            int Printed = 0;  //print output in rows of 10
            Console.WriteLine("FILLED ENVELOPES:  ");
            for (int i = 1; i < 101; i++)  //loop IsFilled Array, True = filled
            {
                if (IsFilled[i] == true)
                {
                    Console.Write(i.ToString().PadLeft(3) + "  ");  //write values, 
                    Printed++;  //increment items printed
                    if (Printed % 10 == 0)  //Printed / 10 = 0 goto next row
                    {
                        Printed = 0;  //reset printed count
                        Console.WriteLine();  //new line
                    }
                }
            }
        }

        static void ShowEmpty()  //show empty envelopes  all other code is identical tofilled  function 
        {
            int Printed = 0;
            Console.WriteLine("\n\nEMPTY ENVELOPES:");

            for (int i = 1; i < 101; i++)
            {
                if (IsFilled[i] == false)
                {
                    Console.Write(i.ToString().PadLeft(3) + "  ");
                    Printed++;
                    if ((Printed % 10) == 0)
                    {
                        Printed = 0;
                        Console.WriteLine();
                    }
                }
            }
        }

        static void SaveToFile()  //save status to file
        {
            //writes file in csv format.  envelope amount, filled status true / false
            Directory.CreateDirectory(AppDirectory);  //make sure directory exists before writing
            StreamWriter SW = new StreamWriter(EnvSave);    //open file for writing
            
            for (int i = 1;i <101; i++)  //loop through all numbers 1-100
            {
                SW.WriteLine(i + "," + IsFilled[i]);  //write to file, csv format
            }
            SW.Close();  //close file
        }

        static void GetStats(bool Show = false)  //show savings stats
        {
            FilledCount = 0;  //clear variables before looping
            double Unsaved, TotalSaved = 0;
            double PercentSaved;
            for (int i = 1;i < 101; i++)
            {
                if ( IsFilled[i] == true)
                {
                    FilledCount++;  //add 1 to filled count
                    TotalSaved += i;  //total money saved
                }
            }

            Unsaved = 5050-TotalSaved;  //money left to save
            PercentSaved = TotalSaved / 5050 * 100;
            if (Show)  //if stats are to be shown, not just calculated
            {
                Console.WriteLine("\nSTATISTICS:");  //write to screen 
                Console.WriteLine("            Total Saved:  $" + TotalSaved.ToString("n2"));
                Console.WriteLine("     Money Left to Save:  $" + Unsaved.ToString("n2"));
                Console.WriteLine("       Percentage Saved:  " + PercentSaved.ToString("n2"));
                Console.WriteLine("       Envelopes Filled:  " + FilledCount.ToString() + " of 100");
            }
        }

        static void MainMenu(bool Show = false)  //main program menu
        {
            if (Show)  //if menu options are desired
            {
                Console.WriteLine("\n100 Envelope Savings Challenge - Main Menu\n");
                Console.WriteLine("C - Check Envelope Status");
                Console.WriteLine("M - Mark as Filled");
                Console.WriteLine("R - Remove money from an envelope");
                Console.WriteLine("S - Savings Stats");
                Console.WriteLine("X - Exit");
            }

            GetStats(false);    //makes dure stats are up to date
            char MenuKey = WaitForKeyPress("");  //get 1 keypress, lowercase
                
                switch(MenuKey)  // run functions based on number 
                {
                    case 'x':  //exit 
                        SaveToFile(); 
                        Environment.Exit(0);  //exit app
                        break;
                    case 'c':  //envelope status
                        CheckEnvelopes();
                        break;
                    case 'm':  //mark as filled
                        MarkFilled();  //mark an envelope as filled
                        break;
                case 'r':
                        UnMarkFilled();  //remove money from an envelope
                        break;
                case 's':  //get stats 
                        GetStats(true);  //show savings stats
                        break;
                    default:  //all other input 
                        MainMenu(false);  //try again, no options display 
                        break;
                }
            }
        
        static void MarkFilled(bool ShowHeader = false)  //mark as filled
        {
            int EnvChosen;  //envelope chosen to fill
            if (ShowHeader == false )  //show header only once 
            {
                Console.WriteLine("\n100 Envelope Saving Challenge - Mark Envelope as Filled\n");
                ShowEmpty();  //show empty envelopes 
                ShowHeader = true;
            }

            while (true)  //until good input
            {  
                EnvChosen = GetNumber("\n\nChooes an envolope from 1 to 100:  ", 0, 100);
                
                if (EnvChosen == 0)  //goto main menu
                {
                    return;
                }
                
                if (IsFilled[EnvChosen] == true)  //if envelope is filled, try again
                {
                    Console.WriteLine("\nEnvelope #" + EnvChosen + " has already been filled.");
                    MarkFilled(true);
                }
                else
                {
                    IsFilled[EnvChosen] = true;  //set chosen envelope to true
                    Console.WriteLine("Ok, Envelope #" + EnvChosen + " has been marked as filled.");
                    Console.WriteLine("Be sure to put $" + EnvChosen + " in the appropriate place.");
                    MainMenu(true);
                    return;
                }
            }
        }

        static void UnMarkFilled(bool ShowHeader = false)  //unmark envelope / remove money
        {
            int EnvChosen;
            if (ShowHeader == false)
            {
                Console.WriteLine("\n100 Envelope Savings Challenge - Remove Money From and Envelope\n");
                ShowFilled();
                ShowHeader = true;
            }

            while (true)
            {
                EnvChosen = GetNumber("\n\nChooes an envolope from 1 to 100:  ", 0, 100);

                if (EnvChosen == 0)
                {
                    return;
                }

                if (IsFilled[EnvChosen] == true)
                {
                    IsFilled[EnvChosen] = false;
                    Console.WriteLine("Ok, the status of Envelope #" + EnvChosen + " has been changed to empty.");
                    Console.WriteLine("Be sure to remove $" + EnvChosen + " from the envelope");
                    MainMenu(true);
                    return;
                }
                else
                {
                    Console.WriteLine("\nEnvelope #" + EnvChosen + " is already empty.");
                    UnMarkFilled(true);
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

                if (int.TryParse(line, out rtn))  //if string can convert to int
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

        static void LoadFile()  //load file from disk into array
        {
            if ( ! File.Exists(EnvSave) )  //if file does not exist, don't try to load
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
                EnvNumber = Convert.ToInt16(subs[0]);  //sub zero, johnny cage, raiden
                EnvStatus = Convert.ToBoolean(subs[1]); //get number and status
                IsFilled[EnvNumber] = EnvStatus;  //store results of current line
            }
            CsvRip.Close();
        }

        static char WaitForKeyPress(string prompt)  //get a keypress and store it
        {
            Console.WriteLine(prompt);
            ConsoleKeyInfo keyInfo = Console.ReadKey(true); // Prevents the key from being displayed
            return keyInfo.KeyChar;
        }
    }  //end class helps me keep all the braces straight
}  //end namespace
