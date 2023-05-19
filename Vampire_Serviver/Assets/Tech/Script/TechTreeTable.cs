using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechTree
{
    [CreateAssetMenu(menuName = "TechTree/Table", fileName = "TechTree")]
    public class TechTreeTable : ScriptableObject
    {
        [SerializeField] private List<TechTreeNode> nodes = new List<TechTreeNode>();
        public List<TechTreeNode> Nodes => nodes;
    }
}
