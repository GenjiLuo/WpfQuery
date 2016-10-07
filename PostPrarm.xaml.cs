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

namespace WpfQuery
{
    /// <summary>
    /// PostPrarm.xaml 的交互逻辑
    /// </summary>
    public partial class PostPrarm : Window
    {
        public PostPrarm()
        {
            InitializeComponent();
        }

        private void btnGo_Click(object sender, RoutedEventArgs e)
        {
            wbPostPrarm.Navigate(txtUrl.Text);
        }

        private void btnGet_Click(object sender, RoutedEventArgs e)
        {
            //string postDataText = System.Text.Encoding.ASCII.GetString(PostData as byte[]);

        }
    }
}
