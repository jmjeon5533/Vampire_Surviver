using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName ="TechObject/Selection",fileName ="SelectObj")]
public class TechSelection : ScriptableObject
{
    public int ID;
    public string Name;
    public string Description;
    public int privious;

}
