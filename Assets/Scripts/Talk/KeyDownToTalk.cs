using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyDownToTalk : MonoBehaviour
{
    public GameObject keyDown;
    public GameObject talkUi;
    public bool isTalk = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        keyDown.SetActive(true);
        isTalk = true;
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        keyDown.SetActive(false);
        isTalk = false;
    }

    private void Update()
    {
        Talking();
        OutToTalk();
    }

    private void Talking()
    {
        if (isTalk && Input.GetKeyDown(KeyCode.R))
        {
            talkUi.SetActive(true);
        }
    }

    private void OutToTalk()
    {
        if (isTalk == false)
            talkUi.SetActive(false);
    }
}
