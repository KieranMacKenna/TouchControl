//
//  FacebookBinding.m
//  Facebook
//
//  Created by prime31
//

#import "FacebookManager.h"
#import <FBSDKCoreKit/FBSDKCoreKit.h>
#import <FBSDKLoginKit/FBSDKLoginKit.h>
#import <FBSDKShareKit/FBSDKShareKit.h>
#import "P31SharedTools.h"


// Converts NSString to C style string by way of copy (Mono will free it)
#define MakeStringCopy( _x_ ) ( _x_ != NULL && [_x_ isKindOfClass:[NSString class]] ) ? strdup( [_x_ UTF8String] ) : NULL

// Converts C style string to NSString
#define GetStringParam( _x_ ) ( _x_ != NULL ) ? [NSString stringWithUTF8String:_x_] : [NSString stringWithUTF8String:""]

// Converts C style string to NSString as long as it isnt empty
#define GetStringParamOrNil( _x_ ) ( _x_ != NULL && strlen( _x_ ) ) ? [NSString stringWithUTF8String:_x_] : nil



void _facebookInit()
{
	// check for a URL scheme
	NSDictionary *dict = [[NSBundle mainBundle] infoDictionary];
	if( ![[dict allKeys] containsObject:@"CFBundleURLTypes"] )
		NSLog( @"ERROR: You have not setup a URL scheme. Authentication via Safari or the Facebook.app will not work" );
}


const char* _facebookGetAppLaunchUrl()
{
	NSString *url = [FacebookManager sharedManager].appLaunchUrl;
	if( url )
		return MakeStringCopy( url );
	return MakeStringCopy( @"" );
}


void _facebookSetSessionLoginBehavior( int behavior )
{
	[FacebookManager sharedLoginManager].loginBehavior = (FBSDKLoginBehavior)behavior;
}


void _facebookSetDefaultAudience( int audience )
{
	[FacebookManager sharedLoginManager].defaultAudience = (FBSDKDefaultAudience)audience;
}


void _facebookRenewCredentialsForAllFacebookAccounts()
{
	[[FacebookManager sharedManager] renewCredentialsForAllFacebookAccounts];
}


bool _facebookIsLoggedIn()
{
	return [FBSDKAccessToken currentAccessToken] != nil;
}


const char* _facebookGetFacebookAccessToken()
{
	return MakeStringCopy( [FBSDKAccessToken currentAccessToken].tokenString );
}


const char* _facebookGetSessionPermissions()
{
	NSString *permissionsString = [P31 jsonStringFromObject:[FBSDKAccessToken currentAccessToken].permissions.allObjects];
	return MakeStringCopy( permissionsString );
}


void _facebookLoginWithReadPermissions( const char* perms )
{
	NSString *permsString = GetStringParam( perms );
	NSMutableArray *permissions = nil;
	
	if( permsString.length == 0 )
		permissions = [NSMutableArray array];
	else
		permissions = [[permsString componentsSeparatedByString:@","] mutableCopy];
	
	[[FacebookManager sharedManager] loginWithReadPermissions:permissions];
}


void _facebookLoginWithPublishPermissions( const char* perms )
{
	NSString *permsString = GetStringParam( perms );
	NSMutableArray *permissions = nil;

	if( permsString.length == 0 )
		permissions = [NSMutableArray array];
	else
		permissions = [[permsString componentsSeparatedByString:@","] mutableCopy];

	[[FacebookManager sharedManager] loginWithPublishPermissions:permissions];
}


void _facebookLogout()
{
	[[FacebookManager sharedLoginManager] logOut];
}


