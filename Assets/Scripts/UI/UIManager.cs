using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public PlayerStateBar playerStateBar;
    [Header("ÊÂ¼þ¼àÌý")]
    public CharacterEventSO healthEvent;
    public CharacterEventSO powerEvent;
    private void OnEnable()
    {
        healthEvent.OnEventRaised += OnHealthEvent;
        powerEvent.OnEventRaised += OnPowerEvent;
    }

    private void OnDisable()
    {
        healthEvent.OnEventRaised -= OnHealthEvent;
        powerEvent.OnEventRaised -= OnPowerEvent;
    }

    private void OnPowerEvent(Character character)
    {
        var percentage = character.nowPower / character.maxPower;
        playerStateBar.OnPowerChange(percentage);
    }

    private void OnHealthEvent(Character character)
    {
        var percentage = character.nowHealth / character.maxHealth;
        playerStateBar.OnHealthChange(percentage);
    }
}
