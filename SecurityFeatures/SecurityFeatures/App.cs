using System;
using MvvmCross.Core.ViewModels;

namespace SecurityFeatures
{
	public class App: MvxApplication
	{
		public App()
		{
			RegisterAppStart<AuthenticationViewModel>();
		}
	}
}