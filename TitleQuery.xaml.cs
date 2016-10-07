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
using System.Data;
using System.IO;
using HtmlAgilityPack;
using System.Net;
using System.Threading;
using System.Text.RegularExpressions;


namespace WpfQuery
{
    /// <summary>
    /// TitleQuery.xaml 的交互逻辑
    /// </summary>
    public partial class TitleQuery : UserControl
    {
        DataTable dt1 = new DataTable();
        int num = 0;  //行数
        List<string> replacedoc = new List<string>();
        int x = 0;//总共查询数
        int y = 0;//已经查询数
        int bdpage = 1;//百度查询页面数
        int j = 0;//收录数

        public TitleQuery()
        {
            InitializeComponent();

            dt1.Columns.Add("url", typeof(string));
            dt1.Columns.Add("title", typeof(string));
            dt1.Columns.Add("include", typeof(string));
            dt1.Columns.Add("rank", typeof(string));
        }


        #region 获取标题
        private void getTitle(object sender, RoutedEventArgs e)
        {
            num = 0;
            getReplaceDoc();
            wtTitle.Text = "";
            dt1.Clear();
            List<string> url = new List<string>();
            if (wtURL.Text.Trim() == String.Empty)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("请输入要查询的URL.", "警告", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            else
            {
                for (int i = 0; i < wtURL.LineCount; i++)
                {
                    ++num;
                    dt1.Rows.Add(wtURL.GetLineText(i).Trim(),"","","");
                }
            }


            //初始化进度条
            y = 0;
            x = dt1.Rows.Count;
            pbQuery.Maximum = x;
            pbQuery.Value = 0;

            //线程版--可以同时修改界面元素
            Thread t = new Thread(Go);
            t.Start();

            /*****单线程
            //显示等待窗体
            //WaitingDlg dlg = new WaitingDlg();
            //dlg.ShowInTaskbar = false;
            //dlg.Topmost = true;
            //dlg.Show();

            //得到标题
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                try
                {
                    string pageUrl = dt1.Rows[i][0].ToString();
                    WebClient wc = new WebClient();
                    byte[] pageSourceBytes = wc.DownloadData(new Uri(pageUrl));

                    //获取编码
                    string encode = "";
                    var r_utf8 = new System.IO.StreamReader(new System.IO.MemoryStream(pageSourceBytes), Encoding.UTF8); //将html放到utf8编码的StreamReader内
                    var r_gbk = new System.IO.StreamReader(new System.IO.MemoryStream(pageSourceBytes), Encoding.Default); //将html放到gbk编码的StreamReader内
                    var t_utf8 = r_utf8.ReadToEnd(); //读出html内容
                    var t_gbk = r_gbk.ReadToEnd(); //读出html内容
                    if (!isLuan(t_utf8)) //判断utf8是否有乱码
                    {
                        encode = "utf8";
                    }
                    else
                    {
                        encode = "gbk";
                    }

                    //根据编码读取
                    string pageSource = "";
                    if (encode == "utf8")
                    {
                        pageSource = Encoding.GetEncoding("utf-8").GetString(pageSourceBytes);
                    }
                    else
                    {
                        pageSource = Encoding.GetEncoding("gb2312").GetString(pageSourceBytes);
                    }
                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(pageSource);

                    //获取标题
                    HtmlNodeCollection keyNodes = doc.DocumentNode.SelectNodes("/html[1]/head[1]/title[1]");
                    List<string> keyWords = new List<string>();
                    foreach (HtmlNode keyNode in keyNodes)
                    {
                        dt1.Rows[i][1] = replace(keyNode.InnerText.Trim()).Trim();
                        wtTitle.Text += dt1.Rows[i][1].ToString() + "\r\n"; ;
                    }

                }
                catch
                {
                    wtTitle.Text += "\r\n"; ;
                }
                finally
                {
                    ++y;
                    //实时更新界面元素
                    Thread trd = new Thread(new ThreadStart(this.ThreadTask));
                    trd.IsBackground = true;
                    trd.Start();
                }
            }
            
            //dlg.Close();
             * *****/
        }

        public void Go()
        {
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                Gather(new IURL { i = i, url = dt1.Rows[i][0].ToString() });
            }
        }

        public class IURL
        {
            public int i { get; set; }
            public string url { get; set; }
        }


