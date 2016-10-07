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
using System.Windows.Shapes;

using WpfQuery.AccountService;

using System.IO;
using System.Runtime.InteropServices;//引用命名空间


namespace WpfQuery
{
    /// <summary>
    /// BaiduAccount.xaml 的交互逻辑
    /// </summary>
    /// 删除了继承Window
    public partial class BaiduAccount
    {

        #region "声明变量"

        /// <summary>
        /// 写入INI文件
        /// </summary>
        /// <param name="section">节点名称[如[TypeName]]</param>
        /// <param name="key">键</param>
        /// <param name="val">值</param>
        /// <param name="filepath">文件路径</param>
        /// <returns></returns>
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filepath);
        /// <summary>
        /// 读取INI文件
        /// </summary>
        /// <param name="section">节点名称</param>
        /// <param name="key">键</param>
        /// <param name="def">值</param>
        /// <param name="retval">stringbulider对象</param>
        /// <param name="size">字节大小</param>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retval, int size, string filePath);

        private string strFilePath = "config\\account.ini";//获取INI文件路径
        private string strSec = ""; //INI文件名

        #endregion


        public BaiduAccount()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(MainWindow_Loaded);


            //读取
            if (File.Exists(strFilePath))//读取时先要判读INI文件是否存在
            {
                strSec = System.IO.Path.GetFileNameWithoutExtension(strFilePath);
                txtUsername.Text = ContentValue(strSec, "Username");
                txtPassword.Password = Common.DecryptDES(ContentValue(strSec, "Password"), "WpfQuery");//密码解密Common.DecryptDES
                txtToken.Text = ContentValue(strSec, "Token");
            }
            else
            {
                MessageBox.Show("INI文件不存在");
            }

        }

        /// <summary>
        /// 自定义读取INI文件中的内容方法
        /// </summary>
        /// <param name="Section">键</param>
        /// <param name="key">值</param>
        /// <returns></returns>
        private string ContentValue(string Section, string key)
        {
            StringBuilder temp = new StringBuilder(1024);
            GetPrivateProfileString(Section, key, "", temp, 1024, strFilePath);
            return temp.ToString();
        }

        //保存帐号
        public void saveAccountINI(string username, string password, string token)
        {
            try
            {
                //根据INI文件名设置要写入INI文件的节点名称
                //此处的节点名称完全可以根据实际需要进行配置
                strSec = System.IO.Path.GetFileNameWithoutExtension(strFilePath);
                WritePrivateProfileString(strSec, "Username", username, strFilePath);
                //WritePrivateProfileString(strSec, "Password", password, strFilePath);
                //密码加密Common.EncryptDES
                WritePrivateProfileString(strSec, "Password", Common.EncryptDES(password, "WpfQuery"), strFilePath);
                WritePrivateProfileString(strSec, "Token", token, strFilePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());

            }

        }


        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            txtUsername.Focus();
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            if (txtUsername.Text.Trim() == "")
            {
                MessageBox.Show("请输入百度帐号用户名！");
            }
            else if (txtPassword.Password.Trim() == "")
            {
                MessageBox.Show("请输入百度帐号密码！");
            }
            else if (txtToken.Text.Trim() == "")
            {
                MessageBox.Show("请输入帐号Token！");
            }
            else
            {

                AuthHeader h = new AuthHeader();
                h.username = txtUsername.Text.Trim();
                h.password = txtPassword.Password.Trim();
                h.token = txtToken.Text.Trim();


                AccountServiceClient client = new AccountServiceClient();
                AccountInfoType response;
                string[] requestData = new string[] { };
                ResHeader resHeader = client.getAccountInfo(h, requestData, 1, out response);
                if (resHeader.desc == "success")
                {
                    MessageBox.Show("登录成功");
                    saveAccountINI(txtUsername.Text.Trim(), txtPassword.Password.Trim(), txtToken.Text.Trim());
                }
                else
                {
                    MessageBox.Show("登录失败");
                }
            }   
        }



        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


    }
}
