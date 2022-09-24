using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public string naming;
    public int maxHP;
    public int currentHP;
    public int actions;
    public int damage;
    public int bleeding;
    public bool legsFine;

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
}
