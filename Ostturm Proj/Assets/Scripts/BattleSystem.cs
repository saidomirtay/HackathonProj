using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using TMPro;
using UnityEditor.Search;
using UnityEngine.SceneManagement;
using System;
using System.ComponentModel;
using Random = UnityEngine.Random;
using System.Runtime.CompilerServices;

public enum BattleState { FIRSTTURN, SECONDTURN, THIRDTURN, GAMEOVER }

public class BattleSystem : MonoBehaviour
{
    public static BattleState state;

    public static GameObject[] characters;


    [HideInInspector]
    public Unit[] charactersUnits;

    public BattleHUD[] charactersHUD;
    [SerializeField] TMP_Text actionsLeftText;

    [SerializeField] private int deadEnemies;

    private void Start()
    {
        characters = new GameObject[3];
        characters[0] = GameObject.Find("Player");
        characters[1] = GameObject.Find("Enemy 1");
        characters[2] = GameObject.Find("Enemy 2");

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
            if (charactersHUD[i] != HUD)
            {
                charactersHUD[i].turn.text = "";
                charactersHUD[i].Circle.SetActive(false);
            }
            else
            {
                charactersHUD[i].SetHP(charactersUnits[i].currentHP);
                charactersHUD[i].turn.text = "turn";
                charactersHUD[i].Circle.SetActive(true);
                charactersUnits[i].currentActions = charactersUnits[i].maxActions;
                SetActionsLeftText(charactersUnits[i].currentActions);
                if (charactersUnits[i].bleeding == 3)
                {
                    charactersUnits[i].currentHP = 0;
                    OnEnemyDeath(i);
                }
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
        if (charactersUnits[index].currentActions > 0)
        {
            Vector3 attackerPos = characters[index].transform.position;
            for (int i = 0; i < characters.Length; i++)
            {
                if (Vector3.Distance(characters[i].transform.position, attackerPos) <= charactersUnits[index].maxAttackDistance && characters[i] != characters[index])
                {
                    BodyPart(i);
                    
                    SubtractActions(index);
                }
            }
        }
        else
            NextTurn(index);
    }

    private void BodyPart(int i)
    {
        int bodyPart = Random.Range(0, 6);
        if (bodyPart == 0)
        {
            Debug.Log("head");
            if (Random.Range(0, 3) == 0)
            {
                Debug.Log("instant kill");
                charactersUnits[i].currentHP = 0;
                charactersHUD[i].SetHP(charactersUnits[i].currentHP);
                OnEnemyDeath(i);
            }
            else
            {
                bool isEnemyDead = charactersUnits[i].TakeDamage(1);
                charactersHUD[i].SetHP(charactersUnits[i].currentHP);
                if (isEnemyDead)
                {
                    OnEnemyDeath(i);
                }
            }
        }
        else if (bodyPart > 3)
        {
            Debug.Log("legs");
            if (Random.Range(0, 3) == 0)
            {
                Debug.Log("immobilization");
                charactersUnits[i].legsFine++;
                charactersHUD[i].SetStatus("immobilized");
            }
            else
            {
                bool isEnemyDead = charactersUnits[i].TakeDamage(1);
                charactersHUD[i].SetHP(charactersUnits[i].currentHP);
                if (isEnemyDead)
                {
                    OnEnemyDeath(i);
                }
            }
        }
        else
        {
            Debug.Log("body");
            if (Random.Range(0, 5) == 0)
            {
                Debug.Log("bleeding");
                charactersUnits[i].bleeding++;
                charactersHUD[i].SetStatus("bleeding");
            }
            else
            {
                bool isEnemyDead = charactersUnits[i].TakeDamage(1);
                charactersHUD[i].SetHP(charactersUnits[i].currentHP);
                if (isEnemyDead)
                {
                    OnEnemyDeath(i);
                }
            }
        }
    }

    private void OnEnemyDeath(int i)
    {
        deadEnemies++;
        if (deadEnemies > 1)
        {
            state = BattleState.GAMEOVER;
            EndBattle();
        }
        charactersUnits[i].maxActions = 0;
        charactersUnits[i].currentActions = 0;
        charactersHUD[i].SetHP(0);
        characters[i].transform.position = new Vector3(999, 999, 999);
    }

    public void SubtractActions(int index)
    {
        charactersUnits[index].currentActions--;
        SetActionsLeftText(charactersUnits[index].currentActions);
        if (charactersUnits[index].currentActions <= 0)
        {
            NextTurn(index);
        }
    }

    private void SetActionsLeftText(int actions)
    {
        actionsLeftText.text = "Actions left: " + actions;
    }

    public void NextTurn(int index)
    {
        if (charactersUnits[index].bleeding > 0)
        {
            charactersUnits[index].bleeding++;
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

    public void RangedWeapon(int index)
    {
        charactersUnits[index].maxAttackDistance *= 2;
        charactersUnits[index].ChangeWeapon();
    }

    private void EndBattle()
    {
        Debug.Log("Game Over");
        SceneManager.LoadScene(0);
    }
}
