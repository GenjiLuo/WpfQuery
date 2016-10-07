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
using System.ComponentModel;

using HtmlAgilityPack;
using System.Net;
using System.IO;

//----------
using Odyssey.Controls;
using Odyssey.Controls.Classes;
//----------
using ExtendedGrid.ExtendedGridControl;
//----------
using PropertyTools.Wpf;
//----------
using WpfGauge;

//OutlookBar控件有问题
namespace WpfQuery
{
    /// <summary>
    /// BaiduIndex.xaml 的交互逻辑
    /// </summary>
    public partial class BaiduIndex : UserControl
    {
        DataTable dt1 = new DataTable();
        List<string> key = new List<string>();
        int y = 0;//已经查询数
        //string indexXPath = "/html/body/div[5]/div/div/div[2]/div[2]/div[2]/div/table/tbody/tr[2]/td[2]/div/span";
        //string moblieXPath = "/html/body/div[5]/div/div/div[2]/div[2]/div[2]/div/table/tbody/tr[2]/td[2]/div/span[2]";

        public BaiduIndex()
        {
            
            InitializeComponent();

            dt1.Columns.Add("key", typeof(string));
            dt1.Columns.Add("index", typeof(string));
            dt1.Columns.Add("moblie", typeof(string));

            edgResult.ItemsSource = dt1.DefaultView;

        }


        private void btQuery(object sender, RoutedEventArgs e)
        {
            key.Clear();
            dt1.Clear();

            if (wtKey.Text.Trim() == "")
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("请输入要查询的关键词.", "警告", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            else
            {
                //得到关键词List
                for (int i = 0; i < wtKey.LineCount; i++)
                {
                    string keystr = wtKey.GetLineText(i).Trim();
                    if (keystr != "")
                    {
                        if (!key.Contains(keystr))
                            key.Add(keystr);
                    }
                }



                y = 0;
                this.IsEnabled = false;
                this.wheel.IsSpinning = true;

                Thread t = new Thread(Go);
                t.Start();

            }
        }

        public void Go()
        {
            foreach (string keystr in key)
            {
                getBaiduIndex(keystr);
                Thread.Sleep(500);
            }
        }

        private void getBaiduIndex(string keystr)
        {
            string pageUrl = "http://index.baidu.com/?tpl=trend&word=" + Common.UrlEncode(keystr).ToUpper();
            WebClient wc = new WebClient();
            byte[] pageSourceBytes = wc.DownloadData(new Uri(pageUrl));
            //string pageSource = Encoding.GetEncoding("gb2312").GetString(pageSourceBytes);


            //获取编码
            string encode = "";
            var r_utf8 = new System.IO.StreamReader(new System.IO.MemoryStream(pageSourceBytes), Encoding.UTF8);
            var r_gbk = new System.IO.StreamReader(new System.IO.MemoryStream(pageSourceBytes), Encoding.Default);
            var t_utf8 = r_utf8.ReadToEnd();
            var t_gbk = r_gbk.ReadToEnd();
            if (!Common.isLuan(t_utf8))
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

            //string index = doc.DocumentNode.SelectSingleNode(indexXPath).InnerText;
            //string moblie = doc.DocumentNode.SelectSingleNode(moblieXPath).InnerText;

            //dt1.Rows.Add(keystr, index, moblie);
            dt1.Rows.Add(keystr, "333","444");
            dt1.Rows.Add(keystr, "444", "111");
            dt1.Rows.Add(keystr, "555", "23");
            dt1.Rows.Add(keystr, "666", "45");
            dt1.Rows.Add(keystr, "777", "21");



            //System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(pageUrl);
            //request.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 5.1; Trident/4.0; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022)";
            //System.Net.WebResponse response = request.GetResponse();
            //System.IO.Stream resStream = response.GetResponseStream();
            //System.IO.StreamReader sr = new System.IO.StreamReader(resStream, Encoding.Default); 
            //string html = (sr.ReadToEnd());
            //resStream.Close();
            //sr.Close();
            //MessageBox.Show(Common.StripHT(html));

            ++y;
            OutDelegate outdelegate = new OutDelegate(OutText);
            this.Dispatcher.BeginInvoke(outdelegate, new object[] { "xx" });
        }

        public delegate void OutDelegate(string text);
        public void OutText(string text)
        {
            dt1.AcceptChanges();

            if (y == key.Count)
            {
                this.IsEnabled = true;
                this.wheel.IsSpinning = false;
                //平均指数
                int baiduindex = 0;
                foreach (DataRow dr in dt1.Rows)
                {
                    baiduindex += Convert.ToInt32(dr[1]);
                }
                gauge.Value = baiduindex / dt1.Rows.Count;
            }

        }


        private void btExport(object sender, RoutedEventArgs e)
        {

        }


        private void BlueSkinClick(object sender, RoutedEventArgs e)
        {
            SkinManager.SkinId = SkinId.OfficeBlue;

        }

        private void SilverSkinClick(object sender, RoutedEventArgs e)
        {
            SkinManager.SkinId = SkinId.OfficeSilver;
        }

        private void BlackSkinClick(object sender, RoutedEventArgs e)
        {
            SkinManager.SkinId = SkinId.OfficeBlack;
        }

        private void btDefaultThemes(object sender, RoutedEventArgs e)
        {
            string str = "Default";
            ExtendedDataGrid.Themes themes = (ExtendedDataGrid.Themes)Enum.Parse(typeof(ExtendedDataGrid.Themes), str);
            edgResult.Theme = themes;
        }

        private void btSystemThemes(object sender, RoutedEventArgs e)
        {
            string str = "System";
            ExtendedDataGrid.Themes themes = (ExtendedDataGrid.Themes)Enum.Parse(typeof(ExtendedDataGrid.Themes), str);
            edgResult.Theme = themes;
        }

        private void btMediaThemes(object sender, RoutedEventArgs e)
        {
            string str = "Media";
            ExtendedDataGrid.Themes themes = (ExtendedDataGrid.Themes)Enum.Parse(typeof(ExtendedDataGrid.Themes), str);
            edgResult.Theme = themes;
        }

        private void btForeground(object sender, RoutedEventArgs e)
        {
            edgResult.Foreground = new SolidColorBrush(cp.SelectedColor.Value);
        }


    }
}
