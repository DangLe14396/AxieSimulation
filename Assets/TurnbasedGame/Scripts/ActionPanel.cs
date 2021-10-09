using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionPanel : Panel
{
    public static ActionPanel Instance;
    [SerializeField]
    private GameObject moveBtn, attackBtn, minimizeButton;
    public override void PostInit()
    {
        Instance = this;
    }

    public void SetUp(bool moveable,bool attackable)
    {
        moveBtn.SetActive(moveable);
        attackBtn.SetActive(attackable);
        minimizeButton.SetActive(true);
        Show();
    }
    public void Attack()
    {
    }
    public void Move()
    {
    }
    public void StandBy()
    {

    }
    public void Minimize()
    {
    }
    
}
