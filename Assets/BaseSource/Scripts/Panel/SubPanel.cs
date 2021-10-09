using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubPanel : MonoBehaviour
{

   public virtual void SetUp() { }
    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }
}
