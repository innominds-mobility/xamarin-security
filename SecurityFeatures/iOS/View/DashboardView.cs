using System;
using MvvmCross.Binding.BindingContext;
using MvvmCross.iOS.Views;
using UIKit;

namespace SecurityFeatures.iOS
{
	public partial class DashboardView : MvxViewController
	{
		public DashboardView() : base("DashboardView", null)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			this.NavigationController.NavigationBarHidden = true;
			// Perform any additional setup after loading the view, typically from a nib.
			this.CreateBinding(logoutBtn).To((DashboardViewModel vm) => vm.LogoutBtnClickCommand).Apply();
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use
		}
	}
}
