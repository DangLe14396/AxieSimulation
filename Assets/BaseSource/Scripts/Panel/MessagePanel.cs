using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MessagePanel : Panel
{
    public static MessagePanel Instance;
    [SerializeField]
    private TextMeshProUGUI messageText,titleText;

    public override void PostInit()
    {
        Instance = this;
    }
    public void SetUp(string message,string title="MESSAGE",bool overrideBack=false)
    {
      
        Material lastMaterial = messageText.fontSharedMaterial;
        this.messageText.text = message;
        titleText.text = title;
        this.overrideBack = overrideBack;
        Show();
        //GameManager.instance.canPlay = false;
    }
    public void ShowComingSoon()
    {
        SetUp("Coming Soon");
    }
    public override void Deactive()
    {
        base.Deactive();

    }
}
