// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace SecurityFeatures.iOS
{
    [Register ("DashboardView")]
    partial class DashboardView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton logoutBtn { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (logoutBtn != null) {
                logoutBtn.Dispose ();
                logoutBtn = null;
            }
        }
    }
}