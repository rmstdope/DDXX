using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Threading;

namespace Dope.DDXX.Utility
{
    public class Time
    {
        static bool initialized = false;
        static long lastTime = 0;
        static long startTime = 0;
        static long frequency = 0;
        static float deltaTime = 0;
        static bool paused = false;
        //static long pausedTime = 0;

        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceCounter(out long lpPerformanceCount);

        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceFrequency(out long lpFrequency);

        public static float StepTime
        {
            get
            {
                if (!initialized)
                    throw new InvalidOperationException("Time.Initialize() must be called first.");

                return (float)lastTime / (float)frequency;
            }
        }

        public static float CurrentTime
        {
            get
            {
                if (!initialized)
                    throw new InvalidOperationException("Time.Initialize() must be called first.");
                long time;
                if (paused)
                {
                    return StepTime;
                }
                else
                {
                    QueryPerformanceCounter(out time);
                    return (float)(time - startTime) / (float)frequency;
                }
            }
            set
            {
                if (!initialized)
                    throw new InvalidOperationException("Time.Initialize() must be called first.");
                if (paused)
                {
                    lastTime = (long)(value * frequency);
                }
                else
                {
                    float delta = value - CurrentTime;
                    startTime -= (long)(delta * frequency);
                    lastTime = (long)(value * frequency);
                }
            }
        }

        public static float DeltaTime
        {
            get
            {
                if (!initialized)
                    throw new InvalidOperationException("Time.Initialize() must be called first.");
                return deltaTime;
            }
        }

        public static void Initialize()
        {
            if (!initialized)
            {
                initialized = true;
                QueryPerformanceFrequency(out frequency);
                QueryPerformanceCounter(out startTime);
                lastTime = 0;
                deltaTime = 0.0f;
                paused = false;
            }
        }

        public static void Step()
        {
            long time;
            if (paused)
            {
                deltaTime = 0.0f;
            }
            else
            {
                QueryPerformanceCounter(out time);
                deltaTime = (float)(time - lastTime - startTime) / (float)frequency;
                lastTime = time - startTime;
            }
        }

        public static void Pause()
        {
            paused = true;
        }

        public static void Resume()
        {
            if (paused)
            {
                paused = false;
                CurrentTime = (float)lastTime / (float)frequency;
            }
        }
    }
}
