using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyBase
{
    void Attack();

    void Damaged(float value);

    void Movement();

    void Death();

    bool IsAlive();

    void Initialize();

    float GetHp();

    float GetMaxHp();

    void SetHp(float value);

    
}
