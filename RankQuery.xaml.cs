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
using System.Data;
using System.Threading;
using System.Text.RegularExpressions;
using DotNet.Utilities;

namespace WpfQuery
{
    /// <summary>
    /// RankQuery.xaml 的交互逻辑
    /// </summary>
    public partial class RankQuery : UserControl
    {
        DataTable dt1 = new DataTable();
        int j = 0;//收录数
        int x = 0;//总共查询数
        int y = 0;//已经查询数
        List<string> key = new List<string>();
        List<string> url = new List<string>();
        int BDpages = 1;

        public RankQuery()
        {
            InitializeComponent();

            dt1.Columns.Add(new DataColumn("num", typeof(int)));
            dt1.Columns.Add(new DataColumn("key", typeof(string)));
            dt1.Columns.Add(new DataColumn("url", typeof(string)));
            dt1.Columns.Add(new DataColumn("include", typeof(bool)));
            dt1.Columns.Add(new DataColumn("rank", typeof(string)));
            dt1.Columns.Add(new DataColumn("time", typeof(string)));
        }

        private void ReturnQuery(object sender, RoutedEventArgs e)
        {
            this.Tab.SelectedIndex = 0;
        }

        private void TwoMode(object sender, RoutedEventArgs e)
        {
            this.TabMode.SelectedIndex = 1;
        }

        private void OneMode(object sender, RoutedEventArgs e)
        {
            this.TabMode.SelectedIndex = 0;
        }

        private void Grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            tbQueryKeyTwoMode.Width = TabMode.Width / 2;
        }

