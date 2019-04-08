//
//  P31Unity.m
//  P31SharedTools
//
//  Created by prime31
//

#import "P31Unity.h"



#ifdef __cplusplus
extern "C" {
#endif
	void UnitySendMessage( const char * className, const char * methodName, const char * param );
#ifdef __cplusplus
}
#endif


void UnityPause( int pause );


@implementation P31Unity

+ (void)unityPause:(NSNumber*)shouldPause
{
	UnityPause( shouldPause.intValue );
}

@end
