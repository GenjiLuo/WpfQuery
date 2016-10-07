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
using System.Text.RegularExpressions;
using System.IO;
using System.Net;
using System.Data;
using System.Threading;

using org.in2bits.MyXls;

namespace WpfQuery
{
    /// <summary>
    /// IncludeQuery.xaml 的交互逻辑
    /// </summary>
    public partial class IncludeQuery : UserControl
    {
        DataTable dt1 = new DataTable();
        int j = 0;//收录数
        int x = 0;//总共查询数
        int y = 0;//已经查询数


        public IncludeQuery()
        {
            InitializeComponent();

            dt1.Columns.Add(new DataColumn("num", typeof(int)));
            dt1.Columns.Add(new DataColumn("title", typeof(string)));
            dt1.Columns.Add(new DataColumn("url", typeof(string)));
            dt1.Columns.Add(new DataColumn("include", typeof(bool)));
            dt1.Columns.Add(new DataColumn("time", typeof(string)));

        }



        private void btQuery(object sender, RoutedEventArgs e)
        {
            List<string> url=new List<string>();
            List<int> num = new List<int>();
            if (chkBaidu.IsChecked == false && chk360.IsChecked == false)
            {
                MessageBox.Show("请选择要查询的搜索引擎");
            }
            else if (txtQueryURL.Text.Trim() == String.Empty)
            {
                MessageBox.Show("请输入要查询的网址");
            }
            else if (txtQueryURL.Text.Trim() != String.Empty)
            {
                for (int i = 0; i < txtQueryURL.LineCount; i++)
                {
                    if (txtQueryURL.GetLineText(i).Trim() != "")
                    {
                        if (Common.isurl(txtQueryURL.GetLineText(i).Trim()))
                        {
                            url.Add(txtQueryURL.GetLineText(i).Trim());
                        }
                        else
                        {
                            num.Add(i);
                        }
                    }
                }
                if (num.Count!=0)
                {
                    string numstr = "";
                    for (int i = 0; i < num.Count; i++)
                    {
                        numstr += "   " + (num[i]+1).ToString();
                    }
                    MessageBox.Show("非法网址数：" + num.Count.ToString() + "，分别在行数："+numstr);
                }
                else
                {
                    this.Tab.SelectedIndex = 1;

                    //查询收录
                    #region
                    //初始化
                    j = 0;
                    pbQuery.Maximum = x = url.Count;
                    pbQuery.Value = 0;
                    tbTotal.Text = "总：" + url.Count.ToString() + "   ";
                    y = 0;
                    dt1.Clear();
                    for (int n = 0; n < url.Count; n++)
                    {
                        dt1.Rows.Add(new object[5] { n+1, "",url[n],false,""});
                    }
                    dgQueryResult.ItemsSource = dt1.DefaultView;
                    //双线程
                    //ThreadPool.SetMaxThreads(2, 2);
                    //for (int i = 0; i < url.Count; i++)
                    //{
                    //    ThreadPool.QueueUserWorkItem(new WaitCallback(Gather), new Person { Id = i, Url = url[i] });
                    //}

                    Thread t = new Thread(Go);
                    t.Start();
                    #endregion
                }
            }
        }

        public void Go()
        {
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                Gather(new Person { Id = i, Url = dt1.Rows[i][2].ToString() });
            }
        }




        //线程池只能带一个参数，多个参数的方法
        public class Person
        {
            public int Id { get; set; }
            public string Url { get; set; }
        }

