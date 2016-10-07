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
using System.IO;
using System.Net;


namespace WpfQuery
{
    /// <summary>
    /// Downword.xaml 的交互逻辑
    /// </summary>
    /// 
    public partial class Downword : UserControl
    {
        DataTable dt1 = new DataTable();
        DataTable dt2 = new DataTable();
        int id = 0;
        int x = 0;//总共查询数
        int y = 0;//已经查询数
        List<string> key = new List<string>();
        string includestr = "";
        string noincludestr = "";
        bool iscycle = false;
        Thread baiduthread;
        Thread taobaothread;
        Thread sothread;
        Thread sogouthread;
        int searchindex = 0;

        public Downword()
        {
            InitializeComponent();

            dt1.Columns.Add(new DataColumn("id", typeof(int)));
            dt1.Columns.Add(new DataColumn("downword", typeof(string)));
            dt1.Columns.Add(new DataColumn("source", typeof(string)));
            dt1.Columns.Add(new DataColumn("search", typeof(string)));

            dt2.Columns.Add(new DataColumn("id", typeof(int)));
            dt2.Columns.Add(new DataColumn("downword", typeof(string)));
            dt2.Columns.Add(new DataColumn("source", typeof(string)));
            dt2.Columns.Add(new DataColumn("search", typeof(string)));

        }

        #region 获取下拉词按钮
        private void Query(object sender, RoutedEventArgs e)
        {
            id = 0;
            dt1.Clear();
            tvKey.Items.Clear();
            key.Clear();
            includestr = IncludeKey.Text.Trim();
            noincludestr = NoIncludeKey.Text.Trim();
            dgQueryResult.Items.Clear();

            if (IsCycle.IsChecked == true)
            {
                iscycle = true;
            }
            else
            {
                iscycle = false;
            }

            for (int i = 0; i < txtQueryKey.LineCount; i++)
            {
                if (txtQueryKey.GetLineText(i).Trim() != "")
                {
                    if (!key.Contains(txtQueryKey.GetLineText(i).Trim()))
                    {
                        key.Add(txtQueryKey.GetLineText(i).Trim());
                        tvKey.Items.Add(txtQueryKey.GetLineText(i).Trim());
                        dt1.Rows.Add(new object[4] { id++, txtQueryKey.GetLineText(i).Trim(), txtQueryKey.GetLineText(i).Trim(), "" });
                    }
                }
            }

            if (key.Count == 0)
            {
                MessageBox.Show("请输入关键词！");
                return;
            }
            else
            {
                NowKey.Text = "正在查询...";
                tbTotal.Text = "关键词总数：" + key.Count.ToString();
                this.Tab.SelectedIndex = 1;
                pbQuery.Maximum = x = key.Count;
                pbQuery.Value = y = 0;
                btAll.IsEnabled = false;
                btReturn.IsEnabled = false;
                //----------------------
                if (rbBaiduMode.IsChecked == true)
                {
                    searchindex = 1;
                    baiduthread = new Thread(BaiduDeal);
                    baiduthread.Start();
                }
                else if (rbTaobaoMode.IsChecked == true)
                {
                    searchindex = 2;
                    taobaothread = new Thread(TaobaoDeal);
                    taobaothread.Start();
                }
                else if (rbSoMode.IsChecked == true)
                {
                    searchindex = 3;
                    sothread = new Thread(SoDeal);
                    sothread.Start();
                }
                else if (rbSogouMode.IsChecked == true)
                {
                    searchindex = 4;
                    sogouthread = new Thread(SogouDeal);
                    sogouthread.Start();
                }


            }

            this.Tab.SelectedIndex = 1;
            
        }
        #endregion

        #region 线程
        public void BaiduDeal()
        {
            if (!iscycle)//不循环时
            {
                for (int i = 0; i < key.Count; i++)
                {
                    getBaiduDownword2(key[i]);
                }
            }
            else//循环时
            {
                for (int i = 0; i < 10000; i++)
                {
                    if (i < dt1.Rows.Count)
                    {
                        getBaiduDownword2(dt1.Rows[i]["downword"].ToString());
                    }
                }
            }
        }

