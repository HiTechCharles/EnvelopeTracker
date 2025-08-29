using System;
using System.Collections.Generic;
using System.IO;
using System.Speech.Synthesis;
using System.Speech;

namespace EnvelopeTracker
{
internal class Program
{
    #region VARIABLES
    public static string AppDirectory = Environment.GetEnvironmentVariable("onedriveconsumer") + "\\documents\\EnvelopeTracker\\";  //path to save and read from
    public static string CSVSave = AppDirectory + "EnvelopeStatus.csv";  //envelope status save file 
    public static string TextSave = AppDirectory + "Savings Report.txt";  //save text report
    public static bool[] IsFilled = new bool[101];  //envelope status 
    public static int FilledCount;  //number of envelopes filled
        public static bool TextToSpeech;  //check if speech is on
        static readonly SpeechSynthesizer ESpeak = new SpeechSynthesizer();
        #endregion

        static void Main()  //entrypoint of program
    {
        Console.Title = "100 Envelopes Savings Tracker";  //console window title
        Console.ForegroundColor = ConsoleColor.White;  //text color for console
            ESpeak.Rate = 4;

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
                Print("All envelopes are empty.  Start Saving!  \n");
                return;
            case 100:  //all filled 
                Print("All envelopes are filled, Great Job!\n");
                return;  //exit function
        }


        if (TextToSpeech)
            {
                Speak("Empty and full envelopes are not read to reduce amount of speech.");
            }

