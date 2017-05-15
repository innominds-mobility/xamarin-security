using System;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Views;

namespace SecurityFeatures.Droid
{
	public class CustomPresenter: MvxAndroidViewPresenter
	{

		public override void Show(MvxViewModelRequest request)
		{
			if (request.ParameterValues != null && request.ParameterValues.ContainsKey("pop"))
			{
				if (Activity != null)
				{
					Activity.Finish();
				}
			}
			else {
				base.Show(request);
			}
			return;
		}

	}
}

