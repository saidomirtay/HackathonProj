using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using TMPro;
using UnityEditor.Search;
using UnityEngine.SceneManagement;

public enum BattleState { FIRSTTURN, SECONDTURN, THIRDTURN, GAMEOVER }

public class BattleSystem : MonoBehaviour
{
    public BattleState state;

    public GameObject[] characters;


    [HideInInspector]
    public Unit[] charactersUnits;

    public BattleHUD[] charactersHUD;

    private float maxAttackDistance = 3.8f;
    [SerializeField] private  int deadEnemies;

    private void Start()
    {
        deadEnemies = 0;
        state = BattleState.FIRSTTURN;
        charactersUnits = new Unit[characters.Length];
        SetupBattle();
    }

    private void Update()
    {
        OnAttackButton();
    }

    private void SetupBattle()
    {
        for (int i = 0; i < characters.Length; i++)
        {
            charactersUnits[i] = characters[i].GetComponent<Unit>();
        }

        for (int i = 0; i < characters.Length; i++)
        {
            charactersHUD[i].SetHUD(charactersUnits[i]);
        }

        WhosTurn(charactersHUD[0]);
    }

    private void WhosTurn(BattleHUD HUD)
    {
        for (int i = 0; i < charactersHUD.Length; i++)
        {
            if(charactersHUD[i] != HUD)
            {
                charactersHUD[i].turn.text = "";
                charactersHUD[i].Circle.SetActive(false);
            }
            else
            {
                charactersHUD[i].turn.text = "turn";
                charactersHUD[i].Circle.SetActive(true);
            }
        }
    }

    private void OnAttackButton()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (state == BattleState.FIRSTTURN)
            {
                AttackCheck(0);
            }
            else if (state == BattleState.SECONDTURN)
            {
                AttackCheck(1);
            }
            else if (state == BattleState.THIRDTURN)
            {
                AttackCheck(2);
            }
        }
    }

    private void AttackCheck(int index)
    {
        Vector3 attackerPos = characters[index].transform.position;
        for (int i = 0; i < characters.Length; i++)
        {
            if (Vector3.Distance(characters[i].transform.position, attackerPos) <= maxAttackDistance && characters[i] != characters[index])
            {
                bool isEnemyDead = charactersUnits[i].TakeDamage(1);
                charactersHUD[i].SetHP(charactersUnits[i].currentHP);

                if (isEnemyDead)
                {
                    deadEnemies++;
                    if (deadEnemies > 1)
                    {
                        state = BattleState.GAMEOVER;
                        EndBattle();
                    }
                }

                switch (index)
                {
                    case 0:
                        state = BattleState.SECONDTURN;
                        WhosTurn(charactersHUD[1]);
                        break;
                    case 1:
                        state = BattleState.THIRDTURN;
                        WhosTurn(charactersHUD[2]);
                        break;
                    case 2:
                        state = BattleState.FIRSTTURN;
                        WhosTurn(charactersHUD[0]);
                        break;
                }
            }
        }
    }

    private void EndBattle()
    {
        SceneManager.LoadScene(0);
    }
}
