﻿#pragma checksum "..\..\..\..\View\AddReportView_BakedCakes.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "2FED407BBFA00ED74558BD4E3BAA4CC90E68D954"
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


namespace ConfectSoft.View {
    
    
    /// <summary>
    /// AddReportView_BakedCakes
    /// </summary>
    public partial class AddReportView_BakedCakes : System.Windows.Window, System.Windows.Markup.IComponentConnector, System.Windows.Markup.IStyleConnector {
        
        
        #line 25 "..\..\..\..\View\AddReportView_BakedCakes.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel pnlControlBar;
        
        #line default
        #line hidden
        
        
        #line 32 "..\..\..\..\View\AddReportView_BakedCakes.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnClose;
        
        #line default
        #line hidden
        
        
        #line 39 "..\..\..\..\View\AddReportView_BakedCakes.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnMinimize;
        
        #line default
        #line hidden
        
        
        #line 52 "..\..\..\..\View\AddReportView_BakedCakes.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid bakedCakesReport_DailyReportForm;
        
        #line default
        #line hidden
        
        
        #line 146 "..\..\..\..\View\AddReportView_BakedCakes.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox comboBoxCakes;
        
        #line default
        #line hidden
        
        
        #line 159 "..\..\..\..\View\AddReportView_BakedCakes.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtCakePriceKg;
        
        #line default
        #line hidden
        
        
        #line 173 "..\..\..\..\View\AddReportView_BakedCakes.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtBakedQuantity;
        
        #line default
        #line hidden
        
        
        #line 187 "..\..\..\..\View\AddReportView_BakedCakes.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DatePicker dateOfBaking;
        
        #line default
        #line hidden
        
        
        #line 198 "..\..\..\..\View\AddReportView_BakedCakes.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox comboBoxOrderType;
        
        #line default
        #line hidden
        
        
        #line 204 "..\..\..\..\View\AddReportView_BakedCakes.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnAddBakedCakesReport;
        
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
            System.Uri resourceLocater = new System.Uri("/ConfectSoft;component/view/addreportview_bakedcakes.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\View\AddReportView_BakedCakes.xaml"
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
            this.pnlControlBar = ((System.Windows.Controls.StackPanel)(target));
            
            #line 29 "..\..\..\..\View\AddReportView_BakedCakes.xaml"
            this.pnlControlBar.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.pnlControlBar_MouseLeftButtonDown);
            
            #line default
            #line hidden
            
            #line 30 "..\..\..\..\View\AddReportView_BakedCakes.xaml"
            this.pnlControlBar.MouseEnter += new System.Windows.Input.MouseEventHandler(this.pnlControlBar_MouseEnter);
            
            #line default
            #line hidden
            return;
            case 2:
            this.btnClose = ((System.Windows.Controls.Button)(target));
            
            #line 35 "..\..\..\..\View\AddReportView_BakedCakes.xaml"
            this.btnClose.Click += new System.Windows.RoutedEventHandler(this.btnClose_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.btnMinimize = ((System.Windows.Controls.Button)(target));
            
            #line 42 "..\..\..\..\View\AddReportView_BakedCakes.xaml"
            this.btnMinimize.Click += new System.Windows.RoutedEventHandler(this.btnMinimize_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.bakedCakesReport_DailyReportForm = ((System.Windows.Controls.DataGrid)(target));
            return;
            case 6:
            this.comboBoxCakes = ((System.Windows.Controls.ComboBox)(target));
            
            #line 150 "..\..\..\..\View\AddReportView_BakedCakes.xaml"
            this.comboBoxCakes.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.comboBoxCakes_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 7:
            this.txtCakePriceKg = ((System.Windows.Controls.TextBox)(target));
            
            #line 161 "..\..\..\..\View\AddReportView_BakedCakes.xaml"
            this.txtCakePriceKg.PreviewTextInput += new System.Windows.Input.TextCompositionEventHandler(this.numberFValidationTextBox);
            
            #line default
            #line hidden
            return;
            case 8:
            this.txtBakedQuantity = ((System.Windows.Controls.TextBox)(target));
            
            #line 174 "..\..\..\..\View\AddReportView_BakedCakes.xaml"
            this.txtBakedQuantity.PreviewTextInput += new System.Windows.Input.TextCompositionEventHandler(this.numberFValidationTextBox);
            
            #line default
            #line hidden
            return;
            case 9:
            this.dateOfBaking = ((System.Windows.Controls.DatePicker)(target));
            return;
            case 10:
            this.comboBoxOrderType = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 11:
            this.btnAddBakedCakesReport = ((System.Windows.Controls.Button)(target));
            
            #line 208 "..\..\..\..\View\AddReportView_BakedCakes.xaml"
            this.btnAddBakedCakesReport.Click += new System.Windows.RoutedEventHandler(this.btnAddBakedCakesReport_Click);
            
            #line default
            #line hidden
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
            case 5:
            
            #line 114 "..\..\..\..\View\AddReportView_BakedCakes.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.RemoveButton_Click);
            
            #line default
            #line hidden
            break;
            }
        }
    }
}
