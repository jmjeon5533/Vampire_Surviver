using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using TechTree;

[System.Serializable]
public class NodeInfo
{
    public TechTreeNode targetnode;
    public int previousIndex;
    public List<int> nextIndexs = new List<int>();
    public string command;
    public bool isActive;

    public RectTransform nodeObj;
}

public class ClickEvent : MonoBehaviour, IPointerDownHandler
{   
    [SerializeField] private GameObject xMark;
    [SerializeField] private List<NodeInfo> nodeInfos = new List<NodeInfo>();
    [SerializeField] private TechTreeTable table;
    
    private UpgradeSkill UpgradeNodes;
    private Vector2 startPos;
    private bool isSelected;
    private float curAnimationTime;
    private Action<bool> clickAction;

    public List<NodeInfo> NodeInfos => nodeInfos;
    public UpgradeSkill upgradeNodes => UpgradeNodes;
    public TechTreeTable Table 
    {
        get { return table; }
        set { table = value; }
    }

    public bool IsPlayingAnimation => curAnimationTime > 0;

    private void Awake(){
        UpgradeNodes = GetComponent<UpgradeSkill>();
    }
    private void Start() 
    {
        xMark.SetActive(false);
        startPos = (transform as RectTransform).anchoredPosition;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(curAnimationTime > 0) return;

        isSelected = !isSelected;
        xMark.SetActive(isSelected);

        if(isSelected)
        {
            (transform as RectTransform).DOAnchorPos(Vector2.down * 400, 0.5f);
            curAnimationTime = 0.6f;
        }
        else
        {
            (transform as RectTransform).DOAnchorPos(startPos, 0.5f);
            curAnimationTime = 0.6f;
        }

        clickAction?.Invoke(isSelected);

        if(isSelected)
            this.Invoke(() => 
            {
                for(int i = 1; i < nodeInfos.Count; i++) nodeInfos[i].nodeObj.gameObject.SetActive(true);
            }, 0.5f);
        else
            for(int i = 1; i < nodeInfos.Count; i++) nodeInfos[i].nodeObj.gameObject.SetActive(false);
        UIManager.instance.UIUpdate();
    }
    
    private void Update() 
    {
        if(curAnimationTime > 0) curAnimationTime -= Time.deltaTime;    
    }
    
    public void Init(Action<bool> action)
    {
        clickAction = action;
    }
}
