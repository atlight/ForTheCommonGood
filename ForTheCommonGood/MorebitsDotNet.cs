using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace ForTheCommonGood
{
    public delegate void Action();

    public enum Wiki
    {
        Local,
        Commons
    }

    public static class MorebitsDotNet
    {
        public const string UserAgent = "ForTheCommonGood [[w:en:User:This, that and the other]]";

        public const int DefaultTimeout = 45000;
        public const int UploadTimeout = 1200000;  // short timeout was blocking huge uploads

        public static string GetProtocol()
        {
            return Settings.UseHttps ? "https" : "http";
        }

        public static string GetDomain(Wiki w)
        {
            switch (w)
            {
                case Wiki.Local:
                    return Settings.LocalDomain + ".org";
                case Wiki.Commons:
                    return Settings.CommonsDomain + ".org";
                default:
                    throw new ArgumentOutOfRangeException("w");
            }
        }

        private static string GetApiUri(Wiki w)
        {
            return GetProtocol() + "://" + GetDomain(w) + "/w/api.php";
        }

        public class LoginInfo
        {
            public bool LoggedIn { get; set; }
            public string UserName { get; set; }
            public string UserID { get; set; }
            public CookieContainer CookieJar { get; set; }
        }

        // current state
        public static Dictionary<Wiki, LoginInfo> LoginSessions = new Dictionary<Wiki, LoginInfo> {
            { Wiki.Local, new LoginInfo() },
            { Wiki.Commons, new LoginInfo() }
        };

        private static void TimeoutCallback(object state, bool timedOut)
        {
            if (timedOut)
            {
                HttpWebRequest request = state as HttpWebRequest;
                if (request != null)
                    request.Abort();
            }
        }

        // a general-purpose error handler, with logic to find a parent form for the MessageBox

        private delegate DialogResult MessageBoxShowAction(IWin32Window a, string b, string c, MessageBoxButtons d, MessageBoxIcon e);

        public static void DefaultErrorHandler(string message)
        {
            DefaultErrorHandler(message, MessageBoxIcon.Stop);
        }

        public static void DefaultErrorHandler(string message, MessageBoxIcon icon)
        {
            Form fm;
            try
            {
                fm = Form.ActiveForm ?? Application.OpenForms[0];
                if (fm.InvokeRequired)
                    fm.Invoke((MessageBoxShowAction) MessageBox.Show, fm, message, Application.ProductName, MessageBoxButtons.OK, icon);
                else
                    MessageBox.Show(fm, message, Application.ProductName, MessageBoxButtons.OK, icon);
            }
            catch (Exception)
            {
                MessageBox.Show(message, Application.ProductName, MessageBoxButtons.OK, icon);
            }
        }

        // API interaction

        public static void LogIn(Wiki wiki, string userName, string password, Action onSuccess,
            Action<string> onError)
        {
            StringDictionary query = new StringDictionary
            {
                { "action", "login" },
                { "lgname", userName },
                { "lgpassword", password },
            };
            PostApi(wiki, query, delegate(XmlDocument doc)
            {
                XmlNode login = doc.GetElementsByTagName("login")[0];
                if (login.Attributes["result"].Value != "NeedToken")
                {
                    onError(Localization.GetString("MorebitsDotNet_LoginFailure", login.Attributes["result"].Value));
                    return;
                }

                StringDictionary loginQuery = new StringDictionary
                {
                    { "action", "login" },
                    { "lgname", userName },
                    { "lgpassword", password },
                    { "lgtoken", login.Attributes["token"].Value }
                };
                PostApi(wiki, loginQuery, delegate(XmlDocument innerDoc)
                {
                    XmlNode innerLogin = innerDoc.GetElementsByTagName("login")[0];
                    LoginSessions[wiki].UserName = innerLogin.Attributes["lgusername"].Value;
                    LoginSessions[wiki].UserID = innerLogin.Attributes["lguserid"].Value;
                    LoginSessions[wiki].LoggedIn = true;
                    onSuccess();
                }, onError, true, false);
            }, onError);

        }

        public static void PostApi(Wiki wiki, StringDictionary query, Action<XmlDocument> onSuccess)
        {
            PostApi(wiki, query, onSuccess, DefaultErrorHandler, false, false);
        }

        public static void PostApi(Wiki wiki, StringDictionary query, Action<XmlDocument> onSuccess,
            Action<string> onError)
        {
            PostApi(wiki, query, onSuccess, onError, false, false);
        }

        public static void PostApi(Wiki wiki, StringDictionary query, Action<XmlDocument> onSuccess,
            Action<string> onError, bool synchronous)
        {
            PostApi(wiki, query, onSuccess, onError, false, synchronous);
        }

        private static void PostApi(Wiki wiki, StringDictionary query, Action<XmlDocument> onSuccess,
            Action<string> onError, bool loggingIn, bool synchronous)
        {
            string requestContent = "format=xml&";
            foreach (DictionaryEntry i in query)
                requestContent += Uri.EscapeDataString((string) i.Key) + "=" + Uri.EscapeDataString((string) i.Value ?? "") + "&";
            requestContent = requestContent.TrimEnd('&');

            WebRequest req = HttpWebRequest.Create(GetApiUri(wiki));
            ((HttpWebRequest) req).UserAgent = UserAgent;
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";

            LoginInfo session = LoginSessions[wiki];
            if (session.CookieJar == null)
                session.CookieJar = new CookieContainer();
            ((HttpWebRequest) req).CookieContainer = session.CookieJar;

            // login doesn't seem to work properly when done asycnhronously
            if (loggingIn || synchronous)
            {
                Stream s = req.GetRequestStream();
                byte[] bytes = Encoding.UTF8.GetBytes(requestContent);
                s.Write(bytes, 0, bytes.Length);
                s.Close();
            }
            else
            {
                req.BeginGetRequestStream(delegate(IAsyncResult innerResult)
                {
                    try
                    {
                        using (Stream s = req.EndGetRequestStream(innerResult))
                        {
                            byte[] bytes = Encoding.UTF8.GetBytes(requestContent);
                            s.Write(bytes, 0, bytes.Length);
                            s.Close();
                        }
                    }
                    catch (WebException e)
                    {
                        onError(Localization.GetString("MorebitsDotNet_NetRequestFailure") + "\n\n" + e.Message);
                        return;
                    }
                }, null);
            }

            IAsyncResult result = (IAsyncResult) req.BeginGetResponse(delegate(IAsyncResult innerResult)
            {
                WebResponse resp = null;
                try
                {
                    resp = req.EndGetResponse(innerResult);
                }
                catch (WebException e)
                {
                    onError(Localization.GetString("MorebitsDotNet_NetRequestFailure") + "\n\n" + e.Message);
                    return;
                }

                XmlDocument doc = new XmlDocument();
                doc.Load(resp.GetResponseStream());

                if (loggingIn)
                {
                    // have to handle login errors (wrong password, etc.) before cookies are read
                    try
                    {
                        XmlNode login = doc.GetElementsByTagName("login")[0];
                        if (login.Attributes["result"].Value != "Success")
                        {
                            onError(Localization.GetString("MorebitsDotNet_LoginFailure", login.Attributes["result"].Value));
                            return;
                        }
                        // copy over the cookies for ".commons.wikimedia.org" (hack, not sure why it's needed...)
                        CookieCollection jar = session.CookieJar.GetCookies(new Uri(GetProtocol() + "://a." + GetDomain(Wiki.Commons)));
                        session.CookieJar.Add(jar);
                    }
                    catch (Exception x)
                    {
                        onError(Localization.GetString("MorebitsDotNet_UnknownLoginFailure") + "\n\n" + x.Message + "\n\nHere is some debugging info:\n" + doc.OuterXml);
                    }
                }
                
#if REQUEST_LOG
                // simple request logging; doesn't include request body, so it should be used in conjunction with a debug session
                System.IO.File.AppendAllText("RequestLog.txt", "====\r\n\r\n" + req.RequestUri + "\r\n\r\nREQUEST HEADERS:\r\n" + req.Headers + "\r\n\r\nRESPONSE HEADERS:\r\n" + resp.Headers);
#endif

                XmlNodeList list = doc.GetElementsByTagName("error");
                if (list.Count == 0)
                    onSuccess(doc);
                else
                    onError(Localization.GetString("MorebitsDotNet_ApiError") + "\n\n" + list[0].Attributes["info"].Value);
            }, null);

            ThreadPool.RegisterWaitForSingleObject(result.AsyncWaitHandle, new WaitOrTimerCallback(TimeoutCallback), req, DefaultTimeout, true);
        }

        public static void UploadFile(Wiki wiki, StringDictionary query, byte[] file, string fileName, //string fileMimeType, 
            string fileParamName, Action<XmlDocument> onSuccess)
        {
            UploadFile(wiki, query, file, fileName, fileParamName, onSuccess, DefaultErrorHandler);
        }

        public static void UploadFile(Wiki wiki, StringDictionary query, byte[] file, string fileName, //string fileMimeType, 
            string fileParamName, Action<XmlDocument> onSuccess, Action<string> onError)
        {
            // thanks to http://www.paraesthesia.com/archive/2009/12/16/posting-multipartform-data-using-.net-webrequest.aspx

            query.Add("format", "xml");

            WebRequest req = HttpWebRequest.Create(GetApiUri(wiki));
            ((HttpWebRequest) req).UserAgent = UserAgent;
            req.Method = "POST";

            LoginInfo session = LoginSessions[wiki];
            if (session.CookieJar == null)
                session.CookieJar = new CookieContainer();
            ((HttpWebRequest) req).CookieContainer = session.CookieJar;

            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            req.ContentType = "multipart/form-data; boundary=" + boundary;

            req.BeginGetRequestStream(delegate(IAsyncResult innerResult)
            {
                Stream stream = req.EndGetRequestStream(innerResult);

                foreach (DictionaryEntry e in query)
                {
                    string item = String.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"\r\nContent-Type: text/plain; charset=UTF-8\r\nContent-Transfer-Encoding: 8bit\r\n\r\n{2}\r\n",
                        boundary, e.Key.ToString(), e.Value.ToString());
                    byte[] bytes = Encoding.UTF8.GetBytes(item);
                    stream.Write(bytes, 0, bytes.Length);
                }

                if (file != null)
                {
                    string header = String.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"; filename=\"{2}\"\r\nContent-Type: {3}\r\n\r\n",
                        boundary, fileParamName, fileName, "text/plain; charset=UTF-8");  // last param was |fileMimeType|
                    byte[] headerbytes = Encoding.UTF8.GetBytes(header);
                    stream.Write(headerbytes, 0, headerbytes.Length);

                    stream.Write(file, 0, file.Length);

                    byte[] newline = Encoding.UTF8.GetBytes("\r\n");
                    stream.Write(newline, 0, newline.Length);
                }
                byte[] endBytes = Encoding.UTF8.GetBytes("--" + boundary + "--");
                stream.Write(endBytes, 0, endBytes.Length);
                stream.Close();
            }, null);

            IAsyncResult result = (IAsyncResult) req.BeginGetResponse(new AsyncCallback(delegate(IAsyncResult innerResult)
            {
                WebResponse resp = null;
                try
                {
                    resp = req.EndGetResponse(innerResult);
                }
                catch (WebException e)
                {
                    onError(Localization.GetString("MorebitsDotNet_NetRequestFailure") + "\n\n" + e.Message);
                    return;
                }

                XmlDocument doc = new XmlDocument();
                doc.Load(resp.GetResponseStream());

#if REQUEST_LOG
                // simple request logging; doesn't include request body, so it should be used in conjunction with a debug session
                System.IO.File.AppendAllText("RequestLog.txt", "====\r\n\r\n" + req.RequestUri + "\r\n\r\nREQUEST HEADERS:\r\n" + req.Headers + "\r\n\r\nRESPONSE HEADERS:\r\n" + resp.Headers);
#endif

                XmlNodeList list = doc.GetElementsByTagName("error");
                if (list.Count == 0)
                    onSuccess(doc);
                else
                    onError(Localization.GetString("MorebitsDotNet_ApiError") + "\n\n" + list[0].Attributes["info"].Value);
            }), null);

            ThreadPool.RegisterWaitForSingleObject(result.AsyncWaitHandle, new WaitOrTimerCallback(TimeoutCallback), req, UploadTimeout, true);
        }

        public class ActionCompleted
        {
            private int num;
            private object syncLock = new object();
            private bool done = false;

            public event Action Done;
            public event Action Finally;

            public ActionCompleted(int actionCount)
            {
                num = actionCount;
            }
            public void DoneOne()
            {
                if (done)
                    return;
                lock (syncLock)
                {
                    num--;
                    if (num <= 0)
                    {
                        done = true;
                    }
                }
                if (num <= 0)
                {
                    Done();
                    // Finally();   -- this class is only used once, and this line causes problems there?
                }
            }
            public void Fail()
            {
                if (done)
                    return;
                lock (syncLock)
                {
                    num = 0;
                    done = true;
                }
                Finally();
            }
        }

    }
}
