using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class SnailChaseState : BaseState
{
    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        Debug.Log("chasestate");
        currentEnemy.anim.SetBool("PrepareAttack", true);
        currentEnemy.cooldown = false;
        currentEnemy.coolDownTimeCounter = currentEnemy.coolDownTime;
        currentEnemy.nowSpeed = 0;

    }

    public override void LogicUpdate()
    {
        if (currentEnemy.lostTimeCounter <= 0)//设置追击时间
        {
            currentEnemy.SwitchState(NPCState.Patrol);
        }
        if (currentEnemy.lostTimeCounter >= 0)//
        {
            currentEnemy.FoundPlayerPosition();
        }

        if (currentEnemy.cooldown && currentEnemy.isDead != true)
        //if (currentEnemy.prepareAttack)
        {
            Vector2 dir =new Vector2(0, 1);
            currentEnemy.nowSpeed = currentEnemy.runingSpeed;
            currentEnemy.cooldown = false;
            currentEnemy.rb.AddForce(dir * currentEnemy.SnailAttackForce, ForceMode2D.Impulse);
            Debug.Log("冲！！");
            //Debug.Log($"Direction: {dashDirection}, Normalized: {dashDirection.normalized}");
        }
        else
        {
            
        }

    }

    public override void PhysicsUpdate()
    {
        if ((currentEnemy.physicsCheck.touchLeftWall && currentEnemy.faceDir.x < 0) || currentEnemy.physicsCheck.touchRightWall && currentEnemy.faceDir.x > 0)
        {
            currentEnemy.nowSpeed = currentEnemy.normalSpeed;
            currentEnemy.SwitchState(NPCState.Patrol);

        }
    }

    public override void OnExit()
    {
        currentEnemy.lostTimeCounter = currentEnemy.lostTime;
        currentEnemy.anim.SetBool("PrepareAttack", false);
        currentEnemy.anim.SetTrigger("AttackOver");
        currentEnemy.nowSpeed = currentEnemy.normalSpeed;
    }

}
