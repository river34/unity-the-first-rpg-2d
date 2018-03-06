using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public static CameraFollow Instance;

    private Transform player;
    private Level.LevelCameraFollow cameraFollowMode;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

        DontDestroyOnLoad(this);

        player = Player.Instance.transform;
    }

    private void Update()
    {
        Vector3 position = transform.position;
        if (cameraFollowMode == Level.LevelCameraFollow.Scrolling)
        {
            position.x = player.position.x;
            transform.position = position;
        }
        else if (cameraFollowMode == Level.LevelCameraFollow.Following)
        {
            position.x = player.position.x;
            position.y = player.position.y;
            transform.position = position;
        }
    }

    public void SetCameraFollow(Level.LevelCameraFollow cameraFollowMode)
    {
        this.cameraFollowMode = cameraFollowMode;
    }
}
