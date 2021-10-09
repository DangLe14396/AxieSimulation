using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelLoading : MonoBehaviour
{
    public  static LevelLoading Instance;
  
    public void Init()
    {
        Instance = this;
    }
    public void Active()
    {
        Debug.Log("ACTIVEEEEEEEEEEE");
        gameObject.SetActive(true);
    }
    public void Deactive()
    {
        gameObject.SetActive(false);
    }
}
