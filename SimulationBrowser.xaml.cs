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
using System.Reflection;
using System.Threading;
namespace WpfQuery
{
    /// <summary>
    /// SimulationBrowser.xaml 的交互逻辑
    /// </summary>
    public partial class SimulationBrowser : Window
    {
        public SimulationBrowser()
        {
            InitializeComponent();
            //string url = "http://www.baidu.com/#wd=%E5%8C%97%E4%BA%AC%E6%B5%B7%E5%8D%8E%E5%8C%BB%E9%99%A2%E4%BD%8D%E7%BD%AE&tn=baidu&ie=utf-8&f=3&rsv_bp=1&rsv_sug3=1&rsv_sug4=48&rsv_sug1=1&oq=%E5%8C%97%E4%BA%AC%E6%B5%B7%E5%8D%8E%E5%8C%BB%E9%99%A2&rsv_sug2=1&rsp=2&rsv_sug=1&bs=%E5%8C%97%E4%BA%AC%E6%B5%B7%E5%8D%8E%E5%8C%BB%E9%99%A2&rsv_spt=3";
            //wbSimulation.Navigate(url);
        }

        /*
        public void SuppressScriptErrors(WebBrowser wb, bool Hide)
        {
            FieldInfo fiComWebBrowser = typeof(WebBrowser).GetField("_axIWebBrowser2", BindingFlags.Instance | BindingFlags.NonPublic);
            if (fiComWebBrowser == null) return;

            object objComWebBrowser = fiComWebBrowser.GetValue(wb);
            if (objComWebBrowser == null) return;

            objComWebBrowser.GetType().InvokeMember("Silent", BindingFlags.SetProperty, null, objComWebBrowser, new object[] { Hide });
        }

        void wbSimulation_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            SuppressScriptErrors(wbSimulation, true);
        }

        private void wbSimulation_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            mshtml.IHTMLDocument2 doc2 = (mshtml.IHTMLDocument2)wbSimulation.Document;
        }
        */

    }
        
    
}
