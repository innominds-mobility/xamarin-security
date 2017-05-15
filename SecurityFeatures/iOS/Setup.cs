using System;
using MvvmCross.Core.ViewModels;
using MvvmCross.iOS.Platform;
using MvvmCross.iOS.Views.Presenters;

namespace SecurityFeatures.iOS
{
	public class Setup: MvxIosSetup
	{
		public Setup(MvxApplicationDelegate appDelegate, IMvxIosViewPresenter presenter)
		: base(appDelegate, presenter)
		{
		}

		protected override IMvxApplication CreateApp()
		{
			return new SecurityFeatures.App();
		}
	}
}