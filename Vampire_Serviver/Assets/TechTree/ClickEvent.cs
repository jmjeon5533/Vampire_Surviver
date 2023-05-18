using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickEvent : MonoBehaviour,IPointerClickHandler
{
    public Vector3 MyPos;
    RectTransform rect;
    float time = 0;
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
        MyPos = rect.anchoredPosition3D;
    }
    public void TechAnchor()
    {
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
        while (time < 1)
        {
            rect.anchoredPosition3D = Vector3.Lerp(MyPos, 
                new Vector3(-550,0,0), Easing.easeOutSine(time));

            time += Time.deltaTime;
            yield return null;
        }
    }
    IEnumerator ResetMove()
    {
        while (time > 0)
        {
            rect.anchoredPosition3D = Vector3.Lerp(MyPos,
                new Vector3(-550, 0, 0), Easing.easeOutSine(time));

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
