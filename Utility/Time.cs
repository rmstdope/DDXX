using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Threading;

namespace Utility
{
    public class Time
    {
        static bool initialized = false;
        static long lastTime = 0;
        static long startTime = 0;
        static long frequency = 0;
        static float deltaTime = 0;
        static bool paused = false;
        static long pausedTime = 0;

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

                if (paused)
                {
                    return (float)pausedTime / (float)frequency;
                }
                else
                {
                    return (float)lastTime / (float)frequency;
                }
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
                    return (float)pausedTime / (float)frequency;
                }
                else
                {
                    QueryPerformanceCounter(out time);
                    return (float)(time - startTime) / (float)frequency;
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

        internal static void Initialize()
        {
            initialized = true;
            QueryPerformanceFrequency(out frequency);
            QueryPerformanceCounter(out startTime);
            lastTime = 0;
        }

        internal static void Step()
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

        internal static void Pause()
        {
            paused = true;
            pausedTime = lastTime;
        }

        internal static void Resume()
        {
            paused = false;
        }
    }
}
