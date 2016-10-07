using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using mshtml;
using System.Threading;

namespace WpfQuery
{
    /// <summary>
    /// DownloadWeb.xaml 的交互逻辑
    /// </summary>
    public partial class DownloadWeb : UserControl
    {
        string url = "";
        string encodingstr = "utf-8";
        string filename = "";
        string foldername = "";
        string mode = "";
        string log = "";
        Thread t;
        List<String> imgList = new List<String>();
        List<String> cssList = new List<String>();//绝对地址
        List<String> jsList = new List<String>();
        List<String> swfList = new List<String>();
        List<String> imgListB = new List<String>();
        List<String> cssListB = new List<String>();//源地址
        List<String> jsListB = new List<String>();
        List<String> swfListB = new List<String>();
        List<String> cssListC = new List<String>();//本地化地址
        List<String> imgListC = new List<String>();
        List<String> jsListC = new List<String>();
        List<String> swfListC = new List<String>();
        List<String> cssListD = new List<String>();//背景图片地址
        bool UrlLog = false;
        string result = null;
        string domain = "";

        public DownloadWeb()
        {
            InitializeComponent();
        }

        #region 下载按钮
        private void Download_Click(object sender, RoutedEventArgs e)
        {
            //判断网址
            url = this.WebUrl.Text.Trim();
            if(!Common.isurl(url) || url.Replace("http://","") == "")
            {
                MessageBox.Show("请输入有效的网址！");
                return;
            }
            //判断文件保存路径
            filename = this.Filepath.Text.Trim();
            if (filename == "")
            {
                filename = System.Environment.CurrentDirectory;
            }
            //判断路径合法
            foldername = System.IO.Path.GetDirectoryName(filename);
            if (foldername == "")
            {
                MessageBox.Show("请输入有效的路径！");
                return;
            }
            //判断编码
            if (rbUft8.IsChecked == true)
            {
                encodingstr = "utf-8";
            }
            else
            {
                encodingstr = "gb2312";
            }
            //判断保存形式
            mode = "path";
            if (rbPathMode.IsChecked == true)
            {
                mode = "path";
            }
            else
            {
                mode = "type";
                CreateFolder();
            }
            //写出文件下载地址与保存地址对应日志
            if (chkUrlLog.IsChecked == true)
            {
                UrlLog = true;
            }
            else
            {
                UrlLog = false;
            }

            if (filename != "" && Directory.Exists(foldername))
            {

                t = new Thread(Down);
                t.Start();
            }
            else
            {
                MessageBox.Show("请输入网页保存路径！");
            }
        }
        #endregion

