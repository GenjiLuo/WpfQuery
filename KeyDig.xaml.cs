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

namespace WpfQuery
{
    /// <summary>
    /// KeyDig.xaml 的交互逻辑
    /// 逐级挖掘
    /// </summary>
    public partial class KeyDig : UserControl
    {
        DataTable dt = new DataTable();
        int id = 1;
        int y = 0;//已经查询数
        List<string> theadkey = new List<string>();
        int grade;
        //ObservableCollection<TestClass> list = new ObservableCollection<TestClass>();

        public KeyDig()
        {

            InitializeComponent();

            dt.Columns.Add("id", typeof(Int32));
            dt.Columns.Add("key", typeof(string));
            dt.Columns.Add("engine", typeof(string));
            lbDigKey.DataContext = dt;

            //List<TestClass> list = new List<TestClass>();
            //list.Add(new TestClass() { id = 1, key = "first line", engine = "baidu" });
            //list.Add(new TestClass() { id = 2, key = "second line", engine = "360" });
            //list.Add(new TestClass() { id = 3, key = "xxxxxxx", engine = "sogou" });
            //list.Add(new TestClass() { id = 4, key = "fsdfafas", engine = "sogou" });
            //list.Add(new TestClass() { id = 5, key = "sdafasdfasd", engine = "baidu" });
            //this.lbDigKey.ItemsSource = list;
        }

        //public class TestClass
        //{
        //    public int id { get; set; }
        //    public string key { get; set; }
        //    public string engine { get; set; }
        //}



        WaitingDlg dlg = new WaitingDlg();
        private void btDig(object sender, RoutedEventArgs e)
        {
            dlg.ShowInTaskbar = false;
            dlg.Topmost = true;


            List<string> key = new List<string>();
            id = 1;
            includekey = IncludeKey.Text.Trim();
            theadkey.Clear();

            if (txtQueryKey.Text.Trim() == String.Empty)
            {
                MessageBox.Show("请输入要挖掘的关键词！");
                return;
            }
            else
            {
                for (int i = 0; i < txtQueryKey.LineCount; i++)
                {
                    if (txtQueryKey.GetLineText(i).Trim() != "")
                    {
                        key.Add(txtQueryKey.GetLineText(i).Trim());
                        theadkey.Add(txtQueryKey.GetLineText(i).Trim());//线程
                    }
                }
            }
            dt.Clear();

            grade = Convert.ToInt16(sliderGrade.Value);
            pbQuery.Maximum = key.Count;
            pbQuery.Value = 0;
            y = 0;

            //显示等待画面
            dlg.Show();

            //线程池
            //ThreadPool.SetMaxThreads(2, 2);
            //for (int i = 0; i < key.Count; i++)
            //{
            //    ThreadPool.QueueUserWorkItem(new WaitCallback(Gather), new keyengine { key = key[i], engine = "Baidu" });
            //}



            //等待主线程执行完---------------------------------------
            //var mainAutoResetEvent = new AutoResetEvent(false);
            //ThreadPool.QueueUserWorkItem(new WaitCallback(delegate(object obj)
            //{
            //    Thread.Sleep(100);
            //    for (int i = 0; i < key.Count; i++)
            //    {
            //        ThreadPool.QueueUserWorkItem(new WaitCallback(Gather), new keyengine { key = key[i], engine = "Baidu" });
            //    }
            //}));
            //RegisteredWaitHandle registeredWaitHandle = null;
            //registeredWaitHandle = ThreadPool.RegisterWaitForSingleObject(new AutoResetEvent(false), new WaitOrTimerCallback(delegate(object obj, bool timeout)
            //{
            //    int workerThreads = 0;
            //    int maxWordThreads = 0;
            //    int compleThreads = 0;
            //    ThreadPool.GetAvailableThreads(out workerThreads, out compleThreads);
            //    ThreadPool.GetMaxThreads(out maxWordThreads, out compleThreads);
            //    //当可用的线数与池程池最大的线程相等时表示线程池中所有的线程已经完成
            //    if (workerThreads == maxWordThreads)
            //    {
            //        mainAutoResetEvent.Set();
            //        registeredWaitHandle.Unregister(null);
            //    }
            //}), null, 1000, false);
            //mainAutoResetEvent.WaitOne();
            //-------------------------------------------------

            //普通线程-----------------------------------------
            //Thread staThread = new Thread(new ThreadStart(Go));
            //staThread.SetApartmentState(ApartmentState.STA);
            //staThread.Start();
            //staThread.Join();

            txtQueryKey.IsReadOnly = true;
            IncludeKey.IsEnabled = false;
            buttonDig.IsEnabled = false;
            buttonExport.IsEnabled = false;
            Thread t = new Thread(Go);
            t.Start();

            //-------------------------------------------------
            //不用线程
            //Go();
            //End();
        }

