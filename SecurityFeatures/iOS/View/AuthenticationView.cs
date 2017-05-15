using System;
using Foundation;
using LocalAuthentication;
using MvvmCross.Binding.BindingContext;
using MvvmCross.iOS.Views;
using UIKit;

namespace SecurityFeatures.iOS
{
	public partial class AuthenticationView : MvxViewController
	{
		public AuthenticationView() : base("AuthenticationView", null)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			// Perform any additional setup after loading the view, typically from a nib.
			this.CreateBinding(touchIdStatusLbl).To((AuthenticationViewModel vm) => vm.AuthenticationText).Apply();
			loginBtn.TouchDown += delegate
			{
				AuthenticateUser();
			};
		}

		/// <summary>
		/// Authenticates the user.
		/// </summary>
		private void AuthenticateUser()
		{
			var viewModel = (AuthenticationViewModel)this.ViewModel;
			var context = new LAContext();
			NSError AuthError;
			var authenticationReason = new NSString("Authentication is needed to access the application");

			if (context.CanEvaluatePolicy(LAPolicy.DeviceOwnerAuthenticationWithBiometrics, out AuthError))
			{
				var replyHandler = new LAContextReplyHandler((success, error) =>
				{
					this.InvokeOnMainThread(() =>
					{
						if (viewModel != null)
						{
							if (success)
							{
								viewModel.OnAuthenticationSuccess();
								Console.WriteLine("You logged in!");
							}
							else {
								viewModel.OnAuthenticationFailure();
								//Show fallback mechanism her
								//showPasswordAlert();
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

		/// <summary>
		/// Shows the password alert.
		/// </summary>
		private void showPasswordAlert()
		{
			var passwordAlert = new UIAlertView("SecurityFeature", "Please type your password", null, "Cancel", "Ok");
			passwordAlert.AlertViewStyle = UIAlertViewStyle.SecureTextInput;
			passwordAlert.Show();
			passwordAlert.Clicked += (object senders, UIButtonEventArgs index) =>
				{
					if (index.ButtonIndex == 0)
					{
						// do something if Cancel
					}
					else
					{
						// Do something if Ok
					}
				};
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}


