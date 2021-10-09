using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deactivehandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Deactive()
    {
        gameObject.SetActive(false);
    }
}
