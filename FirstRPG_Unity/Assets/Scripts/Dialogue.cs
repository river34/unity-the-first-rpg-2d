using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue : RPGEvent
{
    public bool FromPlayer;

    [TextArea(3, 10)]
    public string[] Sentences;
}
