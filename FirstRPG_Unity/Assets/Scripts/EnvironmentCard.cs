using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentCard : MonoBehaviour
{
    public float Width = 20.0f;

    public float CameraWidth = 16.0f;

    public BoolObject PlayerMoving;

    public FloatObject PlayerSpeed;

    public int SpeedDevider = 1;

    private Transform player;
    private float dist1, dist2;
    private EnvironmentCard leftNeighbour, rightNeighbour;
    private float MERCY_DIST = 1;

    private void Start()
    {
        dist1 = Width / 2 - CameraWidth / 2;
        dist2 = Width / 2 + CameraWidth / 2 + MERCY_DIST;
    }

    private void Update()
    {
        if (PlayerMoving.Value == true)
        {
            transform.position += Vector3.left * PlayerSpeed.Value * Time.deltaTime / SpeedDevider;
        }

        if (player == null)
        {
            player = Player.Instance.transform;
        }
        if (Vector2.Distance(transform.position, player.position) > dist1)
        {
            if (player.position.x < transform.position.x)
            {
                if (rightNeighbour == null)
                {
                    GameObject newCardGO = Instantiate(gameObject);
                    newCardGO.transform.SetParent(transform.parent);
                    Vector3 position = transform.position;
                    position.x -= Width;
                    newCardGO.transform.position = position;
                    newCardGO.name = name;
                    rightNeighbour = newCardGO.GetComponent<EnvironmentCard>();
                    rightNeighbour.leftNeighbour = this;
                }
            }
            else
            {
                if (leftNeighbour == null)
                {
                    GameObject newCardGO = Instantiate(gameObject);
                    newCardGO.transform.SetParent(transform.parent);
                    Vector3 position = transform.position;
                    position.x += Width;
                    newCardGO.transform.position = position;
                    newCardGO.name = name;
                    leftNeighbour = newCardGO.GetComponent<EnvironmentCard>();
                    leftNeighbour.rightNeighbour = this;
                }
            }
        }
        if (Vector2.Distance(transform.position, player.position) > dist2)
        {
            if (rightNeighbour != null)
            {
                rightNeighbour.leftNeighbour = null;
            }

            if (leftNeighbour != null)
            {
                leftNeighbour.rightNeighbour = null;
            }

            Destroy(gameObject);
        }
    }
}
