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
using System.Net.NetworkInformation;

namespace WpfQuery
{
    /// <summary>
    /// Ping.xaml 的交互逻辑
    /// </summary>
    public partial class Ping : UserControl
    {
        DataTable dt1 = new DataTable();
        int x = 0;//总共查询数
        int y = 0;//已经查询数
        List<string> forbid_url = new List<string>();

        public Ping()
        {
            InitializeComponent();

            dt1.Columns.Add(new DataColumn("num", typeof(int)));
            dt1.Columns.Add(new DataColumn("url", typeof(string)));
            dt1.Columns.Add(new DataColumn("ip", typeof(string)));
            dt1.Columns.Add(new DataColumn("status", typeof(string)));
            dt1.Columns.Add(new DataColumn("time", typeof(string)));
        }

        private void btPing(object sender, RoutedEventArgs e)
        {
            List<string> url = new List<string>();
            List<int> num = new List<int>();

            if (txtQueryURL.Text.Trim() == String.Empty)
            {
                MessageBox.Show("请输入要查询的网址");
            }
            else if (txtQueryURL.Text.Trim() != String.Empty)
            {
                for (int i = 0; i < txtQueryURL.LineCount; i++)
                {
                    //匹配网址（包括一级和二级）
                    string cur_url = txtQueryURL.GetLineText(i).Trim().ToLower();
                    if (cur_url != "")
                    {
                        cur_url = cur_url.Replace("http://", "");
                        if (cur_url.Contains('/'))
                        {
                            string[] str = cur_url.Split('/');
                            cur_url = str[0];
                        }
                        else
                        {
                            Regex reg = new Regex(@"^(\w*://)?\w*\.?(\w*\.(com.cn|net.cn|org.cn|gov.cn|org|net|gov|cn|com|mobi|me|info|name|biz|cc|tv|asia|hk|网络|公司|中国)).*$");
                            Match mat = reg.Match(cur_url);
                            cur_url = mat.Groups[0].Value;
                        }
                        if (Common.isurl2("http://" + cur_url))
                        {
                            url.Add(cur_url);
                        }
                        else
                        {
                            num.Add(i);
                        }
                    }
                }
                if (num.Count != 0)
                {
                    string numstr = "";
                    for (int i = 0; i < num.Count; i++)
                    {
                        numstr += "   " + (num[i] + 1).ToString();
                    }
                    MessageBox.Show("非法网址数：" + num.Count.ToString() + "，分别在行数：" + numstr);
                }
                else
                {
                    this.Tab.SelectedIndex = 1;

                    //查询收录
                    #region
                    //初始化
                    pbQuery.Maximum = x = url.Count;
                    pbQuery.Value = 0;
                    tbTotal.Text = "总：" + url.Count.ToString() + "   ";
                    y = 0;
                    dt1.Clear();
                    for (int n = 0; n < url.Count; n++)
                    {
                        dt1.Rows.Add(new object[5] { n + 1, url[n], "", "", "" });
                    }
                    dgQueryResult.ItemsSource = dt1.DefaultView;

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
                GetPingResult_new(new Person { Id = i, Url = dt1.Rows[i][1].ToString() });
            }

            for (int i = 0; i < forbid_url.Count; i++)
            {
                string ip = getIP(forbid_url[i]);
                foreach (DataRow dr in dt1.Rows)
                {
                    if (dr[1].ToString() == forbid_url[i])
                    {
                        dr[2] = ip;
                        dr[3] = "no-ping";
                    }
                }

                y++;
                OutDelegateNew outdelegate = new OutDelegateNew(OutTextNew);
                this.Dispatcher.BeginInvoke(outdelegate, new object[] { forbid_url[i], "模式no-ping" });
            }
            foreach (DataRow dr in dt1.Rows)
            {
                if (dr[2].ToString() == "")
                {
                    dr[3] = "error";
                }
            }
        }

        private void GetPingResult_new(object son)
        {
            Person person = son as Person;
            int i = person.Id;
            string url = person.Url;

            try
            {
                System.Net.NetworkInformation.Ping p1 = new System.Net.NetworkInformation.Ping(); //只是演示，没有做错误处理 
                PingReply reply = p1.Send(url);//阻塞方式 
                if (reply.Status == IPStatus.Success)
                {
                    dt1.Rows[i][2] = reply.Address.ToString();
                    dt1.Rows[i][3] = "OK";
                    dt1.Rows[i][4] = reply.RoundtripTime;


                    y++;
                    OutDelegateNew outdelegate = new OutDelegateNew(OutTextNew);
                    this.Dispatcher.BeginInvoke(outdelegate, new object[] { url, "模式ping" });
                }
                else
                {
                    forbid_url.Add(url);
                }
            }
            catch
            {
                forbid_url.Add(url);
            }

        }

        ///<summary>
        /// 传入域名返回对应的IP
        ///</summary>
        ///<param name="domain">域名</param>
        ///<returns></returns>
        public static string getIP(string domain)
        {
            try
            {
                domain = domain.Replace("http://", "").Replace("https://", "");
                IPHostEntry hostEntry = Dns.GetHostEntry(domain);
                IPEndPoint ipEndPoint = new IPEndPoint(hostEntry.AddressList[0], 0);
                return ipEndPoint.Address.ToString();
            }
            catch
            {

            }

            return "";
        }

        public delegate void OutDelegateNew(string url, string search);
        public void OutTextNew(string url, string mode)
        {
            pbQuery.Value = y;
            ((TextBlock)pbQuery.Content).Text = pbQuery.Value + " of " + pbQuery.Maximum + "   网址：" + url + "   " + mode;
            if (y == pbQuery.Maximum)
            {
                ((TextBlock)pbQuery.Content).Foreground = Brushes.Red;
                ((TextBlock)pbQuery.Content).Text = "Ping结束！";
            }
        }

        //线程池只能带一个参数，多个参数的方法
        public class Person
        {
            public int Id { get; set; }
            public string Url { get; set; }
        }

        private void ReturnQuery(object sender, RoutedEventArgs e)
        {
            this.Tab.SelectedIndex = 0;
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
                    Worksheet sheet = doc.Workbook.Worksheets.Add("Ping");
                    //单元格格式
                    XF xf = doc.NewXF();
                    xf.Format = StandardFormats.Date_Time;

                    //列格式
                    ColumnInfo colid = new ColumnInfo(doc, sheet);
                    colid.ColumnIndexStart = 0;
                    colid.ColumnIndexEnd = 0;
                    colid.Width = 5 * 256;
                    sheet.AddColumnInfo(colid);
                    ColumnInfo colurl = new ColumnInfo(doc, sheet);
                    colurl.ColumnIndexStart = 1;
                    colurl.ColumnIndexEnd = 1;
                    colurl.Width = 30 * 256;
                    sheet.AddColumnInfo(colurl);
                    ColumnInfo colip = new ColumnInfo(doc, sheet);
                    colip.ColumnIndexStart = 2;
                    colip.ColumnIndexEnd = 2;
                    colip.Width = 30 * 256;
                    sheet.AddColumnInfo(colip);
                    ColumnInfo colstatus = new ColumnInfo(doc, sheet);
                    colstatus.ColumnIndexStart = 3;
                    colstatus.ColumnIndexEnd = 3;
                    colstatus.Width = 10 * 256;
                    sheet.AddColumnInfo(colstatus);
                    ColumnInfo coltime = new ColumnInfo(doc, sheet);
                    coltime.ColumnIndexStart = 4;
                    coltime.ColumnIndexEnd = 4;
                    coltime.Width = 8 * 256;
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
