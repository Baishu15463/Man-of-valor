using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public PlayerCtrler playerCtrler;
    private Animator anim;//调用Animator的组件使用权
    private Rigidbody2D rb;//因为需要检测Rigidbody组件中的移动方向所以需要调用Rigidbody2D中我们之前写好的函数
    public PhysicsCheck physicsCheck; //因为需要获取之前写好的在PhysicsCheck中的IsGround变量
    private void Awake()
    {
        playerCtrler = GetComponent<PlayerCtrler>();
        anim = GetComponent<Animator>();//使anim变量能够访问Animator组件内的内容
        rb = GetComponent<Rigidbody2D>();//使rb变量能够访问Rigidbody2D组件内的内容
        physicsCheck = GetComponent<PhysicsCheck>();//使physicsCheck变量能够访问组件PhysicsCheck内的内容
    }
    private void Update()//因为执行动画变化是每一帧都需要去检测的，因此我们需要将执行动画的函数放在Update函数中。
    {
        SetAnimation();
    }

    public void SetAnimation()   //因为要执行很多动画的转换，单独写一个公开的函数方法
    {
        anim.SetFloat("velocityX", Mathf.Abs(rb.velocity.x));//用法SetFloat（"希望获取的float名称"，变量）；
        //而其中的Mathf.Abs(变量)是取绝对值的意思Abs是absolute的缩写
        anim.SetFloat("velocityY", rb.velocity.y);
        anim.SetBool("isGround", physicsCheck.isGround);//获取IsGround的布尔值
        anim.SetBool("die", playerCtrler.isDead);
        anim.SetBool("isAttack", playerCtrler.isAttack);
        anim.SetBool("isSlide", playerCtrler.isSlide);
    }

    public void PlayHurt()
    {
        anim.SetTrigger("hurt");
    }

    public void PlayerAttack()
    {
        anim.SetTrigger("attack");
    }
}
