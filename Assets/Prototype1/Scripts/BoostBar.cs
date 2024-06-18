using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoostBar : GameBehaviour
{
    public Image healthFill;

    public void UpdateBoostBar(float _health, float _maxHealth = 5)
    {
        healthFill.fillAmount = MapTo01(_health, 0, _maxHealth);
    }
}
