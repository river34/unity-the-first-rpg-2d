using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform player;

    private void Start()
    {
        player = Player.Instance.transform;
    }

    private void Update()
    {
        Vector3 position = transform.position;
        position.x = player.position.x;
        transform.position = position;
    }
}
