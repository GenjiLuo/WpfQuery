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
using System.Data;
using System.ComponentModel;

using System.Net;
using System.Threading;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;

using HtmlAgilityPack;

namespace WpfQuery
{
    /// <summary>
    /// RelatedSearch.xaml 的交互逻辑
    /// 逐个获取相关搜索
    /// </summary>
    public partial class RelatedSearch : UserControl
    {
        DataTable dt1 = new DataTable();
        int id = 1;
        List<string> key = new List<string>();
        string includekey = "";
        string includestr = "";
        string noincludestr = "";
        Thread t;

        public RelatedSearch()
        {
            InitializeComponent();
            System.Diagnostics.PresentationTraceSources.SetTraceLevel(lbDigKey.ItemContainerGenerator, System.Diagnostics.PresentationTraceLevel.High);

            dt1.Columns.Add("id", typeof(Int32));
            dt1.Columns.Add("key", typeof(string));
            dt1.Columns.Add("source", typeof(string));
            dt1.Columns.Add("engine", typeof(string));

        }

        private void btRelatedSearch(object sender, RoutedEventArgs e)
        {
            id = 1;
            includekey = IncludeKey.Text.Trim();
            key.Clear();
            dt1.Clear();
            includestr = IncludeKey.Text.Trim();
            noincludestr = NoIncludeKey.Text.Trim();
            lbDigKey.Items.Clear();

            if (txtQueryKey.Text.Trim() == String.Empty)
            {
                MessageBox.Show("请输入关键词！");
                return;
            }
            else
            {
                for (int i = 0; i < txtQueryKey.LineCount; i++)
                {
                    if (txtQueryKey.GetLineText(i).Trim() != "" && !key.Contains(txtQueryKey.GetLineText(i).Trim()))
                    {
                        key.Add(txtQueryKey.GetLineText(i).Trim());
                        dt1.Rows.Add(new object[4] { id++, txtQueryKey.GetLineText(i).Trim(), txtQueryKey.GetLineText(i).Trim(), "So" });
                    }
                }
            }

            txtQueryKey.IsReadOnly = true;
            IncludeKey.IsEnabled = false;
            NoIncludeKey.IsEnabled = false;
            buttonDig.IsEnabled = false;

            t = new Thread(Go);
            t.Start();
        }

        public void Go()
        {
            for (int i = 0; i < 10000; i++)
            {
                if (i < dt1.Rows.Count)
                {
                    getSoRelatedword(dt1.Rows[i]["key"].ToString());
                }
                else
                {
                    OutDelegateEnd outdelegate = new OutDelegateEnd(OutTextEnd);
                    this.Dispatcher.BeginInvoke(outdelegate);
                }
            }
        }

        public delegate void OutDelegateEnd();
        public void OutTextEnd()
        {
            txtQueryKey.IsReadOnly = false;
            IncludeKey.IsEnabled = true;
            NoIncludeKey.IsEnabled = true;
            buttonDig.IsEnabled = true;
        }

        #region 360相关搜索词
        private void getSoRelatedword(string sourcekey)
        {
            string pageUrl = "http://www.so.com/s?ie=utf-8&shb=1&src=360sou_newhome&q=" + sourcekey;
            WebClient wc = new WebClient();
            byte[] pageSourceBytes = wc.DownloadData(new Uri(pageUrl));
            string pageSource = Encoding.GetEncoding("utf-8").GetString(pageSourceBytes);
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(pageSource);


            HtmlNodeCollection keyNodes = doc.DocumentNode.SelectNodes("//*[@id=\"rs\"]");
            Regex href = new Regex(@"<a\s*[^>]*>([\s\S]+?)</a>", RegexOptions.IgnoreCase);
            try
            {
                MatchCollection like_key = href.Matches(keyNodes[0].InnerHtml);
                foreach (Match ma in like_key)
                {
                    string str = ma.Groups[1].Value;
                    if (MatchInclude(str) && MatchNoInclude(str) && MatchDataTable(str))
                    {
                        dt1.Rows.Add(new object[4] { id++, ma.Groups[1].Value, sourcekey, "So" });

                        OutDelegateNew outdelegate = new OutDelegateNew(OutTextNew);
                        this.Dispatcher.BeginInvoke(outdelegate, new object[4] { id, ma.Groups[1].Value, sourcekey, "So" });
                    }
                }
            }
            catch
            {

            }

            Thread.Sleep(500);
        }
        #endregion

        public delegate void OutDelegateNew(int id, string key, string sourcekey, string search);
        public void OutTextNew(int id, string key,string sourcekey, string search)
        {
            tbQuerying.Text = sourcekey;
            lbDigKey.Items.Add(new { id = id, key = key, source = sourcekey, engine=search });
        }


        #region 包涵与不包涵
        public bool MatchInclude(string context)
        {
            if (includestr == "")
            {
                return true;
            }
            else
            {
                string[] str = includestr.Split('|');
                foreach (string s1 in str)
                {
                    if (context.Contains(s1.Trim()))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool MatchNoInclude(string context)
        {
            if (noincludestr == "")
            {
                return true;
            }
            else
            {
                string[] str = noincludestr.Split('|');
                bool isclude = false;
                foreach (string s1 in str)
                {
                    if (context.Contains(s1.Trim()))
                    {
                        isclude = true;
                    }
                }
                return !isclude;
            }
        }
        
        public bool MatchDataTable(string context)
        {
            foreach (DataRow dr in dt1.Rows)
            {
                if (dr["source"].ToString() == context.Trim())
                {
                    return false;
                }
            }
            return true;
        }
        #endregion

        #region 导出
        private void btExport(object sender, RoutedEventArgs e)
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

                ExcelWriterOC excel = new ExcelWriterOC();
                excel.ExcelWriter(filename);
                excel.BeginWrite();
                excel.WriteString(0, 0, "ID");
                excel.WriteString(0, 1, "关键词");
                excel.WriteString(0, 2, "源词");
                excel.WriteString(0, 3, "搜索引擎");
                short excelline = 1;

                foreach (DataRow dr in dt1.Rows)
                {
                    excel.WriteString(excelline, 0, dr[0].ToString());
                    excel.WriteString(excelline, 1, dr[1].ToString());
                    excel.WriteString(excelline, 2, dr[2].ToString());
                    excel.WriteString(excelline, 3, dr[3].ToString());
                    excelline++;
                }
                excel.EndWrite();


            }
        }
        #endregion

        #region lbDigKey排序
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
        #endregion

        #region 线程控制
        private void btStop(object sender, RoutedEventArgs e)
        {
            t.Abort();
            txtQueryKey.IsReadOnly = false;
            IncludeKey.IsEnabled = true;
            NoIncludeKey.IsEnabled = true;
            buttonDig.IsEnabled = true;
        }

        private void btSuspend(object sender, RoutedEventArgs e)
        {
            if (buttonSuspend.Content.ToString() == "暂停")
            {
                t.Suspend();
                buttonStop.IsEnabled = false;
                buttonSuspend.Content = "继续";
            }
            else
            {
                t.Resume();
                buttonStop.IsEnabled = true;
                buttonSuspend.Content = "暂停";
            }
        }
        #endregion
    }
}
