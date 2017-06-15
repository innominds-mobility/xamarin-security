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

##### Step 1 
###### Create/Add below files in your PCL project
1. Create an Interface say IAuthentication having success and failure callbacks.
```C#
public interface IAuthentication
{
void OnAuthenticationSuccess();
void OnAuthenticationFailure();
}
```

2. Create a ViewModel say AuthenticationViewModel implements IAuthentication and having its binding properties and listeners.
```C#
public class AuthenticationViewModel: BaseViewModel, IAuthentication
{
  public AuthenticationViewModel()
  {
  AuthenticationText = "";
  }

#region Properties
  private string _authenticationText;
  
  public string AuthenticationText
  {
    get { return _authenticationText; }
    set
    {
      _authenticationText = value;
      RaisePropertyChanged(() => AuthenticationText);
    }
  }

  private bool _authenticationStatus;
  public bool AuthenticationStatus
  {
    get { return _authenticationStatus; }
    set
    {
      _authenticationStatus = value;

    if (_authenticationStatus)
    {
      AuthenticationText = "";
    }
    else {
      AuthenticationText = "Authentication failure...";
    }
      RaisePropertyChanged(() => AuthenticationStatus);
    }
  }
  #endRegion

  #region IAuthentication callbacks
  public void OnAuthenticationSuccess()
  {
    //Handle the callback for authentication success response
    AuthenticationStatus = true;
    ShowViewModel<DashboardViewModel>();
  }

  public void OnAuthenticationFailure()
  {
    //Handle the callback for authentication failure response
    AuthenticationStatus = false;
  }
  #endRegion
}
```
##### Step 2 (Android)
###### Create/Add below files in your Android project

1. Create an Activity in which you want to authenticate the user, which inherits MvxActivity

```C#
public class AuthenticationView : MvxActivity
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
//check for fingerprint and authenticate
var authenticationObj = new 		FingerprintAuthentication();
authenticationObj.fingerPrintDetection(this);
};

}
}
```

2. Create FingerPrintAuthentication class, where will check the hardware compatibility, touch id enrollment state and fingerprint scanning

```C#
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
// No permission. Go and ask for permissions and don't start the scanner
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
```

3. CryptoObjectHelper class needs to be created to generate AppStoreKey which is used for authentication. The class looks like below:

```C#
public class CryptoObjectHelper
{
// This can be key name you want. Should be unique for the app.
static readonly string KEY_NAME = "com.innomind.safetyfeatures.sample_fingerprint_key";

// We always use this keystore on Android.
static readonly string KEYSTORE_NAME = "AndroidKeyStore";

// Should be no need to change these values.
static readonly string KEY_ALGORITHM = KeyProperties.KeyAlgorithmAes;
static readonly string BLOCK_MODE = KeyProperties.BlockModeCbc;
static readonly string ENCRYPTION_PADDING = KeyProperties.EncryptionPaddingPkcs7;
static readonly string TRANSFORMATION = KEY_ALGORITHM + "/" +
BLOCK_MODE + "/" +
ENCRYPTION_PADDING;
readonly KeyStore _keystore;

public CryptoObjectHelper()
{
_keystore = KeyStore.GetInstance(KEYSTORE_NAME);
_keystore.Load(null);
}

public FingerprintManagerCompat.CryptoObject BuildCryptoObject()
{
Cipher cipher = CreateCipher();
return new FingerprintManagerCompat.CryptoObject(cipher);
}

Cipher CreateCipher(bool retry = true)
{
IKey key = GetKey();
Cipher cipher = Cipher.GetInstance(TRANSFORMATION);
try
{
cipher.Init(CipherMode.EncryptMode | CipherMode.DecryptMode, key);
}
catch (KeyPermanentlyInvalidatedException e)
{
_keystore.DeleteEntry(KEY_NAME);
if (retry)
{
CreateCipher(false);
}
else
{
throw new Exception("Could not create the cipher for fingerprint authentication.", e);
}
}
return cipher;
}

IKey GetKey()
{
IKey secretKey;
if (!_keystore.IsKeyEntry(KEY_NAME))
{
CreateKey();
}

secretKey = _keystore.GetKey(KEY_NAME, null);
return secretKey;
}

void CreateKey()
{
KeyGenerator keyGen = KeyGenerator.GetInstance(KeyProperties.KeyAlgorithmAes, KEYSTORE_NAME);
KeyGenParameterSpec keyGenSpec =
new KeyGenParameterSpec.Builder(KEY_NAME, KeyStorePurpose.Encrypt | KeyStorePurpose.Decrypt)
.SetBlockModes(BLOCK_MODE).SetEncryptionPaddings(ENCRYPTION_PADDING)
.SetUserAuthenticationRequired(true)
.Build();
keyGen.Init(keyGenSpec);
keyGen.GenerateKey();
}
}
```

4. Create a callback class to handle the success and error response while scanning fingerprint using FingerprintManagerCompat.AuthenticationCallback

```C#
class SimpleAuthCallbacks : FingerprintManagerCompat.AuthenticationCallback
{
// Can be any byte array, keep unique to application.
static readonly byte[] SECRET_BYTES = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
// The TAG can be any string, this one is for demonstration.
static readonly string TAG = "X:" + typeof(SimpleAuthCallbacks).Name;

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

##### Step 2 (iOS)
###### Create/Add below files in your iOS project

1. Create a ViewController in which you want to authenticate the user, which inherits MvxViewController

```C#
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
//Authentication starts here
TouchIDAuthentication.AuthenticateUser(this);
};
}
}
```

2. Create TouchIDAuthentication class, where will check the hardware compatibility, touch id enrollment state and scann of touch id

```C#
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
//Calling the failure handler
viewModel.OnAuthenticationFailure();
}
}
}
```
* You can download the source code here [Source code link](https://github.com/innominds-mobility/xamarin-security) and check how its implemented in real time to have a better understanding.

### Refernces used to make this:

* [TouchId](https://developer.xamarin.com/guides/ios/platform_features/introduction_to_touchid/) for TouchId Authentication in iOS
* [FingerPrint](https://developer.xamarin.com/guides/android/platform_features/fingerprint-authentication/getting-started/) for FingerPrint Authentication in Android
