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

using System.Data;
using System.Threading;
using HtmlAgilityPack;
using ScrapySharp.Extensions;
using ScrapySharp.Network;


using DotNet.Utilities;
namespace WpfQuery
{
    /// <summary>
    /// LinksExtracted.xaml 的交互逻辑
    /// </summary>
    public partial class ConditionQuery : UserControl
    {
        List<string> strcon=new List<string>();

        DataTable dt1 = new DataTable();
        int id = 0;
        int j = 0;//收录数
        int x = 0;//总共查询数
        int y = 0;//已经查询数
        int bdpage = 1;//百度查询页面数
        string rule = "";
        string mode = "TitleMode";
        //List<string> theadkey = new List<string>();
        List<string> key = new List<string>();

        //新增
        bool baidu = false;
        bool so = false;
        bool sogou = false;
        int s = 0;//搜索引擎数


        public ConditionQuery()
        {
            InitializeComponent();

            dt1.Columns.Add(new DataColumn("id", typeof(int)));
            dt1.Columns.Add(new DataColumn("key", typeof(string)));
            dt1.Columns.Add(new DataColumn("rule", typeof(string)));
            dt1.Columns.Add(new DataColumn("search", typeof(string)));
            dt1.Columns.Add(new DataColumn("rank", typeof(int)));
            dt1.Columns.Add(new DataColumn("title", typeof(string)));
            dt1.Columns.Add(new DataColumn("url", typeof(string)));

            dgQueryResult.ItemsSource = dt1.DefaultView;

            GetCondition();
        }

        public class CatagoryInfo
        {
            public int id
            {
                get;
                set;
            }

            public string key
            {
                get;
                set;
            }
        }

        #region 按钮查询
        private void Query(object sender, RoutedEventArgs e)
        {
            id = 0;
            dt1.Clear();
            tvKey.Items.Clear();
            bdpage = GetBDPages();
            SetMode();
            getSearch();
            key.Clear();

            for (int i = 0; i < txtQueryKey.LineCount; i++)
            {
                if (txtQueryKey.GetLineText(i).Trim() != "")
                {
                    if(!key.Contains(txtQueryKey.GetLineText(i).Trim()))
                    {
                        key.Add(txtQueryKey.GetLineText(i).Trim());
                        tvKey.Items.Add(txtQueryKey.GetLineText(i).Trim());
                    }
                }
            }

            if (key.Count == 0)
            {
                MessageBox.Show("请输入关键词！");
                return;
            }

            if (cbCondition.Text.Trim() != String.Empty)
            {
                //保存条件
                bool isexist = false;
                foreach (string str in strcon)
                {
                    if (str == cbCondition.Text.Trim())
                    {
                        isexist = true;
                    }
                }
                if (!isexist)
                {
                    string str = cbCondition.Text.Trim();
                    StreamWriter sw = new StreamWriter("config\\condition.txt", true, Encoding.GetEncoding("gb2312"));
                    sw.WriteLine(str);
                    sw.Close();
                }
            }
            else
            {
                MessageBox.Show("请输入条件！");
                return;
            }
            rule = cbCondition.Text.Trim();

            if (s != 0)
            {
                NowKey.Text = "正在查询...";
                tbTotal.Text = "关键词总数：" + key.Count.ToString();
                this.Tab.SelectedIndex = 1;
                pbQuery.Maximum = x = s * key.Count;
                pbQuery.Value = y = 0;
                //this.Tab.IsEnabled = false;
                //this.btExport.IsEnabled = true;
                //dgQueryResult.IsEnabled = false;

                //Thread t = new Thread(Go);
                //t.Start();

                //----------------------
                if (baidu)
                {
                    Thread bdthread = new Thread(BaiduDeal);
                    bdthread.Start();
                }
                if (so)
                {
                    Thread sothread = new Thread(SogouDeal);
                    sothread.Start();
                }
                if (sogou)
                {
                    //getSogouRank(key[i]);
                }

            }
            else
            {
                MessageBox.Show("请选择查询的搜索引擎！");
                return;
            }

        }

