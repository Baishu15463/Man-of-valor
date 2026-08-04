using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Processors;

public class Enemy : MonoBehaviour
{
    public PhysicsCheck physicsCheck;
    public Rigidbody2D rb; //protected表示仅有该父类的子类可以使用
    public Animator anim;

    [Header("基本参数")]
    public float normalSpeed;
    public float runingSpeed;
    public float nowSpeed;
    public Vector3 faceDir;
    public Transform attacker;
    public float hurtForce;
    public Collider2D collider;
    public Vector3 playerPosition;
    public float SnailAttackForce;

    [Header("计时器")]
    public float waitTime;
    public float waitTimeCounter;
    public bool wait;
    public float isHurtWaitTime;
    public float lostTime;
    public float lostTimeCounter;
    public float coolDownTime;
    public float coolDownTimeCounter;

    [Header("状态")]
    public bool isHurt;
    public bool isDead;
    private BaseState currentState;
    protected BaseState patrolState;
    protected BaseState chaseState;
    public bool cooldown = true;

    [Header("检测")]
    public Vector2 centerOffset;
    public Vector2 checkSize;
    public float checkDistance;
    public LayerMask attackLayer;
    public float checkRadius;
    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        nowSpeed = normalSpeed;
        physicsCheck = GetComponent<PhysicsCheck>();
        waitTimeCounter = waitTime;
        
    }
    private void OnEnable()
    {
        currentState = patrolState;
        currentState.OnEnter(this);
    }

    protected virtual void Update()
    {
        faceDir = new Vector3(-transform.localScale.x, 0, 0);
        currentState.LogicUpdate();
        TimeCounter();
    }

    private void FixedUpdate() 
    {
        if (!isHurt && !isDead&&!wait)
        {
            Move();
        }
        currentState.PhysicsUpdate();
    }
    public virtual void Move()
    {
        rb.velocity = new Vector2(nowSpeed * faceDir.x * Time.deltaTime,rb.velocity.y);
    }

    public void TimeCounter()
    {
        if (wait)
        {
            waitTimeCounter -= Time.deltaTime;
            rb.velocity = new Vector2(0, rb.velocity.y);
            if (waitTimeCounter <= 0)
            {
                wait = false;
                waitTimeCounter = waitTime;
                transform.localScale = new Vector3(faceDir.x, 1, 1);
            }
        }

        if (!FoundPlayer() && coolDownTimeCounter > 0)
        {
            lostTimeCounter -= Time.deltaTime;
        }
        //else
        //{
        //    lostTimeCounter = lostTime;
        //}
        if (!cooldown)
        {
           coolDownTimeCounter -= Time.deltaTime;
            if (coolDownTimeCounter <= 0)
            {
                cooldown = true;
                coolDownTimeCounter = coolDownTime;
            }
        }

        if (!FoundPlayer() && coolDownTimeCounter > 0)
        {
            coolDownTimeCounter -= Time.deltaTime;
        }
    }

    public bool FoundPlayer()
    {
        
        return Physics2D.BoxCast(transform.position + (Vector3)centerOffset, checkSize, 0, faceDir, checkDistance, attackLayer);
        //返回是否检测到玩家
    }
    public void FoundPlayerPosition()
    {
        collider = Physics2D.OverlapCircle(transform.position, checkRadius, attackLayer);
        if (collider != null)
        {
            Transform playerTransform = collider.transform;
            playerPosition = playerTransform.position;
        }
        else
        {
            //Debug.Log("未检测到目标");
        }
}

    public void SwitchState(NPCState state)
    {
        var newState = state switch
        {
            NPCState.Patrol => patrolState,
            NPCState.Chase => chaseState,
            _ => null,
        };
        currentState.OnExit();//退出当前状态
        currentState = newState;//切换当前状态为该方法中的新状态
        currentState.OnEnter(this);//进入新状态
    }
    #region 事件执行方法
    public void OnTakeDamage(Transform attackTrans)
    {
        attacker = attackTrans;
        if(attackTrans.position.x - transform.position.x > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            wait = false;
            waitTimeCounter = waitTime;
        }
        if(attackTrans.position.x - transform.position.x < 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
            wait = false;
            waitTimeCounter = waitTime;
        }
        isHurt = true;
        anim.SetTrigger("isHurt");
        Vector2 dir = new Vector2(transform.position.x - attackTrans.position.x, 0).normalized;
        rb.velocity = new Vector2(0, rb.velocity.y);
        StartCoroutine(OnHurt(dir));
        
    }

    IEnumerator OnHurt(Vector2 dir)
    {
        rb.AddForce(dir * hurtForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(isHurtWaitTime);
        isHurt = false;
    }

    public void OnDie()
    {
        anim.SetBool("isDead", true);
        isDead = true;
        gameObject.layer = 2;
    }

    public void DestroyAfterAnimation()
    {
        if (isDead)
        {
            Destroy(this.gameObject);
        }
    }
    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position + (Vector3)centerOffset+new Vector3(checkDistance*-transform.localScale.x,0), 0.2f);
        Gizmos.DrawWireSphere(transform.position, checkRadius);
    }

}

