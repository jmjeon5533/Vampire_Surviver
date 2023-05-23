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

    private void Start()
    {
        MaxHP = HP;
        XP = 0;
    }
    void Update()
    {
        var dir = new Vector3(JoyStick.stick.Value.x, 0, JoyStick.stick.Value.y);
        dir = transform.TransformDirection(dir);
        transform.Translate(dir * MoveSpeed * Time.deltaTime, Space.World);
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
}
