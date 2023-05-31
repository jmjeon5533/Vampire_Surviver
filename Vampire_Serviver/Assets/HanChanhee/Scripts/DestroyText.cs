using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DestroyText : MonoBehaviour
{
   public void DestroyT()
    {
        //Destroy(gameObject);
        this.gameObject.GetComponent<TextMeshProUGUI>().color = Color.white;
        ObjectPool.ReturnPoolObject(this.gameObject, "DmgText");

    }
}
