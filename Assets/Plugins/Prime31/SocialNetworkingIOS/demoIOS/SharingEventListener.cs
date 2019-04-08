using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace Prime31
{
	public class SharingEventListener : MonoBehaviour
	{
		#if UNITY_IOS

		// Listens to all the events.  All event listeners MUST be removed before this object is disposed!
		void OnEnable()
		{
			// listen to the events fired by the SharingManager for illustration purposes
			SharingManager.sharingFinishedWithActivityTypeEvent += sharingFinishedWithActivityTypeEvent;
			SharingManager.sharingCancelledEvent += sharingCancelledEvent;
			SharingManager.accessDeniedToAccountsEvent += accessDeniedToAccountsEvent;
			SharingManager.accessGrantedForAccountsWithUsernamesEvent += accessGrantedForAccountsWithUsernamesEvent;
		}


		// Remove all the event handlers when disabled
		void OnDisable()
		{
			SharingManager.sharingFinishedWithActivityTypeEvent -= sharingFinishedWithActivityTypeEvent;
			SharingManager.sharingCancelledEvent -= sharingCancelledEvent;
			SharingManager.accessDeniedToAccountsEvent -= accessDeniedToAccountsEvent;
			SharingManager.accessGrantedForAccountsWithUsernamesEvent -= accessGrantedForAccountsWithUsernamesEvent;
		}


		void sharingFinishedWithActivityTypeEvent( string activityType )
		{
			Debug.Log( "sharingFinishedWithActivityTypeEvent: " + activityType );
		}


		void sharingCancelledEvent()
		{
			Debug.Log( "sharingCancelledEvent" );
		}


		void accessDeniedToAccountsEvent( string error )
		{
			Debug.Log( "accessDeniedToAccountsEvent: " + error );
		}


		void accessGrantedForAccountsWithUsernamesEvent( List<string> usernames )
		{
			Debug.Log( "accessGrantedForAccountsWithUsernamesEvent" );
			Utils.logObject( usernames );
		}

		#endif
	}
}