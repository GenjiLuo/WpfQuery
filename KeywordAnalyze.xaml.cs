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

using WPF.MDI;

namespace WpfQuery
{
    /// <summary>
    /// KeywordAnalyze.xaml 的交互逻辑
    /// </summary>
    public partial class KeywordAnalyze : UserControl
    {
        public KeywordAnalyze()
        {
            InitializeComponent();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Container.Children.Add(new MdiChild
            //{
            //    Title = "Empty Window Using Code",
            //    Width = 100,
            //    Height = 100,
            //});

            //Container.Children.Add(new MdiChild
            //{
            //    Title = "Window Using Code",
            //    Content = new BaiduIndex(),
            //    Width = 714,
            //    Height = 734,
            //    Position = new Point(200, 30)
            //});

        }
    }
}