void _facebookGraphRequest( const char* graphPath, const char* httpMethod, const char* jsonDict )
{
	// make sure we have a legit dictionary
	NSString *jsonString = GetStringParam ( jsonDict );
	NSDictionary *dict = (NSDictionary*)[P31 objectFromJsonString:jsonString];
	
	if( ![dict isKindOfClass:[NSDictionary class]] )
	{
		NSLog( @"aborting request. Data was not convertable to an NSDictionary" );
		return;
	}
	
	[[FacebookManager sharedManager] requestWithGraphPath:GetStringParam( graphPath )
											   httpMethod:GetStringParam( httpMethod )
												   params:dict];
}



// Facebook Composer and share dialog methods
bool _facebookCanUserUseFacebookComposer()
{
	return [FacebookManager userCanUseFacebookComposer];
}


void _facebookShowFacebookComposer( const char* message, const char* imagePath, const char* link )
{
	NSString *path = GetStringParamOrNil( imagePath );
	UIImage *image = nil;
	if( [[NSFileManager defaultManager] fileExistsAtPath:path] )
		image = [UIImage imageWithContentsOfFile:path];
	
	[[FacebookManager sharedManager] showFacebookComposerWithMessage:GetStringParam( message ) image:image link:GetStringParamOrNil( link )];
}


void _facebookShowFacebookShareDialog( const char* json )
{
	// make sure we have a legit dictionary
	NSString *jsonString = GetStringParamOrNil( json );
	
	if( jsonString && jsonString.length && [jsonString isKindOfClass:[NSString class]] )
	{
		NSMutableDictionary *dict = [(NSDictionary*)[P31 objectFromJsonString:jsonString] mutableCopy];
		if( [dict isKindOfClass:[NSDictionary class]] )
		{
			// translate the dict to FBSDKShareLinkContent
			FBSDKShareLinkContent *content = [[FBSDKShareLinkContent alloc] init];
			
			// valid keys: description, title, contentURL, imageURL
			NSArray *allKeys = [dict allKeys];
			if( [allKeys containsObject:@"contentURL"] )
				content.contentURL = [NSURL URLWithString:dict[@"contentURL"]];
			
			if( [allKeys containsObject:@"quote"] )
				content.quote = dict[@"quote"];
			
			//			[FBSDKShareDialog showFromViewController:[P31 unityViewController]
			//										 withContent:content
			//											delegate:[FacebookManager sharedManager]];
			
			FBSDKShareDialog* dialog = [[FBSDKShareDialog alloc] init];
			dialog.mode = (FBSDKShareDialogMode)[dict[@"mode"] intValue]; // or FBSDKShareDialogModeBrowser for Safari
			dialog.shareContent = content;
			dialog.fromViewController = [P31 unityViewController];
			dialog.delegate = [FacebookManager sharedManager];
			[dialog show];
		}
	}
	else
	{
		NSLog( @"failed to show share dialog due to null or invalid parameters: %@", jsonString );
	}
}


void _facebookShowFacebookShareDialogWithImage( const char* imagePath, const char* caption )
{
	NSString *path = GetStringParamOrNil( imagePath );
	UIImage *image = nil;
	if( [[NSFileManager defaultManager] fileExistsAtPath:path] )
		image = [UIImage imageWithContentsOfFile:path];
	
	if( image == nil )
	{
		NSLog( @"no valid imagePath provided!" );
		return;
	}
	
	FBSDKSharePhoto* photoShare = [FBSDKSharePhoto photoWithImage:image userGenerated:YES];
	photoShare.caption = GetStringParamOrNil( caption );
	
	FBSDKSharePhotoContent* content = [[FBSDKSharePhotoContent alloc] init];
	content.photos = @[photoShare];
	
	FBSDKShareDialog* dialog = [[FBSDKShareDialog alloc] init];
	dialog.mode = FBSDKShareDialogModeNative;
	dialog.shareContent = content;
	dialog.fromViewController = [P31 unityViewController];
	dialog.delegate = [FacebookManager sharedManager];
	[dialog show];
}


