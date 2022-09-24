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
    private int bleedingDegree;

    private void Start()
    {
        actions = 2;
        hp = 2;
        bleedingDegree = 0;
        isUrTurn = true;
    }

    private void Update()
    {
        DamageDeal();
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

    private void DamageDeal()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && DistanceCheck() && actions > 0)
        {
            int bodyPart = Random.Range(0, 6);
            //head damage
            if (bodyPart == 0)
            {
                //instant kill
                if (Random.Range(0, 3) == 0)
                {
                    hp = 0;
                }
            }
            //body damage
            else if (bodyPart > 0 && bodyPart < 4)
            {
                //bleeding
                if (Random.Range(0, 5) == 0)
                {
                    bleedingDegree++;
                }
                else
                {
                    hp--;
                }
            }
            //leg damage
            else
            {
                if (Random.Range(0, 3) == 0)
                {
                    PlayerMovement.legsFine = false;
                }
                else
                {
                    hp--;
                }
            }

            actions--;
            DamageTaken();
        }
    }

    private void DamageTaken()
    {
        if (actions <= 0)
        {
            isUrTurn = false;
            if (bleedingDegree > 0)
            {
                bleedingDegree++;
            }
        }

        if (hp <= 0 || bleedingDegree > 3)
            Application.Quit();
    }
}
