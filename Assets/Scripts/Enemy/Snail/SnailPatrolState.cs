using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnailPatrolState : BaseState
{
    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.nowSpeed = currentEnemy.normalSpeed;
        currentEnemy.anim.SetBool("isWalk", true);
    }

    public override void LogicUpdate()
    {
        if (currentEnemy.FoundPlayer() && currentEnemy.isDead != true)
        {
            currentEnemy.SwitchState(NPCState.Chase);
        }
        if ( (currentEnemy.physicsCheck.touchLeftWall && currentEnemy.faceDir.x < 0) || currentEnemy.physicsCheck.touchRightWall && currentEnemy.faceDir.x > 0)
        {
            currentEnemy.wait = true;
            currentEnemy.anim.SetBool("isWalk", false);
        }
        else
        {
            currentEnemy.anim.SetBool("isWalk", true);
        }
    }

    public override void PhysicsUpdate()
    {
        
    }

    public override void OnExit()
    {
        currentEnemy.anim.SetBool("isWalk", false);
    }
}
 
