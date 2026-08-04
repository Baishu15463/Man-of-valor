using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.Playables;
using UnityEngine.InputSystem;

public class PlayerCtrler : MonoBehaviour
{
    [Header("监听事件")]
    public SceneLoadEventSO sceneLoadEvent;
    public VoidEventSO afterSceneLoadEvent;

    public PlayerAnimation playerAnimation;
    public PlayInputControls inputControl;//调用先前创建的class PlayInputControls
    public Rigidbody2D rb;
    public Vector2 inputDirection;//创建一个vector2变量用于给角色移动数值赋值
    public Character character;
    public CapsuleCollider2D coll;
    [Header("移动相关基本参数")]
    public Vector2 walkSpeed;
    public float speed;
    public float jumpForce;
    public float slideForce;
    private PhysicsCheck physicsCheck;
    private KeyDownToTalk keyDownToTalk;
    [Header("受击相关参数")]
    public float hurtForce;//定义受到攻击造成的击退的力
    [Header("状态")]
    public bool isHurt;//判断是否受击
    public bool isDead;
    public bool isAttack;
    public bool isSlide; //滑铲
    [Header("物理材质")]
    public PhysicsMaterial2D normal;
    public PhysicsMaterial2D wall;
    public static PlayerCtrler instance;
    [Header("对话状态")]
    public bool talking;
    public UnityEvent OnPowerChange;//定义一个事件，当玩家的能量发生变化时触发

    //[Header("事件")]

    private void Awake()
    {
        playerAnimation = GetComponent<PlayerAnimation>();

        inputControl = new PlayInputControls();//在程序执行之前先调用PlayInputControls

        physicsCheck = GetComponent<PhysicsCheck>();

        keyDownToTalk = GetComponent<KeyDownToTalk>();

        coll = GetComponent<CapsuleCollider2D>();
        //跳跃
        inputControl.Gameplay.Jump.started += Jump;

        //攻击
        inputControl.Gameplay.Attack.started += PlayerAttack;

        inputControl.Gameplay.Slide.started += Slide;

    }

   
    private void OnEnable()
    {
        inputControl.Enable();
        sceneLoadEvent.LoadRequestEvent += OnLoadRequestEvent;
        afterSceneLoadEvent.OnEventRaised += OnAfterSceneLoadEvent;
    }

    private void OnDisable()
    {
        inputControl.Disable();
        sceneLoadEvent.LoadRequestEvent -= OnLoadRequestEvent;
        afterSceneLoadEvent.OnEventRaised -= OnAfterSceneLoadEvent;
    }

    private void OnAfterSceneLoadEvent()
    {
        inputControl.Gameplay.Enable();
    }

    private void OnLoadRequestEvent(GameScenceSO arg0, Vector3 arg1, bool arg2)
    {
        inputControl.Gameplay.Disable();
    }

    private void Update()
    {
        inputDirection = inputControl.Gameplay.Move.ReadValue<Vector2>();
        chackState();
    }


    private void FixedUpdate()
    {
        if (!isHurt&&!isAttack&&!isSlide)
            Move();
        LayerCheak();
    }

    private void Jump(InputAction.CallbackContext obj)
    {
        //Debug.Log("Jump");
        if(physicsCheck.isGround)//检测人物是否在地面，是为真，否为假
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }
    private void Slide(InputAction.CallbackContext obj)
    {
        PlayerSlide();
    }

    private void PlayerAttack(InputAction.CallbackContext obj)
    {
        playerAnimation.PlayerAttack();
        isAttack = true;
    }

    //测试触发器
    private void OnTriggerStay2D(Collider2D other) //other表示其他被碰撞的物体
    {
        //debug.log(other.name);
    }

    public void Move()
    { 
        rb.velocity = new Vector2(speed * Time.deltaTime * inputDirection.x, rb.velocity.y);
        //人物翻转
        int faceDir = (int)transform.localScale.x;
        if (inputDirection.x > 0)
            faceDir = 1;
        if (inputDirection.x<0)  
            faceDir = -1;
        transform.localScale = new Vector3(faceDir,1,1);

        //检测人物是否在地面，若不在地面则不允许跳跃。
    }

    public void GetHurt(Transform attacker)   //受伤击退
    {
        isHurt = true;
        rb.velocity = Vector2.zero;
        Vector2 dir = new Vector2((transform.position.x - attacker.transform.position.x), 0).normalized; //normalized表示将这个vector2类型的变量保留符号取1
        rb.AddForce(dir * hurtForce,ForceMode2D.Impulse);//ForceMode2D.Impulse表示一个2D的瞬时的力。
    }

    public void PlayDie()
    {
        isDead = true;
        inputControl.Gameplay.Disable();
    }

    public void PlayerSlide()
    {
        if(character.nowPower >= 20 && isSlide == false)
        {
            isSlide = true;
            rb.velocity = Vector2.zero;
            Vector2 slideDir = new Vector2((int)transform.localScale.x, 0);
            rb.AddForce(slideDir * slideForce, ForceMode2D.Impulse);
            gameObject.layer = 2;
            OnPowerChange?.Invoke();
        }
        else
        {
            Debug.Log("能量不足，无法滑铲");
        }


    }

    private void chackState()
    {
        coll.sharedMaterial = physicsCheck.isGround ? normal : wall;
    }

    private void LayerCheak()
    {
        if (!isSlide)
        {
            gameObject.layer = 6;
        }
    }
}
