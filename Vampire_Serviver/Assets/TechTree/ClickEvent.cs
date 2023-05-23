using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickEvent : MonoBehaviour,IPointerClickHandler
{
    public Vector3 MyPos;
    public GameObject CancelImage;
    public Text TechName;
    public int Index;
    public int ID;
    RectTransform rect;
    float time = 0;
    int ActiveNum;
    public void OnPointerClick(PointerEventData eventData)
    {
        CardManager.instance.ActiveCard(this);
    }
    void Awake()
    {
        rect = GetComponent<RectTransform>();
    }
    void Start()
    {
        CancelImage.SetActive(false);
        MyPos = rect.anchoredPosition3D;
    }
    public void TechAnchor(int ButtonIndex)
    {
        ActiveNum = ButtonIndex;
        StopCoroutine("ResetMove");
        StartCoroutine("TechMove");
    }
    public void ResetAnchor()
    {
        StopCoroutine("TechMove");
        StartCoroutine("ResetMove");
    }
    IEnumerator TechMove()
    {

        CancelImage.SetActive(true);
        while (time < 1)
        {
            rect.anchoredPosition3D = Vector3.Lerp(MyPos, 
                new Vector3(0,-400,0), Easing.easeOutSine(time));

            time += Time.deltaTime;
            yield return null;
        }
        CardManager.instance.NodeActive(CardManager.instance.TransformObject[ActiveNum]);
    }
    IEnumerator ResetMove()
    {
        CancelImage.SetActive(false);
        while (time > 0)
        {
            rect.anchoredPosition3D = Vector3.Lerp(MyPos,
                new Vector3(0, -400, 0), Easing.easeOutSine(time));

            time -= Time.deltaTime;
            yield return null;
        }
    }
    public void EndTabResetAnchor()
    {
        rect.anchoredPosition3D = MyPos;
        time = 0;
        CardManager.instance.ElementSetActive(true);
        print(rect.anchoredPosition3D);
        StopCoroutine("ResetMove");
        StopCoroutine("TechMove");
    }
}
