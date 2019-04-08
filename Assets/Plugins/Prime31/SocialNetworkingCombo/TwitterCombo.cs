using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


#if UNITY_IOS || UNITY_ANDROID

#if UNITY_IOS
using TWITTER = Prime31.TwitterBinding;
#else
using TWITTER = Prime31.TwitterAndroid;
#endif


namespace Prime31
{
	public static class TwitterCombo
	{
		private static string _username;


		static TwitterCombo()
		{
			TwitterManager.loginSucceededEvent += username => { _username = username; };
		}


		/// <summary>
		/// Initializes the Twitter plugin and sets up the required oAuth information
		/// </summary>
		public static void init( string consumerKey, string consumerSecret )
		{
			TWITTER.init( consumerKey, consumerSecret );
		}


		/// <summary>
		/// Checks to see if there is a currently logged in user
		/// </summary>
		public static bool isLoggedIn()
		{
			return TWITTER.isLoggedIn();
		}


		/// <summary>
		/// Retuns the currently logged in user's username
		/// </summary>
		public static string loggedInUsername()
		{
			return _username;
		}


		/// <summary>
		/// Logs in the user using oAuth which request displaying the login prompt via an in app browser
		/// </summary>
		public static void showLoginDialog()
		{
			TWITTER.showLoginDialog();
		}


		/// <summary>
		/// Logs out the current user
		/// </summary>
		public static void logout()
		{
			TWITTER.logout();
		}


		/// <summary>
		/// Posts the status text.  Be sure status text is less than 140 characters!
		/// </summary>
		public static void postStatusUpdate( string status )
		{
			TWITTER.postStatusUpdate( status );
		}


		/// <summary>
		/// Posts the status text and an image.  Note that the url will be appended onto the tweet so you don\'t have the full 140 characters
		/// </summary>
		public static void postStatusUpdate( string status, string pathToImage )
		{
#if UNITY_ANDROID
			TWITTER.postStatusUpdate( status, System.IO.File.ReadAllBytes( pathToImage ) );
#elif UNITY_IOS
			TWITTER.postStatusUpdate( status, pathToImage );
#endif
		}


		/// <summary>
		/// Performs a request for any available Twitter API methods. methodType must be either "get" or "post".  path is the
		/// url fragment from the API docs (excluding https:///api.twitter.com) and parameters is a dictionary of key/value pairs
		/// for the given method.  See Twitter's API docs for all available methods
		/// </summary>
		public static void performRequest( string methodType, string path, Dictionary<string,string> parameters )
		{
			TWITTER.performRequest( methodType, path, parameters );
		}

	}

}
#endif
