using UnityEngine;
using System;



namespace Prime31
{
	public partial class TwitterManager : MonoBehaviour
	{
#if UNITY_IOS || UNITY_ANDROID
		/// <summary>
		/// Fired after a successful login attempt was made. Provides the screenname of the user.
		/// </summary>
		public static event Action<string> loginSucceededEvent;

		/// <summary>
		/// Fired when an error occurs while logging in
		/// </summary>
		public static event Action<string> loginFailedEvent;

		/// <summary>
		/// Fired when a custom request completes. The object will be either an IList or an IDictionary
		/// </summary>
		public static event Action<object> requestDidFinishEvent;

		/// <summary>
		/// Fired when a custom request fails
		/// </summary>
		public static event Action<string> requestDidFailEvent;

		/// <summary>
		/// iOS only. Fired when the tweet sheet completes. True indicates success and false cancel/failure.
		/// </summary>
		public static event Action<bool> tweetSheetCompletedEvent;

		/// <summary>
		/// Android only. Fired after the Twitter service is initialized and ready for use.
		/// </summary>
		public static event Action twitterInitializedEvent;



		static TwitterManager()
		{
			AbstractManager.initialize( typeof( TwitterManager ) );
		}


		public static void noop() { }


		public void loginSucceeded( string screenname )
		{
			if( loginSucceededEvent != null )
				loginSucceededEvent( screenname );
		}


		public void loginFailed( string error )
		{
			if( loginFailedEvent != null )
				loginFailedEvent( error );
		}


		public void requestSucceeded( string results )
		{
			if( requestDidFinishEvent != null )
				requestDidFinishEvent( Json.decode( results ) );
		}


		public void requestFailed( string error )
		{
			if( requestDidFailEvent != null )
				requestDidFailEvent( error );
		}


		public void tweetSheetCompleted( string oneOrZero )
		{
			if( tweetSheetCompletedEvent != null )
				tweetSheetCompletedEvent( oneOrZero == "1" );
		}


		public void twitterInitialized()
		{
			if( twitterInitializedEvent != null )
				twitterInitializedEvent();
		}

#endif
	}

}
