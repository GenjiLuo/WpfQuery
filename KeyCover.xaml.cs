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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;
using System.IO;
using System.Threading;
using System.Text.RegularExpressions;

namespace WpfQuery
{
    /// <summary>
    /// KeyCover.xaml 的交互逻辑
    /// </summary>
    public partial class KeyCover : UserControl
    {

        DataTable dt1 = new DataTable();
        int x = 0;//总共查询数
        int y = 0;//已经查询数
        List<string> key = new List<string>();
        List<string> con = new List<string>();


        public KeyCover()
        {
            InitializeComponent();

            dt1.Columns.Add(new DataColumn("rank", typeof(int)));
            dt1.Columns.Add(new DataColumn("title", typeof(string)));
            dt1.Columns.Add(new DataColumn("exist", typeof(bool)));
            dt1.Columns.Add(new DataColumn("time", typeof(string)));
            dt1.Columns.Add(new DataColumn("rule", typeof(string)));
            dt1.Columns.Add(new DataColumn("key", typeof(string)));



        }

        private void btQuery(object sender, RoutedEventArgs e)
        {
            key.Clear();
            con.Clear();

            if (tbQueryKey.Text.Trim() == String.Empty || tbQueryCondition.Text.Trim() == String.Empty)
            {
                MessageBox.Show("请输入关键词或条件！");
                return;
            }
            else
            {
                //关键词长
                if (tbQueryKey.LineCount > tbQueryCondition.LineCount)
                {
                    for (int i = tbQueryCondition.LineCount; i < tbQueryKey.LineCount; i++)
                    {
                        if (tbQueryKey.GetLineText(i).Trim() != "")
                        {
                            MessageBox.Show("关键词与条件不匹配");
                            return;
                        }
                    }

                    //等长
                    for (int i = 0; i < tbQueryCondition.LineCount; i++)
                    {
                        if (tbQueryKey.GetLineText(i).Trim() != "" && tbQueryCondition.GetLineText(i).Trim() != "")
                        {
                            //有有
                            key.Add(tbQueryKey.GetLineText(i).Trim());
                            con.Add(tbQueryCondition.GetLineText(i).Trim());
                        }
                        else if (tbQueryKey.GetLineText(i).Trim() != "" && tbQueryCondition.GetLineText(i).Trim() == "")
                        {
                            //有空
                            MessageBox.Show("没有对应的条件，行数：" + (i + 1).ToString());
                            return;
                        }
                        else if (tbQueryKey.GetLineText(i).Trim() == "" && tbQueryCondition.GetLineText(i).Trim() != "")
                        {
                            //空有
                            MessageBox.Show("没有对应的关键词，行数：" + (i + 1).ToString());
                            return;
                        }
                    }

                }
                //条件长
                else if (tbQueryKey.LineCount < tbQueryCondition.LineCount)
                {
                    for (int i = tbQueryKey.LineCount; i < tbQueryCondition.LineCount; i++)
                    {
                        if (tbQueryCondition.GetLineText(i).Trim() != "")
                        {
                            MessageBox.Show("关键词与条件不匹配");
                            return;
                        }
                    }

                    //等长
                    for (int i = 0; i < tbQueryKey.LineCount; i++)
                    {
                        if (tbQueryKey.GetLineText(i).Trim() != "" && tbQueryCondition.GetLineText(i).Trim() != "")
                        {
                            //有有
                            key.Add(tbQueryKey.GetLineText(i).Trim());
                            con.Add(tbQueryCondition.GetLineText(i).Trim());
                        }
                        else if (tbQueryKey.GetLineText(i).Trim() != "" && tbQueryCondition.GetLineText(i).Trim() == "")
                        {
                            //有空
                            MessageBox.Show("没有对应的条件，行数：" + (i + 1).ToString());
                            return;
                        }
                        else if (tbQueryKey.GetLineText(i).Trim() == "" && tbQueryCondition.GetLineText(i).Trim() != "")
                        {
                            //空有
                            MessageBox.Show("没有对应的关键词，行数：" + (i + 1).ToString());
                            return;
                        }
                    }


                }
                else
                {
                    //等长
                    for (int i = 0; i < tbQueryKey.LineCount; i++)
                    {
                        if (tbQueryKey.GetLineText(i).Trim() != "" && tbQueryCondition.GetLineText(i).Trim() != "")
                        {
                            //有有
                            key.Add(tbQueryKey.GetLineText(i).Trim());
                            con.Add(tbQueryCondition.GetLineText(i).Trim());
                        }
                        else if (tbQueryKey.GetLineText(i).Trim() != "" && tbQueryCondition.GetLineText(i).Trim() == "")
                        {
                            //有空
                            MessageBox.Show("没有对应的条件，行数：" + (i + 1).ToString());
                            return;
                        }
                        else if (tbQueryKey.GetLineText(i).Trim() == "" && tbQueryCondition.GetLineText(i).Trim() != "")
                        {
                            //空有
                            MessageBox.Show("没有对应的关键词，行数：" + (i + 1).ToString());
                            return;
                        }
                    }

                }



            }

            //////////////////////////
            tbTotal.Text = "关键词总数：" + key.Count.ToString();
            pbQuery.Maximum = x = key.Count;
            pbQuery.Value = 0;
            Tab.SelectedIndex = 1;
            dt1.Clear();

            //ThreadPool.SetMaxThreads(1, 1);
            //for (int i = 0; i < key.Count; i++)
            //{
            //    ThreadPool.QueueUserWorkItem(new WaitCallback(Gather), new KeyRule { key = key[i], rule = con[i] });
            //}

            //等待主线程执行完
            var mainAutoResetEvent = new AutoResetEvent(false);
            ThreadPool.QueueUserWorkItem(new WaitCallback(delegate(object obj)
            {
                Thread.Sleep(1000);
                for (int i = 0; i < key.Count; i++)
                {
                    ThreadPool.QueueUserWorkItem(new WaitCallback(Gather), new KeyRule { key = key[i], rule = con[i] });
                }
            }));
            RegisteredWaitHandle registeredWaitHandle = null;
            registeredWaitHandle = ThreadPool.RegisterWaitForSingleObject(new AutoResetEvent(false), new WaitOrTimerCallback(delegate(object obj, bool timeout)
            {
                int workerThreads = 0;
                int maxWordThreads = 0;
                int compleThreads = 0;
                ThreadPool.GetAvailableThreads(out workerThreads, out compleThreads);
                ThreadPool.GetMaxThreads(out maxWordThreads, out compleThreads);
                //当可用的线数与池程池最大的线程相等时表示线程池中所有的线程已经完成
                if (workerThreads == maxWordThreads)
                {
                    mainAutoResetEvent.Set();
                    registeredWaitHandle.Unregister(null);
                }
            }), null, 1000, false);
            mainAutoResetEvent.WaitOne();


            BuildDataSource();
            DataSource = new ObservableCollection<Person>();
            foreach (var p in People)
            {
                p.PropertyChanged += p_PropertyChanged;
                DataSource.Add(p);
            }
            TreeGrid.ItemsSource = DataSource;

            //统计
            DataRow[] include = dt1.Select("exist=true");
            tbInclude.Text = "  首页排名总个数：" + include.Length.ToString();
            tbIncludeRate.Text = "   比率：" + (Convert.ToDouble(include.Length) / dt1.Rows.Count).ToString("0.00");
        }

