using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoarChaseState : BaseState
{
    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        Debug.Log("chasestate");
        currentEnemy.nowSpeed = currentEnemy.runingSpeed;
        currentEnemy.anim.SetBool("isRun",true);
    }

    public override void LogicUpdate()
    {
        if (!currentEnemy.physicsCheck.isGround || (currentEnemy.physicsCheck.touchLeftWall && currentEnemy.faceDir.x < 0) || (currentEnemy.physicsCheck.touchRightWall) && currentEnemy.faceDir.x > 0)
        {
            currentEnemy.transform.localScale = new Vector3(currentEnemy.faceDir.x , 1, 1);
        }
        if (currentEnemy.lostTimeCounter <= 0)
        {
            currentEnemy.SwitchState(NPCState.Patrol);
        }
    }

    public override void PhysicsUpdate()
    {
        
    }
    public override void OnExit()
    {
        currentEnemy.anim.SetBool("isRun", false);
        currentEnemy.lostTimeCounter = currentEnemy.lostTime;
    }
}

