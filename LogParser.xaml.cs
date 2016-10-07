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

using LogQuery = MSUtil.LogQueryClass;
using IISInputFormat = MSUtil.COMIISW3CInputContextClass;
using LogRecordSet = MSUtil.ILogRecordset;
using EvtInputFormat = MSUtil.COMETWInputContextClass;

//-----------------------
using TsvInputFormat = MSUtil.COMTSVInputContextClass;
using LogRecord = MSUtil.ILogRecord;
using CSVLogOutputFormat = MSUtil.COMCSVOutputContextClass;
//-----------------------

namespace WpfQuery
{
    /// <summary>
    /// LogParser.xaml 的交互逻辑
    /// </summary>
    public partial class LogParser : UserControl
    {
        public LogParser()
        {
            InitializeComponent();
        }

        private void btIIS(object sender, RoutedEventArgs e)
        {
            if (Common.checkLogParser())
            {
                #region 日志分析
                //string filename = "";
                Microsoft.Win32.OpenFileDialog dialogOpenFile = new Microsoft.Win32.OpenFileDialog();
                dialogOpenFile.Filter = "IIS日志文件（*.log）|*.log";
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

                //初始化
                LogQuery oLogQuery = new LogQuery();
                IISInputFormat oIISInputFormat = new IISInputFormat();
                DataTable datat = new DataTable();
                datat.Columns.Add("date", typeof(string));
                datat.Columns.Add("time", typeof(string));
                datat.Columns.Add("method", typeof(string));
                datat.Columns.Add("uri", typeof(string));
                datat.Columns.Add("query", typeof(string));
                datat.Columns.Add("ip", typeof(string));
                datat.Columns.Add("status", typeof(string));
                datat.Columns.Add("useragent", typeof(string));

                foreach (string filename in filenames)
                {
                    string query = @"SELECT date,time,cs-method,cs-uri-stem,cs-uri-query,c-ip,sc-status,cs(User-Agent)  from '" + filename + "'";// GROUP BY c-ip
                    LogRecordSet oRecordSet = oLogQuery.Execute(query, oIISInputFormat);
                    while (!oRecordSet.atEnd())
                    {
                        var itemData = oRecordSet.getRecord();
                        DataRow dr = datat.NewRow();
                        dr["date"] = itemData.getValue("date").ToString("yyyy-MM-dd");
                        dr["time"] = itemData.getValue("time").ToString("hh:mm:ss");
                        dr["method"] = itemData.getValue("cs-method").ToString();
                        dr["uri"] = itemData.getValue("cs-uri-stem").ToString();
                        dr["query"] = itemData.getValue("cs-uri-query").ToString();
                        dr["ip"] = itemData.getValue("c-ip").ToString();
                        dr["status"] = itemData.getValue("sc-status").ToString();
                        dr["useragent"] = itemData.getValue("cs(User-Agent)").ToString();
                        datat.Rows.Add(dr);
                        oRecordSet.moveNext();
                    }
                    oRecordSet.close();
                }
                dgQueryResult.Columns[0].Header = (object)("日期");
                dgQueryResult.Columns[1].Header = (object)("时间");
                dgQueryResult.Columns[2].Header = (object)("方法");
                dgQueryResult.Columns[3].Header = (object)("相对路径");
                dgQueryResult.Columns[4].Header = (object)("查询");
                dgQueryResult.Columns[5].Header = (object)("IP");
                dgQueryResult.Columns[6].Header = (object)("状态");
                dgQueryResult.Columns[7].Header = (object)("用户代理");
                dgQueryResult.DataContext = datat.DefaultView;
                #endregion
            }
            else
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("该功能需要安装LogParser.", "警告", MessageBoxButton.OK, MessageBoxImage.Exclamation);

                System.Diagnostics.ProcessStartInfo Info = new System.Diagnostics.ProcessStartInfo();

                //设置外部程序名  
                Info.FileName = "LogParser.msi";

                //设置外部程序工作目录为   C:\  
                //Info.WorkingDirectory = @"D:\常用软件\eclipse";

                //最小化方式启动
                //Info.WindowStyle = System.Diagnostics.ProcessWindowStyle.Minimized;

                //声明一个程序类  
                System.Diagnostics.Process Proc;

                try
                {
                    Proc = System.Diagnostics.Process.Start(Info);
                    System.Threading.Thread.Sleep(500);
                }
                catch (System.ComponentModel.Win32Exception)
                {
                    return;
                }
            }

        }

        private void btSystem(object sender, RoutedEventArgs e)
        {
            string filename = "";
            Microsoft.Win32.OpenFileDialog dialogOpenFile = new Microsoft.Win32.OpenFileDialog();
            dialogOpenFile.Filter = "系统日志文件（*.evt）|*.evt";
            if (dialogOpenFile.ShowDialog() == true)
            {
                filename = dialogOpenFile.FileName;
            }
            else
            {
                return;
            }


            string sql = @"SELECT EventID, TimeGenerated, SourceName, Message FROM '" +filename + "'";//此为系统日志文件路径  
            DataTable dt = readFromEvt(sql);
            dgQueryResult.ItemsSource = dt.DefaultView;
            MessageBox.Show("读取完毕！");  
        }

        public DataTable readFromEvt(string sql)
        {
            try
            {
                DataTable datat = new DataTable();
                datat.Columns.Add("事件ID", typeof(string));
                datat.Columns.Add("日期", typeof(string));
                datat.Columns.Add("来源", typeof(string));
                datat.Columns.Add("描述", typeof(string));
                // Instantiate the LogQuery object    
                LogQuery oLogQuery = new LogQuery();
                // Instantiate the Event Log Input Format object    
                EvtInputFormat oEvtInputFormat = new EvtInputFormat();
                // Set its "direction" parameter to "BW"    
                //oEvtInputFormat.direction = "BW";
                // Create the query    
                string query = sql;
                // Execute the query    
                LogRecordSet oRecordSet = oLogQuery.Execute(query, oEvtInputFormat);
                while (!oRecordSet.atEnd())
                {
                    var itemData = oRecordSet.getRecord();
                    DataRow dr = datat.NewRow();
                    dr["事件ID"] = itemData.getValue("EventID").ToString();
                    dr["日期"] = itemData.getValue("TimeGenerated").ToString();
                    dr["来源"] = itemData.getValue("SourceName").ToString();
                    dr["描述"] = itemData.getValue("Message").ToString();
                    datat.Rows.Add(dr);
                    oRecordSet.moveNext();
                }

                // Close the recordset    
                oRecordSet.close();
                return datat;
            }
            catch (System.Runtime.InteropServices.COMException exc)
            {
                MessageBox.Show("Unexpected error: " + exc.Message);
                return null;
            } 
        }
    }
}
