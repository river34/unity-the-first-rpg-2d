﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : Character
{
    public static Player Instance;

    public BoolObject PlayerMoving;

    public FloatObject PlayerSpeed;

    public AudioClip ClickSound;

	public event Action Pickup, Cancel;

    private Level.LevelControl control;

    private float joystickIgnore = 0.1f;
	private bool moveLeft, moveRight, moveForward, moveBack, eventPickup, eventCancel = false;
	private bool moveJump, eventJump;

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

        Cancel += OnPlayerCancelHandler;
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

    public void SetSort(bool setSort)
    {
        ResetSortingOrder resetSort = GetComponent<ResetSortingOrder>();
        Debug.Log("resetSort = " + resetSort);
        if (resetSort != null)
        {
            resetSort.enabled = setSort;
        }
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
        float forward = Input.GetAxis("Vertical");
        float side = Input.GetAxis("Horizontal");
		float pickup = Input.GetAxis("Submit");
        float cancel = Input.GetAxis("Cancel");
		float jump = Input.GetAxis ("Jump");

        if (forward < joystickIgnore && forward > -joystickIgnore)
        {
            forward = 0;
            moveForward = false;
            moveBack = false;
        }
        else if (forward <= -joystickIgnore)
        {
            moveForward = false;
            moveBack = true;
        }
        else
        {
            moveForward = true;
            moveBack = false;
        }

        if (side < joystickIgnore && side > -joystickIgnore)
        {
            side = 0;
            moveLeft = false;
            moveRight = false;
        }
        else if (side <= -joystickIgnore)
        {
            moveLeft = true;
            moveRight = false;
        }
        else
        {
            moveLeft = false;
            moveRight = true;
        }

		if (pickup < joystickIgnore && pickup > -joystickIgnore)
        {
            eventPickup = false;
        }
        else
		{
            if (eventPickup == false)
            {
                eventPickup = true;
                if (Pickup != null)
                {
                    Pickup();
                }
                SoundManager.Instance.PlaySFX(ClickSound);
            }
        }

		if (jump < joystickIgnore && jump > -joystickIgnore)
		{
			eventJump = false;
		}
		else
		{
			if (eventJump == false)
			{
				eventJump = true;
                Jump();
			}
		}

        if (cancel < joystickIgnore && cancel > -joystickIgnore)
        {
            eventCancel = false;
        }
        else
        {
            if (eventCancel == false)
            {
                eventCancel = true;
                if (Cancel != null)
                {
                    Cancel();
                }
                SoundManager.Instance.PlaySFX(ClickSound);
            }
        }

        if (control == Level.LevelControl.TwoDirections)
        {
            if (moveLeft)
            {
                Move(MoveDirection.Left);
                if (canMove == true)
                {
                    PlayerMoving.Value = true;
                    PlayerSpeed.Value = -1 * MoveSpeed;
                }
            }
            else if (moveRight)
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
			if (moveLeft) {
				Move (MoveDirection.Left);
				if (canMove == true) {
					PlayerMoving.Value = true;
					PlayerSpeed.Value = -1 * MoveSpeed;
				}
			} else if (moveRight) {
				Move (MoveDirection.Right);
				if (canMove == true) {
					PlayerMoving.Value = true;
					PlayerSpeed.Value = MoveSpeed;
				}
			} else if (moveForward) {
				Move (MoveDirection.Up);
				PlayerMoving.Value = true;
			} else if (moveBack) {
				Move (MoveDirection.Down);
				PlayerMoving.Value = true;
			} else if (eventPickup) {
				
			}
            else
            {
                PlayerMoving.Value = false;
                PlayerSpeed.Value = 0;
            }
        }
    }

    private void OnPlayerCancelHandler()
    {
        Inventory.Instance.RemoveLastItem();
    }
}
