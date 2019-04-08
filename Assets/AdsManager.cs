using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;
using System;
using GoogleMobileAds.Api;


public class AdsManager : MonoBehaviour
{
	private RewardBasedVideoAd rewardBasedVideo;
	public Text admobCoinsTxt;
	public Text unityCoinsTxt;

	double admobCoins;
	double unityCoins;

	public void Start()
	{
		Advertisement.Initialize ("3089166");

		#if UNITY_ANDROID
		string appId = "ca-app-pub-3940256099942544~3347511713";
		#elif UNITY_IPHONE
		string appId = "ca-app-pub-3940256099942544~1458002511";
		#else
		string appId = "unexpected_platform";
		#endif

		// Initialize the Google Mobile Ads SDK.
		MobileAds.Initialize(appId);

		// Get singleton reward based video ad reference.
		this.rewardBasedVideo = RewardBasedVideoAd.Instance;

		// Called when an ad request has successfully loaded.
		rewardBasedVideo.OnAdLoaded += HandleRewardBasedVideoLoaded;
		// Called when an ad request failed to load.
		rewardBasedVideo.OnAdFailedToLoad += HandleRewardBasedVideoFailedToLoad;
		// Called when an ad is shown.
		rewardBasedVideo.OnAdOpening += HandleRewardBasedVideoOpened;
		// Called when the ad starts to play.
		rewardBasedVideo.OnAdStarted += HandleRewardBasedVideoStarted;
		// Called when the user should be rewarded for watching a video.
		rewardBasedVideo.OnAdRewarded += HandleRewardBasedVideoRewarded;
		// Called when the ad is closed.
		rewardBasedVideo.OnAdClosed += HandleRewardBasedVideoClosed;
		// Called when the ad click caused the user to leave the application.
		rewardBasedVideo.OnAdLeavingApplication += HandleRewardBasedVideoLeftApplication;


        RequestRewardBasedVideo();
	}

	private void RequestRewardBasedVideo()
	{
			#if UNITY_ANDROID
			string adUnitId = "ca-app-pub-3940256099942544/5224354917";
			#elif UNITY_IPHONE
			string adUnitId = "ca-app-pub-3940256099942544/1712485313";
			#else
			strsing adUnitId = "unexpected_platform";
			#endif

			// Create an empty ad request.
			AdRequest request = new AdRequest.Builder().Build();
			// Load the rewarded video ad with the request.
			this.rewardBasedVideo.LoadAd(request, adUnitId);
	}

    // Update is called once per frame
    void Update()
    {
        
    }
	
		public void onUnityReward(){
		var options = new ShowOptions { resultCallback = HandleShowResult };

			if (Advertisement.IsReady ("rewardedVideo")) {
				Advertisement.Show ("rewardedVideo", options);
			}
		}

		private void HandleShowResult(ShowResult result)
		{
			switch (result)
			{
				case ShowResult.Finished:
					Debug.Log ("The ad was successfully shown.");
							//
							// YOUR CODE TO REWARD THE GAMER
							// Give coins etc.
					unityCoins += 10;
					unityCoinsTxt.text = "unity coins : " + unityCoins;
				break;
				case ShowResult.Skipped:
					Debug.Log("The ad was skipped before reaching the end.");
				break;
				case ShowResult.Failed:
					Debug.LogError("The ad failed to be shown.");
				break;
			}
		}

		public void onAdmobReward(){
			if (rewardBasedVideo.IsLoaded()) {
				rewardBasedVideo.Show();
			}
		}

		public void HandleRewardBasedVideoLoaded(object sender, EventArgs args)
		{
			MonoBehaviour.print("HandleRewardBasedVideoLoaded event received");
		}

		public void HandleRewardBasedVideoFailedToLoad(object sender, AdFailedToLoadEventArgs args)
		{
			MonoBehaviour.print("HandleRewardBasedVideoFailedToLoad event received with message: " + args.Message);
		}

		public void HandleRewardBasedVideoOpened(object sender, EventArgs args)
		{
			MonoBehaviour.print("HandleRewardBasedVideoOpened event received");
		}

		public void HandleRewardBasedVideoStarted(object sender, EventArgs args)
		{
			MonoBehaviour.print("HandleRewardBasedVideoStarted event received");
		}

		public void HandleRewardBasedVideoClosed(object sender, EventArgs args)
		{
			MonoBehaviour.print("HandleRewardBasedVideoClosed event received");
			RequestRewardBasedVideo ();
		}

		public void HandleRewardBasedVideoRewarded(object sender, Reward args)
		{
			string type = args.Type;
			double amount = args.Amount;
			MonoBehaviour.print(
			"HandleRewardBasedVideoRewarded event received for "
			+ amount.ToString() + " " + type);

			admobCoins += amount;
			admobCoinsTxt.text = "admob coins : " + admobCoins;
		}

		public void HandleRewardBasedVideoLeftApplication(object sender, EventArgs args)
		{
			MonoBehaviour.print("HandleRewardBasedVideoLeftApplication event received");
		}
}
