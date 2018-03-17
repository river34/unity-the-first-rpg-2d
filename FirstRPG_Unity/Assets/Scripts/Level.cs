using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public BoolObject InLoading;

    public FloatObject LoadingTime;

    public Transform StartPosition;

    public enum LevelControl { TwoDirections, FourDirections};

    public enum LevelCameraFollow { Scrolling, Following };

    public LevelControl ControlMode;

    public LevelCameraFollow CameraFollowMode;

    public bool UsePhysics;

    private EnvironmentCard[] envs;

    public void StartLevel()
    {
        //gameObject.SetActive(true);

        //Player.Instance.transform.position = StartPosition.position;
        //Player.Instance.SetControl(ControlMode);
        //Player.Instance.SetPhysics(UsePhysics);

        //CameraFollow.Instance.SetCameraFollow(CameraFollowMode);

        EnvironmentCard[] cards = GetComponentsInChildren<EnvironmentCard>();
        if (cards.Length > 0)
        {
            foreach (EnvironmentCard card in cards)
            {
                card.enabled = false;
            }
        }

        gameObject.SetActive(true);

        envs = GetComponentsInChildren<EnvironmentCard>(true);
        if (envs.Length > 0)
        {
            foreach (EnvironmentCard env in envs)
            {
                env.gameObject.SetActive(false);
            }
        }

        StartCoroutine(WaitAndStart(LoadingTime.Value / 2));
    }

    public void StopLevel()
    {
        //gameObject.SetActive(false);

        InLoading.Value = true;
        StartCoroutine(WaitAndStop(LoadingTime.Value / 2));
    }

    IEnumerator WaitAndStart(float delay)
    {
        yield return new WaitForSeconds(delay);

        gameObject.SetActive(true);

		envs = GetComponentsInChildren<EnvironmentCard>(true);
		if (envs.Length > 0)
		{
			foreach (EnvironmentCard env in envs)
			{
				env.gameObject.SetActive(true);
			}
		}

        Player.Instance.transform.position = StartPosition.position;
        Player.Instance.SetControl(ControlMode);
        Player.Instance.SetPhysics(UsePhysics);

        CameraFollow.Instance.SetCameraFollow(CameraFollowMode);

        Item[] items = GetComponentsInChildren<Item>(true);
        if (items.Length > 0)
        {
            foreach (Item item in items)
            {
                item.Reset();
            }
        }

        EnvironmentCard[] cards = GetComponentsInChildren<EnvironmentCard>(true);
        if (cards.Length > 0)
        {
            foreach (EnvironmentCard card in cards)
            {
                card.enabled = true;
            }
        }
    }

    IEnumerator WaitAndStop(float delay)
    {
        yield return new WaitForSeconds(delay);

        gameObject.SetActive(false);
    }
}