        private void Query(object sender, RoutedEventArgs e)
        {
            key.Clear();
            url.Clear();

            #region 分别得到关键词和网址
            if (rbOneMode.IsChecked == true) //单模式
            {
                if (tbQueryKeyURL.Text.Trim() == String.Empty)
                {
                    MessageBox.Show("请输入查询关键词！");
                    return;
                }
                else
                {
                    for (int i = 0; i < tbQueryKeyURL.LineCount; i++)
                    {
                        if (tbQueryKeyURL.GetLineText(i).Trim() != "")
                        {
                            if (tbQueryKeyURL.GetLineText(i).Trim().ToLower().Contains("http://"))
                            {
                                string[] split = tbQueryKeyURL.GetLineText(i).Trim().ToLower().Replace("http://", "Φ").Split('Φ');

                                if (split[0] == String.Empty)
                                {
                                    MessageBox.Show("没有包含关键词，行数： " + (i + 1).ToString());
                                    return;
                                }
                                else if (!Common.isurl("http://"+split[1]))
                                {
                                    MessageBox.Show("包含非法网址，行数： " + (i + 1).ToString());
                                    return;
                                }
                                else
                                {
                                    key.Add(split[0]);
                                    url.Add(split[1]);
                                }
                            }
                            else
                            {
                                MessageBox.Show("没有包含网址，行数： " + (i + 1).ToString());
                                return;
                            }
                        }
                    }
                }
            }
            else //双模式
            {
                if (tbQueryKeyTwoMode.Text.Trim() == String.Empty || tbQueryURLTwoMode.Text.Trim() == String.Empty)
                {
                    MessageBox.Show("没有包含关键词或网址");
                }
                else
                {
                    //关键词长
                    if (tbQueryKeyTwoMode.LineCount > tbQueryURLTwoMode.LineCount)
                    {
                        for (int i = tbQueryURLTwoMode.LineCount; i < tbQueryKeyTwoMode.LineCount; i++)
                        {
                            if (tbQueryKeyTwoMode.GetLineText(i).Trim() != "" )
                            {
                                MessageBox.Show("关键词与网址不匹配");
                                return;
                            }
                        }

                        //等长
                        for (int i = 0; i < tbQueryURLTwoMode.LineCount; i++)
                        {
                            if (tbQueryKeyTwoMode.GetLineText(i).Trim() != "" && tbQueryURLTwoMode.GetLineText(i).Trim() != "")
                            {
                                //有有
                                if (Common.isurl(tbQueryURLTwoMode.GetLineText(i).Trim()))
                                {
                                    key.Add(tbQueryKeyTwoMode.GetLineText(i).Trim());
                                    url.Add(tbQueryURLTwoMode.GetLineText(i).Trim());
                                }
                                else
                                {
                                    MessageBox.Show("包含非法的网址，行数：" + (i + 1).ToString());
                                    return;
                                }
                            }
                            else if (tbQueryKeyTwoMode.GetLineText(i).Trim() != "" && tbQueryURLTwoMode.GetLineText(i).Trim() == "")
                            {
                                //有空
                                MessageBox.Show("没有对应的网址，行数：" + (i + 1).ToString());
                                return;
                            }
                            else if (tbQueryKeyTwoMode.GetLineText(i).Trim() == "" && tbQueryURLTwoMode.GetLineText(i).Trim() != "")
                            {
                                //空有
                                MessageBox.Show("没有对应的关键词，行数：" + (i + 1).ToString());
                                return;
                            }
                        }

                    }
                    //网址长
                    else if (tbQueryKeyTwoMode.LineCount < tbQueryURLTwoMode.LineCount)
                    {
                        for (int i = tbQueryKeyTwoMode.LineCount; i < tbQueryURLTwoMode.LineCount; i++)
                        {
                            if (tbQueryURLTwoMode.GetLineText(i).Trim() != "")
                            {
                                MessageBox.Show("关键词与网址不匹配");
                                return;
                            }
                        }

                        //等长
                        for (int i = 0; i < tbQueryKeyTwoMode.LineCount; i++)
                        {
                            if (tbQueryKeyTwoMode.GetLineText(i).Trim() != "" && tbQueryURLTwoMode.GetLineText(i).Trim() != "")
                            {
                                //有有
                                if (Common.isurl(tbQueryURLTwoMode.GetLineText(i).Trim()))
                                {
                                    key.Add(tbQueryKeyTwoMode.GetLineText(i).Trim());
                                    url.Add(tbQueryURLTwoMode.GetLineText(i).Trim());
                                }
                                else
                                {
                                    MessageBox.Show("包含非法的网址，行数：" + (i + 1).ToString());
                                    return;
                                }
                            }
                            else if (tbQueryKeyTwoMode.GetLineText(i).Trim() != "" && tbQueryURLTwoMode.GetLineText(i).Trim() == "")
                            {
                                //有空
                                MessageBox.Show("没有对应的网址，行数：" + (i + 1).ToString());
                                return;
                            }
                            else if (tbQueryKeyTwoMode.GetLineText(i).Trim() == "" && tbQueryURLTwoMode.GetLineText(i).Trim() != "")
                            {
                                //空有
                                MessageBox.Show("没有对应的关键词，行数：" + (i + 1).ToString());
                                return;
                            }
                        }


                    }
                    else
                    {
                        //等长
                        for (int i = 0; i < tbQueryKeyTwoMode.LineCount; i++)
                        {
                            if (tbQueryKeyTwoMode.GetLineText(i).Trim() != "" && tbQueryURLTwoMode.GetLineText(i).Trim() != "")
                            {
                                //有有
                                if (Common.isurl(tbQueryURLTwoMode.GetLineText(i).Trim()))
                                {
                                    key.Add(tbQueryKeyTwoMode.GetLineText(i).Trim());
                                    url.Add(tbQueryURLTwoMode.GetLineText(i).Trim());
                                }
                                else
                                {
                                    MessageBox.Show("包含非法的网址，行数：" + (i + 1).ToString());
                                    return;
                                }
                            }
                            else if (tbQueryKeyTwoMode.GetLineText(i).Trim() != "" && tbQueryURLTwoMode.GetLineText(i).Trim() == "")
                            {
                                //有空
                                MessageBox.Show("没有对应的网址，行数：" + (i + 1).ToString());
                                return;
                            }
                            else if (tbQueryKeyTwoMode.GetLineText(i).Trim() == "" && tbQueryURLTwoMode.GetLineText(i).Trim() != "")
                            {
                                //空有
                                MessageBox.Show("没有对应的关键词，行数：" + (i + 1).ToString());
                                return;
                            }
                        }

                    }



                }
            }
            #endregion

            this.Tab.SelectedIndex = 1;

            #region
            //初始化
            y = 0;
            j = 0;
            x = url.Count;
            pbQuery.Maximum = 2 * url.Count;
            pbQuery.Value = 0;
            tbTotal.Text = "总：" + url.Count.ToString() + "   ";

            dt1.Clear();
            for (int n = 0; n < url.Count; n++)
            {
                dt1.Rows.Add(new object[6] { n + 1, key[n], url[n], false,"", "" });
            }
            dgQueryResult.ItemsSource = dt1.DefaultView;
            BDpages = GetBDPages();

            //双线程
            //ThreadPool.SetMaxThreads(2, 2);
            //for (int i = 0; i < url.Count; i++)
            //{
            //    ThreadPool.QueueUserWorkItem(new WaitCallback(Gather), new IdUrl { id = i, url = url[i] });
            //}
            //for (int i = 0; i < key.Count; i++)
            //{
            //    ThreadPool.QueueUserWorkItem(new WaitCallback(GatherbyKey), new IdKeyUrl { id = i, key = key[i], url = url[i],bdpage=BDpages });
            //}

            Thread t = new Thread(Go);
            t.Start();

            #endregion


        }

