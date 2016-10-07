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
using Fluent;
using Xceed.Wpf.AvalonDock.Layout;
using System.IO;
using Tomers.WPF.Themes.Skins;
using System.Reflection;

namespace WpfQuery
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : RibbonWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(MainWindow_Loaded);

            string readmefilepath = @"readme.html";
            if (File.Exists(readmefilepath))
            {
                string path=System.Environment.CurrentDirectory.Replace(@"\", "/") + "/images/";
                string readme = File.ReadAllText(readmefilepath, System.Text.Encoding.Default).Replace("images/", path);
                defaultBrowser.NavigateToString(readme);
            }

            //设定初始选项卡
            ribbon.SelectedTabIndex = 1;
        }

        /// <summary>
        /// 收录查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IncludeQuery(object sender, RoutedEventArgs e)
        {
            Fluent.Button button = (Fluent.Button)sender;
            string head = button.Header.ToString();

            var firstDocumentPane = dockManager.Layout.Descendents().OfType<LayoutDocumentPane>().FirstOrDefault();
            if (firstDocumentPane != null)
            {
                LayoutDocument doc2 = new LayoutDocument();
                IncludeQuery control1 = new IncludeQuery();
                doc2.Title = head;
                doc2.Content = control1;
                doc2.IsActive = true;
                firstDocumentPane.Children.Add(doc2);
            }
        }


        private void RankQuery(object sender, RoutedEventArgs e)
        {
            Fluent.Button button = (Fluent.Button)sender;
            string head = button.Header.ToString();

            var firstDocumentPane = dockManager.Layout.Descendents().OfType<LayoutDocumentPane>().FirstOrDefault();
            if (firstDocumentPane != null)
            {
                LayoutDocument doc2 = new LayoutDocument();
                RankQuery control1 = new RankQuery();
                doc2.Title = head;
                doc2.Content = control1;
                doc2.IsActive = true;
                firstDocumentPane.Children.Add(doc2);
            }
        }

        private void TitleQuery(object sender, RoutedEventArgs e)
        {
            Fluent.Button button = (Fluent.Button)sender;
            string head = button.Header.ToString();

            var firstDocumentPane = dockManager.Layout.Descendents().OfType<LayoutDocumentPane>().FirstOrDefault();
            if (firstDocumentPane != null)
            {
                LayoutDocument doc2 = new LayoutDocument();
                TitleQuery control1 = new TitleQuery();
                doc2.Title = head;
                doc2.Content = control1;
                doc2.IsActive = true;
                firstDocumentPane.Children.Add(doc2);
            }
        }

        private void RankStat(object sender, RoutedEventArgs e)
        {
            Fluent.Button button = (Fluent.Button)sender;
            string head = button.Header.ToString();

            var firstDocumentPane = dockManager.Layout.Descendents().OfType<LayoutDocumentPane>().FirstOrDefault();
            if (firstDocumentPane != null)
            {
                LayoutDocument doc2 = new LayoutDocument();
                RankStat control1 = new RankStat();
                doc2.Title = head;
                doc2.Content = control1;
                doc2.IsActive = true;
                firstDocumentPane.Children.Add(doc2);
            }
        }

        private void PlatformQuery(object sender, RoutedEventArgs e)
        {
            Fluent.Button button = (Fluent.Button)sender;
            string head = button.Header.ToString();

            var firstDocumentPane = dockManager.Layout.Descendents().OfType<LayoutDocumentPane>().FirstOrDefault();
            if (firstDocumentPane != null)
            {
                LayoutDocument doc2 = new LayoutDocument();
                PlatformQuery control1 = new PlatformQuery();
                doc2.Title = head;
                doc2.Content = control1;
                doc2.IsActive = true;
                firstDocumentPane.Children.Add(doc2);
            }
        }

        private void ConditionQuery(object sender, RoutedEventArgs e)
        {
            Fluent.Button button = (Fluent.Button)sender;
            string head = button.Header.ToString();

            var firstDocumentPane = dockManager.Layout.Descendents().OfType<LayoutDocumentPane>().FirstOrDefault();
            if (firstDocumentPane != null)
            {
                LayoutDocument doc2 = new LayoutDocument();
                ConditionQuery control1 = new ConditionQuery();
                doc2.Title = head;
                doc2.Content = control1;
                doc2.IsActive = true;
                firstDocumentPane.Children.Add(doc2);
            }
        }

        private void KeyDig(object sender, RoutedEventArgs e)
        {
            Fluent.Button button = (Fluent.Button)sender;
            string head = button.Header.ToString();

            var firstDocumentPane = dockManager.Layout.Descendents().OfType<LayoutDocumentPane>().FirstOrDefault();
            if (firstDocumentPane != null)
            {
                LayoutDocument doc2 = new LayoutDocument();
                KeyDig control1 = new KeyDig();
                doc2.Title = head;
                doc2.Content = control1;
                doc2.IsActive = true;
                firstDocumentPane.Children.Add(doc2);
            }
        }

        private void KeyCover(object sender, RoutedEventArgs e)
        {
            Fluent.Button button = (Fluent.Button)sender;
            string head = button.Header.ToString();

            var firstDocumentPane = dockManager.Layout.Descendents().OfType<LayoutDocumentPane>().FirstOrDefault();
            if (firstDocumentPane != null)
            {
                LayoutDocument doc2 = new LayoutDocument();
                KeyCover control1 = new KeyCover();
                doc2.Title = head;
                doc2.Content = control1;
                doc2.IsActive = true;
                firstDocumentPane.Children.Add(doc2);
            }
        }

        private void BasicInfo(object sender, RoutedEventArgs e)
        {
            Fluent.Button button = (Fluent.Button)sender;
            string head = button.Header.ToString();

            var firstDocumentPane = dockManager.Layout.Descendents().OfType<LayoutDocumentPane>().FirstOrDefault();
            if (firstDocumentPane != null)
            {
                LayoutDocument doc2 = new LayoutDocument();
                BasicInfo control1 = new BasicInfo();
                doc2.Title = head;
                doc2.Content = control1;
                doc2.IsActive = true;
                firstDocumentPane.Children.Add(doc2);
            }
        }

        private void LogParser(object sender, RoutedEventArgs e)
        {
            Fluent.Button button = (Fluent.Button)sender;
            string head = button.Header.ToString();

            var firstDocumentPane = dockManager.Layout.Descendents().OfType<LayoutDocumentPane>().FirstOrDefault();
            if (firstDocumentPane != null)
            {
                LayoutDocument doc2 = new LayoutDocument();
                LogParser control1 = new LogParser();
                doc2.Title = head;
                doc2.Content = control1;
                doc2.IsActive = true;
                firstDocumentPane.Children.Add(doc2);
            }
        }

        private void SentenceSplit(object sender, RoutedEventArgs e)
        {
            Fluent.Button button = (Fluent.Button)sender;
            string head = button.Header.ToString();

            var firstDocumentPane = dockManager.Layout.Descendents().OfType<LayoutDocumentPane>().FirstOrDefault();
            if (firstDocumentPane != null)
            {
                LayoutDocument doc2 = new LayoutDocument();
                SentenceSplit control1 = new SentenceSplit();
                doc2.Title = head;
                doc2.Content = control1;
                doc2.IsActive = true;
                firstDocumentPane.Children.Add(doc2);
            }
        }

        private void Post(object sender, RoutedEventArgs e)
        {
            Fluent.Button button = (Fluent.Button)sender;
            string head = button.Header.ToString();

            var firstDocumentPane = dockManager.Layout.Descendents().OfType<LayoutDocumentPane>().FirstOrDefault();
            if (firstDocumentPane != null)
            {
                LayoutDocument doc2 = new LayoutDocument();
                Post control1 = new Post();
                doc2.Title = head;
                doc2.Content = control1;
                doc2.IsActive = true;
                firstDocumentPane.Children.Add(doc2);
            }
        }

        private void BaiduIndex(object sender, RoutedEventArgs e)
        {
            Fluent.Button button = (Fluent.Button)sender;
            string head = button.Header.ToString();

            var firstDocumentPane = dockManager.Layout.Descendents().OfType<LayoutDocumentPane>().FirstOrDefault();
            if (firstDocumentPane != null)
            {
                LayoutDocument doc2 = new LayoutDocument();
                BaiduIndex control1 = new BaiduIndex();
                doc2.Title = head;
                doc2.Content = control1;
                doc2.IsActive = true;
                firstDocumentPane.Children.Add(doc2);
            }
        }

        private void Login(object sender, RoutedEventArgs e)
        {
            new Login().ShowDialog();
        }

        private void About(object sender, RoutedEventArgs e)
        {
            new About().ShowDialog();
        }

        private void BaiduAccount(object sender, RoutedEventArgs e)
        {
            new BaiduAccount().ShowDialog();
        }

        #region 通知栏图标
        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            InitialTray();
        }

        private System.Windows.Forms.NotifyIcon notifyIcon = null;

        private void InitialTray()
        {
            //设置托盘的各个属性
            notifyIcon = new System.Windows.Forms.NotifyIcon();
            notifyIcon.BalloonTipText = @"程序已经启动!";
            notifyIcon.Text = @"托盘图标";
            notifyIcon.Icon = WpfQuery.Properties.Resources.notifyIcon;// new System.Drawing.Icon("Fluent.ico");//new Icon(System.Windows.Forms.Application.StartupPath + "/Images/usergroup.ico");
            notifyIcon.Visible = true;
            notifyIcon.ShowBalloonTip(2000);
            notifyIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(notifyIcon_MouseClick);

            //设置菜单项
            System.Windows.Forms.MenuItem showMenu = new System.Windows.Forms.MenuItem("显示");
            showMenu.Click += new EventHandler(showMenu_Click);
            System.Windows.Forms.MenuItem hideMenu = new System.Windows.Forms.MenuItem("隐藏");
            hideMenu.Click += new EventHandler(hideMenu_Click);
            //退出菜单项
            System.Windows.Forms.MenuItem exit = new System.Windows.Forms.MenuItem("退出系统");
            exit.Click += new EventHandler(OnExitSystem);
            //关联托盘控件
            System.Windows.Forms.MenuItem[] childen = new System.Windows.Forms.MenuItem[] { showMenu, hideMenu, exit };
            notifyIcon.ContextMenu = new System.Windows.Forms.ContextMenu(childen);

            //窗体状态改变时候触发
            this.StateChanged += new EventHandler(SysTray_StateChanged);
        }

        void hideMenu_Click(object sender, EventArgs e)
        {
            this.Visibility = Visibility.Hidden;
            notifyIcon.BalloonTipText = @"系统已隐藏到托盘!";
            notifyIcon.ShowBalloonTip(2000);
        }

        void showMenu_Click(object sender, EventArgs e)
        {
            this.Visibility = Visibility.Visible;
            this.Activate();
        }

        ///
        /// 窗体状态改变时候触发
        ///
        ///
        ///
        private void SysTray_StateChanged(object sender, EventArgs e)
        {
            //注释：任务栏隐藏
            //if (this.WindowState == WindowState.Minimized)
            //{
            //    this.Visibility = Visibility.Hidden;
            //    notifyIcon.BalloonTipText = @"系统已隐藏到托盘!";
            //    notifyIcon.ShowBalloonTip(2000);
            //}
        }

        ///
        /// 退出选项
        ///
        ///
        ///
        private void OnExitSystem(object sender, EventArgs e)
        {
            if (System.Windows.MessageBox.Show("确定要退出系统吗?",
                                               "退出",
                                                MessageBoxButton.YesNo,
                                                MessageBoxImage.Question,
                                                MessageBoxResult.No) == MessageBoxResult.Yes)
            {
                notifyIcon.Dispose();
                System.Windows.Application.Current.Shutdown();
            }
        }

        ///
        /// 鼠标单击
        ///
        ///
        ///
        private void notifyIcon_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (this.Visibility == Visibility.Visible)
                {
                    this.Visibility = Visibility.Hidden;
                }
                else
                {
                    this.Visibility = Visibility.Visible;
                    this.Activate();
                }
            }
        }

        private void window_Closed(object sender, EventArgs e)
        {
            if (notifyIcon.Visible == true)
            {
                notifyIcon.Visible = false;
            }
        }

        #endregion

        private void Setting(object sender, RoutedEventArgs e)
        {
            new Options().ShowDialog();
        }

        private void KeywordAnalyze(object sender, RoutedEventArgs e)
        {
            Fluent.Button button = (Fluent.Button)sender;
            string head = button.Header.ToString();

            var firstDocumentPane = dockManager.Layout.Descendents().OfType<LayoutDocumentPane>().FirstOrDefault();
            if (firstDocumentPane != null)
            {
                LayoutDocument doc2 = new LayoutDocument();
                KeywordAnalyze control1 = new KeywordAnalyze();
                doc2.Title = head;
                doc2.Content = control1;
                doc2.IsActive = true;
                firstDocumentPane.Children.Add(doc2);
            }
        }

        private void Gather(object sender, RoutedEventArgs e)
        {
            Fluent.Button button = (Fluent.Button)sender;
            string head = button.Header.ToString();

            var firstDocumentPane = dockManager.Layout.Descendents().OfType<LayoutDocumentPane>().FirstOrDefault();
            if (firstDocumentPane != null)
            {
                LayoutDocument doc2 = new LayoutDocument();
                Gather control1 = new Gather();
                doc2.Title = head;
                doc2.Content = control1;
                doc2.IsActive = true;
                firstDocumentPane.Children.Add(doc2);
            }
        }

        private void BDKeywordRecommend(object sender, RoutedEventArgs e)
        {
            Fluent.Button button = (Fluent.Button)sender;
            string head = button.Header.ToString();

            var firstDocumentPane = dockManager.Layout.Descendents().OfType<LayoutDocumentPane>().FirstOrDefault();
            if (firstDocumentPane != null)
            {
                LayoutDocument doc2 = new LayoutDocument();
                BDKeywordRecommend control1 = new BDKeywordRecommend();
                doc2.Title = head;
                doc2.Content = control1;
                doc2.IsActive = true;
                firstDocumentPane.Children.Add(doc2);
            }
        }

        private void BDWMInterest(object sender, RoutedEventArgs e)
        {
            Fluent.Button button = (Fluent.Button)sender;
            string head = button.Header.ToString();

            var firstDocumentPane = dockManager.Layout.Descendents().OfType<LayoutDocumentPane>().FirstOrDefault();
            if (firstDocumentPane != null)
            {
                LayoutDocument doc2 = new LayoutDocument();
                BDWMInterest control1 = new BDWMInterest();
                doc2.Title = head;
                doc2.Content = control1;
                doc2.IsActive = true;
                firstDocumentPane.Children.Add(doc2);
            }
        }

        private void SogouAccount(object sender, RoutedEventArgs e)
        {
            new SogouAccount().ShowDialog();
        }

        private void SogouKRService(object sender, RoutedEventArgs e)
        {
            Fluent.Button button = (Fluent.Button)sender;
            string head = button.Header.ToString();

            var firstDocumentPane = dockManager.Layout.Descendents().OfType<LayoutDocumentPane>().FirstOrDefault();
            if (firstDocumentPane != null)
            {
                LayoutDocument doc2 = new LayoutDocument();
                SogouKRService control1 = new SogouKRService();
                doc2.Title = head;
                doc2.Content = control1;
                doc2.IsActive = true;
                firstDocumentPane.Children.Add(doc2);
            }
        }

        private void CreativeCollection(object sender, RoutedEventArgs e)
        {
            Fluent.Button button = (Fluent.Button)sender;
            string head = button.Header.ToString();

            var firstDocumentPane = dockManager.Layout.Descendents().OfType<LayoutDocumentPane>().FirstOrDefault();
            if (firstDocumentPane != null)
            {
                LayoutDocument doc2 = new LayoutDocument();
                CreativeCollection control1 = new CreativeCollection();
                doc2.Title = head;
                doc2.Content = control1;
                doc2.IsActive = true;
                firstDocumentPane.Children.Add(doc2);
            }
        }

        private void Downword(object sender, RoutedEventArgs e)
        {
            Fluent.Button button = (Fluent.Button)sender;
            string head = button.Header.ToString();

            var firstDocumentPane = dockManager.Layout.Descendents().OfType<LayoutDocumentPane>().FirstOrDefault();
            if (firstDocumentPane != null)
            {
                LayoutDocument doc2 = new LayoutDocument();
                Downword control1 = new Downword();
                doc2.Title = head;
                doc2.Content = control1;
                doc2.IsActive = true;
                firstDocumentPane.Children.Add(doc2);
            }
        }

        #region 换皮肤
        private readonly List<Skin> _skins = new List<Skin>();
        private void SkinClick(object sender, RoutedEventArgs e)
        {

        }

        public Skin CurrentSkin
        {
            get { return (Skin)GetValue(CurrentSkinProperty); }
            set { SetValue(CurrentSkinProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentSkin.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentSkinProperty =
            DependencyProperty.Register(
                "CurrentSkin",
                typeof(Skin),
                typeof(MainWindow),
                new UIPropertyMetadata(Skin.Null, OnCurrentSkinChanged, OnCoerceSkinValue));

        private static void OnCurrentSkinChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                Skin oldSkin = e.OldValue as Skin;
                oldSkin.Unload();
                Skin newSkin = e.NewValue as Skin;

                int ticks = Environment.TickCount;

                newSkin.Load();

                ticks = Environment.TickCount - ticks;
                MessageBox.Show(string.Format("Time to load {0} skin of type {1}: {2}ms",
                    newSkin.Name, newSkin.GetType().Name, ticks));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Change error: " + ex.Message);
                throw;
            }
        }

        private static object OnCoerceSkinValue(DependencyObject d, object baseValue)
        {
            try
            {
                if (baseValue == null)
                {
                    return Skin.Null;
                }
                return baseValue;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Coerce error: " + ex.Message);
                throw;
            }
        }
        #endregion

        private void Browser(object sender, RoutedEventArgs e)
        {
            window.WindowState = WindowState.Minimized;
            SimulationBrowser sb = new SimulationBrowser();
            sb.ShowDialog();
        }

        private void IISConfig(object sender, RoutedEventArgs e)
        {
            Fluent.Button button = (Fluent.Button)sender;
            string head = button.Header.ToString();

            var firstDocumentPane = dockManager.Layout.Descendents().OfType<LayoutDocumentPane>().FirstOrDefault();
            if (firstDocumentPane != null)
            {
                LayoutDocument doc2 = new LayoutDocument();
                IISConfig control1 = new IISConfig();
                doc2.Title = head;
                doc2.Content = control1;
                doc2.IsActive = true;
                firstDocumentPane.Children.Add(doc2);
            }
        }

        private void Ping(object sender, RoutedEventArgs e)
        {
            Fluent.Button button = (Fluent.Button)sender;
            string head = button.Header.ToString();

            var firstDocumentPane = dockManager.Layout.Descendents().OfType<LayoutDocumentPane>().FirstOrDefault();
            if (firstDocumentPane != null)
            {
                LayoutDocument doc2 = new LayoutDocument();
                Ping control1 = new Ping();
                doc2.Title = head;
                doc2.Content = control1;
                doc2.IsActive = true;
                firstDocumentPane.Children.Add(doc2);
            }
        }

        private void RelatedSearch(object sender, RoutedEventArgs e)
        {
            Fluent.Button button = (Fluent.Button)sender;
            string head = button.Header.ToString();

            var firstDocumentPane = dockManager.Layout.Descendents().OfType<LayoutDocumentPane>().FirstOrDefault();
            if (firstDocumentPane != null)
            {
                LayoutDocument doc2 = new LayoutDocument();
                RelatedSearch control1 = new RelatedSearch();
                doc2.Title = head;
                doc2.Content = control1;
                doc2.IsActive = true;
                firstDocumentPane.Children.Add(doc2);
            }
        }

        private void DownloadWeb(object sender, RoutedEventArgs e)
        {
            Fluent.Button button = (Fluent.Button)sender;
            string head = button.Header.ToString();

            var firstDocumentPane = dockManager.Layout.Descendents().OfType<LayoutDocumentPane>().FirstOrDefault();
            if (firstDocumentPane != null)
            {
                LayoutDocument doc2 = new LayoutDocument();
                DownloadWeb control1 = new DownloadWeb();
                doc2.Title = head;
                doc2.Content = control1;
                doc2.IsActive = true;
                firstDocumentPane.Children.Add(doc2);
            }
        }

    }


}