        #region 分析
        public void Down()
        {
            //初始化
            imgList.Clear();
            cssList.Clear();
            jsList.Clear();
            swfList.Clear();
            imgListB.Clear();
            cssListB.Clear();
            jsListB.Clear();
            swfListB.Clear();
            cssListC.Clear();
            imgListC.Clear();
            jsListC.Clear();
            swfListC.Clear();
            cssListD.Clear();

            OutDelegateButtonDown downdelegate = new OutDelegateButtonDown(OutDelegateDown);
            this.Dispatcher.BeginInvoke(downdelegate);
            OutDelegateAddO adddelegate = new OutDelegateAddO(OutTextAddO);
            log = "#程序开始运行...\r\n";
            this.Dispatcher.BeginInvoke(adddelegate, new object[1] { log });
            log = "#开始下载网页：" + url + "\r\n";
            this.Dispatcher.BeginInvoke(adddelegate, new object[1] { log });
            try
            {
                result = GetWebClient(url);
                //StreamWriter sw = new StreamWriter(filename, false, Encoding.GetEncoding(encodingstr));
                //sw.Write(result);
                //sw.Close();
            }
            catch (Exception ex)
            {
                txtLog.Text += ex.Message + "\r\n";
                return;
            }
            log = "#下载网页：" + url + "完成！\r\n";
            this.Dispatcher.BeginInvoke(adddelegate, new object[1] { log });
            log = "#开始下载其它网页文件：\r\n#开始获取CSS文件地址，如果网页较大，请耐心等待！\r\n#开始获取JS文件地址，如果网页较大，请耐心等待！\r\n#开始获取IMG文件地址，如果网页较大，请耐心等待！\r\n#开始获取CSS背景图片地址，如果网页较大，请耐心等待！\r\n#开始获取SWF文件地址，如果网页较大，请耐心等待！\r\n";
            this.Dispatcher.BeginInvoke(adddelegate, new object[1] { log });

            domain = "http://" + GetUrlDomainName(url);
            //下载CSS
            log = "#开始下载CSS文件...\r\n";
            this.Dispatcher.BeginInvoke(adddelegate, new object[1] { log });
            cssList = getLinkHref(result);
            foreach (string src in cssList)
            {
                GetWebImage(src, foldername + "\\css" + "\\" + GetUrlFileName(src), "css/" + GetUrlFileName(src));
                cssListC.Add("css/"+GetUrlFileName(src));
            }



            ConvertToAbsoluteUrls(result, new Uri(domain));

            //新方法
            log = "#开始下载IMG图片...\r\n";
            this.Dispatcher.BeginInvoke(adddelegate, new object[1] { log });
            foreach (string src in imgList)
            {
                GetWebImage(src, foldername + "\\images" + "\\" + GetUrlFileName(src), "images/" + GetUrlFileName(src));
                imgListC.Add("images/" + GetUrlFileName(src));
            }
            log = "#开始下载JS文件...\r\n";
            this.Dispatcher.BeginInvoke(adddelegate, new object[1] { log });
            foreach (string src in jsList)
            {
                GetWebImage(src, foldername + "\\js" + "\\" + GetUrlFileName(src), "js/" + GetUrlFileName(src));
                jsListC.Add("js/" + GetUrlFileName(src));
            }
            log = "#开始下载SWF文件...\r\n";
            this.Dispatcher.BeginInvoke(adddelegate, new object[1] { log });
            foreach (string src in swfList)
            {
                GetWebImage(src, foldername + "\\swf" + "\\" + GetUrlFileName(src), "swf/" + GetUrlFileName(src));
                swfListC.Add("swf/" + GetUrlFileName(src));
            }

            log = "#开始下载CSS文件内背景图片...\r\n";
            this.Dispatcher.BeginInvoke(adddelegate, new object[1] { log });
            foreach (string src in cssList)
            {
                string path = src.Replace(GetUrlFileName(src), "");
                string cssstr = GetWebClient(src);
                string regexstr = "url\\(.*?\\)";
                Regex r = new Regex(regexstr, RegexOptions.IgnoreCase);
                MatchCollection mc = r.Matches(cssstr);
                foreach (Match m in mc)
                {
                    string bgimg = path + m.Value.Replace("url(", "").Replace(")", "");
                    if (!cssListD.Contains(bgimg))
                    {
                        cssListD.Add(bgimg);
                    }
                }
            }
            foreach (string src in cssListD)
            {
                GetWebImage(src, foldername + "\\images" + "\\" + GetUrlFileName(src), "images/" + GetUrlFileName(src));
            }

            log = "#开始本地化文件...\r\n";
            this.Dispatcher.BeginInvoke(adddelegate, new object[1] { log });
            for (int i = 0; i < cssListC.Count; i++)
            {
                result = result.Replace(cssListB[i], cssListC[i]);
            }
            for (int i = 0; i < imgList.Count; i++)
            {
                result = result.Replace(imgListB[i], imgListC[i]);
            }
            for (int i = 0; i < jsList.Count; i++)
            {
                result = result.Replace(jsListB[i], jsListC[i]);
            }
            for (int i = 0; i < swfList.Count; i++)
            {
                result = result.Replace(swfListB[i], swfListC[i]);
            }

            StreamWriter sw2 = new StreamWriter(filename, false, Encoding.GetEncoding(encodingstr));
            sw2.Write(result);
            sw2.Close();

            log = "#所有本地化文件完成！\r\n#运行完成！";
            this.Dispatcher.BeginInvoke(adddelegate, new object[1] { log });
            //修改下载按钮可视状态
            this.Dispatcher.BeginInvoke(downdelegate);
        }
        #endregion

