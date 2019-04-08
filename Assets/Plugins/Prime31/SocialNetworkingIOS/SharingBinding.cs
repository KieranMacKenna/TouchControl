using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Prime31;


#if UNITY_IOS

namespace Prime31
{
	public enum AccountType
	{
		Facebook,
		Twitter,
		SinaWeibo,
		TencentWeibo,
		LinkedIn
	}

	public enum FBAudienceKey
	{
		Everyone,
		Friends,
		OnlyMe
	}

	public enum SharingRequestMethod
	{
		Get,
		Post,
		Put,
		Delete
	}

	public class SharingBinding
	{
	    [DllImport("__Internal")]
	    private static extern void _sharingShareItems( string items, string excludedActivityTypes );

		// Shows the share sheet with the given items. Items can be text, urls or full and proper paths to sharable files
	    public static void shareItems( string[] items )
	    {
	        if( Application.platform == RuntimePlatform.IPhonePlayer )
				_sharingShareItems( Json.encode( items ), null );
	    }


		// Shows the share sheet with the given items with a list of excludedActivityTypes. See Apple's docs for more information on excludedActivityTypes.
	    public static void shareItems( string[] items, string[] excludedActivityTypes )
	    {
	        if( Application.platform == RuntimePlatform.IPhonePlayer )
				_sharingShareItems( Json.encode( items ), Json.encode( excludedActivityTypes ) );
	    }


		[DllImport("__Internal")]
		private static extern void _sharingSetPopoverPosition( float x, float y );

		// iOS 8+ only and iPad only. Sets the popover arrow position for displaying the share sheet. Set this to match your share button location.
		public static void setPopoverPosition( float x, float y )
		{
			if( Application.platform == RuntimePlatform.IPhonePlayer )
				_sharingSetPopoverPosition( x, y );
		}



		[DllImport("__Internal")]
		private static extern void _sharingFetchAccountsWithAccountType( string accountType, string options );

		// Fetches all the accounts of the given AccountType. Results in the accessGrantedForAccountsWithUsernamesEvent or accessDeniedToAccountsEvent firing.
		public static void fetchAccountsWithAccountType( AccountType accountType, Dictionary<string,object> options )
		{
			var account = "com.apple." + accountType.ToString().ToLower();
			if( Application.platform == RuntimePlatform.IPhonePlayer )
				_sharingFetchAccountsWithAccountType( account, Json.encode( options ?? new Dictionary<string,object>() ) );
		}


		[DllImport("__Internal")]
		private static extern void _sharingPerformRequest( string accountType, string username, string url, string requestMethod, string parameters );

		// Performs a request using the specified AccountType and username (retrieved from fetchAccountsWithAccountType). Each service has it's own API format for the url so refer
		// to their documentation directly for details on the url and parameters. Calling this reaults in the requestSucceeded/FailedEvent event firing.
		public static void performRequest( AccountType accountType, string username, string url, SharingRequestMethod requestMethod = SharingRequestMethod.Get, Dictionary<string,string> parameters = null )
		{
			var account = "com.apple." + accountType.ToString().ToLower();
			if( Application.platform == RuntimePlatform.IPhonePlayer )
				_sharingPerformRequest( account, username, url, requestMethod.ToString().ToLower(), Json.encode( parameters ?? new Dictionary<string,string>() ) );
		}


		// Helper method that creates the options Dictionary for a Facebook login. Pass this to fetchAccountsWithAccountType.
		public static Dictionary<string,object> createFacebookOptions( string appId, FBAudienceKey key, string[] permissions )
		{
			string audience = null;
			switch( key )
			{
				case FBAudienceKey.Everyone:
					audience = "everyone";
					break;
				case FBAudienceKey.Friends:
					audience = "friends";
					break;
				case FBAudienceKey.OnlyMe:
					audience = "me";
					break;
			}

			var options = new Dictionary<string,object>();
			options["ACFacebookAppIdKey"] = appId;
			options["ACFacebookPermissionsKey"] = permissions;
			options["ACFacebookAudienceKey"] = audience;

			return options;
		}

	}

}
#endif
