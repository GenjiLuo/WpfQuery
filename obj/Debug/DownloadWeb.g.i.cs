﻿#pragma checksum "..\..\DownloadWeb.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "E1FF2D9A64F78680FD89CCD05804CDC6"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace WpfQuery {
    
    
    /// <summary>
    /// DownloadWeb
    /// </summary>
    public partial class DownloadWeb : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 23 "..\..\DownloadWeb.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox WebUrl;
        
        #line default
        #line hidden
        
        
        #line 24 "..\..\DownloadWeb.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Paste;
        
        #line default
        #line hidden
        
        
        #line 33 "..\..\DownloadWeb.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton rbUft8;
        
        #line default
        #line hidden
        
        
        #line 34 "..\..\DownloadWeb.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton rbGb2312;
        
        #line default
        #line hidden
        
        
        #line 39 "..\..\DownloadWeb.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton rbPathMode;
        
        #line default
        #line hidden
        
        
        #line 40 "..\..\DownloadWeb.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton rbTypeMode;
        
        #line default
        #line hidden
        
        
        #line 51 "..\..\DownloadWeb.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox Filepath;
        
        #line default
        #line hidden
        
        
        #line 52 "..\..\DownloadWeb.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Folder;
        
        #line default
        #line hidden
        
        
        #line 60 "..\..\DownloadWeb.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox chkUrlLog;
        
        #line default
        #line hidden
        
        
        #line 61 "..\..\DownloadWeb.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Download;
        
        #line default
        #line hidden
        
        
        #line 62 "..\..\DownloadWeb.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button OpenFolder;
        
        #line default
        #line hidden
        
        
        #line 65 "..\..\DownloadWeb.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtLog;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/WpfQuery;component/downloadweb.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\DownloadWeb.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.WebUrl = ((System.Windows.Controls.TextBox)(target));
            return;
            case 2:
            this.Paste = ((System.Windows.Controls.Button)(target));
            
            #line 24 "..\..\DownloadWeb.xaml"
            this.Paste.Click += new System.Windows.RoutedEventHandler(this.Paste_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.rbUft8 = ((System.Windows.Controls.RadioButton)(target));
            return;
            case 4:
            this.rbGb2312 = ((System.Windows.Controls.RadioButton)(target));
            return;
            case 5:
            this.rbPathMode = ((System.Windows.Controls.RadioButton)(target));
            return;
            case 6:
            this.rbTypeMode = ((System.Windows.Controls.RadioButton)(target));
            return;
            case 7:
            this.Filepath = ((System.Windows.Controls.TextBox)(target));
            return;
            case 8:
            this.Folder = ((System.Windows.Controls.Button)(target));
            
            #line 52 "..\..\DownloadWeb.xaml"
            this.Folder.Click += new System.Windows.RoutedEventHandler(this.Folder_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            this.chkUrlLog = ((System.Windows.Controls.CheckBox)(target));
            return;
            case 10:
            this.Download = ((System.Windows.Controls.Button)(target));
            
            #line 61 "..\..\DownloadWeb.xaml"
            this.Download.Click += new System.Windows.RoutedEventHandler(this.Download_Click);
            
            #line default
            #line hidden
            return;
            case 11:
            this.OpenFolder = ((System.Windows.Controls.Button)(target));
            
            #line 62 "..\..\DownloadWeb.xaml"
            this.OpenFolder.Click += new System.Windows.RoutedEventHandler(this.OpenFolder_Click);
            
            #line default
            #line hidden
            return;
            case 12:
            this.txtLog = ((System.Windows.Controls.TextBox)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

