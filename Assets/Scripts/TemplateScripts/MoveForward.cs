using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using udoEventSystem;
using UnityEngine;

public class MoveForward : MonoBehaviour
{
    public float moveSpeed;

    public float gameSlowDownMultiplier = 0.8f;

    [SerializeField] public bool canMove;

    public bool notAffectedByShift;

    public bool localMove;

    public bool moveToTransformForward;

    private float initialSpeed;

    private float gameSlowDownDurationLeft;

    private void Awake()
    {
        initialSpeed = moveSpeed;
    }

    private void OnEnable()
    {
        if (!notAffectedByShift)
        {
            EventManager.Get<StartForwardMovement>().AddListener(GameStarted);
            EventManager.Get<StopForwardMovement>().AddListener(GameStopped);
            EventManager.Get<StartGame>().AddListener(GameStarted);
            EventManager.Get<PauseGame>().AddListener(GameStopped);
            EventManager.Get<SlowTheGameDown>().AddListener(SlowGameDown);
        }
    }

    private void OnDisable()
    {
        if (!notAffectedByShift)
        {
            EventManager.Get<StartForwardMovement>().RemoveListener(GameStarted);
            EventManager.Get<StopForwardMovement>().RemoveListener(GameStopped);
            EventManager.Get<StartGame>().RemoveListener(GameStarted);
            EventManager.Get<PauseGame>().RemoveListener(GameStopped);
            EventManager.Get<SlowTheGameDown>().RemoveListener(SlowGameDown);
        }
    }

    private void GameStarted()
    {
        canMove = true;
    }

    private void GameStopped()
    {
        canMove = false;
    }
    
    private void ToggleCanMove()
    {
        canMove = !canMove;
    }

    private void SlowGameDown(float slowDownDurationToAdd)
    {
        gameSlowDownDurationLeft += slowDownDurationToAdd;
    }

    private void Update()
    {
        if (!canMove) return;

        gameSlowDownDurationLeft -= Time.deltaTime;
        
        if (gameSlowDownDurationLeft > 0)
        {
            moveSpeed = initialSpeed * gameSlowDownMultiplier;
        }
        else
        {
            moveSpeed = initialSpeed;

            gameSlowDownDurationLeft = 0;
        }
        
        if (!notAffectedByShift)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                moveSpeed = initialSpeed * 4;
            }
        }
        
        if (localMove)
        {
            transform.localPosition += Vector3.forward * Time.deltaTime * moveSpeed;    //Move Forward
        }
        else if (moveToTransformForward)
        {
            transform.position += transform.forward * Time.deltaTime * moveSpeed;
        }
        else
        {
            transform.position += Vector3.forward * Time.deltaTime * moveSpeed;    //Move Forward
        }
        
    }
    public void SetSpeed(float speed)
    {
        
        moveSpeed = speed;
        initialSpeed = speed;
    }
    
}
