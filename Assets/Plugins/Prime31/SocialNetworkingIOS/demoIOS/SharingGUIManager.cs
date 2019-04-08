using UnityEngine;
using System.Collections;
using Prime31;
using System.Collections.Generic;
using System.Linq;




namespace Prime31
{
	public class SharingGUIManager : MonoBehaviourGUI
	{
#if UNITY_IOS
		public static string screenshotFilename = "someScreenshot.png";
		List<string> _facebookAccounts, _twitterAccounts;
		AccountType _pendingAccountType;


		void Start()
		{
			// list for authenticated accounts so we can grab the usernames to perform requests with.
			SharingManager.accessGrantedForAccountsWithUsernamesEvent += usernames =>
			{
				if( _pendingAccountType == AccountType.Twitter )
					_twitterAccounts = usernames;
				else
					_facebookAccounts = usernames;
			};

			// grab a screenshot for later use
			captureScreenshot( screenshotFilename );
		}


		void OnGUI()
		{
			beginColumn();

			GUILayout.Label( "Sharing" );

			if( GUILayout.Button( "Share URL and Text" ) )
			{
				// Note that calling setPopoverPosition is only required on iOS 8+ when on an iPad
				SharingBinding.setPopoverPosition( Input.touches[0].position.x / 2f, ( Screen.height - Input.touches[0].position.y ) / 2f );
				SharingBinding.shareItems( new string[] { "http://prime31.com", "Here is some text with the URL" } );
			}


			if( GUILayout.Button( "Share Text" ) )
			{
				// Note that calling setPopoverPosition is only required on iOS 8+ when on an iPad
				SharingBinding.setPopoverPosition( Input.touches[0].position.x / 2f, ( Screen.height - Input.touches[0].position.y ) / 2f );
				int score = 45454;
				SharingBinding.shareItems( new string[] { string.Format( "Here is some score: {0}", score ) } );
			}


			if( GUILayout.Button( "Share Screenshot" ) )
			{
				var pathToImage = System.IO.Path.Combine( Application.persistentDataPath, screenshotFilename );
				if( !System.IO.File.Exists( pathToImage ) )
				{
					Debug.LogError( "there is no screenshot avaialable at path: " + pathToImage );
					return;
				}

				// Note that calling setPopoverPosition is only required on iOS 8+ when on an iPad
				SharingBinding.setPopoverPosition( Input.touches[0].position.x / 2f, ( Screen.height - Input.touches[0].position.y ) / 2f );
				SharingBinding.shareItems( new string[] { pathToImage } );
			}


			if( GUILayout.Button( "Share Screenshot and Text" ) )
			{
				var pathToImage = System.IO.Path.Combine( Application.persistentDataPath, screenshotFilename );
				if( !System.IO.File.Exists( pathToImage ) )
				{
					Debug.LogError( "there is no screenshot avaialable at path: " + pathToImage );
					return;
				}

				// Note that calling setPopoverPosition is only required on iOS 8+ when on an iPad
				SharingBinding.setPopoverPosition( Input.touches[0].position.x / 2f, ( Screen.height - Input.touches[0].position.y ) / 2f );
				SharingBinding.shareItems( new string[] { pathToImage, "Here is some text with the image", "http://prime31.com" } );
			}


			endColumn( true );


			GUILayout.Label( "Account Access" );

			if( button( "Fetch Twitter Accounts" ) )
			{
				_pendingAccountType = AccountType.Twitter;
				SharingBinding.fetchAccountsWithAccountType( AccountType.Twitter, null );
			}


			if( button( "Fetch Facebook Accounts" ) )
			{
				_pendingAccountType = AccountType.Facebook;

				// replace with your own Facebook app ID!
				var options = SharingBinding.createFacebookOptions( "208082439215773", FBAudienceKey.Everyone, new string[] { "email", "user_friends", "publish_actions" } );
				SharingBinding.fetchAccountsWithAccountType( AccountType.Facebook, options );
			}


			GUILayout.Space( 15 );
			if( _twitterAccounts != null && _twitterAccounts.Count > 0 && button( "Twitter Timeline Request" ) )
			{
				SharingBinding.performRequest( AccountType.Twitter, _twitterAccounts.Last(), "https://api.twitter.com/1.1/statuses/home_timeline.json" );
			}


			if( _twitterAccounts != null && _twitterAccounts.Count > 0 && button( "Twitter Status Update" ) )
			{
				var parameters = new Dictionary<string,string>
				{
					{ "status", "Hi. It's now " + System.DateTime.Now }
				};
				SharingBinding.performRequest( AccountType.Twitter, _twitterAccounts.Last(), "https://api.twitter.com/1.1/statuses/update.json", SharingRequestMethod.Post, parameters );
			}


			GUILayout.Space( 15 );
			if( _facebookAccounts != null && _facebookAccounts.Count > 0 && button( "Facebook me Request" ) )
			{
				SharingBinding.performRequest( AccountType.Facebook, _facebookAccounts.Last(), "https://graph.facebook.com/me" );
			}


			if( _facebookAccounts != null && _facebookAccounts.Count > 0 && button( "Facebook Post Message + Link" ) )
			{
				var parameters = new Dictionary<string,string>
				{
					{ "message", "Hi. It's now " + System.DateTime.Now },
					{ "link", "https://prime31.com" }
				};
				SharingBinding.performRequest( AccountType.Facebook, _facebookAccounts.Last(), "https://graph.facebook.com/me/feed", SharingRequestMethod.Post, parameters );
			}


			if( _facebookAccounts != null && _facebookAccounts.Count > 0 && button( "Facebook me/permissions Request" ) )
			{
				SharingBinding.performRequest( AccountType.Facebook, _facebookAccounts.Last(), "https://graph.facebook.com/me/permissions" );
			}

			endColumn();


			if( bottomRightButton( "Facebook..." ) )
			{
				loadLevel( "FacebookTestScene" );
			}
		}

#endif
	}

}
