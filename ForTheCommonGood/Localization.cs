using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using System.IO;

namespace ForTheCommonGood
{
    internal static class Localization
    {
        // default strings
        private static StringDictionary DefaultStrings;

        private static StringDictionary CurrentStrings;

        internal static string GetString(string key)
        {
#if DEBUG
            if ((Localized ? CurrentStrings[key] : DefaultStrings[key]) == null)
                System.Diagnostics.Debugger.Break();
#endif
            return (Localized ? CurrentStrings[key] : DefaultStrings[key]) ?? key;
        }

        internal static string GetString(string key, params string[] arguments)
        {
#if DEBUG
            if ((Localized ? CurrentStrings[key] : DefaultStrings[key]) == null)
                System.Diagnostics.Debugger.Break();
#endif
            return (Localized ?
                (CurrentStrings[key] == null ? key : String.Format(CurrentStrings[key], arguments)) :
                (DefaultStrings[key] == null ? key : String.Format(DefaultStrings[key], arguments)));
        }

        internal static bool Localized { get; private set; }

        internal static void Init()
        {
            // load default strings from resources
            DefaultStrings = new StringDictionary();
            foreach (string l in Properties.Resources.DefaultStrings.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
                if (l.Length > 0 && !l.StartsWith("#"))
                    DefaultStrings.Add(l.Substring(0, l.IndexOf("=")), l.Substring(l.IndexOf("=") + 1));

            // look for localized language file
            Localized = false;
            if (File.Exists("ForTheCommonGood.lang"))
            {
                Localized = true;
                CurrentStrings = new StringDictionary();
                foreach (string l in File.ReadAllLines("ForTheCommonGood.lang", Encoding.UTF8))
                    if (l.Length > 0 && !l.StartsWith("#"))
                        CurrentStrings.Add(l.Substring(0, l.IndexOf("=")), l.Substring(l.IndexOf("=") + 1));
            }
        }
    }
}
