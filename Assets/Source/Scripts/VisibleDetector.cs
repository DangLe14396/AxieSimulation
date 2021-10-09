using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibleDetector : MonoBehaviour
{
    CharacterController cc;
    [SerializeField]
    MeshRenderer mr;
    // Start is called before the first frame update
    void Awake()
    {
        cc = GetComponentInParent<CharacterController>();
        
    }
    private void OnEnable()
    {
        CheckVisible();
    }
    public void CheckVisible()
    {
        if (mr.isVisible)
        {
            OnBecameVisible();
        }
        else
        {
            OnBecameInvisible();
        }
    }
    private void OnBecameVisible()
    {
        cc.OnVisible();
    }
    private void OnBecameInvisible()
    {
        cc.OnInvisible();
    }

}
