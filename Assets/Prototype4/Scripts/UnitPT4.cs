using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PartyType { Player, Enemy }
public enum EquationType { Addition, Subtraction, Multiplication, Division }

public class UnitPT4 : MonoBehaviour
{
    public PartyType party;
    public EquationType equation;
    public BattleHudPT4 HUD;
    public string unitName;
    public int unitLevel;

    public int damage;

    public int maxHP;
    public int currentHP;

    public bool TakeDamage(int _dmg)
    {
        currentHP -= _dmg;

        if (currentHP <= 0)
            return true;
        else
            return false;
    }

    public void Heal(int _amount)
    {
        currentHP += _amount;
        if (currentHP > maxHP)
            currentHP = maxHP;
    }
}