        private void Gather(object url)
        {
            Person person = url as Person;
            int i = person.Id;
            string inputString = person.Url;

            StreamReader sr;
            try
            {
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create("http://www.baidu.com/s?wd=" + inputString);
                //HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create("http://www.baidu.com/#wd=" + inputString);
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
            //匹配标题
            //Regex reg = new Regex("抱歉，没有找到与");//没有找到该URL。您可以直接访问//抱歉，没有找到与
            //Match mat = reg.Match(totalLines);
            //if (!mat.Success)
            if (!totalLines.Contains("抱歉，没有找到与") && !totalLines.Contains("没有找到该URL。您可以直接访问"))
            {
                //MessageBox.Show("xxx");
                MatchCollection mc_title = Common.MatchURLs(totalLines, "<h3 class=\"t\">", "</h3>");
                MatchCollection mc_url = Common.MatchURLs(totalLines, "<span class=\"g\">", "</span>");
                MatchCollection mc_time = null;
                if (mc_url.Count != 0)
                {
                    mc_time = Common.MatchURLs(mc_url[0].Value, "&nbsp;", "&nbsp;");
                }
                if (mc_title.Count != 0 && mc_time.Count != 0 && compareString(Common.StripHT(mc_url[0].Value), inputString))
                {
                    j++;
                    dt1.Rows[i][1] = Common.StripHT(mc_title[0].Value);
                    dt1.Rows[i][3] = true;
                    dt1.Rows[i][4] = mc_time[0].Value;
                    dt1.AcceptChanges();
                }
            }


            y++;
            //实时更新界面元素
            Thread trd = new Thread(new ThreadStart(this.ThreadTask));
            trd.IsBackground = true;
            trd.Start();

            //频繁访问百度拒绝搜索
            if (y < 1000)
            {
                Thread.Sleep(500);
            }
            else
            {
                Thread.Sleep(Common.RandomInt(500,1000));
            }
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

        ////通用单个匹配
        //public static string MatchURL(string ma, string start, string end)
        //{
        //    Regex regex = new Regex("(?<=(" + start + "))[.\\s\\S]*?(?=(" + end + "))", RegexOptions.Multiline | RegexOptions.Singleline);
        //    Match mc = regex.Match(ma);
        //    return mc.ToString();
        //}

        ////通用多个匹配
        //public static MatchCollection MatchURLs(string ma, string start, string end)
        //{
        //    Regex regex = new Regex("(?<=(" + start + "))[.\\s\\S]*?(?=(" + end + "))", RegexOptions.Multiline | RegexOptions.Singleline);
        //    MatchCollection mc = regex.Matches(ma);
        //    return mc;
        //}

        ////从html中提取纯文本
        //public string StripHT(string strHtml)
        //{
        //    strHtml = strHtml.Replace("\r\n", " ").Replace("\r", " ").Replace("\n", " ");
        //    Regex regex = new Regex("<.+?>", RegexOptions.IgnoreCase);
        //    string strOutput = regex.Replace(strHtml, "");//替换掉"<"和">"之间的内容
        //    strOutput = strOutput.Replace("<", "");
        //    strOutput = strOutput.Replace(">", "");
        //    strOutput = strOutput.Replace("&nbsp;", "");
        //    return strOutput;
        //}

        //private bool isurl(string url)
        //{
        //    Regex r = new Regex(@"http://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?");
        //    if (r.Match(url).Success)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        private bool compareString(string murl, string url)
        {
            url=url.Replace("http://","");
            int n = murl.Length < url.Length ? murl.Length : url.Length;
            int m = 0;
            for (int i = 0; i < n; i++)
            {
                if (murl[i] == url[i]) 
                    m++;
                else
                    break;
            }
            if (m>=4) //如：g.cn
                return true;
            else 
                return false;
        }


        private void ReturnQuery(object sender, RoutedEventArgs e)
        {
            this.Tab.SelectedIndex = 0;
        }

        //private void ResultExport(object sender, RoutedEventArgs e)
        //{
        //    if (dt1.Rows.Count == 0)
        //    {
        //        MessageBox.Show("没有数据！");
        //    }
        //    else
        //    {
        //        string filename = "";
        //        //Microsoft.Win32.OpenFileDialog dialogOpenFile = new Microsoft.Win32.OpenFileDialog();
        //        //dialogOpenFile.ShowDialog();
        //        Microsoft.Win32.SaveFileDialog dialogSaveFile = new Microsoft.Win32.SaveFileDialog();
        //        dialogSaveFile.Filter = "Excel文件（*.xls）|*.xls";
        //        if (dialogSaveFile.ShowDialog() == true)
        //        {
        //            filename = dialogSaveFile.FileName;
        //        }
        //        else
        //        {
        //            return;
        //        }

        //        ExcelWriterOC excel = new ExcelWriterOC();
        //        excel.ExcelWriter(filename);
        //        excel.BeginWrite();
        //        excel.WriteString(0, 0, "ID");
        //        excel.WriteString(0, 1, "标题");
        //        excel.WriteString(0, 2, "链接");
        //        excel.WriteString(0, 3, "收录");
        //        excel.WriteString(0, 4, "时间");
        //        short excelline = 1;

        //        foreach (DataRow dr in dt1.Rows)
        //        {
        //            excel.WriteString(excelline, 0, dr[0].ToString());
        //            excel.WriteString(excelline, 1, dr[1].ToString());
        //            excel.WriteString(excelline, 2, dr[2].ToString());
        //            excel.WriteString(excelline, 3, dr[3].ToString());
        //            excel.WriteString(excelline, 4, dr[4].ToString());
        //            excelline++;
        //        }
        //        excel.EndWrite();


        //    }
        //}

        private void ResultExport(object sender, RoutedEventArgs e)
        {
            if (dt1.Rows.Count == 0)
            {
                MessageBox.Show("没有数据！");
            }
            else
            {
                string filename = "";
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

                string extension = System.IO.Path.GetExtension(filename);
                DataTable exportdt = dt1;



                if (extension == ".xls")
                {
                    XlsDocument doc = new XlsDocument();
                    doc.FileName = filename;
                    Worksheet sheet = doc.Workbook.Worksheets.Add("百度收录");
                    //单元格格式
                    XF xf = doc.NewXF();
                    xf.Format = StandardFormats.Date_Time;

                    //列格式
                    ColumnInfo colid = new ColumnInfo(doc, sheet);
                    colid.ColumnIndexStart = 0;
                    colid.ColumnIndexEnd = 0;
                    colid.Width = 8 * 256;
                    sheet.AddColumnInfo(colid);
                    ColumnInfo coltitle = new ColumnInfo(doc, sheet);
                    coltitle.ColumnIndexStart = 1;
                    coltitle.ColumnIndexEnd = 1;
                    coltitle.Width = 70 * 256;
                    sheet.AddColumnInfo(coltitle);
                    ColumnInfo colurl = new ColumnInfo(doc, sheet);
                    colurl.ColumnIndexStart = 2;
                    colurl.ColumnIndexEnd = 2;
                    colurl.Width = 70 * 256;
                    sheet.AddColumnInfo(colurl);
                    ColumnInfo colinclude = new ColumnInfo(doc, sheet);
                    colinclude.ColumnIndexStart = 3;
                    colinclude.ColumnIndexEnd = 3;
                    colinclude.Width = 8 * 256;
                    sheet.AddColumnInfo(colinclude);
                    ColumnInfo coltime = new ColumnInfo(doc, sheet);
                    coltime.ColumnIndexStart = 4;
                    coltime.ColumnIndexEnd = 4;
                    coltime.Width = 18 * 256;
                    sheet.AddColumnInfo(coltime);

                    Worksheet ws = sheet;
                    for (int row = 0; row < dt1.Rows.Count; row++)
                    {
                        for (int column = 0; column < dt1.Columns.Count; column++)
                        {
                            ws.Cells.Add(row + 1, column + 1, dt1.Rows[row][column].ToString());
                        }
                    }

                    doc.Save();
                }




            }
        }



    }
}