        public void BaiduDeal()
        {
            for (int i = 0; i < key.Count; i++)
            {
                 Gather((object)key[i]);
            }
        }

        public void SogouDeal()
        {
            for (int i = 0; i < key.Count; i++)
            {
                getSoRank(key[i]);
            }
        }

        //public void Go()
        //{
        //    for (int i = 0; i < key.Count; i++)
        //    {
        //        if (baidu)
        //        {
        //            Gather((object)key[i]);
        //        }
        //        if (so)
        //        {
        //            getSoRank(key[i]);
        //        }
        //        if (sogou)
        //        {
        //            //getSogouRank(key[i]);
        //        }
        //    }
        //}
        #endregion

        #region 百度排名
        private void Gather(object obj)
        {
            string key = obj.ToString();

            StreamReader sr;
            try
            {
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create("http://www.baidu.com/s?wd=" + key);
                HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
                Stream srtStream = webResponse.GetResponseStream();
                //sr = new StreamReader(webResponse.GetResponseStream(), Encoding.GetEncoding("utf-8"));
                sr = new StreamReader(srtStream, Encoding.GetEncoding("utf-8"));
            }
            catch(Exception ee)
            {
                MessageBox.Show("对不起！网络没有连接。"+"230行--"+ee.ToString());
                return;
            }
            finally
            {

            }
            //String totalLines = sr.ReadToEnd();
            //避免sr为空
            String totalLines = "";
            if (sr != null)
            {
                totalLines = sr.ReadToEnd();
            }

            //被屏蔽的时候
            if (totalLines.Contains("很抱歉，您的请求暂时无法响应！"))
            {
                MessageBox.Show("对不起！在点击确定之前解除百度屏蔽！");
                Thread.Sleep(30000);
            }


            Thread.Sleep(Common.RandomInt(500, 700));


            for (int n = 1; n <= 10; n++)
            {
                string str = " id=\"" + n.ToString() + "\"";
                string html = Common.BDSegment(totalLines, n); //totalLines.Substring(totalLines.IndexOf(str), 1592);//需要改进
                string title = (Common.StripHT(Common.MatchURL(html, "<h3.*>", "</h3>"))).Trim();

                Regex reg = new Regex(@"([a-zA-Z0-9]([a-zA-Z0-9\-]{0,61}[a-zA-Z0-9])?\.)+[a-zA-Z]{2,6}");
                Match mat = reg.Match(Common.StripHT(html));
                string url = mat.Value;

                //if(title != "" && url != "")
                //{
                //    dt1.Rows.Add(new object[6] { id, key, rule, n, title,url });
                //    id++;
                //}

                if (mode == "TitleMode")
                {
                    if (MatchRule(title))
                    {
                        dt1.Rows.Add(new object[7] { id, key, rule,"Baidu", n, title, url });
                        id++;
                    }
                }
                else if (mode == "DetailMode")
                {
                    if (MatchRule(html))
                    {
                        dt1.Rows.Add(new object[7] { id, key, rule, "Baidu", n, title, url });
                        id++;
                    }
                }
                else if (mode == "SnapshotMode")
                {
                    //快照模式包含标题模式和详细模式
                    //bool TitleMode = false;
                    //bool DetailMode = false;
                    //if (MatchRule(title))
                    //{
                    //    dt1.Rows.Add(new object[6] { id, key, rule, n, title, url });
                    //    id++;
                    //    TitleMode = true;
                    //}
                    //if (MatchRule(html) && !TitleMode==true)
                    //{
                    //    dt1.Rows.Add(new object[6] { id, key, rule, n, title, url });
                    //    id++;
                    //    DetailMode = true;
                    //}
                    
                    string snapshot_url = Common.MatchURL(html, "<a data-nolog href=\"", "\"");
                    if (snapshot_url != "")//&& !TitleMode == true && !DetailMode == true
                    {
                        WebClient wc = new WebClient();
                        //byte[] pageSourceBytes = wc.DownloadData(new Uri(snapshot_url));
                        byte[] pageSourceBytes = null;
                        try
                        {
                            pageSourceBytes = wc.DownloadData(new Uri(snapshot_url));
                        }
                        catch( Exception ee)
                        {
                            MessageBox.Show("318行--"+ee.ToString());
                            url = "404";
                        }

                        if (pageSourceBytes != null)
                        {
                            string pageSource = Encoding.GetEncoding("gb2312").GetString(pageSourceBytes);
                            //HtmlDocument doc = new HtmlDocument();
                            //doc.LoadHtml(pageSource);

                            //网址在快照body之外,不能用HtmlAgilityPack
                            string urlstr1 = Common.MatchURL(pageSource,"<div id=\"bd_snap_note\">","</div>");
                            string urlstr2 = Common.MatchURL(urlstr1, "<a href=\"", "\">");
                            if (urlstr2 != "")
                            {
                                url = urlstr2;
                            }

                            if (MatchRule(pageSource))
                            {
                                dt1.Rows.Add(new object[7] { id, key, rule, "Baidu", n, title, url });
                                id++;
                            }
                        }
                        Thread.Sleep(Common.RandomInt(500, 700));
                    }
                }


            }

            Thread.Sleep(Common.RandomInt(500, 700));

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
                    GetTitleUrl(BDurl[p], key, p + 1);
                }

            }



