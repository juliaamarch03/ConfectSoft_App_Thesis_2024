﻿#pragma checksum "..\..\..\..\View\RecipePage.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "BB53BF68889729B61B5C57591035DC7A261A3B50"
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
    /// RecipePage
    /// </summary>
    public partial class RecipePage : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 12 "..\..\..\..\View\RecipePage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Frame MainFrame;
        
        #line default
        #line hidden
        
        
        #line 25 "..\..\..\..\View\RecipePage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Media.ImageBrush headerRecipeImg;
        
        #line default
        #line hidden
        
        
        #line 46 "..\..\..\..\View\RecipePage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock recipeTitle;
        
        #line default
        #line hidden
        
        
        #line 56 "..\..\..\..\View\RecipePage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock prepTimeTxt;
        
        #line default
        #line hidden
        
        
        #line 64 "..\..\..\..\View\RecipePage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock cookTimeTxt;
        
        #line default
        #line hidden
        
        
        #line 72 "..\..\..\..\View\RecipePage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock genTimeTxt;
        
        #line default
        #line hidden
        
        
        #line 92 "..\..\..\..\View\RecipePage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Media.ImageBrush imageCake;
        
        #line default
        #line hidden
        
        
        #line 112 "..\..\..\..\View\RecipePage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock txtIngredients;
        
        #line default
        #line hidden
        
        
        #line 120 "..\..\..\..\View\RecipePage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock txtCreamInfo;
        
        #line default
        #line hidden
        
        
        #line 128 "..\..\..\..\View\RecipePage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock txtHowToCook;
        
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
            System.Uri resourceLocater = new System.Uri("/ConfectSoft;component/view/recipepage.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\View\RecipePage.xaml"
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
            this.MainFrame = ((System.Windows.Controls.Frame)(target));
            return;
            case 2:
            this.headerRecipeImg = ((System.Windows.Media.ImageBrush)(target));
            return;
            case 3:
            
            #line 40 "..\..\..\..\View\RecipePage.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.recipeTitle = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 5:
            this.prepTimeTxt = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 6:
            this.cookTimeTxt = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 7:
            this.genTimeTxt = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 8:
            this.imageCake = ((System.Windows.Media.ImageBrush)(target));
            return;
            case 9:
            this.txtIngredients = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 10:
            this.txtCreamInfo = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 11:
            this.txtHowToCook = ((System.Windows.Controls.TextBlock)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}
