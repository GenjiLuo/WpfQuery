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
using System.Reflection;

namespace WpfQuery
{
    /// <summary>
    /// CreativeCollection.xaml 的交互逻辑
    /// </summary>
    public partial class CreativeCollection : UserControl
    {
        List<string> strcon = new List<string>();

        DataTable dt1 = new DataTable();
        DataTable dt2 = new DataTable();
        int id = 0;
        //int j = 0;//收录数
        int x = 0;//总共查询数
        int y = 0;//已经查询数
        int bdpage = 1;//百度查询页面数
        string rule = "";
        string mode = "DomainMode";
        List<string> key = new List<string>();

        int refreshnum = 1;//刷新页数
        //左右边排名
        bool leftrank = true;
        bool rightrank = true;

        //新增
        bool baidu = false;
        bool so = false;
        bool sogou = false;
        int s = 0;//搜索引擎数

        //排除域名
        List<string> deldomain = new List<string>();

        //string htmlSim = "";
        public CreativeCollection()
        {
            InitializeComponent();

            dt1.Columns.Add(new DataColumn("id", typeof(int)));
            dt1.Columns.Add(new DataColumn("key", typeof(string)));
            dt1.Columns.Add(new DataColumn("rule", typeof(string)));
            dt1.Columns.Add(new DataColumn("search", typeof(string)));
            dt1.Columns.Add(new DataColumn("rank", typeof(string)));
            dt1.Columns.Add(new DataColumn("title", typeof(string)));
            dt1.Columns.Add(new DataColumn("desc1", typeof(string)));
            dt1.Columns.Add(new DataColumn("desc2", typeof(string)));
            dt1.Columns.Add(new DataColumn("biddomain", typeof(string)));
            dt1.Columns.Add(new DataColumn("hospital", typeof(string)));


            //dgQueryResult.ItemsSource = dt1.DefaultView;


            //子表
            dt2.Columns.Add(new DataColumn("id", typeof(int)));
            dt2.Columns.Add(new DataColumn("key", typeof(string)));
            dt2.Columns.Add(new DataColumn("rule", typeof(string)));
            dt2.Columns.Add(new DataColumn("search", typeof(string)));
            dt2.Columns.Add(new DataColumn("rank", typeof(string)));
            dt2.Columns.Add(new DataColumn("title", typeof(string)));
            dt2.Columns.Add(new DataColumn("desc1", typeof(string)));
            dt2.Columns.Add(new DataColumn("desc2", typeof(string)));
            dt2.Columns.Add(new DataColumn("biddomain", typeof(string)));
            dt2.Columns.Add(new DataColumn("hospital", typeof(string)));

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
            GetRefreshNum();
            SetCollectionLR();
            SetMode();
            getSearch();
            key.Clear();
            getDelDomain();
            rule = "";

            for (int i = 0; i < txtQueryKey.LineCount; i++)
            {
                if (txtQueryKey.GetLineText(i).Trim() != "")
                {
                    if (!key.Contains(txtQueryKey.GetLineText(i).Trim()))
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
                    StreamWriter sw = new StreamWriter("config\\creative.txt", true, Encoding.GetEncoding("gb2312"));
                    sw.WriteLine(str);
                    sw.Close();
                }

                rule = cbCondition.Text.Trim();
            }

            if (!leftrank && !rightrank)
            {
                MessageBox.Show("请选择采集排名设置！");
                return;
            }

            if (s != 0)
            {
                NowKey.Text = "正在查询...";
                tbTotal.Text = "关键词总数：" + key.Count.ToString();
                this.Tab.SelectedIndex = 1;
                pbQuery.Maximum = x = s * key.Count;
                pbQuery.Value = y = 0;
                btAll.IsEnabled = false;
                btReturn.IsEnabled = false;
                //----------------------
                if (baidu)
                {
                    Thread bdthread = new Thread(BaiduDeal);
                    bdthread.Start();
                }

                if (sogou)
                {
                    Thread sogouthread = new Thread(SogouDeal);
                    sogouthread.Start();
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
                getBaiduCreative(key[i]);
            }
        }

        public void SogouDeal()
        {
            for (int i = 0; i < key.Count; i++)
            {
                getSogouRank(key[i]);
            }
        }
        #endregion

        #region 百度创意采集
        private void getBaiduCreative(string key)
        {
            List<string> BDurl = new List<string>();
            for (int i = 0; i < bdpage; i++)
            {
                BDurl.Add("http://www.baidu.com/s?wd=" + key + "&pn=" + (i * 10).ToString() + "&ie=utf-8&usm=4");
            }


            for (int j = 0; j < BDurl.Count; j++)
            {
                restart:
                //OutDelegateSim simdelegate = new OutDelegateSim(OutTextSim);
                //this.Dispatcher.BeginInvoke(simdelegate, new object[] { BDurl[j] });
                //Thread.Sleep(5000);
                //string bd_source = htmlSim;
                //MessageBox.Show(htmlSim);
                string bd_source = GetWebPageSource(BDurl[j]);


                //被屏蔽的时候
                if (bd_source.Contains("很抱歉，您的请求暂时无法响应！"))
                {
                    MessageBox.Show("对不起！在点击确定之前解除百度屏蔽！");
                    Thread.Sleep(30000);
                    goto restart;
                }

                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(bd_source);

                try
                {
                    //采集左边排名
                    if (leftrank)
                    {
                        int bdbig = 1;
                        HtmlNode BDLeftHN = doc.GetElementbyId("content_left");
                        string bdlefthtml = "<!doctype html><html><head><title>baidu</title></head><body>" + BDLeftHN.InnerHtml + "</body></html>";
                        HtmlDocument docright = new HtmlDocument();
                        docright.LoadHtml(bdlefthtml);
                        HtmlNodeCollection LeftNodes = docright.DocumentNode.SelectNodes("/html/body/div");
                        foreach (HtmlNode Node in LeftNodes)
                        {
                            if (!isContains(Node.OuterHtml))
                            {
                                string Nodehtml = "<!doctype html><html><head><title>baidu</title></head><body>" + Node.InnerHtml + "</body></html>";
                                HtmlDocument html_node = new HtmlDocument();
                                html_node.LoadHtml(Nodehtml);
                                HtmlNodeCollection hncNode = html_node.DocumentNode.SelectNodes("/html/body/div");
                                if (hncNode.Count == 3)
                                {
                                    string bdlefthtml_node = "<!doctype html><html><head><title>baidu</title></head><body>" + Node.InnerHtml + "</body></html>";
                                    HtmlDocument docleft_node = new HtmlDocument();
                                    docleft_node.LoadHtml(bdlefthtml_node);
                                    string title = docleft_node.DocumentNode.SelectSingleNode("/html/body/div").InnerText;
                                    string desc1 = docleft_node.DocumentNode.SelectSingleNode("/html/body/div[2]").InnerText.Replace("&nbsp;", " ");
                                    string desc2 = "";
                                    string biddomain = docleft_node.DocumentNode.SelectSingleNode("/html/body/div[3]/span").InnerText;
                                    string hospital = Common.MatchURL(Node.InnerHtml, "data-renzheng=\"{title:'", "：'");
                                    //dt1.Rows.Add(new object[10] { id++, key, rule, "Baidu", "左" + (bdbig++).ToString(), format(title), format(desc1), desc2, biddomain, hospital });
                                    if (rule.Trim() == "")
                                    {
                                        dt1.Rows.Add(new object[10] { id++, key, rule, "Baidu", "左" + (bdbig++).ToString(), format(title), format(desc1), desc2, biddomain, hospital });
                                    }
                                    else
                                    {
                                        if (mode == "NameMode" && MatchRule(hospital))
                                        {
                                            dt1.Rows.Add(new object[10] { id++, key, rule, "Baidu", "左" + (bdbig++).ToString(), format(title), format(desc1), desc2, biddomain, hospital });
                                        }
                                        else if (mode == "DomainMode" && MatchRule(biddomain))
                                        {
                                            dt1.Rows.Add(new object[10] { id++, key, rule, "Baidu", "左" + (bdbig++).ToString(), format(title), format(desc1), desc2, biddomain, hospital });
                                        }
                                    }
                                }
                                else if (hncNode.Count == 4)
                                {
                                    string bdlefthtml_node = "<!doctype html><html><head><title>baidu</title></head><body>" + Node.InnerHtml + "</body></html>";
                                    HtmlDocument docleft_node = new HtmlDocument();
                                    docleft_node.LoadHtml(bdlefthtml_node);
                                    string title = docleft_node.DocumentNode.SelectSingleNode("/html/body/div").InnerText;
                                    string desc1 = docleft_node.DocumentNode.SelectSingleNode("/html/body/div[2]").InnerText.Replace("&nbsp;", " ");
                                    string desc2 = docleft_node.DocumentNode.SelectSingleNode("/html/body/div[3]").InnerText.Replace("&nbsp;", " ");
                                    string biddomain = docleft_node.DocumentNode.SelectSingleNode("/html/body/div[4]/span").InnerText;
                                    string hospital = Common.MatchURL(Node.InnerHtml, "data-renzheng=\"{title:'", "：'");
                                    //dt1.Rows.Add(new object[10] { id++, key, rule, "Baidu", "左" + (bdbig++).ToString(), format(title), format(desc1), format(desc2), biddomain, hospital });
                                    if (rule.Trim() == "")
                                    {
                                        dt1.Rows.Add(new object[10] { id++, key, rule, "Baidu", "左" + (bdbig++).ToString(), format(title), format(desc1), format(desc2), biddomain, hospital });
                                    }
                                    else
                                    {
                                        if (mode == "NameMode" && MatchRule(hospital))
                                        {
                                            dt1.Rows.Add(new object[10] { id++, key, rule, "Baidu", "左" + (bdbig++).ToString(), format(title), format(desc1), format(desc2), biddomain, hospital });
                                        }
                                        else if (mode == "DomainMode" && MatchRule(biddomain))
                                        {
                                            dt1.Rows.Add(new object[10] { id++, key, rule, "Baidu", "左" + (bdbig++).ToString(), format(title), format(desc1), format(desc2), biddomain, hospital });
                                        }
                                    }
                                }
                            }
                        }
                    }
                    //采集右边排名
                    if (rightrank)
                    {
                        int bdbig = 1;
                        HtmlNode BDRightHN = doc.GetElementbyId("ec_im_container");
                        string bdrighthtml = "<!doctype html><html><head><title>baidu</title></head><body>" + BDRightHN.InnerHtml + "</body></html>";
                        HtmlDocument docright = new HtmlDocument();
                        docright.LoadHtml(bdrighthtml);
                        HtmlNodeCollection RightNodes = docright.DocumentNode.SelectNodes("/html/body/div");
                        RightNodes.Remove(0);
                        foreach (HtmlNode Node in RightNodes)
                        {
                            string bdrighthtml_node = "<!doctype html><html><head><title>baidu</title></head><body>" + Node.InnerHtml + "</body></html>";
                            HtmlDocument docright_node = new HtmlDocument();
                            docright_node.LoadHtml(bdrighthtml_node);
                            string title = docright_node.DocumentNode.SelectSingleNode("/html/body/a").InnerText;
                            string desc1 = docright_node.DocumentNode.SelectSingleNode("/html/body/a[2]").InnerText;
                            string desc2 = "";
                            string biddomain = docright_node.DocumentNode.SelectSingleNode("/html/body/a[2]/font[2]").InnerText;
                            string hospital = Common.MatchURL(Node.InnerHtml, "data-renzheng=\"{title:'", "：'");
                            if (rule.Trim() == "")
                            {
                                dt1.Rows.Add(new object[10] { id++, key, rule, "Baidu", "右" + (bdbig++).ToString(), format(title), format(desc1.Replace(biddomain, "")), desc2, biddomain, hospital });
                            }
                            else
                            {
                                if (mode == "NameMode" && MatchRule(hospital))
                                {
                                    dt1.Rows.Add(new object[10] { id++, key, rule, "Baidu", "右" + (bdbig++).ToString(), format(title), format(desc1.Replace(biddomain, "")), desc2, biddomain, hospital });
                                }
                                else if (mode == "DomainMode" && MatchRule(biddomain))
                                {
                                    dt1.Rows.Add(new object[10] { id++, key, rule, "Baidu", "右" + (bdbig++).ToString(), format(title), format(desc1.Replace(biddomain, "")), desc2, biddomain, hospital });
                                }
                            }
                        }

                    }
                }
                catch
                {

                }

            }

            Thread.Sleep(2000);

            y++;
            OutDelegateNew outdelegate = new OutDelegateNew(OutTextNew);
            this.Dispatcher.BeginInvoke(outdelegate, new object[] { key, "Baidu" });
        }

        public bool isContains(string html)
        {
            bool iscontain = false;
            for (int i = 1; i <= 10; i++)
            {
                string str = " id=\"" + i.ToString() + "\"";
                if (html.Contains(str))
                {
                    iscontain = true;
                }
            }
            return iscontain;
        }

        public string format(string str)
        {
            return str.Replace("&lt;", "<").Replace("&gt;", ">");
        }
        #endregion

        #region 模拟
        //public void SuppressScriptErrors(WebBrowser wb, bool Hide)
        //{
        //    FieldInfo fiComWebBrowser = typeof(WebBrowser).GetField("_axIWebBrowser2", BindingFlags.Instance | BindingFlags.NonPublic);
        //    if (fiComWebBrowser == null) return;

        //    object objComWebBrowser = fiComWebBrowser.GetValue(wb);
        //    if (objComWebBrowser == null) return;

        //    objComWebBrowser.GetType().InvokeMember("Silent", BindingFlags.SetProperty, null, objComWebBrowser, new object[] { Hide });
        //}

        //void wbSimulation_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        //{
        //    SuppressScriptErrors(wbSimulation, true);
        //}

        //private void wbSimulation_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        //{
        //    mshtml.IHTMLDocument2 doc2 = (mshtml.IHTMLDocument2)wbSimulation.Document;
        //    htmlSim = doc2.body.innerHTML;
        //    //MessageBox.Show(htmlSim);
        //}

        //public delegate void OutDelegateSim(string sbdurl);
        //public void OutTextSim(string sbdurl)
        //{
        //    wbSimulation.Navigate(sbdurl);
        //}
        #endregion

        #region Sogou排名
        private void getSogouRank(string key)
        {
            List<string> BDurl = new List<string>();
            for (int i = 0; i < bdpage; i++)
            {
                BDurl.Add("http://www.sogou.com/web?query=" + System.Net.WebUtility.UrlEncode(key) + "&ie=utf8&cid=null&page=" + (i + 1).ToString() + "&p=40040100&dp=1&w=01029901&dr=1");
            }

            for (int j = 0; j < BDurl.Count; j++)
            {
            restartso:
                string pageSource = GetWebPageSource(BDurl[j]);
                //MessageBox.Show(pageSource);
                //被屏蔽的时候
                if (pageSource.Contains("document.location.href = "))
                {
                    MessageBox.Show("对不起！Sogou已被屏蔽，先切换IP，后确定！");
                    Thread.Sleep(30000);
                    goto restartso;
                }
                MatchCollection htmlSeg = Common.MatchURLs(pageSource, "<!-- a -->", "<!-- z -->");
                //去掉第一个<!-- a -->"),d=b.indexOf("<!-- z -->
                for (int i = 1; i < htmlSeg.Count; i++)
                {
                    string html = htmlSeg[i].Value;
                    string titlehtml = Common.MatchURL(html, "<h3", "</h3>");
                    string title = Common.StripHT("<h3" + titlehtml + "</h3>");
                    string url = Common.MatchURL(titlehtml, "href=\"", "\"");

                    if (mode == "TitleMode")
                    {
                        if (MatchRule(title) && !MatchDelDomain(url))
                        {
                            dt1.Rows.Add(new object[7] { id++, key, rule, "Sogou", j * 10 + i, title, url });
                        }
                        Thread.Sleep(Common.RandomInt(500, 700));
                    }
                    else if (mode == "DetailMode")
                    {
                        if (MatchRule(html) && !MatchDelDomain(url))
                        {
                            dt1.Rows.Add(new object[7] { id++, key, rule, "Sogou", j * 10 + i, title, url });
                        }
                        Thread.Sleep(Common.RandomInt(500, 700));
                    }
                    else if (mode == "SnapshotMode")
                    {
                        if (url != "")
                        {
                            string pshtml = GetWebPageSource(url);
                            if (MatchRule(pshtml) && !MatchDelDomain(url))
                            {
                                dt1.Rows.Add(id++, key, rule, "Sogou", j * 10 + i, title, url);
                            }
                        }
                    }

                }
            }

            y++;
            OutDelegateNew outdelegate = new OutDelegateNew(OutTextNew);
            this.Dispatcher.BeginInvoke(outdelegate, new object[] { key, "Sogou" });
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

        public HttpResult GetWebPageSource_All(string url)
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
            return result;
        }

        #endregion

        #region 修改界面
        public delegate void OutDelegateNew(string key, string search);
        public void OutTextNew(string key, string search)
        {
            pbQuery.Value = y;
            NowKey.Text = "正在查询搜索引擎：" + search + "   关键词：" + key;

            tvKey.Focus();
            tvKey.SelectedItem = (object)key;
            tvKey.ScrollIntoView(tvKey.SelectedItem);


            if (y == pbQuery.Maximum)
            {
                NowKey.Text = "查询结束！";
                btAll.IsEnabled = true;
                btReturn.IsEnabled = true;
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
            string keywordfilepath = "config\\creative.txt";
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
            FileStream fs = new FileStream("config\\creative.txt", FileMode.Create, FileAccess.Write);
            fs.Close();
            GetCondition();
        }
        #endregion

        #region 查询结果单个结果显示 点击超链接打开该页面(问题：点击dgQueryResult边上是也会打开)
        private void tvKey_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tvKey.SelectedIndex != -1)
            {
                DataRow[] DR = dt1.Select("Key = '" + tvKey.SelectedItem.ToString() + "'");

                dt2.Clear();

                int i = 1;
                foreach (DataRow dr in DR)
                {
                    dt2.Rows.Add(new object[10] { i, dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), dr[6].ToString(), dr[7].ToString(), dr[8].ToString(), dr[9].ToString() });
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

        //Hyperlink.Click="dgQueryResult_Click"
        private void dgQueryResult_Click(object sender, RoutedEventArgs e)
        {
            if (dgQueryResult.SelectedItem == null)
            {
                return;
            }
            string url = (dgQueryResult.SelectedItem as DataRowView)[6].ToString();
            System.Diagnostics.Process.Start(url);
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

        public void GetRefreshNum()
        {
            refreshnum = Convert.ToInt32(iudRefreshNum.Value);
        }

        public void SetCollectionLR()
        {
            if (chkLeftRank.IsChecked == true)
            {
                leftrank = true;
            }
            else
            {
                leftrank = false;
            }

            if (chkRightRank.IsChecked == true)
            {
                rightrank = true;
            }
            else
            {
                rightrank = false;
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
            if (rbNameMode.IsChecked == true)
            {
                mode = "NameMode";
            }
            else
            {
                mode = "DomainMode";
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
                excel.WriteString(0, 6, "描述1");
                excel.WriteString(0, 7, "描述2");
                excel.WriteString(0, 8, "竞价域名");
                excel.WriteString(0, 9, "单位名称");


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
                    excel.WriteString(excelline, 7, dr[7].ToString());
                    excel.WriteString(excelline, 8, dr[8].ToString());
                    excel.WriteString(excelline, 9, dr[9].ToString());
                    excelline++;
                }
                excel.EndWrite();


            }
        }
        #endregion

        #region 设置排除域名
        private void btDelDomain(object sender, RoutedEventArgs e)
        {
            DelDomain deldomain = new DelDomain();
            deldomain.Show();
            //得到更新后的排除域名
            getDelDomain();
        }

        public void getDelDomain()
        {
            deldomain.Clear();
            string filepath = "config\\deldomain.txt";
            if (File.Exists(filepath))
            {
                string[] str = File.ReadAllLines(filepath, System.Text.Encoding.Default);
                foreach (string item in str)
                {
                    if (item.Trim() != "")
                    {
                        deldomain.Add(item.Trim());
                    }
                }
            }
        }

        public bool MatchDelDomain(string url)
        {
            if (deldomain.Count == 0)
            {
                return false;
            }
            else
            {
                foreach (string domain in deldomain)
                {
                    if (url.Contains(domain))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        #endregion

    }
}
