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

using FillSkynet.WordStock;
using System.Collections;

namespace WpfQuery
{
    /// <summary>
    /// SentenceSplit.xaml 的交互逻辑
    /// </summary>
    public partial class SentenceSplit : UserControl
    {
        private SentenceSplitter SS;

        public SentenceSplit()
        {
            InitializeComponent();

            SS = new SentenceSplitter();//默认的构造函数， 是才采用内置的词库,如果是采用外部的词库，在构造函数的参数里可以指定

        }

        private void btQuery(object sender, RoutedEventArgs e)
        {
            StringBuilder Sb;
            long Time;

            Sb = new StringBuilder("");

            Time = DateTime.Now.Ticks;

            //label1.Text = "开始时间：" + DateTime.Now.ToString();
            if (cbSingleChar.IsChecked == true)
            {
                SS.IgnoreSingleWord = true;
            }
            ArrayList Al = SS.SplitSentence(this.txtContent.Text);
            //label2.Text = "耗时：" + ((DateTime.Now.Ticks - Time) / 10000).ToString() + "毫秒";
            //label5.Text = "结束时间：" + DateTime.Now.ToString();
            if (Al != null)
                foreach (object obj in Al)
                {
                    Sb.Append(obj.ToString() + " ");
                }
            this.txtSSResult.Text = Sb.ToString();

        }

        private void txtContent_TextChanged(object sender, TextChangedEventArgs e)
        {
            tbLength.Text = "原始字符串长度：" + txtContent.Text.Length.ToString();
            this.buttonQuery.IsEnabled = txtContent.Text.Length > 0;
        }
    }
}