        public void Go()
        {
            for (int i = 0; i < url.Count; i++)
            {
                Gather(new IdUrl { id = i, url = url[i] });
                if (i % 100 == 0)
                    Thread.Sleep(10000);
            }
            for (int i = 0; i < key.Count; i++)
            {
                GatherbyKey(new IdKeyUrl { id = i, key = key[i], url = url[i], bdpage = BDpages });
                if (i % 100 == 0)
                    Thread.Sleep(10000);
            }
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
            else if (Rand100.IsChecked == true)
            {
                return 10;
            }
            else
            {
                return 0;
            }
        }

        //线程池只能带一个参数，多个参数的方法
        public class IdUrl
        {
            public int id { get; set; }
            public string url { get; set; }
        }
        public class IdKeyUrl
        {
            public int id { get; set; }
            public string key { get; set; }
            public string url { get; set; }
            public int bdpage { get; set; }
        }

        #region 查询收录
        private void Gather(object obj)
        {
            IdUrl idurl = obj as IdUrl;
            int i = idurl.id;
            string url = idurl.url;

            StreamReader sr;
            try
            {
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create("http://www.baidu.com/s?wd=" + url);
                HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
                //Stream srtStream = webResponse.GetResponseStream();
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
            //匹配标题
            Regex reg = new Regex("没有找到该URL。您可以直接访问");
            Match mat = reg.Match(totalLines);
            if (!mat.Success)
            {
                MatchCollection mc_title = Common.MatchURLs(totalLines, "<h3 class=\"t\">", "</h3>");
                MatchCollection mc_url = Common.MatchURLs(totalLines, "<span class=\"g\">", "</span>");
                MatchCollection mc_time = null;
                if (mc_url.Count != 0)
                {
                    mc_time = Common.MatchURLs(mc_url[0].Value, "&nbsp;", "&nbsp;");
                }
                if (mc_title.Count != 0 && mc_time.Count != 0 && Common.compareString(Common.StripHT(mc_url[0].Value), url))
                {
                    j++;
                    //dt1.Rows[i][1] = Common.StripHT(mc_title[0].Value);
                    dt1.Rows[i][3] = true;
                    dt1.Rows[i][5] = mc_time[0].Value;
                    //dt1.AcceptChanges();
                }
            }


            y++;
            //实时更新界面元素
            Thread trd = new Thread(new ThreadStart(this.ThreadTask));
            trd.IsBackground = true;
            trd.Start();

            Thread.Sleep(Common.RandomInt(500, 700));
        }

        //利用委托更新界面元素
        private delegate void UpdateProgressBarDelegate(System.Windows.DependencyProperty dp, Object value);
        private delegate void UpdateIncludeInfo(System.Windows.DependencyProperty txt, Object value);
        private void ThreadTask()
        {
            UpdateProgressBarDelegate updatePbDelegate = new UpdateProgressBarDelegate(pbQuery.SetValue);
            Dispatcher.Invoke(updatePbDelegate, System.Windows.Threading.DispatcherPriority.Background, new object[] { System.Windows.Controls.ProgressBar.ValueProperty, Convert.ToDouble(y) });

            UpdateIncludeInfo updateInclude = new UpdateIncludeInfo(tbInclude.SetValue);
            Dispatcher.Invoke(updateInclude, System.Windows.Threading.DispatcherPriority.Background, new object[] { System.Windows.Controls.TextBlock.TextProperty, "收录：" + j.ToString() + "   " });

            UpdateIncludeInfo updateIncludeRate = new UpdateIncludeInfo(tbIncludeRate.SetValue);
            Dispatcher.Invoke(updateIncludeRate, System.Windows.Threading.DispatcherPriority.Background, new object[] { System.Windows.Controls.TextBlock.TextProperty, "比率：" + (Convert.ToDouble(j) / x).ToString("0.00") + "   " });

            UpdateIncludeInfo updateUnInclude = new UpdateIncludeInfo(tbUnInclude.SetValue);
            Dispatcher.Invoke(updateUnInclude, System.Windows.Threading.DispatcherPriority.Background, new object[] { System.Windows.Controls.TextBlock.TextProperty, "未收录：" + (x - j).ToString() + "   " });

            UpdateIncludeInfo updateUnIncludeRate = new UpdateIncludeInfo(tbUnIncludeRate.SetValue);
            Dispatcher.Invoke(updateUnIncludeRate, System.Windows.Threading.DispatcherPriority.Background, new object[] { System.Windows.Controls.TextBlock.TextProperty, "比率：" + (1 - Convert.ToDouble(j) / x).ToString("0.00") + "   " });
        }
        #endregion

        #region 获取网页源码
        public string GetWebPageSource(string url)
        {
            HttpHelper http = new HttpHelper();
            HttpItem item = new HttpItem()
            {
                URL = url,//URL     必需项
                Method = "GET",//URL     可选项 默认为Get
                Timeout = 30000,//连接超时时间     可选项默认为100000
                ReadWriteTimeout = 30000,//写入Post数据超时时间     可选项默认为30000
                IsToLower = false,//得到的HTML代码是否转成小写     可选项默认转小写
                Cookie = "",//字符串Cookie     可选项
                UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:18.0) Gecko/20100101 Firefox/18.0",//用户的浏览器类型，版本，操作系统     可选项有默认值
                Accept = "text/html, application/xhtml+xml, */*",//    可选项有默认值
                ContentType = "text/html",//返回类型    可选项有默认值
                Referer = "",//来源URL     可选项
                Allowautoredirect = true,//是否根据３０１跳转     可选项
                Connectionlimit = 1024,//最大连接数     可选项 默认为1024
                Postdata = "",//Post数据     可选项GET时不需要写
                ResultType = ResultType.String,//返回数据类型，是Byte还是String
            };
            HttpResult result = http.GetHtml(item);
            return result.Html;
        }
        #endregion