        #region 修改static 获取CSS文件绝对路径
        public List<String> getLinkHref(String htmlCode)
        {
            List<String> cssSrcList = new List<String>();
            string regexstr = "<link.*?>";
            Regex r = new Regex(regexstr, RegexOptions.IgnoreCase);
            MatchCollection mc = r.Matches(htmlCode);
            foreach (Match m in mc)
            {
                Regex imgstr = new Regex("href=\".*?\"", RegexOptions.IgnoreCase);
                Match ma = imgstr.Match(m.Value);
                string imgurl = ma.Value.Split('"')[1];
                if (imgurl != "")
                {
                    cssListB.Add(imgurl);
                    if (imgurl.Substring(0,1) =="/")
                    {
                        cssSrcList.Add(domain+imgurl);
                    }
                    else if (imgurl.Contains("http:"))
                    {
                        cssSrcList.Add(imgurl);
                    }
                    else
                    {
                        cssSrcList.Add(url.Replace(GetUrlFileName(url), "") + imgurl);
                    }
                }
            }
            return cssSrcList;
        }
        #endregion

        #region 下载文件
        private void GetWebImage(string imgurl,string filepath,string savepath)
        {
            WebClient mywebclient = new WebClient();
            try
            {
                mywebclient.DownloadFile(imgurl, filepath);
                string log = "#" + imgurl + "下载完成！\r\n";
                if (UrlLog)
                {
                    log = "#" + imgurl + "--->" + savepath + "下载完成！\r\n";
                }
                OutDelegateAddO adddelegate = new OutDelegateAddO(OutTextAddO);
                this.Dispatcher.BeginInvoke(adddelegate, new object[1] { log });
            }
            catch
            {
                string log = "#" + imgurl + "未找到文件！\r\n";
                OutDelegateAddO adddelegate = new OutDelegateAddO(OutTextAddO);
                this.Dispatcher.BeginInvoke(adddelegate, new object[1] { log });
            }
        }
        #endregion

        #region 修改界面
        public delegate void OutDelegateAddO(string log);
        public void OutTextAddO(string log)
        {
            txtLog.Text += log;
            txtLog.Select(txtLog.Text.Length, 0);
            Keyboard.Focus(txtLog);
        }

        public delegate void OutDelegateButtonDown();
        public void OutDelegateDown()
        {
            if (this.Download.IsEnabled == true)
            {
                this.Download.IsEnabled = false;
            }
            else
            {
                this.Download.IsEnabled = true;
            }
        }
        #endregion

        #region 获取网址中域名,目录,文件名的函数
        public string GetUrlDomainName2(string strHtmlPagePath)
        {
            string p = @"http://[^\.]*\.(?<domain>[^\.]*)";
            Regex reg = new Regex(p, RegexOptions.IgnoreCase);
            Match m = reg.Match(strHtmlPagePath);
            return m.Groups["domain"].Value;
        }

        public string GetUrlDomainName(string strHtmlPagePath)
        {
            string p = @"(?<=http://)[\w\.]+[^/]";
            Regex reg = new Regex(p, RegexOptions.IgnoreCase);
            Match m = reg.Match(strHtmlPagePath);
            return m.Groups[0].Value;
        }
        public string[] GetUrlFolerName(string strHtmlPagePath)
        {
            //抓取网址字符串中的文件目录

            int at = 0;
            int start = 0;
            int notei = 0;
            int endi = 0;
            int[] myIntArray = new int[10];
            string[] ArrayFolderName = null;
            string NewFolderName;
            while ((start < strHtmlPagePath.Length) && (at > -1))
            {
                at = strHtmlPagePath.IndexOf('/', start);
                if (at == -1) break;
                myIntArray[notei] = at;
                start = at + 1;
                notei = notei + 1;
                endi = at;
            }
            ArrayFolderName = new string[notei - 1];
            for (int i = 0; i < notei; i++)
            {
                if (myIntArray[i] > 0)
                {
                    if (myIntArray[i + 1] > 0)
                    {
                        NewFolderName = strHtmlPagePath.Substring(myIntArray[i] + 1, myIntArray[i + 1] - myIntArray[i] - 1);
                        ArrayFolderName.SetValue(NewFolderName, i);
                    }

                }
            }
            return ArrayFolderName;
        }

