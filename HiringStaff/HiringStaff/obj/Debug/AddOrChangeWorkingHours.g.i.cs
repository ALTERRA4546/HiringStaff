﻿#pragma checksum "..\..\AddOrChangeWorkingHours.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "6A118C8DAC61A5A79695BF943AA8C8AD72BD830A83184531C24D41F0278F02D8"
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
    /// AddOrChangeWorkingHours
    /// </summary>
    public partial class AddOrChangeWorkingHours : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 18 "..\..\AddOrChangeWorkingHours.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox Search;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\AddOrChangeWorkingHours.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox ChoiceOfPosition;
        
        #line default
        #line hidden
        
        
        #line 24 "..\..\AddOrChangeWorkingHours.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label SelectedUser;
        
        #line default
        #line hidden
        
        
        #line 26 "..\..\AddOrChangeWorkingHours.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid EmployeeData;
        
        #line default
        #line hidden
        
        
        #line 29 "..\..\AddOrChangeWorkingHours.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DatePicker StartingWorkingDate;
        
        #line default
        #line hidden
        
        
        #line 30 "..\..\AddOrChangeWorkingHours.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox StartingWorkingTime;
        
        #line default
        #line hidden
        
        
        #line 35 "..\..\AddOrChangeWorkingHours.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DatePicker EndWorkingDate;
        
        #line default
        #line hidden
        
        
        #line 36 "..\..\AddOrChangeWorkingHours.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox EndWorkingTime;
        
        #line default
        #line hidden
        
        
        #line 39 "..\..\AddOrChangeWorkingHours.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Save;
        
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
            System.Uri resourceLocater = new System.Uri("/HiringStaff;component/addorchangeworkinghours.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\AddOrChangeWorkingHours.xaml"
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
            
            #line 8 "..\..\AddOrChangeWorkingHours.xaml"
            ((HiringStaff.AddOrChangeWorkingHours)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Window_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.Search = ((System.Windows.Controls.TextBox)(target));
            
            #line 18 "..\..\AddOrChangeWorkingHours.xaml"
            this.Search.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.Search_TextChanged);
            
            #line default
            #line hidden
            return;
            case 3:
            this.ChoiceOfPosition = ((System.Windows.Controls.ComboBox)(target));
            
            #line 20 "..\..\AddOrChangeWorkingHours.xaml"
            this.ChoiceOfPosition.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.ChoiceOfPosition_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 4:
            this.SelectedUser = ((System.Windows.Controls.Label)(target));
            return;
            case 5:
            this.EmployeeData = ((System.Windows.Controls.DataGrid)(target));
            
            #line 26 "..\..\AddOrChangeWorkingHours.xaml"
            this.EmployeeData.MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(this.EmployeeData_MouseDoubleClick);
            
            #line default
            #line hidden
            return;
            case 6:
            this.StartingWorkingDate = ((System.Windows.Controls.DatePicker)(target));
            return;
            case 7:
            this.StartingWorkingTime = ((System.Windows.Controls.TextBox)(target));
            return;
            case 8:
            this.EndWorkingDate = ((System.Windows.Controls.DatePicker)(target));
            return;
            case 9:
            this.EndWorkingTime = ((System.Windows.Controls.TextBox)(target));
            return;
            case 10:
            this.Save = ((System.Windows.Controls.Button)(target));
            
            #line 39 "..\..\AddOrChangeWorkingHours.xaml"
            this.Save.Click += new System.Windows.RoutedEventHandler(this.Save_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
