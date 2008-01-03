using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Threading;

namespace Dope.DDXX.Utility
{
    public class Time
    {
        static float lastTime = 0;
        static float deltaTime = 0;
        static bool paused = false;

        public static float CurrentTime
        {
            get { return lastTime; }
            set { lastTime = value; }
        }

        public static float DeltaTime
        {
            get { return deltaTime; }
            set { deltaTime = value; }
        }

        public static void Step(float delta)
        {
            if (IsPaused())
            {
                deltaTime = 0;
            }
            else
            {
                deltaTime = delta;
                lastTime += delta;
            }
        }

        public static bool IsPaused()
        {
            return paused;
        }

        public static void Pause()
        {
            paused = true;
        }

        public static void Resume()
        {
            paused = false;
        }

        public static void Reset()
        {
            lastTime = 0;
            deltaTime = 0;
            paused = false;
        }
    }
}