        public string GetUrlFileName(string strHtmlPagePath)
        {
            //抓取网址字符串中的文件名称
            int at = 0;
            int start = 0;
            int notei = 0;
            int endi = 0;
            int[] myIntArray = new int[10];
            string NewFileName = "";
            while ((start < strHtmlPagePath.Length) && (at > -1))
            {
                at = strHtmlPagePath.IndexOf('/', start);
                if (at == -1) break;
                myIntArray[notei] = at;
                start = at + 1;
                notei = notei + 1;
                endi = at;
            }

            for (int i = 0; i < notei; i++)
            {
                if (myIntArray[i] > 0)
                {
                    if (myIntArray[i + 1] == 0)
                    {
                        NewFileName = strHtmlPagePath.Substring(myIntArray[i] + 1, strHtmlPagePath.Length - myIntArray[i] - 1);

                    }
                }
            }
            return NewFileName.ToLower();

        }
        #endregion 

        #region 相对地址变绝对地址static
        private  string ConvertToAbsoluteUrls(string html, Uri relativeLocation)
        {
            //在引用ＣＯＭ组件的时候，出现了无法嵌入互操作类型“……”，请改用适用的接口的错误提示。查阅资料，找到解决方案，记录如下：
            //选中项目中引入的dll，鼠标右键，选择属性，把“嵌入互操作类型”设置为False。
            IHTMLDocument2 doc = new HTMLDocumentClass();
            doc.write(new object[] { html });
            doc.close();

            foreach (IHTMLAnchorElement anchor in doc.links)
            {
                IHTMLElement element = (IHTMLElement)anchor;
                string href = (string)element.getAttribute("href", 2);
                if (href != null)
                {
                    Uri addr = new Uri(relativeLocation, href);
                    anchor.href = addr.AbsoluteUri;
                }
            }
           
            foreach (IHTMLImgElement image in doc.images)
            {
                IHTMLElement element = (IHTMLElement)image;
                string src = (string)element.getAttribute("src", 2);
                if (src != null)
                {
                    Uri addr = new Uri(relativeLocation, src);
                    imgListB.Add(image.src.Replace("about:",""));
                    image.src = addr.AbsoluteUri;
                    imgList.Add(addr.AbsoluteUri);
                }
            }

            foreach (IHTMLScriptElement js in doc.scripts)
            {
                IHTMLElement element = (IHTMLElement)js;
                string src = (string)element.getAttribute("src", 2);
                if (src != null)
                {
                    Uri addr = new Uri(relativeLocation, src);
                    jsListB.Add(js.src);
                    js.src = addr.AbsoluteUri;
                    jsList.Add(addr.AbsoluteUri);
                }
            }

            foreach (IHTMLEmbedElement swf in doc.embeds)
            {
                IHTMLElement element = (IHTMLElement)swf;
                string src = (string)element.getAttribute("src", 2);
                if (src != null)
                {
                    Uri addr = new Uri(relativeLocation, src);
                    swfListB.Add(swf.src);
                    swf.src = addr.AbsoluteUri;
                    swfList.Add(addr.AbsoluteUri);
                }
            }

            string ret = doc.body.innerHTML;//不完整

            return ret;
        }
        #endregion

        #region 建立文件夹
        private void CreateFolder()
        {
            try
            {
                if (!Directory.Exists(foldername+"\\images"))
                {
                    Directory.CreateDirectory(foldername + "\\images");
                }
                if (!Directory.Exists(foldername + "\\css"))
                {
                    Directory.CreateDirectory(foldername + "\\css");
                }
                if (!Directory.Exists(foldername + "\\js"))
                {
                    Directory.CreateDirectory(foldername + "\\js");
                }
                if (!Directory.Exists(foldername + "\\swf"))
                {
                    Directory.CreateDirectory(foldername + "\\swf");
                }
                if (!Directory.Exists(foldername + "\\other"))
                {
                    Directory.CreateDirectory(foldername + "\\other");
                }
            }
            catch (Exception e)
            {
                txtLog.Text += e.Message + "\r\n";
            }
            finally { }
        }
        #endregion

