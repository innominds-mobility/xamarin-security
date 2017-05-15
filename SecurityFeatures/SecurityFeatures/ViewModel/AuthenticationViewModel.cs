﻿using System;

namespace SecurityFeatures
{
	public class AuthenticationViewModel: BaseViewModel
	{
		public AuthenticationViewModel()
		{
			AuthenticationText = "";
		}

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

		public void OnAuthenticationSuccess()
		{
			AuthenticationStatus = true;
			ShowViewModel<DashboardViewModel>();
		}

		public void OnAuthenticationFailure()
		{
			AuthenticationStatus = false;
		}
	}
}