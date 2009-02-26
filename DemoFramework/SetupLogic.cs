using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;
using Dope.DDXX.Utility;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace Dope.DDXX.DemoFramework
{
    public class SetupLogic
    {
        private ISetupDialog dialog;
        private bool ok;
        private const string DEFAULTS_FILENAME = ".dope_setup_defaults";

        public SetupLogic()
        {
        }

        public bool OK
        {
            get { return ok; }
            set { ok = value; }
        }

        public ISetupDialog Dialog
        {
            set { dialog = value; }
        }

        public DeviceParameters DeviceParameters
        {
            get { return new DeviceParameters(ResolutionWidth, ResolutionHeight, !dialog.Windowed, dialog.Reference, dialog.Multisampling, false); }
        }

        public int ResolutionWidth
        {
            get { return Int32.Parse(dialog.SelectedResolution.Split('x')[0]); }
        }

        public int ResolutionHeight
        {
            get { return Int32.Parse(dialog.SelectedResolution.Split('x')[1]); }
        }

        public void UpdateResolution(AspectRatio.Ratios ratio)
        {
            int i;
            DisplayMode[] modes = GetDisplayModes(ratio);
            Array.Sort(modes, CompareDM);

            string[] values = new string[modes.Length];
            int n = 0;
            foreach (DisplayMode m in modes)
            {
                values[n++] = m.Width + "x" + m.Height;
            }

            ArrayList noDups = new ArrayList();
            for (i = 0; i < values.Length; i++)
            {
                if (!noDups.Contains(values[i].Trim()))
                {
                    noDups.Add(values[i].Trim());
                }
            }
            string[] uniqueValues = new string[noDups.Count];
            noDups.CopyTo(uniqueValues);
            dialog.Resolution = uniqueValues;
        }

        public DisplayMode[] GetDisplayModes(AspectRatio.Ratios ratio)
        {
            List<DisplayMode> list = new List<DisplayMode>();
            foreach (DisplayMode mode in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes)
            {
                if (new AspectRatio(mode.Width, mode.Height).Ratio == ratio)
                {
                    list.Add(mode);
                }
            }
            return list.ToArray();
        }
        
        private static int CompareDM(DisplayMode m1, DisplayMode m2)
        {
            if (m1.Height == m2.Height)
            {
                if (m1.Width == m2.Width)
                    return 0;
                else if (m1.Width > m2.Width)
                    return 1;
                else
                    return -1;
            }
            else if (m1.Height > m2.Height)
                return 1;
            else
                return -1;
        }

        private static bool EqualsDM(DisplayMode m1, DisplayMode m2)
        {
            return m1.Height == m2.Height && m1.Width == m2.Width;
        }

        public void Initialize()
        {
            if (CheckAndReadSavedValues())
            {
                return;
            }
            DisplayMode[] modes;

            modes = GetDisplayModes(AspectRatio.Ratios.RATIO_4_3);
            if (modes.Length == 0)
                dialog.EnableRadio4_3 = false;
            else
                dialog.CheckedRadio4_3 = true;

            modes = GetDisplayModes(AspectRatio.Ratios.RATIO_16_10);
            if (modes.Length == 0)
                dialog.EnableRadio16_10 = false;
            else
                dialog.CheckedRadio16_10 = true;

            modes = GetDisplayModes(AspectRatio.Ratios.RATIO_16_9);
            if (modes.Length == 0)
                dialog.EnableRadio16_9 = false;
            else
                dialog.CheckedRadio16_9 = true;

            dialog.Reference = false;
            dialog.Windowed = true;
        }

        public void SaveDefaultValues()
        {
            File.Delete(DEFAULTS_FILENAME);
            StreamWriter writer = new StreamWriter(DEFAULTS_FILENAME);
            WriteKeyValue(writer, "resolution", dialog.SelectedResolution);
            WriteKeyBoolValue(writer, "windowed", dialog.Windowed);
            WriteKeyBoolValue(writer, "reference", dialog.Reference);
            WriteKeyBoolValue(writer, "multisampling", dialog.Multisampling);
            writer.Close();
            writer.Dispose();
        }

        private static void WriteKeyBoolValue(StreamWriter writer, string key, bool value)
        {
            WriteKeyValue(writer, key, value ? "true" : "false");
        }

        private static void WriteKeyValue(StreamWriter writer, string key, string value)
        {
            writer.WriteLine("{0}={1}", key, value);
        }

        private bool CheckAndReadSavedValues()
        {
            if (System.IO.File.Exists(DEFAULTS_FILENAME))
            {
                return ReadAndSetSavedValues();
            }
            return false;
        }

        private bool ReadAndSetSavedValues()
        {
            Regex rx = new Regex(@"\b(?<key>\w+)=(?<value>\w+)\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            StreamReader reader = new StreamReader(DEFAULTS_FILENAME);
            try
            {
                string value = ReadToken(reader, rx, "resolution");
                int width = Int32.Parse(value.Split('x')[0]);
                int height = Int32.Parse(value.Split('x')[1]);
                AspectRatio.Ratios ratio = new AspectRatio(width, height).Ratio;
                if (ratio == AspectRatio.Ratios.RATIO_4_3)
                    dialog.CheckedRadio4_3 = true;
                else if (ratio == AspectRatio.Ratios.RATIO_16_9)
                    dialog.CheckedRadio16_9 = true;
                else if (ratio == AspectRatio.Ratios.RATIO_16_10)
                    dialog.CheckedRadio16_10 = true;
                else
                    throw new InvalidDataException("ratio invalid");
                dialog.SelectedResolution = width + "x" + height;
                dialog.Windowed = ReadBoolToken(reader, rx, "windowed");
                dialog.Reference = ReadBoolToken(reader, rx, "reference");
                dialog.Multisampling = ReadBoolToken(reader, rx, "multisampling");
                return true;
            }
            catch (Exception e)
            {
                Debug.Print("Error reading defaults file: {0}", e.ToString());
                return false;
            }
            finally
            {
                reader.Close();
                reader.Dispose();
            }
        }

        private bool ReadBoolToken(StreamReader reader, Regex rx, string key)
        {
            return ReadToken(reader, rx, key) == "true";
        }

        private string ReadToken(StreamReader reader, Regex rx, string key)
        {
            string line = reader.ReadLine();
            MatchCollection matches = rx.Matches(line);
            if (matches.Count != 1)
            {
                throw new InvalidDataException("invalid line: " + line);
            }
            if (key != matches[0].Groups["key"].Value)
            {
                throw new InvalidDataException("Expected " + key + " but found " + matches[0].Groups["key"]);
            }
            return matches[0].Groups["value"].Value;
        }
    }
}
