using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStateBar : MonoBehaviour
{
    public Image healImage;
    public Image healDelayImage;
    public Image powerImage;
    public GameObject playerStateBar;

    public SceneLoadEventSO sceneLoad;

    private void OnEnable()
    {
        sceneLoad.LoadRequestEvent += ResetStateBar;
    }

    private void OnDisable()
    {
        sceneLoad.LoadRequestEvent -= ResetStateBar;
    }

    private void ResetStateBar(GameScenceSO sceneToLoad, Vector3 arg1, bool arg2)
    {
        if(sceneToLoad.sceneType == SceneType.Menu)
        {
            playerStateBar.SetActive(false);
        }
        else
        {
            playerStateBar.SetActive(true);
        }


    }

    private void Update()
    {
        if(healDelayImage.fillAmount > healImage.fillAmount)
        {
            healDelayImage.fillAmount -= Time.deltaTime * 0.5f;
        }
    }
    public void OnHealthChange(float percentage)
    {
        healImage.fillAmount = percentage;
    }

    public void OnPowerChange(float percentage)
    {
        powerImage.fillAmount = percentage;
    }
}