        private void Gather(object obj)
        {
            IURL iurl = obj as IURL;
            int i = iurl.i;
            string url = iurl.url;

            try
            {
                string pageUrl = url;
                WebClient wc = new WebClient();
                byte[] pageSourceBytes = wc.DownloadData(new Uri(pageUrl));

                //获取编码
                string encode = "";
                var r_utf8 = new System.IO.StreamReader(new System.IO.MemoryStream(pageSourceBytes), Encoding.UTF8); //将html放到utf8编码的StreamReader内
                var r_gbk = new System.IO.StreamReader(new System.IO.MemoryStream(pageSourceBytes), Encoding.Default); //将html放到gbk编码的StreamReader内
                var t_utf8 = r_utf8.ReadToEnd(); //读出html内容
                var t_gbk = r_gbk.ReadToEnd(); //读出html内容
                if (!isLuan(t_utf8)) //判断utf8是否有乱码
                {
                    encode = "utf8";
                }
                else
                {
                    encode = "gbk";
                }

                //根据编码读取
                string pageSource = "";
                if (encode == "utf8")
                {
                    pageSource = Encoding.GetEncoding("utf-8").GetString(pageSourceBytes);
                }
                else
                {
                    pageSource = Encoding.GetEncoding("gb2312").GetString(pageSourceBytes);
                }
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(pageSource);

                //获取标题
                HtmlNodeCollection keyNodes = doc.DocumentNode.SelectNodes("/html[1]/head[1]/title[1]");
                List<string> keyWords = new List<string>();
                foreach (HtmlNode keyNode in keyNodes)
                {
                    dt1.Rows[i][1] = replace(keyNode.InnerText.Trim()).Trim();
                }

            }
            catch
            {

            }
            finally
            {
                //修改图标控件--在线程中修改界面需要使用委托
                OutDelegate outdelegate = new OutDelegate(OutText);
                this.Dispatcher.BeginInvoke(outdelegate, new object[] { "xx" });

                ++y;
                //实时更新界面元素
                Thread trd = new Thread(new ThreadStart(this.ThreadTask));
                trd.IsBackground = true;
                trd.Start();
            }
        }

        public delegate void OutDelegate(string text);
        public void OutText(string text)
        {
            wtTitle.Text = "";
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                wtTitle.Text += dt1.Rows[i][1] + "\r\n";
            }
        }


        //修改进度条
        private delegate void UpdateProgressBarDelegate(System.Windows.DependencyProperty dp, Object value);
        private void ThreadTask()
        {
            UpdateProgressBarDelegate updatePbDelegate = new UpdateProgressBarDelegate(pbQuery.SetValue);
            Dispatcher.Invoke(updatePbDelegate, System.Windows.Threading.DispatcherPriority.Background, new object[] { System.Windows.Controls.ProgressBar.ValueProperty, Convert.ToDouble(y) });
        }

        //判断是否乱码
        bool isLuan(string txt)
        {
            var bytes = Encoding.UTF8.GetBytes(txt);
            //239 191 189
            for (var i = 0; i < bytes.Length; i++)
            {
                if (i < bytes.Length - 3)
                    if (bytes[i] == 239 && bytes[i + 1] == 191 && bytes[i + 2] == 189)
                    {
                        return true;
                    }
            }
            return false;
        }

        #endregion

