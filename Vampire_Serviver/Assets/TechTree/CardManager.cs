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
    public Transform SelectTransform; //���ѿ� ī�� transform
    public ClickEvent TechCard; //������ ��ũī��
    public ClickEvent[] SelectAbleCard; //���� ���� ������ ��ũ
    [SerializeField] Text SelectCountText; //���� ����Ʈ ǥ��
    public GameObject UsePointButton; //�ӽ����� ����Ʈ ��� ��ư
    [Header("List")]
    public List<GameObject> techTables = new List<GameObject>(); //������ ��ũ ����
    public List<GameObject> SelectedTable = new List<GameObject>(); //���õ� ��ũ ����

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
    public void SelectStart() //���� �� Ȱ��ȭ
    {
        SelectTab.SetActive(true);
        UsePointButton.SetActive(false);
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

            // ���õ� ��ü�� ����Ʈ���� ����
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