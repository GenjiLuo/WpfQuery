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

using System.Net;
using System.IO;
using System.Threading;
using System.Data;
using System.Drawing;


namespace WpfQuery
{
    /// <summary>
    /// Post.xaml 的交互逻辑
    /// </summary>
    public partial class Post : UserControl
    {

        //DataTable dt1 = new DataTable();
        int delay = 1;  //延迟秒数
        int y = 0;      //发布数
        List<string> title = new List<string>();
        string posturl = "";
        string postdata = "";
        string context = "";
        string successfeature = "";
        int s_num = 0; //成功数
        Thread th = null;
        CookieContainer cc = new CookieContainer();
        string method = "";

        public Post()
        {
            InitializeComponent();
        }

        #region 同步通过POST方式发送数据
        /// <summary>
        /// 通过POST方式发送数据
        /// </summary>
        /// <param name="Url">url</param>
        /// <param name="postDataStr">Post数据</param>
        /// <param name="cookie">Cookie容器</param>
        /// <returns></returns>
        public string SendDataByPost(string Url, string postDataStr, ref CookieContainer cookie)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            if (cookie.Count == 0)
            {
                request.CookieContainer = new CookieContainer();
                cookie = request.CookieContainer;
            }
            else
            {
                request.CookieContainer = cookie;
            }

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = postDataStr.Length;
            Stream myRequestStream = request.GetRequestStream();
            StreamWriter myStreamWriter = new StreamWriter(myRequestStream, Encoding.GetEncoding("gb2312"));
            myStreamWriter.Write(postDataStr);
            myStreamWriter.Close();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }
        #endregion

