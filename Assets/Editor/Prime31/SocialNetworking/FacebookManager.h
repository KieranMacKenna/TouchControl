//
//  FacebookManager.h
//  Facebook
//
//  Created by prime31
//

#import <Foundation/Foundation.h>
#import <FBSDKCoreKit/FBSDKCoreKit.h>
#import <FBSDKLoginKit/FBSDKLoginKit.h>
#import <FBSDKShareKit/FBSDKShareKit.h>
#import "P31SharedTools.h"



extern NSString *const kFacebookAppIdKey;


@interface FacebookManager : NSObject <FBSDKSharingDelegate, FBSDKAppInviteDialogDelegate, FBSDKGameRequestDialogDelegate>
@property (nonatomic) BOOL hasFacebookId;
@property (nonatomic, copy) NSString *urlSchemeSuffix;
@property (nonatomic, copy) NSString *appLaunchUrl;


+ (FacebookManager*)sharedManager;

+ (FBSDKLoginManager*)sharedLoginManager;


- (void)renewCredentialsForAllFacebookAccounts;


// Composer and share dialog
+ (BOOL)userCanUseFacebookComposer;

- (void)showFacebookComposerWithMessage:(NSString*)message image:(UIImage*)image link:(NSString*)link;


// auth and graph api
- (void)loginWithReadPermissions:(NSMutableArray*)permissions;

- (void)loginWithPublishPermissions:(NSMutableArray*)permissions;

- (void)requestWithGraphPath:(NSString*)path httpMethod:(NSString*)method params:(NSDictionary*)params;

@end
