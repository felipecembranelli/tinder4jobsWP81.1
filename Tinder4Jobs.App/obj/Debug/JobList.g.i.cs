﻿

#pragma checksum "C:\Users\feliperc\OneDrive\Public\Estudo\Estudo\POCMigrationTinder4Jobs\PrototypeTinder4Jobs\JobList.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "D0AB9B815B27FB1CAF0EEA9A375B0A1B"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PrototypeTinder4Jobs
{
    partial class JobList : global::Windows.UI.Xaml.Controls.Page
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.Pivot pivot; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.ListView ListViewReproved; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.ListView ListViewApproved; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private bool _contentLoaded;

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent()
        {
            if (_contentLoaded)
                return;

            _contentLoaded = true;
            global::Windows.UI.Xaml.Application.LoadComponent(this, new global::System.Uri("ms-appx:///JobList.xaml"), global::Windows.UI.Xaml.Controls.Primitives.ComponentResourceLocation.Application);
 
            pivot = (global::Windows.UI.Xaml.Controls.Pivot)this.FindName("pivot");
            ListViewReproved = (global::Windows.UI.Xaml.Controls.ListView)this.FindName("ListViewReproved");
            ListViewApproved = (global::Windows.UI.Xaml.Controls.ListView)this.FindName("ListViewApproved");
        }
    }
}



