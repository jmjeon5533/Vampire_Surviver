using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TechTree;

[System.Serializable]
public class StringBoolDictionary : SerializableDictionary<string, bool> { }

public interface IStringBoolDictionary
{
    IDictionary<string, bool> StringBoolDictionary { get; set; }
}

public class CardManager : MonoBehaviour, IStringBoolDictionary
{

    public static CardManager instance = new CardManager();

    [Header("Tab")]
    public GameObject SelectTab;
    [SerializeField] Transform CardTable;
    [Header("Element")]
    public Transform SelectTransform;
    public ClickEvent TechCard;
    public ClickEvent[] SelectAbleCard;
    [SerializeField] Text SelectCountText;
    [Header("List")]
    public List<GameObject> techTables = new List<GameObject>();
    public List<GameObject> SelectedTable = new List<GameObject>();
    public List<int> SelectedValue = new List<int>();
    public List<GameObject> CreatedTransform = new List<GameObject>();
    public GameObject TechPrefab;
    public Transform TechTransform;
    public List<GameObject> TransformObject = new List<GameObject>();
    public StringBoolDictionary isEnableDic;
    public List<TechTreeTable> TechScript = new List<TechTreeTable>();
    public RectTransform[] TechTableRect = new RectTransform[3];
    public enum PanelType
    {
        Card,
        Tech
    }
    public PanelType panelType = PanelType.Card;

    public IDictionary<string, bool> StringBoolDictionary
    {
        get { return isEnableDic; }
        set { isEnableDic.CopyFrom(value); }
    }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        for (int i = 0; i < CardTable.childCount; i++)
        {
            SelectAbleCard[i] = CardTable.GetChild(i).GetComponent<ClickEvent>();
        }
        CreateTechTree();
    }

    public void CreateTechTree()
    {
        for (int i = 0; i < TechScript.Count; i++)
        {
            var tempi = techTables[i].GetComponent<ClickEvent>().ID;
            var parent = Instantiate(new GameObject(), TechTransform);
            parent.name = TechScript[i].name;
            for (int j = 1; j < TechScript[i].Nodes.Count; j++)
            {
                var tempj = j;
                var EachTechPos = TechScript[i].Nodes[j].Position;
                var Prefab = Instantiate(TechPrefab, parent.transform);
                Prefab.GetComponent<RectTransform>().anchoredPosition3D = EachTechPos;
                isEnableDic.Add(tempi.ToString("D2") + tempj, TechScript[i].Nodes[j].Active);
                var button = Prefab.GetComponent<Button>();
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() => SkillPointUse(tempi.ToString("D2") + tempj));
            }
            TransformObject.Add(parent);
            parent.SetActive(false);
        }

        for (int i = 0; i < SelectedTable.Count; i++)
        {
            var obj = Instantiate(SelectedTable[i], TechTableRect[i].position, Quaternion.identity, SelectTransform);
            SelectAbleCard[i] = obj.GetComponent<ClickEvent>();
        }
    }

    public void NodeActive(GameObject Obj = null)
    {
        for (int i = 0; i < TransformObject.Count; i++)
        {
            TransformObject[i].SetActive(false);
        }
        if (Obj != null) Obj.SetActive(true);
    }

    public void SkillPointUse(string ID)
    {
        print(ID);
        if (GameManager.Instance.player.SelectCount != 0)
        {
            if (!isEnableDic[ID])
            {
                isEnableDic[ID] = true;
                GameManager.Instance.player.SelectCount--;
                if (GameManager.Instance.player.SelectCount <= 0) SelectEnd();
            }
            else
            {
                print("사용함");
            }
        }
        UIUpdate();
    }

    public void ActiveCard(ClickEvent card)
    {
        switch (panelType)
        {
            case PanelType.Card:
                {
                    TechCard = card;
                    int ActiveNum = 0;
                    for (int i = 0; i < SelectAbleCard.Length; i++)
                    {
                        if (SelectAbleCard[i] != card)
                        {
                            SelectAbleCard[i].GetComponent<Image>().enabled = false;
                            SelectAbleCard[i].TechName.enabled = false;
                        }
                        else
                        {
                            SelectAbleCard[i].GetComponent<Image>().enabled = true;
                            SelectAbleCard[i].TechName.enabled = true;
                            ActiveNum = i;
                        }
                    }
                    TechCard.TechAnchor(ActiveNum);

                    panelType = PanelType.Tech;
                    break;
                }
            case PanelType.Tech:
                {
                    for (int i = 0; i < SelectAbleCard.Length; i++)
                    {
                        SelectAbleCard[i].TechName.enabled = true;
                    }
                    for (int i = 0; i < TransformObject.Count; i++)
                    {
                        TransformObject[i].SetActive(false);
                    }
                    TechCard.ResetAnchor();
                    TechCard = null;
                    ElementSetActive(true);
                    panelType = PanelType.Card;
                    break;
                }
        }
        UIUpdate();
    }

    public void ElementSetActive(bool b)
    {
        for (int i = 0; i < SelectAbleCard.Length; i++)
        {
            SelectAbleCard[i].GetComponent<Image>().enabled = b;
        }
    }

    public void SelectStart()
    {
        SelectTab.SetActive(true);
        SelectShuffle();
        NodeActive();
        UIUpdate();
    }

    public void SelectEnd()
    {
        panelType = PanelType.Card;
        TechCard.EndTabResetAnchor();
        TechCard = null;
        SelectTab.SetActive(false);
    }

    public void UIUpdate()
    {
        SelectCountText.text = "남은 포인트 : " + GameManager.Instance.player.SelectCount;
    }

    private void SelectShuffle()
    {
        List<GameObject> availableObjects = new List<GameObject>(techTables);

        if (SelectAbleCard[0] != null)
        {
            SelectedTable.Clear();
            SelectedValue.Clear();
            for (int i = 0; i < 3; i++)
            {
                Destroy(SelectAbleCard[i].gameObject);
            }
        }

        for (int i = 0; i < 3; i++)
        {
            if (availableObjects.Count == 0)
            {
                Debug.LogWarning("There are not enough available objects to select.");
                break;
            }

            int randomIndex = Random.Range(0, availableObjects.Count);

            GameObject selectedObject = availableObjects[randomIndex];
            SelectedTable.Add(selectedObject);
            selectedObject.GetComponent<ClickEvent>().Index = i;
            SelectedValue.Add(selectedObject.GetComponent<ClickEvent>().ID);

            availableObjects.RemoveAt(randomIndex);
        }

        for (int i = 0; i < SelectedTable.Count; i++)
        {
            var obj = Instantiate(SelectedTable[i], TechTableRect[i].position, Quaternion.identity, SelectTransform);
            SelectAbleCard[i] = obj.GetComponent<ClickEvent>();
        }
    }
}
