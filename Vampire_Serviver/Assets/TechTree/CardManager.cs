using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{
    public static CardManager instance = new CardManager();
    [Header("Tab")]
    public GameObject SelectTab; //����â
    [SerializeField] Transform CardTable; //���ö��� ����ִ� Transform
    [Header("Element")]
    public ClickEvent TechCard; //������ ��ũī��
    public ClickEvent[] SelectAbleCard; //���� ���� ������ ��ũ
    [SerializeField] Text SelectCountText; //���� ����Ʈ ǥ��

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
        InitUI();
    }
    public void ElementSetActive(bool b)
    {
        for (int i = 0; i < CardTable.childCount; i++)
        {
            SelectAbleCard[i].GetComponent<Image>().enabled = b;
        }
    }
    public void SelectStart(int SelectChance) //���� �� Ȱ��ȭ
    {
        this.SelectCount = SelectChance;
        SelectTab.SetActive(true);
        InitUI();
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
    public void InitUI()
    {
        SelectCountText.text = "���� ����Ʈ : " + SelectCount;
    }
}