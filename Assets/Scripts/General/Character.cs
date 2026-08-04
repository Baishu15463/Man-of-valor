using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
    [Header("基本属性")]
    public float maxHealth;
    public float nowHealth;
    public float maxPower;
    public float nowPower;
    public UnityEvent<Character> OnHealthChanged; //当血量发生变化时触发的事件，参数为当前角色实例
    public UnityEvent<Character> HavePowerChange;//定义一个事件，当玩家的能量发生变化时触发，参数为当前角色实例,用于调整UI显示
    [Header("事件监听")]
    public VoidEventSO newGameEvent;
    [Header("受伤无敌")]
    public float invincibleTime;
    private float invincibleCounter;
    public bool invincible;
    public UnityEvent<Transform> OnTakeDamage;
    public UnityEvent Ondie;

    private void OnEnable()
    {
        newGameEvent.OnEventRaised += NewGame;
    }

    private void OnDisable()
    {
        newGameEvent.OnEventRaised -= NewGame;
    }

    private void NewGame()
    {
        nowHealth = maxHealth;
        nowPower = maxPower;
        OnHealthChanged?.Invoke(this); //在Start方法中触发OnHealthChanged事件，确保UI等系统在游戏开始时正确显示初始血量 
    }

    private void Update()
    {
        if (invincible)
        {
            invincibleCounter -= Time.deltaTime;
                if (invincibleCounter <= 0)
                    invincible = false;
        }
        PowerRecovery();

    }

    public void TakeDamage(Attack attacker) //此处的（Attack attacker）表示方法重载
    {
        if (invincible)
            return;//此处表示，当此时为无敌状态时，将不再执行下面的函数，直接取消判定；
        //Debug.Log(attacker.atk);
        if (nowHealth >= attacker.atk)
        {
            nowHealth -= attacker.atk; //此处为快速运算符nowHealth -= attacker.atk
            TriggerInvincible();
            //执行受伤
            OnTakeDamage?.Invoke(attacker.transform);//其中的？是检查是否有需要执行的，若没有则省略
        }
        else
        {
            nowHealth = 0;
            //触发死亡
            Ondie?.Invoke();
        }
        OnHealthChanged?.Invoke(this); //当血量发生变化时，触发OnHealthChanged事件，并将当前角色实例作为参数传递
    }

    private void TriggerInvincible()
    {
        if(!invincible)     //如果当前无敌装填为false，则将invincible的状态改为true
        {
            invincible = true;
            invincibleCounter = invincibleTime; //invinciblCounter为创建的计数器，当无敌触发时，计数器中的时间等于无敌时间；
        }
    }

    public void PowerChange()//表示当能量发生变化时触发事件
    {

        nowPower -= 20;
        HavePowerChange.Invoke(this);
    }

    private void PowerRecovery()
    {
        if(nowPower < maxPower)
        nowPower += 5 * Time.deltaTime;
        HavePowerChange.Invoke(this);
    }

}
