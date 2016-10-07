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


namespace WpfQuery
{
    /// <summary>
    /// PlatformQuery.xaml 的交互逻辑
    /// </summary>
    public partial class PlatformQuery : UserControl
    {
        DataTable dt1 = new DataTable();
        int id = 0;
        int j = 0;//收录数
        int x = 0;//总共查询数
        int y = 0;//已经查询数
        int bdpage = 1;//百度查询页面数
        List<string> key = new List<string>();


        public PlatformQuery()
        {
            InitializeComponent();

            dt1.Columns.Add(new DataColumn("id", typeof(int)));
            dt1.Columns.Add(new DataColumn("key", typeof(string)));
            dt1.Columns.Add(new DataColumn("url", typeof(string)));
            dt1.Columns.Add(new DataColumn("rank", typeof(int)));
            dt1.Columns.Add(new DataColumn("title", typeof(string)));

        }



        private void btQuery(object sender, RoutedEventArgs e)
        {
            tvKey.Items.Clear();
            dt1.Clear();
            bdpage = GetBDPages();

            if (txtQueryKey.Text.Trim() == String.Empty)
            {
                MessageBox.Show("请输入要查询的关键词");
                return;
            }
            else
            {
                for (int i = 0; i < txtQueryKey.LineCount; i++)
                {
                    if (txtQueryKey.GetLineText(i).Trim() != "")
                    {
                        key.Add(txtQueryKey.GetLineText(i).Trim());
                        tvKey.Items.Add(txtQueryKey.GetLineText(i).Trim());
                    }
                }
                tbTotal.Text = "关键词总数：" + key.Count.ToString();
                this.Tab.SelectedIndex = 1;
                pbQuery.Maximum = x = key.Count;
                pbQuery.Value = 0;


                //ThreadPool.SetMaxThreads(1, 1);
                //for (int i = 0; i < key.Count; i++)
                //{
                //    ThreadPool.QueueUserWorkItem(new WaitCallback(Gather), (object)key[i]);
                //}
                Thread t = new Thread(Go);
                t.Start();


            }
        }

        public void Go()
        {
            for (int i = 0; i < key.Count; i++)
            {
                Gather(key[i]);
            }
        }


        private void Gather(object obj)
        {
            string key = obj.ToString();

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
            for (int n = 1; n <= 10; n++)
            {
                string str = " id=\"" + n.ToString() + "\"";
                string html = totalLines.Substring(totalLines.IndexOf(str), 1592);
                string title = (Common.StripHT(Common.MatchURL(html, "<h3.*>", "</h3>"))).Trim();

                Regex reg = new Regex(@"([a-zA-Z0-9]([a-zA-Z0-9\-]{0,61}[a-zA-Z0-9])?\.)+[a-zA-Z]{2,6}");
                Match mat = reg.Match(Common.StripHT(html));
                string url = mat.Value;

                if (title != "" && url != "")
                {
                    dt1.Rows.Add(new object[5] { id, key, url, n, title });
                    id++;
                }
            }

            Thread.Sleep(Common.RandomInt(500, 700));

            //foreach (DataRow dr in dt1.Rows)
            //{
            //    MessageBox.Show(dr[4].ToString() + "\r\n" + dr[2].ToString());
            //}

            //获得百度搜索结果页的2-10的title和url
            if (bdpage != 1)
            {
                string baiduurls = Common.MatchURL(totalLines, "<p id=\"page\"", "</p>");
                MatchCollection mcbaiduurls = Common.MatchURLs(baiduurls, "<a href=\"", "\">");
                List<string> BDurl = new List<string>();
                j = 1;
                foreach (Match ma in mcbaiduurls)
                {
                    if (j < bdpage)
                    {
                        BDurl.Add("http://www.baidu.com" + ma.Value);
                        j++;
                    }
                }

                for (int p = 0; p < BDurl.Count; p++)
                {
                    GetTitleUrl(BDurl[p],key,p+1);
                }

            }



            y++;
            //实时更新界面元素
            Thread trd = new Thread(new ThreadStart(this.ThreadTask));
            trd.IsBackground = true;
            trd.Start();

            Thread.Sleep(100);
        }


