using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{
    public static CardManager instance = new CardManager();
    [Header("Tab")]
    public GameObject SelectTab; //선택창
    [SerializeField] Transform CardTable; //선택란이 담겨있는 Transform
    [SerializeField] GameObject Warningtab; //포인트가 남아있을 때 나오는 확인탭
    [Header("Element")]
    public ClickEvent TechCard; //선택한 테크카드
    public ClickEvent[] SelectAbleCard; //현재 선택 가능한 테크
    [SerializeField] GameObject SelectButton; //선택 버튼
    [SerializeField] Text SelectCountText; //선택 포인트 표시

    int SelectCount;
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
                        }
                        else
                        {
                            SelectAbleCard[i].GetComponent<Image>().enabled = true;
                        }
                    }
                    TechCard.TechAnchor();
                    panelType = PanelType.Tech;
                    SelectButton.SetActive(true);
                    break;
                }
            case PanelType.Tech:
                {
                    TechCard.ResetAnchor();
                    TechCard = null;
                    ElementSetActive(true);
                    panelType = PanelType.Card;
                    SelectButton.SetActive(false);
                    break;
                }
        }
        InitUI();
    }
    public void ElementSetActive(bool b)
    {
        for (int i = 0; i < CardTable.childCount; i++)
        {
            SelectAbleCard[i].GetComponent<Image>().enabled = b;
        }
    }
    public void SelectStart(int SelectChance) //선택 탭 활성화
    {
        this.SelectCount = SelectChance;
        SelectTab.SetActive(true);
        InitUI();
    }
    public void SelectCalculate() //선택 완료 계산
    {
        if (SelectCount > 1)
        {
            WarningActive(true);
        }
        else
        {
            SelectEnd();
        }
    }
    public void WarningActive(bool b)
    {
        Warningtab.SetActive(b);
    }
    public void SelectEnd()
    {
        //패널 다시 제자리로
        panelType = PanelType.Card;
        TechCard.EndTabResetAnchor();
        TechCard = null;
        //선택창
        SelectTab.SetActive(false);
        Warningtab.SetActive(false);
    }
    public void InitUI()
    {
        SelectCountText.text = "남은 포인트 : " + SelectCount;
    }
}