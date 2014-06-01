using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Speech.Synthesis;
using System.Threading;

namespace Twacker
{
    /// <summary>
    /// Say text so it doesn't step on itself but also without blocking the main thread.
    /// </summary>
    class Speech
    {
        private static SpeechSynthesizer _speaker = new SpeechSynthesizer();

        private static DateTime lastZeno = new DateTime();

        public static void Say(string text) 
        {
            _speaker.Rate = Properties.Settings.Default.SpeechRate;
            ThreadPool.QueueUserWorkItem(queuedSay, text);
        }

        private static void queuedSay(object oText)
        {
            string text = (string)oText;
            if (text.ToLower().IndexOf("zeno_magatama") == 0)
            {
                if (DateTime.Now > lastZeno.AddMinutes(1))
                {
                    lastZeno = DateTime.Now;
                    _speaker.Speak("Matt Damone said something about pie... probably.");
                }
            }
            else if (text.IndexOf(": !") > 0)
            { 
                // do nothing
            }
            else
            {
                _speaker.Speak(text.ToString());
            }
        }

    }
}
