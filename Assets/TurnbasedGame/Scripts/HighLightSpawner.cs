using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighLightSpawner : Spawner
{
    public new static HighLightSpawner Instance;
    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        Init();
    }

    
}
