using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleHUD : MonoBehaviour
{
    [SerializeField] TMP_Text nick;
    public TMP_Text turn;
    [SerializeField] TMP_Text status;
    [SerializeField] Slider sliderHP;
    public GameObject Circle;

    public void SetHUD(Unit unit)
    {
        nick.text = unit.naming;
        sliderHP.maxValue = unit.maxHP;
        sliderHP.value = unit.currentHP;
    }

    public void SetHP(int hp)
    {
        sliderHP.value = hp;
    }
}
