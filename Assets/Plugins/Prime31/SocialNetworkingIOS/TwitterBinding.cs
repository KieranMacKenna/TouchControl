using UnityEngine;
using System.Collections.Generic;
using System.Runtime.InteropServices;



#if UNITY_IOS

namespace Prime31
{
	public class TwitterBinding
	{
		static TwitterBinding()
		{
			TwitterManager.noop();
		}


		[DllImport( "__Internal" )]
		private static extern void _twitterInit( string consumerKey, string consumerSecret );

		/// <summary>
		/// Initializes the Twitter plugin and sets up the required oAuth information. You must call this before any other methods
		/// are used!
		/// </summary>
		public static void init( string consumerKey, string consumerSecret )
		{
			if( Application.platform == RuntimePlatform.IPhonePlayer )
				_twitterInit( consumerKey, consumerSecret );
		}


		[DllImport( "__Internal" )]
		private static extern bool _twitterIsLoggedIn();

		/// <summary>
		/// Checks to see if there is a currently logged in user
		/// </summary>
		public static bool isLoggedIn()
		{
			if( Application.platform == RuntimePlatform.IPhonePlayer )
				return _twitterIsLoggedIn();
			return false;
		}


		[DllImport( "__Internal" )]
		private static extern string _twitterLoggedInUsername();

		/// <summary>
		/// Retuns the currently logged in user's username
		/// </summary>
		public static string loggedInUsername()
		{
			if( Application.platform == RuntimePlatform.IPhonePlayer )
				return _twitterLoggedInUsername();
			return string.Empty;
		}


		[DllImport( "__Internal" )]
		private static extern void _twitterShowOauthLoginDialog();

		/// <summary>
		/// Shows the login dialog via the Twitter app or in-app browser
		/// </summary>
		public static void showLoginDialog()
		{
			if( Application.platform == RuntimePlatform.IPhonePlayer )
				_twitterShowOauthLoginDialog();
		}


		[DllImport( "__Internal" )]
		private static extern void _twitterLogout();

		/// <summary>
		/// Logs out the current user
		/// </summary>
		public static void logout()
		{
			if( Application.platform == RuntimePlatform.IPhonePlayer )
				_twitterLogout();
		}


		/// <summary>
		/// Posts the status text. Be sure status text is less than 140 characters!
		/// </summary>
		public static void postStatusUpdate( string status )
		{
			if( Application.platform == RuntimePlatform.IPhonePlayer )
			{
				var dict = new Dictionary<string, string>();
				dict.Add( "status", status );
				TwitterBinding.performRequest( "POST", "1.1/statuses/update.json", dict );
			}
		}


		[DllImport( "__Internal" )]
		private static extern void _twitterPostStatusUpdateWithVideo( string status, string videoPath );

		/// <summary>
		/// Posts the status text and a video or gif file. Note that this method requires that a native iOS twitter account is being used.
		/// </summary>
		public static void postStatusUpdateWithVideoOrGif( string status, string videoPath )
		{
			if( Application.platform == RuntimePlatform.IPhonePlayer )
				_twitterPostStatusUpdateWithVideo( status, videoPath );
		}


		[DllImport( "__Internal" )]
		private static extern void _twitterPostStatusUpdateWithImage( string status, string imagePath );

		/// <summary>
		/// Posts the status text and an image. Note that the url will be appended onto the tweet so you don't have the full 140 characters
		/// </summary>
		public static void postStatusUpdate( string status, string pathToImage )
		{
			if( Application.platform == RuntimePlatform.IPhonePlayer )
				_twitterPostStatusUpdateWithImage( status, pathToImage );
		}


		[DllImport( "__Internal" )]
		private static extern void _twitterPostStatusUpdateWithRawImage( string status, byte[] data, int length );

		/// <summary>
		/// Posts the status text and an image. Note that the url will be appended onto the tweet so you don't have the full 140 characters
		/// </summary>
		public static void postStatusUpdate( string status, byte[] imageData )
		{
			if( Application.platform == RuntimePlatform.IPhonePlayer )
				_twitterPostStatusUpdateWithRawImage( status, imageData, imageData.Length );
		}


		/// <summary>
		/// Receives tweets from the users home timeline
		/// </summary>
		public static void getHomeTimeline()
		{
			if( Application.platform == RuntimePlatform.IPhonePlayer )
				TwitterBinding.performRequest( "GET", "1.1/statuses/home_timeline.json", null );
		}


		[DllImport( "__Internal" )]
		private static extern void _twitterPerformRequest( string methodType, string path, string parameters );

		/// <summary>
		/// Performs a request for any available Twitter API methods.  methodType must be either "get" or "post".  path is the
		/// url fragment from the API docs (excluding https:///api.twitter.com) and parameters is a dictionary of key/value pairs
		/// for the given method.  Path must request .json!  See Twitter's API docs for all available methods.
		/// </summary>
		public static void performRequest( string methodType, string path, Dictionary<string, string> parameters )
		{
			if( Application.platform == RuntimePlatform.IPhonePlayer )
				_twitterPerformRequest( methodType, path, parameters != null ? parameters.toJson() : null );
		}


		[DllImport( "__Internal" )]
		private static extern void _twitterSendDM( string message, string to );

		/// <summary>
		/// sends a direct message
		/// </summary>
		/// <param name="message"></param>
		/// <param name="to"></param>
		public static void sendDirectMessage( string to, string message )
		{
			if( Application.platform == RuntimePlatform.IPhonePlayer )
				_twitterSendDM( message, to );
		}


		#region Tweet Sheet methods

		[DllImport( "__Internal" )]
		private static extern void _twitterShowTweetComposer( string status, string imagePath, string url );

		/// <summary>
		/// Shows the tweet composer with the status message and optional image and link
		/// </summary>
		public static void showTweetComposer( string status, string pathToImage = null, string link = null )
		{
			if( Application.platform == RuntimePlatform.IPhonePlayer )
				_twitterShowTweetComposer( status, pathToImage, link );
		}


		[DllImport( "__Internal" )]
		private static extern void _twitterShowTweetComposerViewController( string status, string imagePath, string videoPath );

		/// <summary>
		/// Shows the tweet composer view controller. Note that only an image OR a video can be included, not both.
		/// </summary>
		public static void showTweetComposerViewController( string status, string imagePath = null, string videoPath = null )
		{
			if( Application.platform == RuntimePlatform.IPhonePlayer )
				_twitterShowTweetComposerViewController( status, imagePath, videoPath );
		}

		#endregion

	}

}
#endif
