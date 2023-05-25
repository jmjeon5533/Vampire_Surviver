using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Red : EnemyBase
{
    [SerializeField]
    Transform player;

    bool isAttack = false;

    WaitForSeconds attackDelay = new WaitForSeconds(0.5f);
    
    public override void Damaged(float value)
    {
        base.Damaged(value);
    }

    public override void Death()
    {
        base.Death();
    }

    public override void Initialize()
    {
        base.Initialize();
    }

    public override void Attack()
    {
        if(Vector3.Distance(this.transform.position, player.position) < 0.3f && !isAttack)
        {
            StartCoroutine(AttackDelay());
        }
    }

    IEnumerator AttackDelay()
    {
        isAttack = true;
           yield return attackDelay;
        isAttack = false;

    }

    public override void Movement()
    {
        Vector3 dir = player.position - transform.position;
        dir = dir.normalized;

        // 타겟 방향으로 다가감
        transform.position += dir * speed * Time.deltaTime;

        // 타겟 방향으로 회전함
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0,angle,0);
        //transform.Translate(transform.forward * speed * Time.deltaTime);

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!isAttack)
            Movement();
        Attack();
    }
}
