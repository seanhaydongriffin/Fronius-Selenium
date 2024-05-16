using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.IO;
using System.Diagnostics;
using System.Threading;

namespace Toolkit
{

    public static class Timer
    {
        private static Dictionary<string, Stopwatch> StopwatchDict = new Dictionary<string, Stopwatch>();



        public static void StartTimer(this string timerName)
        {
            StopwatchDict.put(timerName, new Stopwatch());
            Stopwatch tmp = StopwatchDict.get(timerName);
            tmp.Start();
        }

        public static long GetTimer(this string timerName)
        {
            Stopwatch tmp = StopwatchDict.get(timerName);
            return tmp.ElapsedMilliseconds / 1000;
        }

        public static void StopTimer(this string timerName)
        {
            Stopwatch tmp = StopwatchDict.get(timerName);
            tmp.Stop();
        }

        public static long GetAndStopTimer(this string timerName)
        {
            Stopwatch tmp = StopwatchDict.get(timerName);
            tmp.Stop();
            return tmp.ElapsedMilliseconds / 1000;
        }


        // ===============================================================================
        // Name...........:	Sleep()
        // Description....:	Gets the value of the test object.
        // Syntax.........:	Sleep(this double seconds, int logging_level, int method = 1)
        // Parameters.....:	seconds         - the number of seconds to sleep for
        //                  logging_level   - the logging level
        //                                      1 = Debug (default)
        //                                      2 = Info
        //                  method		    - Optional: A number denoting the method to use to sleep.
        //								        1 = via Thread.Sleep (default)
        //                                      2 = via AutoIt Sleep (suspends and resumes the app process also for CPU improvement)
        // Return values..: None.
        // Remarks........:	None.
        // ==========================================================================================
        public static void Sleep(this double seconds, int logging_level = 1, int method = 1)
        {
            switch (logging_level)
            {
                case 1:

                    ("Sleeping for " + seconds + " seconds ...").LogDebug();
                    break;

                case 2:

                    ("Sleeping for " + seconds + " seconds ...").LogDebug();
                    break;
            }

            if (method == 1)

                Thread.Sleep((int)(seconds * 1000));
            else
            {
                int pid1 = System.Diagnostics.Process.GetCurrentProcess().Id;
                int pid2 = -1;

                //if (BrowserTestObject.CurrentBrowserPID > -1)

                //    pid2 = BrowserTestObject.CurrentBrowserPID;

                //AutoItX.RunWait("\"" + Test_Environment.project_path + "\\Toolkit\\Windows\\AutoItSleep.exe\" " + seconds + " " + pid1 + " " + pid2, "", 0);
            }
        }

        // ===============================================================================
        // Name...........:	Sleep()
        // Description....:	Gets the value of the test object.
        // Syntax.........:	Sleep(this double seconds, int logging_level, int method = 1)
        // Parameters.....:	seconds         - the number of seconds to sleep for
        //                  logging_level   - the logging level
        //                                      1 = Debug (default)
        //                                      2 = Info
        //                  method		    - Optional: A number denoting the method to use to sleep.
        //								        1 = via Thread.Sleep (default)
        //                                      2 = via AutoIt Sleep (suspends and resumes the app process also for CPU improvement)
        // Return values..: None.
        // Remarks........:	None.
        // ==========================================================================================
        public static void Sleep(this String seconds_str, int logging_level = 1, int method = 1)
        {
            if (seconds_str.Equals("") || seconds_str.Equals("0"))

                return;

            double seconds = Convert.ToDouble(seconds_str);

            switch (logging_level)
            {
                case 1:

                    ("Sleeping for " + seconds + " seconds ...").LogDebug();
                    break;

                case 2:

                    ("Sleeping for " + seconds + " seconds ...").LogDebug();
                    break;
            }

            if (method == 1)

                Thread.Sleep((int)(seconds * 1000));
            else
            {
                int pid1 = System.Diagnostics.Process.GetCurrentProcess().Id;
                int pid2 = -1;

                //if (BrowserTestObject.CurrentBrowserPID > -1)

                //    pid2 = BrowserTestObject.CurrentBrowserPID;

                //AutoItX.RunWait("\"" + Test_Environment.project_path + "\\Toolkit\\Windows\\AutoItSleep.exe\" " + seconds + " " + pid1 + " " + pid2, "", 0);
            }
        }

        // ===============================================================================
        // Name...........:	Sleep()
        // Description....:	Gets the value of the test object.
        // Syntax.........:	Sleep(this double seconds, int logging_level, int method = 1)
        // Parameters.....:	seconds         - the number of seconds to sleep for
        //                  logging_level   - the logging level
        //                                      1 = Debug (default)
        //                                      2 = Info
        //                  method		    - Optional: A number denoting the method to use to sleep.
        //								        1 = via Thread.Sleep (default)
        //                                      2 = via AutoIt Sleep (suspends and resumes the app process also for CPU improvement)
        // Return values..: None.
        // Remarks........:	None.
        // ==========================================================================================
        public static void Sleep(this int seconds, int logging_level = 1, int method = 1)
        {
            switch (logging_level)
            {
                case 1:

                    ("Sleeping for " + seconds + " seconds ...").LogDebug();
                    break;

                case 2:

                    ("Sleeping for " + seconds + " seconds ...").LogDebug();
                    break;
            }

            if (method == 1)

                Thread.Sleep((int)(seconds * 1000));
            else
            {
                int pid1 = System.Diagnostics.Process.GetCurrentProcess().Id;
                int pid2 = -1;

                //if (BrowserTestObject.CurrentBrowserPID > -1)

                //    pid2 = BrowserTestObject.CurrentBrowserPID;

                //AutoItX.RunWait("\"" + Test_Environment.project_path + "\\Toolkit\\Windows\\AutoItSleep.exe\" " + seconds + " " + pid1 + " " + pid2, "", 0);
            }
        }


        public static void Stop(this int seconds)
        {
            ("Stop() called.").LogDebug();

            (System.Environment.StackTrace).LogDebug();

            // Stop video logging if required
            //StopDesktopVlog();

            Environment.Exit(0);
        }

    }
}