            y++;
            //实时更新界面元素
            //Thread trd = new Thread(new ThreadStart(this.ThreadTask));
            //trd.IsBackground = true;
            //trd.Start();


            //修改图标控件
            //OutDelegate outdelegate = new OutDelegate(OutText);
            //this.Dispatcher.BeginInvoke(outdelegate, new object[] { "xx" });

            OutDelegateNew outdelegate = new OutDelegateNew(OutTextNew);
            this.Dispatcher.BeginInvoke(outdelegate, new object[] { key, "Baidu" });
            Thread.Sleep(Common.RandomInt(500, 700));
        }

        public void GetTitleUrl(string bdurl, string key, int p)
        {
            StreamReader sr;
            try
            {
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(bdurl);
                HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
                Stream srtStream = webResponse.GetResponseStream();
                //sr = new StreamReader(webResponse.GetResponseStream(), Encoding.GetEncoding("utf-8"));
                sr = new StreamReader(srtStream, Encoding.GetEncoding("utf-8"));
            }
            catch( Exception ee)
            {
                MessageBox.Show("405行--"+ee.ToString()+"对不起！网络没有连接。");
                return;
            }
            finally
            {

            }

            Thread.Sleep(500);
            //避免sr为空
            String totalLines = "";
            if (sr != null)
            {
                totalLines = sr.ReadToEnd();
            }
            else
            {
                return;
            }

            for (int n = (p * 10 + 1); n <= (p * 10 + 10); n++)
            {
                string str = " id=\"" + n.ToString() + "\"";
                string html = Common.BDSegment(totalLines, n);//totalLines.Substring(totalLines.IndexOf(str), 1592);
                string title = (Common.StripHT(Common.MatchURL(html, "<h3.*>", "</h3>"))).Trim();

                Regex reg = new Regex(@"([a-zA-Z0-9]([a-zA-Z0-9\-]{0,61}[a-zA-Z0-9])?\.)+[a-zA-Z]{2,6}");
                Match mat = reg.Match(Common.StripHT(html));
                string url = mat.Value;

                //if (title != "" && url != "")
                //{
                //    dt1.Rows.Add(new object[6] { id, key, rule, n, title, url });
                //    id++;
                //}

                if (mode == "TitleMode")
                {
                    if (MatchRule(title))
                    {
                        dt1.Rows.Add(new object[7] { id, key, rule, "Baidu", n, title, url });
                        id++;
                    }
                }
                else if (mode == "DetailMode")
                {
                    if (MatchRule(html))
                    {
                        dt1.Rows.Add(new object[7] { id, key, rule, "Baidu", n, title, url });
                        id++;
                    }
                }
                else if (mode == "SnapshotMode")
                {
                    //快照模式包含标题模式和详细模式
                    //bool TitleMode = false;
                    //bool DetailMode = false;
                    //if (MatchRule(title))
                    //{
                    //    dt1.Rows.Add(new object[6] { id, key, rule, n, title, url });
                    //    id++;
                    //    TitleMode = true;
                    //}
                    //if (MatchRule(html) && !TitleMode == true)
                    //{
                    //    dt1.Rows.Add(new object[6] { id, key, rule, n, title, url });
                    //    id++;
                    //    DetailMode = true;
                    //}

                    string snapshot_url = Common.MatchURL(html, "<a data-nolog href=\"", "\"");
                    if (snapshot_url != "")// && !TitleMode == true && !DetailMode == true
                    {
                        WebClient wc = new WebClient();
                        //byte[] pageSourceBytes = wc.DownloadData(new Uri(snapshot_url));
                        byte[] pageSourceBytes = null;
                        try
                        {
                            pageSourceBytes = wc.DownloadData(new Uri(snapshot_url));
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("486行--"+ex.ToString());
                            url = "404";
                        }

                        if (pageSourceBytes != null)
                        {
                            string pageSource = Encoding.GetEncoding("gb2312").GetString(pageSourceBytes);
                            //HtmlDocument doc = new HtmlDocument();
                            //doc.LoadHtml(pageSource);


                            //网址在快照body之外,不能用HtmlAgilityPack
                            string urlstr1 = Common.MatchURL(pageSource, "<div id=\"bd_snap_note\">", "</div>");
                            string urlstr2 = Common.MatchURL(urlstr1, "<a href=\"", "\">");
                            if (urlstr2 != "")
                            {
                                url = urlstr2;
                            }


                            if (MatchRule(pageSource))
                            {
                                dt1.Rows.Add(new object[7] { id, key, rule, "Baidu", n, title, url });
                                id++;
                            }
                        }
                        Thread.Sleep(Common.RandomInt(500, 700));
                    }
                }

            }

            //修改图标控件
            //OutDelegate outdelegate = new OutDelegate(OutText);
            //this.Dispatcher.BeginInvoke(outdelegate, new object[] { "xx" });

        }
        #endregion

        #region 360排名
        //得到360排名
        private void getSoRank(string key)
        {
            string pageUrl = "http://www.so.com/s?q=" + key;
            WebClient wc = new WebClient();
            byte[] pageSourceBytes = null;
            try
            {
                pageSourceBytes = wc.DownloadData(new Uri(pageUrl));
                //wc.DownloadString(pageUrl);
            }
            catch (Exception ex)
            {
                MessageBox.Show("538行--"+ex.ToString());
                return;
            }
            string pageSource = Encoding.GetEncoding("utf-8").GetString(pageSourceBytes);

            //被屏蔽的时候
            if (pageSource.Contains("很抱歉，您的请求暂时无法响应！"))
            {
                MessageBox.Show("对不起！在点击确定之前解除360屏蔽！");
                Thread.Sleep(30000);
            }

            MatchCollection htmlSeg = Common.MatchURLs(pageSource, "class=\"res-list\">", "</li>");
            

            for (int n = 0; n < htmlSeg.Count; n++)
            {
                string titlehtml = Common.MatchURL(htmlSeg[n].Value, "<h3", "</h3>");
                string title = Common.StripHT("<h3" + titlehtml + "</h3>");
                string url = Common.MatchURL(titlehtml,"<a href=\"","\"");
                string html = htmlSeg[n].Value;

                if (mode == "TitleMode")
                {
                    if (MatchRule(title))
                    {
                        if (n + 1 > 10)
                        {
                            dt1.Rows.Add(new object[7] { id++, key, rule, "So__1", n + 1, title, url });
                        }
                        else
                        {
                            dt1.Rows.Add(new object[7] { id++, key, rule, "So", n + 1, title, url });
                        }
                    }
                }
                else if (mode == "DetailMode")
                {
                    if (MatchRule(html))
                    {
                        if (n + 1 > 10)
                        {
                            dt1.Rows.Add(new object[7] { id++, key, rule, "So__1", n + 1, title, url });
                        }
                        else
                        {
                            dt1.Rows.Add(new object[7] { id++, key, rule, "So", n + 1, title, url });
                        }
                    }
                }
                else if (mode == "SnapshotMode")
                {
                    if (url != "")
                    {
                        //WebClient wcso = new WebClient();
                        //byte[] psbso = null;
                        //try
                        //{
                        //    psbso = wcso.DownloadData(new Uri(url));
                        //}
                        //catch (Exception ex)
                        //{
                        //    MessageBox.Show("602行--"+ex.ToString());
                        //    continue;
                        //}
                        //string pshtml = GetHtmlEncode(psbso);

                        MessageBox.Show(url.ToString());
                        string pshtml = GetWebPageSource(url);
                        MessageBox.Show(pshtml);

                        //string pshtml = GetPageData(url,"");
                        //MessageBox.Show(pshtml);


                        //string pshtml = "";
                        //if (Common.isurl(url))
                        //{
                        //    pshtml = GetWebPageSource(url);
                        //    //MessageBox.Show(pshtml);
                        //}
                        if(MatchRule(pshtml))
                        {
                            if (n + 1 > 10)
                            {
                                dt1.Rows.Add(new object[7] { id++, key, rule, "So__1", n + 1, title, url });
                            }
                            else
                            {
                                dt1.Rows.Add(new object[7] { id++, key, rule, "So", n + 1, title, url });
                            }
                        }
                        Thread.Sleep(Common.RandomInt(400, 700));
                    }
                }
            }

            //获得360搜索结果页的2-5的url和排名
            if (bdpage != 1)
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

            y++;
            OutDelegateNew outdelegate = new OutDelegateNew(OutTextNew);
            this.Dispatcher.BeginInvoke(outdelegate, new object[] { key,"So" });
            Thread.Sleep(500);
        }

        //得到360 2-5页的排名
        public void getSoRank_25(string key, int m, string purl)
        {
            WebClient wc = new WebClient();
            byte[] pageSourceBytes = null;
            try
            {
                pageSourceBytes = wc.DownloadData(new Uri(purl));
            }
            catch (Exception ex)
            {
                MessageBox.Show("673行--"+ex.ToString());
                return;
            }
            Thread.Sleep(Common.RandomInt(500, 700));
            string pageSource = Encoding.GetEncoding("utf-8").GetString(pageSourceBytes);
            MatchCollection htmlSeg = Common.MatchURLs(pageSource, "class=\"res-list\">", "</li>");
            //----------------------------
            for (int n = 0; n < htmlSeg.Count; n++)
            {
                string titlehtml = Common.MatchURL(htmlSeg[n].Value, "<h3", "</h3>");
                string title = Common.StripHT("<h3" + titlehtml + "</h3>");
                string url = Common.MatchURL(titlehtml, "<a href=\"", "\"");
                string html = htmlSeg[n].Value;

                if (mode == "TitleMode")
                {
                    if (MatchRule(title))
                    {
                        dt1.Rows.Add(new object[7] { id++, key, rule, "So", (m - 1) * 10 + n + 1, title, url });
                    }
                }
                else if (mode == "DetailMode")
                {
                    if (MatchRule(html))
                    {
                        dt1.Rows.Add(new object[7] { id++, key, rule, "So", (m - 1) * 10 + n + 1, title, url });
                    }
                }
                else if (mode == "SnapshotMode")
                {
                    if (url != "")
                    {
                        WebClient wcso = new WebClient();
                        byte[] psbso = null; ;
                        try
                        {
                            psbso = wcso.DownloadData(new Uri(url));
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("711行--" + ex.ToString());
                            continue;
                        }

                        string pshtml = GetHtmlEncode(psbso);

                        if (MatchRule(pshtml))
                        {
                            dt1.Rows.Add(id++, key, rule, "So", (m - 1) * 10 + n + 1, title, url);
                        }
                        Thread.Sleep(Common.RandomInt(500, 700));
                    }
                }
            }
            //============================
        }

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

        string GetHtmlEncode(byte[] NewpageSourceBytes)
        {
            //获取编码
            string encode = "";
            var r_utf8 = new System.IO.StreamReader(new System.IO.MemoryStream(NewpageSourceBytes), Encoding.UTF8); //将html放到utf8编码的StreamReader内
            //var r_gbk = new System.IO.StreamReader(new System.IO.MemoryStream(NewpageSourceBytes), Encoding.Default); //将html放到gbk编码的StreamReader内
            var t_utf8 = r_utf8.ReadToEnd(); //读出html内容
            //var t_gbk = r_gbk.ReadToEnd(); //读出html内容
            if (!isLuan(t_utf8)) //判断utf8是否有乱码
            {
                encode = "utf8";
            }
            else
            {
                encode = "gbk";
            }

            //根据编码读取
            if (encode == "utf8")
            {
                return Encoding.GetEncoding("utf-8").GetString(NewpageSourceBytes);
            }
            else
            {
                return Encoding.GetEncoding("gb2312").GetString(NewpageSourceBytes);
            }
        }

        #endregion

        #region 修改界面
        //public delegate void OutDelegate(string text);
        //public void OutText(string text)
        //{
        //    dgQueryResult.Items.Refresh();

        //    //处理图表数据
        //    ChartTitle.Text = "关键词：" + y.ToString() + "  个";
        //    int num = dt1.Rows.Count;
        //    Paiming.YValue = num;
        //    noPaiming.YValue = bdpage * 10 * y - num;
        //}

        //private delegate void UpdateProgressBarDelegate(System.Windows.DependencyProperty dp, Object value);
        //private void ThreadTask()
        //{
        //    UpdateProgressBarDelegate updatePbDelegate = new UpdateProgressBarDelegate(pbQuery.SetValue);
        //    Dispatcher.Invoke(updatePbDelegate, System.Windows.Threading.DispatcherPriority.Background, new object[] { System.Windows.Controls.ProgressBar.ValueProperty, Convert.ToDouble(y) });
        //}

        public delegate void OutDelegateNew(string key,string search);
        public void OutTextNew(string key,string search)
        {
            //dgQueryResult.Items.Refresh();
            pbQuery.Value = y;
            NowKey.Text = "正在查询搜索引擎：" + search + "   关键词：" + key;

            tvKey.Focus();
            tvKey.SelectedItem = (object)key;
            tvKey.ScrollIntoView(tvKey.SelectedItem);

            //显示最后一个
            //if (id > 3)
            //{
            //    dgQueryResult.SelectedIndex = dgQueryResult.Items.Count - 1;
            //    dgQueryResult.ScrollIntoView(dgQueryResult.SelectedItem);
            //}
            if (y == pbQuery.Maximum)
            {
                NowKey.Text = "查询结束！";
                //Tab.IsEnabled = true;
            }
            //处理图表数据
            ChartTitle.Text = "关键词：" + y.ToString() + "  个";
            int num = dt1.Rows.Count;
            Paiming.YValue = num;
            noPaiming.YValue = bdpage * 10 * y - num;
        }
        #endregion

        #region 获取查询条件  清空查询条件
        private void GetCondition()
        {
            //读取条件
            List<CatagoryInfo> condition = new List<CatagoryInfo>();
            //CatagoryInfo con = new CatagoryInfo();
            //con.id = 1;
            //con.key = "aaa";
            //condition.Add(con);
            string keywordfilepath = "config\\condition.txt";//System.Environment.CurrentDirectory + "\\condition.txt";
            int num = 0;
            if (File.Exists(keywordfilepath))
            {
                string[] str = File.ReadAllLines(keywordfilepath, System.Text.Encoding.Default);
                foreach (string item in str)
                {
                    if (item.Trim() != "")
                    {
                        CatagoryInfo con = new CatagoryInfo();
                        con.id = num++;
                        con.key = item.Trim();
                        strcon.Add(item.Trim());
                        condition.Add(con);
                    }
                }
            }


            cbCondition.ItemsSource = condition;
            cbCondition.SelectedValuePath = "id";
            cbCondition.DisplayMemberPath = "key";
        }

        private void btClear(object sender, RoutedEventArgs e)
        {
            //清空
            FileStream fs = new FileStream("config\\condition.txt", FileMode.Create, FileAccess.Write);
            fs.Close();
            GetCondition();
        }
        #endregion

        #region 查询结果单个结果显示
        private void tvKey_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tvKey.SelectedIndex != -1)
            {
                DataRow[] DR = dt1.Select("Key = '" + tvKey.SelectedItem.ToString() + "'");

                DataTable dt2 = new DataTable();
                dt2.Columns.Add(new DataColumn("id", typeof(int)));
                dt2.Columns.Add(new DataColumn("key", typeof(string)));
                dt2.Columns.Add(new DataColumn("rule", typeof(string)));
                dt2.Columns.Add(new DataColumn("search", typeof(string)));
                dt2.Columns.Add(new DataColumn("rank", typeof(int)));
                dt2.Columns.Add(new DataColumn("title", typeof(string)));
                dt2.Columns.Add(new DataColumn("url", typeof(string)));
                int i = 1;
                foreach (DataRow dr in DR)
                {
                    dt2.Rows.Add(new object[7] { i, dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), (int)dr[4], dr[5].ToString(), dr[6].ToString() });
                    i++;
                }

                dgQueryResult.ItemsSource = dt2.DefaultView;

                //处理图表数据
                ChartTitle.Text = "关键词：" + tvKey.SelectedItem.ToString();
                //Random ra = new Random();
                Paiming.YValue = dt2.Rows.Count; //ra.Next(1, 10);
                noPaiming.YValue = bdpage * 10 - dt2.Rows.Count; //ra.Next(60, 90);

            }
        }
        #endregion

        #region 处理URL 获取查询页数 判断搜索引擎 匹配条件 设置匹配模式
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
            else if (Rand100.IsChecked == true)
            {
                return 10;
            }
            else
            {
                return 0;
            }
        }

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

        public bool MatchRule(string context)
        {
            string[] str = rule.Split('|');
            foreach (string s1 in str)
            {
                if (context.Contains(s1.Trim()))
                {
                    return true;
                }
            }
            return false;
        }

        public void SetMode()
        {
            mode = "TitleMode";
            if (rbTwoMode.IsChecked == true)
            {
                mode = "DetailMode";
            }
            else if (rbThreeMode.IsChecked == true)
            {
                mode = "SnapshotMode";
            }
        }
        #endregion

        #region 按钮--返回查询  结果总计 结果导出
        private void ReturnQuery(object sender, RoutedEventArgs e)
        {
            this.Tab.SelectedIndex = 0;
            GetCondition();
        }

        private void AllResult(object sender, RoutedEventArgs e)
        {
            dgQueryResult.ItemsSource = dt1.DefaultView;

            //修改图标控件
            //OutDelegate outdelegate = new OutDelegate(OutText);
            //this.Dispatcher.BeginInvoke(outdelegate, new object[] { "xx" });
        }

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
                excel.WriteString(0, 2, "规则");
                excel.WriteString(0, 3, "搜索引擎");
                excel.WriteString(0, 4, "排名");
                excel.WriteString(0, 5, "标题");
                excel.WriteString(0, 6, "链接");
                short excelline = 1;

                foreach (DataRow dr in dt1.Rows)
                {
                    excel.WriteString(excelline, 0, dr[0].ToString());
                    excel.WriteString(excelline, 1, dr[1].ToString());
                    excel.WriteString(excelline, 2, dr[2].ToString());
                    excel.WriteString(excelline, 3, dr[3].ToString());
                    excel.WriteString(excelline, 4, dr[4].ToString());
                    excel.WriteString(excelline, 5, dr[5].ToString());
                    excel.WriteString(excelline, 6, dr[6].ToString());
                    excelline++;
                }
                excel.EndWrite();


            }
        }
        #endregion

    }
}
