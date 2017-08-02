using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Windows.Forms;
using System.Xml;

namespace ForTheCommonGood
{
    /// <summary>
    /// Generates a sequence of files which the user moves through one at a time.
    /// </summary>
    abstract class FileSequence
    {
        public abstract void NextFile(Action<string> successCallback, MorebitsDotNetError errorCallback,
            MethodInvoker exhaustedCallback);

        // the blacklist contains files that we have tried to load and found that
        // they contain {{now Commons}}, so we should skip over them
        private List<string> fileBlacklist = new List<string>();
        public virtual void BlacklistFile(string fileName)
        {
            fileBlacklist.Add(fileName);
        }
        public virtual bool IsFileBlacklisted(string fileName)
        {
            return fileBlacklist.Contains(fileName);
        }
    }

    /// <summary>
    /// Represents a sequence of files drawn from a user-selected text file, with
    /// one file on each line.
    /// </summary>
    class TextFileSequence: FileSequence
    {
        public string FileName { get; protected set; }
        public bool IsDirty { get; protected set; }

        private Queue<string> fileLines;

        public TextFileSequence(string fileName)
        {
            FileName = fileName;
            IsDirty = false;
        }

        public override void NextFile(Action<string> successCallback, MorebitsDotNetError errorCallback,
            MethodInvoker exhaustedCallback)
        {
            // initialise the file cache if needed
            if (fileLines == null)
            {
                string[] linesRaw;
                try
                {
                    linesRaw = File.ReadAllLines(FileName);
                }
                catch (Exception e)
                {
                    errorCallback(Localization.GetString("FailedToReadTextFile", FileName) + "\n\n" + e.Message);
                    return;
                }

                fileLines = new Queue<string>();
                foreach (string str in linesRaw)
                    if (!String.IsNullOrEmpty(str))
                        fileLines.Enqueue(str);
            }

            // work through the file sequentially
            try
            {
                string fileName = fileLines.Dequeue();
                IsDirty = true;
                successCallback(fileName);
            }
            catch (InvalidOperationException)
            {
                // we've run out of files!
                exhaustedCallback();
            }
        }

        public string[] GetFileLines()
        {
            return fileLines == null ? null : fileLines.ToArray();
        }
    }

    /// <summary>
    /// Retrieves a list of files from the MediaWiki API and steps through the
    /// list at random.
    /// </summary>
    abstract class RandomApiFileSequence: FileSequence
    {
        private string continueString = null;
        private List<string> randomCache = new List<string>(500);
        private Random rand = new Random();

        protected abstract string GetApiPrefix();
        protected abstract string GetApiXmlElementName();
        protected abstract StringDictionary GetApiQuery();

        public override void NextFile(Action<string> successCallback, MorebitsDotNetError errorCallback,
            MethodInvoker exhaustedCallback)
        {
            if (randomCache.Count == 0)
                LoadFromApi(GetApiQuery(), successCallback, errorCallback, exhaustedCallback);
            else
                LoadFileCore(successCallback);
        }

        private void LoadFileCore(Action<string> successCallback)
        {
            int index = rand.Next(randomCache.Count);
            successCallback(randomCache[index]);
            randomCache.RemoveAt(index);
        }

        protected void LoadFromApi(StringDictionary query, Action<string> successCallback, MorebitsDotNetError errorCallback,
            MethodInvoker exhaustedCallback)
        {
            if (continueString != null)
                query.Add(GetApiPrefix() + "continue", continueString);
            MorebitsDotNet.PostApi(Wiki.Local, query, delegate(XmlDocument doc)
            {
                foreach (XmlNode i in doc.GetElementsByTagName(GetApiXmlElementName()))
                    if (!IsFileBlacklisted(i.Attributes["title"].Value))
                        randomCache.Add(i.Attributes["title"].Value);

                if (randomCache.Count == 0)
                {
                    exhaustedCallback();
                    return;
                }

                XmlNodeList continues = doc.GetElementsByTagName("query-continue");
                if (continues.Count > 0)
                    continueString = continues[0].FirstChild.Attributes[GetApiPrefix() + "continue"].Value;
                else
                    continueString = null;

                LoadFileCore(successCallback);
            }, errorCallback, WebRequestMethods.Http.Get);
        }

        public override void BlacklistFile(string fileName)
        {
            base.BlacklistFile(fileName);
            // remove this file from the current cache, if it is present
            randomCache.Remove(fileName);
        }
    }

    class CategoryRandomApiFileSequence: RandomApiFileSequence
    {
        protected override string GetApiPrefix()
        {
            return "cm";
        }
        
        protected override string GetApiXmlElementName()
        {
            return "cm";
        }

        protected override StringDictionary GetApiQuery()
        {
            return new StringDictionary {
                { "action", "query" },
                { "list", "categorymembers" },
                { "cmtitle", "Category:" + CategoryName },
                { "cmnamespace", "6" },  // File:, obviously
                { "cmsort", "timestamp" },
                { "cmprop", "title" },
                { "cmlimit", "500" },
                { "rawcontinue", "" },
            };
        }

        public string CategoryName { get; protected set; }

        public CategoryRandomApiFileSequence(string categoryName)
        {
            CategoryName = categoryName;
        }
    }

    class UserRandomApiFileSequence: RandomApiFileSequence
    {
        protected override string GetApiPrefix()
        {
            return "ai";
        }
        
        protected override string GetApiXmlElementName()
        {
            return "img";
        }

        protected override StringDictionary GetApiQuery()
        {
            return new StringDictionary {
                { "action", "query" },
                { "list", "allimages" },
                { "aiuser", UserName },
                { "aisort", "timestamp" },
                { "aiprop", "" },
                { "ailimit", "500" },
                { "rawcontinue", "" },
            };
        }

        public string UserName { get; protected set; }

        public UserRandomApiFileSequence(string userName)
        {
            UserName = userName;
        }
    }
}
