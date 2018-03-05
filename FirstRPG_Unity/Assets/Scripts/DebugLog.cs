using UnityEngine;
using System.Collections;

public class DebugLog
{
    public enum LogType { Log, Warning, Debug }

    public static void Print (LogType type, string txt)
    {
        string color = "white";
        switch(type)
        {
            case LogType.Warning:
                color = "red";
                break;
            case LogType.Debug:
                color = "yellow";
                break;
        }
        Debug.LogFormat("<color={0}>{1}</color>", color, txt);
    }
}
