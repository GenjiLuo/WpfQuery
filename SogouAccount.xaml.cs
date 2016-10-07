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


using WpfQuery.SgAccountService;
using System.IO;
using System.Runtime.InteropServices;//引用命名空间

namespace WpfQuery
{
    /// <summary>
    /// SogouAccount.xaml 的交互逻辑
    /// </summary>
    public partial class SogouAccount : Window
    {
        #region "声明变量"
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filepath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retval, int size, string filePath);
        private string strFilePath = "config\\account.ini";
        private string strSec = "";
        #endregion


        public SogouAccount()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(MainWindow_Loaded);

            //读取
            if (File.Exists(strFilePath))
            {
                strSec = System.IO.Path.GetFileNameWithoutExtension(strFilePath);
                txtUsername.Text = ContentValue(strSec, "SoUsername");
                txtPassword.Password = Common.DecryptDES(ContentValue(strSec, "SoPassword"), "WpfQuery");
                txtToken.Text = ContentValue(strSec, "SoToken");
            }
            else
            {
                MessageBox.Show("INI文件不存在");
            }
        }

        private string ContentValue(string Section, string key)
        {
            StringBuilder temp = new StringBuilder(1024);
            GetPrivateProfileString(Section, key, "", temp, 1024, strFilePath);
            return temp.ToString();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            txtUsername.Focus();
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            if (txtUsername.Text.Trim() == "")
            {
                MessageBox.Show("请输入搜狗帐号用户名！");
            }
            else if (txtPassword.Password.Trim() == "")
            {
                MessageBox.Show("请输入搜狗帐号密码！");
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
                ResHeader resHeader = client.getAccountInfo(h, out response);//.getAccountInfo(h, requestData, 1, out response);
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
        public void saveAccountINI(string username, string password, string token)
        {
            try
            {
                strSec = System.IO.Path.GetFileNameWithoutExtension(strFilePath);
                WritePrivateProfileString(strSec, "SoUsername", username, strFilePath);
                WritePrivateProfileString(strSec, "SoPassword", Common.EncryptDES(password, "WpfQuery"), strFilePath);
                WritePrivateProfileString(strSec, "SoToken", token, strFilePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());

            }
        }


    }
}
