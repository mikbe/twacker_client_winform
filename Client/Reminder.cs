using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Twacker
{
    class Reminder
    {
        int _timeoutMilliseconds = Properties.Settings.Default.ReminderTimerMins * 60 * 1000;
        System.Timers.Timer _timer;

        string _message;

        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }

        public Reminder(string message)
        {
            _message = message;
        }

        public void Start()
        {
            Stop();
            configureTimer();
            _timer.Start();
        }

        public void Stop()
        {
            if (_timer != null)
            {
                _timer.Stop();
                _timer.Enabled = false;
                _timer.Dispose();
            }
        }

        private void configureTimer()
        {
            _timer = new System.Timers.Timer(15000);
            _timer.Elapsed += new System.Timers.ElapsedEventHandler(_timer_Elapsed);
            _timer.Enabled = true;        
        }

        void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Speech.Say(_message);
        }

    }
}