        #region 获取收录排名
        private void getRank(object sender, RoutedEventArgs e)
        {
            bdpage = GetBDPages();
            List<string> title = new List<string>();
            if (wtTitle.Text.Trim() == String.Empty)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("请获取对应的标题.", "警告", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                if (dt1.Rows.Count > 0)
                {
                    //更新标题列----手动修改的情况
                    for (int i = 0; i < wtTitle.LineCount; i++)
                    {
                        if (i < num)
                        {
                            dt1.Rows[i][1] = wtTitle.GetLineText(i).Trim();
                        }
                    }
                }
                else
                {
                    Xceed.Wpf.Toolkit.MessageBox.Show("请先输入要查询的URL\r\n 然后点击【获取标题】按钮", "警告", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }

            y = 0;
            pbQuery.Value = 0;

            Thread t = new Thread(GoInclude);
            t.Start();


        }

        public void GoInclude()
        {
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                IsInclude(new IURL { i = i, url = dt1.Rows[i][0].ToString() });
                //只有收录的时候查排名
                if (dt1.Rows[i][2].ToString() == "收录")
                {
                    IsRank(new IURL { i = i, url = dt1.Rows[i][1].ToString() });
                }
            }
        }

        private void IsRank(object obj)
        {
            IURL iurl = obj as IURL;
            int i = iurl.i;
            string key = iurl.url;

            StreamReader sr;
            try
            {
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create("http://www.baidu.com/s?wd=" + key);
                HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
                Stream srtStream = webResponse.GetResponseStream();
                sr = new StreamReader(webResponse.GetResponseStream(), Encoding.GetEncoding("utf-8"));
            }
            catch
            {
                MessageBox.Show("对不起！网络没有连接。");
                return;
            }
            finally
            {

            }
            String totalLines = sr.ReadToEnd();
            bool pageinclude = false;
            for (int n = 1; n <= 10; n++)
            {
                string str = " id=\"" + n.ToString() + "\"";
                string html = Common.BDSegment(totalLines, n); //totalLines.Substring(totalLines.IndexOf(str), 1592);
                string nohtmlstr = Common.StripHT(html);
                if (nohtmlstr.Contains(domain(dt1.Rows[i][0].ToString())))
                {
                    dt1.Rows[i][3] = n.ToString();
                    pageinclude = true;
                    break;
                }
            }

            Thread.Sleep(Common.RandomInt(500, 700));
            //-------------------------------------
            //获得百度搜索结果页的2-10的url
            if (!pageinclude && bdpage != 1)//如果首页没有收录和排名设置不为10时
            {
                string baiduurls = Common.MatchURL(totalLines, "<p id=\"page\"", "</p>");
                MatchCollection mcbaiduurls = Common.MatchURLs(baiduurls, "<a href=\"", "\">");
                List<string> BDurl = new List<string>();
                j = 0;
                foreach (Match ma in mcbaiduurls)
                {
                    if (j < 9)
                    {
                        BDurl.Add("http://www.baidu.com" + ma.Value);
                        j++;
                    }
                }

                int m = 1;
                for (int p = 0; p < BDurl.Count; p++)
                {
                    if (m < bdpage)
                    {
                        if (GetRank(BDurl[p], m, domain(dt1.Rows[i][0].ToString()), i) == 1)
                        {
                            break;
                        }
                        m++;
                    }
                }
            }
            //-------------------------------------


            OutDelegateR outdelegate = new OutDelegateR(OutTextR);
            this.Dispatcher.BeginInvoke(outdelegate, new object[] { "xx" });

        }

        public int GetRank(string bdurl, int m, string url, int i)
        {
            StreamReader sr;
            try
            {
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(bdurl);
                HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
                Stream srtStream = webResponse.GetResponseStream();
                sr = new StreamReader(webResponse.GetResponseStream(), Encoding.GetEncoding("utf-8"));
            }
            catch
            {
                MessageBox.Show("对不起！网络没有连接。");
                return 0;
            }
            finally
            {

            }
            String totalLines = sr.ReadToEnd();
            bool pageinclude = false;
            for (int n = (m * 10 + 1); n <= (m * 10 + 10); n++)
            {
                string str = " id=\"" + n.ToString() + "\"";
                string html = Common.BDSegment(totalLines, n); //totalLines.Substring(totalLines.IndexOf(str), 1592);
                string nohtmlstr = Common.StripHT(html);
                if (nohtmlstr.Contains(url))
                {
                    dt1.Rows[i][3] = n.ToString();
                    pageinclude = true;
                    break;
                }
            }

            Thread.Sleep(Common.RandomInt(500, 700));

            if (pageinclude)
            {
                return 1;
            }
            else
            {
                return 0;
            }

        }

        public delegate void OutDelegateR(string text);
        public void OutTextR(string text)
        {
            wtRank.Text = "";
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                if (dt1.Rows[i][0].ToString().Trim() != "")
                {
                    wtRank.Text += dt1.Rows[i][2] + "    " + dt1.Rows[i][3] + "\r\n";
                }
                else
                {
                    wtRank.Text += "\r\n";
                }
            }
        }


        public string domain(string urlstr)
        {
            string url = DealURL(urlstr);
            Regex reg = new Regex(@"^(\w*://)?\w*\.?(\w*\.(com.cn|net.cn|org.cn|gov.cn|org|net|gov|cn|com|mobi|me|info|name|biz|cc|tv|asia|hk|网络|公司|中国)).*$");
            Match mat = reg.Match(url);
            return mat.Groups[2].Value;
        }

        public string DealURL(string url)
        {
            string newurl = "";
            if (url.Split('.').Length == 2)
            {
                return "w." + url;
            }
            else if (url.Split('.').Length == 3)
            {
                return url;
            }
            else if (url.Split('.').Length >= 4)
            {
                newurl = url.Replace(".com.cn", "").Replace(".net.cn", "").Replace(".org.cn", "").Replace(".gov.cn", "").Replace(".org", "").Replace(".net", "").Replace(".gov", "").Replace(".cn", "").Replace(".com", "").Replace(".mobi", "").Replace(".me", "").Replace(".info", "").Replace(".name", "").Replace(".biz", "").Replace(".cc", "").Replace(".tv", "").Replace(".asia", "").Replace(".hk", "").Replace(".网络", "").Replace(".公司", "").Replace(".中国", "");
                string[] str = newurl.Split('.');
                string substr = "";
                for (int i = 0; i < str.Length - 1; i++)
                {
                    substr += str[i] + ".";
                }
                return url.Replace(substr, "w.");
            }

            return String.Empty;
        }

        public int GetBDPages()
        {
            if (Rand10.IsChecked == true)
            {
                return 1;
            }
            else if (Rand20.IsChecked == true)
            {
                return 2;
            }
            else if (Rand50.IsChecked == true)
            {
                return 5;
            }
            else
            {
                return 0;
            }
        }

