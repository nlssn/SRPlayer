using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using NAudio.Wave;

namespace SRPlayer
{
    /*
     * The Stream class handles streaming of audio from a given source (URL).
     * This is done using the NAudio package. More info available here: https://github.com/naudio/NAudio
     */
    class Stream
    {
        // Properties
        private string Source; // The URL to the audio stream
        private MediaFoundationReader mf; // Reads the audio file
        private WaveOutEvent wo; // Plays the audio using WaveOut
        public static int count = 0;

        // Constructor
        public Stream(string source)
        {
            Interlocked.Increment(ref count);
            Source = source;
            mf = new MediaFoundationReader(Source);
            wo = new WaveOutEvent();
            wo.Init(mf);
        }

        // Starts the stream
        public void Play()
        {
            wo.Play();
        }

        // Pauses the stream
        public void Pause()
        {
            wo.Pause();
        }

        // Stops the stream
        public void Stop()
        {
            wo.Stop();
        }

        // Returns the stat of the stream (playing, paused, stopped)
        public string GetPbState()
        {
            return Convert.ToString(wo.PlaybackState);
        }
    }
}
