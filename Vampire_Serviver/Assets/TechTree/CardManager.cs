using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TechTree;

public class CardManager : MonoBehaviour
{
    public static CardManager instance = new CardManager();
    [Header("Tab")]
    public GameObject SelectTab; //선택창
    [SerializeField] Transform CardTable; //선택란이 담겨있는 Transform
    [Header("Element")]
    public Transform SelectTransform; //리롤용 카드 transform
    public ClickEvent TechCard; //선택한 테크카드
    public ClickEvent[] SelectAbleCard; //현재 선택 가능한 테크
    [SerializeField] Text SelectCountText; //선택 포인트 표시
    [Header("List")]
    public List<GameObject> techTables = new List<GameObject>(); //생성될 테크 종류
    public List<GameObject> SelectedTable = new List<GameObject>(); //선택된 테크 종류
    public List<int> SelectedValue = new List<int>(); //선택된 테크의 Index 번호
    public List<GameObject> CreatedTransform = new List<GameObject>(); //선택된 테크가 생성되고 파괴될 변수
    public GameObject TechPrefab;//생성될 테크 프리팹
    public Transform TechTransform; //생성될 테크 프리팹의 Transform
    public List<GameObject> TransformObject = new List<GameObject>(); //생성된 Transform 오브젝트 배열
    public Dictionary<string, bool> isEnableDic = new Dictionary<string, bool>(); //각각의 테크 노드 활성화값

    public List<TechTreeTable> TechScript = new List<TechTreeTable>(); //생성될 테크 스크립트

    public RectTransform[] TechTableRect = new RectTransform[3]; //복제될 3개의 좌표
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
            var parent = Instantiate(new GameObject(), TechTransform); //추가할 테크노드의 Transform 생성 (제거 편하게)
            parent.name = TechScript[i].name;
            for (int j = 1; j < TechScript[i].Nodes.Count; j++)
            {
                //각각의 TechScript의 Position 저장
                var EachTechPos = TechScript[i].Nodes[j].Position;
                var Prefab = Instantiate(TechPrefab, parent.transform);
                Prefab.GetComponent<RectTransform>().anchoredPosition3D = EachTechPos;
                isEnableDic.Add(i.ToString("D2") + j, TechScript[i].Nodes[j].Active); //i번째 테크의 i번째 노드값
                var button = Prefab.GetComponent<Button>();
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() => SkillPointUse(i.ToString("D2") + j));
            }
            TransformObject.Add(parent);
            parent.SetActive(false);
        }
        for (int i = 0; i < SelectedTable.Count; i++)
        {
            //랜덤으로 선택된 버튼들 생성
            var obj = Instantiate(SelectedTable[i], TechTableRect[i].position, Quaternion.identity, SelectTransform);
            //선택된 버튼에 있는 ClickEvent 가져오기
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
    public void SelectStart() //선택 탭 활성화
    {
        SelectTab.SetActive(true);
        SelectShuffle();
        UIUpdate();
    }
    public void SelectEnd()
    {
        //패널 다시 제자리로
        panelType = PanelType.Card;
        TechCard.EndTabResetAnchor();
        TechCard = null;
        //선택창
        SelectTab.SetActive(false);
    }
    public void UIUpdate()
    {
        SelectCountText.text = "남은 포인트 : " + GameManager.Instance.player.SelectCount;
    }
    private void SelectShuffle()
    {
        // 개체 리스트 복사
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

            // 랜덤 인덱스 선택
            int randomIndex = Random.Range(0, availableObjects.Count);

            // 개체 선택 및 결과 리스트에 추가
            GameObject selectedObject = availableObjects[randomIndex];
            SelectedTable.Add(selectedObject);
            selectedObject.GetComponent<ClickEvent>().Index = i;
            SelectedValue.Add(selectedObject.GetComponent<ClickEvent>().ID);

            // 선택된 개체를 리스트에서 제거
            availableObjects.RemoveAt(randomIndex);
        }
        for (int i = 0; i < SelectedTable.Count; i++)
        {
            var obj = Instantiate(SelectedTable[i], TechTableRect[i].position, Quaternion.identity, SelectTransform);
            SelectAbleCard[i] = obj.GetComponent<ClickEvent>();

        }
    }
}