        #region 查询关键词
        private void GatherbyKey(object obj)
        {
            IdKeyUrl idkeyurl = obj as IdKeyUrl;
            int i = idkeyurl.id;
            string key = idkeyurl.key;
            string url = idkeyurl.url;
            int bdpage = idkeyurl.bdpage;

            StreamReader sr;
            try
            {
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create("http://www.baidu.com/s?wd=" + key);
                HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
                //Stream srtStream = webResponse.GetResponseStream();
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
            //string totalLines = GetWebPageSource("http://www.baidu.com/s?wd=" + key);

            bool pageinclude = false;
            for (int n = 1; n <= 10; n++)
            {
                string str = " id=\"" + n.ToString() + "\"";
                string html = Common.BDSegment(totalLines, n);//totalLines.Substring(totalLines.IndexOf(str), 1592);
                string nohtmlstr = Common.StripHT(html);
                //MessageBox.Show(nohtmlstr);
                //MessageBox.Show(Common.domain(url.Replace("http://", "")));
                if (nohtmlstr.Contains(Common.domain(url.Replace("http://", ""))))
                {
                    dt1.Rows[i][4] = n.ToString();
                    //dt1.AcceptChanges();
                    pageinclude = true;
                    break;
                }
            }


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
                        if (GetRank(BDurl[p], m, url, i)==1)
                        {
                            break;
                        }
                        m++;
                    }
                }
            }


            y++;
            //实时更新界面元素
            Thread trd = new Thread(new ThreadStart(this.ThreadTaskUpdatePb));
            trd.IsBackground = true;
            trd.Start();

            Thread.Sleep(Common.RandomInt(500, 800));
        }

        public int GetRank(string bdurl,int m,string url,int i)
        {
            StreamReader sr;
            try
            {
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(bdurl);
                HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
                //Stream srtStream = webResponse.GetResponseStream();
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
            for (int n = (m * 10+1); n <= (m * 10+10); n++)
            {
                string str = " id=\"" + n.ToString() + "\"";
                string html = totalLines.Substring(totalLines.IndexOf(str), 1592);
                string nohtmlstr = Common.StripHT(html);
                if (nohtmlstr.Contains(Common.domain(url.Replace("http://", ""))))
                {
                    dt1.Rows[i][4] = n.ToString();
                    dt1.AcceptChanges();
                    pageinclude = true;
                    break;
                }
            }

            Thread.Sleep(Common.RandomInt(500, 600));

            if (pageinclude)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        private void ThreadTaskUpdatePb()
        {
            UpdateProgressBarDelegate updatePbDelegate = new UpdateProgressBarDelegate(pbQuery.SetValue);
            Dispatcher.Invoke(updatePbDelegate, System.Windows.Threading.DispatcherPriority.Background, new object[] { System.Windows.Controls.ProgressBar.ValueProperty, Convert.ToDouble(y) });
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
                excel.WriteString(0, 0, "ID");
                excel.WriteString(0, 1, "关键词");
                excel.WriteString(0, 2, "链接");
                excel.WriteString(0, 3, "收录");
                excel.WriteString(0, 4, "排名");
                excel.WriteString(0, 5, "时间");
                short excelline = 1;

                foreach (DataRow dr in dt1.Rows)
                {
                    excel.WriteString(excelline, 0, dr[0].ToString());
                    excel.WriteString(excelline, 1, dr[1].ToString());
                    excel.WriteString(excelline, 2, dr[2].ToString());
                    excel.WriteString(excelline, 3, dr[3].ToString());
                    excel.WriteString(excelline, 4, dr[4].ToString());
                    excel.WriteString(excelline, 5, dr[5].ToString());
                    excelline++;
                }
                excel.EndWrite();


            }
        }
        #endregion
    }
}
