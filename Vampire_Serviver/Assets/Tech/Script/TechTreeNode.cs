using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechTree
{
    public class TechTreeNode : ScriptableObject
    {
        [SerializeField] private TechTreeTable table;
        [SerializeField] private int id;
        [SerializeField] private bool active;
        [SerializeField] private Vector3 position;
        [SerializeField] private string command;
        [SerializeField] private List<TechTreeNode> nexts = new List<TechTreeNode>();

        public int Id => id;
        public bool Active { get { return active; } set { active = value; } }
        public Vector3 Position => position;
        public string Command => command;
        public List<TechTreeNode> Nexts => nexts;

#if UNITY_EDITOR
        /// <summary>
        /// Change this node id. Only can use in Unity Editor.
        /// </summary>
        /// <param name="id">target id</param>
        public void ChangeID(int id) => this.id = id;

        /// <summary>
        /// Set this node table. Only can use in Unity Editor.
        /// </summary>
        /// <param name="table">table</param>
        public void SetTable(TechTreeTable table) => this.table = table;
#endif
    }
}
