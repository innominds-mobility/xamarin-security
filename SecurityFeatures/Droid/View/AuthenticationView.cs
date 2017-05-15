
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
	[Activity(Label = "AuthenticationView")]
	public class AuthenticationView : Activity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Create your application here
			SetContentView(Resource.Layout.Layout_Authentication);

			//check fingerprint is available on the device or not
			FingerprintAuthentication.fingerPrintContext(this);
		}
	}
}
