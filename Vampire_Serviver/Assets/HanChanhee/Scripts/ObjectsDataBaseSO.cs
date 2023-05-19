using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class ObjectsDataBaseSO : ScriptableObject
{
    public List<ObjectData> objectDatas;
}

[System.Serializable]
public class ObjectData
{
    [field: SerializeField]
    public string Name { get; private set; }

    [field: SerializeField]
    public int Id { get; private set; }

    [field: SerializeField]
    public Vector2Int Size { get; private set; } = Vector2Int.one;

    [field: SerializeField]
    public GameObject Prefab { get; set; }

}