        public class KeyRule
        {
            public string key { get; set; }
            public string rule { get; set; }
        }


        private void Gather(object obj)
        {
            KeyRule keyrule = obj as KeyRule;
            string key = keyrule.key;
            string rule = keyrule.rule;

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

                Regex reg = new Regex(@"\d{4}-\d{1,2}-\d{1,2}");
                Match mat = reg.Match(Common.StripHT(html));
                string time = mat.Value;
                bool exist = false;
                string[] gzstr= rule.Split('|');
                foreach (string gz in gzstr)
                {
                    if (title.Contains(gz.Trim()))
                    {
                        exist = true;
                        break;
                    }
                }

                if (title != "")
                {
                    dt1.Rows.Add(new object[6] { n, title, exist, time, rule, key });
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
            for (int i = 0; i < DataSource.Count; i++)
            {
                var p = DataSource[i];
                DataSource.Remove(p);
            }
            TreeGrid.ItemsSource = DataSource;

            Tab.SelectedIndex = 0;
        }

        #region
        private ObservableCollection<Person> People;
        private ObservableCollection<Person> DataSource;
        private void BuildDataSource()
        {
            People = new ObservableCollection<Person>();
            for (int i = 0; i < key.Count; i++)
            {
                var p1 = new Person("ID" + i.ToString(), key[i], "", "", "");
                p1.MarginLeft = 10;
                DataRow[] dr = dt1.Select("key='"+key[i]+"'");
                for (int j = 0; j < dr.Length; j++)
                {
                    var p2 = new Person(dr[j]["rank"].ToString(), dr[j]["title"].ToString(), dr[j]["time"].ToString(), dr[j]["exist"].ToString(), dr[j]["rule"].ToString());
                    p2.MarginLeft = 30;
                    p2.IsVisible = Visibility.Collapsed;
                    p1.Subordinates.Add(p2);
                }
                People.Add(p1);
            }
        }

        void p_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var selectedObj = sender as Person;
            if (selectedObj == null)
                return;
            if (selectedObj.IsChecked)
            {
                int next = DataSource.IndexOf(selectedObj) + 1;
                for (int i = 0; i < selectedObj.Subordinates.Count;i++ )
                {
                    var p = selectedObj.Subordinates[i];
                    if (DataSource.FirstOrDefault(t => t.Id == p.Id) != null)
                        return;
                    p.PropertyChanged += p_PropertyChanged;
                    p.IsChecked = false;
                    DataSource.Insert(next++, p);
                }
            }
            else if (!selectedObj.IsChecked)
            {
                for (int i = 0; i < selectedObj.Subordinates.Count; i++)
                {
                    var p = selectedObj.Subordinates[i];
                    RemoveNode(p);
                }
            }
        }

