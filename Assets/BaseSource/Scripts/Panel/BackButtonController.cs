using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackButtonController : MonoBehaviour
{
    public static BackButtonController Instance;
    public bool canBack = false;
    public Stack<IBack> stack = new Stack<IBack>();
    [SerializeField]
    private float timeBetweenBack = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        canBack = true;
    }

    public void UnRegister(IBack panel)
    {
        if (stack.Count>0&&stack.Peek() == panel)
        {
            stack.Pop();
        }
    }
    public void Register(IBack panel,bool setBackManually=false)
    {
        if (!stack.Contains(panel))
        {
            Debug.Log("REG: " + panel.ToString());
            stack.Push(panel);
            canBack = false;
            if (!setBackManually)
            {
                Invoke("SetBack", 1);
            }
        }
    }
    void SetBack()
    {
        canBack = true;
    }

    public void Back()
    {
        if (stack.Count > 0)
        {
            stack.Pop().OnBack();
        }
    }
    float timer = 0;
    private void Update()
    {
        if (canBack &&timer<=0&&Input.GetKeyDown(KeyCode.Escape))
        {
            Back();
            timer = timeBetweenBack;
        }
        else if(timer>0)
        {
            timer -= Time.deltaTime;
        }
    }
}


public interface IBack
{
    void OnBack();
}