        #region 同步通过GET方式发送数据
        /// <summary>
        /// 通过GET方式发送数据
        /// </summary>
        /// <param name="Url">url</param>
        /// <param name="postDataStr">GET数据</param>
        /// <param name="cookie">GET容器</param>
        /// <returns></returns>
        public string SendDataByGET(string Url, string postDataStr, ref CookieContainer cookie)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url + (postDataStr == "" ? "" : "?") + postDataStr);
            if (cookie.Count == 0)
            {
                request.CookieContainer = new CookieContainer();
                cookie = request.CookieContainer;
            }
            else
            {
                request.CookieContainer = cookie;
            }

            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }
        #endregion

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (txtUrl.Text.Trim() == "")
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("请输入POST网址.", "警告", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                txtUrl.Focus();
                return;
            }
            else if (txtPostData.Text.Trim() == "")
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("请输入POST数据.", "警告", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                txtPostData.Focus();
                return;
            }
            else if(title.Count==0)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("请导入标题.", "警告", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            else if(txtContext.Text.Trim()=="")
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("请输入内容.", "警告", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            else
            {
                if (((ComboBoxItem)cbRequestContentType.SelectedItem).Content.ToString() != "File")
                {
                    //初始化进度条
                    pbQuery.Maximum = title.Count;
                    pbQuery.Value = y = 0;

                    posturl = txtUrl.Text.Trim();
                    postdata = txtPostData.Text;
                    context = txtContext.Text;
                    successfeature = txtSuccess.Text.Trim();
                    s_num = 0;
                    tbSuccess.Text = "0";
                    tbFail.Text = "0";
                    txtUrl.IsEnabled = false;
                    txtPostData.IsEnabled = false;
                    txtContext.IsEnabled = false;
                    btnImport.IsEnabled = false;
                    btnSubmit.IsEnabled = false;
                    //Cookie =  menuitems=1_1%2C2_1%2C3_1%2C5_1%2C4_1; ECS[visit_times]=6; lastCid=1; lastCid__ckMd5=b6bbbe5abad70174; PHPSESSID=t2l9k5uq2le928cnrn4d1pngh0
                    //注意：处理Replace(";", ",")
                    cc.SetCookies(new Uri(txtUrl.Text.Trim()), txtCookie.Text.Trim().Replace(";", ","));

                    if (((ComboBoxItem)cbMethod.SelectedItem).Content.ToString() == "POST")
                    {
                        method = "POST";
                    }
                    else
                    {
                        method = "GET";
                    }

                    th = new Thread(Go);
                    th.Start();

                    //txtRespone.Text = PostToUrl.PostDataToUrl(txtPostData.Text, txtUrl.Text);
                }
                else
                {
                    //Hashtable param = new Hashtable();
                    //foreach (ListViewItem item in lvParam.Items)
                    //{
                    //    param.Add(item.SubItems[0].Text, item.SubItems[1].Text);
                    //}

                    //List<string> files = new List<string>();
                    //foreach (ListViewItem item in lvFiles.Items)
                    //{
                    //    files.Add(item.SubItems[0].Text);
                    //}

                    //txtRespone.Text = PostToUrl.PostFileToUrl(param, files, txtUrl.Text);
                }
            }


        }

        public void Go()
        {
            for (int i = 0; i < title.Count; i++)
            {
                string responsestr = "";
                try
                {
                    ++y;
                    if (method == "POST")
                    {
                        responsestr = PostToUrl.PostDataToUrl(postdata.Replace("{title}", title[i]).Replace("{context}", context), posturl, cc);
                    }
                    else
                    {
                        responsestr = PostToUrl.SendDataByGET(posturl, postdata.Replace("{title}", title[i]).Replace("{context}", context), cc);
                    }
                }
                catch (Exception)
                {
                    Thread.CurrentThread.Abort();
                }

                OutDelegate outdelegate = new OutDelegate(OutText);
                this.Dispatcher.BeginInvoke(outdelegate, new object[] { responsestr });
                Thread.Sleep(delay*1000);
            }
        }

        public delegate void OutDelegate(string text);
        public void OutText(string text)
        {
            txtRespone.Text = text;
            pbQuery.Value = y;
            lbTitle.SelectedIndex = y-1;
            lbTitle.ScrollIntoView(lbTitle.SelectedItem);

            if (text.Contains(successfeature))
            {
                ++s_num;
                tbSuccess.Text = s_num.ToString();
            }
            else
            {
                tbFail.Text = (y - s_num).ToString();
            }

            if (y == title.Count)
            {
                txtUrl.IsEnabled = true;
                txtPostData.IsEnabled = true;
                txtContext.IsEnabled = true;
                btnImport.IsEnabled = true;
                btnSubmit.IsEnabled = true;
            }
        }


        #region 导入标题
        private void btnImport_Click(object sender, RoutedEventArgs e)
        {
            //清空
            lbTitle.Items.Clear();
            title.Clear();


            Microsoft.Win32.OpenFileDialog dialogOpenFile = new Microsoft.Win32.OpenFileDialog();
            dialogOpenFile.Filter = "关键词文件（*.txt）|*.txt";
            dialogOpenFile.RestoreDirectory = true;
            dialogOpenFile.Multiselect = true; 
            List<string> filenames = new List<string>();
            if (dialogOpenFile.ShowDialog() == true)
            {
                for (int fi = 0; fi < dialogOpenFile.FileNames.Length; fi++)
                {
                    filenames.Add(dialogOpenFile.FileNames[fi].ToString());
                }
            }
            else
            {
                return;
            }

            foreach (string filename in filenames)
            {
                if (File.Exists(filename))
                {
                    string[] str = File.ReadAllLines(filename, System.Text.Encoding.Default);
                    foreach (string item in str)
                    {
                        if (item.Trim() != "")
                        {
                            lbTitle.Items.Add(item);
                            title.Add(item);
                        }
                    }
                }

            }
        }

        #endregion

        private void cbRequestContentType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((ComboBoxItem)cbRequestContentType.SelectedItem).Content.ToString() == "Form")
            {
                PostToUrl.ContentType = PostToUrl._ContentType.Form;
                Tab.SelectedIndex = 0;
            }
            else if (((ComboBoxItem)cbRequestContentType.SelectedItem).Content.ToString() == "Json")
            {
                PostToUrl.ContentType = PostToUrl._ContentType.Json;
                Tab.SelectedIndex = 0;
            }
            else if (((ComboBoxItem)cbRequestContentType.SelectedItem).Content.ToString() == "Xml")
            {
                PostToUrl.ContentType = PostToUrl._ContentType.Xml;
                Tab.SelectedIndex = 0;
            }
            else if (((ComboBoxItem)cbRequestContentType.SelectedItem).Content.ToString() == "File")
            {
                PostToUrl.ContentType = PostToUrl._ContentType.File;
                Tab.SelectedIndex = 1;
            }
        }

        private void cbRequestCharset_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((ComboBoxItem)cbRequestCharset.SelectedItem).Content.ToString() == "UTF-8")
            {
                PostToUrl.RequestEncoding = PostToUrl._Encoding.UTF8;
            }
            else if (((ComboBoxItem)cbRequestCharset.SelectedItem).Content.ToString() == "ASCII")
            {
                PostToUrl.RequestEncoding = PostToUrl._Encoding.ASCII;
            }
            else if (((ComboBoxItem)cbRequestCharset.SelectedItem).Content.ToString() == "GB2312")
            {
                PostToUrl.RequestEncoding = PostToUrl._Encoding.GB2312;
            }

        }

        private void cbResponseCharset_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((ComboBoxItem)cbResponseCharset.SelectedItem).Content.ToString() == "UTF-8")
            {
                PostToUrl.ResponseEncoding = PostToUrl._Encoding.UTF8;
            }
            else if (((ComboBoxItem)cbResponseCharset.SelectedItem).Content.ToString() == "ASCII")
            {
                PostToUrl.ResponseEncoding = PostToUrl._Encoding.ASCII;
            }
            else if (((ComboBoxItem)cbResponseCharset.SelectedItem).Content.ToString() == "GB2312")
            {
                PostToUrl.ResponseEncoding = PostToUrl._Encoding.GB2312;
            }

        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (th.ThreadState != ThreadState.Aborted && th != null)
                {
                    th.Abort();
                }
            }
            catch (Exception)
            {

            }
            finally
            {
                txtUrl.IsEnabled = true;
                txtPostData.IsEnabled = true;
                txtContext.IsEnabled = true;
                btnImport.IsEnabled = true;
                btnSubmit.IsEnabled = true;
            }
        }

        private void btnInsertTitle_Click(object sender, RoutedEventArgs e)
        {
            string poststr = txtPostData.Text;
            string post1 = poststr.Substring(0,txtPostData.SelectionStart);
            string post2 = poststr.Substring(txtPostData.SelectionStart, txtPostData.Text.Length - txtPostData.SelectionStart);
            txtPostData.Text = post1 + "{title}" + post2;
        }

        private void btnInsertContext_Click(object sender, RoutedEventArgs e)
        {
            string poststr = txtPostData.Text;
            string post1 = poststr.Substring(0, txtPostData.SelectionStart);
            string post2 = poststr.Substring(txtPostData.SelectionStart, txtPostData.Text.Length - txtPostData.SelectionStart);
            txtPostData.Text = post1 + "{context}" + post2;
        }

        private void btnGetParam_Click(object sender, RoutedEventArgs e)
        {
            PostPrarm pp = new PostPrarm();
            pp.ShowDialog();
        }
    }
}
