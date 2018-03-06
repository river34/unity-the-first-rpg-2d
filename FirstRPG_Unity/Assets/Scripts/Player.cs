using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    public static Player Instance;

    public BoolObject PlayerMoving;

    public FloatObject PlayerSpeed;

    private Level.LevelControl control;

    protected override void Start()
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

        base.Start();
        PlayerSpeed.Value = MoveSpeed;
        PlayerMoving.Value = true;
    }

    protected override void Update()
    {
        base.Update();
        CheckInput();
    }

    public void SetControl(Level.LevelControl control)
    {
        this.control = control;
    }

    //public void SetPhysics(bool usePhysics)
    //{
    //    if (usePhysics == true)
    //    {
    //        Rigidbody2D rigid = GetComponent<Rigidbody2D>();
    //        if (rigid != null)
    //        {
    //            rigid.bodyType = RigidbodyType2D.Dynamic;
    //        }
    //    }
    //    else
    //    {
    //        Rigidbody2D rigid = GetComponent<Rigidbody2D>();
    //        if (rigid != null)
    //        {
    //            rigid.bodyType = RigidbodyType2D.Kinematic;
    //        }
    //    }
    //}

    void CheckInput()
    {
        if (control == Level.LevelControl.TwoDirections)
        {
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                Move(MoveDirection.Left);
                if (canMove == true)
                {
                    PlayerMoving.Value = true;
                    PlayerSpeed.Value = -1 * MoveSpeed;
                }
            }
            else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                Move(MoveDirection.Right);
                if (canMove == true)
                {
                    PlayerMoving.Value = true;
                    PlayerSpeed.Value = MoveSpeed;
                }
            }
            else
            {
                PlayerMoving.Value = false;
                PlayerSpeed.Value = 0;
            }
        }
        else if (control == Level.LevelControl.FourDirections)
        {
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                Move(MoveDirection.Left);
                if (canMove == true)
                {
                    PlayerMoving.Value = true;
                    PlayerSpeed.Value = -1 * MoveSpeed;
                }
            }
            else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                Move(MoveDirection.Right);
                if (canMove == true)
                {
                    PlayerMoving.Value = true;
                    PlayerSpeed.Value = MoveSpeed;
                }
            }
            else if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                Move(MoveDirection.Up);
                PlayerMoving.Value = true;
            }
            else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                Move(MoveDirection.Down);
                PlayerMoving.Value = true;
            }
            else
            {
                PlayerMoving.Value = false;
                PlayerSpeed.Value = 0;
            }
        }
    }
}
