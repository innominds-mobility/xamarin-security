using Foundation;
using System;
using UIKit;

namespace SecurityFeatures.iOS
{
	public partial class NotesDetailViewController : UIViewController
	{
		Notes currentNote { get; set; }
		public NotesViewController Delegate { get; set; } // will be used to Save, Delete later

		public NotesDetailViewController(IntPtr handle) : base(handle)
		{
		}

		// when displaying, set-up the propertiess
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			Title.Text = currentNote.name;
			//Description.Text = currentNote.description;/
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
		}

		// this will be called before the view is displayedd
		public void SetTask(NotesViewController controller, Notes note)
		{
			Delegate = controller;
			currentNote = note;
		}

		/// <summary>
		/// Saves the current note.
		/// </summary>
		/// <param name="sender">Sender.</param>
		partial void SaveNoteHandler(UIButton sender)
		{
			currentNote.name = Title.Text;
			currentNote.description = Description.Text;
			Delegate.SaveTask(currentNote);
		}
	
		/// <summary>
		/// Deletes the current note.
		/// </summary>
		/// <param name="sender">Sender.</param>
		partial void DeleteNoteHandler(UIButton sender)
		{
			Delegate.DeleteTask(currentNote);
		}

	partial void HandleKeyboard(UIButton sender)
		{
			Title.ResignFirstResponder();
			Description.ResignFirstResponder();
		}
	}
}
