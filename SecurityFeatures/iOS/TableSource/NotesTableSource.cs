using System;
using UIKit;

namespace SecurityFeatures.iOS
{
	public class NotesTableSource: UITableViewSource
	{
		// there is NO database or storage of Tasks in this example, just an in-memory List<>
		Notes[] tableItems;
		string cellIdentifier = "notescell"; // set in the Storyboard

		public NotesTableSource(Notes[] items)
		{
			tableItems = items;
		}
		public override nint RowsInSection(UITableView tableview, nint section)
		{
			return tableItems.Length;
		}
		public override UITableViewCell GetCell(UITableView tableView, Foundation.NSIndexPath indexPath)
		{
			// in a Storyboard, Dequeue will ALWAYS return a cell,
			UITableViewCell cell = tableView.DequeueReusableCell(cellIdentifier);
			// now set the properties as normal
			cell.TextLabel.Text = tableItems[indexPath.Row].name;
			return cell;
		}
		public Notes GetItem(int id)
		{
			return tableItems[id];
		}
	}
}