void _facebookShowGameRequestDialog( const char* json )
{
	// make sure we have a legit dictionary
	NSString* jsonString = GetStringParamOrNil( json );
	NSDictionary* dict = nil;

	if( jsonString && jsonString.length && [jsonString isKindOfClass:[NSString class]] )
	{
		dict = (NSDictionary*)[P31 objectFromJsonString:jsonString];
		if( ![dict isKindOfClass:[NSDictionary class]] )
		{
			NSLog( @"did not receive a dictionary. aborting." );
			return;
		}
	}

	NSArray* dictKeys = dict.allKeys;
	FBSDKGameRequestContent* content = [[FBSDKGameRequestContent alloc] init];

	// for now, sensible defaults. we'll expose these later
	content.filters = FBSDKGameRequestFilterNone;
	content.actionType = FBSDKGameRequestActionTypeNone;

	if( [dictKeys containsObject:@"objectId"] )
		content.objectID = dict[@"objectId"]; // required for actions: Send or AskFor

	if( [dictKeys containsObject:@"data"] )
		content.data = dict[@"data"];

	if( [dictKeys containsObject:@"actionType"] && dict[@"actionType"] != [NSNull null] && [dict[@"actionType"] length] > 0 )
	{
		NSLog( @"setting GameRequestContent.actionType: %@", dict[@"actionType"] );
		if( [dict[@"actionType"] isEqualToString:@"Send"] )
			content.actionType = FBSDKGameRequestActionTypeSend;
		else if( [dict[@"actionType"] isEqualToString:@"AskFor"] )
			content.actionType = FBSDKGameRequestActionTypeAskFor;
		else if( [dict[@"actionType"] isEqualToString:@"Turn"] )
			content.actionType = FBSDKGameRequestActionTypeTurn;
	}

	content.message = dict[@"message"];
	content.title = dict[@"title"];

	if( [dictKeys containsObject:@"recipients"] )
		content.recipients = dict[@"recipients"];

	if( [dictKeys containsObject:@"recipientSuggestions"] )
		content.recipientSuggestions = dict[@"recipientSuggestions"]; // cant be used with filters

	FBSDKGameRequestDialog* dialog = [[FBSDKGameRequestDialog alloc] init];
	dialog.content = content;
	dialog.frictionlessRequestsEnabled = [dict[@"frictionlessRequestsEnabled"] boolValue];
	dialog.delegate = [FacebookManager sharedManager];
	[dialog show];
}



// App Events
void _facebookLogEvent( const char* event )
{
	[FBSDKAppEvents logEvent:GetStringParam( event )];
}


void _facebookLogPurchaseEvent( double amount, const char* currency )
{
	[FBSDKAppEvents logPurchase:amount currency:GetStringParam( currency )];
}


void _facebookLogEventWithParameters( const char* event, const char* json )
{
	NSString *jsonString = GetStringParamOrNil( json );
	
	if( jsonString && jsonString.length && [jsonString isKindOfClass:[NSString class]] )
	{
		NSDictionary *dict = (NSDictionary*)[P31 objectFromJsonString:jsonString];
		if( [dict isKindOfClass:[NSDictionary class]] )
		{
			[FBSDKAppEvents logEvent:GetStringParam( event ) parameters:dict];
		}
	}
}


void _facebookLogEventAndValueToSum( const char* event, double valueToSum )
{
	[FBSDKAppEvents logEvent:GetStringParam( event ) valueToSum:valueToSum];
}


void _facebookLogEventAndValueToSumWithParameters( const char* event, double valueToSum, const char* json )
{
	NSString *jsonString = GetStringParamOrNil( json );
	
	if( jsonString && jsonString.length && [jsonString isKindOfClass:[NSString class]] )
	{
		NSDictionary *dict = (NSDictionary*)[P31 objectFromJsonString:jsonString];
		if( [dict isKindOfClass:[NSDictionary class]] )
		{
			[FBSDKAppEvents logEvent:GetStringParam( event ) valueToSum:valueToSum parameters:dict];
		}
	}
}

