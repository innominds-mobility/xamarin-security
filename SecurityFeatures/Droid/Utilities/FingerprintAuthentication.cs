using System;
using Android;
using Android.App;
using Android.Hardware.Fingerprints;
using Android.Support.V4.Content;
using Android.Support.V4.Hardware.Fingerprint;
using Android.Util;
using Java.Lang;
using Javax.Crypto;

namespace SecurityFeatures.Droid
{
	public class FingerprintAuthentication
	{
		public FingerprintAuthentication()
		{
		}

		public void fingerPrintDetection(Activity context)
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
						CryptoObjectHelper cryptoHelper = new CryptoObjectHelper();
						var cancellationSignal = new Android.Support.V4.OS.CancellationSignal();

						// AuthCallbacks is a C# class defined elsewhere in code.
						FingerprintManagerCompat.AuthenticationCallback authenticationCallback = new SimpleAuthCallbacks(context);

						// Here is where the CryptoObjectHelper builds the CryptoObject. 
						fingerprintManager.Authenticate(cryptoHelper.BuildCryptoObject(), (int)FingerprintAuthenticationFlags.None, cancellationSignal, authenticationCallback, null);
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

	class SimpleAuthCallbacks : FingerprintManagerCompat.AuthenticationCallback
	{
		// ReSharper disable once MemberHidesStaticFromOuterClass
		static readonly string TAG = "X:" + typeof(SimpleAuthCallbacks).Name;
		static readonly byte[] SECRET_BYTES = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
		private AuthenticationViewModel activityModel;
		public SimpleAuthCallbacks(Activity activityContext)
		{
			var activity = (AuthenticationView)activityContext;
			if (activity != null)
			{
				activityModel = (AuthenticationViewModel)activity.ViewModel;
			}

		}
		public override void OnAuthenticationSucceeded(FingerprintManagerCompat.AuthenticationResult result)
		{
			Log.Debug(TAG, "OnAuthenticationSucceeded");
			if (result.CryptoObject.Cipher != null)
			{
				try
				{
					// Calling DoFinal on the Cipher ensures that the encryption worked.
					Log.Debug(TAG, "Fingerprint authentication succeeded");
					ReportSuccess();
				}
				catch (BadPaddingException bpe)
				{
					Log.Error(TAG, "Failed to encrypt the data with the generated key." + bpe);
					ReportAuthenticationFailed();
				}
				catch (IllegalBlockSizeException ibse)
				{
					Log.Error(TAG, "Failed to encrypt the data with the generated key." + ibse);
					ReportAuthenticationFailed();
				}
			}
			else
			{
				// No cipher used, assume that everything went well and trust the results.
				Log.Debug(TAG, "Fingerprint authentication succeeded.");
				ReportSuccess();
			}
		}

		void ReportSuccess()
		{
			if (activityModel != null)
			{
				activityModel.OnAuthenticationSuccess();
			}
		}

		void ReportScanFailure(int errMsgId, string errorMessage)
		{
			if (activityModel != null)
			{
				activityModel.OnAuthenticationFailure();
			}
		}

		void ReportAuthenticationFailed()
		{
			if (activityModel != null)
			{
				activityModel.OnAuthenticationFailure();
			}
		}

		public override void OnAuthenticationError(int errMsgId, ICharSequence errString)
		{
			// There are some situations where we don't care about the error. For example, 
			// if the user cancelled the scan, this will raise errorID #5. We don't want to
			// report that, we'll just ignore it as that event is a part of the workflow.
			bool reportError = (errMsgId == (int)FingerprintState.ErrorCanceled);
			string debugMsg = string.Format("OnAuthenticationError: {0}:`{1}`.", errMsgId, errString);
			Log.Debug(TAG, debugMsg);
		}

		public override void OnAuthenticationFailed()
		{
			Log.Info(TAG, "Authentication failed.");
			ReportAuthenticationFailed();
		}

		public override void OnAuthenticationHelp(int helpMsgId, ICharSequence helpString)
		{
			Log.Debug(TAG, "OnAuthenticationHelp: {0}:`{1}`", helpString, helpMsgId);
			ReportScanFailure(helpMsgId, helpString.ToString());
		}
	}
	
}
