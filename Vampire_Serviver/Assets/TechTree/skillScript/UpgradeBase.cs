using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UpgradeBase : MonoBehaviour
{
    [SerializeField] protected string explain;

    public abstract void Upgrade();

    protected abstract void UpgradeInfo();
}
