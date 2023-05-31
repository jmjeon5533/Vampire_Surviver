using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("체력")]
    public float HP;
    public float MaxHP;
    [Header("경험치")]
    public float XP;
    public float MaxXP = 100;
    private int AddLevelXP = 5;
    public int Level = 1;

    [Header("공격 스탯")]
    public float AttackDamage;
    [SerializeField] private float TotalDamage;

    public float attackSpeed;
    public float BulletSpeed = 1;
    public float BulletSize = 1;
    public int BulletAmount = 1;
    [Header("부가 스탯")]
    public float MoveSpeed = 2;
    public int SelectCount;
    [Header("특수 효과")]
    public bool isAtkBulletSize = false;
    public bool isAtkMoveSpeed = false;

    private float curAttackSpeed;
    private float curAttackTime = 0;

    public static Player Instance { get; private set; }

    [SerializeField] private float attackRange = 5;

    private void Start()
    {
        if (Instance == null) Instance = this;
        else Destroy(this.gameObject);
        AtkSpeedInit();
        TotalDamageInit();
        MaxHP = HP;
        XP = 0;

    }



    void Update()
    {
        Move();
        Attack();
    }

    void Move()
    {
        var dir = new Vector3(JoyStick.stick.Value.x, 0, JoyStick.stick.Value.y);
        dir = transform.TransformDirection(dir);
        transform.Translate(dir * MoveSpeed * Time.deltaTime, Space.World);
    }

    public void AtkSpeedInit() => curAttackSpeed = 1 / attackSpeed;
    public void TotalDamageInit() => TotalDamage
    = isAtkBulletSize ? AttackDamage + (BulletSize * 10) : AttackDamage;

   

    public void GetXP(int Value)
    {
        XP += Value;
        while(XP >= MaxXP)
        {
            XP -= MaxXP;
            MaxXP += AddLevelXP;
            Level++;
            SelectCount++;
            CardManager.instance.SelectStart();
        }
        UIManager.instance.UIUpdate();
    }

    public float GetTotalDamage()
    {
        return TotalDamage;
    }


    void Attack()
    {
        curAttackTime += Time.deltaTime;
        if (curAttackTime > curAttackSpeed)
        {
            Transform nearestEnemy = GetNearestEnemy();
            curAttackTime = 0;
            GameObject bullet = ObjectPool.GetPoolObject("PlayerBullet");
            bullet.transform.position = transform.position;
            bullet.GetComponent<Bullet>().speed = BulletSpeed * 2;
           
            var dir = transform.position - nearestEnemy.position;
            var rot = (Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg + 180) % 360;

            // var dir = transform.position - target.transform.position;
            // var rot = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg - 180;
            // Instantiate(bulletPrefeb, transform.position, Quaternion.Euler(0, rot, 0));

            bullet.transform.rotation = Quaternion.Euler(0,rot/2,0);
            
        }
    }

    Transform GetNearestEnemy()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, 10, LayerMask.GetMask("Enemy"));

        Transform nearestEnemy = transform;
        foreach (Collider enemy in hits)
        {

            if (nearestEnemy == transform) nearestEnemy = enemy.transform;
            float prevDistance = Vector3.Distance(transform.position, nearestEnemy.position);
            float newDistance = Vector3.Distance(transform.position, enemy.transform.position);
            if (prevDistance >= newDistance) nearestEnemy = enemy.transform;
            else if (prevDistance < newDistance) continue;
        }
        return nearestEnemy;
    }


    public void Damaged(float value)
    {
        HP -= value;
        DamageUI.Instance.SpawnDamageText(transform, value.ToString(), Color.red);
        if(HP <= 0)
        {
            Death();
        }
    }

    void Death()
    {
        
        gameObject.SetActive(false);
        Time.timeScale = 0f;
    }
}
