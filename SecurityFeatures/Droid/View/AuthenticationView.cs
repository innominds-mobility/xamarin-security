
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace SecurityFeatures.Droid
{
	[Activity(Label = "AuthenticationView", MainLauncher = true, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
	public class AuthenticationView : BaseActivity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Create your application here
			SetContentView(Resource.Layout.Layout_Authentication);

			Button loginBtn = FindViewById<Button>(Resource.Id.loginBtn);
			loginBtn.Click += (sender, e) =>
			{
				// Perform action on click
				//check fingerprint is available on the device or not
				var authenticationObj = new FingerprintAuthentication();
				authenticationObj.fingerPrintDetection(this);
			};

		}
	}
}
