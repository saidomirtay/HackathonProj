using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public string naming;
    public int maxHP;
    public int currentHP;
    public int maxActions;
    public int currentActions;
    public int bleeding;
    public int legsFine;
    public float maxAttackDistance;
    public GameObject meleeWeapon;
    public GameObject rangedWeapon;

    public bool TakeDamage(int dmg)
    {
        currentHP -= dmg;

        if (currentHP <= 0)
        {
            return true;
        }
        else
            return false;
    }

    public void ChangeWeapon()
    {
        meleeWeapon.SetActive(false);
        rangedWeapon.SetActive(true);
    }
}
