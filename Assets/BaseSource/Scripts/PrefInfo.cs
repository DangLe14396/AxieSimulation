using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefInfo : MonoBehaviour
{

    public static System.DateTime GetLastStress()
    {
        if (!PlayerPrefs.HasKey("LastStressTime"))
        {
            SetLastStress(System.DateTime.Now);
        }
        return System.DateTime.Parse(PlayerPrefs.GetString("LastStressTime"));
    }
    public static void SetLastStress(System.DateTime time)
    {
        PlayerPrefs.SetString("LastStressTime", time.ToString());
    }

    public static bool IsQuestFinished(int level)
    {
        return PlayerPrefs.GetInt("QuestState" + level, 0)==1;
    }
    public static void SetQuestFinished(int level)
    {
        PlayerPrefs.SetInt("QuestState" + level, 1);
    }
    public static int GetPopCount()
    {
        return PlayerPrefs.GetInt("PopCount", 0);
    }
    public static void SetPopCount(int progress)
    {
        PlayerPrefs.SetInt("PopCount", progress);
    }
    public static int GetGiftProgress()
    {
        return PlayerPrefs.GetInt("Gift", 0);
    }
    public static void SetGiftProgress(int progress)
    {
        progress = progress > 100 ? 100 : progress;
        PlayerPrefs.SetInt("Gift", progress);
    }

    public static int GetStress()
    {
        return PlayerPrefs.GetInt("Stress", 50);
    }
    public static void SetStress(int stress)
    {
        stress = stress > 100 ? 100 : (stress<0?0:stress);
        PlayerPrefs.SetInt("Stress", stress);
    }
    public static int GetHP()
    {
        return PlayerPrefs.GetInt("HP", 5);
    }
    public static void SetHP(int hp)
    {
        PlayerPrefs.SetInt("HP", hp);
    }

    public static int GetAdCount(int id,int type)
    {
        return PlayerPrefs.GetInt("AdCount" + id + "_" + type, 0);
    }
    public static void SetAdCount(int id,int type,int adCount)
    {
        PlayerPrefs.SetInt("AdCount" + id + "_" + type, adCount);
    }


    public static int GetPlayerLevel()
    {
        return PlayerPrefs.GetInt("PlayerLevel" , 0);
    }
    public static void SetPlayerLevel(int current)
    {
        PlayerPrefs.SetInt("PlayerLevel", current);
    }

    public static int GetPlayerRank()
    {
        return PlayerPrefs.GetInt("PlayerRank", 0);
    }
    public static void SetPlayerRank(int current)
    {
        PlayerPrefs.SetInt("PlayerRank", current);
    }

    public static int GetTrialSkin()
    {
        return PlayerPrefs.GetInt("TrialSkin" , -1);
    }
    public static void SetTrialSkin(int id)
    {
        PlayerPrefs.SetInt("TrialSkin", id);
    }
    public static int GetBeforeTrialSkin()
    {
        return PlayerPrefs.GetInt("BeforeTrialSkin", 0);
    }
    public static void SetBeforeTrialSkin(int id)
    {
        PlayerPrefs.SetInt("BeforeTrialSkin", id);
    }
    public static System.DateTime GetLastTrialTime()
    {
        return System.DateTime.Parse(PlayerPrefs.GetString("LastTrialTime"));
    }
    public static void SetLastTrialTime(System.DateTime time)
    {
        PlayerPrefs.SetString("LastTrialTime", time.ToString());
    }
    public static System.DateTime GetLastSpinTime()
    {
        if (!PlayerPrefs.HasKey("LastSpinTime"))
        {
            SetLastSpinTime(System.DateTime.Now.AddDays(-1));
        }
        return System.DateTime.Parse(PlayerPrefs.GetString("LastSpinTime"));
    }
    public static void SetLastSpinTime(System.DateTime time)
    {
        PlayerPrefs.SetString("LastSpinTime", time.ToString());
    }
   

    public static int GetDailyStreak()
    {
        return PlayerPrefs.GetInt("DailyStreek", 0);
    }
    public static void SetDailyStreak(int c)
    {
        PlayerPrefs.SetInt("DailyStreek", c);
    }
    public static System.DateTime GetLastRewardTime()
    {
        if (!PlayerPrefs.HasKey("LastRewardTime"))
        {
            SetNextRewardTime(System.DateTime.Now.AddDays(-2));
        }
        return System.DateTime.Parse(PlayerPrefs.GetString("LastRewardTime"));
    }
    public static void SetNextRewardTime(System.DateTime time)
    {
        PlayerPrefs.SetString("LastRewardTime", time.ToShortDateString());
    }

    public static int GetRewardProgress()
    {
        return PlayerPrefs.GetInt("GiftProgress", 0);
    }
    public static void SetRewardProgress(int c)
    {
        PlayerPrefs.SetInt("GiftProgress", c);
    }
    public static int GetHint()
    {
        return PlayerPrefs.GetInt("TotalHint", 1);
    }
    public static void SetHint(int key)
    {
        PlayerPrefs.SetInt("TotalHint", key);
    }

    public static int GetKey()
    {
        return PlayerPrefs.GetInt("Key", 0);
    }
    public static void SetKey(int key)
    {
        PlayerPrefs.SetInt("Key", key);
    }
    public static float GetLastRefillTime()
    {
        return PlayerPrefs.GetFloat("LastRT", 1);
    }
    public static void SetLastRefillTime(float t)
    {
        PlayerPrefs.SetFloat("LastRT", t);
    }


    public static System.DateTime GetLastDieTime()
    {
        return System.DateTime.Parse(PlayerPrefs.GetString("LastDieTime", System.DateTime.Now.ToString()));
    }
    public static void SetLastDieTime(System.DateTime time )
    {
        PlayerPrefs.SetString("LastDieTime", time.ToString());
    }
    public static bool IsRoundFinished(int level, GameMode mode, int round)
    {
        return PlayerPrefs.GetInt("Round" + level + "_" + mode+"_"+round, 0) == 1;
    }
    public static void SetRoundFinished(int level, GameMode mode,int round, bool active = true)
    {
        PlayerPrefs.SetInt("Round" + level + "_" + mode + "_" + round, active ? 1 : 0);
    }
    public static bool IsHint(int level, GameMode mode = GameMode.IQ)
    {
        return PlayerPrefs.GetInt("Hint" + level + "_" + mode, 0) == 1;
    }
    public static void SetHint(int level, GameMode mode = GameMode.IQ, bool active = true)
    {
        PlayerPrefs.SetInt("Hint" + level + "_" + mode, active ? 1 : 0);
    }
    public static int GetPlayTime(string id = "")
    {
        return PlayerPrefs.GetInt("PlayTime" + id, 0);
    }
    public static int GetMaxRound(int level,GameMode mode)
    {
        int count = 0;
        for(int i = 0; i < 6; i++)
        {
            if (IsRoundFinished(level, mode, i))
            {
                count++;
            }
            else
            {
                return count;
            }
        }

        return count;
    }
    public static int GetRound(int level,GameMode mode)
    {
        return PlayerPrefs.GetInt("Round_"+level+"_"+(int)mode, 0);
    }
    public static void SetRound(int level, GameMode mode,int round)
    {
         PlayerPrefs.SetInt("Round_" + level + "_" + (int)mode, round);
    }
    public static int GetEnergy()
    {
        return Mathf.Max(0, PlayerPrefs.GetInt("Energy", 3));
    }
    public static void SetEnergy(int c)
    {
        PlayerPrefs.SetInt("Energy", c);
    }
    public static void SetPlayTime(int c, string id = "")
    {
        PlayerPrefs.SetInt("PlayTime" + id, c);
    }
    public static void IncreasePlayTime(int am, string id = "")
    {
        PlayerPrefs.SetInt("PlayTime" + id, GetPlayTime(id) + am);
    }
    //
   
    public static int GetSpin()
    {
        return PlayerPrefs.GetInt("Spin", 0);
    }
    public static void SetSpin(int c)
    {
        PlayerPrefs.SetInt("Spin", c);
    }
    public static int GetGem()
    {
        return PlayerPrefs.GetInt("Gem", 0);
    }
    public static void SetGem(int c)
    {
        PlayerPrefs.SetInt("Gem", c);
    }
    public static void AddGem(int c)
    {
        PlayerPrefs.SetInt("Gem", GetGem()+c);
    }

    public static bool GetItemStatus(int id)
    {
        if (id == 0) return true;
        return PlayerPrefs.GetInt("Equipment_" + id , 0) != 0;
    }
    public static void SetItemStatus(int id,bool status=true)
    {
        PlayerPrefs.SetInt("Equipment_" + id , status?1:0);
    }

 
    public static string GetName()
    {
        return PlayerPrefs.GetString("PlayerName","You");
    }
    public static void SetName(string name)
    {
        PlayerPrefs.SetString("PlayerName", name);
    }
    public static bool IsUnlimited()
    {
        return PlayerPrefs.GetInt("UnlimitedHeart", 0) == 1;
    }
    public static void SetUnlimited(bool active = false)
    {
        PlayerPrefs.SetInt("UnlimitedHeart", active ? 1 : 0);
    }
    public static bool IsUsingAd3()
    {
        return PlayerPrefs.GetInt("AdEnabled3", 1) == 1;
    }
    public static void SetAd3()
    {
        PlayerPrefs.SetInt("AdEnabled3", 0);
    }
    public static bool IsUsingAd2()
    {
        return PlayerPrefs.GetInt("AdEnabled2", 1) == 1;
    }
    public static void SetAd2()
    {
        PlayerPrefs.SetInt("AdEnabled2", 0);
    }
    public static bool IsUsingAd()
    {
        //return false;
        return PlayerPrefs.GetInt("AdEnabled", 1) == 1;
    }
    public static void SetAd(bool active=false)
    {
        PlayerPrefs.SetInt("AdEnabled", active ? 1 : 0);
    }
    public static float GetSensitivity()
    {
        return PlayerPrefs.GetFloat("Sensitivity", 0.5f);
    }
    public static void SetSensitivity(float total)
    {
        PlayerPrefs.SetFloat("Sensitivity", total);
    }
    public static int GetCurrentSword()
    {
        return PlayerPrefs.GetInt("Sword", 0);
    }
    public static void SetCurrentSword(int c)
    {
        PlayerPrefs.SetInt("Sword", c);
    }
    public static bool IsSwordUnlocked(int id)
    {
        if (id == 0) return true;
        return PlayerPrefs.GetInt("Sword_" + id, 0) == 0 ? false : true;
    }

    public static void SetSwordUnlocked(int id, bool active)
    {
        PlayerPrefs.SetInt("Sword_"  + id, active ? 1 : 0);
    }

    public static int GetCurrentSkin(int type=0)
    {
        type = 0;
        return PlayerPrefs.GetInt("Skin" + type, 0);
    }
    public static void SetCurrentSkin(int c, int type)
    {
        type = 0;
        PlayerPrefs.SetInt("Skin" + type, c);
    }
   
    public static int GetTotalStar(GameMode mode)
    {
        return PlayerPrefs.GetInt("TotalStar_" + mode, 0);
    }
    public static void SetTotalStar(int totalStar, GameMode mode)
    {
        PlayerPrefs.SetInt("TotalStar_" + mode, totalStar);
    }
    public static void SetTotalUnlockedLevel(int total, GameMode mode)
    {
        PlayerPrefs.SetInt("TotalUnlockedLevel_" + mode, total);
    }
    public static int GetTotalUnlockedLevel(GameMode mode)
    {
        return PlayerPrefs.GetInt("TotalUnlockedLevel_" + mode, (mode==0 || mode == GameMode.Coin) ?1:0);
    }
    public static void SetTotalLevel(int total, GameMode mode)
    {
        PlayerPrefs.SetInt("TotalLevel_" + mode, total);
    }
    public static int GetTotalLevel(GameMode mode)
    {
        return PlayerPrefs.GetInt("TotalLevel_" + mode, 1);
    }

    public static int GetCoin()
    {
        return PlayerPrefs.GetInt("Coin", 0);
    }

    public static void SetCoin(int total)
    {
        PlayerPrefs.SetInt("Coin", total);
    }
    public static void AddCoin(int total)
    {
        PlayerPrefs.SetInt("Coin", GetCoin()+total);
    }

    public static string GetLevelStats(int level, GameMode mode)
    {
        return PlayerPrefs.GetString("Stats_" + level+ "_" + mode, "0" + (level == 1 ? "1" : "0"));
    }
    public static void SetLevelStats(int level, GameMode mode, int totalStar, bool isUnlocked)
    {
        PlayerPrefs.SetString("Stats_" + level + "_" + mode, string.Format("{0}{1}", totalStar, isUnlocked ? 1 : 0));
    }

    public static bool IsUnlocked(int id, int type)
    {
        if (id == 0 && type==0) return true;
        if (id == 0 && type==3) return true;
        //Debug.LogError("item: " + id + " " + type);
        //if (id == 0 && type==100) return true;
        return PlayerPrefs.GetInt("Lock_" + type + "_" + id, 0) == 0 ? false : true;
    }

    public static void SetUnlocked(int id, bool active, int type)
    {
        PlayerPrefs.SetInt("Lock_" + type + "_" + id, active ? 1 : 0);
    }
    public static int GetLandLevel(int id)
    {
        return PlayerPrefs.GetInt("LandLevel_"+id, -1);
    }

    public static void SetLandLevel(int id,int level)
    {
        PlayerPrefs.SetInt("LandLevel_"+id, level);
    }
}
