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

        public static void Say(string text) 
        {
            ThreadPool.QueueUserWorkItem(queuedSay, text);
        }

        private static void queuedSay(object text)
        {
            _speaker.Speak(text.ToString());
        }

    }
}