        private void IsInclude(object obj)
        {
            IURL iurl = obj as IURL;
            int i = iurl.i;
            string url = iurl.url;

            StreamReader sr;
            try
            {
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create("http://www.baidu.com/s?wd=" + url);
                HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
                Stream srtStream = webResponse.GetResponseStream();
                sr = new StreamReader(webResponse.GetResponseStream(), Encoding.GetEncoding("utf-8"));

                String totalLines = sr.ReadToEnd();
                //匹配标题
                Regex reg = new Regex("没有找到");
                Match mat = reg.Match(totalLines);
                if (!mat.Success)
                {
                    dt1.Rows[i][2] = "收录";
                }
                else
                {
                    dt1.Rows[i][2] = "——";
                }

            }
            catch
            {
                MessageBox.Show("对不起！网络没有连接。");
                return;
            }
            finally
            {
                y++;
                //修改图标控件--在线程中修改界面需要使用委托
                //OutDelegateQ outdelegate = new OutDelegateQ(OutTextQ);
                //this.Dispatcher.BeginInvoke(outdelegate, new object[] { "xx" });

                //实时更新界面元素
                Thread trd = new Thread(new ThreadStart(this.ThreadTask));
                trd.IsBackground = true;
                trd.Start();
            }

            Thread.Sleep(500);
        }

        //public delegate void OutDelegateQ(string text);
        //public void OutTextQ(string text)
        //{
        //    wtRank.Text = "";
        //    for (int i = 0; i < dt1.Rows.Count; i++)
        //    {
        //        wtRank.Text += dt1.Rows[i][2] + "\r\n";
        //    }
        //}


        #endregion

        #region 替换标题
        private void btReplacedoc(object sender, RoutedEventArgs e)
        {
            Replace replace = new Replace();
            //replace.WindowStartupLocation = WindowStartupLocation.Manual;
            //replace.Top = 400;
            //replace.Left = 300;
            replace.Show();
            //new Replace().ShowDialog();
            //得到更新后的替换文本
            getReplaceDoc();
        }

        private void btReplace(object sender, RoutedEventArgs e)
        {
            getReplaceDoc();
            if (dt1.Rows.Count > 0)
            {
                for (int i = 0; i < wtTitle.LineCount; i++)
                {
                    if (i < num)
                    {
                        dt1.Rows[i][1] = replace(wtTitle.GetLineText(i).Trim());
                    }
                }
                wtTitle.Text = "";
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    wtTitle.Text += dt1.Rows[i][1] + "\r\n";
                }
            }
            else
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("请先获取标题.", "警告", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        public string replace(string str)
        {
            foreach (string replacestr in replacedoc)
            {
                str = str.Replace(replacestr, "");
            }
            return str;
        }

        public void getReplaceDoc()
        {
            //读取替换文本
            replacedoc.Clear();
            string keywordfilepath = "config\\replacedoc.txt";
            if (File.Exists(keywordfilepath))
            {
                string[] str = File.ReadAllLines(keywordfilepath, System.Text.Encoding.Default);
                foreach (string item in str)
                {
                    if (item.Trim() != "")
                    {
                        replacedoc.Add(item.Trim());
                    }
                }
            }
        }
        #endregion

        #region 导出
        private void ResultExport(object sender, RoutedEventArgs e)
        {
            if (dt1.Rows.Count == 0)
            {
                MessageBox.Show("没有数据！");
            }
            else
            {
                string filename = "";
                //Microsoft.Win32.OpenFileDialog dialogOpenFile = new Microsoft.Win32.OpenFileDialog();
                //dialogOpenFile.ShowDialog();
                Microsoft.Win32.SaveFileDialog dialogSaveFile = new Microsoft.Win32.SaveFileDialog();
                dialogSaveFile.Filter = "Excel文件（*.xls）|*.xls";
                if (dialogSaveFile.ShowDialog() == true)
                {
                    filename = dialogSaveFile.FileName;
                }
                else
                {
                    return;
                }

                ExcelWriterOC excel = new ExcelWriterOC();
                excel.ExcelWriter(filename);
                excel.BeginWrite();
                excel.WriteString(0, 0, "链接");
                excel.WriteString(0, 1, "标题");
                excel.WriteString(0, 2, "收录");
                excel.WriteString(0, 3, "排名");
                short excelline = 1;

                foreach (DataRow dr in dt1.Rows)
                {
                    if (dr[0].ToString() != "")
                    {
                        excel.WriteString(excelline, 0, dr[0].ToString());
                        excel.WriteString(excelline, 1, dr[1].ToString());
                        excel.WriteString(excelline, 2, dr[2].ToString());
                        excel.WriteString(excelline, 3, dr[3].ToString());
                        excelline++;
                    }
                }
                excel.EndWrite();


            }
        }
        #endregion
    }
}