        private delegate void UpdateProgressBarDelegate(System.Windows.DependencyProperty dp, Object value);
        private void ThreadTask()
        {
            UpdateProgressBarDelegate updatePbDelegate = new UpdateProgressBarDelegate(pbQuery.SetValue);
            Dispatcher.Invoke(updatePbDelegate, System.Windows.Threading.DispatcherPriority.Background, new object[] { System.Windows.Controls.ProgressBar.ValueProperty, Convert.ToDouble(y) });
        }

        private void ReturnQuery(object sender, RoutedEventArgs e)
        {
            tvKey.UnselectAll();
            this.Tab.SelectedIndex = 0;
        }

        public void GetTitleUrl(string bdurl,string key,int p)
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
                return;
            }
            finally
            {

            }
            String totalLines = sr.ReadToEnd();
            for (int n = (p * 10 + 1); n <= (p * 10 + 10); n++)
            {
                string str = " id=\"" + n.ToString() + "\"";
                string html = totalLines.Substring(totalLines.IndexOf(str), 1592);
                string title = (Common.StripHT(Common.MatchURL(html, "<h3.*>", "</h3>"))).Trim();

                Regex reg = new Regex(@"([a-zA-Z0-9]([a-zA-Z0-9\-]{0,61}[a-zA-Z0-9])?\.)+[a-zA-Z]{2,6}");
                Match mat = reg.Match(Common.StripHT(html));
                string url = mat.Value;

                if (title != "" && url != "")
                {
                    dt1.Rows.Add(new object[5] { id, key, url, n, title });
                    id++;
                }
            }

            Thread.Sleep(Common.RandomInt(500, 700));
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

        private void tvKey_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tvKey.SelectedIndex!=-1)
            {
                DataRow[] DR = dt1.Select("Key = '" + tvKey.SelectedItem.ToString() + "'");

                DataTable dt2 = new DataTable();
                dt2.Columns.Add(new DataColumn("id", typeof(int)));
                dt2.Columns.Add(new DataColumn("url", typeof(string)));
                dt2.Columns.Add(new DataColumn("rank", typeof(int)));
                dt2.Columns.Add(new DataColumn("title", typeof(string)));
                int i = 1;
                foreach (DataRow dr in DR)
                {
                    dt2.Rows.Add(new object[4] { i, dr[2].ToString(), (int)dr[3], dr[4].ToString() });
                    i++;
                }

                dgQueryResult.ItemsSource = dt2.DefaultView;

                //处理图表数据
                DataTable dt3 = dt2.Copy(); 
                ChartTitle.Text = "关键词：" + tvKey.SelectedItem.ToString();
                List<string> domain = new List<string>();//根域名
                foreach (DataRow dr in dt3.Rows)
                {
                    string url = DealURL(dr["url"].ToString());
                    Regex reg = new Regex(@"^(\w*://)?\w*\.?(\w*\.(com.cn|net.cn|org.cn|gov.cn|org|net|gov|cn|com|mobi|me|info|name|biz|cc|tv|asia|hk|网络|公司|中国)).*$");
                    Match mat = reg.Match(url);
                    dr["url"] = mat.Groups[2].Value;
                }

                DataTable dtdomain = new DataTable();
                dtdomain.Columns.Add(new DataColumn("url", typeof(string)));
                dtdomain.Columns.Add(new DataColumn("num", typeof(int)));

                //Linq
                var query = from t in dt3.AsEnumerable()
                            group t by new { t1 = t.Field<string>("url") } into m
                            select new
                            {
                                url = m.Key.t1,
                                num = m.Count()
                            };

                foreach (var item in query.ToList())
                {
                    DataRow newdr = dtdomain.NewRow();
                    newdr["url"] = item.url;
                    newdr["num"] = item.num;
                    dtdomain.Rows.Add(newdr);  
                }

                dataSeries.DataSource = dtdomain.DefaultView;

            }
        }

        public string DealURL(string url)
        {
            string newurl="";
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
                if (substr != "")
                {
                    return url.Replace(substr, "w.");
                }
            }

            return String.Empty;
        }
    }
}
