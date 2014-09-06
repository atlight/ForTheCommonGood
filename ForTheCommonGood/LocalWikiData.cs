using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Text;

namespace ForTheCommonGood
{
    static class LocalWikiData
    {
        public static string LocalDomain { get; private set; }
        public static string DisplayName { get; private set; }

        public static string Category1 { get; private set; }
        public static string Category2 { get; private set; }
        public static string Category3 { get; private set; }
        public static string DefaultCategory { get; private set; }

        public static string Information { get; private set; }
        public static string Description { get; private set; }
        public static string Date { get; private set; }
        public static string Source { get; private set; }
        public static string Author { get; private set; }
        public static string Permission { get; private set; }
        public static string Other_versions { get; private set; }

        public static string NowCommonsTag { get; private set; }
        public static string CopyToCommonsRegex { get; private set; }

        public static string Summary { get; private set; }
        public static string Licensing { get; private set; }
        public static string CategoryNamespace { get; private set; }

        public static string NowCommonsDeletionSummary { get; private set; }
        public static string NowCommonsAddingTagEditSummary { get; private set; }
        public static string NowCommonsReplacingTagEditSummary { get; private set; }

        public static string LocalFtcgPage { get; private set; }

        public static string FileTalkMinimumSize { get; private set; }

        public class PotentialProblem
        {
            public string Test { get; set; }
            public bool IsRegex { get; set; }
            public string Message { get; set; }
        }
        public static PotentialProblem[] PotentialProblems { get; private set; }

        // cannot use Dictionary<string, string> or StringDictionary classes here, 
        // because they do not preserve the order in which elements are added
        public static KeyValuePair<string, string>[] Replacements { get; private set; }
        public static KeyValuePair<string, string>[] SelfLicenseReplacements { get; private set; }

        public static void LoadWikiData(string data)
        {
            LocalDomain = Category1 = Category2 = Category3 = Information = Description =
                Date = Source = Author = Permission = Other_versions = Summary = Licensing = "";
            List<PotentialProblem> problems = new List<PotentialProblem>();
            List<KeyValuePair<string, string>> replaces = new List<KeyValuePair<string, string>>();
            List<KeyValuePair<string, string>> selfReplaces = new List<KeyValuePair<string, string>>();
            DefaultCategory = "1";

            // newer properties that require default values (since they are not specified in 
            // older .wiki files)
            NowCommonsDeletionSummary = "Media file now available on Commons";
            NowCommonsAddingTagEditSummary = "Added {{now Commons}} tag";
            NowCommonsReplacingTagEditSummary = "Replaced {{move to Commons}} tag with {{now Commons}} tag";
            LocalFtcgPage = "w:en:WP:FTCG";
            FileTalkMinimumSize = "120";  // small default

            string[] lines = data.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < lines.Length; i++)
            {
                string l = lines[i];
                if (l == "" || l.StartsWith("#"))
                    continue;
                if (l == "[PotentialProblem]")
                {
                    string test = lines[i + 1];
                    string message = lines[i + 2];
                    PotentialProblem prob = new PotentialProblem();
                    prob.Test = test.Substring(test.IndexOf('=') + 1);
                    prob.IsRegex = test.StartsWith("IfRegex=");
                    prob.Message = message.Substring(message.IndexOf('=') + 1);
                    problems.Add(prob);
                    i += 2;
                }
                else if (l == "[Replacement]")
                {
                    string lookfor = lines[i + 1];
                    string replacewith = lines[i + 2];
                    replaces.Add(new KeyValuePair<string, string>(lookfor.Substring(lookfor.IndexOf('=') + 1),
                        replacewith.Substring(replacewith.IndexOf('=') + 1)));
                    i += 2;
                }
                else if (l == "[SelfLicenseReplacement]")
                {
                    string lookfor = lines[i + 1];
                    string replacewith = lines[i + 2];
                    selfReplaces.Add(new KeyValuePair<string, string>(lookfor.Substring(lookfor.IndexOf('=') + 1),
                        replacewith.Substring(replacewith.IndexOf('=') + 1)));
                    i += 2;
                }
                else
                {
                    try
                    {
                        // a very naughty little bit of covert reflection here
                        typeof(LocalWikiData).GetProperty(l.Substring(0, l.IndexOf('=')), BindingFlags.Static | BindingFlags.Public | BindingFlags.IgnoreCase)
                            .SetValue(null, l.Substring(l.IndexOf('=') + 1), null);
                    }
                    catch (Exception)
                    {
                        if (Debugger.IsAttached)
                            Debugger.Break();
                        //MessageBox.Show(Localization.GetString("LocalWikiDataLoadFailed") + "\n\n" + e.Message);
                        // silently fail for the moment
                    }
                }
            }

            // validate FileTalkMinimumSize
            int dummy;
            if (!int.TryParse(FileTalkMinimumSize, out dummy))
                FileTalkMinimumSize = "120";

            PotentialProblems = problems.ToArray();
            Replacements = replaces.ToArray();
            SelfLicenseReplacements = selfReplaces.ToArray();
        }

        public static bool LoadWikiDataHosted(string uri)
        {
            WebClient loader = new WebClient();
            loader.Headers.Add("User-Agent", MorebitsDotNet.UserAgent);
            loader.Encoding = Encoding.UTF8;
            string result;
            try
            {
                result = loader.DownloadString(new Uri(uri));
            }
            catch (Exception)
            {
                MorebitsDotNet.DefaultErrorHandler(Localization.GetString("FailedToLoadHostedLocalWikiData", uri));
                return false;
            }
            LocalWikiData.LoadWikiData(result);
            return true;
        }
    }
}
