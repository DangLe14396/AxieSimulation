using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public static MenuController Instance;
    public Home home;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        home.PostInit();
    }
    public void SetUp()
    {
        home.SetUp();
        home.Show();
    }
}
