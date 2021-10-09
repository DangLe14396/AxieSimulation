using UnityEngine;
using System.Collections;

public static class Vibration
{

    public static void Vibrate()
    {
      
    }


    public static void Vibrate(long milliseconds)
    {
       
    }

  
    public static bool HasVibrator()
    {
        return isAndroid();
    }

    public static void Cancel()
    {
      
    }

    private static bool isAndroid()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
	return true;
#else
        return false;
#endif
    }
}