        public void Go()
        {
            for (int i = 0; i < theadkey.Count; i++)
            {
                Gather(new keyengine { key = theadkey[i], engine = "Baidu" });
            }
        }

        //public void End()
        //{
        //    Baidu.YValue = dt.Select("engine='Baidu'").Length;
        //    So.YValue = dt.Select("engine='360'").Length;
        //    Sogou.YValue = dt.Select("engine='Sogou'").Length;
        //}

        public class keyengine
        {
            public string  key { get; set; }
            public string engine { get; set; }
        }


        private void Gather(object obj)
        {
            keyengine ke = obj as keyengine;
            string key = ke.key;
            string engine = ke.engine;

            StreamReader sr;
            try
            {
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create("http://www.baidu.com/s?wd=" + key);
                HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
                Stream srtStream = webResponse.GetResponseStream();
                sr = new StreamReader(srtStream, Encoding.GetEncoding("utf-8"));
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
            List<string> second = new List<string>();
            //---------百度修改了代码：20141009：<div id="rs"><div class="tt">相关搜索</div> ....  </table></div>
            Regex reg = new Regex("<div id=\"rs\">.*?</table></div>");
            Match mat = reg.Match(totalLines);
            //MessageBox.Show(mat.Value);
            if (mat.Success)
            {
                Regex href = new Regex(@"<a\s*[^>]*>([\s\S]+?)</a>", RegexOptions.IgnoreCase);
                MatchCollection like_key = href.Matches(mat.Value);
                foreach (Match ma in like_key)
                {
                    if (dt.Select("key='" + ma.Groups[1].Value + "'").Length == 0)
                    {
                        second.Add("http://www.baidu.com" + Common.MatchURL(ma.Groups[0].Value,"<a href=\"","\">"));
                        if (includekey == "")
                        {
                            dt.Rows.Add(id++, ma.Groups[1].Value, engine);
                        }
                        else if (MatchRule(ma.Groups[1].Value))
                        {
                            dt.Rows.Add(id++, ma.Groups[1].Value, engine);
                        }
                    }
                }
            }


            Thread.Sleep(500);

            if (grade > 1)
            {
                SecondGrade(second);
            }

            //实时更新进度条
            y++;
            Thread trd = new Thread(new ThreadStart(this.ThreadTask));
            trd.IsBackground = true;
            trd.Start();


            //修改图标控件
            OutDelegate outdelegate = new OutDelegate(OutText);
            this.Dispatcher.BeginInvoke(outdelegate, new object[] { "xx" });
        }

        public delegate void OutDelegate(string text);
        public void OutText(string text)
        {
            Baidu.YValue = dt.Select("engine='Baidu'").Length;
            So.YValue = dt.Select("engine='360'").Length;
            Sogou.YValue = dt.Select("engine='Sogou'").Length;
            lbDigKey.Items.Refresh();
            if (Convert.ToDouble(y) == pbQuery.Maximum)
            {
                dlg.Hide();
                //dlg.Close();
                txtQueryKey.IsReadOnly = false;
                IncludeKey.IsEnabled = true;
                buttonDig.IsEnabled = true;
                buttonExport.IsEnabled = true;
            }
        }

        public void SecondGrade(List<string> second )
        {
            foreach (string url in second)
            {
                GradeGather(url);
            }
        }

        private void GradeGather(string url)
        {
            string engine = "Baidu";

            StreamReader sr;
            try
            {
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
                Stream srtStream = webResponse.GetResponseStream();
                sr = new StreamReader(srtStream, Encoding.GetEncoding("utf-8"));
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
            List<string> three = new List<string>();
            Regex reg = new Regex("<div id=\"rs\">.*?</table></div>");
            Match mat = reg.Match(totalLines);
            if (mat.Success)
            {
                Regex href = new Regex(@"<a\s*[^>]*>([\s\S]+?)</a>", RegexOptions.IgnoreCase);
                MatchCollection like_key = href.Matches(mat.Value);
                foreach (Match ma in like_key)
                {
                    if (dt.Select("key='" + ma.Groups[1].Value + "'").Length == 0)
                    {
                        three.Add("http://www.baidu.com" + Common.MatchURL(ma.Groups[0].Value, "<a href=\"", "\">"));
                        if (includekey == "")
                        {
                            dt.Rows.Add(id++, ma.Groups[1].Value, engine);
                        }
                        else if (MatchRule(ma.Groups[1].Value))
                        {
                            dt.Rows.Add(id++, ma.Groups[1].Value, engine);
                        }
                    }
                }
            }


            Thread.Sleep(500);

            if (grade > 2)
            {
                foreach (string threeurl in three)
                {
                    ThreeGrade(threeurl);
                }
            }

        }

        private void ThreeGrade(string url)
        {
            string engine = "Baidu";

            StreamReader sr;
            try
            {
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
                Stream srtStream = webResponse.GetResponseStream();
                sr = new StreamReader(srtStream, Encoding.GetEncoding("utf-8"));
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
            Regex reg = new Regex("<div id=\"rs\">.*?</table></div>");
            Match mat = reg.Match(totalLines);
            if (mat.Success)
            {
                Regex href = new Regex(@"<a\s*[^>]*>([\s\S]+?)</a>", RegexOptions.IgnoreCase);
                MatchCollection like_key = href.Matches(mat.Value);
                foreach (Match ma in like_key)
                {
                    if (dt.Select("key='" + ma.Groups[1].Value + "'").Length == 0)
                    {
                        if (includekey == "")
                        {
                            dt.Rows.Add(id++, ma.Groups[1].Value, engine);
                        }
                        else if (MatchRule(ma.Groups[1].Value))
                        {
                            dt.Rows.Add(id++, ma.Groups[1].Value, engine);
                        }
                    }
                }
            }


            Thread.Sleep(500);
        }

        string includekey = "";
        public bool MatchRule(string context)
        {
            string[] str = includekey.Split('|');
            foreach (string s1 in str)
            {
                if (context.Contains(s1.Trim()))
                {
                    return true;
                }
            }
            return false;
        }


        private delegate void UpdateProgressBarDelegate(System.Windows.DependencyProperty dp, Object value);
        private void ThreadTask()
        {
            UpdateProgressBarDelegate updatePbDelegate = new UpdateProgressBarDelegate(pbQuery.SetValue);
            Dispatcher.Invoke(updatePbDelegate, System.Windows.Threading.DispatcherPriority.Background, new object[] { System.Windows.Controls.ProgressBar.ValueProperty, Convert.ToDouble(y) });
        }

        //lbDigKey排序
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

        private void btExport(object sender, RoutedEventArgs e)
        {
            if (lbDigKey.Items.Count == 0)
            {
                MessageBox.Show("没有数据！");
            }
            else
            {
                string filename = "";
                //Microsoft.Win32.OpenFileDialog dialogOpenFile = new Microsoft.Win32.OpenFileDialog();
                //dialogOpenFile.ShowDialog();
                Microsoft.Win32.SaveFileDialog dialogSaveFile = new Microsoft.Win32.SaveFileDialog();
                dialogSaveFile.Filter = "文本文件（*.txt）|*.txt";
                if (dialogSaveFile.ShowDialog() == true)
                {
                    filename = dialogSaveFile.FileName;
                }
                else
                {
                    return;
                }

                StreamWriter sw = new StreamWriter(filename, true, Encoding.GetEncoding("gb2312"));
                foreach (DataRow dr in dt.Rows)
                {
                    string str = dr["key"].ToString().Trim();
                    sw.WriteLine(str);
                }
                sw.Close();


            }
        }
    }
}
