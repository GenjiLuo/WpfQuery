using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Collections;
using System.Diagnostics;
using System.Web;

namespace WpfQuery
{
    class PostToUrl
    {
        const string boundary = "---------7d930d1a850658";
        public PostToUrl()
        {
        }

        public struct _ContentType
        {
            public static string Form = "application/x-www-form-urlencoded";
            public static string Json = "application/json";
            public static string Xml = "application/xml";
            public static string File = "multipart/form-data; boundary=" + boundary;
        }
        public static string ContentType = _ContentType.Form;

        public struct _Encoding
        {
            public static Encoding UTF8 = Encoding.UTF8;
            public static Encoding GB2312 = Encoding.GetEncoding("gb2312");
            public static Encoding ASCII = Encoding.ASCII;
        }

        public static Encoding RequestEncoding = _Encoding.UTF8;

        public static Encoding ResponseEncoding = _Encoding.UTF8;

        const string sUserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.2; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
        //const string sContentType = "application/x-www-form-urlencoded";
        //const string sContentType = "application/json; charset=utf-8";

        public static string PostDataToUrl(string data, string url,CookieContainer cc)
        {
            Encoding encoding = RequestEncoding;
            byte[] bytesToPost = encoding.GetBytes(data);
            return PostDataToUrl(bytesToPost, url,cc);
        }

        public static string PostDataToUrl(byte[] data, string url, CookieContainer cc)
        {
            WebRequest webRequest = WebRequest.Create(url);
            HttpWebRequest httpRequest = webRequest as HttpWebRequest;
            if (httpRequest == null)
            {
                return "";
            }
            httpRequest.CookieContainer = cc;

            httpRequest.UserAgent = sUserAgent;
            httpRequest.ContentType = ContentType;
            httpRequest.Method = "POST";

            httpRequest.ContentLength = data.Length;
            Stream requestStream = httpRequest.GetRequestStream();
            requestStream.Write(data, 0, data.Length);
            requestStream.Close();

            Stream responseStream;
            try
            {
                responseStream = httpRequest.GetResponse().GetResponseStream();
            }
            catch (Exception e)
            {
                return "【服务器返回错误，错误信息：\"" + e.Message + "\"】";
            }

            string stringResponse = "";
            using (StreamReader responseReader = new StreamReader(responseStream, ResponseEncoding))
            {
                stringResponse = responseReader.ReadToEnd();
            }
            responseStream.Close();


            return stringResponse;

        }

        //public static string PostFileToUrl(Hashtable param, List<string> files, string url)
        //{
        //    // 文本域的数据
        //    StringBuilder sbParam = new StringBuilder();
        //    foreach (string key in param.Keys)
        //    {
             
        //        string value = param[key].ToString();
        //        sbParam.Append("--" + boundary);
        //        sbParam.Append("\r\n");
        //        sbParam.Append("Content-Disposition: form-data; name=\"" + key + "\"");
        //        sbParam.Append("\r\n\r\n");
        //        sbParam.Append(URLEncode(value));
        //        sbParam.Append("\r\n");
        //    }
        //    byte[] paramBytes = Encoding.UTF8.GetBytes(sbParam.ToString());

        //    byte[][] PostHeaderBytes = new byte[files.Count][];
        //    byte[][] PostFileBytes = new byte[files.Count][];

        //    // 文件域的数据 
        //    for (int i = 0; i < files.Count; i++)
        //    {
        //        StringBuilder sb = new StringBuilder();

        //        sb.Append("\r\n--" + boundary);
        //        sb.Append("\r\n");
        //        sb.Append("Content-Disposition: form-data; name=\"file" + i + "\";filename=\"" + files[i] + "\"");
        //        sb.Append("\r\n");

        //        sb.Append("Content-Type: ");
        //        sb.Append("application/octet-stream");
        //        sb.Append("\r\n\r\n");

        //        PostHeaderBytes[i] = Encoding.UTF8.GetBytes(sb.ToString());
        //        FileStream fs = new FileStream(files[i], FileMode.Open);
        //        PostFileBytes[i] = new byte[fs.Length];
        //        fs.Read(PostFileBytes[i], 0, (int)fs.Length);
        //        fs.Close();
        //    }

        //    byte[] PostEndBytes = Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");


        //    // 返回数据流
        //    int length = 0;
        //    length += paramBytes.Length;
        //    for (int i = 0; i < files.Count; i++)
        //    {
        //        length += PostHeaderBytes[i].Length;
        //        length += PostFileBytes[i].Length;
        //    }

        //    length += PostEndBytes.Length;
        //    byte[] data = new byte[length];

        //    int pos = 0;
        //    paramBytes.CopyTo(data, pos);
        //    pos += paramBytes.Length;

        //    for (int i = 0; i < files.Count; i++)
        //    {
        //        PostHeaderBytes[i].CopyTo(data, pos);
        //        pos += PostHeaderBytes[i].Length;
        //        PostFileBytes[i].CopyTo(data, pos);
        //        pos += PostFileBytes[i].Length;
        //    }

        //    PostEndBytes.CopyTo(data, pos);
        //    pos += PostEndBytes.Length;

        //    Debug.Assert(pos == length, string.Format("error: pos{0} length{1}", pos, length));

        //    return PostDataToUrl(data, url);
        //}

        private static string URLEncode(string value)
        {
            return (HttpUtility.UrlEncode(value, Encoding.UTF8).Replace("+", "%20").Replace("%2b", "+"));
        }


        //-------------------------------------
        public static string SendDataByGET(string Url, string postDataStr, CookieContainer cookie)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url + (postDataStr == "" ? "" : "?") + postDataStr);
            if (cookie.Count == 0)
            {
                request.CookieContainer = new CookieContainer();
                cookie = request.CookieContainer;
            }
            else
            {
                request.CookieContainer = cookie;
            }
            //request.CookieContainer = cookie;

            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }

    }
}
