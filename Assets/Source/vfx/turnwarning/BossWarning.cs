using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class BossWarning : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI turnText;
    void Start()
    {
        
    }
    public void Show(int round)
    {
        turnText.text = "ROUND " + round;
        gameObject.SetActive(true);

    }
  
    public void Deactive()
    {
        gameObject.SetActive(false);
    }
   
    
}
