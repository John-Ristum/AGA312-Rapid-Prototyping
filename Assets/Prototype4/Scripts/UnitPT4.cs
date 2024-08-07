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
    public Animator anim;
    public string unitName;
    public int unitLevel;

    public int attackDamage;
    public int specialDamage;
    public bool isBlocking;

    public int maxHP;
    public int currentHP;
    public string attackAnim;
    public Transform oppStandPoint;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        anim.SetBool("Idle", true);
    }

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
