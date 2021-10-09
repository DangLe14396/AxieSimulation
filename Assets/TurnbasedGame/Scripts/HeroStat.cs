using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class HeroStat : ScriptableObject
{
    public int health, damageRange,moveRange;
    public float speed;
    public string nickname = "Axie";
    public HeroClass classType;
}

[System.Serializable]
public enum HeroClass
{
    Frontier,Healer
}
