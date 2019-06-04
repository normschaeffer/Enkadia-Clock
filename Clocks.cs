using System;

using Windows.System.Threading;

namespace Enkadia.Helpers.Clocks
{
    public class Clock
    { 
        private static string shutdownAlarm { get; set; }
        private static string startupAlarm { get; set; }
        private static int duration { get; set; }
        private static double totalSeconds { get; set; }
        private static double counter { get; set; }
        private string currentTime { get; set; }
        
        public string TodaysDate { get; set; }
        public string TodaysTime { get; set; }
        public string TodaysTime24Hour { get; set; }
        public string TodaysDay { get; set; }
        public string Hours { get; set; }
        public string Minutes { get; set; }
        public string Seconds { get; set; }
        public bool ShutdownAlarm { get; set; }
        public bool StartupAlarm { get; set; }
        public bool CountdownTriggered { get; set; }
        public string TimeRemaining { get; set; }

        private static TimeSpan timeRemaining;
        private ThreadPoolTimer timer;

        /// <summary>
        /// Initialize a clock and start it running
        /// </summary>
        public Clock()
        {
            Start();
        }

        /// <summary>
        /// Initialize a clock to shutdown at a given time  (hh:mm AM/PM)
        /// </summary>
        /// <param name="ShutdownTime">When would you like the system to shutdown?</param>
        public Clock(string ShutdownTime)
        {
            SetShutdown(ShutdownTime);
            Start();
        }

        /// <summary>
        /// Initialize a clock to startup and shutdown a system at a specified time (hh:mm AM/PM)
        /// </summary>
        /// <param name="StartupTime">When would you like the system to shutdown?</param>
        /// <param name="ShutdownTime">When would you like the system to startup?</param>
        public Clock(string StartupTime, string ShutdownTime)
        {
            SetStartup(StartupTime);
            SetShutdown(ShutdownTime);
            Start();
        }

        /// <summary>
        /// Starts a clock to return day, date and time - clock stops and resets to zero on app exit
        /// </summary>
        private void Start()
        {
            timer = ThreadPoolTimer.CreatePeriodicTimer(GetTime, TimeSpan.FromMilliseconds(500));
        }

        /// <summary>
        /// Stop the clock
        /// </summary>
        public void Stop()
        {
            timer.Cancel();
        }

        private void GetTime(ThreadPoolTimer timer)
        {
            TodaysDate = DateTime.Now.ToShortDateString();
            TodaysTime = DateTime.Now.ToShortTimeString();
            TodaysTime24Hour = DateTime.Now.ToString("HHmm");
            TodaysDay = DateTime.Now.DayOfWeek.ToString();
            Hours = DateTime.Now.Hour.ToString();
            Minutes = DateTime.Now.Minute.ToString();
            Seconds = DateTime.Now.Second.ToString();

            if(shutdownAlarm == TodaysTime24Hour)
            {
                ShutdownAlarm = true;
            }
            else
            {
                ShutdownAlarm = false;
            }

            if(startupAlarm == TodaysTime24Hour)
            {
                StartupAlarm = true;
            }
            else
            {
                StartupAlarm = false;
            }
        }

        /// <summary>
        /// Set desired system shutdown time
        /// </summary>
        /// <param name="ShutdownTime">Set desired time to shut system down</param>
        public void SetShutdown(string ShutdownTime)
        {
            DateTime dt;
            bool response = DateTime.TryParse(ShutdownTime, out dt); //can this be parsed as 24 hour clock?
            //TODO: handle if response is false
            shutdownAlarm = dt.ToString("HHmm");   //output as 24-hour clock to trigger alarm properly
        }

        /// <summary>
        /// Set desired system startup time
        /// </summary>
        /// <param name="StartupTime">Set desired time to shut system down</param>
        public void SetStartup(string StartupTime)
        {
            DateTime dt;
            bool response = DateTime.TryParse(StartupTime, out dt); //can this be parsed as 24 hour clock?
            //TODO: handle if response is false
            startupAlarm = dt.ToString("HHmm");   //output as 24-hour clock to trigger alarm properly
        }
    }
}
