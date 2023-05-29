
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TechTree;
using System.Linq;

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
    [SerializeField] private List<ClickEvent> rootinfos = new List<ClickEvent>();
    private List<RectTransform> curRootPrefab = new List<RectTransform>();
    private int selectedIndex;
    private int currentIndex;

    private Player player;

    public void SelectStart()
    {
        shadowPanel.gameObject.SetActive(true);
        techrootparent.gameObject.SetActive(true);
        UIManager.instance.UIUpdate();
    }

    private void Awake()
    {
        instance = this;
        player = GameManager.Instance.player;
        selectedIndex = -1;
    }

    private void Start()
    {
        var remainindex = new List<int>();
        for (int i = 0; i < techTreeTables.Count; i++) remainindex.Add(i);
        for (int i = 0; i < techRootPrefebs.Count; i++)
        {
            curRootPrefab.Add(Instantiate(techRootPrefebs[i], techrootparent).GetComponent<RectTransform>());
            curRootPrefab[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < techRootPositions.Count; i++)
        {
            var index = i;
            var select = remainindex[Random.Range(0, remainindex.Count)];
            var root = curRootPrefab[select];
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
                var nodeindex = j;
                var nodeinfos = rootinfos[i].NodeInfos;

                nodeinfos.Add(new NodeInfo());
                nodeinfos[j].targetnode = table.Nodes[j];
                for (int k = 0; k < table.Nodes[j].Nexts.Count; k++) nodeinfos[j].nextIndexs.Add(table.Nodes[j].Nexts[k].Id);
                nodeinfos[j].command = table.Nodes[j].Command;

                var nodeObj = Instantiate(techTreeNodePrefeb, rootinfos[i].transform);
                nodeObj.transform.localPosition = table.Nodes[j].Position + Vector3.up * 400;
                nodeObj.SetActive(false);

                nodeObj.GetComponent<Button>().onClick.AddListener(() =>
                {
                    if (nodeinfos[nodeinfos[nodeindex].previousIndex].isActive && !nodeinfos[nodeindex].isActive && player.SelectCount > 0) 
                    {
                        nodeinfos[nodeindex].isActive = true;
                        nodeObj.GetComponent<Image>().color = new Color(0, 0, 0, 0.5f);
                        player.SelectCount--;

                        var split = nodeinfos[nodeindex].command.Split("/");
                        var parameters = split.Skip(1).ToArray<object>();

                        Debug.Log(nodeinfos[nodeindex].command);
                        UpgradeSkill.instance.GetDelegate(split[0]).DynamicInvoke(parameters);
                        UIManager.instance.UIUpdate();
                        
                        TestCommand.Instance.Command("Skill/Upgrade/" + techTreeTables[selectedTechTreeIndex[selectedIndex]].name + "/Node" + nodeindex);
                    }
                });

                nodeinfos[j].nodeObj = nodeObj.transform as RectTransform;
            }

            rootinfos[i].NodeInfos[0].isActive = true;
        }

        for (int i = 0; i < rootinfos.Count; i++)
        {
            var nodeinfos = rootinfos[i].NodeInfos;
            for (int j = 0; j < nodeinfos.Count; j++)
            {
                var nodeindex = j;
                foreach (var index in nodeinfos[j].nextIndexs) nodeinfos[index].previousIndex = nodeindex;
            }
        }
    }
}