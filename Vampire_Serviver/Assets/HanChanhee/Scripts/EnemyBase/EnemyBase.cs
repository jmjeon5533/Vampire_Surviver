using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour, IEnemyBase
{
    [SerializeField]
    protected float hp;
    [SerializeField]
    protected float maxHp;

    [SerializeField]
    protected float atkDamage;

    [SerializeField]
    protected float speed;

    [SerializeField]
    bool isAlive;
    

    public string Name = "";

    
    public abstract void Attack();
  

    public virtual void Damaged(float value)
    {
       hp-=value;
        if(hp < 0)
        {
            hp = 0;

            Death();
        }
    }

    public virtual void Death()
    {
        isAlive = false;
       
    }

    public float GetHp()
    {
        return hp;
    }

    public float GetMaxHp()
    {
       return maxHp;
    }

    public virtual void Initialize()
    {
        maxHp = hp;
        isAlive = true;
    }

    public bool IsAlive()
    {
        
        return isAlive;
    }

    public abstract void Movement();
    

    public void SetHp(float value)
    {
        hp = value;
    }

    
}
