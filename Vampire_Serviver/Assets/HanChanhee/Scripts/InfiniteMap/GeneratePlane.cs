using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratePlane : MonoBehaviour
{
    public GameObject player;
    [SerializeField, Header("Render Range")]
    private int range = 5; //거리
    [SerializeField, Header("Plane")]
    private int offset = 10; //생성될 거리 Plane Size에 맞춰서 조절

    private Vector3 startPos = Vector3.zero;

    private int XPlayerMove => (int)(player.transform.position.x - startPos.x);

    private int ZPlayerMove => (int)(player.transform.position.z - startPos.z);

    private int XPlayerLocation => (int)Mathf.Floor(player.transform.position.x / offset);

    private int ZPlayerLocation => (int)Mathf.Floor(player.transform.position.z / offset);

    private Hashtable tilePlane = new Hashtable();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (startPos == Vector3.zero)
        {
            CreatePlane();

        }
        if (IsPlayerMove())
        {
            DestroyPlaneOutOfRange();
            CreatePlane();

        }
    }

    //void Reposition()
    //{
    //    Vector3 playerPos = player.transform.position;

    //}
    //맵 생성
    void CreatePlane()
    {
        for (int x = -range; x < range; x++)
        {
            for (int z = -range; z < range; z++)
            {
                Vector3 pos = new Vector3((x * offset + XPlayerLocation), 0, (z * offset + ZPlayerLocation));
                if (!tilePlane.Contains(pos))
                {
                    GameObject _plane = ObjectPool.GetPoolObject("Plane");
                    _plane.transform.position = pos; _plane.transform.rotation = Quaternion.identity;
                    tilePlane.Add(pos, _plane);

                }
            }
        }
    }
    //플레이어 이동 감지
    bool IsPlayerMove()
    {
        if (Mathf.Abs(XPlayerMove) >= offset || Mathf.Abs(ZPlayerMove) >= offset)
        {
            return true;
        }
        return false;
    }
    //멀리 있는 맵 제거
    void DestroyPlaneOutOfRange()
    {
        List<Vector3> planesToRemove = new List<Vector3>();

        foreach (DictionaryEntry entry in tilePlane)
        {
            Vector3 planePos = (Vector3)entry.Key;
            GameObject planeObject = (GameObject)entry.Value;

            float distance = Vector3.Distance(planePos, player.transform.position);
            if (distance > offset * range)
            {
                planesToRemove.Add(planePos);
                planeObject.name = planeObject.name.Replace("(Clone)", "");
                ObjectPool.ReturnPoolObject(planeObject, "Plane");
            }
        }

        foreach (Vector3 pos in planesToRemove)
        {
            tilePlane.Remove(pos);
        }
    }

}


