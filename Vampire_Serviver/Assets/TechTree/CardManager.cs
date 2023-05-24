using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TechTree;

public class CardManager : MonoBehaviour
{
    public static CardManager instance { get; private set; }

    [SerializeField] private RectTransform shadowPanel;
    [SerializeField] private RectTransform techrootparent;
    [Space(10)]
    [SerializeField] private List<TechTreeTable> techTreeTables = new List<TechTreeTable>();
    [SerializeField] private List<GameObject> techRootPrefebs = new List<GameObject>();
    [SerializeField] private List<RectTransform> techRootPositions = new List<RectTransform>();
    [SerializeField] private GameObject techTreeNodePrefeb;

    private List<int> selectedTechTreeIndex = new List<int>();
    private List<RootInfo> rootinfos = new List<RootInfo>();

    public void SelectStart()
    {
        shadowPanel.gameObject.SetActive(true);
        techrootparent.gameObject.SetActive(true);
    }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        var remainindex = new List<int>();
        for (int i = 0; i < techTreeTables.Count; i++) remainindex.Add(i);

        for (int i = 0; i < techRootPositions.Count; i++)
        {
            var index = i;
            var select = remainindex[Random.Range(0, remainindex.Count)];

            var root = Instantiate(techRootPrefebs[select], techrootparent).GetComponent<RectTransform>();
            root.anchoredPosition = techRootPositions[i].anchoredPosition;

            selectedTechTreeIndex.Add(select);

            rootinfos.Add(new RootInfo());
            rootinfos[i].table = techTreeTables[selectedTechTreeIndex[i]];
            rootinfos[i].rootObj = root.GetComponent<ClickEvent>();

            rootinfos[i].rootObj.Init(() => 
            {
                rootinfos[index].isSelected = true;
            });

            remainindex.Remove(select);
        }

        for (int i = 0; i < techRootPositions.Count; i++)
        {
            var table = rootinfos[i].table;
            for (int j = 0; j < table.Nodes.Count; j++)
            {
                rootinfos[i].nodeInfos.Add(new NodeInfo());
                rootinfos[i].nodeInfos[j].targetnode = table.Nodes[j];

                for (int k = 0; k < table.Nodes[j].Nexts.Count; k++) rootinfos[i].nodeInfos[j].nextIndexs.Add(table.Nodes[j].Nexts[k].Id);

                var nodeObj = Instantiate(techTreeNodePrefeb, rootinfos[i].rootObj.transform);
                nodeObj.transform.localPosition = table.Nodes[j].Position + Vector3.up * 550;
                nodeObj.SetActive(false);

                rootinfos[i].nodeInfos[j].nodeObj = nodeObj.transform as RectTransform;
            }
        }
    }
}

[System.Serializable]
public class RootInfo
{
    public TechTreeTable table;
    public List<NodeInfo> nodeInfos = new List<NodeInfo>();

    public ClickEvent rootObj;
    public bool isSelected;
}

[System.Serializable]
public class NodeInfo
{
    public TechTreeNode targetnode;
    public List<int> nextIndexs = new List<int>();
    public bool isActive;

    public RectTransform nodeObj;
}