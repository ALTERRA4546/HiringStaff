﻿#pragma checksum "..\..\DrawingUpWorkSchedule.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "16855F1A15C5D487A547B76A374DDB4BF51E320793419C6F16CD221F0567314C"
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
    /// DrawingUpWorkSchedule
    /// </summary>
    public partial class DrawingUpWorkSchedule : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 18 "..\..\DrawingUpWorkSchedule.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox Search;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\DrawingUpWorkSchedule.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox ChoiceOfPosition;
        
        #line default
        #line hidden
        
        
        #line 21 "..\..\DrawingUpWorkSchedule.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox DateChecker;
        
        #line default
        #line hidden
        
        
        #line 23 "..\..\DrawingUpWorkSchedule.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DatePicker StartingDate;
        
        #line default
        #line hidden
        
        
        #line 25 "..\..\DrawingUpWorkSchedule.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DatePicker EndDate;
        
        #line default
        #line hidden
        
        
        #line 27 "..\..\DrawingUpWorkSchedule.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid WorkingHoursData;
        
        #line default
        #line hidden
        
        
        #line 30 "..\..\DrawingUpWorkSchedule.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button AddGraph;
        
        #line default
        #line hidden
        
        
        #line 31 "..\..\DrawingUpWorkSchedule.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button RemoveGraph;
        
        #line default
        #line hidden
        
        
        #line 33 "..\..\DrawingUpWorkSchedule.xaml"
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
            System.Uri resourceLocater = new System.Uri("/HiringStaff;component/drawingupworkschedule.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\DrawingUpWorkSchedule.xaml"
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
            
            #line 8 "..\..\DrawingUpWorkSchedule.xaml"
            ((HiringStaff.DrawingUpWorkSchedule)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Window_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.Search = ((System.Windows.Controls.TextBox)(target));
            
            #line 18 "..\..\DrawingUpWorkSchedule.xaml"
            this.Search.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.Search_TextChanged);
            
            #line default
            #line hidden
            return;
            case 3:
            this.ChoiceOfPosition = ((System.Windows.Controls.ComboBox)(target));
            
            #line 20 "..\..\DrawingUpWorkSchedule.xaml"
            this.ChoiceOfPosition.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.ChoiceOfPosition_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 4:
            this.DateChecker = ((System.Windows.Controls.CheckBox)(target));
            
            #line 21 "..\..\DrawingUpWorkSchedule.xaml"
            this.DateChecker.Click += new System.Windows.RoutedEventHandler(this.DateChecker_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.StartingDate = ((System.Windows.Controls.DatePicker)(target));
            
            #line 23 "..\..\DrawingUpWorkSchedule.xaml"
            this.StartingDate.SelectedDateChanged += new System.EventHandler<System.Windows.Controls.SelectionChangedEventArgs>(this.StartingDate_SelectedDateChanged);
            
            #line default
            #line hidden
            return;
            case 6:
            this.EndDate = ((System.Windows.Controls.DatePicker)(target));
            
            #line 25 "..\..\DrawingUpWorkSchedule.xaml"
            this.EndDate.SelectedDateChanged += new System.EventHandler<System.Windows.Controls.SelectionChangedEventArgs>(this.StartingDate_SelectedDateChanged);
            
            #line default
            #line hidden
            return;
            case 7:
            this.WorkingHoursData = ((System.Windows.Controls.DataGrid)(target));
            
            #line 27 "..\..\DrawingUpWorkSchedule.xaml"
            this.WorkingHoursData.MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(this.WorkingHoursData_MouseDoubleClick);
            
            #line default
            #line hidden
            return;
            case 8:
            this.AddGraph = ((System.Windows.Controls.Button)(target));
            
            #line 30 "..\..\DrawingUpWorkSchedule.xaml"
            this.AddGraph.Click += new System.Windows.RoutedEventHandler(this.AddGraph_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            this.RemoveGraph = ((System.Windows.Controls.Button)(target));
            
            #line 31 "..\..\DrawingUpWorkSchedule.xaml"
            this.RemoveGraph.Click += new System.Windows.RoutedEventHandler(this.RemoveGraph_Click);
            
            #line default
            #line hidden
            return;
            case 10:
            this.Export = ((System.Windows.Controls.Button)(target));
            
            #line 33 "..\..\DrawingUpWorkSchedule.xaml"
            this.Export.Click += new System.Windows.RoutedEventHandler(this.Export_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

