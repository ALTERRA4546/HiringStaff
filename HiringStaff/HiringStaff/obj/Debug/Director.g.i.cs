﻿#pragma checksum "..\..\Director.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "852485A02DB16C574E389EC76977D617C1868A95F43231250C6BA605C445AC9F"
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
    /// Director
    /// </summary>
    public partial class Director : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 15 "..\..\Director.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem ChangeUser;
        
        #line default
        #line hidden
        
        
        #line 16 "..\..\Director.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem Exit;
        
        #line default
        #line hidden
        
        
        #line 19 "..\..\Director.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem Export;
        
        #line default
        #line hidden
        
        
        #line 21 "..\..\Director.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem WorkSchedule;
        
        #line default
        #line hidden
        
        
        #line 23 "..\..\Director.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem SalaryReport;
        
        #line default
        #line hidden
        
        
        #line 24 "..\..\Director.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem ReportOnHoursWorked;
        
        #line default
        #line hidden
        
        
        #line 29 "..\..\Director.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label UserName;
        
        #line default
        #line hidden
        
        
        #line 36 "..\..\Director.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox Search;
        
        #line default
        #line hidden
        
        
        #line 38 "..\..\Director.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox ChoiceOfPosition;
        
        #line default
        #line hidden
        
        
        #line 40 "..\..\Director.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid EmployeeData;
        
        #line default
        #line hidden
        
        
        #line 42 "..\..\Director.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button AddEmployees;
        
        #line default
        #line hidden
        
        
        #line 43 "..\..\Director.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button RemoveEmployees;
        
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
            System.Uri resourceLocater = new System.Uri("/HiringStaff;component/director.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\Director.xaml"
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
            
            #line 8 "..\..\Director.xaml"
            ((HiringStaff.Director)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Window_Loaded);
            
            #line default
            #line hidden
            
            #line 8 "..\..\Director.xaml"
            ((HiringStaff.Director)(target)).Closing += new System.ComponentModel.CancelEventHandler(this.Window_Closing);
            
            #line default
            #line hidden
            return;
            case 2:
            this.ChangeUser = ((System.Windows.Controls.MenuItem)(target));
            
            #line 15 "..\..\Director.xaml"
            this.ChangeUser.Click += new System.Windows.RoutedEventHandler(this.ChangeUser_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.Exit = ((System.Windows.Controls.MenuItem)(target));
            
            #line 16 "..\..\Director.xaml"
            this.Exit.Click += new System.Windows.RoutedEventHandler(this.Exit_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.Export = ((System.Windows.Controls.MenuItem)(target));
            
            #line 19 "..\..\Director.xaml"
            this.Export.Click += new System.Windows.RoutedEventHandler(this.Export_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.WorkSchedule = ((System.Windows.Controls.MenuItem)(target));
            
            #line 21 "..\..\Director.xaml"
            this.WorkSchedule.Click += new System.Windows.RoutedEventHandler(this.WorkSchedule_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.SalaryReport = ((System.Windows.Controls.MenuItem)(target));
            
            #line 23 "..\..\Director.xaml"
            this.SalaryReport.Click += new System.Windows.RoutedEventHandler(this.SalaryReport_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.ReportOnHoursWorked = ((System.Windows.Controls.MenuItem)(target));
            
            #line 24 "..\..\Director.xaml"
            this.ReportOnHoursWorked.Click += new System.Windows.RoutedEventHandler(this.ReportOnHoursWorked_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            this.UserName = ((System.Windows.Controls.Label)(target));
            return;
            case 9:
            this.Search = ((System.Windows.Controls.TextBox)(target));
            
            #line 36 "..\..\Director.xaml"
            this.Search.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.Search_TextChanged);
            
            #line default
            #line hidden
            return;
            case 10:
            this.ChoiceOfPosition = ((System.Windows.Controls.ComboBox)(target));
            
            #line 38 "..\..\Director.xaml"
            this.ChoiceOfPosition.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.ChoiceOfPosition_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 11:
            this.EmployeeData = ((System.Windows.Controls.DataGrid)(target));
            
            #line 40 "..\..\Director.xaml"
            this.EmployeeData.MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(this.EmployeeData_MouseDoubleClick);
            
            #line default
            #line hidden
            return;
            case 12:
            this.AddEmployees = ((System.Windows.Controls.Button)(target));
            
            #line 42 "..\..\Director.xaml"
            this.AddEmployees.Click += new System.Windows.RoutedEventHandler(this.AddEmployees_Click);
            
            #line default
            #line hidden
            return;
            case 13:
            this.RemoveEmployees = ((System.Windows.Controls.Button)(target));
            
            #line 43 "..\..\Director.xaml"
            this.RemoveEmployees.Click += new System.Windows.RoutedEventHandler(this.RemoveEmployees_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

