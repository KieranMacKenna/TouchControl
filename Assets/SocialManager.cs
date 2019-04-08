using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prime31;

public class SocialManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        FacebookCombo.init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onFacebookShareBtn()
    {
        var content = new FacebookShareContent
        {
               quote = "any message you can share facebook"
        };
        
        FacebookCombo.showFacebookShareDialog(content);
    }
}