        Console.WriteLine("\n100 Envelope Savings Challenge - Envelope Status\n");  //menu header
        ShowFilled();  //show filled envelopes 
        ShowEmpty();   //display empty ones
        Print();
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
                    Print();  //new line
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
                    Print();
                }
            }
        }
    }

    static void SaveToCSV()  //save status to file
    {
        //writes file in csv format.  envelope amount, filled status true / false
        Directory.CreateDirectory(AppDirectory);  //make sure directory exists before writing
        StreamWriter SW = new StreamWriter(CSVSave);    //open file for writing

        for (int i = 1; i < 101; i++)  //loop through all numbers 1-100
        {
            SW.WriteLine(i + "," + IsFilled[i]);  //write to file, csv format
        }
        SW.Close();  //close file
    }

    static void SaveToText()
    {
        StreamWriter sw = new StreamWriter(TextSave);

        //SAVE save full envelopes
        int Printed = 0;  //print output in rows of 10
        sw.WriteLine("FILLED ENVELOPES:  ");
        for (int i = 1; i < 101; i++)  //loop IsFilled Array, True = filled
        {
            if (IsFilled[i] == true)
            {
                sw.Write(i.ToString().PadLeft(3) + "  ");  //write values, 
                Printed++;  //increment items printed
                if (Printed % 10 == 0)  //Printed / 10 = 0 goto next row
                {
                    Printed = 0;  //reset printed count
                    sw.WriteLine();  //new line
                }
            }
        }

        //save empty envelopes
        Printed = 0;
        sw.WriteLine("\n\nEMPTY ENVELOPES:");
        for (int i = 1; i < 101; i++)
        {
            if (IsFilled[i] == false)
            {
                sw.Write(i.ToString().PadLeft(3) + "  ");
                Printed++;
                if ((Printed % 10) == 0)
                {
                    Printed = 0;
                    sw.WriteLine();
                }
            }
        }

        FilledCount = 0;  //clear variables before looping
        double Unsaved, TotalSaved = 0;
        double PercentSaved;
        for (int i = 1; i < 101; i++)
        {
            if (IsFilled[i] == true)
            {
                FilledCount++;  //add 1 to filled count
                TotalSaved += i;  //total money saved
            }
        }

        Unsaved = 5050 - TotalSaved;  //money left to save
        PercentSaved = TotalSaved / 5050 * 100;
        sw.WriteLine("\n\nSTATISTICS:");  //write to screen 
        sw.WriteLine("            Total Saved:  $" + TotalSaved.ToString("n2"));
        sw.WriteLine("     Money Left to Save:  $" + Unsaved.ToString("n2"));
        sw.WriteLine("       Percentage Saved:  " + PercentSaved.ToString("n2"));
        sw.WriteLine("       Envelopes Filled:  " + FilledCount.ToString() + " of 100");
        sw.Close();
    }

    static void GetStats(bool Show = false)  //show savings stats
    {
        FilledCount = 0;  //clear variables before looping
        double Unsaved, TotalSaved = 0;
        double PercentSaved;
        for (int i = 1; i < 101; i++)
        {
            if (IsFilled[i] == true)
            {
                FilledCount++;  //add 1 to filled count
                TotalSaved += i;  //total money saved
            }
        }

        Unsaved = 5050 - TotalSaved;  //money left to save
        PercentSaved = TotalSaved / 5050 * 100;
        if (Show)  //if stats are to be shown, not just calculated
        {
            Print("\nSTATISTICS:");  //write to screen 
            Print("            Total Saved:  $" + TotalSaved.ToString("n2"));
            Print("     Money Left to Save:  $" + Unsaved.ToString("n2"));
            Print("       Percentage Saved:  " + PercentSaved.ToString("n2"));
            Print("       Envelopes Filled:  " + FilledCount.ToString() + " of 100");
        }
    }

    static void MainMenu(bool Show = false)  //main program menu
    {
        if (Show)  //if menu options are desired
        {
            Print("\n100 Envelope Savings Challenge - Main Menu\n");
            Print("C - Check Envelope Status");
            Print("M - Mark as Filled");
            Print("R - Remove money from an envelope");
            Print("S - Savings Stats");
                
            if ( TextToSpeech)
            {
                Print("T - Toggle Text to Speech, currently ON");
            }
            else
            {
                Print("T - Toggle Text to Speech, currently OFF");
            }

            Print("X - Exit");
        }

        GetStats(false);    //make aure stats are up to date
        SaveToCSV();  //save status to csv file
        SaveToText();  //save report to text file
        char MenuKey = WaitForKeyPress("");  //get 1 keypress, lowercase

        switch (MenuKey)  // run functions based on letter
        {
            case 'x':  //exit 
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
            case 't':
                TextToSpeech = !TextToSpeech;
                break;
            default:  //all other input 
                MainMenu(false);  //try again, no options display 
                break;
        }
    }

    static void MarkFilled(bool ShowHeader = false)  //mark as filled
    {
        int EnvChosen;  //envelope chosen to fill
        if (ShowHeader == false)  //show header only once 
        {
            Print("\n100 Envelope Savings Challenge - Mark Envelope as Filled\n");
            ShowEmpty();  //show empty envelopes 
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
                Print("\nEnvelope #" + EnvChosen + " has already been filled.");
                MarkFilled(true);
            }
            else
            {
                IsFilled[EnvChosen] = true;  //set chosen envelope to true
                Print("Ok, Envelope #" + EnvChosen + " has been marked as filled.");
                PrintBillBreakdown(EnvChosen);
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
            Print("\n100 Envelope Savings Challenge - Remove Money From and Envelope\n");
            ShowFilled();
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
                Print("Ok, the status of Envelope #" + EnvChosen + " has been changed to empty.");
                Print("Be sure to remove $" + EnvChosen + " from the envelope");
                MainMenu(true);
                return;
            }
            else
            {
                Print("\nEnvelope #" + EnvChosen + " is already empty.");
                UnMarkFilled(true);
            }
        }
    }

    static void Print(string prompt = "", bool SameLine = false)
    {
        if ( SameLine == true)
        {
            Console.Write(prompt);
        }
        else
        { 
            Console.WriteLine(prompt); 
        }
        Speak(prompt);
    }

    static void Speak(string prompt)
    {


            if (TextToSpeech)
            {
                ESpeak.SpeakAsync(prompt);
            }
                
    }

    static int GetNumber(String Prompt, int Low, int High)  //get a number from thee user
    {
        string line; int rtn;
        while (true)  //loop until valid input
        {
            Print(Prompt, true);  //display prompt message before getting input
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
        if (!File.Exists(CSVSave))  //if file does not exist, don't try to load
        {
            return;
        }

        string line; int EnvNumber; bool EnvStatus;  //holds complete line, number and status 
        StreamReader CsvRip = new StreamReader
        (CSVSave);  //open a file

        for (int i = 1; i < 101; i++)
        {
            line = CsvRip.ReadLine();  //read a line from file
            string[] subs = line.Split(',');  //split string by ,
            EnvNumber = Convert.ToInt16(subs[0]);  //sub zero, johnny cage, raiden
            EnvStatus = Convert.ToBoolean(subs[1]); //get number and status
            IsFilled[EnvNumber] = EnvStatus;  //store results of current line
        }
        CsvRip.Close();
    }

    static void PrintBillBreakdown(int amount)
    {
        int[] denominations = new int[] { 100, 50, 20, 10, 5, 1 };
        Dictionary<int, int> billCounts = new Dictionary<int, int>();

        foreach (int bill in denominations)
        {
            billCounts[bill] = amount / bill;
            amount %= bill;
        }

        Print("\nYou will need the following bills for this envelope:");

        foreach (KeyValuePair<int, int> kvp in billCounts)
        {
            if (kvp.Value > 0)
            {
                Print($"     ${kvp.Key,-3}: {kvp.Value,2} {(kvp.Value == 1 ? "bill" : "bills")}");
            }
        }
    }

    static char WaitForKeyPress(string prompt)  //get a keypress and store it
    {
        Print(prompt);
        ConsoleKeyInfo keyInfo = Console.ReadKey(true); // Prevents the key from being displayed
        return keyInfo.KeyChar;
    }
}  //end class helps me keep all the braces straight
}  //end namespace