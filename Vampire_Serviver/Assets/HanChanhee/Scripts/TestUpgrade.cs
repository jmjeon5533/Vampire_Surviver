using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestUpgrade : UpgradeBase
{
    public override void Upgrade()
    {
        UpgradeInfo();
        Debug.Log("Archer Upgrade!");
    }

    protected override void UpgradeInfo()
    {
        Debug.Log(explain);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
