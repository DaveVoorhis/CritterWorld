using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using NAudio.FireAndForget;

namespace CritterWorld
{
    // IntervalBlocker can be used to prevent a sound from playing too often within a given (somewhat variable) period of time.
    public class IntervalBlocker
    {
        private static Random rnd = new Random(Guid.NewGuid().GetHashCode());

        private Timer timer;
        private int _maxTimeout;
        private int _maxSimultaneous;
        private int simultaneousCount;

        public IntervalBlocker(int maxTimeout, int maxSimultaneous = 1)
        {
            _maxTimeout = maxTimeout;
            _maxSimultaneous = maxSimultaneous;
            simultaneousCount = 0;
        }

        public bool IsBlocked()
        {
            if (++simultaneousCount < _maxSimultaneous)
            {
                return false;
            }
            if (timer != null)
            {
                return true;
            }
            timer = new Timer
            {
                Interval = rnd.Next(_maxTimeout / 3, _maxTimeout),
                AutoReset = false
            };
            timer.Elapsed += (e, evt) =>
            {
                timer = null;
                simultaneousCount = 0;
            };
            timer.Start();
            return false;
        }
    }

    public class Sound
    {
        static AudioPlaybackEngine player = new AudioPlaybackEngine(44100, 2);

        static Dictionary<string, CachedSound> sounds = new Dictionary<string, CachedSound>();

        private static void Play(String soundName)
        {
            if (!sounds.TryGetValue(soundName, out CachedSound sound))
            {
                sound = new CachedSound("Sounds/" + soundName + ".wav");
                sounds.Add(soundName, sound);
            }
            player.PlaySound(sound);
        }

        public static void PlayCrash()
        {
            Play("Crash");
        }

        private static IntervalBlocker bumping = new IntervalBlocker(100, 5);

        public static void PlayBump()
        {
            if (bumping.IsBlocked())
            {
                return;
            }
            Play("Bump");
        }

        private static IntervalBlocker zapping = new IntervalBlocker(500, 3);

        public static void PlayZap()
        {
            if (zapping.IsBlocked())
            {
                return;
            }
            Play("Zap");
        }

        public static void PlayBoom()
        {
            Play("Explosion");
        }

        public static void PlayGulp()
        {
            Play("Gulp");
        }

        public static void PlayYay()
        {
            Play("Yay");
        }
    }

}
