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
using System.Xml;
using System.IO;

namespace WpfQuery
{
    /// <summary>
    /// IISConfig.xaml 的交互逻辑
    /// </summary>
    public partial class IISConfig : UserControl
    {
        DataTable datat = new DataTable();

        public IISConfig()
        {
            InitializeComponent();

            datat.Columns.Add("disc", typeof(string));
            datat.Columns.Add("identifier", typeof(string));
            datat.Columns.Add("status", typeof(string));
            datat.Columns.Add("hostheadervalue", typeof(string));
            datat.Columns.Add("ip", typeof(string));
            datat.Columns.Add("port", typeof(string));
            datat.Columns.Add("path", typeof(string));
        }

        private void btIIS(object sender, RoutedEventArgs e)
        {
            datat.Rows.Clear();

            Microsoft.Win32.OpenFileDialog dialogOpenFile = new Microsoft.Win32.OpenFileDialog();
            dialogOpenFile.Filter = "IIS配置文件（*.xml）|*.xml";
            dialogOpenFile.RestoreDirectory = true;
            dialogOpenFile.Multiselect = true;//允许同时选择多个文件 
            //dialogOpenFile.InitialDirectory = "c:\\";
            //dialogOpenFile.FilterIndex = 2;
            List<string> filenames = new List<string>();
            if (dialogOpenFile.ShowDialog() == true)
            {
                //filename = dialogOpenFile.FileName;
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
                XmlDocument doc = new XmlDocument();
                doc.Load(filename);
                XmlNodeList list = doc.GetElementsByTagName("IIsWebServer");
                //XmlNodeList list_vd = doc.GetElementsByTagName("IIsWebVirtualDir");

                //修改iis配置文件--去掉多余的IIsWebVirtualDir
//                string xml = doc.OuterXml.ToString().Replace(@">
//</IIsWebVirtualDir>
//<IIsWebVirtualDir", "");
//                XmlDocument doc_vd = new XmlDocument();
//                doc_vd.LoadXml(xml);
//                XmlNodeList list_vd = doc_vd.GetElementsByTagName("IIsWebVirtualDir");

                int j=0;
                for (int i = 0; i < list.Count - 2; i++)
                {
                    XmlNode xn = list[i];
                    string disc=xn.Attributes["ServerComment"].Value;
                    string identifier = xn.Attributes["Location"].Value.Replace("/LM/W3SVC/","");
                    string status = "";
                    if (xn.Attributes["ServerAutoStart"].Value == "TRUE")
                        status = "正在运行";
                    else
                        status = "已停止";
                    string hostheadervalue = "";
                    string ip = "";
                    string port = "";
                    string[] str1 = xn.Attributes["ServerBindings"].Value.Replace("			","?").Split('?');
                    foreach (string str0 in str1)
                    {
                        string[] str2 = str0.Split(':');
                        hostheadervalue += str2[2];
                        ip = str2[0];
                        port = str2[1];
                    }
                    string path="";
                    //if (list_vd[j].Attributes["DefaultDoc"] != null)
                    //{
                    //    path = list_vd[j++].Attributes["Path"].Value;
                    //}
                    //else
                    //{
                    //    path = list_vd[++j].Attributes["Path"].Value;
                    //    ++j;
                    //}
                    datat.Rows.Add(disc, identifier, status, hostheadervalue, ip, port,path);
                }
            }

            dgQueryResult.ItemsSource = datat.DefaultView;
        }

        private void btExport(object sender, RoutedEventArgs e)
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
            excel.WriteString(0, 0, "描述");
            excel.WriteString(0, 1, "标识");
            excel.WriteString(0, 2, "状态");
            excel.WriteString(0, 3, "主机头值");
            excel.WriteString(0, 4, "IP地址");
            excel.WriteString(0, 5, "端口");
            excel.WriteString(0, 6, "目录");
            short excelline = 1;

            foreach (DataRow dr in datat.Rows)
            {
                excel.WriteString(excelline, 0, dr[0].ToString());
                excel.WriteString(excelline, 1, dr[1].ToString());
                excel.WriteString(excelline, 2, dr[2].ToString());
                excel.WriteString(excelline, 3, dr[3].ToString());
                excel.WriteString(excelline, 4, dr[4].ToString());
                excel.WriteString(excelline, 5, dr[5].ToString());
                excel.WriteString(excelline, 6, dr[6].ToString());
                excelline++;
            }
            excel.EndWrite();
        }
    }
}
