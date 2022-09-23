using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Attack : MonoBehaviour
{
    public static bool isUrTurn;
    private float maxAttackDistance = 4.9f;
    public static int hp;
    public static int actions;

    private void Start()
    {
        actions = 2;
        hp = 2;
        isUrTurn = true;
    }

    private bool DistanceCheck()
    {
        foreach (GameObject enemy in PlayerMovement.enemies)
        {
            if (Vector3.Distance(transform.position, enemy.transform.position) <= maxAttackDistance)
            {
                return true;
            }
        }
        return false;
    }

    private void Damage()
    {
        if (DistanceCheck() && actions > 0)
        {
            //make instant kill chance

            hp--;
            if(hp == 0)
                Application.Quit();
            actions--;
            if (actions == 0)
                isUrTurn = false;
        }
    }
}
