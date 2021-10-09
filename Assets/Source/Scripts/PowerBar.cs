using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PowerBar : MonoBehaviour
{
    [SerializeField]
    private RectTransform indicator;
    [SerializeField]
    private TextMeshProUGUI[] powerTexts;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    Vector2 pos = Vector2.zero;
    public void UpdateProgress(float myPower,float enemyPower)
    {
        powerTexts[0].text = ((int)myPower).ToString();
        powerTexts[1].text = ((int)enemyPower).ToString();
        pos.x = -200+ 400*(myPower / (myPower + enemyPower));
        indicator.anchoredPosition = pos;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
