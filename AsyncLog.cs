namespace LogTest
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Threading;

    public class AsyncLog : ILog
    {
        private Boolean isTestingMode; //swich that turns the testing mode on or off for testing puposes
        private Thread _runThread;
        private List<LogLine> _lines = new List<LogLine>();
        private StreamWriter _writer;
        private bool _exit; // triggers exit without flush functionality
        private bool _QuitWithFlush = false; //triggers exit with flush functionality
        DateTime dtCurrent; // declared a date time variable for comparing if the date has changed
        Boolean injectedError = false; // introduced error to test if the program wont stop executing even after the error has been caught
        DateTime _curDate = DateTime.Now;

        public void ExitNoFlushTestingMode() // entering testing mode
        {
            isTestingMode = true;
        }

        public void ExitFlushTestingMode() // exiting testing mode
        {
            isTestingMode = false;
        }

        public AsyncLog()
        {
            createNewFile();
            this._runThread = new Thread(this.MainLoop);
            this._runThread.Start();
        }

        public void ReleaseFile()
        {
            if (_writer != null)
                _writer.Close();
        }

        private void createNewFile()
        {
             ReleaseFile();
            if (!Directory.Exists(@"C:\LogTest"))
                Directory.CreateDirectory(@"C:\LogTest");

            dtCurrent = DateTime.Now;

            this._writer = File.AppendText(@"C:\LogTest\Log" + DateTime.Now.ToString("yyyyMMdd HHmmss fff") + ".log");
            this._writer.Write("Timestamp".PadRight(25, ' ') + "\t" + "Data".PadRight(15, ' ') + "\t" + Environment.NewLine);
            this._writer.AutoFlush = true;

        }


        private Boolean isDayAfter(DateTime current, DateTime baseDate) //check if its a new date or it is past midnight
        {
            if (current.Year > baseDate.Year) return true;
            if (current.Year < baseDate.Year) return false;
            if (current.Year == baseDate.Year)
            {
                if (current.Month > baseDate.Month)
                {
                    return true;
                }
                else
                {
                    if (current.Month < baseDate.Month)
                    {
                        return false;

                    }
                    if (current.Month == baseDate.Month)
                    {
                        if (current.Day > baseDate.Day) return true; else return false;
                    }
                }
            }

            return false;

        }

        private void MainLoop()
        {
            while (!this._exit)
            {


                try // the try ... catch error handler has been introduced to catch the errors during execution
                {

                    DateTime now = DateTime.Now;

                    if (this._lines.Count > 0)
                    {
                     
                        List<LogLine> _handled = new List<LogLine>();

                        /* The lines of code below the comment line is to be activated/uncommented 
                        when testing if the system will continue execution even
                        though some errors have been detected */
                       
                        //if (!injectedError)
                        //{
                        //    injectedError = true;
                        //    int a = 30; int b = 0; int c = a / b;
                        //    int a1 = 30; int b1 = 0; int c1 = a1 / b1;
                        //    int a3 = 30; int b3 = 0; int c3 = a3 / b3;
                        //}
                        

                        for (int i = 0; i < this._lines.Count; i++)
                        {
                          if (isTestingMode)
                          Thread.Sleep(300);
                                
                            if (this._exit)
                            {
                                ReleaseFile();
                                break;
                            }
                            LogLine logLine = this._lines[i];
                          

                            // Condition that instructs how the program should behave when exit or exit with flush function is triggered
                            if (!this._exit || this._QuitWithFlush)
                            {
                                /*comparing or matching the current date and date from the first file created
                                if there is a change in date then the method to create a new file is called */
                                if (isDayAfter(now, dtCurrent)) 
                                {
                                    Console.Write("DETECTED DAY CHANGE" + now.ToLongTimeString() + " _ " + dtCurrent.ToLongTimeString());
                                    createNewFile();
                                }
                                
                                _handled.Add(logLine);

                                StringBuilder stringBuilder = new StringBuilder();
                                stringBuilder.Append(logLine.Timestamp.ToString("yyyy-MM-dd HH:mm:ss:fff"));
                                stringBuilder.Append("\t");
                                stringBuilder.Append(logLine.LineText());
                                stringBuilder.Append("\t");

                                stringBuilder.Append(Environment.NewLine);

                                this._writer.Write(stringBuilder.ToString());
                            }
                        }

                        for (int y = 0; y < _handled.Count; y++)
                        {
                            this._lines.Remove(_handled[y]);
                        }

                        //if quit with flush is true and logline to be written is empty then the program stops
                        if (this._QuitWithFlush == true && this._lines.Count == 0)
                            this._exit = true;
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(" List of error detected  " + ex); //catching errors detected during execution
                }
            }
            
        }

        public void StopWithoutFlush() 
        {
            this._exit = true;
        }

        public void StopWithFlush()
        {
            this._QuitWithFlush = true;
        }

        public void Write(string text)
        {
            this._lines.Add(new LogLine() { Text = text, Timestamp = DateTime.Now });
        }
    }
}