        private void RemoveNode(Person person)
        {
            for (int i = 0; i < person.Subordinates.Count; i++)
            {
                var p = person.Subordinates[i];
                RemoveNode(p);
            }
            for (int i = 0; i < DataSource.Count; i++)
            {
                var p = DataSource[i];
                if (p.Id == person.Id)
                    DataSource.Remove(p);
            }
        }

        #endregion
    }


    class Person : INotifyPropertyChanged
    {
        private String _id;
        public String Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }
        public String _name;
        public String Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }
        public String _time;
        public String Time
        {
            get
            {
                return _time;
            }
            set
            {
                _time = value;
            }
        }

        public String _exist;
        public String Exist
        {
            get
            {
                return _exist;
            }
            set
            {
                _exist = value;
            }
        }

        public String _rule;
        public String Rule
        {
            get
            {
                return _rule;
            }
            set
            {
                _rule = value;
            }
        }


        private double _marginLeft;
        public double MarginLeft
        {
            get
            {
                return _marginLeft;
            }
            set
            {
                _marginLeft = value;
            }
        }

        private bool _isChecked = false;
        public bool IsChecked
        {
            get
            {
                return _isChecked;
            }
            set
            {
                _isChecked = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("IsChecked"));
            }
        }

        private Visibility _isVisible = Visibility.Visible;
        public Visibility IsVisible
        {
            get
            {
                return _isVisible;
            }
            set
            {
                _isVisible = value;
            }
        }

        private ObservableCollection<Person> _subordinates;
        public ObservableCollection<Person> Subordinates
        {
            get
            {
                return _subordinates ?? (_subordinates = new ObservableCollection<Person>());
            }
            set
            {
                _subordinates = value;
            }
        }

        public Person()
        {
        }
        public Person(String id, String name, String time, String exist, String rule)
        {
            Id = id;
            Name = name;
            Time = time;
            Exist = exist;
            Rule = rule;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
