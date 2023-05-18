using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class JoyStick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public static JoyStick stick { get; private set; }

    [SerializeField] private RectTransform bg;

    private Vector2 value;
    private float radius;
    private RectTransform rt;

    public Vector2 Value { get { return value; } }

    private void Awake()
    {
        stick = this;
    }
    private void Start()
    {
        rt = transform as RectTransform;

        radius = bg.rect.width * 0.5f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos = eventData.position - (Vector2)bg.position;
        pos = Vector2.ClampMagnitude(pos, radius);

        value = pos / radius;
        rt.localPosition = pos;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Vector2 pos = eventData.position - (Vector2)bg.position;
        pos = Vector2.ClampMagnitude(pos, radius);

        value = pos / radius;
        rt.localPosition = pos;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        rt.localPosition = Vector3.zero;
        value = Vector2.zero;
    }
}