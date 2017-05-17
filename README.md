# xamarin-security
Xamarin support for iOS/Android Security features
## Project Details
# Security Features - Xamarin MVVMCross

#### Useful feature:
* TouchId/FingerPrint integration in Xamarin MVVM Cross

#### How it works:
* Checks either the hardware supports fingerprint/TouchId or not.
* If supports, check either user is enrolled for a fingerprint or not.
* If enrolled, scan the fingerprint and handle the respective callbacks.

#### Steps for integration:

* Create an Interface say IAuthentication having success and failure callbacks.
* Create a ViewModel say AuthenticationViewModel implements IAuthentication and having its binding properties and listeners.
* Create TouchIdAuthentication class in Android/iOS project to execute the platform specific authentication process

And here's android code! :+1:

```C#
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
            // No permission. Go and ask for permissions and don't start the scanner.
        }
    }
    else {
        //user has not enrolled for fingerprint
    }
}
else {
    //Device is not compatible for fingerprint scanning
}
```

Create a callback class to handle the success and error response while scanning fingerprint

Look here for sample code:

```C#
class SimpleAuthCallbacks : FingerprintManagerCompat.AuthenticationCallback
{
    // ReSharper disable once MemberHidesStaticFromOuterClass
    static readonly string TAG = "X:" + typeof(SimpleAuthCallbacks).Name;
    static readonly byte[] SECRET_BYTES = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

    public override void OnAuthenticationSucceeded(FingerprintManagerCompat.AuthenticationResult result)
    {
        Log.Debug(TAG, "OnAuthenticationSucceeded");
        if (result.CryptoObject.Cipher != null)
        {
            try
            {
                // Calling DoFinal on the Cipher ensures that the encryption worked.
                Log.Debug(TAG, "Fingerprint authentication succeeded");
                //Calling the success handler
            }
            catch (BadPaddingException bpe)
            {
                Log.Error(TAG, "Failed to encrypt the data with the generated key." + bpe);
                //Calling the failure handler
            }
            catch (IllegalBlockSizeException ibse)
            {
                Log.Error(TAG, "Failed to encrypt the data with the generated key." + ibse);
                //Calling the failure handler
            }
        }
        else
        {
            // No cipher used, assume that everything went well and trust the results.
            Log.Debug(TAG, "Fingerprint authentication succeeded.");
            //Calling the success handler
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
        //Calling the failure handler
    }

    public override void OnAuthenticationHelp(int helpMsgId, ICharSequence helpString)
    {
        Log.Debug(TAG, "OnAuthenticationHelp: {0}:`{1}`", helpString, helpMsgId);
        //Calling the failure handler
    }
}
```

And here's iOS code! :+1:
```C#
var context = new LAContext();
NSError AuthError;
var authenticationReason = new NSString("Authentication is needed to access the application");
if (context.CanEvaluatePolicy(LAPolicy.DeviceOwnerAuthenticationWithBiometrics, out AuthError))
{
    var replyHandler = new LAContextReplyHandler((success, error) =>{
        controller.InvokeOnMainThread(() =>
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
        });
    });
    context.EvaluatePolicy(LAPolicy.DeviceOwnerAuthenticationWithBiometrics, authenticationReason, replyHandler);
}
else {
    //Calling the failure handler
    viewModel.OnAuthenticationFailure();
}
```

### Refernces used to make this:

* [TouchId](https://developer.xamarin.com/guides/ios/platform_features/introduction_to_touchid/) for TouchId Authentication in iOS
* [FingerPrint](https://developer.xamarin.com/guides/android/platform_features/fingerprint-authentication/getting-started/) for FingerPrint Authentication in Android
