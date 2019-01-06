using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Media;

namespace CritterWorld
{
    public class Sound
    {
        static AudioFileReader boom = new AudioFileReader("Sounds/Explosion.wav");
        static AudioFileReader zap = new AudioFileReader("Sounds/Zap.wav");
        static AudioFileReader bump = new AudioFileReader("Sounds/Bump.wav");
        static AudioFileReader crash = new AudioFileReader("Sounds/Crash.wav");

        static IWavePlayer playBoom = new WaveOut();

        public static void PlayBoom()
        {
            AudioFileReader boom = new AudioFileReader("Sounds/Explosion.wav");
            playBoom.Init(boom);
            playBoom.Play();
        }

        static bool zapPlaying = true;

        public static void PlayZap()
        {
            /*
            if (zapPlaying)
            {
                return;
            }
            zapPlaying = true;
            IWavePlayer zap = NewPlayer("Zap");
            zap.Play();
            Timer timer = new Timer
            {
                AutoReset = false,
                Interval = 500
            };
            timer.Elapsed += (e, evt) =>
            {
                zap.Stop();
                zapPlaying = false;
            };
            timer.Start();
            */
        }

        public static void PlayBump()
        {
      //      AudioFileReader bump = new AudioFileReader("Sounds/Bump.wav");
      //      playBoom.Init(bump);
      //      playBoom.Play();
        }

        public static void PlayCrash()
        {
            AudioFileReader crash = new AudioFileReader("Sounds/Crash.wav");
            playBoom.Init(crash);
            playBoom.Play();
        }

    }
}
