using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetSortingOrder : MonoBehaviour
{
    SpriteRenderer[] rends;

    private void Start()
    {
        rends = GetComponentsInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        if (rends != null)
        {
            foreach (SpriteRenderer rend in rends)
            {
                rend.sortingOrder = Mathf.RoundToInt(rend.transform.position.y * 100f) * -1;
            }
        }
    }
}
