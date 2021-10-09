using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class SpeedSlider : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI speedText;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void OnSpeedChange(float value)
    {
        GameManager.Instance.SetWorldSpeed(value);
       
        if (value == 0)
        {
            speedText.text = "PAUSED";
        }
        else
        {
            float speed = ((int)(value * 10)) / 10f;
            speedText.text = "x" + speed;
        }
    }
    public void OnSliderDown()
    {
        ScrollBack.Instance.active = false;
    }
    public void OnSliderUp()
    {
        ScrollBack.Instance.active = true;
        ScrollBack.Instance.isDown = false;
    }
}
