using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Attack : MonoBehaviour
{
    public static int turn;
    private float maxAttackDistance = 4.9f;
    public static int hp;
    public static int actions;
    public static int bleedingDegree;

    private void Start()
    {
        turn = 0;
        hp = 2;
        actions = 2;
    }

    private bool DistanceCheck()
    {
        foreach (GameObject character in BattleSystem.characters)
        {
            if (Vector3.Distance(transform.position, character.transform.position) <= maxAttackDistance)
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
            if (turn < BattleSystem.characters.Length)
            {
                turn++;
            }
            else
            {
                turn = 0;
            }

            if (bleedingDegree > 0)
            {
                bleedingDegree++;
            }
        }

        if (hp <= 0 || bleedingDegree > 3)
            Debug.Log("Game Over");
    }
}
