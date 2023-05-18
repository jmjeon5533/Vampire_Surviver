using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    
    void Update()
    {
        var dir = new Vector3(JoyStick.stick.Value.x, 0, JoyStick.stick.Value.y);
        dir = transform.TransformDirection(dir);
        transform.Translate(dir * 2 * Time.deltaTime, Space.World);
    }
}
