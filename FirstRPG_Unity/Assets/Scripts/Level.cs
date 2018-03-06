using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour {

    public Transform StartPosition;

    public enum LevelControl { TwoDirections, FourDirections};

    public enum LevelCameraFollow { Scrolling, Following };

    public LevelControl ControlMode;

    public LevelCameraFollow CameraFollowMode;

    public bool UsePhysics;

    public void StartLevel()
    {
        gameObject.SetActive(true);

        Player.Instance.transform.position = StartPosition.position;
        Player.Instance.SetControl(ControlMode);
        Player.Instance.SetPhysics(UsePhysics);

        CameraFollow.Instance.SetCameraFollow(CameraFollowMode);
    }

    public void StopLevel()
    {
        gameObject.SetActive(false);
    }
}
