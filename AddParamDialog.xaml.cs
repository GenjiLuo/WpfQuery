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
    /// AddParamDialog.xaml 的交互逻辑
    /// </summary>
    public partial class AddParamDialog : Window
    {
        public string key;
        public string value;


        public AddParamDialog()
        {
            InitializeComponent();

        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            key = txtKey.Text;
            value = txtValue.Text;
        }
    }
}
