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
    [Register ("NotesDetailViewController")]
    partial class NotesDetailViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextView Description { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField Title { get; set; }

        [Action ("DeleteNoteHandler:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void DeleteNoteHandler (UIKit.UIButton sender);

        [Action ("HandleKeyboard:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void HandleKeyboard (UIKit.UIButton sender);

        [Action ("SaveNoteHandler:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void SaveNoteHandler (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (Description != null) {
                Description.Dispose ();
                Description = null;
            }

            if (Title != null) {
                Title.Dispose ();
                Title = null;
            }
        }
    }
}