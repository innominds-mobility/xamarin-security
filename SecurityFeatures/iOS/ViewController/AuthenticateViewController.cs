using Foundation;
using System;
using UIKit;
using LocalAuthentication;

namespace SecurityFeatures.iOS
{
    public partial class AuthenticateViewController : UIViewController
    {
        public AuthenticateViewController (IntPtr handle) : base (handle)
        {
        }
		// when displaying, set-up the properties
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			this.AuthenticateUser();
		}

		/// <summary>
		/// Authenticates the user.
		/// </summary>
		private void AuthenticateUser()
		{
			var context = new LAContext();
			NSError AuthError;
			var authenticationReason = new NSString("Authentication is needed to access the application");

			if (context.CanEvaluatePolicy(LAPolicy.DeviceOwnerAuthenticationWithBiometrics, out AuthError))
			{
				var replyHandler = new LAContextReplyHandler((success, error) =>
				{

					this.InvokeOnMainThread(() =>
					{
						if (success)
						{
							Console.WriteLine("You logged in!");
							PerformSegue("AuthenticationSegue", this);
						}
						else {
							//Show fallback mechanism her
							showPasswordAlert();
						}
					});

				});
				context.EvaluatePolicy(LAPolicy.DeviceOwnerAuthenticationWithBiometrics, authenticationReason, replyHandler);
			};
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
	}
}