using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Red : EnemyBase
{
    private const int Value = -50;
    [SerializeField]
    Transform player;

    bool isAttack = false;

    WaitForSeconds attackDelay = new WaitForSeconds(0.5f);
    
    public override void Damaged(float value)
    {
        base.Damaged(value);
        DamageUI.Instance.SpawnDamageText(transform, Player.Instance.GetTotalDamage().ToString(), Color.white);

    }

    public override void Death()
    {
        base.Death();
        string[] returnName = gameObject.name.Split('(');
        ObjectPool.ReturnPoolObject(this.gameObject, returnName[0]);
        
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
            Player.Instance.Damaged(5);
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
        float angle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0,angle,0);
        //transform.Translate(transform.forward * speed * Time.deltaTime);
        if(Vector3.Distance(Player.Instance.transform.position, transform.position) > 15)
        {
            //int minusPlusX = (player.transform.position.x <= transform.position.x) ? -1 : 1;
            //int minusPlusZ = (player.transform.position.y <= transform.position.y) ? -1 : 1;
            //transform.position = Player.Instance.transform.position + new Vector3((player.transform.position.x + transform.position.x) * minusPlusX, 0, (player.transform.position.z + transform.position.z) * minusPlusZ); 
            Vector3 playerToObject = player.position - transform.position;
            Vector3 targetPosition = player.position + playerToObject.normalized * (15);
            transform.position = targetPosition;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        player = Player.Instance.transform;
        maxHp = hp;
    }

    private void Awake()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Bullet")) {
            ObjectPool.ReturnPoolObject(other.gameObject, "PlayerBullet");
            Damaged(Player.Instance.GetTotalDamage());
            

        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!isAttack)
            Movement();
        Attack();
    }
}
