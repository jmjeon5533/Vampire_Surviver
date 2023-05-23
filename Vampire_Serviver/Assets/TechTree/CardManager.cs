using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TechTree;

public class CardManager : MonoBehaviour
{
    public static CardManager instance = new CardManager();
    [Header("Tab")]
    public GameObject SelectTab; //����â
    [SerializeField] Transform CardTable; //���ö��� ����ִ� Transform
    [Header("Element")]
    public Transform SelectTransform; //���ѿ� ī�� transform
    public ClickEvent TechCard; //������ ��ũī��
    public ClickEvent[] SelectAbleCard; //���� ���� ������ ��ũ
    [SerializeField] Text SelectCountText; //���� ����Ʈ ǥ��
    [Header("List")]
    public List<GameObject> techTables = new List<GameObject>(); //������ ��ũ ����
    public List<GameObject> SelectedTable = new List<GameObject>(); //���õ� ��ũ ����
    public List<int> SelectedValue = new List<int>(); //���õ� ��ũ�� Index ��ȣ
    public List<GameObject> CreatedTransform = new List<GameObject>(); //���õ� ��ũ�� �����ǰ� �ı��� ����
    public GameObject TechPrefab;//������ ��ũ ������
    public Transform TechTransform; //������ ��ũ �������� Transform
    public List<GameObject> TransformObject = new List<GameObject>(); //������ Transform ������Ʈ �迭
    public Dictionary<string, bool> isEnableDic = new Dictionary<string, bool>(); //������ ��ũ ��� Ȱ��ȭ��

    public List<TechTreeTable> TechScript = new List<TechTreeTable>(); //������ ��ũ ��ũ��Ʈ

    public RectTransform[] TechTableRect = new RectTransform[3]; //������ 3���� ��ǥ
    public enum PanelType
    {
        Card,
        Tech
    }
    public PanelType panelType = PanelType.Card;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        for (int i = 0; i < CardTable.childCount; i++)
        {
            SelectAbleCard[i]
                = CardTable.GetChild(i).GetComponent<ClickEvent>();
        }
        CreateTechTree();
    }
    public void CreateTechTree()
    {
        for (int i = 0; i < TechScript.Count; i++)
        {
            var parent = Instantiate(new GameObject(), TechTransform); //�߰��� ��ũ����� Transform ���� (���� ���ϰ�)
            parent.name = TechScript[i].name;
            for (int j = 1; j < TechScript[i].Nodes.Count; j++)
            {
                //������ TechScript�� Position ����
                var EachTechPos = TechScript[i].Nodes[j].Position;
                var Prefab = Instantiate(TechPrefab, parent.transform);
                Prefab.GetComponent<RectTransform>().anchoredPosition3D = EachTechPos;
                isEnableDic.Add(i.ToString("D2") + j, TechScript[i].Nodes[j].Active); //i��° ��ũ�� i��° ��尪
                var button = Prefab.GetComponent<Button>();
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() => SkillPointUse(i.ToString("D2") + j));
            }
            TransformObject.Add(parent);
            parent.SetActive(false);
        }
        for (int i = 0; i < SelectedTable.Count; i++)
        {
            //�������� ���õ� ��ư�� ����
            var obj = Instantiate(SelectedTable[i], TechTableRect[i].position, Quaternion.identity, SelectTransform);
            //���õ� ��ư�� �ִ� ClickEvent ��������
            SelectAbleCard[i] = obj.GetComponent<ClickEvent>();
        }
    }
    public void NodeActive(GameObject Obj)
    {
        for (int i = 0; i < TransformObject.Count; i++)
        {
            TransformObject[i].SetActive(false);
        }
        Obj.SetActive(true);
    }
    public void SkillPointUse(string ID)
    {
        if (GameManager.Instance.player.SelectCount != 0)
        {
            isEnableDic[ID] = true;
            GameManager.Instance.player.SelectCount--;
            if (GameManager.Instance.player.SelectCount <= 0) SelectEnd();
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
    public void SelectStart() //���� �� Ȱ��ȭ
    {
        SelectTab.SetActive(true);
        SelectShuffle();
        UIUpdate();
    }
    public void SelectEnd()
    {
        //�г� �ٽ� ���ڸ���
        panelType = PanelType.Card;
        TechCard.EndTabResetAnchor();
        TechCard = null;
        //����â
        SelectTab.SetActive(false);
    }
    public void UIUpdate()
    {
        SelectCountText.text = "���� ����Ʈ : " + GameManager.Instance.player.SelectCount;
    }
    private void SelectShuffle()
    {
        // ��ü ����Ʈ ����
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

            // ���� �ε��� ����
            int randomIndex = Random.Range(0, availableObjects.Count);

            // ��ü ���� �� ��� ����Ʈ�� �߰�
            GameObject selectedObject = availableObjects[randomIndex];
            SelectedTable.Add(selectedObject);
            selectedObject.GetComponent<ClickEvent>().Index = i;
            SelectedValue.Add(selectedObject.GetComponent<ClickEvent>().ID);

            // ���õ� ��ü�� ����Ʈ���� ����
            availableObjects.RemoveAt(randomIndex);
        }
        for (int i = 0; i < SelectedTable.Count; i++)
        {
            var obj = Instantiate(SelectedTable[i], TechTableRect[i].position, Quaternion.identity, SelectTransform);
            SelectAbleCard[i] = obj.GetComponent<ClickEvent>();

        }
    }
}