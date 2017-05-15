using System;
using Android.Content;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Platform;
using MvvmCross.Droid.Views;

namespace SecurityFeatures.Droid
{
	public class Setup: MvxAndroidSetup
	{
		public Setup(Context applicationContext)
			: base(applicationContext)
		{
		}

		protected override IMvxApplication CreateApp()
		{
			return new SecurityFeatures.App();
		}

		protected override void InitializeLastChance()
		{
			base.InitializeLastChance();
		}

		protected override IMvxAndroidViewPresenter CreateViewPresenter()
		{
			var customPresenter = new CustomPresenter();
			return customPresenter;
		}
	}
}