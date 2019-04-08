using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using Prime31;



namespace Prime31
{
	public class TwitterGUIManager : Prime31.MonoBehaviourGUI
	{
		bool _hasNativeAccount;
		public string _screenshotFilename = "someScreenshot.png";


#if UNITY_IOS
		void Start()
		{
			// prep a screenshot for later use
			ScreenCapture.CaptureScreenshot( _screenshotFilename );
		}


		// common event handler used for all graph requests that logs the data to the console
		void completionHandler( string error, object result )
		{
			if( error != null )
				Debug.LogError( error );
			else
				Utils.logObject( result );
		}


		void OnGUI()
		{
			beginColumn();

			if( GUILayout.Button( "Init" ) )
			{
				// Replace these with your own CONSUMER_KEY and CONSUMER_SECRET!
				TwitterBinding.init( "I1hxdhKOrQm6IsR0szOxQ", "lZDRqdzWJq3cATgfXMDjk0kaYajsP9450wKXYXAnpw" );
				setTrigger( "init called" );
			}

			// until init is called nothing else is allowed
			if( !checkTrigger( "init called" ) )
			{
				endColumn();
				return;
			}


			GUILayout.Label( "Authentication" );
			if( GUILayout.Button( "Login with Oauth" ) )
			{
				TwitterBinding.showLoginDialog();
			}


			if( GUILayout.Button( "Is Logged In?" ) )
			{
				bool isLoggedIn = TwitterBinding.isLoggedIn();
				Debug.Log( "Twitter is logged in: " + isLoggedIn );
			}


			if( GUILayout.Button( "Logged in Username" ) )
			{
				string username = TwitterBinding.loggedInUsername();
				Debug.Log( "Twitter username: " + username );
			}


			GUILayout.Label( "Tweet Composers" );
			if( GUILayout.Button( "Show Tweet Composer" ) )
			{
				var pathToImage = Path.Combine( Application.persistentDataPath, _screenshotFilename );
				TwitterBinding.showTweetComposer( "I'm posting this from Unity with a fancy image: " + Time.deltaTime, pathToImage );
			}


			if( GUILayout.Button( "Show Tweet Composer VC" ) )
			{
				var pathToVideo = Path.Combine( Application.streamingAssetsPath, "sample-video.mp4" );
				if( !File.Exists( pathToVideo ) )
				{
					Debug.Log( "The video file does not exist at path: " + pathToVideo + ". Make sure you put a video in the StreamingAssets folder to test with named sample-video.mp4!" );
					pathToVideo = null;
				}

				TwitterBinding.showTweetComposerViewController( "I'm posting this from Unity with a video: " + Time.deltaTime, null, pathToVideo );
			}


			endColumn( true );


			GUILayout.Label( "Raw Posting (No UI)" );
			if( GUILayout.Button( "Post Status Update" ) )
			{
				TwitterBinding.postStatusUpdate( "im posting this from Unity: " + Time.deltaTime );
			}


			if( GUILayout.Button( "Post Status Update + Video" ) )
			{
				var pathToVideo = Path.Combine( Application.streamingAssetsPath, "sample-video.mp4" );
				if( !File.Exists( pathToVideo ) )
				{
					Debug.Log( "The video file does not exist at path: " + pathToVideo + ". Make sure you put a video in the StreamingAssets folder to test with named sample-video.mp4!" );
					return;
				}
					
				TwitterBinding.postStatusUpdateWithVideoOrGif( "I'm posting this video from Unity with a video: " + Time.deltaTime, pathToVideo );
			}


			if( GUILayout.Button( "Post Status Update + Image" ) )
			{
				var pathToImage = Path.Combine( Application.persistentDataPath, _screenshotFilename );
				if( !File.Exists( pathToImage ) )
				{
					Debug.LogError( "The file " + pathToImage + " does not exist on disk. Aborting attempting to post it." );
					return;
				}

				// posting an image already saved to disk
				TwitterBinding.postStatusUpdate( "I'm posting this from Unity with a fancy image: " + Time.deltaTime, pathToImage );


				// posting raw image data
				TwitterBinding.postStatusUpdate( "I'm posting a raw image from Unity with a fancy image: " + Time.deltaTime, File.ReadAllBytes( pathToImage ) );
			}


			if( GUILayout.Button( "Send Direct Message" ) )
			{
				TwitterBinding.sendDirectMessage( "@jack", "Sending a DM to someone!" );
			}


			GUILayout.Label( "Raw API Requests" );
			if( GUILayout.Button( "Custom Request" ) )
			{
				var dict = new Dictionary<string,string>();
				dict.Add( "count", "2" );
				TwitterBinding.performRequest( "GET", "1.1/statuses/home_timeline.json", dict );
			}


			if( GUILayout.Button( "Get Home Timeline" ) )
			{
				TwitterBinding.getHomeTimeline();
			}

			endColumn( false );


			if( bottomRightButton( "Sharing..." ) )
			{
				loadLevel( "SharingTestScene" );
			}
		}
#endif
	}

}
