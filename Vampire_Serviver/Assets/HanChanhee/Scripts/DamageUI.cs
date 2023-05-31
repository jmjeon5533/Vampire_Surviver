using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DamageUI : MonoBehaviour
{
    public static DamageUI Instance;
    public GameObject canvus;
    
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnDamageText(Transform vicitm, string myText, Color color)
    {
        var text = ObjectPool.GetPoolObject("DmgText");
        text.transform.parent = canvus.transform;
        text.GetComponent<TextMeshProUGUI>().text = myText;
        text.GetComponent<TextMeshProUGUI>().color = color;
        text.GetComponent<TextMeshProUGUI>().transform.position  = vicitm.position + new Vector3(0,1,0);
    }
}
