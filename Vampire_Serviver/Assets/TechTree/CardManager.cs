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
    public GameObject UsePointButton; //임시적인 포인트 사용 버튼
    [Header("List")]
    public List<GameObject> techTables = new List<GameObject>(); //생성될 테크 종류
    public List<GameObject> SelectedTable = new List<GameObject>(); //선택된 테크 종류
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
    }
    public void ActiveCard(ClickEvent card)
    {
        switch (panelType)
        {
            case PanelType.Card:
                {
                    TechCard = card;
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
                        }
                    }
                    TechCard.TechAnchor();
                    panelType = PanelType.Tech;
                    break;
                }
            case PanelType.Tech:
                {
                    for (int i = 0; i < SelectAbleCard.Length; i++)
                    {
                        SelectAbleCard[i].TechName.enabled = true;
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
        UsePointButton.SetActive(false);
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

            // 선택된 개체를 리스트에서 제거
            availableObjects.RemoveAt(randomIndex);
        }
        for (int i = 0; i < SelectedTable.Count; i++)
        {
            var obj = Instantiate(SelectedTable[i],TechTableRect[i].position,Quaternion.identity,SelectTransform);
            SelectAbleCard[i] = obj.GetComponent<ClickEvent>();
        }
    }
    public void SkillPointUse()
    {
        GameManager.Instance.player.SelectCount--;
        if (GameManager.Instance.player.SelectCount <= 0) SelectEnd();
        UIUpdate();
    }
}