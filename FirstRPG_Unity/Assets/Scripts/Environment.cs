using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment : MonoBehaviour {

    public Transform Background;

    public Transform Middleground;

    public Transform Foreground;

    public Transform Ground;

    public BoolObject PlayerMoving;

    public FloatObject PlayerSpeed;

    private void Update()
    {
        if (PlayerMoving.Value == true)
        {
            //Foreground.transform.position += Vector3.left * PlayerSpeed.Value * Time.deltaTime / 2;
            Middleground.transform.position += Vector3.left * PlayerSpeed.Value * Time.deltaTime / 8;
            Background.transform.position += Vector3.left * PlayerSpeed.Value * Time.deltaTime / 16;
        }
    }
}
