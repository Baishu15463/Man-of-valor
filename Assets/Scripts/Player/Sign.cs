using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Sign : MonoBehaviour
{
    private Animator animator;
    public GameObject signSprite; 
    private bool isPlayerInRange = false; // 玩家是否在触发范围内
    public Transform playerTransform; // 玩家位置
    public PlayInputControls playerInput; // 玩家输入组件
    private Iinteractable targetItem; // 当前交互对象
    private void Awake()
    {
        //animator = GetComponentInChildren<Animator>();
        animator = signSprite.GetComponent<Animator>();
        playerInput = new PlayInputControls();
        playerInput.Enable();
        
    }

    private void OnEnable()
    {
        playerInput.Gameplay.Confirm.started += OnConfirm;
    }

    private void OnDisable()
    {
        playerInput.Gameplay.Confirm.started -= OnConfirm;
    }

    private void OnConfirm(InputAction.CallbackContext context)
    {
        if (isPlayerInRange)
        {
            targetItem.TriggerAction();
        }
    }

    private void Update()
    {
        if (isPlayerInRange)
        {
            signSprite.SetActive(true);
            signSprite.transform.localScale = playerTransform.localScale * 0.01f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Interactable"))
        {
            isPlayerInRange = true;
            targetItem = collision.GetComponent<Iinteractable>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
            isPlayerInRange = false;
            signSprite.SetActive(false);
    }
}
