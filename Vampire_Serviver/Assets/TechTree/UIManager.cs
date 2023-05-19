using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public static UIManager Instance => instance;
    public Player player;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        player = GameManager.Instance.player;
        UIUpdate();
    }
    public Image PersentImage;
    public Text PersentText;
    public void UIUpdate()
    {
        PersentImage.fillAmount = player.XP / player.MaxXP;
        PersentText.text = $"{Mathf.RoundToInt(player.XP * 100 / player.MaxXP)}%";
    }
}