        #region 获取网页源码3种方法
        //WebClient 
        private string GetWebClient(string url)
        {
            string strHTML = "";
            WebClient myWebClient = new WebClient();
            Stream myStream = myWebClient.OpenRead(url);
            StreamReader sr = new StreamReader(myStream, System.Text.Encoding.GetEncoding(encodingstr));
            strHTML = sr.ReadToEnd();
            myStream.Close();
            return strHTML;
        }

        //WebRequest 
        private string GetWebRequest(string url)
        {
            Uri uri = new Uri(url);
            WebRequest myReq = WebRequest.Create(uri);
            WebResponse result = myReq.GetResponse();
            Stream receviceStream = result.GetResponseStream();
            StreamReader readerOfStream = new StreamReader(receviceStream, System.Text.Encoding.GetEncoding(encodingstr));
            string strHTML = readerOfStream.ReadToEnd();
            readerOfStream.Close();
            receviceStream.Close();
            result.Close();
            return strHTML;
        }

        //HttpWebRequest 
        private string GetHttpWebRequest(string url)
        {
            Uri uri = new Uri(url);
            HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(uri);
            myReq.UserAgent = "User-Agent:Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705";
            myReq.Accept = "*/*";
            myReq.KeepAlive = true;
            myReq.Headers.Add("Accept-Language", "zh-cn,en-us;q=0.5");
            HttpWebResponse result = (HttpWebResponse)myReq.GetResponse();
            Stream receviceStream = result.GetResponseStream();
            StreamReader readerOfStream = new StreamReader(receviceStream, System.Text.Encoding.GetEncoding(encodingstr));
            string strHTML = readerOfStream.ReadToEnd();
            readerOfStream.Close();
            receviceStream.Close();
            result.Close();
            return strHTML;
        }
        #endregion

        #region 粘贴与选择文件夹、打开文件夹
        private void Paste_Click(object sender, RoutedEventArgs e)
        {
            IDataObject iData = Clipboard.GetDataObject();
            if (iData.GetDataPresent(DataFormats.Text))
            {
                this.WebUrl.Text = (String)iData.GetData(DataFormats.Text);
            }
        }

        private void Folder_Click(object sender, RoutedEventArgs e)
        {
            string filename = "";
            Microsoft.Win32.SaveFileDialog dialogSaveFile = new Microsoft.Win32.SaveFileDialog();
            dialogSaveFile.Filter = "文本文件（*.html）|*.html |文本文件（*.htm）|*.htm";
            if (dialogSaveFile.ShowDialog() == true)
            {
                filename = dialogSaveFile.FileName;
            }
            else
            {
                return;
            }
            this.Filepath.Text = filename;

            //选择所在的文件夹
            //System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            //folderBrowserDialog1.Description = "请选择图片所在的文件夹";
            //folderBrowserDialog1.ShowNewFolderButton = true;
            //folderBrowserDialog1.RootFolder = Environment.SpecialFolder.Personal;
            //System.Windows.Forms.DialogResult result = folderBrowserDialog1.ShowDialog();
            //if (result == System.Windows.Forms.DialogResult.OK)
            //{
            //    string folderName = folderBrowserDialog1.SelectedPath;
            //    if (folderName != "")
            //    {
            //        this.Filepath.Text = folderName;
            //    }
            //}
        }

        private void OpenFolder_Click(object sender, RoutedEventArgs e)
        {
            string filename = this.Filepath.Text.Trim();
            if (filename != "" && Directory.Exists(System.IO.Path.GetDirectoryName(filename)))
            {
                System.Diagnostics.Process.Start("explorer.exe ", System.IO.Path.GetDirectoryName(filename));
            }
            else
            {
                System.Diagnostics.Process.Start("explorer.exe ", System.Environment.CurrentDirectory);
            }
        }
        #endregion

    }
}
