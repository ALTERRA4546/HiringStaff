﻿#pragma checksum "..\..\MultiReport.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "5EE6A2E080A3322D57054D49FBAD30CAE9AAEADE4741EA69EEC64B68A121B7DE"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using HiringStaff;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
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


namespace HiringStaff {
    
    
    /// <summary>
    /// MultiReport
    /// </summary>
    public partial class MultiReport : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 18 "..\..\MultiReport.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label ReportName;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\MultiReport.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.WrapPanel YearPanel;
        
        #line default
        #line hidden
        
        
        #line 21 "..\..\MultiReport.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox ChekedSelectedYear;
        
        #line default
        #line hidden
        
        
        #line 23 "..\..\MultiReport.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox SelectedYear;
        
        #line default
        #line hidden
        
        
        #line 26 "..\..\MultiReport.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid ReportData;
        
        #line default
        #line hidden
        
        
        #line 28 "..\..\MultiReport.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Export;
        
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
            System.Uri resourceLocater = new System.Uri("/HiringStaff;component/multireport.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\MultiReport.xaml"
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
            
            #line 8 "..\..\MultiReport.xaml"
            ((HiringStaff.MultiReport)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Window_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.ReportName = ((System.Windows.Controls.Label)(target));
            return;
            case 3:
            this.YearPanel = ((System.Windows.Controls.WrapPanel)(target));
            return;
            case 4:
            this.ChekedSelectedYear = ((System.Windows.Controls.CheckBox)(target));
            
            #line 21 "..\..\MultiReport.xaml"
            this.ChekedSelectedYear.Click += new System.Windows.RoutedEventHandler(this.ChekedSelectedYear_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.SelectedYear = ((System.Windows.Controls.ComboBox)(target));
            
            #line 23 "..\..\MultiReport.xaml"
            this.SelectedYear.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.SelectedYear_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 6:
            this.ReportData = ((System.Windows.Controls.DataGrid)(target));
            return;
            case 7:
            this.Export = ((System.Windows.Controls.Button)(target));
            
            #line 28 "..\..\MultiReport.xaml"
            this.Export.Click += new System.Windows.RoutedEventHandler(this.Export_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

