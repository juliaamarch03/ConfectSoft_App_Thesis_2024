﻿#pragma checksum "..\..\..\..\..\View\DailyReportChildsView\UsedRawView_DR.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "0C1729087ED6AB55A7FAA211E4541D7C531B6EFF"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using FontAwesome.Sharp;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
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


namespace SweetBakeryWpfApp.View {
    
    
    /// <summary>
    /// UsedRawView_DR
    /// </summary>
    public partial class UsedRawView_DR : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector, System.Windows.Markup.IStyleConnector {
        
        
        #line 38 "..\..\..\..\..\View\DailyReportChildsView\UsedRawView_DR.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DatePicker dateOfBaking;
        
        #line default
        #line hidden
        
        
        #line 55 "..\..\..\..\..\View\DailyReportChildsView\UsedRawView_DR.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnSearchData;
        
        #line default
        #line hidden
        
        
        #line 74 "..\..\..\..\..\View\DailyReportChildsView\UsedRawView_DR.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnCancelSearchData;
        
        #line default
        #line hidden
        
        
        #line 98 "..\..\..\..\..\View\DailyReportChildsView\UsedRawView_DR.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnPrintData;
        
        #line default
        #line hidden
        
        
        #line 124 "..\..\..\..\..\View\DailyReportChildsView\UsedRawView_DR.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnAddBakedCakesReport;
        
        #line default
        #line hidden
        
        
        #line 134 "..\..\..\..\..\View\DailyReportChildsView\UsedRawView_DR.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid usedRawReport_DailyReportForm;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "5.0.5.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/ConfectSoft;component/view/dailyreportchildsview/usedrawview_dr.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\View\DailyReportChildsView\UsedRawView_DR.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "5.0.5.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.dateOfBaking = ((System.Windows.Controls.DatePicker)(target));
            return;
            case 2:
            this.btnSearchData = ((System.Windows.Controls.Button)(target));
            
            #line 57 "..\..\..\..\..\View\DailyReportChildsView\UsedRawView_DR.xaml"
            this.btnSearchData.Click += new System.Windows.RoutedEventHandler(this.btnSearchData_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.btnCancelSearchData = ((System.Windows.Controls.Button)(target));
            
            #line 76 "..\..\..\..\..\View\DailyReportChildsView\UsedRawView_DR.xaml"
            this.btnCancelSearchData.Click += new System.Windows.RoutedEventHandler(this.btnCancelSearchData_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.btnPrintData = ((System.Windows.Controls.Button)(target));
            
            #line 100 "..\..\..\..\..\View\DailyReportChildsView\UsedRawView_DR.xaml"
            this.btnPrintData.Click += new System.Windows.RoutedEventHandler(this.btnPrintData_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.btnAddBakedCakesReport = ((System.Windows.Controls.Button)(target));
            
            #line 127 "..\..\..\..\..\View\DailyReportChildsView\UsedRawView_DR.xaml"
            this.btnAddBakedCakesReport.Click += new System.Windows.RoutedEventHandler(this.btnAddBakedCakesReport_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.usedRawReport_DailyReportForm = ((System.Windows.Controls.DataGrid)(target));
            return;
            }
            this._contentLoaded = true;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "5.0.5.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        void System.Windows.Markup.IStyleConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 7:
            
            #line 187 "..\..\..\..\..\View\DailyReportChildsView\UsedRawView_DR.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.EditButton_Click);
            
            #line default
            #line hidden
            break;
            case 8:
            
            #line 190 "..\..\..\..\..\View\DailyReportChildsView\UsedRawView_DR.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.RemoveButton_Click);
            
            #line default
            #line hidden
            break;
            }
        }
    }
}

