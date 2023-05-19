using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TechTree;
using UnityEngine.UI;

public class NodeManager : MonoBehaviour
{
    public TechTreeTable treeTable;
    public GameObject NodePrefab;
    public RectTransform rect;
    public Text Num;
    private void Start()
    {
        CreateNode();
    }
    private void OnDrawGizmos()
    {
        var tree = treeTable.Nodes;
        for (int i = 0; i < tree.Count; i++)
        {
            Color color = tree[i].Active ? Color.red : Color.green;
            for (int j = 0; j < tree[i].Nexts.Count; j++)
            {
                Debug.DrawRay(tree[i].Position + rect.position, (tree[i].Nexts[j].Position - tree[i].Position), color);
            }
        }
    }
    void CreateNode()
    {
        for (int i = 0; i < treeTable.Nodes.Count; i++)
        {
            var GameObject = Instantiate(NodePrefab, rect);
            GameObject.transform.localPosition = treeTable.Nodes[i].Position;

            var NumText = Instantiate(Num.gameObject, rect);
            Text text = NumText.GetComponent<Text>();
            NumText.gameObject.transform.localPosition = treeTable.Nodes[i].Position;
            text.text = $"{i}";
        }
    }
}
