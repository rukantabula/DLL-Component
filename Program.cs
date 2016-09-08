using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogUsers
{
    using System.Threading;

    using LogTest;
    using System.Runtime.InteropServices;

    class Program
    {

        // method to add 1 day to the system date
        // Date format is different from one computer to another so one must make sure the format is the same
        private static void increaseDayByOne ()
        {
            DateTime d = DateTime.Now.AddDays(1);
            setDate(d.Day +"-" + d.Month + "-2016");

        }

        // method that changes the system date
        static void setDate ( string dateInYourSystemFormat )
        {
            var proc = new System.Diagnostics.ProcessStartInfo();
            proc.UseShellExecute = true;
            proc.WorkingDirectory = @"C:\Windows\System32";
            proc.CreateNoWindow = true;
            proc.FileName = @"C:\Windows\System32\cmd.exe";
            proc.Verb = "runas";
            proc.Arguments = "/C date " + dateInYourSystemFormat;
            try
            {
                System.Diagnostics.Process.Start(proc);
            }
            catch
            {
                Console.Write("Error to change time of your system");

            }
        }

        static void Main ( string[] args )
        {
            // the stopAt variable is initiated to 50 to marka breakinmg point where a new date is detected
            int stopAt = 50;
            Console.WriteLine("Which test do you want to run  ?");
            Console.WriteLine("0 - No Stops / No Date Change");
            Console.WriteLine("1 - No Stops / With Date Change");
            Console.WriteLine("2 - Exit With Flush");
            Console.WriteLine("3 - Exit With No Flush");
            Console.WriteLine("4 - Exit Program");
             Char keyChar = 'A';
             while (keyChar != '4')
             {
                 ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                 keyChar = keyInfo.KeyChar;
                 switch (keyChar)
                 {

                    // Test that checks that the program is running/writing something 
                     case ('0'):
                         {
                             ILog logger0 = new AsyncLog();
                             for (int i = 0; i < 200; i++)
                             {
                                 Console.WriteLine("Test line with No Stop # " + i);
                                 logger0.Write("Number with No Stop: " + i.ToString() + "\n");
                                 Thread.Sleep(20);
                             }

                             Console.WriteLine("Test line NO STOPS Completed");
                             break;
                         }

                        //Test to check if a new log file is created upon passing midnight
                     case ('1'):
                         {
                             ILog logger0 = new AsyncLog();
                             for (int i = 0; i < 200; i++)
                             {
                                 Console.WriteLine("Test line with No Stop # " + i);
                                 logger0.Write("Number with No Stop: " + i.ToString() + "\n");
                                 Thread.Sleep(100);
                                 if (i == stopAt)
                                 {
                                     Console.WriteLine("Increasing day by one");
                                     increaseDayByOne();
                                 }
                             }
                             
                             Console.WriteLine("Test line NO STOPS Completed");
                             break;
                         }

                        //Test to check if the system exits with flush
                     case ('2'):
                         {
                             Console.WriteLine("Testing With Flush");
                             ILog logger = new AsyncLog();
                             logger.ExitFlushTestingMode();
                             for (int i = 0; i < 150; i++)
                             {
                                 Console.WriteLine("Test line with flush # " + i);
                                 logger.Write("Number with Flush: " + i.ToString() + "\n");
                                 Thread.Sleep(100);
                                 if (i == stopAt)
                                 {
                                     Console.WriteLine("Increasing day by one");
                                     increaseDayByOne();

                                 }
                             }


                             logger.StopWithFlush();

                             Console.WriteLine("Testing Without Flush");
                             break;
                         }

                        //Test to check if the system exits without flush
                     case ('3'):
                         {

                             ILog logger2 = new AsyncLog();
                            logger2.ExitNoFlushTestingMode();
                             for (int i = 100; i > 0; i--)
                             {
                                 Console.WriteLine("Test line (no flush) # " + i);
                                 logger2.Write("Number with No flush: " + i.ToString());
                                 Thread.Sleep(35);
                                 if (i == stopAt)
                                 {
                                     Console.WriteLine("Increasing day by one");
                                     increaseDayByOne();
                                 }
                             }

                             logger2.StopWithoutFlush();
                             break;
                         }
                     case ('4'):
                         {
                             Console.Write("Program Completed");
                             Console.ReadLine();
                             break;
                         }
                 }
                 Console.Write("Program Completed");
                 Console.ReadLine();
             }


        }
    }
}
