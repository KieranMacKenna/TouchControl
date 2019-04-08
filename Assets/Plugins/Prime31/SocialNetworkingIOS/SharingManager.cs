using UnityEngine;
using System.Collections;
using System;
using Prime31;
using System.Collections.Generic;



#if UNITY_IOS

namespace Prime31
{
	public class SharingManager : AbstractManager
	{
		// Fired when sharing completes and the user chose one of the sharing options
		public static event Action<string> sharingFinishedWithActivityTypeEvent;

		// Fired when the user cancels sharing without choosing any share options
		public static event Action sharingCancelledEvent;

		// Fired when a call to fetchAccountsWithAccountType completes successfully. Includes all the account names on the device.
		public static event Action<List<string>> accessGrantedForAccountsWithUsernamesEvent;

		// Fired when a call to fetchAccountsWithAccountType fails
		public static event Action<string> accessDeniedToAccountsEvent;

		// Fired when a call to performRequest succeeds. The event includes the response data string unmodifed. Refer to each services
		// documentation for the format/spec of the actual data.
		public static event Action<string> requestSucceededEvent;

		// Fired when a call to performRequest fails. Includes the error message if present.
		public static event Action<string> requestFailedEvent;



		static SharingManager()
		{
			AbstractManager.initialize( typeof( SharingManager ) );
		}


		void sharingFinishedWithActivityType( string activityType )
		{
			sharingFinishedWithActivityTypeEvent.fire( activityType );
		}


		void sharingCancelled( string empty )
		{
			sharingCancelledEvent.fire();
		}


		void accessGrantedForAccountsWithUsernames( string json )
		{
			accessGrantedForAccountsWithUsernamesEvent.fire( Json.decode<List<string>>( json ) );
		}


		void accessDeniedToAccounts( string error )
		{
			accessDeniedToAccountsEvent.fire( error );
		}


		void requestSucceeded( string response )
		{
			requestSucceededEvent.fire( response );
		}


		void requestFailedWithError( string error )
		{
			requestFailedEvent.fire( error );
		}

	}

}
#endif