        public void TaobaoDeal()
        {
            if (!iscycle)//不循环时
            {
                for (int i = 0; i < key.Count; i++)
                {
                    getTaobaoDownword(key[i]);
                }
            }
            else//循环时
            {
                for (int i = 0; i < 10000; i++)
                {
                    if (i < dt1.Rows.Count)
                    {
                        getTaobaoDownword(dt1.Rows[i]["downword"].ToString());
                    }
                }
            }
        }

        public void SoDeal()
        {
            if (!iscycle)//不循环时
            {
                for (int i = 0; i < key.Count; i++)
                {
                    getSoDownword(key[i]);
                }
            }
            else//循环时
            {
                for (int i = 0; i < 10000; i++)
                {
                    if (i < dt1.Rows.Count)
                    {
                        getSoDownword(dt1.Rows[i]["downword"].ToString());
                    }
                }
            }
        }

        public void SogouDeal()
        {
            if (!iscycle)//不循环时
            {
                for (int i = 0; i < key.Count; i++)
                {
                    getSogouDownword(key[i]);
                }
            }
            else//循环时
            {
                for (int i = 0; i < 10000; i++)
                {
                    if (i < dt1.Rows.Count)
                    {
                        getSogouDownword(dt1.Rows[i]["downword"].ToString());
                    }
                }
            }
        }
        #endregion

        #region 百度下拉词1.0
        private void getBaiduDownword(string key)
        {
            string url = "http://suggestion.baidu.com/su?wd=" + System.Net.WebUtility.UrlEncode(key) + "&p=3&t=1273278850500";
            string filename = "downword\\su.txt";
            if (File.Exists(filename))
            {
                File.Delete(filename);
            }
            SaveFileFromUrl(filename, url);

            if (File.Exists(filename))
            {
                string sufile = File.ReadAllText(filename, System.Text.Encoding.Default);
                string[] su = sufile.Split('[')[1].Split(']')[0].Split(',');
                foreach (string item in su)
                {
                    dt1.Rows.Add(new object[4] { id++, key, item.Replace("\"",""), "Baidu"});
                }
            }

            y++;
            OutDelegateNew outdelegate = new OutDelegateNew(OutTextNew);
            this.Dispatcher.BeginInvoke(outdelegate, new object[] { key, "Baidu" });
        }

        /// <summary>
        /// 从URL地址下载文件到本地磁盘
        /// </summary>
        /// <param name="ToLocalPath">本地磁盘地址</param>
        /// <param name="Url">URL网址</param>
        /// <returns></returns>
        public static long SaveFileFromUrl(string FileName, string Url)
        {
            long Value = 0;

            WebResponse response = null;
            Stream stream = null;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);

                response = request.GetResponse();
                stream = response.GetResponseStream();

