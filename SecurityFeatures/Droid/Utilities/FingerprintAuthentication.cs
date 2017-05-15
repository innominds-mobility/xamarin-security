using System;
using Android;
using Android.App;
using Android.Hardware.Fingerprints;
using Android.Support.V4.Content;
using Android.Support.V4.Hardware.Fingerprint;

namespace SecurityFeatures.Droid
{
	public class FingerprintAuthentication
	{
		public FingerprintAuthentication()
		{
		}

		public static void fingerPrintDetection(Activity context)
		{
			// Using the Android Support Library v4
			FingerprintManagerCompat fingerprintManager = FingerprintManagerCompat.From(context);
			if (fingerprintManager.IsHardwareDetected)
			{
				if (fingerprintManager.HasEnrolledFingerprints)
				{
					// The context is typically a reference to the current activity.
					Android.Content.PM.Permission permissionResult = ContextCompat.CheckSelfPermission(context, Manifest.Permission.UseFingerprint);
					if (permissionResult == Android.Content.PM.Permission.Granted)
					{
						// Permission granted - go ahead and start the fingerprint scanner.

					}
					else
					{
						// No permission. Go and ask for permissions and don't start the scanner. See
						// http://developer.android.com/training/permissions/requesting.html
					}
				}
				else {
					//user has not enrolled for fingerprint
				}

			}
			else {
				//Device is not compatible for fingerprint scanning
			}
		}
	}
}
