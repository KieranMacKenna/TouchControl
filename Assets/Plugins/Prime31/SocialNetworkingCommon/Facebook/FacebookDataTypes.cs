using System.Collections.Generic;
using System;



namespace Prime31
{
	public class FacebookBaseDTO
	{
		public override string ToString()
		{
			return JsonFormatter.prettyPrint( Json.encode( this ) ) ?? string.Empty;
		}
	}


	public class FacebookFriendsResult : FacebookBaseDTO
	{
		public List<FacebookFriend> data = new List<FacebookFriend>();
		public FacebookResultsPaging paging;
	}


	public class FacebookResultsPaging : FacebookBaseDTO
	{
		public string next;
		public string previous;
	}


	public class FacebookFriend : FacebookBaseDTO
	{
		public string name;
		public string id;
	}


	public class FacebookMeResult : FacebookBaseDTO
	{
		public class FacebookMeHometown : FacebookBaseDTO
		{
			public string id;
			public string name;
		}

		public class FacebookMeLocation : FacebookBaseDTO
		{
			public string id;
			public string name;
		}

		public string id;
		public string name;
		public string first_name;
		public string last_name;
		public string link;
		public string username;
		public FacebookMeHometown hometown;
		public FacebookMeLocation location;
		public string gender;
		public string email;
		public int timezone;
		public string locale;
		public bool verified;
		public DateTime updated_time;
	}


	public enum FacebookGameRequestActionType
	{
		None,
		Send,
		AskFor,
		Turn
	}


	public class FacebookGameRequestContent
	{
		public FacebookGameRequestActionType requestActionType
		{
			set
			{
				switch( value )
				{
				case FacebookGameRequestActionType.AskFor:
				actionType = "AskFor";
				break;
				case FacebookGameRequestActionType.Send:
				actionType = "Send";
				break;
				case FacebookGameRequestActionType.Turn:
				actionType = "Turn";
				break;
				default:
				actionType = string.Empty;
				break;
				}
			}
		}
#pragma warning disable
		string actionType;
#pragma warning restore

		public string title = string.Empty;
		public string message = string.Empty;
		public string data = string.Empty;

		/// <summary>
		/// objectId should only be provided when requestActionType is Send or AskFor
		/// </summary>
		public string objectId = string.Empty;
		public List<string> recipients = new List<string>();
		public List<string> recipientSuggestions = new List<string>();
		// iOS only!
		public bool frictionlessRequestsEnabled = false;
	}


	public enum FacebookShareDialogMode
	{
		/// <summary>
		/// Acts with the most appropriate mode that is available.
		/// </summary>
		Automatic = 0,
		/// <summary>
		/// Displays the dialog in the main native Facebook app.
		/// </summary>
		Native,
		/// <summary>
		/// Displays the dialog in the iOS integrated share sheet.
		/// </summary>
		ShareSheet,
		/// <summary>
		/// Displays the dialog in Safari.
		/// </summary>
		Browser,
		/// <summary>
		/// Displays the dialog in a UIWebView within the app.
		/// </summary>
		Web,
		/// <summary>
		/// Displays the feed dialog in Safari.
		/// </summary>
		FeedBrowser,
		/// <summary>
		/// Displays the feed dialog in a UIWebView within the app.
		/// </summary>
		FeedWeb
	}

	public class FacebookShareContent
	{
		[Obsolete( "Facebook has deprecated this property" )]
		public string title = string.Empty;

		[Obsolete( "Facebook has deprecated this property" )]
		public string description = string.Empty;

		[Obsolete( "Facebook has deprecated this property" )]
		public string imageURL = string.Empty;

		public string contentURL = string.Empty;
		public string quote = string.Empty;
		public FacebookShareDialogMode mode = FacebookShareDialogMode.Automatic;
	}

}
