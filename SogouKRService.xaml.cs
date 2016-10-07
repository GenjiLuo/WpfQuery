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
using System.Runtime.InteropServices;//引用命名空间

using System.Data;

using WpfQuery.SgKRService;


namespace WpfQuery
{
    /// <summary>
    /// SgKRService.xaml 的交互逻辑
    /// </summary>
    public partial class SogouKRService : UserControl
    {
        #region "声明变量"
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retval, int size, string filePath);
        private string strFilePath = "config\\account.ini";//获取INI文件路径
        private string strSec = ""; //INI文件名
        #endregion

        DataTable dt = new DataTable();


        public SogouKRService()
        {
            InitializeComponent();

            dt.Columns.Add("id", typeof(Int32));
            dt.Columns.Add("key", typeof(string));
            dt.Columns.Add("day", typeof(string));
            dt.Columns.Add("vie", typeof(string));

            dgQueryResult.ItemsSource = dt.DefaultView;

        }

        private void btGet_Click(object sender, RoutedEventArgs e)
        {
            if (txtKeyword.Text.Trim() == "")
            {
                MessageBox.Show("请输入关键词！");
            }
            else
            {
                dt.Rows.Clear();
                string key = txtKeyword.Text.Trim();//赋值
                AuthHeader h = new AuthHeader();
                //读取
                if (File.Exists(strFilePath))//读取时先要判读INI文件是否存在
                {
                    strSec = System.IO.Path.GetFileNameWithoutExtension(strFilePath);
                    h.username = ContentValue(strSec, "SoUsername");
                    h.password = Common.DecryptDES(ContentValue(strSec, "SoPassword"), "WpfQuery"); //ContentValue(strSec, "Password");
                    h.token = ContentValue(strSec, "SoToken");
                }
                else
                {
                    MessageBox.Show("account.ini文件不存在");
                    return;
                }
                KRServiceClient client = new KRServiceClient();

                SeedWordKRResultType[] response = getKRbySeedWord(client, h, txtKeyword.Text.Trim());

                
                
                int num = 1;
                if (response != null)
                {
                    foreach (var ba in response)
                    {
                        foreach (var ss in ba.krResultTypes)
                        {
                            dt.Rows.Add(num, ss.word, SogouRule(ss.broadPV), SogouRule(ss.competition));
                            num++;
                        }
                    }
                }
            }
        }

        public string SogouRule(int sg)
        {
            if (sg == 1)
            {
                return "低";
            }
            else if (sg == 2)
            {
                return "中";
            }
            else
            {
                return "高";
            }
        }

        public static SeedWordKRResultType[] getKRbySeedWord(KRServiceClient client, AuthHeader h, string keyword)
        {
            //SeedFilter kt = new SeedFilter();

            //kt.matchType = 1;
            //kt.matchTypeSpecified = true;

            //kt.negativeWord = new string[] { };

            //kt.maxNum = 300;
            //kt.maxNumSpecified = true;

            SeedWordKRResultType[] response;
            string[] seedword=new string[]{keyword};
            FilterType ft = new FilterType();
            ft.maxNum = 300;

            ResHeader resHeader = client.getKRBySeedWords(h, seedword, ft, out response);//.getKRbySeedWord(h, keyword, kt, 1, out response);

            return response;
        }

        private string ContentValue(string Section, string key)
        {
            StringBuilder temp = new StringBuilder(1024);
            GetPrivateProfileString(Section, key, "", temp, 1024, strFilePath);
            return temp.ToString();
        }
    }
}
