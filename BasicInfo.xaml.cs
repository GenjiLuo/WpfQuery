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
using HtmlAgilityPack;
using ScrapySharp.Extensions;
using ScrapySharp.Network;
//ScapySharp有了一个真实的浏览器包装类（处理Reference，Cookie等），另外一个就是使用类似于jQuery一样的Css选择器和Linq语法。
using System.Net;
using System.IO;

using System.Data;
using System.ComponentModel;
using System.Threading;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;

namespace WpfQuery
{
    /// <summary>
    /// BasicInfo.xaml 的交互逻辑
    /// </summary>

    public partial class BasicInfo : UserControl
    {
        DataTable dt = new DataTable();
        List<string> domain = new List<string>();
        int y = 0;

        public BasicInfo()
        {
            InitializeComponent();

            dt.Columns.Add("id", typeof(Int32));
            dt.Columns.Add("key", typeof(string));
            dt.Columns.Add("engine", typeof(string));

            lbDigKey.DataContext = dt;
        }

        private void test(object sender, RoutedEventArgs e)
        {
            //var uri = new Uri("http://www.cnblogs.com/shanyou/archive/2012/05/20/2509435.html");
            //var browser1 = new ScrapingBrowser();
            //var html1 = browser1.DownloadString(uri);
            //var htmlDocument = new HtmlDocument();
            //htmlDocument.LoadHtml(html1);
            //var html = htmlDocument.DocumentNode;

            //var title = html.CssSelect("title");
            //foreach (var htmlNode in title)
            //{
            //    MessageBox.Show(htmlNode.InnerHtml);
            //}
            //var divs = html.CssSelect("div.postBody");

            //foreach (var htmlNode in divs)
            //{
            //    MessageBox.Show(htmlNode.InnerHtml);
            //}

            //divs = html.CssSelect("#cnblogs_post_body");
            //foreach (var htmlNode in divs)
            //{
            //    MessageBox.Show(htmlNode.InnerHtml);
            //}
            //------------------------------------------------
            //var divs = html.CssSelect(“div”);  //all div elements
            //var nodes = html.CssSelect(“div.content”); //all div elements with css class ‘content’
            //var nodes = html.CssSelect(“div.widget.monthlist”); //all div elements with the both css class
            //var nodes = html.CssSelect(“#postPaging”); //all HTML elements with the id postPaging
            //var nodes = html.CssSelect(“div#postPaging.testClass”); // all HTML elements with the id postPaging and css class testClass

            //var nodes = html.CssSelect(“div.content > p.para”); //p elements who are direct children of div elements with css class ‘content’
 
            //var nodes = html.CssSelect(“input[type=text].login”); // textbox with css class login
            //ScrapingBrowser browser = new ScrapingBrowser();

            ////set UseDefaultCookiesParser as false if a website returns invalid cookies format
            ////browser.UseDefaultCookiesParser = false;

            //WebPage homePage = browser.NavigateToPage(new Uri("http://www.bing.com/"));

            //PageWebForm form = homePage.FindFormById("sb_form");
            //form["q"] = "scrapysharp";
            //form.Method = HttpVerb.Get;
            //WebPage resultsPage = form.Submit();

            //HtmlNode[] resultsLinks = resultsPage.Html.CssSelect("div.sb_tlst h3 a").ToArray();

            //WebPage blogPage = resultsPage.FindLinks(By.Text("romcyber blog | Just another WordPress site")).Single().Click();
            //------------------------------------------------
            string pageUrl = "http://top.baidu.com/buzz.php?p=top_keyword";
            WebClient wc = new WebClient();
            byte[] pageSourceBytes = wc.DownloadData(new Uri(pageUrl));
            string pageSource = Encoding.GetEncoding("gb2312").GetString(pageSourceBytes);
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(pageSource);


            HtmlNodeCollection keyNodes = doc.DocumentNode.SelectNodes("/html[1]/head[1]/title[1]");
            List<string> keyWords = new List<string>();
            foreach (HtmlNode keyNode in keyNodes)
            {
                keyWords.Add(keyNode.InnerText);
                MessageBox.Show(keyNode.InnerText);
            }

            MessageBox.Show(doc.GetElementbyId("ba_link_help").InnerText);

            //------------------------------------------------

            //HtmlWeb webClient = new HtmlWeb();
            //HtmlDocument doc = webClient.Load("http://www.sohu.com/");

            //HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("/html[1]/head[1]/title[1]");

            //foreach (HtmlNode node in nodes)
            //{
            //    MessageBox.Show(node.InnerText.Trim());
            //}

            //doc = null;
            //nodes = null;
            //webClient = null;

            //------------------------------------------------
            //// 下載 Yahoo 奇摩股市資料 (範例為 2317 鴻海)
            //WebClient client = new WebClient();
            //MemoryStream ms = new MemoryStream(client.DownloadData("http://tw.stock.yahoo.com/q/q?s=2317"));

            //// 使用預設編碼讀入 HTML
            //HtmlDocument doc = new HtmlDocument();
            //doc.Load(ms, Encoding.Default);

            //// 裝載第一層查詢結果
            //HtmlDocument docStockContext = new HtmlDocument();

            //docStockContext.LoadHtml(doc.DocumentNode.SelectSingleNode("/html[1]/body[1]/center[1]/table[2]/tr[1]/td[1]/table[1]").InnerHtml);

            //// 取得個股標頭
            //HtmlNodeCollection nodeHeaders = docStockContext.DocumentNode.SelectNodes("./tr[1]/th");
            //// 取得個股數值
            //string[] values = docStockContext.DocumentNode.SelectSingleNode("./tr[2]").InnerText.Trim().Split('\n');
            //int i = 0;

            //// 輸出資料
            //foreach (HtmlNode nodeHeader in nodeHeaders)
            //{
            //    MessageBox.Show(nodeHeader.InnerText+ values[i].Trim());
            //    i++;
            //}

            //doc = null;
            //docStockContext = null;
            //client = null;
            //ms.Close();
            //---------------------------------
            //System.Net.WebClient client = new WebClient();
            //byte[] page = client.DownloadData("http://www.google.com"); 
            //string content = System.Text.Encoding.UTF8.GetString(page);
        }

