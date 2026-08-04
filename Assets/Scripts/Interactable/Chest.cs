using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour , Iinteractable
{
    public Sprite closeSprite;
    public Sprite openSprite;
    public bool isDone = false;
    private SpriteRenderer spriteRenderer;
    public void TriggerAction()
    {
        if (!isDone)
        {
            OpenChest();
        }
       
    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = closeSprite;
    }

    private void OnEnable()
    {
        spriteRenderer.sprite = isDone ? openSprite : closeSprite;
    }

    private void OpenChest()
    {
        spriteRenderer.sprite = openSprite;
        isDone = true;
        gameObject.tag = "Untagged";
    }
}
