using System;
namespace SecurityFeatures
{
	public interface IAuthentication
	{
		void OnAuthenticationSuccess();
		void OnAuthenticationFailure();

	}
}