        private void btQuery(object sender, RoutedEventArgs e)
        {
            domain.Clear();

            if (txtQueryDomain.Text.Trim()=="")
            {
                MessageBox.Show("请输入域名");
            }
            else
            {
                for (int i = 0; i < txtQueryDomain.LineCount; i++)
                {
                    if (txtQueryDomain.GetLineText(i).Trim() != "")
                    {
                        Regex reg = new Regex(@"^(\w*://)?(\w*\.?\w*\.(com.cn|com|net.cn|net|org.cn|org|gov.cn|gov|cn|mobi|me|info|name|biz|cc|tv|asia|hk|网络|公司|中国)).*$");
                        Match mat = reg.Match(txtQueryDomain.GetLineText(i).Trim());
                        bool exist = false;
                        foreach (string str in domain)
                        {
                            if (str == mat.Groups[2].Value)
                            {
                                exist = true;
                            }
                        }

                        if (!exist && mat.Groups[2].Value.Trim()!="")
                        {
                            domain.Add(mat.Groups[2].Value);
                        }
                    }
                }
            }

            if (domain.Count != 0)
            {
                pbQuery.Maximum = domain.Count;
                pbQuery.Value = 0;
                buttonExport.IsEnabled = false;
                buttonQuery.IsEnabled = false;
                dt.Clear();
                y = 0;

                Thread t = new Thread(Go);
                t.Start();
            }

        }

        public void Go()
        {
            for (int i = 0; i < domain.Count; i++)
            {
                Gather(domain[i]);
            }
        }

        public void Gather(string domain)
        {
            //string pageUrl = "http://top.baidu.com/buzz.php?p=top_keyword";
            //string pageUrl = "http://seo.chinaz.com/?host="+domain;
            string pageUrl = "http://www.aizhan.com/siteall/"+domain+"/";
            WebClient wc = new WebClient();
            byte[] pageSourceBytes = wc.DownloadData(new Uri(pageUrl));
            //string pageSource = Encoding.GetEncoding("gb2312").GetString(pageSourceBytes);
            string pageSource = Encoding.GetEncoding("utf-8").GetString(pageSourceBytes);
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(pageSource);


            //HtmlNodeCollection keyNodes = doc.DocumentNode.SelectNodes("/html[1]/body[1]/div[1]/div[9]/form[1]/div[1]/div[1]/div[2]/table[3]/tbody[1]/tr[3]/td[2]/span[1]/a[1]");
            //List<string> keyWords = new List<string>();
            //foreach (HtmlNode keyNode in keyNodes)
            //{
            //    keyWords.Add(keyNode.InnerText);
            //}

            string bdinclude = doc.GetElementbyId("baidu").InnerText;
            dt.Rows.Add(y, domain, bdinclude);


            y++;
            //修改图标控件
            OutDelegate outdelegate = new OutDelegate(OutText);
            this.Dispatcher.BeginInvoke(outdelegate, new object[] { "xx" });

            Thread.Sleep(500);

        }

        public delegate void OutDelegate(string text);
        public void OutText(string text)
        {
            pbQuery.Value = y;
            lbDigKey.Items.Refresh();
            if (Convert.ToDouble(y) == pbQuery.Maximum)
            {
                buttonExport.IsEnabled = true;
                buttonQuery.IsEnabled = true;
            }
        }


        private void btExport(object sender, RoutedEventArgs e)
        {

        }

        private void GridViewColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is GridViewColumnHeader)
            {
                //Get clicked column
                GridViewColumn clickedColumn = (e.OriginalSource as GridViewColumnHeader).Column;
                if (clickedColumn != null)
                {
                    //Get binding property of clicked column
                    string bindingProperty = (clickedColumn.DisplayMemberBinding as Binding).Path.Path;
                    SortDescriptionCollection sdc = lbDigKey.Items.SortDescriptions;
                    ListSortDirection sortDirection = ListSortDirection.Ascending;
                    if (sdc.Count > 0)
                    {
                        SortDescription sd = sdc[0];
                        sortDirection = (ListSortDirection)((((int)sd.Direction) + 1) % 2);
                        sdc.Clear();
                    }
                    sdc.Add(new SortDescription(bindingProperty, sortDirection));
                }
            }
        }

    }
}
