using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prime31;
using Facebook.Unity;

public class SocialManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
//        FacebookCombo.init();
           FB.Init(null,null,null);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onFacebookShareBtn()
    {
       /* var content = new FacebookShareContent
        {
               quote = "any message you can share facebook"
        };
        
        FacebookCombo.showFacebookShareDialog(content);*/
       
       FB.ShareLink(new Uri("https://developers.facebook.com/"), callback:null);
    }
}
