using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BattleHudPT4 : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text levelText;
    public TMP_Text hpText;
    int maxHP;

    public void SetHUD(UnitPT4 _unit)
    {
        maxHP = _unit.maxHP;

        nameText.text = _unit.unitName;
        levelText.text = "Lvl " + _unit.unitLevel;
        hpText.text = "HP: " + _unit.currentHP + "/" + maxHP;
    }

    public void SetHP(int _hp)
    {
        hpText.text = "HP: " + _hp + "/" + maxHP;
    }
}
