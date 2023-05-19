using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance { get; private set; } 

    public GameObject[] poolPrefabs; //���� ����
    public int poolingCount; //�ν��Ͻ� ���� ����

    private Dictionary<object, Queue<GameObject>> poolObjects= new Dictionary<object, Queue<GameObject>>(); 

    //ó�� ���� �� �ν��Ͻ� ����
    private void Initialize()
    {
        for(int i = 0; i < poolPrefabs.Length; i++)
        {
            for(int j = 0; j < poolingCount; j++)
            {
                if(!poolObjects.ContainsKey(poolPrefabs[i].name))
                {
                    Queue<GameObject> list = new Queue<GameObject>();
                    poolObjects.Add(poolPrefabs[i].name, list);
                }
                GameObject obj = Instantiate(poolPrefabs[i], transform);
                obj.SetActive(false);
                poolObjects[poolPrefabs[i].name].Enqueue(obj);
            }
        }
    }

    
    //�̸��� �´� ������Ʈ Ǯ���� ������Ʈ ������ �Լ�  
    public static GameObject GetPoolObject(string _name)
    {
        if(Instance.poolObjects.ContainsKey(_name))
        {
            if(Instance.poolObjects[_name].Count > 1)
            {
                var poolObj = Instance.poolObjects[_name].Dequeue();
                poolObj.transform.SetParent(null);
                poolObj.gameObject.SetActive(true);
                return poolObj;

            } else
            {
                var poolObj = Instantiate(Instance.poolObjects[_name].ToArray()[0]);
                poolObj.transform.SetParent(null);
                poolObj.gameObject.SetActive(true);
                return poolObj;
            }

        }
        Debug.LogError("Does not exist Name in ObjectPool");
        return null;
    }

    //�̸��� �´� ������Ʈ Ǯ�� ������Ʈ �������� �Լ�
    public static void ReturnPoolObject(GameObject obj, string _name)
    {
        if (Instance.poolObjects.ContainsKey(_name))
        {
            obj.gameObject.SetActive(false);
            obj.transform.SetParent(Instance.transform);
            Instance.poolObjects[_name].Enqueue(obj);
        }
        else
        {
            Debug.LogError("Does not exist Name in ObjectPool");
        }
            

    }

    // Start is called before the first frame update
    void Start()
    {
        if(Instance == null) Instance = this;
        else Destroy(this.gameObject);
        Initialize();
    }

    
}
