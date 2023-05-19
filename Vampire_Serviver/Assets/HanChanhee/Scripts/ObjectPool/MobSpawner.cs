using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobSpawner : MonoBehaviour
{
    [SerializeField] private Transform player;

    [SerializeField] private int spawnCount;
    
    [SerializeField] private float spawnTime;

    private float curSpawnTime = 0;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Spawn();
    }

    void Spawn()
    {
        curSpawnTime += Time.deltaTime;
        if(curSpawnTime > spawnTime)
        {
            curSpawnTime = 0;
            for(int i = 0; i < spawnCount; i++)
            {
                GameObject obj;
                switch(Random.Range(0, 3))
                {
                    case 0:
                        obj = ObjectPool.GetPoolObject("Red");
                        break;
                    case 1:
                        obj = ObjectPool.GetPoolObject("Yellow");
                        break;
                    case 2:
                        obj = ObjectPool.GetPoolObject("Orange");
                        break;
                    default:
                        obj = null;
                        break;

                }
                Vector3 spawnPos =Random.insideUnitSphere * 5;
                spawnPos = new Vector3(spawnPos.x, player.position.y, spawnPos.z);
                obj.transform.position = spawnPos;
                

            }
        }
    }




}
