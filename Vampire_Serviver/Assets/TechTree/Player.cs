using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    
    public float HP;
    public float MaxHP;

    public float MoveSpeed = 2;
    public float XP;
    public float MaxXP = 100;
    private int AddLevelXP = 5;

    public int AttackDamage;

    public int Level = 1;
    public int SelectCount;

    public float attackSpeed = 1;

    [SerializeField] private float attackCoolTime;
    private float curAttackTime = 0;

    public static Player Instance { get; private set; }

    [SerializeField] private float attackRange = 5;

    

    private void Start()
    {
        if(Instance == null) Instance = this;
        else Destroy(this.gameObject);
        attackCoolTime = 1 / attackCoolTime;
        MaxHP = HP;
        XP = 0;

    }
    void Update()
    {
        var dir = new Vector3(JoyStick.stick.Value.x, 0, JoyStick.stick.Value.y);
        dir = transform.TransformDirection(dir);
        transform.Translate(dir * MoveSpeed * Time.deltaTime, Space.World);
        Attack();
    }
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

    void Attack()
    {
        curAttackTime += Time.deltaTime;
        Transform nearestEnemy = GetNearestEnemy();
        if (curAttackTime > attackCoolTime)
        {
            curAttackTime = 0;
            GameObject bullet = ObjectPool.GetPoolObject("PlayerBullet");
            bullet.transform.position = transform.position;
           
            Debug.Log(nearestEnemy.position);
            Vector3 dir = nearestEnemy.position - transform.position;
            dir = dir.normalized;

            float angle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg + 90;
           
            bullet.transform.rotation = Quaternion.Euler(0, angle, 0);
            
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
        if(HP <= 0)
        {
            Death();
        }
    }

    void Death()
    {
        
        gameObject.SetActive(false);
    }
}
