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
using System.Net;
using System.IO;
using System.Threading;

namespace WpfQuery
{
    /// <summary>
    /// Gather.xaml 的交互逻辑-----利用FireFox的Firebug获取XPath   或者  Chrome 获取 XPath
    /// </summary>
    public partial class Gather : UserControl
    {
        bool nohtml = true;
        string folder = "";
        List<string> gatherurl = new List<string>();
        int y = 0;//当前采集数
        HtmlNode titleNodes;
        HtmlNode contextNodes;
        string titleXPath = "";
        string contextXPath = "";

        public Gather()
        {
            InitializeComponent();
        }

        private void btnGather_Click(object sender, RoutedEventArgs e)
        {
            if(cbClearHtml.IsChecked==true)
            {
                nohtml=true;
            }
            else
            {
                nohtml=false;
            }

            if (txtUrl.Text.Trim() == "")
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("请输入采集网址.", "警告", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                txtUrl.Focus();
                return;
            }
            else if (txtTitle.Text.Trim() == "")
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("请输入标题XPath.", "警告", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                txtTitle.Focus();
                return;
            }
            else if (txtContext.Text.Trim() == "")
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("请输入内容XPath.", "警告", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                txtContext.Focus();
                return;
            }
            else if (folder == "")
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("请请选择要保存采集内容的文件夹.", "警告", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                txtContext.Focus();
                return;
            }
            else
            {
                //初始化
                gatherurl.Clear();
                for (int i = 0; i < txtUrl.LineCount; i++)
                {
                    if (txtUrl.GetLineText(i).Trim() != "")
                        gatherurl.Add(txtUrl.GetLineText(i).Trim());
                }
                y = 0;
                pbQuery.Maximum = gatherurl.Count;
                pbQuery.Value = 0;
                this.IsEnabled = false;
                titleXPath = txtTitle.Text.Trim();
                contextXPath = txtContext.Text.Trim();

                Thread t = new Thread(Go);
                t.Start();
            }

        }

        public void Go()
        {
            for (int i = 0; i < gatherurl.Count; i++)
            {
                GatherCon(gatherurl[i]);
            }
        }

        private void GatherCon(object obj)
        {
            string pageUrl = obj.ToString();
            WebClient wc = new WebClient();
            byte[] pageSourceBytes = wc.DownloadData(new Uri(pageUrl));


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



            //string pageSource = Encoding.GetEncoding("gb2312").GetString(pageSourceBytes);
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(pageSource);


            titleNodes = doc.DocumentNode.SelectSingleNode(titleXPath);
            contextNodes = doc.DocumentNode.SelectSingleNode(contextXPath);

            ++y;
            OutDelegateQ outdelegate = new OutDelegateQ(OutTextQ);
            this.Dispatcher.BeginInvoke(outdelegate, new object[] { "xx" });


            Thread.Sleep(Common.RandomInt(500, 700));
        }

        public delegate void OutDelegateQ(string text);
        public void OutTextQ(string text)
        {
            pbQuery.Value = y;

            txtGather.Text = "【标题】：" + titleNodes.InnerText;

            string context = "";
            if (nohtml == true)
            {
                context = contextNodes.InnerText;
            }
            else
            {
                context = contextNodes.InnerHtml;
            }
            txtGather.Text += "\r\n\r\n" + "【内容】：" + context;

            string Path = folder + "\\" + titleNodes.InnerText + ".txt";
            if (!File.Exists(Path))
            {
                System.IO.FileStream f = File.Create(Path);
                f.Close();
                f.Dispose();
            }
            System.IO.StreamWriter f2 = new System.IO.StreamWriter(Path, true, System.Text.Encoding.UTF8);
            f2.WriteLine(context);
            f2.Close();
            f2.Dispose();


            if (y == pbQuery.Maximum)
                this.IsEnabled = true;
        }


        private void btnSaveFolder_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
            fbd.ShowDialog();
            if (fbd.SelectedPath != string.Empty)
                folder = fbd.SelectedPath;
            if (folder != "")
                btnSaveFolder.Content = "已选择保存的文件夹";
        }

        private void btnGenUrl_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
