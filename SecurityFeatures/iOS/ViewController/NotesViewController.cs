using Foundation;
using System;
using UIKit;
using System.Collections.Generic;

namespace SecurityFeatures.iOS
{
    public partial class NotesViewController : UIViewController
    {
		List<Notes> notes;

		public NotesViewController (IntPtr handle) : base (handle)
        {
			// Custom initializationn
			notes = new List<Notes> {
				new Notes() {name="Salary Account", description="A/c No:123456789, Ifsc: SC00014578", isfavourite=true},
				new Notes() {name="Saving Account", description="A/c No:123456789, Ifsc: SC00014578", isfavourite=false}
			};
        }

		partial void NewNoteHandler(UIButton sender)
		{
			CreateTask();
		}

		public void CreateTask()
		{
			// first, add the task to the underlying data
			var newId = notes[notes.Count - 1].id + 1;
			var newNote = new Notes() { id = newId };
			notes.Add(newNote);

			// then open the detail view to edit notet
			var detail = Storyboard.InstantiateViewController("NotesDetailViewController") as NotesDetailViewController;
			detail.SetTask(this, newNote);
			NavigationController.PushViewController(detail, true);
		}

		public void SaveTask(Notes note)
		{
			//var oldTask = chores.Find(t => t.Id == chore.Id);
			NavigationController.PopViewController(true);
		}

		public void DeleteTask(Notes note)
		{
			var oldTask = notes.Find(t => t.id == note.id);
			notes.Remove(oldTask);
			NavigationController.PopViewController(true);
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			// bind every time, to reflect in the table vieww
			notesTableView.Source = new NotesTableSource(notes.ToArray());
		}

		public override void DidReceiveMemoryWarning()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning();
		}
    }
}