using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UpgradeSkill : MonoBehaviour
{
    public static UpgradeSkill instance { get; private set; }

    private Dictionary<string, Delegate> NodeFunction = new Dictionary<string, Delegate>();

    public Delegate GetDelegate(string key) => NodeFunction[key];

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        NodeFunction.Add("AtkStat",new Action<string>(AtkStat));
        NodeFunction.Add("SizeStat",new Action<string>(SizeStat));
        NodeFunction.Add("MoveSpeed",new Action<string>(MoveSpeed));
        NodeFunction.Add("AddStat",new Action<string>(AddStat));
        NodeFunction.Add("AtkSpeed",new Action<string>(AtkSpeed));
        NodeFunction.Add("BulletSpeed",new Action<string>(BulletSpeed));
        NodeFunction.Add("None",new Action<string>(None));
        NodeFunction.Add("SizeOfAtk",new Action(SizeOfAtk));
    }
    private void AtkStat(string value)
    {
        GameManager.Instance.player.AttackDamage += float.Parse(value);
        GameManager.Instance.player.TotalDamageInit();
    }
    private void SizeStat(string value)
    {
        GameManager.Instance.player.BulletSize += float.Parse(value);
    }
    private void MoveSpeed(string value)
    {
        GameManager.Instance.player.MoveSpeed += float.Parse(value);
    }
    private void AddStat(string value)
    {
        GameManager.Instance.player.BulletAmount += int.Parse(value);
    }
    private void AtkSpeed(string value){
        GameManager.Instance.player.attackSpeed += float.Parse(value);
        GameManager.Instance.player.AtkSpeedInit();
    }
    private void BulletSpeed(string value){
        GameManager.Instance.player.BulletSpeed += float.Parse(value);
    }
    private void SizeOfAtk(){
        var gm = GameManager.Instance;
        gm.player.isAtkBulletSize = true;
        gm.player.TotalDamageInit();
    }
    private void None(string value){
        print(value);
    }
}