                if (!response.ContentType.ToLower().StartsWith("text/"))
                {
                    SaveBinaryFile(response, FileName);

                    Value = response.ContentLength;

                }

            }
            catch (Exception err)
            {
                Value = 0;
                string aa = err.ToString();
            }
            return Value;
        }

        /// <summary>
        /// Save a binary file to disk.
        /// </summary>
        /// <param name="response">The response used to save the file</param>
        // 将二进制文件保存到磁盘
        private static bool SaveBinaryFile(WebResponse response, string FileName)
        {
            bool Value = true;
            byte[] buffer = new byte[1024];

            try
            {
                if (File.Exists(FileName))
                    File.Delete(FileName);
                Stream outStream = System.IO.File.Create(FileName);
                Stream inStream = response.GetResponseStream();

                int l;
                do
                {
                    l = inStream.Read(buffer, 0, buffer.Length);
                    if (l > 0)
                        outStream.Write(buffer, 0, l);
                }
                while (l > 0);

                outStream.Close();
                inStream.Close();
            }
            catch
            {
                Value = false;
            }
            return Value;
        }
        #endregion

        #region 百度下拉词2.0
        private void getBaiduDownword2(string key)
        {
            string url = "http://suggestion.baidu.com/su?wd=" + System.Net.WebUtility.UrlEncode(key) + "&sugmode=2&zxmode=1&json=1&p=3";
            string source = getHtmlInfo(url);
            //写入文件
            //string filename = "downword\\su.txt";
            //StreamWriter sw = new StreamWriter(filename);//文件保存位置
            //sw.Write(source);
            //sw.Close();
            string[] su = source.Split('[')[1].Split(']')[0].Split(',');
            foreach (string item in su)
            {
                string str = item.Trim().Replace("\"", "");
                if (MatchInclude(str) && MatchNoInclude(str) && MatchDataTable(str))
                {
                    dt1.Rows.Add(new object[4] { id++,  str,key, "Baidu" });
                    //if (iscycle == true)
                    //{
                    //    OutDelegateAdd adddelegate = new OutDelegateAdd(AddTvKey);
                    //    this.Dispatcher.BeginInvoke(adddelegate, new object[] { str, "Baidu" });
                    //}
                    OutDelegateAddO adddelegate = new OutDelegateAddO(OutTextAddO);
                    this.Dispatcher.BeginInvoke(adddelegate, new object[4] { id, str , key,"Baidu" });
                }
            }

            y++;
            OutDelegateNew outdelegate = new OutDelegateNew(OutTextNew);
            this.Dispatcher.BeginInvoke(outdelegate, new object[] { key, "Baidu" });
            Thread.Sleep(500);
        }
        #endregion

        #region 淘宝下拉词
        private void getTaobaoDownword(string key)
        {
            string url = "http://suggest.taobao.com/sug?code=utf-8&q=" + System.Net.WebUtility.UrlEncode(key) + "&_ksTS=1412814635814_1642&callback=jsonp1643&k=1&area=c2c&bucketid=7";
            string source = getHtmlInfo(url);
            int startindex=source.IndexOf(":[");
            int lastindex=source.LastIndexOf("],\"magic\"");
            string taobao = "";
            string[] su = null;
            try
            {
                taobao = source.Substring(startindex + 2, lastindex - startindex - 2).Replace("\",\"", ",").Replace("\"],[\"", ",").Replace("[\"", "").Replace("\"]", "");
                su = taobao.Split(',');
            }
            catch
            {

            }
            int i = 1;
            if (su != null)
            {
                foreach (string item in su)
                {
                    if (i % 2 == 1)
                    {
                        string str = Common.StripHT(item.Trim());
                        if (MatchInclude(str) && MatchNoInclude(str) && MatchDataTable(str))
                        {
                            dt1.Rows.Add(new object[4] { id++, key, str, "Taobao" });
                            //if (iscycle == true)
                            //{
                            //    OutDelegateAdd adddelegate = new OutDelegateAdd(AddTvKey);
                            //    this.Dispatcher.BeginInvoke(adddelegate, new object[] { str, "Baidu" });
                            //}
                            OutDelegateAddO adddelegate = new OutDelegateAddO(OutTextAddO);
                            this.Dispatcher.BeginInvoke(adddelegate, new object[4] { id, str, key, "Taobao" });
                        }
                    }
                    i++;
                }
            }

            y++;
            OutDelegateNew outdelegate = new OutDelegateNew(OutTextNew);
            this.Dispatcher.BeginInvoke(outdelegate, new object[] { key, "Taobao" });
            Thread.Sleep(500);
        }
        #endregion

        #region 360下拉词
        private void getSoDownword(string key)
        {
            string url = "http://sug.so.360.cn/suggest?callback=suggest_so&encodein=utf-8&encodeout=utf-8&format=json&fields=word,obdata&word=" + key;
            string source = getHtmlInfo(url);
            string[] su = source.Split('[')[1].Split(']')[0].Split(',');
            foreach (string item in su)
            {
                string str = item.Trim().Replace("{\"word\":\"", "").Replace("\"}", "");
                if (MatchInclude(str) && MatchNoInclude(str) && MatchDataTable(str))
                {
                    dt1.Rows.Add(new object[4] { id++, key, str, "So" });
                    //if (iscycle == true)
                    //{
                    //    OutDelegateAdd adddelegate = new OutDelegateAdd(AddTvKey);
                    //    this.Dispatcher.BeginInvoke(adddelegate, new object[] { str, "So" });
                    //}
                    OutDelegateAddO adddelegate = new OutDelegateAddO(OutTextAddO);
                    this.Dispatcher.BeginInvoke(adddelegate, new object[4] { id, str, key, "So" });
                }
            }

            y++;
            OutDelegateNew outdelegate = new OutDelegateNew(OutTextNew);
            this.Dispatcher.BeginInvoke(outdelegate, new object[] { key, "So" });
            Thread.Sleep(500);
        }
        #endregion

        #region Sogou下拉词
        private void getSogouDownword(string key)
        {
            string url = "http://w.sugg.sogou.com/sugg/ajaj_json.jsp?key=" + key + "&type=web&ori=yes&pr=web&abtestid=8&ipn=";
            string source = getHtmlInfo(url);
            string[] su = null;
            try
            {
                su = source.Split('[')[2].Split(']')[0].Split(',');
            }
            catch
            {

            }
            if (su != null)
            {
                foreach (string item in su)
                {
                    string str = item.Trim().Replace("\"", "");
                    if (MatchInclude(str) && MatchNoInclude(str) && MatchDataTable(str))
                    {
                        dt1.Rows.Add(new object[4] { id++, key, str, "Sogou" });
                        //if (iscycle == true)
                        //{
                        //    OutDelegateAdd adddelegate = new OutDelegateAdd(AddTvKey);
                        //    this.Dispatcher.BeginInvoke(adddelegate, new object[] { str, "Sogou" });
                        //}
                        OutDelegateAddO adddelegate = new OutDelegateAddO(OutTextAddO);
                        this.Dispatcher.BeginInvoke(adddelegate, new object[4] { id, str, key, "Sogou" });
                    }
                }
            }

            y++;
            OutDelegateNew outdelegate = new OutDelegateNew(OutTextNew);
            this.Dispatcher.BeginInvoke(outdelegate, new object[] { key, "Sogou" });
            Thread.Sleep(500);
        }
        #endregion

        #region 修改界面
        public delegate void OutDelegateNew(string key, string search);
        public void OutTextNew(string key, string search)
        {
            pbQuery.Value = y;
            NowKey.Text = "正在查询搜索引擎：" + search + "   关键词：" + key;

            //逐个
            //tvKey.Focus();
            //tvKey.SelectedItem = (object)key;
            //tvKey.ScrollIntoView(tvKey.SelectedItem);

            if (y == pbQuery.Maximum)
            {
                NowKey.Text = "查询结束！";
                btAll.IsEnabled = true;
                btReturn.IsEnabled = true;
            }
        }

        public delegate void OutDelegateAddO(int id, string key, string source, string search);
        public void OutTextAddO(int id, string key, string source, string search)
        {
            //tbQuerying.Text = sourcekey;
            dgQueryResult.Items.Add(new { id = id, downword = key, source = source, search = search });
        }

        public delegate void OutDelegateAdd(string key, string search);
        public void AddTvKey(string key, string search)
        {
            tvKey.Items.Add(key);
        }        
        #endregion

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
                if (dr["downword"].ToString() == context.Trim() || dr["source"].ToString() == context.Trim())
                {
                    return false;
                }
            }
            return true;
        }
        #endregion

        #region 按钮
        private void ReturnQuery(object sender, RoutedEventArgs e)
        {
            this.Tab.SelectedIndex = 0;
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
                excel.WriteString(0, 1, "下拉词");
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

        private void StopQuery(object sender, RoutedEventArgs e)
        {
            switch (searchindex)
            {
                case 1:
                    baiduthread.Abort();
                    break;
                case 2:
                    taobaothread.Abort();
                    break;
                case 3:
                    sothread.Abort();
                    break;
                case 4:
                    sogouthread.Abort();
                    break;
                default:       //相当于if语句的else  
                    break;
            }
        }

        private void tvKey_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                //dgQueryResult.Items.Clear();
                if (tvKey.SelectedIndex != -1)
                {
                    DataRow[] DR = dt1.Select("source = '" + tvKey.SelectedItem.ToString() + "'");

                    dt2.Clear();
                    int i = 1;
                    foreach (DataRow dr in DR)
                    {
                        if (dr[3].ToString() != "")
                        {
                            dt2.Rows.Add(new object[4] { i, dr[1].ToString(), dr[2].ToString(), dr[3].ToString() });
                            i++;
                        }
                    }

                    dgQueryResult.ItemsSource = dt2.DefaultView;
                }
            }
            catch
            {

            }
        }
        #endregion

        #region 获取网页源码3种方法
        //WebClient 
        private string GetWebClient(string url)
        {
            string strHTML = "";
            WebClient myWebClient = new WebClient();
            Stream myStream = myWebClient.OpenRead(url);
            StreamReader sr = new StreamReader(myStream, System.Text.Encoding.GetEncoding("utf-8"));
            strHTML = sr.ReadToEnd();
            myStream.Close();
            return strHTML;
        }

        //WebRequest 
        private string GetWebRequest(string url)
        {
            Uri uri = new Uri(url);
            WebRequest myReq = WebRequest.Create(uri);
            WebResponse result = myReq.GetResponse();
            Stream receviceStream = result.GetResponseStream();
            StreamReader readerOfStream = new StreamReader(receviceStream, System.Text.Encoding.GetEncoding("utf-8"));
            string strHTML = readerOfStream.ReadToEnd();
            readerOfStream.Close();
            receviceStream.Close();
            result.Close();
            return strHTML;
        }

        //HttpWebRequest 
        private string GetHttpWebRequest(string url)
        {
            Uri uri = new Uri(url);
            HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(uri);
            myReq.UserAgent = "User-Agent:Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705";
            myReq.Accept = "*/*";
            myReq.KeepAlive = true;
            myReq.Headers.Add("Accept-Language", "zh-cn,en-us;q=0.5");
            HttpWebResponse result = (HttpWebResponse)myReq.GetResponse();
            Stream receviceStream = result.GetResponseStream();
            StreamReader readerOfStream = new StreamReader(receviceStream, System.Text.Encoding.GetEncoding("utf-8"));
            string strHTML = readerOfStream.ReadToEnd();
            readerOfStream.Close();
            receviceStream.Close();
            result.Close();
            return strHTML;
        }
        #endregion

        #region 取得HTML源码不会乱码
        //取得HTML源码
        private string getHtmlInfo(string urlSelet)
        {
            string strResult = "";
            if (urlSelet.Equals(""))
                return strResult = "";
            //HTML源码
            //Console.WriteLine("**********************=" + urlSelet);
            try
            {
                //声明一个HttpWebRequest请求 
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(urlSelet);
                webRequest.Method = "GET";
                webRequest.UserAgent = "Opera/9.25 (Windows NT 6.0; U; en)";
                HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
                //Encoding encoding = Encoding.GetEncoding("GB2312");
                //取得要取得的网页的编码方式
                Encoding encoding = GetEncoding(webResponse);
                using (System.IO.Stream stream = webResponse.GetResponseStream())
                {
                    using (System.IO.StreamReader reader = new StreamReader(stream, encoding))
                    {
                        strResult = reader.ReadToEnd();
                    }
                }
            }
            catch (Exception exp)
            {

                MessageBox.Show("出错:" + exp.Message);

            }
            return strResult;
        }

        //取得要取得的网页的编码方式
        public Encoding GetEncoding(HttpWebResponse response)
        {
            Encoding code = Encoding.Default;
            string charset = null;
            //如果发现content-type头   
            string ctypeLower = response.Headers["content-type"];
            string ctypeOrder = response.Headers["Content-Type"];
            string ctype = "";
            if (!ctypeLower.Equals(""))
                ctype = ctypeLower;
            if (!ctypeOrder.Equals(""))
                ctype = ctypeOrder;
            //Console.WriteLine("ctype:" + ctype);
            if (ctype != null)
            {
                int ind = ctype.IndexOf("charset=");
                if (ind != -1)
                {
                    charset = ctype.ToLower().Substring(ind + 8);
                }
            }
            //Console.WriteLine("charset编码格式:" + charset);
            if (charset != "")
            {
                try
                {
                    code = Encoding.GetEncoding(charset);
                }
                catch { }
            }
            return code;
        } 
        #endregion

    }
}
