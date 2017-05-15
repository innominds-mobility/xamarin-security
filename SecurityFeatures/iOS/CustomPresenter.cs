using System;
using MvvmCross.Core.ViewModels;
using MvvmCross.iOS.Views.Presenters;
using UIKit;

namespace SecurityFeatures.iOS
{
	public class CustomPresenter: MvxIosViewPresenter
	{

		/// <summary>
		/// Initializes a new instance of the <see cref="T:actchargers.iOS.CustomPresenter"/> class.
		/// </summary>
		/// <param name="applicationDelegate">Application delegate.</param>
		/// <param name="window">Window.</param>
		public CustomPresenter(UIApplicationDelegate applicationDelegate, UIWindow window)
			: base(applicationDelegate, window)
		{

		}

		/// <summary>
		/// Show the specified request.
		/// </summary>
		/// <param name="request">Request.</param>
		public override void Show(MvxViewModelRequest request)
		{
			//if Host null then set the rootview  
			//Handling viewmodel navigations from LoginViewModel
			if (request.ParameterValues != null && request.ParameterValues.Keys.Contains("pop"))
			{
				MasterNavigationController.PopViewController(true);
				return;
			}
			base.Show(request);
			return;
		}
	}
}
