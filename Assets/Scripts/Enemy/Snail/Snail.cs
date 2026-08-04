using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Snail : Enemy
{
    protected override void Awake()
    {
        base.Awake(); 
        patrolState = new SnailPatrolState();
        chaseState = new SnailChaseState();
    }

    //protected override void Update()
    //{
    //    base.Update();


    //}

}
