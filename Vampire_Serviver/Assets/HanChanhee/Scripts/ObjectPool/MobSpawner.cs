using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobSpawner : MonoBehaviour
{
    [SerializeField] private Transform player;

    [SerializeField] private int spawnCount;
    
    [SerializeField] private float spawnTime;

    [SerializeField] private float distanceThreshold;

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
                Vector3 randomDirection = Random.insideUnitSphere.normalized;

                // 플레이어로부터 최소 거리 이상 떨어진 좌표 계산
                Vector3 spawnPosition = player.position + randomDirection * distanceThreshold;
               
                obj.transform.position = new Vector3(spawnPosition.x, player.position.y, spawnPosition.z);
                

            }
        }
    }




}
