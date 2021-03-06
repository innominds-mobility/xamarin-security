﻿using System;
using Foundation;
using LocalAuthentication;
using MvvmCross.iOS.Views;

namespace SecurityFeatures.iOS
{
	public class TouchIDAuthentication
	{
		public TouchIDAuthentication()
		{
		}

		/// <summary>
		/// Authenticates the user.
		/// </summary>
		public static void AuthenticateUser(MvxViewController controller)
		{
			var viewModel = (AuthenticationViewModel)controller.ViewModel;
			var context = new LAContext();
			NSError AuthError;
			var authenticationReason = new NSString("Authentication is needed to access the application");
			if (context.CanEvaluatePolicy(LAPolicy.DeviceOwnerAuthenticationWithBiometrics, out AuthError))
			{
				var replyHandler = new LAContextReplyHandler((success, error) =>
				{
					controller.InvokeOnMainThread(() =>
					{
						if (viewModel != null)
						{
							if (success)
							{
								//Calling the success handler
								viewModel.OnAuthenticationSuccess();
								Console.WriteLine("You logged in!");
							}
							else {
								//Calling the failure handler
								viewModel.OnAuthenticationFailure();
							}
						}
					});
				});
				context.EvaluatePolicy(LAPolicy.DeviceOwnerAuthenticationWithBiometrics, authenticationReason, replyHandler);
			}
			else {
				viewModel.OnAuthenticationFailure();
			}
		}
	}
}
