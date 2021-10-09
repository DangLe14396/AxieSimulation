using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class SoundPrefab : ScriptableObject
{

    public List<Sound> clips;
    public AudioClip[] coinClips,menuClips,gameClips;
    public AudioClip[] popClips;
    public AudioClip[] pickUpClips;
    public AudioClip[] winClips;
    public AudioClip[] dropClips;
    public AudioClip[] loseClips;
    public AudioClip[] levelUps;
    public SoundPrefab()
    {
    }
    public AudioClip GetPopClip()
    {
        return popClips[Random.Range(0, popClips.Length)];
    }
    public AudioClip GetWinClip()
    {
        return winClips[Random.Range(0, winClips.Length)];
    }
    public AudioClip GetDropClip()
    {
        return dropClips[Random.Range(0, dropClips.Length)];
    }
    public AudioClip GetPickUpClip ()
    {
        return pickUpClips[Random.Range(0, pickUpClips.Length)];
    }

    public AudioClip GetLevelUpClip()
    {
        return levelUps[Random.Range(0, levelUps.Length)];
    }
    public AudioClip GetLoseClip()
    {
        return loseClips[Random.Range(0, loseClips.Length)];
    }
    public AudioClip GetCoinClip()
    {
        return coinClips[Random.Range(0, coinClips.Length)];
    }
    public AudioClip GetClip(SFX name)
    {

        AudioClip clip = null;
       
        for (int i = 0; i < clips.Count; i++)
        {
            if (name.Equals(clips[i].name))
            {
                clip = clips[i].clip;
            }
        }

        return clip;
    }


}
public enum SFX
{
    clickSFX,theme,popUpOpen,popUpClose, backhome, nextLevel,openGame,restart,endgame,endgamefail,firework,clap,
    purchase,win1,win2,lose1,lose2,pinBlock,lavaAppear,dragpin,menutheme,smoke,slice,drop,bodydropchest,switchpad,ice,trap,poisonAppear,buttonAppear,correct,wrong,water,thundercharge,thunderstorm,lightningzap,buildTheme,teleport1,teleport2,comboSlice,
    victory,bonusLevel
}
[System.Serializable]
public class Sound
{
    public AudioClip clip;
    public SFX name;
}