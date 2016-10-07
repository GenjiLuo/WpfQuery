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
using System.Threading;
using System.Text.RegularExpressions;
using System.Net;

namespace WpfQuery
{
    /// <summary>
    /// RankStat.xaml 的交互逻辑
    /// </summary>
    public partial class RankStat : UserControl
    {
        bool baidu = false;
        bool so = false;
        bool sogou = false;
        List<string> key = new List<string>(); 
        List<string> domain = new List<string>();
        int s = 0;//搜索引擎数
        int x = 0;//总共查询数
        int y = 0;//已经查询数
        int bdpage = 1;//百度查询页面数
        DataTable dt1 = new DataTable();


        public RankStat()
        {
            InitializeComponent();

            dt1.Columns.Add("key", typeof(string));
            dt1.Columns.Add("domain", typeof(string));
            dt1.Columns.Add("search", typeof(string));
            dt1.Columns.Add("rank", typeof(string));
        }

        private void getRank(object sender, RoutedEventArgs e)
        {
            if (!getSearch())
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("请选择要查询的搜索引擎.", "警告", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            else if (wtKey.Text.Trim() == "")
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("请输入要查询的关键词.", "警告", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                wtKey.Focus();
                return;
            }
            else if (wtDomain.Text.Trim() == "")
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("请输入要查询的域名.", "警告", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                wtDomain.Focus();
                return;
            }
            else
            {
                //初始化List
                key.Clear();
                domain.Clear();
                dt1.Clear();
                //控件内变灰，不能操作
                this.IsEnabled = false;

                //得到关键词List
                for (int i = 0; i < wtKey.LineCount; i++)
                {
                    if (wtKey.GetLineText(i).Trim() != "")
                        key.Add(wtKey.GetLineText(i).Trim());
                }
                //得到域名List
                for (int i = 0; i < wtDomain.LineCount; i++)
                {
                    if (wtDomain.GetLineText(i).Trim() != "")
                    {
                        string domainstr = getdomain(wtDomain.GetLineText(i).Trim());
                        if (domainstr != "" && !domain.Contains(domainstr))
                            domain.Add(domainstr);
                    }
                }

                //初始化界面
                pbQuery.Maximum = x = s*key.Count;//搜索引擎数 * 关键词数
                pbQuery.Value = 0;
                y = 0;
                bdpage = GetBDPages();

                //超线程
                Thread t = new Thread(Go);
                t.SetApartmentState(ApartmentState.STA);
                t.IsBackground = true;
                t.Start();
            }

        }

        public void Go()
        {
            //显示等待窗体
            //WaitingDlg dlg = new WaitingDlg();
            //dlg.ShowInTaskbar = false;
            //dlg.Topmost = true;
            //dlg.Show();

            for (int i = 0; i < key.Count; i++)
            {
                if (baidu)
                {
                    getBaiduRank(key[i]);
                }
                if (so)
                {
                    getSoRank(key[i]);
                }
                if (sogou)
                {
                    getSogouRank(key[i]);
                }
            }

            //dlg.Close();
            
        }

        #region 百度排名
        //得到百度排名
        private void getBaiduRank(string key)
        {
            string pageUrl = "http://www.baidu.com/s?wd=" + key;
            WebClient wc = new WebClient();
            byte[] pageSourceBytes = wc.DownloadData(new Uri(pageUrl));
            string pageSource = Encoding.GetEncoding("utf-8").GetString(pageSourceBytes);
            for (int n = 1; n <= 10; n++)
            {
                string str = " id=\"" + n.ToString() + "\"";
                string html = Common.BDSegment(pageSource, n);
                string nohtmlstr = Common.StripHT(html);
                isContains(key,nohtmlstr,"Baidu",n);
            }

            //获得百度搜索结果页的2-5的url和排名
            if (bdpage != 1)//如果首页没有收录和排名设置不为10时
            {
                string baiduurls = Common.MatchURL(pageSource, "<p id=\"page\"", "</p>");
                MatchCollection mcbaiduurls = Common.MatchURLs(baiduurls, "<a href=\"", "\">");
                List<string> BDurl = new List<string>();

                for (int i = 0; i < mcbaiduurls.Count; i++)
                {
                    if (i < bdpage - 1)
                        BDurl.Add("http://www.baidu.com" + mcbaiduurls[i].Value);
                }

                for (int p = 0; p < BDurl.Count; p++)
                {
                    getBaiduRank_25(key,p+1,BDurl[p]);
                    Thread.Sleep(500);
                }

            }


            y++;
            updateUI();
            Thread.Sleep(300);
        }

        //得到百度2-5页的排名
        public void getBaiduRank_25(string key, int m, string url)
        {
            string pageUrl = url;
            WebClient wc = new WebClient();
            byte[] pageSourceBytes = wc.DownloadData(new Uri(pageUrl));
            string pageSource = Encoding.GetEncoding("utf-8").GetString(pageSourceBytes);
            for (int n = (m * 10 + 1); n <= (m * 10 + 10); n++)
            {
                string str = " id=\"" + n.ToString() + "\"";
                string html = Common.BDSegment(pageSource, n);
                string nohtmlstr = Common.StripHT(html);
                isContains(key, nohtmlstr, "Baidu", n);
            }
        }
        #endregion

        #region 判断是否有排名
        public void isContains(string key,string html,string search,int rank)
        {
            foreach (string domainstr in domain)
            {
                if (html.Contains(domainstr))
                {
                    if (search == "Baidu")
                    {
                        dt1.Rows.Add(key, domainstr, "/WpfQuery;component/Images/baidu.png", rank);
                    }
                    if (search == "So")
                    {
                        dt1.Rows.Add(key, domainstr, "/WpfQuery;component/Images/360.png", rank);
                    }
                    if (search == "Sogou")
                    {
                        dt1.Rows.Add(key, domainstr, "/WpfQuery;component/Images/sogou.png", rank);
                    }
                
                }
            }
        }
        #endregion

        #region 360排名
        //得到360排名
        private void getSoRank(string key)
        {
            string pageUrl = "http://www.so.com/s?q=" + key;
            WebClient wc = new WebClient();
            byte[] pageSourceBytes = wc.DownloadData(new Uri(pageUrl));
            string pageSource = Encoding.GetEncoding("utf-8").GetString(pageSourceBytes);
            MatchCollection htmlSeg = Common.MatchURLs(pageSource, "class=\"res-list\">", "</li>");

            for (int n = 0; n < htmlSeg.Count; n++)
            {
                string nohtmlstr = Common.StripHT(htmlSeg[n].Value);
                SoisContains(key, nohtmlstr, "So", n+1,1);
            }

            //获得360搜索结果页的2-5的url和排名
            if (bdpage != 1)//如果首页没有收录和排名设置不为10时
            {
                string baiduurls = Common.MatchURL(pageSource, "<div id=\"page\"", "</div>");
                MatchCollection mcbaiduurls = Common.MatchURLs(baiduurls, "<a href=\"", "\">");
                List<string> BDurl = new List<string>();

                for (int i = 0; i < mcbaiduurls.Count; i++)
                {
                    if (i < bdpage - 1)
                        BDurl.Add("http://www.so.com/" + mcbaiduurls[i].Value);
                }

                for (int p = 0; p < BDurl.Count; p++)
                {
                    getSoRank_25(key, p + 2, BDurl[p]);
                    Thread.Sleep(500);
                }

            }


            //dt1.Rows.Add(key, domain[1], "/WpfQuery;component/Images/360.png", "4");
            y++;
            updateUI();
            Thread.Sleep(300);
        }

        //得到360 2-5页的排名
        public void getSoRank_25(string key, int m, string url)
        {
            string pageUrl = url;
            WebClient wc = new WebClient();
            byte[] pageSourceBytes = wc.DownloadData(new Uri(pageUrl));
            string pageSource = Encoding.GetEncoding("utf-8").GetString(pageSourceBytes);
            MatchCollection htmlSeg = Common.MatchURLs(pageSource, "class=\"res-list\">", "</li>");

            for (int n = 0; n < htmlSeg.Count; n++)
            {
                string nohtmlstr = Common.StripHT(htmlSeg[n].Value);
                SoisContains(key, nohtmlstr, "So", n + 1, m);
            }
        }

        #endregion

        #region 判断360是否有排名
        public void SoisContains(string key, string html, string search, int rank,int p)
        {
            foreach (string domainstr in domain)
            {
                if (html.Contains(domainstr))
                {
                    if (search == "So")
                    {
                        dt1.Rows.Add(key, domainstr, "/WpfQuery;component/Images/360.png", p.ToString() +"-"+rank.ToString());
                    }
                }
            }
        }
        #endregion

        #region Sogou排名
        //得到Sogou排名
        private void getSogouRank(string key)
        {
            string pageUrl = "http://www.sogou.com/web?query=" + key;
            WebClient wc = new WebClient();
            byte[] pageSourceBytes = wc.DownloadData(new Uri(pageUrl));
            string pageSource = Encoding.GetEncoding("utf-8").GetString(pageSourceBytes);
            MatchCollection htmlSeg = Common.MatchURLs(pageSource, "<!-- a -->", "<!-- z -->");

            for (int n = 0; n < htmlSeg.Count; n++)
            {
                string nohtmlstr = Common.StripHT(htmlSeg[n].Value);
                SogouisContains(key, nohtmlstr, "Sogou", n + 1, 1);
            }

            //获得Sogou搜索结果页的2-5的url和排名
            if (bdpage != 1)//如果首页没有收录和排名设置不为10时
            {
                string baiduurls = Common.MatchURL(pageSource, "<div class=\"p\"", "</div>");
                MatchCollection mcbaiduurls = Common.MatchURLs(baiduurls, "href=\"", "\">");
                List<string> BDurl = new List<string>();

                for (int i = 0; i < mcbaiduurls.Count; i++)
                {
                    if (i < bdpage - 1)
                        BDurl.Add("http://www.sogou.com/web" + mcbaiduurls[i].Value);
                }

                for (int p = 0; p < BDurl.Count; p++)
                {
                    getSogouRank_25(key, p + 2, BDurl[p]);
                    Thread.Sleep(500);
                }

            }


            //dt1.Rows.Add(key, domain[1], "/WpfQuery;component/Images/sogou.png", "4");
            y++;
            updateUI();
            Thread.Sleep(1000);
        }

        //得到Sogou 2-5页的排名
        public void getSogouRank_25(string key, int m, string url)
        {
            string pageUrl = url;
            WebClient wc = new WebClient();
            byte[] pageSourceBytes = wc.DownloadData(new Uri(pageUrl));
            string pageSource = Encoding.GetEncoding("utf-8").GetString(pageSourceBytes);
            MatchCollection htmlSeg = Common.MatchURLs(pageSource, "<!-- a -->", "<!-- z -->");

            for (int n = 0; n < htmlSeg.Count; n++)
            {
                string nohtmlstr = Common.StripHT(htmlSeg[n].Value);
                SogouisContains(key, nohtmlstr, "Sogou", n + 1, m);
            }
        }

        #endregion

        #region 判断Sogou是否有排名
        public void SogouisContains(string key, string html, string search, int rank, int p)
        {
            foreach (string domainstr in domain)
            {
                if (html.Contains(domainstr))
                {
                    if (search == "Sogou")
                    {
                        dt1.Rows.Add(key, domainstr, "/WpfQuery;component/Images/sogou.png", p.ToString() + "-" + rank.ToString());
                    }
                }
            }
        }
        #endregion

        #region 修改界面
        public void updateUI()
        {
            OutDelegate outdelegate = new OutDelegate(OutText);
            this.Dispatcher.BeginInvoke(outdelegate, new object[] { "xx" });
        }

        public delegate void OutDelegate(string text);
        public void OutText(string text)
        {
            pbQuery.Value = y;
            //wtRank.Text = "";
            //for (int i = 0; i < dt1.Rows.Count; i++)
            //{
            //    wtRank.Text += dt1.Rows[i][0].ToString() + "——" + dt1.Rows[i][1] + "——" + dt1.Rows[i][2] + "——" + dt1.Rows[i][3] + "\r\n";
            //}
            lvQueryResult.ItemsSource = dt1.DefaultView;
            dt1.AcceptChanges();//加上后能实时更新
            //ListView显示最后一行
            lvQueryResult.SelectedIndex = lvQueryResult.Items.Count - 1;
            lvQueryResult.ScrollIntoView(lvQueryResult.SelectedItem);
            //结束的时候，是界面可以操作，在线程中需要在委托中修改
            if(y==pbQuery.Maximum)
                this.IsEnabled = true;
        }
        #endregion

        #region 判断搜索引擎
        public bool getSearch()
        {
            s = 0;
            if (chkBaidu.IsChecked == true)
            {
                ++s;
                baidu = true;
            }
            else
            {
                baidu = false;
            }

            if (chk360.IsChecked == true)
            {
                ++s;
                so = true;
            }
            else
            {
                so = false;
            }

            if (chkSogou.IsChecked == true)
            {
                ++s;
                sogou = true;
            }
            else
            {
                sogou = false;
            }

            if (baidu || so || sogou)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region 得到查询页数
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
        #endregion

        #region 得到根域名
        public string getdomain(string urlstr)
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
        #endregion

        #region 导出
        /// <summary>
        /// 导出：注意导出是把搜索引擎图标的地址改为文本
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btExport(object sender, RoutedEventArgs e)
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
                excel.WriteString(0, 0, "关键词");
                excel.WriteString(0, 1, "域名");
                excel.WriteString(0, 2, "搜索引擎");
                excel.WriteString(0, 3, "排名");
                short excelline = 1;

                foreach (DataRow dr in dt1.Rows)
                {
                    excel.WriteString(excelline, 0, dr[0].ToString());
                    excel.WriteString(excelline, 1, dr[1].ToString());
                    if (dr[2].ToString() == "/WpfQuery;component/Images/baidu.png")
                        excel.WriteString(excelline, 2, "Baidu");
                    else if (dr[2].ToString() == "/WpfQuery;component/Images/360.png")
                        excel.WriteString(excelline, 2, "360");
                    else if (dr[2].ToString() == "/WpfQuery;component/Images/sogou.png")
                        excel.WriteString(excelline, 2, "Sogou");

                    excel.WriteString(excelline, 3, dr[3].ToString());
                    excelline++;
                }
                excel.EndWrite();


            }
        }
        #endregion
    }
}
