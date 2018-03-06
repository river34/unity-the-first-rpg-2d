using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTrigger : MonoBehaviour
{
    public Level CurrentLevel;

    public Level NextLeve;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CurrentLevel.StopLevel();
            NextLeve.StartLevel();
        }
    }
}
