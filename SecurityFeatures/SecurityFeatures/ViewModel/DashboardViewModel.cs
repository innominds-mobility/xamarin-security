using System;
using MvvmCross.Core.ViewModels;

namespace SecurityFeatures
{
	public class DashboardViewModel: BaseViewModel
	{
		public DashboardViewModel()
		{
		}

		public IMvxCommand LogoutBtnClickCommand
		{
			get { return new MvxCommand(onLogoutBtnClick); }
		}

		void onLogoutBtnClick()
		{
			ShowViewModel<DashboardViewModel>(new { pop = "pop" });
		}
	}
}