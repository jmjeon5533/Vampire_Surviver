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
    private List<ClickEvent> rootinfos = new List<ClickEvent>();
    private int selectedIndex;

    public void SelectStart()
    {
        shadowPanel.gameObject.SetActive(true);
        techrootparent.gameObject.SetActive(true);
    }

    private void Awake()
    {
        instance = this;

        selectedIndex = -1;
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

            rootinfos.Add(root.GetComponent<ClickEvent>());
            rootinfos[i].Table = techTreeTables[selectedTechTreeIndex[i]];

            rootinfos[i].Init((isSelected) =>
            {
                if (!isSelected) selectedIndex = -1;
                else selectedIndex = index;

                if (selectedIndex == -1) for (int i = 0; i < rootinfos.Count; i++) rootinfos[i].gameObject.SetActive(true);
                else
                {
                    for (int i = 0; i < rootinfos.Count; i++)
                    {
                        if (i == selectedIndex) rootinfos[i].gameObject.SetActive(true);
                        else rootinfos[i].gameObject.SetActive(false);
                    }
                }
            });

            remainindex.Remove(select);
        }

        for (int i = 0; i < techRootPositions.Count; i++)
        {
            var table = rootinfos[i].Table;
            for (int j = 0; j < table.Nodes.Count; j++)
            {
                var nodeinfos = rootinfos[i].NodeInfos;

                nodeinfos.Add(new NodeInfo());
                nodeinfos[j].targetnode = table.Nodes[j];

                for (int k = 0; k < table.Nodes[j].Nexts.Count; k++) nodeinfos[j].nextIndexs.Add(table.Nodes[j].Nexts[k].Id);

                var nodeObj = Instantiate(techTreeNodePrefeb, rootinfos[i].transform);
                nodeObj.transform.localPosition = table.Nodes[j].Position + Vector3.up * 400;
                nodeObj.SetActive(false);

                nodeinfos[j].nodeObj = nodeObj.transform as RectTransform;
            }
        }
    }
}